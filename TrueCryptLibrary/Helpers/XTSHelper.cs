using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CSharpTest.Net.Crypto;
using Medo.Security.Cryptography;
using XTSSharp;
using Utilities;

namespace TrueCryptLibrary
{
    public class XTSHelper
    {
        public static byte[] XTSDecrypt(SymmetricAlgorithm algorithm, byte[] key1, byte[] key2, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            XTSDecrypt(algorithm, key1, key2, dataUnitIndex, inputBuffer, inputOffset, inputCount, result, 0);
            return result;
        }

        public static void XTSDecrypt(SymmetricAlgorithm algorithm, byte[] key1, byte[] key2, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] initializationVector = new byte[16];
            ICryptoTransform decryptor1 = algorithm.CreateDecryptor(key1, initializationVector);
            ICryptoTransform encryptor2 = algorithm.CreateEncryptor(key2, initializationVector);
            XtsCryptoTransform transform = new XtsCryptoTransform(decryptor1, encryptor2, true);

            transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, dataUnitIndex);
        }

        public static byte[] XTSEncrypt(SymmetricAlgorithm algorithm, byte[] key1, byte[] key2, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            XTSEncrypt(algorithm, key1, key2, dataUnitIndex, inputBuffer, inputOffset, inputCount, result, 0);
            return result;
        }

        public static void XTSEncrypt(SymmetricAlgorithm algorithm, byte[] key1, byte[] key2, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] initializationVector = new byte[16];
            ICryptoTransform encryptor1 = algorithm.CreateEncryptor(key1, initializationVector);
            ICryptoTransform encryptor2 = algorithm.CreateEncryptor(key2, initializationVector);
            XtsCryptoTransform transform = new XtsCryptoTransform(encryptor1, encryptor2, false);
            transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, dataUnitIndex);
        }

        public static byte[] XTSChainDecrypt(KeyValuePairList<SymmetricAlgorithm, byte[]> algorithmChain, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            XTSChainDecrypt(algorithmChain, dataUnitIndex, inputBuffer, inputOffset, inputCount, result, 0);

            return result;
        }

        public static void XTSChainDecrypt(KeyValuePairList<SymmetricAlgorithm, byte[]> algorithmChain, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            
            KeyValuePairList<SymmetricAlgorithm, byte[]> reversedChain = new KeyValuePairList<SymmetricAlgorithm, byte[]>();
            reversedChain.AddRange(algorithmChain);
            reversedChain.Reverse();

            foreach (KeyValuePair<SymmetricAlgorithm, byte[]> algorithm in reversedChain)
            {
                byte[] key1 = new byte[32];
                byte[] key2 = new byte[32];
                Array.Copy(algorithm.Value, 0, key1, 0, 32);
                Array.Copy(algorithm.Value, 32, key2, 0, 32);

                XTSDecrypt(algorithm.Key, key1, key2, dataUnitIndex, outputBuffer, outputOffset, inputCount, outputBuffer, outputOffset);
            }
        }

        public static byte[] XTSChainEncrypt(KeyValuePairList<SymmetricAlgorithm, byte[]> algorithmChain, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            XTSChainEncrypt(algorithmChain, dataUnitIndex, inputBuffer, inputOffset, inputCount, result, 0);

            return result;
        }

        public static void XTSChainEncrypt(KeyValuePairList<SymmetricAlgorithm, byte[]> algorithmChain, ulong dataUnitIndex, byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            foreach (KeyValuePair<SymmetricAlgorithm, byte[]> algorithm in algorithmChain)
            {
                byte[] key1 = new byte[32];
                byte[] key2 = new byte[32];
                Array.Copy(algorithm.Value, 0, key1, 0, 32);
                Array.Copy(algorithm.Value, 32, key2, 0, 32);

                XTSEncrypt(algorithm.Key, key1, key2, dataUnitIndex, outputBuffer, outputOffset, inputCount, outputBuffer, outputOffset);
            }
        }
    }
}
