using System;
using NUnit.Framework;
using kp.Algo.DataStructures;

namespace kp.Algo.Tests
{
	[TestFixture]
	class DataStructuresTests
	{
		[Test]
		public void PriorityQueueMustWork()
		{
			var rnd = new Random( 123 );
			for ( int len = 1; len <= 1000; ++len )
			{
				var q = new PriorityQueue<int>( len );
				var data = new int[len];
				int mn = int.MaxValue;
				for ( int i = 0; i < len; ++i )
				{
					data[i] = rnd.Next( 20 ) - 10;
					mn = Math.Min( mn, data[i] );
					q.Add( data[i] );
					Assert.AreEqual( mn, q.PeekMin() );
				}
				Array.Sort( data );
				for ( int i = 0; i < len; ++i )
				{
					var num = q.RemoveMin();
					Assert.AreEqual( data[i], num );
				}
			}
		}
	}
}
