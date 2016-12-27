using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities;

namespace TrueCryptLibrary
{
    public class KeyfileHelper
    {
        public const int KeyfileMaxRead = 1048576;

        public static byte[] ReadKeyfile(string path)
        {
            FileInfo info = new FileInfo(path);
            if (info.Length <= KeyfileMaxRead)
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] result = new byte[KeyfileMaxRead];
                stream.Read(result, 0, result.Length);
                return result;
            }
        }

        public static byte[] ApplyKeyFiles(byte[] password, List<byte[]> keyfiles)
        {
            // http://www.truecrypt.org/docs/keyfiles-technical-details
            const int KeyFilePoolLength = 64; // kpl
            byte[] keyFilePool = new byte[KeyFilePoolLength]; // kp
            int passwordLength = password.Length; // pl
            if (KeyFilePoolLength > passwordLength)
            {
                byte[] temp = new byte[KeyFilePoolLength];
                Array.Copy(password, 0, temp, 0, passwordLength);
                password = temp;
            }

            foreach (byte[] keyfile in keyfiles)
            {
                int cursor = 0;
                uint crc32 = 0xffffffff;
                for (int index = 0; index < keyfile.Length; index++)
                {
                    crc32 = CRC32.UPDC32(keyfile[index], crc32);

                    byte[] m = BigEndianConverter.GetBytes(crc32);

                    for (int offset = 0; offset < 4; offset++)
                    {
                        keyFilePool[cursor + offset] = (byte)((keyFilePool[cursor + offset] + m[offset]) % 256);
                    }

                    cursor += 4;

                    if (cursor == KeyFilePoolLength)
                    {
                        cursor = 0;
                    }

                    if (index >= KeyfileMaxRead)
                    {
                        break;
                    }
                }
            }

            byte[] result = new byte[KeyFilePoolLength];
            for (int index = 0; index < KeyFilePoolLength; index++)
            {
                result[index] = (byte)(password[index] + keyFilePool[index]);
            }
            return result;
        }
    }
}
