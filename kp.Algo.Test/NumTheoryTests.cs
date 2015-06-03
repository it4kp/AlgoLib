using System;
using System.Diagnostics;
using System.Numerics;
using kp.Algo.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class NumTheoryTests
	{
		[TestMethod]
		public void GcdTests()
		{
			for ( int a = -100; a <= 100; ++a )
				for ( int b = -100; b <= 100; ++b )
				{
					int g = NumTheoryUtils.Gcd( a, b );
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

		[TestMethod]
		public void EulerPhiMustWork()
		{
			for ( int i = 1; i <= 1000; ++i )
			{
				int cnt = 0;
				for ( int j = 1; j <= i; ++j )
					if ( NumTheoryUtils.Gcd( i, j ) == 1 ) ++cnt;
				Assert.AreEqual( cnt, NumTheoryUtils.EulerPhi( i ) );
			}

			var phiArr = NumTheoryUtils.EulerPhiArray( 100000 );
			for ( int i = 1; i < phiArr.Length; i++ )
			{
				Assert.AreEqual( NumTheoryUtils.EulerPhi( i ), phiArr[i] );
			}
		}

		[TestMethod]
		public void ArithmeticsMustWork()
		{
			for ( int i = -100; i <= 100; ++i )
				for ( int j = -100; j <= 100; ++j )
					for ( int mod = 1; mod <= 1000; ++mod )
						Assert.AreEqual( ( ( i * j ) % mod + mod ) % mod, NumTheoryUtils.Mul( i, j, mod ) );
			long mx = 1000000000000000000L;
			for ( int i = -10; i <= 10; ++i )
				for ( int j = -10; j <= 10; ++j )
					for ( int mod = 1; mod <= 100; ++mod )
					{
						var bi = new BigInteger( mx - i );
						var bj = new BigInteger( mx - j );
						var bmod = new BigInteger( mx - mod );
						Assert.AreEqual( ( ( ( bi * bj ) % bmod + bmod ) % bmod ).ToString(),
							NumTheoryUtils.Mul( mx - i, mx - j, mx - mod ).ToString(), string.Format( "{0}*{1} modulo {2}", bi, bj, bmod ) );

						bi = new BigInteger( -mx + i );
						bj = new BigInteger( mx - j );
						bmod = new BigInteger( mx - mod );
						Assert.AreEqual( ( ( ( bi * bj ) % bmod + bmod ) % bmod ).ToString(),
							NumTheoryUtils.Mul( i - mx, mx - j, mx - mod ).ToString(), string.Format( "{0}*{1} modulo {2}", bi, bj, bmod ) );

						bi = new BigInteger( mx - i );
						bj = new BigInteger( mx - j );
						bmod = new BigInteger( mod );
						Assert.AreEqual( ( ( ( bi * bj ) % bmod + bmod ) % bmod ).ToString(),
							NumTheoryUtils.Mul( mx - i, mx - j, mod ).ToString(), string.Format( "{0}*{1} modulo {2}", bi, bj, bmod ) );

						bi = new BigInteger( -mx + i );
						bj = new BigInteger( mx - j );
						bmod = new BigInteger( mod );
						Assert.AreEqual( ( ( ( bi * bj ) % bmod + bmod ) % bmod ).ToString(),
							NumTheoryUtils.Mul( i - mx, mx - j, mod ).ToString(), string.Format( "{0}*{1} modulo {2}", bi, bj, bmod ) );
					}
			Assert.AreEqual( (MyBigInteger)1, (MyBigInteger)"1" );
			Assert.AreEqual( (MyBigInteger)12, (MyBigInteger)"12" );
			Assert.AreEqual( (MyBigInteger)123, (MyBigInteger)"123" );
			Assert.AreEqual( (MyBigInteger)1234, (MyBigInteger)"1234" );
			Assert.AreEqual( (MyBigInteger)12345, (MyBigInteger)"12345" );

			Assert.AreEqual( (MyBigInteger)( -1 ), (MyBigInteger)"-1" );
			Assert.AreEqual( (MyBigInteger)( -12 ), (MyBigInteger)"-12" );
			Assert.AreEqual( (MyBigInteger)( -123 ), (MyBigInteger)"-123" );
			Assert.AreEqual( (MyBigInteger)( -1234 ), (MyBigInteger)"-1234" );
			Assert.AreEqual( (MyBigInteger)( -12345 ), (MyBigInteger)"-12345" );
		}

		[TestMethod]
		public void PrimalityTests()
		{
			Assert.IsTrue( NumTheoryUtils.IsPrime( 22801763489 ) );
			Assert.IsTrue( NumTheoryUtils.IsPrime( 252097800623 ) );
			Assert.IsTrue( NumTheoryUtils.IsPrime( 2760727302517 ) );
			Assert.IsTrue( NumTheoryUtils.IsPrime( 29996224275833 ) );

			Assert.IsFalse( NumTheoryUtils.IsPrime( 2038074743L * 2038074751L ) );
			Assert.IsFalse( NumTheoryUtils.IsPrime( 2038074743L * 2038074743L ) );

			for ( int n = 1; n <= 10000; ++n )
				Assert.AreEqual( NumTheoryUtils.IsPrimeNaive( n ), NumTheoryUtils.IsPrime( n ), n.ToString() );
			for ( int n = 1; n <= 10000; ++n )
				Assert.AreEqual( NumTheoryUtils.IsPrimeNaive( n + 1234567 ), NumTheoryUtils.IsPrime( n + 1234567 ), ( n + 1234567 ).ToString() );
		}

		[TestMethod]
		public void DivisorArraysTest()
		{
			var minD = NumTheoryUtils.GetMinDivisorArray( 10000 );
			for ( int i = 2; i <= 10000; ++i )
			{
				for ( int j = 2; j * j <= i; ++j )
					if ( i % j == 0 )
					{
						Assert.AreEqual( j, minD[i] );
						break;
					}
			}

			var maxD = NumTheoryUtils.GetMaxDivisorArray( 10000 );
			for ( int i = 2; i <= 10000; ++i )
			{
				int t = i, corrent = 0;
				for ( int j = 2; j * j <= t; ++j )
					if ( t % j == 0 )
					{
						corrent = j;
						while ( t % j == 0 ) t /= j;
					}
				if ( t > 1 ) corrent = t;
				Assert.AreEqual( corrent, maxD[i], i.ToString() );
			}

			for ( int i = 2; i <= 10000; ++i )
			{
				var divs1 = NumTheoryUtils.Divisors( i, minD );
				var divs2 = NumTheoryUtils.Divisors( i );

				Assert.AreEqual( divs2.Length, divs1.Length, i.ToString() );
				for ( int j = 0; j < divs1.Length; ++j )
					Assert.AreEqual( divs2[j], divs1[j], i + " " + j );
			}
		}
	}
}
