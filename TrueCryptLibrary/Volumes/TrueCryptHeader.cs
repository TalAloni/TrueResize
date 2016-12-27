using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Utilities;
using CryptoLib.SymmetricAlgorithms;
using CSharpTest.Net.Crypto;
using Medo.Security.Cryptography;
using XTSSharp;

namespace TrueCryptLibrary
{
    public class TrueCryptHeader
    {
        // http://www.truecrypt.org/docs/volume-format-specification
        public const string TrueCryptSignature = "TRUE";
        public const ushort SupportedFormatVersion = 5;

        public byte[] Salt; // 64 bytes
        public string Signature;
        public ushort FormatVersion;
        public ushort MinProgramVersionRequired;
        public uint Checksum256; // decrypted bytes 256-511 
        // Reserved 16 bytes
        public ulong HiddenVolumeSize;
        public ulong VolumeSize;
        public ulong MasterKeyScopeOffset;
        public ulong MasterKeyEncryptedAreaSize;
        public uint Flags;
        public uint SectorSize;
        // Reserved 120 bytes
        public uint Checksum64; // decrypted bytes 64-251
        public byte[] MasterKey;

        private bool m_isValid;     // indicated whether the header is valid
        private bool m_isSupported; // indicated whether the header format is supported
        private KeyValuePairList<SymmetricAlgorithm, byte[]> m_algorithmChain;
        private HMAC m_hmac;

        public TrueCryptHeader(byte[] buffer, string password) : this(buffer, UTF8Encoding.UTF8.GetBytes(password))
        { }

        public TrueCryptHeader(byte[] buffer, byte[] password)
        {
            Salt = ByteReader.ReadBytes(buffer, 0, 64);
            byte[] headerBytes = new byte[448];
            Array.Copy(buffer, 64, headerBytes, 0, 448);
            byte[] decrypted = DecryptHeader(headerBytes, password);
            if (m_isValid)
            {
                Signature = ByteReader.ReadAnsiString(decrypted, 0, 4);
                FormatVersion = BigEndianConverter.ToUInt16(decrypted, 4);
                MinProgramVersionRequired = BigEndianConverter.ToUInt16(decrypted, 6);
                Checksum256 = BigEndianConverter.ToUInt32(decrypted, 8);
                HiddenVolumeSize = BigEndianConverter.ToUInt64(decrypted, 28);
                VolumeSize = BigEndianConverter.ToUInt64(decrypted, 36);
                MasterKeyScopeOffset = BigEndianConverter.ToUInt64(decrypted, 44);
                MasterKeyEncryptedAreaSize = BigEndianConverter.ToUInt64(decrypted, 52);
                Flags = BigEndianConverter.ToUInt32(decrypted, 60);
                SectorSize = BigEndianConverter.ToUInt32(decrypted, 64);
                Checksum64 = BigEndianConverter.ToUInt32(decrypted, 188);
                MasterKey = ByteReader.ReadBytes(decrypted, 192, 192);

                AssignKey(m_algorithmChain, MasterKey);
            }
        }

        /// <summary>
        /// Including the prefixed salt
        /// </summary>
        public byte[] GetBytes(byte[] password)
        {
            byte[] headerBytes = GetDecryptedHeaderBytes();

            int iterations = m_hmac is HMACRIPEMD160 ? 2000 : 1000;
            Pbkdf2 pbkdf2 = new Pbkdf2(m_hmac, password, Salt, iterations);
            byte[] headerKey = pbkdf2.GetBytes(192);

            AssignKey(m_algorithmChain, headerKey);
            byte[] encrypted = XTSHelper.XTSChainEncrypt(m_algorithmChain, 0, headerBytes, 0, headerBytes.Length);
            AssignKey(m_algorithmChain, MasterKey);

            byte[] buffer = new byte[512];
            ByteWriter.WriteBytes(buffer, 0, Salt);
            ByteWriter.WriteBytes(buffer, 64, encrypted);

            return buffer;
        }

