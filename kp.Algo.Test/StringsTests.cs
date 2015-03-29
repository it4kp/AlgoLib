using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class StringsTests
	{
		[TestMethod]
		public void SuffixArrayTests()
		{
			string s = "abacaba";
			var res = StringUtils.GetSuffixArray( s, 'a', 'c' );
			var resNaive = StringUtils.GetSuffixArrayNaive( s );
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
					res = StringUtils.GetSuffixArray( s, 'a', 'a' + len - 1 );
					resNaive = StringUtils.GetSuffixArrayNaive( s );
					AssertArraysAreEqual( resNaive, res, s );

					int[] lcpNaive = StringUtils.GetLCPNaive( s, resNaive );
					int[] lcp = StringUtils.GetLCP( s, res );
					AssertArraysAreEqual( lcpNaive, lcp, s.Length - 1 );

					res = StringUtils.GetSuffixArrayCyclic( s, 'a' + len - 1 );
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

		[TestMethod]
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
					var res = StringUtils.ZAlgorithm( s.ToCharArray() );
					var resNaive = StringUtils.ZAlgorithmNaive( s.ToCharArray() );
					AssertArraysAreEqual( resNaive, res, s );
				}
			}
		}

		[TestMethod]
		public void AhoCorasickTests()
		{
			var rnd = new Random( 123 );
			for ( int test = 0; test < 100; ++test )
			{
				int n = rnd.Next( 10 ) + 1;
				var patterns = new char[n][];
				var allpat = new HashSet<string>();
				for ( int i = 0; i < n; ++i )
				{
					while ( true )
					{
						int len = rnd.Next( 10 ) + 1;
						patterns[i] = new char[len];
						for ( int j = 0; j < len; ++j )
							patterns[i][j] = (char)( 'a' + rnd.Next( 4 ) );
						if ( !allpat.Contains( new string( patterns[i] ) ) )
						{
							allpat.Add( new string( patterns[i] ) );
							break;
						}
					}
				}
				var tree = StringUtils.GetAhoCorasickTree( patterns, 'a', 'z' );
				var strLen = rnd.Next( 1000 ) + 10;
				var s = new char[strLen];
				for ( int i = 0; i < strLen; ++i ) s[i] = (char)( 'a' + rnd.Next( 26 ) );
				var matches = StringUtils.CountMatches( s, tree );

				long brute = 0;
				var str = new string( s );
				for ( int i = 0; i < patterns.Length; ++i )
				{
					var pat = new string( patterns[i] );
					int p = str.IndexOf( pat );
					while ( p != -1 )
					{
						++brute;
						p = str.IndexOf( pat, p + 1 );
					}
				}
				if ( brute != matches )
				{

				}
				Assert.AreEqual( brute, matches );
			}
		}

		[TestMethod]
		public void PrefixTests()
		{
			var rnd = new Random( 123 );
			for ( int times = 0; times < 10000; times++ )
			{
				int l = rnd.Next( 20 ) + 1;
				var ch = new char[l];
				for ( int i = 0; i < l; i++ )
				{
					ch[i] = (char)( rnd.Next( 3 ) + 'a' );
				}

				var s = new string( ch );
				var my = StringUtils.GetPrefixFunction( s );
				var correct = GetPrefixFunctionNaive( s );
				Assert.IsTrue( correct.SequenceEqual( my ) );

				var myPeriod = StringUtils.GetStringCyclicPeriod( s );
				var correctPeriod = GetCyclicPeriodNaive( s );
				Assert.AreEqual( correctPeriod, myPeriod );

				myPeriod = StringUtils.GetStringLinearPeriod( s );
				correctPeriod = GetLinearPeriodNaive( s );
				Assert.AreEqual( correctPeriod, myPeriod );
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

		private int[] GetPrefixFunctionNaive( string s )
		{
			var p = new int[s.Length];
			for ( int i = 1; i < s.Length; i++ )
			{
				for ( int j = 1; j <= i; ++j )
					if ( s.StartsWith( s.Substring( j, i - j + 1 ) ) )
					{
						p[i] = i - j + 1;
						break;
					}
			}
			return p;
		}

		private int GetCyclicPeriodNaive( string s )
		{
			string t = s;
			for ( int i = 0; i < s.Length; ++i )
			{
				t = t.Substring( 1 ) + t[0];
				if ( t == s )
					return i + 1;
			}
			throw new Exception();
		}

		private int GetLinearPeriodNaive( string s )
		{
			for ( int i = 1; i < s.Length; ++i )
			{
				bool ok = true;

				for ( int j = 0; j < s.Length; j++ )
				{
					if ( s[j] != s[j % i] )
					{
						ok = false;
						break;
					}
				}

				if ( ok ) return i;
			}
			return s.Length;
		}

		#endregion
	}
}
