namespace kp.Algo.DataStructures
{
	/// <summary>
	/// Union of disjoint sets
	/// </summary>
	public class DisjointSetUnion
	{
		private readonly int[] _p;
		private readonly int[] _r;

		/// <summary>
		/// Creates DSU structure.
		/// Sets are numbered from 0 to n-1.
		/// </summary>
		public DisjointSetUnion( int n )
		{
			_p = new int[n];
			_r = new int[n];
			for ( int i = 0; i < n; i++ )
			{
				_p[i] = i;
			}
		}

		/// <summary>
		/// Checks whether a and b are in one set
		/// </summary>
		public bool InOneSet( int a, int b )
		{
			return GetP( a ) == GetP( b );
		}

		/// <summary>
		/// Joins sets which a and b belong to
		/// If they are already in one set, then nothing happens and method returns False
		/// </summary>
		/// <returns>True, if a and b were in different sets, False otherwise</returns>
		public bool Join( int a, int b )
		{
			int pa = GetP( a ), pb = GetP( b );
			if ( pa == pb )
				return false;

			if ( _r[pa] > _r[pb] )
			{
				_p[pb] = pa;
			}
			else
			{
				if ( _r[pa] == _r[pb] )
					++_r[pb];
				_p[pa] = pb;
			}

			return true;
		}

		private int GetP( int a )
		{
			if ( _p[a] == a )
				return a;
			return _p[a] = GetP( _p[a] );
		}
	}
}
