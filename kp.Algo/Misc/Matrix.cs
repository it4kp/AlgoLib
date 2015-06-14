using System;

namespace kp.Algo.Misc
{
	public class Matrix
	{
		public static int MOD = 1000000007;

		public readonly int Rows, Cols;
		public readonly int[,] Data;

		public Matrix( int rows, int cols )
		{
			if ( rows <= 0 ) throw new ArgumentOutOfRangeException( "rows" );
			if ( cols <= 0 ) throw new ArgumentOutOfRangeException( "cols" );
			Rows = rows; Cols = cols;
			Data = new int[rows, cols];
		}

		public static Matrix operator *( Matrix a, Matrix b )
		{
			if ( a == null ) throw new ArgumentNullException( "a" );
			if ( b == null ) throw new ArgumentNullException( "b" );
			if ( a.Cols != b.Rows ) throw new ArgumentException( "Invalid matrix dimensions" );
			Matrix c = new Matrix( a.Rows, b.Cols );
			for ( int i = 0; i < a.Rows; i++ )
				for ( int j = 0; j < b.Cols; ++j )
					for ( int k = 0; k < a.Cols; ++k )
					{
						c.Data[i, j] += (int)( ( (long)a.Data[i, k] * b.Data[k, j] ) % MOD );
						if ( c.Data[i, j] >= MOD ) c.Data[i, j] -= MOD;
					}
			return c;
		}

		public static Matrix operator ^( Matrix a, long b )
		{
			if ( a == null ) throw new ArgumentNullException( "a" );
			if ( a.Rows != a.Cols ) throw new ArgumentException( "Can't power non-square matrix" );
			if ( b < 0 ) throw new ArgumentException( "Power can't be negative" );
			Matrix res = new Matrix( a.Rows, a.Cols );
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