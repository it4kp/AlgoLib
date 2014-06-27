using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class BitUtilsTests
	{
		[TestMethod]
		public void MustWork()
		{
			var powers = new[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
			for ( int i = -10; i <= 1100; ++i )
			{
				bool isPowerOfTwo = powers.Contains( i );
				Assert.AreEqual( isPowerOfTwo, BitUtils.IsPowerOfTwo( i ) );

				Assert.AreEqual( isPowerOfTwo, BitUtils.IsPowerOfTwo( (long)i ) );
			}

			for ( int i = 0; i < 63; ++i )
			{
				Assert.IsTrue( BitUtils.IsPowerOfTwo( 1L << i ) );
			}
		}
	}
}
