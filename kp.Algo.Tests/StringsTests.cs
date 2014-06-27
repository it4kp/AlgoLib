using System.Collections.Generic;
using NUnit.Framework;

namespace kp.Algo.Tests
{
	[TestFixture]
	class StringsTests
	{
		[Test]
		public void SuffixArrayTests()
		{
			string s = "abacaba";
			var res = Strings.GetSuffixArray( s, 'a', 'c' );
			var resNaive = Strings.GetSuffixArrayNaive( s );
			AssertArraysAreEqual( resNaive, res, s );

			for ( int len = 1; len <= 6; ++len )
			{
				int up = 1;
				for ( int i = 1; i <= len; ++i ) up *= len;
				for ( int mask = 0; mask < up; ++mask )
				{
					s = "";
					int x = mask;
					for ( int i = 0; i < len; ++i )
					{
						s += (char)( ( x % len ) + 'a' );
						x /= len;
					}
					res = Strings.GetSuffixArray( s, 'a', 'a' + len - 1 );
					resNaive = Strings.GetSuffixArrayNaive( s );
					AssertArraysAreEqual( resNaive, res, s );

					int[] lcpNaive = Strings.GetLCPNaive( s, resNaive );
					int[] lcp = Strings.GetLCP( s, res );
					AssertArraysAreEqual( lcpNaive, lcp, s.Length - 1 );

					res = Strings.GetSuffixArrayCyclic( s, 'a' + len - 1 );
					bool[] was = new bool[s.Length];
					for ( int i = 0; i < s.Length; ++i )
						was[res[i]] = true;
					for ( int i = 0; i < s.Length; ++i )
						Assert.IsTrue( was[i] );
					Assert.AreEqual( s.Length, res.Length );
					for ( int i = 0; i < s.Length - 1; ++i )
					{
						Assert.IsTrue( string.CompareOrdinal( ( s.Substring( res[i] ) + s.Substring( 0, res[i] ) ),
							s.Substring( res[i + 1] ) + s.Substring( 0, res[i + 1] ) ) <= 0 );
					}
				}
			}
		}

		[Test]
		public void ZAlgorithmTests()
		{
			for ( int len = 1; len <= 6; ++len )
			{
				int up = 1;
				for ( int i = 1; i <= len; ++i ) up *= len;
				for ( int mask = 0; mask < up; ++mask )
				{
					var s = "";
					int x = mask;
					for ( int i = 0; i < len; ++i )
					{
						s += (char)( ( x % len ) + 'a' );
						x /= len;
					}
					var res = Strings.ZAlgorithm( s.ToCharArray() );
					var resNaive = Strings.ZAlgorithmNaive( s.ToCharArray() );
					AssertArraysAreEqual( resNaive, res, s );
				}
			}
		}

		#region Helper methods

		private void AssertArraysAreEqual( int[] a, int[] b, string text, int len )
		{
			Assert.AreEqual( len, a.Length, text );
			Assert.AreEqual( len, b.Length, text );
			for ( int i = 0; i < len; ++i )
				Assert.AreEqual( a[i], b[i], "Position " + i + " differs. Text: " + text );
		}

		private void AssertArraysAreEqual( int[] a, int[] b, string text )
		{
			Assert.AreEqual( text.Length, a.Length, text );
			Assert.AreEqual( text.Length, b.Length, text );
			for ( int i = 0; i < text.Length; ++i )
				Assert.AreEqual( a[i], b[i], "Position " + i + " differs. Text: " + text );
		}

		private void AssertArraysAreEqual( int[] a, int[] b, int len )
		{
			Assert.AreEqual( len, a.Length );
			Assert.AreEqual( len, b.Length );
			for ( int i = 0; i < len; ++i )
				Assert.AreEqual( a[i], b[i], "Position " + i + " differs" );
		}

		#endregion
	}
}
