using System;
using kp.Algo.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class HashTests
	{
		[TestMethod]
		public void StringPolynomialHashesMustWork()
		{
			var rnd = new Random( 123 );
			for ( int times = 0; times < 1000; times++ )
			{
				var len = rnd.Next( 20 ) + 1;
				var s = "";
				for ( int i = 0; i < len; i++ )
				{
					s += (char)( 'a' + rnd.Next( 3 ) );
				}
				long p = 3137;
				var h = new PolynomialHash64Overflow( s, p );
				for ( int i = 0; i < len; i++ )
				{
					for ( int j = 0; j < i + 1; j++ )
					{
						long brute = 0;
						for ( int k = j; k <= i; k++ )
						{
							brute = brute * p + s[k];
						}
						Assert.AreEqual( brute, h.GetSubstringHash( j, i ) );
					}
				}
			}
		}
	}
}
