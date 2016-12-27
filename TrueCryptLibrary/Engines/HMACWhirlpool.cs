using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CSharpTest.Net.Crypto;

namespace TrueCryptLibrary
{
    public class HMACWhirlpool : HMAC
    {
        private WhirlpoolManaged m_whirlpool = new WhirlpoolManaged();

        public HMACWhirlpool() : base()
        {
            this.HashSizeValue = 512;
        }

        public override void Initialize()
        {
        }

        protected override void HashCore(byte[] rgb, int ib, int cb)
        {
            // https://en.wikipedia.org/wiki/Hash-based_message_authentication_code
            byte[] key;
            if (this.Key.Length > this.BlockSizeValue)
            {
                key = m_whirlpool.ComputeHash(this.Key);
            }
            else if (this.Key.Length < this.BlockSizeValue)
            {
                // keys shorter than blocksize are zero-padded
                key = new byte[this.BlockSizeValue];
                Array.Copy(this.Key, key, this.Key.Length);
            }
            else
            {
                key = this.Key;
            }

            byte[] outerKey = new byte[this.BlockSizeValue];
            byte[] innerKey = new byte[this.BlockSizeValue];

            for (int index = 0; index < this.BlockSizeValue; index++)
            {
                outerKey[index] = (byte)(0x5c ^ key[index]);
                innerKey[index] = (byte)(0x36 ^ key[index]);
            }

            byte[] temp = new byte[this.BlockSizeValue + cb];
            Array.Copy(innerKey, 0, temp, 0, this.BlockSizeValue);
            Array.Copy(rgb, 0, temp, this.BlockSizeValue, cb);

            byte[] hash1 = m_whirlpool.ComputeHash(temp, 0, temp.Length);
            byte[] temp2 = new byte[this.BlockSizeValue + hash1.Length];
            Array.Copy(outerKey, 0, temp2, 0, this.BlockSizeValue);
            Array.Copy(hash1, 0, temp2, this.BlockSizeValue, hash1.Length);
            this.HashValue = m_whirlpool.ComputeHash(temp2, 0, temp2.Length);
        }

        protected override byte[] HashFinal()
        {
            return this.HashValue;
        }

        // We override base.Key because it calls InitializeKey() which may call m_hash1.ComputeHash
        // However, m_hash1 is null
        public override byte[] Key
        {
            get
            {
                return (byte[])base.KeyValue.Clone();
            }
            set
            {
                base.KeyValue = value;
            }
        }
    }
}
