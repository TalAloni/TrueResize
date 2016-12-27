using System;

namespace CryptoLib.BlockCiphers
{
	public sealed class SerpentCipher : IBlockCipher
	{
		#region S-Boxes
		private static void ApplySB0( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x1  ^ x2;
			uint t02 = x0  | x3;
			uint t03 = x0  ^ x1;
			uint t04 = t02 ^ t01;
			uint t05 = x2  | t04;
			uint t06 = x0  ^ x3;
			uint t07 = x1  | x2;
			uint t08 = x3  & t05;
			uint t09 = t03 & t07;
			uint t10 = t09 ^ t08;
			uint t11 = t09 & t10;
			uint t12 = x2  ^ x3;
			uint t13 = t07 ^ t11;
			uint t14 = x1  & t06;
			uint t15 = t06 ^ t13;
			uint t16 =     ~ t15;
			uint t17 = t16 ^ t14;
			uint t18 = t12 ^ t17;

			x0 = t16;
			x1 = t18;
			x2 = t10;
			x3 = t04;
		}

		private static void ApplySB1( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  | x3;
			uint t02 = x2  ^ x3;
			uint t03 =     ~ x1;
			uint t04 = x0  ^ x2;
			uint t05 = x0  | t03;
			uint t06 = x3  & t04;
			uint t07 = t01 & t02;
			uint t08 = x1  | t06;
			uint t09 = t02 ^ t05;
			uint t10 = t07 ^ t08;
			uint t11 = t01 ^ t10;
			uint t12 = t09 ^ t11;
			uint t13 = x1  & x3;
			uint t14 =     ~ t10;
			uint t15 = t13 ^ t12;
			uint t16 = t10 | t15;
			uint t17 = t05 & t16;
			uint t18 = x2  ^ t17;

			x0 = t18;
			x1 = t15;
			x2 = t09;
			x3 = t14;
		}

		private static void ApplySB2( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  | x2;
			uint t02 = x0  ^ x1;
			uint t03 = x3  ^ t01;
			uint t04 = t02 ^ t03;
			uint t05 = x2  ^ t04;
			uint t06 = x1  ^ t05;
			uint t07 = x1  | t05;
			uint t08 = t01 & t06;
			uint t09 = t03 ^ t07;
			uint t10 = t02 | t09;
			uint t11 = t10 ^ t08;
			uint t12 = x0  | x3;
			uint t13 = t09 ^ t11;
			uint t14 = x1  ^ t13;
			uint t15 =     ~ t09;
			uint t16 = t12 ^ t14;

			x0 = t04;
			x1 = t11;
			x2 = t16;
			x3 = t15;
		}

		private static void ApplySB3( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  ^ x2;
			uint t02 = x0  | x3;
			uint t03 = x0  & x3;
			uint t04 = t01 & t02;
			uint t05 = x1  | t03;
			uint t06 = x0  & x1;
			uint t07 = x3  ^ t04;
			uint t08 = x2  | t06;
			uint t09 = x1  ^ t07;
			uint t10 = x3  & t05;
			uint t11 = t02 ^ t10;
			uint t12 = t08 ^ t09;
			uint t13 = x3  | t12;
			uint t14 = x0  | t07;
			uint t15 = x1  & t13;
			uint t16 = t08 ^ t11;
			uint t17 = t14 ^ t15;
			uint t18 = t05 ^ t04;

			x0 = t17;
			x1 = t18;
			x2 = t16;
			x3 = t12;
		}

		private static void ApplySB4( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  | x1;
			uint t02 = x1  | x2;
			uint t03 = x0  ^ t02;
			uint t04 = x1  ^ x3;
			uint t05 = x3  | t03;
			uint t06 = x3  & t01;
			uint t07 = t03 ^ t06;
			uint t08 = t07 & t04;
			uint t09 = t04 & t05;
			uint t10 = x2  ^ t06;
			uint t11 = x1  & x2;
			uint t12 = t04 ^ t08;
			uint t13 = t11 | t03;
			uint t14 = t10 ^ t09;
			uint t15 = x0  & t05;
			uint t16 = t11 | t12;
			uint t17 = t13 ^ t08;
			uint t18 = t15 ^ t16;
			uint t19 =     ~ t14;

			x0 = t19;
			x1 = t18;
			x2 = t17;
			x3 = t07;
		}