        public byte[] GetDecryptedHeaderBytes()
        {
            byte[] headerBytes = new byte[448];
            ByteWriter.WriteAnsiString(headerBytes, 0, Signature, 4);
            BigEndianWriter.WriteUInt16(headerBytes, 4, FormatVersion);
            BigEndianWriter.WriteUInt16(headerBytes, 6, MinProgramVersionRequired);
            BigEndianWriter.WriteUInt32(headerBytes, 8, 0); // checksum will be written later
            BigEndianWriter.WriteUInt64(headerBytes, 28, HiddenVolumeSize);
            BigEndianWriter.WriteUInt64(headerBytes, 36, VolumeSize);
            BigEndianWriter.WriteUInt64(headerBytes, 44, MasterKeyScopeOffset);
            BigEndianWriter.WriteUInt64(headerBytes, 52, MasterKeyEncryptedAreaSize);
            BigEndianWriter.WriteUInt32(headerBytes, 60, Flags);
            BigEndianWriter.WriteUInt32(headerBytes, 64, SectorSize);
            BigEndianWriter.WriteUInt32(headerBytes, 188, 0); // checksum will be written later
            ByteWriter.WriteBytes(headerBytes, 192, MasterKey);

            byte[] temp = ByteReader.ReadBytes(headerBytes, 192, 256);
            uint checksum256 = Utilities.CRC32.Compute(temp);
            // we add checksum256 before calculating checksum64
            BigEndianWriter.WriteUInt32(headerBytes, 8, checksum256);
            temp = ByteReader.ReadBytes(headerBytes, 0, 188);
            uint checksum64 = Utilities.CRC32.Compute(temp);

            BigEndianWriter.WriteUInt32(headerBytes, 8, checksum256);
            BigEndianWriter.WriteUInt32(headerBytes, 188, checksum64);

            return headerBytes;
        }

        private byte[] DecryptHeader(byte[] headerBytes, byte[] password)
        {
            Pbkdf2 pbkdf2;

            KeyValuePairList<HMAC, int> hmacs = new KeyValuePairList<HMAC, int>();
            hmacs.Add(new HMACSHA512(), 1000);
            hmacs.Add(new HMACRIPEMD160(), 2000);
            hmacs.Add(new HMACWhirlpool(), 1000);

            foreach (KeyValuePair<HMAC, int> entry in hmacs)
            {
                HMAC hmac = entry.Key;
                int iterations = entry.Value;
                pbkdf2 = new Pbkdf2(hmac, password, Salt, iterations);
                byte[] key = pbkdf2.GetBytes(192);

                List<KeyValuePairList<SymmetricAlgorithm, byte[]>> algorithms = GetAlgorithms(key);

                foreach (KeyValuePairList<SymmetricAlgorithm, byte[]> algorithmChain in algorithms)
                {
                    byte[] decrypt = (byte[])headerBytes.Clone();

                    // TrueCrypt 7.1a Source (Common\Crypto.c):
                    // ----------------------------------------
                    // When encrypting/decrypting a buffer (typically a volume header) the sequential number
                    // of the first XTS data unit in the buffer is always 0 and the start of the buffer is
                    // always assumed to be aligned with the start of the data unit 0.
                    decrypt = XTSHelper.XTSChainDecrypt(algorithmChain, 0, decrypt, 0, 448);

                    string signature = ByteReader.ReadAnsiString(decrypt, 0, 4);
                    ushort formatVersion = BigEndianConverter.ToUInt16(decrypt, 4);
                    uint checksum256 = BigEndianConverter.ToUInt32(decrypt, 8);

                    byte[] temp = ByteReader.ReadBytes(decrypt, 192, 256);
                    uint computedChecksum = Utilities.CRC32.Compute(temp);
                    
                    if (signature == TrueCryptSignature && checksum256 == computedChecksum)
                    {
                        m_isValid = true;
                        if (formatVersion == SupportedFormatVersion)
                        {
                            m_isSupported = true;
                        }
                        m_algorithmChain = algorithmChain;
                        m_hmac = hmac;
                        return decrypt;
                    }
                }
            }

            return null;
        }

