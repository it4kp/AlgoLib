using System;
using System.Collections.Generic;
using System.Linq;

namespace kp.Algo
{
	public static class StringUtils
	{
		/// <summary>
		/// Linear suffix array construction
		/// </summary>
		public static int[] GetSuffixArray( string text, int lowerChar, int upperChar )
		{
			if ( string.IsNullOrEmpty( text ) )
				throw new Exception( "text must not be empty for suffix array construction" );
			if ( text.Length == 1 )
			{
				return new int[1];
			}
			int n = text.Length;
			int[] numText = new int[n + 3];
			for ( int i = 0; i < n; ++i )
				numText[i] = text[i] - lowerChar + 1;
			int K = upperChar - lowerChar + 1;
			int[] res = new int[text.Length];
			GetSuffixArray( numText, res, n, K );
			return res;
		}

		/// <summary>
		/// Gets LCP array when the text and its suffix array are known.
		/// Time complexity is linear
		/// </summary>
		public static int[] GetLCP( string text, int[] suffixArray )
		{
			int n = text.Length;
			int[] pos = new int[n];
			for ( int i = 0; i < n; ++i ) pos[suffixArray[i]] = i;
			int[] res = new int[n - 1];
			for ( int i = 0, k = 0; i < n; ++i )
			{
				if ( pos[i] == n - 1 ) continue;
				for ( int j = suffixArray[pos[i] + 1]; i + k < n && j + k < n && text[i + k] == text[j + k]; ) ++k;
				res[pos[i]] = k;
				if ( k > 0 ) --k;
			}
			return res;
		}

		/// <summary>
		/// Gets suffix array of cyclic shifts.
		/// Time complexity: O(NlogN) where N is the text's length
		/// 
		/// Equal cyclic suffixes are order by their position
		/// </summary>
		public static int[] GetSuffixArrayCyclic( string text, int upperChar )
		{
			int n = text.Length;
			int[] res = new int[n], cnt = new int[Math.Max( n, upperChar ) + 1], c = new int[n];
			for ( int i = 0; i < n; ++i )
				++cnt[text[i]];
			for ( int i = 1; i <= upperChar; ++i )
				cnt[i] += cnt[i - 1];
			for ( int i = 0; i < n; ++i )
				res[--cnt[text[i]]] = i;
			c[res[0]] = 0;
			int numClasses = 1;
			for ( int i = 1; i < n; ++i )
			{
				if ( text[res[i]] != text[res[i - 1]] ) ++numClasses;
				c[res[i]] = numClasses - 1;
			}

			int[] tmpP = new int[n], tmpC = new int[n];
			int powH = 1;
			for ( int h = 0; powH < n; ++h, powH *= 2 )
			{
				for ( int i = 0; i < n; ++i )
				{
					tmpP[i] = res[i] - powH;
					if ( tmpP[i] < 0 ) tmpP[i] += n;
				}
				for ( int i = 0; i < numClasses; ++i ) cnt[i] = 0;
				for ( int i = 0; i < n; ++i )
					++cnt[c[tmpP[i]]];
				for ( int i = 1; i < numClasses; ++i )
					cnt[i] += cnt[i - 1];
				for ( int i = n - 1; i >= 0; --i )
					res[--cnt[c[tmpP[i]]]] = tmpP[i];
				tmpC[res[0]] = 0;
				numClasses = 1;
				for ( int i = 1; i < n; ++i )
				{
					int pos1 = res[i] + powH;
					if ( pos1 >= n ) pos1 -= n;
					int pos2 = res[i - 1] + powH;
					if ( pos2 >= n ) pos2 -= n;
					if ( c[res[i]] != c[res[i - 1]] || c[pos1] != c[pos2] )
						++numClasses;
					tmpC[res[i]] = numClasses - 1;
				}
				Array.Copy( tmpC, c, n );
			}

			// Let's order equal cyclic suffixes
			var period = GetStringCyclicPeriod( text );
			var groupSize = text.Length / period;

			if ( groupSize > 1 )
			{
				for ( int i = 0; i < res.Length; i += groupSize )
				{
					Array.Sort( res, i, groupSize );
				}
			}

			return res;
		}

		/// <summary>
		/// Computes z-array. Time complexity: O(N)
		/// </summary>
		public static int[] ZAlgorithm( char[] a )
		{
			int n = a.Length;
			int[] z = new int[n];
			int l = -1, r = -1;
			for ( int i = 1; i <= n - 1; ++i )
			{
				if ( r < i )
				{
					z[i] = 0;
					int j = i;
					while ( j < n && a[j] == a[j - i] ) ++j;
					z[i] = j - i;
					if ( z[i] > 0 ) { l = i; r = j - 1; }
				}
				else
				{
					if ( z[i - l] < r - i + 1 )
					{
						z[i] = z[i - l];
					}
					else
					{
						int j = r + 1;
						while ( j < n && a[j] == a[j - i] ) ++j;
						z[i] = j - i;
						l = i; r = j - 1;
					}
				}
			}
			return z;
		}

