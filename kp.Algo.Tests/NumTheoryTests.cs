using System;
using NUnit.Framework;

namespace kp.Algo.Tests
{
	[TestFixture]
	class NumTheoryTests
	{
		[Test]
		public void GcdTests()
		{
			for ( int a = -100; a <= 100; ++a )
				for ( int b = -100; b <= 100; ++b )
				{
					int g = NumTheory.Gcd( a, b );
					int brute = 0;
					for ( int i = Math.Max( Math.Abs( a ), Math.Abs( b ) ); i > 0; --i )
					{
						if ( a % i == 0 && b % i == 0 )
						{
							brute = i;
							break;
						}
					}
					Assert.AreEqual( brute, g );
				}
		}
	}
}
