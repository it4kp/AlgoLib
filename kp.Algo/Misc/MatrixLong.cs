using System;

namespace kp.Algo.Misc
{
	public class MatrixLong
	{
		public static long MOD = 1000000007;

		public readonly int Rows, Cols;
		public readonly long[,] Data;

		public MatrixLong( int rows, int cols )
		{
			if ( rows <= 0 ) throw new ArgumentOutOfRangeException( "rows" );
			if ( cols <= 0 ) throw new ArgumentOutOfRangeException( "cols" );
			Rows = rows; Cols = cols;
			Data = new long[rows, cols];
		}

		public static MatrixLong operator *(MatrixLong a, MatrixLong b )
		{
			if ( a == null ) throw new ArgumentNullException( "a" );
			if ( b == null ) throw new ArgumentNullException( "b" );
			if ( a.Cols != b.Rows ) throw new ArgumentException( "Invalid matrix dimensions" );
            MatrixLong c = new MatrixLong( a.Rows, b.Cols );
			for ( int i = 0; i < a.Rows; i++ )
				for ( int j = 0; j < b.Cols; ++j )
					for ( int k = 0; k < a.Cols; ++k )
					{
						c.Data[i, j] += NumTheoryUtils.Mul(a.Data[i, k], b.Data[k, j], MOD);
						if ( c.Data[i, j] >= MOD ) c.Data[i, j] -= MOD;
					}
			return c;
		}

		public static MatrixLong operator ^(MatrixLong a, long b )
		{
			if ( a == null ) throw new ArgumentNullException( "a" );
			if ( a.Rows != a.Cols ) throw new ArgumentException( "Can't power non-square matrix" );
			if ( b < 0 ) throw new ArgumentException( "Power can't be negative" );
            MatrixLong res = new MatrixLong( a.Rows, a.Cols );
			if ( b == 0 )
			{
				for ( int i = 0; i < res.Rows; ++i ) res.Data[i, i] = 1;
			}
			else
			{
				res = a ^ ( b >> 1 );
				res *= res;
				if ( ( b & 1 ) != 0 ) res *= a;
			}
			return res;
		}
	}
}