		private static void ApplySB5( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x1  ^ x3;
			uint t02 = x1  | x3;
			uint t03 = x0  & t01;
			uint t04 = x2  ^ t02;
			uint t05 = t03 ^ t04;
			uint t06 =     ~ t05;
			uint t07 = x0  ^ t01;
			uint t08 = x3  | t06;
			uint t09 = x1  | t05;
			uint t10 = x3  ^ t08;
			uint t11 = x1  | t07;
			uint t12 = t03 | t06;
			uint t13 = t07 | t10;
			uint t14 = t01 ^ t11;
			uint t15 = t09 ^ t13;
			uint t16 = t07 ^ t08;
			uint t17 = t12 ^ t14;

			x0 = t06;
			x1 = t16;
			x2 = t15;
			x3 = t17;
		}

		private static void ApplySB6( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  & x3;
			uint t02 = x1  ^ x2;
			uint t03 = x0  ^ x3;
			uint t04 = t01 ^ t02;
			uint t05 = x1  | x2;
			uint t06 =     ~ t04;
			uint t07 = t03 & t05;
			uint t08 = x1  & t06;
			uint t09 = x0  | x2;
			uint t10 = t07 ^ t08;
			uint t11 = x1  | x3;
			uint t12 = x2  ^ t11;
			uint t13 = t09 ^ t10;
			uint t14 =     ~ t13;
			uint t15 = t06 & t03;
			uint t16 = t12 ^ t07;
			uint t17 = x0  ^ x1;
			uint t18 = t14 ^ t15;
			uint t19 = t17 ^ t18;

			x0 = t19;
			x1 = t06;
			x2 = t14;
			x3 = t16;
		}

		private static void ApplySB7( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  & x2;
			uint t02 =     ~ x3;
			uint t03 = x0  & t02;
			uint t04 = x1  | t01;
			uint t05 = x0  & x1;
			uint t06 = x2  ^ t04;
			uint t07 = t03 ^ t06;
			uint t08 = x2  | t07;
			uint t09 = x3  | t05;
			uint t10 = x0  ^ t08;
			uint t11 = t04 & t07;
			uint t12 = t09 ^ t10;
			uint t13 = x1  ^ t12;
			uint t14 = t01 ^ t12;
			uint t15 = x2  ^ t05;
			uint t16 = t11 | t13;
			uint t17 = t02 | t14;
			uint t18 = t15 ^ t17;
			uint t19 = x0  ^ t16;

			x0 = t18;
			x1 = t12;
			x2 = t19;
			x3 = t07;
		}
		#endregion

		#region Inverse S-Boxes
		private static void ApplyInverseSB0( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x2  ^ x3;
			uint t02 = x0  | x1;
			uint t03 = x1  | x2;
			uint t04 = x2  & t01;
			uint t05 = t02 ^ t01;
			uint t06 = x0  | t04;
			uint t07 =     ~ t05;
			uint t08 = x1  ^ x3;
			uint t09 = t03 & t08;
			uint t10 = x3  | t07;
			uint t11 = t09 ^ t06;
			uint t12 = x0  | t05;
			uint t13 = t11 ^ t12;
			uint t14 = t03 ^ t10;
			uint t15 = x0  ^ x2;
			uint t16 = t14 ^ t13;
			uint t17 = t05 & t13;
			uint t18 = t14 | t17;
			uint t19 = t15 ^ t18;

			x0 = t19;
			x1 = t11;
			x2 = t07;
			x3 = t16;
		}

