using System.Collections.Generic;
using kp.Algo.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class GeometryTests
	{
		[TestMethod]
		public void SortByAngleTests()
		{
			var points = new List<PointInt2D> { new PointInt2D( 0, 0 ), new PointInt2D( 1, 0 ), new PointInt2D( -1, 0 ) };
			var sorted = GeometryUtils.SortByAngle( points, points[0] );
			Assert.AreEqual( points[0], sorted[0] );
			Assert.AreEqual( points[1], sorted[1] );
			Assert.AreEqual( points[2], sorted[2] );

			sorted = GeometryUtils.SortByAngle( points, points[1] );
			Assert.AreEqual( points[0], sorted[1] );
			Assert.AreEqual( points[1], sorted[0] );
			Assert.AreEqual( points[2], sorted[2] );

			sorted = GeometryUtils.SortByAngle( points, points[2] );
			Assert.AreEqual( points[0], sorted[1] );
			Assert.AreEqual( points[1], sorted[2] );
			Assert.AreEqual( points[2], sorted[0] );
		}
	}
}
