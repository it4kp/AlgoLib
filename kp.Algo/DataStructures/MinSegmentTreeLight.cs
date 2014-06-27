using System;

namespace kp.Algo.DataStructures
{
	/// <summary>
	/// Segemnt tree for [0..n) and minimum operation
	/// Supports cell modifications
	/// </summary>
	public class MinSegmentTreeLight<T> where T : IComparable<T>
	{
		private readonly T[] _tree;
		private readonly int _shift;
		private readonly int _n;

		public MinSegmentTreeLight( int n, T initialCellValue, T plusInfinity )
			: this( GetArr( n, initialCellValue ), plusInfinity )
		{

		}

		public MinSegmentTreeLight( T[] initialArray, T plusInfinity )
		{
			_n = initialArray.Length;
			_shift = 1;
			while ( _shift < _n ) _shift *= 2;
			_tree = new T[_shift * 2];
			for ( int i = 0; i < _n; ++i )
				_tree[_shift + i] = initialArray[i];
			for ( int i = _shift + _n; i < _tree.Length; ++i )
				_tree[i] = plusInfinity;
			for ( int i = _shift - 1; i >= 1; --i )
			{
				int l = 2 * i, r = 2 * i + 1;
				_tree[i] = _tree[l];
				if ( _tree[i].CompareTo( _tree[r] ) > 0 )
					_tree[i] = _tree[r];
			}
		}

		/// <summary>
		/// Returns minimum value on a segment [l, r]
		/// The complexity is O(log(r-l))
		/// </summary>
		public T GetMin( int l, int r )
		{
			if ( l < 0 || l >= _n || r < 0 || r >= _n || l > r )
				throw new Exception();
			l += _shift;
			r += _shift;
			T res = _tree[l];
			while ( l <= r )
			{
				if ( ( l & 1 ) == 1 )
				{
					if ( res.CompareTo( _tree[l] ) > 0 )
						res = _tree[l];
					l = ( l + 1 ) / 2;
				}
				else l /= 2;
				if ( ( r & 1 ) == 0 )
				{
					if ( res.CompareTo( _tree[r] ) > 0 )
						res = _tree[r];
					r = ( r - 1 ) / 2;
				}
				else r /= 2;
			}
			return res;
		}

		/// <summary>
		/// Gets or sets cell value
		/// Get is O(1), set is O(log(n))
		/// </summary>
		public T this[int index]
		{
			get
			{
				if ( index < 0 || index >= _n )
					throw new Exception();
				return _tree[_shift + index];
			}
			set
			{
				if ( index < 0 || index >= _n )
					throw new Exception();
				index += _shift;
				_tree[index] = value;
				index /= 2;
				while ( index >= 1 )
				{
					int l = 2 * index, r = 2 * index + 1;
					var mn = _tree[l];
					if ( mn.CompareTo( _tree[r] ) > 0 )
						mn = _tree[r];
					if ( _tree[index].CompareTo( mn ) != 0 )
					{
						_tree[index] = mn;
						index /= 2;
					}
					else break;
				}
			}
		}

		private static T[] GetArr( int n, T initialCellValue )
		{
			var arr = new T[n];
			for ( int i = 0; i < n; ++i ) arr[i] = initialCellValue;
			return arr;
		}
	}
}