		/// <summary>
		/// Returns Aho-Corasic tree for a set of patterns and alphabet parameters
		/// </summary>
		/// <returns></returns>
		public static DataStructures.AhoCorasickTree GetAhoCorasickTree( char[][] patterns, char minChar, char maxChar )
		{
			var tree = new DataStructures.AhoCorasickTree( patterns.Sum( p => p.Length ) + 1, maxChar - minChar + 1, minChar );
			foreach ( var pattern in patterns )
			{
				tree.AddString( pattern );
			}
			return tree;
		}

		/// <summary>
		/// Counts the number of matchings of patterns in a given text
		/// tree must be built with a set of distinct patterns!
		/// </summary>
		public static long CountMatches( char[] text, DataStructures.AhoCorasickTree tree )
		{
			if ( text == null || text.Length == 0 )
				return 0;

			long res = 0;

			var dp = new int[tree.Size];
			ArrayUtils.Fill( dp, -1 );
			dp[0] = 0;

			var st = 0;
			var stack = new int[tree.Size];
			int sz = 0;
			for ( int i = 0; i < text.Length; ++i )
			{
				st = tree.GetNextState( st, (char)( text[i] - tree.MinChar ) );

				int u = st;
				while ( dp[u] == -1 )
				{
					stack[sz++] = u;
					u = tree.GetSuffixLink( u );
				}

				while ( sz > 0 )
				{
					var cur = stack[--sz];
					dp[cur] = ( tree.IsLeaf( cur ) ? 1 : 0 ) + dp[u];
					u = cur;
				}

				res += dp[st];
			}

			return res;
		}

		/// <summary>
		/// Gets the cyclic period of the string
		/// The cyclic period is the least number of cyclic shifts
		/// neede to get the same string.
		/// Time complexity: O(n)
		/// 
		/// EXAMPLE:
		/// for string S = aaaaa the period is 1
		/// for string S = ababab the period is 2
		/// for string S = abba the period is 4
		/// </summary>
		public static int GetStringCyclicPeriod( string s )
		{
			var p = GetPrefixFunction( s );
			var period = s.Length - p[s.Length - 1];
			if ( 2 * period <= s.Length && s.Length % period == 0 )
				return period;
			return s.Length;
		}

		/// <summary>
		/// Gets the linear period of the string
		/// The linear period of string S is the length of smallest string P
		/// such that S is concatenation of P's with last piece possibly trimmed.
		/// Time complexity: O(n)
		/// 
		/// EXAMPLE:
		/// for string S = aaaaa the period is 1, P = a
		/// for string S = ababa the period is 2, P = ab
		/// for string S = abba the period is 3, P = abb
		/// </summary>
		public static int GetStringLinearPeriod( string s )
		{
			var p = GetPrefixFunction( s );
			var period = s.Length - p[s.Length - 1];
			return period;
		}

		/// <summary>
		/// Gets prefix function of a string
		/// 
		/// The firts element is zero, th i-th element
		/// is the maximal length L of a suffix s[i-L..i-1]
		/// that is equal to a prefix s[0..L-1]
		/// 
		/// Time complexity: O(n)
		/// </summary>
		public static int[] GetPrefixFunction( string s )
		{
			var p = new int[s.Length];
			int k = 0;
			for ( int i = 1; i < s.Length; ++i )
			{
				while ( k > 0 && s[k] != s[i] ) k = p[k - 1];
				if ( s[k] == s[i] ) ++k;
				p[i] = k;
			}
			return p;
		}

		/// <summary>
		/// Gets LCP. The time complexity is quadric
		/// </summary>
		internal static int[] GetLCPNaive( string text, int[] suffixArray )
		{
			int n = text.Length;
			int[] res = new int[n - 1];
			for ( int i = 0; i < n - 1; ++i )
			{
				int a = suffixArray[i], b = suffixArray[i + 1];
				while ( a < n && b < n && text[a] == text[b] )
				{
					++a;
					++b;
				}
				res[i] = a - suffixArray[i];
			}
			return res;
		}

