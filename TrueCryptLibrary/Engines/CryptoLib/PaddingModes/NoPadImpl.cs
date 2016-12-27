using System;

using CryptoLib.CipherModes;

namespace CryptoLib.PaddingModes
{
	public sealed class NoPadImpl : PaddingModeImpl
	{
		public NoPadImpl( CipherModeImpl cipher ) : base( cipher )
		{
		}

		protected override byte[] InternalTransformFinal( byte[] inputBuffer, int inputOffset, int inputCount )
		{
			if ( inputCount % InputBlockSize != 0 )
				throw new ArgumentException( "Invalid inputCount value", "inputCount" );

			byte[] data = new byte[inputCount];
			Cipher.Transform( inputBuffer, inputOffset, inputCount, data, 0 );
			return data;
		}
	}
}