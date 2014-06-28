using System;
using System.Collections.Generic;

namespace kp.Algo.Misc
{
	public class MyComparer<T> : Comparer<T>
	{
		private readonly Comparison<T> _comparison;

		public MyComparer( Comparison<T> comparison )
		{
			_comparison = comparison;
		}

		public override int Compare( T x, T y )
		{
			return _comparison( x, y );
		}
	}
}
