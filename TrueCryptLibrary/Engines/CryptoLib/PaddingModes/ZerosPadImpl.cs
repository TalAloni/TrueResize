using System;

using CryptoLib.CipherModes;

namespace CryptoLib.PaddingModes
{
	public sealed class ZerosPadEncryptImpl : PaddingModeImpl
	{
		public ZerosPadEncryptImpl( CipherModeImpl cipher ) : base( cipher )
		{
		}

		protected override byte[] InternalTransformFinal( byte[] inputBuffer, int inputOffset, int inputCount )
		{
			int rem = inputCount % InputBlockSize;
			int evenCount = inputCount - rem;

			byte[] data;
			if ( rem == 0 )
				data = new byte[inputCount];
			else
				data = new byte[evenCount + InputBlockSize];

			Cipher.Transform( inputBuffer, inputOffset, evenCount, data, 0 );

			if ( rem > 0 )
			{
				Buffer.BlockCopy( inputBuffer, inputOffset + evenCount, data, evenCount, rem );
				Cipher.Transform( data, evenCount, InputBlockSize, data, evenCount );
			}

			return data;
		}
	}
}