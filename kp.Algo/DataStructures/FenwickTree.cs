namespace kp.Algo.DataStructures
{
	/// <summary>
	/// Fenwick tree of elements [0..maxValue]
	/// </summary>
	public class FenwickTree
	{
		private readonly int[] _h;
		private readonly int _maxn;

		public FenwickTree( int maxValue )
		{
			_maxn = maxValue;
			_h = new int[maxValue + 1];
		}

		private int prev( int x )
		{
			return x & ( x + 1 );
		}

		private int next( int x )
		{
			return x | ( x + 1 );
		}

		public void Modify( int x, int valueToAdd )
		{
			while ( x <= _maxn )
			{
				_h[x] += valueToAdd;
				x = next( x );
			}
		}

		public int Sum( int l, int r )
		{
			int res = 0;
			while ( r >= 0 )
			{
				res += _h[r];
				r = prev( r ) - 1;
			}
			--l;
			while ( l >= 0 )
			{
				res -= _h[l];
				l = prev( l ) - 1;
			}
			return res;
		}

		/// <summary>
		/// Returns k-th element (1-based) or -1 if less than k elements
		/// </summary>
		public int KthElement( int k )
		{
			if ( _h[0] >= k ) return 0;
			if ( Sum( 0, _maxn ) < k ) return -1;
			int ll = 0, hh = _maxn;
			while ( ll + 1 < hh )
			{
				int mm = ( ll + hh ) / 2;
				if ( Sum( 0, mm ) >= k ) hh = mm; else ll = mm;
			}
			return hh;
		}
	}
}