        public static List<KeyValuePairList<SymmetricAlgorithm, byte[]>> GetAlgorithms(byte[] key)
        {
            // AesCryptoServiceProvider will use AES-NI if availible, but Rijndael is almost 3 times faster if AES-NI is not available.
            Rijndael aes = Rijndael.Create();
            aes.Padding = PaddingMode.None;
            aes.Mode = CipherMode.ECB;

            SerpentManaged serpent = new SerpentManaged();
            serpent.Padding = PaddingMode.None;
            serpent.Mode = CipherMode.ECB;

            TwofishManaged twofish = new TwofishManaged();
            twofish.Padding = PaddingMode.None;
            twofish.Mode = CipherMode.ECB;

            List<KeyValuePairList<SymmetricAlgorithm, byte[]>> algorithms = new List<KeyValuePairList<SymmetricAlgorithm, byte[]>>();

            KeyValuePairList<SymmetricAlgorithm, byte[]> aes_only = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            aes_only.Add(aes, new byte[64]);
            algorithms.Add(aes_only);

            KeyValuePairList<SymmetricAlgorithm, byte[]> serpent_only = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            serpent_only.Add(serpent, new byte[64]);
            algorithms.Add(serpent_only);

            KeyValuePairList<SymmetricAlgorithm, byte[]> twofish_only = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            twofish_only.Add(twofish, new byte[64]);
            algorithms.Add(twofish_only);

            KeyValuePairList<SymmetricAlgorithm, byte[]> twofish_aes = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            twofish_aes.Add(twofish, new byte[64]);
            twofish_aes.Add(aes, new byte[64]);
            algorithms.Add(twofish_aes);

            KeyValuePairList<SymmetricAlgorithm, byte[]> serpent_twofish_aes = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            serpent_twofish_aes.Add(serpent, new byte[64]);
            serpent_twofish_aes.Add(twofish, new byte[64]);
            serpent_twofish_aes.Add(aes, new byte[64]);
            algorithms.Add(serpent_twofish_aes);

            KeyValuePairList<SymmetricAlgorithm, byte[]> aes_serpent = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            aes_serpent.Add(aes, new byte[64]);
            aes_serpent.Add(serpent, new byte[64]);
            algorithms.Add(aes_serpent);

            KeyValuePairList<SymmetricAlgorithm, byte[]> aes_twofish_serpent = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            aes_twofish_serpent.Add(aes, new byte[64]);
            aes_twofish_serpent.Add(twofish, new byte[64]);
            aes_twofish_serpent.Add(serpent, new byte[64]);
            algorithms.Add(aes_twofish_serpent);

            KeyValuePairList<SymmetricAlgorithm, byte[]> serpent_twofish = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            serpent_twofish.Add(serpent, new byte[64]);
            serpent_twofish.Add(twofish, new byte[64]);
            algorithms.Add(serpent_twofish);

            foreach (KeyValuePairList<SymmetricAlgorithm, byte[]> algorithm in algorithms)
            {
                AssignKey(algorithm, key);
            }

            return algorithms;
        }

        private static void AssignKey(KeyValuePairList<SymmetricAlgorithm, byte[]> algorithmChain, byte[] key)
        {
            if (algorithmChain.Count == 1)
            {
                Array.Copy(key, 0, algorithmChain[0].Value, 0, 64);
            }
            else if (algorithmChain.Count == 2)
            {
                Array.Copy(key, 0, algorithmChain[0].Value, 0, 32);
                Array.Copy(key, 64, algorithmChain[0].Value, 32, 32);
                Array.Copy(key, 32, algorithmChain[1].Value, 0, 32);
                Array.Copy(key, 96, algorithmChain[1].Value, 32, 32);
            }
            else if (algorithmChain.Count == 3)
            {
                Array.Copy(key, 0, algorithmChain[0].Value, 0, 32);
                Array.Copy(key, 96, algorithmChain[0].Value, 32, 32);
                Array.Copy(key, 32, algorithmChain[1].Value, 0, 32);
                Array.Copy(key, 128, algorithmChain[1].Value, 32, 32);
                Array.Copy(key, 64, algorithmChain[2].Value, 0, 32);
                Array.Copy(key, 160, algorithmChain[2].Value, 32, 32);
            }
        }

        public bool IsValid
        {
            get
            {
                return m_isValid;
            }
        }

        public bool IsSupported
        {
            get
            {
                return m_isSupported;
            }
        }

        public KeyValuePairList<SymmetricAlgorithm, byte[]> AlgorithmChain
        {
            get
            {
                return m_algorithmChain;
            }
        }
    }
}
