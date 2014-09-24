using System;
namespace Vindinium
{
	public static class Bits
	{
		/// <summary>Gets a (zero index based) UIn32 flag mask.</summary>
		/// <param name="index">
		/// The index of the bit to test for.
		/// </param>
		public static uint GetFlagMaskUInt32(int index)
		{
			switch (index)
			{
				case 00: return 0x00000001;
				case 01: return 0x00000002;
				case 02: return 0x00000004;
				case 03: return 0x00000008;
				case 04: return 0x00000010;
				case 05: return 0x00000020;
				case 06: return 0x00000040;
				case 07: return 0x00000080;
				case 08: return 0x00000100;
				case 09: return 0x00000200;
				case 10: return 0x00000400;
				case 11: return 0x00000800;
				case 12: return 0x00001000;
				case 13: return 0x00002000;
				case 14: return 0x00004000;
				case 15: return 0x00008000;
				case 16: return 0x00010000;
				case 17: return 0x00020000;
				case 18: return 0x00040000;
				case 19: return 0x00080000;
				case 20: return 0x00100000;
				case 21: return 0x00200000;
				case 22: return 0x00400000;
				case 23: return 0x00800000;
				case 24: return 0x01000000;
				case 25: return 0x02000000;
				case 26: return 0x04000000;
				case 27: return 0x08000000;
				case 28: return 0x10000000;
				case 29: return 0x20000000;
				case 30: return 0x40000000;
				case 31: return 0x80000000;
				default: throw new ArgumentOutOfRangeException("index", "Index should be in the range [0, 31].");
			}
		}

		/// <summary>Gets a (zero index based) UIn32 mask.</summary>
		/// <param name="index">
		/// The index of the bit to test for.
		/// </param>
		public static ulong GetFlagMaskUInt64(int index)
		{
			switch (index)
			{
				case 00: return 0x0000000000000001;
				case 01: return 0x0000000000000002;
				case 02: return 0x0000000000000004;
				case 03: return 0x0000000000000008;
				case 04: return 0x0000000000000010;
				case 05: return 0x0000000000000020;
				case 06: return 0x0000000000000040;
				case 07: return 0x0000000000000080;
				case 08: return 0x0000000000000100;
				case 09: return 0x0000000000000200;
				case 10: return 0x0000000000000400;
				case 11: return 0x0000000000000800;
				case 12: return 0x0000000000001000;
				case 13: return 0x0000000000002000;
				case 14: return 0x0000000000004000;
				case 15: return 0x0000000000008000;
				case 16: return 0x0000000000010000;
				case 17: return 0x0000000000020000;
				case 18: return 0x0000000000040000;
				case 19: return 0x0000000000080000;
				case 20: return 0x0000000000100000;
				case 21: return 0x0000000000200000;
				case 22: return 0x0000000000400000;
				case 23: return 0x0000000000800000;
				case 24: return 0x0000000001000000;
				case 25: return 0x0000000002000000;
				case 26: return 0x0000000004000000;
				case 27: return 0x0000000008000000;
				case 28: return 0x0000000010000000;
				case 29: return 0x0000000020000000;
				case 30: return 0x0000000040000000;
				case 31: return 0x0000000080000000;
				case 32: return 0x0000000100000000;
				case 33: return 0x0000000200000000;
				case 34: return 0x0000000400000000;
				case 35: return 0x0000000800000000;
				case 36: return 0x0000001000000000;
				case 37: return 0x0000002000000000;
				case 38: return 0x0000004000000000;
				case 39: return 0x0000008000000000;
				case 40: return 0x0000010000000000;
				case 41: return 0x0000020000000000;
				case 42: return 0x0000040000000000;
				case 43: return 0x0000080000000000;
				case 44: return 0x0000100000000000;
				case 45: return 0x0000200000000000;
				case 46: return 0x0000400000000000;
				case 47: return 0x0000800000000000;
				case 48: return 0x0001000000000000;
				case 49: return 0x0002000000000000;
				case 50: return 0x0004000000000000;
				case 51: return 0x0008000000000000;
				case 52: return 0x0010000000000000;
				case 53: return 0x0020000000000000;
				case 54: return 0x0040000000000000;
				case 55: return 0x0080000000000000;
				case 56: return 0x0100000000000000;
				case 57: return 0x0200000000000000;
				case 58: return 0x0400000000000000;
				case 59: return 0x0800000000000000;
				case 60: return 0x1000000000000000;
				case 61: return 0x2000000000000000;
				case 62: return 0x4000000000000000;
				case 63: return 0x8000000000000000;
				default: throw new ArgumentOutOfRangeException("index", "Index should be in the range [0, 63].");
			}
		}

