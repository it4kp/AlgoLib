using System;

namespace kp.Algo
{
	public static class BitUtils
	{
		/// <summary>
		/// Counts the number of one bits in non-negative number
		/// </summary>
		public static int CountOnes( int x )
		{
			if ( x < 0 ) throw new Exception( "Can't count bits in negative number" );
			int res = 0;
			while ( x > 0 )
			{
				++res;
				x &= ( x - 1 );
			}
			return res;
		}

		/// <summary>
		/// Counts the number of one bits in non-negative number
		/// </summary>
		public static int CountOnes( long x )
		{
			if ( x < 0 ) throw new Exception( "Can't count bits in negative number" );
			int res = 0;
			while ( x > 0 )
			{
				++res;
				x &= ( x - 1 );
			}
			return res;
		}

		/// <summary>
		/// Checks that bit is present in mask
		/// </summary>
		public static bool BitInMask( int mask, int bit )
		{
			return ( mask & ( 1 << bit ) ) != 0;
		}

		/// <summary>
		/// Checks that bit is present in mask
		/// </summary>
		public static bool BitInMask( long mask, int bit )
		{
			return ( mask & ( 1L << bit ) ) != 0;
		}

		/// <summary>
		/// Checks whether x is power of two
		/// </summary>
		public static bool IsPowerOfTwo( int x )
		{
			return x > 0 && ( x & ( x - 1 ) ) == 0;
		}

		/// <summary>
		/// Checks whether x is power of two
		/// </summary>
		public static bool IsPowerOfTwo( long x )
		{
			return x > 0 && ( x & ( x - 1 ) ) == 0;
		}
	}
}
