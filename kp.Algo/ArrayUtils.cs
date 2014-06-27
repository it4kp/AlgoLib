namespace kp.Algo
{
	public static class ArrayUtils
	{
		public static void Fill<T>( T[] a, T value )
		{
			for ( int i = 0; i < a.Length; ++i ) a[i] = value;
		}

		public static void Fill2<T>( T[,] a, T value )
		{
			for ( int i = 0; i < a.GetLength( 0 ); ++i )
				for ( int j = 0; j < a.GetLength( 1 ); ++j ) a[i, j] = value;
		}

		public static void Fill3<T>( T[, ,] a, T value )
		{
			for ( int i = 0; i < a.GetLength( 0 ); ++i )
				for ( int j = 0; j < a.GetLength( 1 ); ++j )
					for ( int k = 0; k < a.GetLength( 2 ); ++k ) a[i, j, k] = value;
		}

		public static void Fill4<T>( T[, , ,] a, T value )
		{
			for ( int i = 0; i < a.GetLength( 0 ); ++i )
				for ( int j = 0; j < a.GetLength( 1 ); ++j )
					for ( int k = 0; k < a.GetLength( 2 ); ++k )
						for ( int t = 0; t < a.GetLength( 3 ); ++t ) a[i, j, k, t] = value;
		}
	}
}
