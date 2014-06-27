using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class MatchingTests
	{
		[TestMethod]
		public void MinAndMaxMatchingMustWork()
		{
			int[,] a = { { 1, 2, 1 }, { 2, 2, 1 } };
			int[] colsMatching;
			int res = MatchingUtils.GetMinMatching( a, out colsMatching );
			Assert.AreEqual( 2, res );
			Assert.AreEqual( 0, colsMatching[0] );
			Assert.AreEqual( 2, colsMatching[1] );

			res = MatchingUtils.GetMaxMatching( a, out colsMatching );
			Assert.AreEqual( 4, res );
			Assert.AreEqual( 1, colsMatching[0] );
			Assert.AreEqual( 0, colsMatching[1] );

			Random rnd = new Random( 123 );
			for ( int tests = 1; tests <= 1000; ++tests )
			{
				int rows = rnd.Next( 8 ) + 1;
				int cols = rnd.Next( 8 ) + 1;
				a = new int[rows, cols];
				for ( int i = 0; i < rows; ++i )
					for ( int j = 0; j < cols; ++j )
						a[i, j] = rnd.Next( 100001 ) - 50000;
				res = MatchingUtils.GetMinMatching( a, out colsMatching );
				int tmp = 0;
				for ( int i = 0; i < rows; ++i )
					if ( colsMatching[i] != -1 ) tmp += a[i, colsMatching[i]];
				Assert.AreEqual( res, tmp );
				if ( rows <= cols )
				{
					tmp = int.MaxValue;
					int[] p = PermutationUtils.First( cols );
					do
					{
						int cur = 0;
						for ( int i = 0; i < rows; ++i ) cur += a[i, p[i]];
						tmp = Math.Min( tmp, cur );
					} while ( PermutationUtils.Next( p ) );
					Assert.AreEqual( res, tmp );
				}
				else
				{
					tmp = int.MaxValue;
					int[] p = PermutationUtils.First( rows );
					do
					{
						int cur = 0;
						for ( int i = 0; i < cols; ++i ) cur += a[p[i], i];
						tmp = Math.Min( tmp, cur );
					} while ( PermutationUtils.Next( p ) );
					Assert.AreEqual( res, tmp );
				}


				res = MatchingUtils.GetMaxMatching( a, out colsMatching );
				tmp = 0;
				for ( int i = 0; i < rows; ++i )
					if ( colsMatching[i] != -1 ) tmp += a[i, colsMatching[i]];
				Assert.AreEqual( res, tmp );
				if ( rows <= cols )
				{
					tmp = int.MinValue;
					int[] p = PermutationUtils.First( cols );
					do
					{
						int cur = 0;
						for ( int i = 0; i < rows; ++i ) cur += a[i, p[i]];
						tmp = Math.Max( tmp, cur );
					} while ( PermutationUtils.Next( p ) );
					Assert.AreEqual( res, tmp );
				}
				else
				{
					tmp = int.MinValue;
					int[] p = PermutationUtils.First( rows );
					do
					{
						int cur = 0;
						for ( int i = 0; i < cols; ++i ) cur += a[p[i], i];
						tmp = Math.Max( tmp, cur );
					} while ( PermutationUtils.Next( p ) );
					Assert.AreEqual( res, tmp );
				}
			}
		}
	}
}