		/// <summary>
		/// Computes z-array. Time complexity: O(N^2)
		/// </summary>
		internal static int[] ZAlgorithmNaive( char[] s )
		{
			int n = s.Length;
			int[] res = new int[n];
			for ( int i = 1; i < n; ++i )
			{
				int a = i;
				while ( a < n && s[a] == s[a - i] ) ++a;
				res[i] = a - i;
			}
			return res;
		}

		/// <summary>
		/// Naive suffix array construction algorithm.
		/// Works in O(N^2*logN) time, where N is the text's length
		/// </summary>
		internal static int[] GetSuffixArrayNaive( string text )
		{
			int n = text.Length;
			int[] res = new int[n];
			for ( int i = 0; i < n; ++i ) res[i] = i;
			Array.Sort( res, delegate( int i, int j )
			{
				if ( i == j ) return 0;
				while ( i < n && j < n && text[i] == text[j] )
				{
					++i;
					++j;
				}
				if ( i == n ) return -1;
				if ( j == n ) return 1;
				return text[i] - text[j];
			} );
			return res;
		}

		private static void GetSuffixArray( int[] s, int[] suffixArray, int n, int upperChar )
		{
			int n0 = ( n + 2 ) / 3, n1 = ( n + 1 ) / 3, n2 = n / 3, n02 = n0 + n2;
			int[] s12 = new int[n02 + 3], sa12 = new int[n02 + 3], s0 = new int[n0], sa0 = new int[n0];
			for ( int i = 0, j = 0; i < n + ( n0 - n1 ); i++ )
				if ( i % 3 != 0 ) s12[j++] = i;
			RadixPass( s, s12, sa12, 2, n02, upperChar );
			RadixPass( s, sa12, s12, 1, n02, upperChar );
			RadixPass( s, s12, sa12, 0, n02, upperChar );

			int name = 0, c0 = -1, c1 = -1, c2 = -1;
			for ( int i = 0; i < n02; ++i )
			{
				if ( s[sa12[i]] != c0 || s[sa12[i] + 1] != c1 || s[sa12[i] + 2] != c2 )
				{
					name++;
					c0 = s[sa12[i]];
					c1 = s[sa12[i] + 1];
					c2 = s[sa12[i] + 2];
				}
				if ( sa12[i] % 3 == 1 )
					s12[sa12[i] / 3] = name;
				else
					s12[sa12[i] / 3 + n0] = name;
			}
			if ( name < n02 )
			{
				GetSuffixArray( s12, sa12, n02, name );
				for ( int i = 0; i < n02; ++i ) s12[sa12[i]] = i + 1;
			}
			else
				for ( int i = 0; i < n02; ++i ) sa12[s12[i] - 1] = i;
			for ( int i = 0, j = 0; i < n02; ++i )
				if ( sa12[i] < n0 ) s0[j++] = 3 * sa12[i];
			RadixPass( s, s0, sa0, 0, n0, upperChar );
			for ( int p = 0, t = n0 - n1, k = 0; k < n; k++ )
			{
				int i = ( sa12[t] < n0 ? sa12[t] * 3 + 1 : ( sa12[t] - n0 ) * 3 + 2 );
				int j = sa0[p];
				if ( sa12[t] < n0 ? leq( s[i], s12[sa12[t] + n0], s[j], s12[j / 3] )
					: leq( s[i], s[i + 1], s12[sa12[t] - n0 + 1], s[j], s[j + 1], s12[j / 3 + n0] ) )
				{
					suffixArray[k] = i;
					t++;
					if ( t == n02 )
						for ( k++; p < n0; p++, k++ ) suffixArray[k] = sa0[p];
				}
				else
				{
					suffixArray[k] = j;
					p++;
					if ( p == n0 )
						for ( k++; t < n02; t++, k++ ) suffixArray[k] = ( sa12[t] < n0 ? sa12[t] * 3 + 1 : ( sa12[t] - n0 ) * 3 + 2 );
				}
			}
		}

		private static bool leq( int a1, int a2, int b1, int b2 )
		{
			return a1 < b1 || a1 == b1 && a2 <= b2;
		}

		private static bool leq( int a1, int a2, int a3, int b1, int b2, int b3 )
		{
			return a1 < b1 || a1 == b1 && leq( a2, a3, b2, b3 );
		}

		private static void RadixPass( int[] numText, int[] a, int[] b, int from, int n, int upperChar )
		{
			int[] c = new int[upperChar + 1];
			for ( int i = 0; i < n; ++i ) c[numText[a[i] + from]]++;
			for ( int i = 0, sum = 0; i <= upperChar; ++i )
			{
				int t = c[i];
				c[i] = sum;
				sum += t;
			}
			for ( int i = 0; i < n; ++i ) b[c[numText[a[i] + from]]++] = a[i];
		}
	}
}
