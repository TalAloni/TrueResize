using System;

using CryptoLib.CipherModes;

namespace CryptoLib.PaddingModes
{
	public sealed class ISO10126PadEncryptImpl : LengthPadEncryptImpl
	{
		public ISO10126PadEncryptImpl( CipherModeImpl cipher ) : base( cipher )
		{
		}

		protected override void FillBytes( byte[] buffer, int start, int count )
		{
			Helper.GetRandomBytes( buffer, start, count );
		}
	}
}