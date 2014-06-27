using System;

namespace kp.Algo.DataStructures
{
	public class Pair<T, K> : IComparable<Pair<T, K>>
		where T : IComparable<T>
		where K : IComparable<K>
	{
		public T First { get; set; }
		public K Second { get; set; }

		#region IComparable<Pair<T,K>> Members

		public int CompareTo( Pair<T, K> other )
		{
			if ( First.CompareTo( other.First ) != 0 )
				return First.CompareTo( other.First );
			return Second.CompareTo( other.Second );
		}

		#endregion
	}
}