		/// <summary>Gets a (zero index based) UIn32 unflag mask.</summary>
		/// <param name="index">
		/// The index of the bit to test for.
		/// </param>
		public static uint GetUnflagMaskUInt32(int index)
		{
			switch (index)
			{
				case 00: return 0xfffffffe;
				case 01: return 0xfffffffd;
				case 02: return 0xfffffffb;
				case 03: return 0xfffffff7;
				case 04: return 0xffffffef;
				case 05: return 0xffffffdf;
				case 06: return 0xffffffbf;
				case 07: return 0xffffff7f;
				case 08: return 0xfffffeff;
				case 09: return 0xfffffdff;
				case 10: return 0xfffffbff;
				case 11: return 0xfffff7ff;
				case 12: return 0xffffefff;
				case 13: return 0xffffdfff;
				case 14: return 0xffffbfff;
				case 15: return 0xffff7fff;
				case 16: return 0xfffeffff;
				case 17: return 0xfffdffff;
				case 18: return 0xfffbffff;
				case 19: return 0xfff7ffff;
				case 20: return 0xffefffff;
				case 21: return 0xffdfffff;
				case 22: return 0xffbfffff;
				case 23: return 0xff7fffff;
				case 24: return 0xfeffffff;
				case 25: return 0xfdffffff;
				case 26: return 0xfbffffff;
				case 27: return 0xf7ffffff;
				case 28: return 0xefffffff;
				case 29: return 0xdfffffff;
				case 30: return 0xbfffffff;
				case 31: return 0x7fffffff;
				default: throw new ArgumentOutOfRangeException("index", "Index should be in the range [0, 31].");
			}
		}

		/// <summary>Unflags blocks of 1 bit.
		public static readonly ulong[] Unflag1 = new ulong[]
		#region Unflags
		{
			0xffffffffffffffff,
			0xfffffffffffffffe,
			0xfffffffffffffffd,
			0xfffffffffffffffb,
			0xfffffffffffffff7,
			0xffffffffffffffef,
			0xffffffffffffffdf,
			0xffffffffffffffbf,
			0xffffffffffffff7f,
			0xfffffffffffffeff,
			0xfffffffffffffdff,
			0xfffffffffffffbff,
			0xfffffffffffff7ff,
			0xffffffffffffefff,
			0xffffffffffffdfff,
			0xffffffffffffbfff,
			0xffffffffffff7fff,
			0xfffffffffffeffff,
			0xfffffffffffdffff,
			0xfffffffffffbffff,
			0xfffffffffff7ffff,
			0xffffffffffefffff,
			0xffffffffffdfffff,
			0xffffffffffbfffff,
			0xffffffffff7fffff,
			0xfffffffffeffffff,
			0xfffffffffdffffff,
			0xfffffffffbffffff,
			0xfffffffff7ffffff,
			0xffffffffefffffff,
			0xffffffffdfffffff,
			0xffffffffbfffffff,
			0xffffffff7fffffff,

			0xfffffffeffffffff,
			0xfffffffdffffffff,
			0xfffffffbffffffff,
			0xfffffff7ffffffff,
			0xffffffefffffffff,
			0xffffffdfffffffff,
			0xffffffbfffffffff,
			0xffffff7fffffffff,
			0xfffffeffffffffff,
			0xfffffdffffffffff,
			0xfffffbffffffffff,
			0xfffff7ffffffffff,
			0xffffefffffffffff,
			0xffffdfffffffffff,
			0xffffbfffffffffff,
			0xffff7fffffffffff,
			0xfffeffffffffffff,
			0xfffdffffffffffff,
			0xfffbffffffffffff,
			0xfff7ffffffffffff,
			0xffefffffffffffff,
			0xffdfffffffffffff,
			0xffbfffffffffffff,
			0xff7fffffffffffff,
			0xfeffffffffffffff,
			0xfdffffffffffffff,
			0xfbffffffffffffff,
			0xf7ffffffffffffff,
			0xefffffffffffffff,
			0xdfffffffffffffff,
			0xbfffffffffffffff,
			0x7fffffffffffffff,
		};
		#endregion

