using NUnit.Framework;

namespace kp.Algo.Tests
{
	[TestFixture]
	class MatchingTests
	{
		[Test]
		public void GetMatchingTests()
		{
			int[,] a = { { 1, 2, 1 }, { 2, 2, 1 } };
			int[] colsMatching;
			int res = Matching.GetMinMatching( a, out colsMatching );
			Assert.AreEqual( 2, res );
			Assert.AreEqual( 0, colsMatching[0] );
			Assert.AreEqual( 2, colsMatching[1] );

			res = Matching.GetMaxMatching( a, out colsMatching );
			Assert.AreEqual( 4, res );
			Assert.AreEqual( 1, colsMatching[0] );
			Assert.AreEqual( 0, colsMatching[1] );
		}
	}
}
