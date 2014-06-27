using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class MatrixUtilsTests
	{
		[TestMethod]
		public void DeterminantMustWork()
		{
			var rnd = new Random( 123 );
			for ( int times = 0; times < 1000; ++times )
			{
				int n = rnd.Next( 20 ) + 1;
				var a = new double[n, n];
				for ( int i = 0; i < n; ++i )
					for ( int j = 0; j < n; ++j )
						a[i, j] = rnd.Next( 10000 ) - 5000;

				double byGauss = MatrixUtils.ComputeDeterminant( n, (double[,])a.Clone() );
				double byNoDivisions = MatrixUtils.ComputeDeterminantNoDivisions( n, (double[,])a.Clone() );

				if ( Math.Abs( byGauss - byNoDivisions ) / Math.Abs( byGauss ) > 1e-6 )
				{
					Assert.IsTrue( Math.Abs( byGauss - byNoDivisions ) < 1e-9 );
				}
			}
		}
	}
}