		/// <summary>Unflags blocks of 3 bit.</summary>
		/// <remarks>
		/// 21 * 3 = 63, so 21 blocks in a ulong.
		/// 
		/// 1111 1111 1000  ff8
		/// 1111 1100 0111  fb7
		/// 1110 0011 1111  e3f
		/// 0001 1111 1111  1ff
		/// </remarks>
		public static readonly ulong [] Unflag3 = new ulong[]
		#region Unflags
		{
			0xfffffffffffffff8,
			0xffffffffffffffb7,
			0xfffffffffffffe3f,
			0xfffffffffffff1ff,

			0xffffffffffff8fff,
			0xfffffffffffb7fff,
			0xffffffffffe3ffff,
			0xffffffffff1fffff,

			0xfffffffff8ffffff,
			0xffffffffb7ffffff,
			0xfffffffe3fffffff,
			0xfffffff1ffffffff,

			0xffffff8fffffffff,
			0xfffffb7fffffffff,
			0xffffe3ffffffffff,
			0xffff1fffffffffff,

			0xfff8ffffffffffff,
			0xffb7ffffffffffff,
			0xfe3fffffffffffff,
			0xf1ffffffffffffff,

			0x8fffffffffffffff,
		};
		#endregion

		/// <summary>Unflags blocks of 4 bit.</summary>
		public static readonly ulong[] Unflag4 = new ulong[]
		#region Unflags
		{
			0xFFFFFFFFFFFFFFF0,
			0xFFFFFFFFFFFFFF0F,
			0xFFFFFFFFFFFFF0FF,
			0xFFFFFFFFFFFF0FFF,
			0xFFFFFFFFFFF0FFFF,
			0xFFFFFFFFFF0FFFFF,
			0xFFFFFFFFF0FFFFFF,
			0xFFFFFFFF0FFFFFFF,

			0xFFFFFFF0FFFFFFFF,
			0xFFFFFF0FFFFFFFFF,
			0xFFFFF0FFFFFFFFFF,
			0xFFFF0FFFFFFFFFFF,
			0xFFF0FFFFFFFFFFFF,
			0xFF0FFFFFFFFFFFFF,
			0xF0FFFFFFFFFFFFFF,
			0x0FFFFFFFFFFFFFFF,
		};
		#endregion

		public const ushort Mask01 = 0x0001;
		public const ushort Mask02 = 0x0003;
		public const ushort Mask03 = 0x0007;
		public const ushort Mask04 = 0x000f;
		public const ushort Mask07 = 0x007f;
		public const ushort Mask08 = 0x00ff;
		public const ushort Mask09 = 0x01ff;
		public const ushort Mask10 = 0x03ff;
		public const ushort Mask12 = 0x0fff;
		public const ushort Mask13 = 0x1fff;
		public const ushort Mask14 = 0x3fff;
		public const ushort Mask15 = 0x7fff;
		public const int Mask16 = 0x0000ffff;
		public const int Mask18 = 0x0003ffff;
		public const int Mask19 = 0x0007ffff;
		public const int Mask20 = 0x000fffff;
		public const int Mask21 = 0x001fffff;
		public const int Mask22 = 0x003fffff;
		public const int Mask23 = 0x007fffff;
		public const int Mask24 = 0x00ffffff;
		public const int Mask27 = 0x07ffffff;
		public const int Mask31 = 0x7fffffff;
		public const int Mask32 = int.MinValue;

		/// <summary>Counts the number of bits.</summary>
		public static int Count(int bits)
		{
			bits = bits - ((bits >> 1) & 0x55555555);
			bits = (bits & 0x33333333) + ((bits >> 2) & 0x33333333);
			return (((bits + (bits >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
		}

		/// <summary>Counts the number of bits.</summary>
		public static int Count(uint bits)
		{
			unchecked
			{
				bits = bits - ((bits >> 1) & 0x55555555);
				bits = (bits & 0x33333333) + ((bits >> 2) & 0x33333333);
				return (int)((((bits + (bits >> 4)) & 0x0F0F0F0FU) * 0x01010101U) >> 24);
			}
		}

		/// <summary>Counts the number of bits.</summary>
		/// <remarks>
		/// See http://stackoverflow.com/questions/109023
		/// </remarks>
		public static int Count(long bits)
		{
			unchecked
			{
				bits = bits - ((bits >> 1) & 0x5555555555555555);
				bits = (bits & 0x3333333333333333) + ((bits >> 2) & 0x3333333333333333);
				return (int)((((bits + (bits >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56);
			}
		}

		/// <summary>Counts the number of bits.</summary>
		public static int Count(ulong bits)
		{
			unchecked
			{
				bits = bits - ((bits >> 1) & 0x5555555555555555UL);
				bits = (bits & 0x3333333333333333UL) + ((bits >> 2) & 0x3333333333333333UL);
				return (int)((((bits + (bits >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
			}
		}
	}
}