		private static void ApplyInverseSB1( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  ^ x1;
			uint t02 = x1  | x3;
			uint t03 = x0  & x2;
			uint t04 = x2  ^ t02;
			uint t05 = x0  | t04;
			uint t06 = t01 & t05;
			uint t07 = x3  | t03;
			uint t08 = x1  ^ t06;
			uint t09 = t07 ^ t06;
			uint t10 = t04 | t03;
			uint t11 = x3  & t08;
			uint t12 =     ~ t09;
			uint t13 = t10 ^ t11;
			uint t14 = x0  | t12;
			uint t15 = t06 ^ t13;
			uint t16 = t01 ^ t04;
			uint t17 = x2  ^ t15;
			uint t18 = t14 ^ t17;

			x0 = t18;
			x1 = t13;
			x2 = t12;
			x3 = t16;
		}

		private static void ApplyInverseSB2( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  ^ x3;
			uint t02 = x2  ^ x3;
			uint t03 = x0  & x2;
			uint t04 = x1  | t02;
			uint t05 = t01 ^ t04;
			uint t06 = x0  | x2;
			uint t07 = x3  | t05;
			uint t08 =     ~ x3;
			uint t09 = x1  & t06;
			uint t10 = t08 | t03;
			uint t11 = x1  & t07;
			uint t12 = t06 & t02;
			uint t13 = t09 ^ t10;
			uint t14 = t12 ^ t11;
			uint t15 = x2  & t13;
			uint t16 = t05 ^ t14;
			uint t17 = t10 ^ t15;
			uint t18 = t16 ^ t17;

			x0 = t05;
			x1 = t14;
			x2 = t18;
			x3 = t13;
		}

		private static void ApplyInverseSB3( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x2  | x3;
			uint t02 = x0  | x3;
			uint t03 = x2  ^ t02;
			uint t04 = x1  ^ t02;
			uint t05 = x0  ^ x3;
			uint t06 = t04 & t03;
			uint t07 = x1  & t01;
			uint t08 = t05 ^ t06;
			uint t09 = x0  ^ t03;
			uint t10 = t07 ^ t03;
			uint t11 = t10 | t05;
			uint t12 = t09 & t11;
			uint t13 = x0  & t08;
			uint t14 = t01 ^ t05;
			uint t15 = x1  ^ t12;
			uint t16 = x1  | t13;
			uint t17 = t14 ^ t16;

			x0 = t10;
			x1 = t15;
			x2 = t08;
			x3 = t17;
		}

		private static void ApplyInverseSB4( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x1  | x3;
			uint t02 = x2  | x3;
			uint t03 = x0  & t01;
			uint t04 = x1  ^ t02;
			uint t05 = x2  ^ x3;
			uint t06 =     ~ t03;
			uint t07 = x0  & t04;
			uint t08 = t05 ^ t07;
			uint t09 = t08 | t06;
			uint t10 = x0  ^ t07;
			uint t11 = t01 ^ t09;
			uint t12 = x3  ^ t04;
			uint t13 = x2  | t10;
			uint t14 = t03 ^ t12;
			uint t15 = x0  ^ t04;
			uint t16 = t11 ^ t13;
			uint t17 = t15 ^ t09;

			x0 = t17;
			x1 = t08;
			x2 = t16;
			x3 = t14;
		}

		private static void ApplyInverseSB5( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  & x3;
			uint t02 = x2  ^ t01;
			uint t03 = x0  ^ x3;
			uint t04 = x1  & t02;
			uint t05 = x0  & x2;
			uint t06 = t03 ^ t04;
			uint t07 = x0  & t06;
			uint t08 = t01 ^ t06;
			uint t09 = x1  | t05;
			uint t10 =     ~ x1;
			uint t11 = t08 ^ t09;
			uint t12 = t10 | t07;
			uint t13 = t06 | t11;
			uint t14 = t02 ^ t12;
			uint t15 = t02 ^ t13;
			uint t16 = x1  ^ x3;
			uint t17 = t16 ^ t15;

			x0 = t06;
			x1 = t11;
			x2 = t17;
			x3 = t14;
		}

