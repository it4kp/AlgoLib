using System;

namespace kp.Algo.DataStructures
{
	public class PriorityQueue<T> where T : IComparable<T>
	{
		private readonly T[] _data;
		public int Size { get; private set; }

		public PriorityQueue( int maxSize )
		{
			_data = new T[maxSize + 1];
		}

		public void Add( T elem )
		{
			if ( Size + 1 == _data.Length ) throw new InvalidOperationException( "Queue overflow" );
			_data[++Size] = elem;
			Up( Size );
		}

		/// <summary>
		/// Removes the smallest element
		/// </summary>
		public T RemoveMin()
		{
			if ( Size == 0 ) throw new InvalidOperationException( "Queue underflow" );
			T res = _data[1];
			_data[1] = _data[Size--];
			Down( 1 );
			return res;
		}

		public T PeekMin()
		{
			if ( Size == 0 ) throw new InvalidOperationException( "Queue underflow" );
			return _data[1];
		}

		private void Down( int u )
		{
			int l = 2 * u, r = l + 1, m = u;
			if ( l <= Size && _data[m].CompareTo( _data[l] ) > 0 ) m = l;
			if ( r <= Size && _data[m].CompareTo( _data[r] ) > 0 ) m = r;
			if ( m != u )
			{
				T tmp = _data[u];
				_data[u] = _data[m];
				_data[m] = tmp;
				Down( m );
			}
		}

		private void Up( int u )
		{
			if ( u > 1 )
			{
				int p = u / 2;
				if ( _data[p].CompareTo( _data[u] ) > 0 )
				{
					T tmp = _data[u];
					_data[u] = _data[p];
					_data[p] = tmp;
					Up( p );
				}
			}
		}
	}
}