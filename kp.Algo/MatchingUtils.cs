using System;

namespace kp.Algo
{
	/// <summary>
	/// Find min/max cost bipartite perfect matching
	/// </summary>
	public static class MatchingUtils
	{
		/// <summary>
		/// Gets minimum matching
		/// </summary>
		public static int GetMinMatching( int[,] array, out int[] matchedCols )
		{
			int rows = array.GetLength( 0 ), cols = array.GetLength( 1 );
			int INF = int.MinValue;
			for ( int i = 0; i < rows; ++i )
				for ( int j = 0; j < cols; ++j )
					INF = Math.Max( INF, array[i, j] + 1 );
			if ( rows > cols )
			{
				int[,] rev = new int[cols, rows];
				for ( int i = 0; i < rows; ++i )
					for ( int j = 0; j < cols; ++j )
						rev[j, i] = array[i, j];
				int[] revMatchedCols = new int[cols];
				int res = GetMinMatching( rev, out revMatchedCols );
				matchedCols = new int[rows];
				for ( int i = 0; i < rows; ++i ) matchedCols[i] = -1;
				for ( int i = 0; i < cols; ++i )
					if ( revMatchedCols[i] != -1 )
					{
						matchedCols[revMatchedCols[i]] = i;
					}
				return res;
			}
			int[,] a = new int[rows + 1, cols + 1];
			for ( int i = 0; i < rows; ++i )
				for ( int j = 0; j < cols; ++j )
					a[i + 1, j + 1] = array[i, j];
			int[] u = new int[rows + 1], v = new int[cols + 1], p = new int[cols + 1],
				way = new int[cols + 1], minv = new int[cols + 1];
			bool[] used = new bool[cols + 1];
			for ( int i = 1; i <= rows; ++i )
			{
				p[0] = i;
				int unmatchedCol = 0;
				for ( int j = 0; j < cols + 1; ++j )
				{
					minv[j] = INF;
					used[j] = false;
				}
				do
				{
					used[unmatchedCol] = true;
					int unmatchedRow = p[unmatchedCol], minVal = INF, col = 0;
					for ( int j = 1; j <= cols; ++j )
						if ( !used[j] )
						{
							int cur = a[unmatchedRow, j] - u[unmatchedRow] - v[j];
							if ( cur < minv[j] )
							{
								minv[j] = cur;
								way[j] = unmatchedCol;
							}
							if ( minv[j] < minVal )
							{
								minVal = minv[j];
								col = j;
							}
						}
					for ( int j = 0; j <= cols; ++j )
						if ( used[j] )
						{
							u[p[j]] += minVal;
							v[j] -= minVal;
						}
						else
							minv[j] -= minVal;
					unmatchedCol = col;
				} while ( p[unmatchedCol] != 0 );
				do
				{
					int col = way[unmatchedCol];
					p[unmatchedCol] = p[col];
					unmatchedCol = col;
				} while ( unmatchedCol != 0 );
			}
			matchedCols = new int[rows];
			for ( int i = 0; i < rows; ++i ) matchedCols[i] = -1;
			for ( int j = 1; j <= cols; ++j ) if ( p[j] > 0 ) matchedCols[p[j] - 1] = j - 1;
			return -v[0];
		}

        /// <summary>
		/// Gets minimum matching
		/// </summary>
		public static int GetMinMatching(int[,] array)
        {
            int rows = array.GetLength(0), cols = array.GetLength(1);
            int INF = int.MinValue;
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    INF = Math.Max(INF, array[i, j] + 1);
            if (rows > cols)
            {
                int[,] rev = new int[cols, rows];
                for (int i = 0; i < rows; ++i)
                    for (int j = 0; j < cols; ++j)
                        rev[j, i] = array[i, j];
                return GetMinMatching(rev);
            }
            int[,] a = new int[rows + 1, cols + 1];
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    a[i + 1, j + 1] = array[i, j];
            int[] u = new int[rows + 1], v = new int[cols + 1], p = new int[cols + 1],
                way = new int[cols + 1], minv = new int[cols + 1];
            bool[] used = new bool[cols + 1];
            for (int i = 1; i <= rows; ++i)
            {
                p[0] = i;
                int unmatchedCol = 0;
                for (int j = 0; j < cols + 1; ++j)
                {
                    minv[j] = INF;
                    used[j] = false;
                }
                do
                {
                    used[unmatchedCol] = true;
                    int unmatchedRow = p[unmatchedCol], minVal = INF, col = 0;
                    for (int j = 1; j <= cols; ++j)
                        if (!used[j])
                        {
                            int cur = a[unmatchedRow, j] - u[unmatchedRow] - v[j];
                            if (cur < minv[j])
                            {
                                minv[j] = cur;
                                way[j] = unmatchedCol;
                            }
                            if (minv[j] < minVal)
                            {
                                minVal = minv[j];
                                col = j;
                            }
                        }
                    for (int j = 0; j <= cols; ++j)
                        if (used[j])
                        {
                            u[p[j]] += minVal;
                            v[j] -= minVal;
                        }
                        else
                            minv[j] -= minVal;
                    unmatchedCol = col;
                } while (p[unmatchedCol] != 0);
                do
                {
                    int col = way[unmatchedCol];
                    p[unmatchedCol] = p[col];
                    unmatchedCol = col;
                } while (unmatchedCol != 0);
            }
            return -v[0];
        }

        /// <summary>
        /// Gets maximum matching
        /// </summary>
        public static int GetMaxMatching( int[,] array, out int[] matchedCols )
		{
			for ( int i = 0; i < array.GetLength( 0 ); ++i )
				for ( int j = 0; j < array.GetLength( 1 ); ++j )
					array[i, j] = -array[i, j];
			int res = GetMinMatching( array, out matchedCols );
			for ( int i = 0; i < array.GetLength( 0 ); ++i )
				for ( int j = 0; j < array.GetLength( 1 ); ++j )
					array[i, j] = -array[i, j];
			return -res;
		}

        /// <summary>
        /// Gets maximum matching
        /// </summary>
        public static int GetMaxMatching(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); ++i)
                for (int j = 0; j < array.GetLength(1); ++j)
                    array[i, j] = -array[i, j];
            int res = GetMinMatching(array);
            for (int i = 0; i < array.GetLength(0); ++i)
                for (int j = 0; j < array.GetLength(1); ++j)
                    array[i, j] = -array[i, j];
            return -res;
        }
    }
}