		private static void ApplyInverseSB6( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  ^ x2;
			uint t02 =     ~ x2;
			uint t03 = x1  & t01;
			uint t04 = x1  | t02;
			uint t05 = x3  | t03;
			uint t06 = x1  ^ x3;
			uint t07 = x0  & t04;
			uint t08 = x0  | t02;
			uint t09 = t07 ^ t05;
			uint t10 = t06 ^ t08;
			uint t11 =     ~ t09;
			uint t12 = x1  & t11;
			uint t13 = t01 & t05;
			uint t14 = t01 ^ t12;
			uint t15 = t07 ^ t13;
			uint t16 = x3  | t02;
			uint t17 = x0  ^ t10;
			uint t18 = t17 ^ t15;
			uint t19 = t16 ^ t14;

			x0 = t11;
			x1 = t10;
			x2 = t19;
			x3 = t18;
		}

		private static void ApplyInverseSB7( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			uint t01 = x0  & x1;
			uint t02 = x0  | x1;
			uint t03 = x2  | t01;
			uint t04 = x3  & t02;
			uint t05 = t03 ^ t04;
			uint t06 = x1  ^ t04;
			uint t07 = x3  ^ t05;
			uint t08 =     ~ t07;
			uint t09 = t06 | t08;
			uint t10 = x1  ^ x3;
			uint t11 = x0  | x3;
			uint t12 = x0  ^ t09;
			uint t13 = x2  ^ t06;
			uint t14 = x2  & t11;
			uint t15 = x3  | t12;
			uint t16 = t01 | t10;
			uint t17 = t13 ^ t15;
			uint t18 = t14 ^ t16;

			x0 = t17;
			x1 = t12;
			x2 = t18;
			x3 = t05;
		}
		#endregion

		#region Linear Transformation
		private static void ApplyTransformation( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			x0 = Helper.RotateLeft( x0, 13 );
			x2 = Helper.RotateLeft( x2, 3 );
			x1 = x1 ^ x0 ^ x2;
			x3 = x3 ^ x2 ^ (x0 << 3);
			x1 = Helper.RotateLeft( x1, 1 );
			x3 = Helper.RotateLeft( x3, 7 );
			x0 = x0 ^ x1 ^ x3;
			x2 = x2 ^ x3 ^ (x1 << 7);
			x0 = Helper.RotateLeft( x0, 5 );
			x2 = Helper.RotateLeft( x2, 22 );
		}

		private static void ApplyInverseTransformation( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			x2 = Helper.RotateRight( x2, 22 );
			x0 = Helper.RotateRight( x0, 5 );
			x2 = x2 ^ x3 ^ (x1 << 7);
			x0 = x0 ^ x1 ^ x3;
			x3 = Helper.RotateRight( x3, 7 );
			x1 = Helper.RotateRight( x1, 1 );
			x3 = x3 ^ x2 ^ (x0 << 3);
			x1 = x1 ^ x0 ^ x2;
			x2 = Helper.RotateRight( x2, 3 );
			x0 = Helper.RotateRight( x0, 13 );
		}
		#endregion

		private const uint _phi = 0x9E3779B9;

		public int BlockSize{ get{ return 16; } }
		public bool Encryption{ get{ return _encrypt; } }

		private readonly bool _encrypt;

		private uint[] _keys;

