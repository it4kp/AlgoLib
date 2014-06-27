using System;

namespace kp.Algo
{
	public static class MatrixUtils
	{
		/// <summary>
		/// Computes matrix determinant.
		/// Complexity: O(n^3)
		/// </summary>
		public static double ComputeDeterminant( int n, double[,] matrix )
		{
			const double EPS = 1e-9;
			double det = 1;
			for ( int i = 0; i < n; ++i )
			{
				int k = i;
				for ( int j = i + 1; j < n; ++j )
					if ( Math.Abs( matrix[j, i] ) > Math.Abs( matrix[k, i] ) )
						k = j;
				if ( Math.Abs( matrix[k, i] ) < EPS )
				{
					det = 0;
					break;
				}
				if ( k != i )
					for ( int j = 0; j < n; ++j )
					{
						double tmp = matrix[i, j];
						matrix[i, j] = matrix[k, j];
						matrix[k, j] = tmp;
					}
				if ( i != k )
					det = -det;
				det *= matrix[i, i];
				for ( int j = i + 1; j < n; ++j )
					matrix[i, j] /= matrix[i, i];
				for ( int j = 0; j < n; ++j )
					if ( j != i && Math.Abs( matrix[j, i] ) > EPS )
						for ( int t = i + 1; t < n; ++t )
							matrix[j, t] -= matrix[i, t] * matrix[j, i];
			}
			return det;
		}

		/// <summary>
		/// Computes determinant.
		/// Uses no divisions.
		/// Complexity: O(n^4)
		/// </summary>
		public static double ComputeDeterminantNoDivisions( int n, double[,] matrix )
		{
			var dp = new double[2, n + 1, n + 1, 2];
			for ( int i = 0; i < n; ++i ) dp[0, i, i, 0] = 1;

			int k1 = 0, k2 = 1;
			for ( int l = 1; l < n; ++l )
			{

				for ( int j = 0; j < n; ++j )
					for ( int k = 0; k <= j; ++k )
						for ( int t = 0; t < 2; ++t )
						{
							dp[k2, j, k, t] = 0;
						}

				for ( int j = 0; j < n; ++j )
					for ( int k = 0; k <= j; ++k )
						for ( int t = 0; t < 2; ++t )
						{
							for ( int c = k + 1; c < n; ++c )
							{
								dp[k2, c, k, t] += dp[k1, j, k, t] * matrix[j, c];
								dp[k2, c, c, 1 - t] += dp[k1, j, k, t] * matrix[j, k];
							}
						}

				k1 ^= 1;
				k2 ^= 1;
			}

			double res = 0;

			for ( int j = 0; j < n; ++j )
				for ( int k = 0; k < n; ++k )
				{
					if ( n % 2 == 0 )
					{
						res += dp[k1, j, k, 1] * matrix[j, k];
						res -= dp[k1, j, k, 0] * matrix[j, k];
					}
					else
					{
						res += dp[k1, j, k, 0] * matrix[j, k];
						res -= dp[k1, j, k, 1] * matrix[j, k];
					}
				}
			return res;
		}
	}
}
