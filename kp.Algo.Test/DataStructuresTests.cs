using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using kp.Algo.DataStructures;

namespace kp.Algo.Test
{
	[TestClass]
	public class DataStructuresTests
	{
		[TestMethod]
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

		[TestMethod]
		public void LightTreesMustWork()
		{
			var rnd = new Random( 123 );

			// min tree
			for ( int tests = 0; tests < 100; ++tests )
			{
				int n = rnd.Next( 100 ) + 1;
				var arr = new int[n];
				for ( int i = 0; i < n; ++i ) arr[i] = rnd.Next();

				var tree = new MinSegmentTreeLight<int>( arr, int.MaxValue );

				for ( int op = 0; op < 100; ++op )
				{
					if ( rnd.Next( 2 ) == 0 )
					{
						int idx = rnd.Next( n );
						int newVal = rnd.Next();
						arr[idx] = newVal;
						tree[idx] = newVal;
					}
					else
					{
						int l = rnd.Next( n );
						int r = rnd.Next( n );
						if ( l > r )
						{
							int tmp = l;
							l = r;
							r = tmp;
						}
						var brute = arr[l];
						for ( int i = l + 1; i <= r; ++i ) brute = Math.Min( brute, arr[i] );
						var my = tree.GetMin( l, r );
						Assert.AreEqual( brute, my );
					}
				}
			}

			// max tree
			for ( int tests = 0; tests < 100; ++tests )
			{
				int n = rnd.Next( 100 ) + 1;
				var arr = new int[n];
				for ( int i = 0; i < n; ++i ) arr[i] = rnd.Next();

				var tree = new MaxSegmentTreeLight<int>( arr, int.MinValue );

				for ( int op = 0; op < 100; ++op )
				{
					if ( rnd.Next( 2 ) == 0 )
					{
						int idx = rnd.Next( n );
						int newVal = rnd.Next();
						arr[idx] = newVal;
						tree[idx] = newVal;
					}
					else
					{
						int l = rnd.Next( n );
						int r = rnd.Next( n );
						if ( l > r )
						{
							int tmp = l;
							l = r;
							r = tmp;
						}
						var brute = arr[l];
						for ( int i = l + 1; i <= r; ++i ) brute = Math.Max( brute, arr[i] );
						var my = tree.GetMax( l, r );
						Assert.AreEqual( brute, my );
					}
				}
			}
		}
	}
}