		public SerpentCipher( byte[] key, bool encrypt )
		{
			if ( key == null )
				throw new ArgumentNullException( "key" );
			if ( key.Length < 16 || key.Length > 32 || key.Length % 8 != 0 )
				throw new ArgumentException( "Invalid key size" );


			_encrypt = encrypt;

			_keys = new uint[132];


			int keyWords = key.Length / 4;

			for ( int i = 0; i < keyWords; i++ )
				_keys[124 + i] = Helper.ToLEUInt32( key, i * 4 );

			if ( keyWords < 8 )
				_keys[124 + keyWords] = 1;

			for ( int i = 0; i < 132; i++ )
			{
				_keys[i] = Helper.RotateLeft(
					_keys[(i + 132 - 8) % 132] ^
					_keys[(i + 132 - 5) % 132] ^
					_keys[(i + 132 - 3) % 132] ^
					_keys[(i + 132 - 1) % 132] ^
					_phi ^
					(uint) i,
					11 );
			}


			for ( int i = 0; i < 128; )
			{
				ApplySB3( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
				ApplySB2( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
				ApplySB1( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
				ApplySB0( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
				ApplySB7( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
				ApplySB6( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
				ApplySB5( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
				ApplySB4( ref _keys[i++], ref _keys[i++], ref _keys[i++], ref _keys[i++] );
			}

			ApplySB3( ref _keys[128], ref _keys[129], ref _keys[130], ref _keys[131] );
		}

		private void ApplyKeying( int index, ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			int keyIndex = index * 4;

			x0 ^= _keys[keyIndex    ];
			x1 ^= _keys[keyIndex + 1];
			x2 ^= _keys[keyIndex + 2];
			x3 ^= _keys[keyIndex + 3];
		}

		private void Encrypt( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			for ( int i = 0; ; )
			{
				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB0( ref x0, ref x1, ref x2, ref x3 );
				ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB1( ref x0, ref x1, ref x2, ref x3 );
				ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB2( ref x0, ref x1, ref x2, ref x3 );
				ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB3( ref x0, ref x1, ref x2, ref x3 );
				ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB4( ref x0, ref x1, ref x2, ref x3 );
				ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB5( ref x0, ref x1, ref x2, ref x3 );
				ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB6( ref x0, ref x1, ref x2, ref x3 );
				ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyKeying( i++, ref x0, ref x1, ref x2, ref x3 );
				ApplySB7( ref x0, ref x1, ref x2, ref x3 );

				if ( i < 32 )
				{
					ApplyTransformation( ref x0, ref x1, ref x2, ref x3 );
				}
				else
				{
					ApplyKeying( i, ref x0, ref x1, ref x2, ref x3 );
					break;
				}
			}
		}

		private void Decrypt( ref uint x0, ref uint x1, ref uint x2, ref uint x3 )
		{
			for ( int i = 32; i >= 0; )
			{
				if ( i == 32 )
					ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );
				else
					ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseSB7( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );
				ApplyInverseSB6( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );
				ApplyInverseSB5( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );
				ApplyInverseSB4( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );
				ApplyInverseSB3( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );
				ApplyInverseSB2( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );
				ApplyInverseSB1( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );

				ApplyInverseTransformation( ref x0, ref x1, ref x2, ref x3 );
				ApplyInverseSB0( ref x0, ref x1, ref x2, ref x3 );
				ApplyKeying( i--, ref x0, ref x1, ref x2, ref x3 );
			}
		}

		public void Transform( byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset )
		{
			uint x0 = Helper.ToLEUInt32( inputBuffer, inputOffset      );
			uint x1 = Helper.ToLEUInt32( inputBuffer, inputOffset + 4  );
			uint x2 = Helper.ToLEUInt32( inputBuffer, inputOffset + 8  );
			uint x3 = Helper.ToLEUInt32( inputBuffer, inputOffset + 12 );

			if ( _encrypt )
				Encrypt( ref x0, ref x1, ref x2, ref x3 );
			else
				Decrypt( ref x0, ref x1, ref x2, ref x3 );

			Helper.ToLEBytes( x0, outputBuffer, outputOffset      );
			Helper.ToLEBytes( x1, outputBuffer, outputOffset + 4  );
			Helper.ToLEBytes( x2, outputBuffer, outputOffset + 8  );
			Helper.ToLEBytes( x3, outputBuffer, outputOffset + 12 );
		}

		public void Dispose()
		{
			if ( _keys != null )
			{
				Array.Clear( _keys, 0, _keys.Length );
				_keys = null;
			}
		}
	}
}