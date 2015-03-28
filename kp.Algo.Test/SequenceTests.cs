using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kp.Algo.Test
{
	[TestClass]
	public class SequenceTests
	{
		[TestMethod]
		public void LisMustWork()
		{
			DoMonotoneTest( MonotoneType.Increasing );
			DoMonotoneTest( MonotoneType.Decreasing );
			DoMonotoneTest( MonotoneType.NonIncreasing );
			DoMonotoneTest( MonotoneType.NonDecreasing );
		}

		private void DoMonotoneTest( MonotoneType monotoneType )
		{
			int n = 8;
			var p = PermutationUtils.First( n );
			do
			{
				var copyP = (int[])p.Clone();
				var my = SequenceUtils.GetLongestMonotoneSubsequenceLength( copyP, monotoneType );
				Assert.IsTrue( p.SequenceEqual( copyP ) );
				var correct = GetLongestMonotoneSubsequenceLengthNaive( p, monotoneType );
				Assert.AreEqual( correct, my );

				var lis = SequenceUtils.GetLongestMonotoneSubsequence( copyP, monotoneType );
				Assert.IsTrue( p.SequenceEqual( copyP ) );

				Assert.AreEqual( my, lis.Length );
				for ( int i = 1; i < lis.Length; ++i )
				{
					Assert.IsTrue( lis[i] >= 0 && lis[i] < n );
					Assert.IsTrue( lis[i] > lis[i - 1] );

					switch ( monotoneType )
					{
						case MonotoneType.Increasing:
							Assert.IsTrue( p[lis[i]] > p[lis[i - 1]] );
							break;
						case MonotoneType.Decreasing:
							Assert.IsTrue( p[lis[i]] < p[lis[i - 1]] );
							break;
						case MonotoneType.NonDecreasing:
							Assert.IsTrue( p[lis[i]] >= p[lis[i - 1]] );
							break;
						case MonotoneType.NonIncreasing:
							Assert.IsTrue( p[lis[i]] <= p[lis[i - 1]] );
							break;
						default:
							throw new System.Exception( "Unknown monotone type" );
					}
				}

			} while ( PermutationUtils.Next( p ) );

			var rnd = new Random( 123 );
			for ( int times = 0; times < 100; ++times )
			{
				n = rnd.Next( 50 ) + 10;
				p = new int[n];
				for ( int i = 0; i < n; i++ )
				{
					p[i] = rnd.Next( 20 );
				}
				var correct = GetLongestMonotoneSubsequenceLengthNaive( p, monotoneType );
				var my = SequenceTests.GetLongestMonotoneSubsequenceLengthNaive( p, monotoneType );
				Assert.AreEqual( correct, my );

				n = 8;
				p = new int[n];
				for ( int i = 0; i < n; i++ )
				{
					p[i] = rnd.Next( 5 );
				}
				Array.Sort( p );
				do
				{
					correct = GetLongestMonotoneSubsequenceLengthNaive( p, monotoneType );
					my = SequenceTests.GetLongestMonotoneSubsequenceLengthNaive( p, monotoneType );
					Assert.AreEqual( correct, my );
				} while ( PermutationUtils.Next( p ) );
			}
		}

		private static int GetLongestMonotoneSubsequenceLengthNaive<T>( IList<T> sequence, MonotoneType monotoneType ) where T : IComparable<T>
		{
			int n = sequence.Count, res = 0;
			var dp = new int[n];
			for ( int i = 0; i < n; i++ )
			{
				dp[i] = 1;
				for ( int j = 0; j < i; j++ )
				{
					switch ( monotoneType )
					{
						case MonotoneType.Increasing:
							if ( sequence[i].CompareTo( sequence[j] ) > 0 )
								dp[i] = Math.Max( dp[i], dp[j] + 1 );
							break;
						case MonotoneType.Decreasing:
							if ( sequence[i].CompareTo( sequence[j] ) < 0 )
								dp[i] = Math.Max( dp[i], dp[j] + 1 );
							break;
						case MonotoneType.NonDecreasing:
							if ( sequence[i].CompareTo( sequence[j] ) >= 0 )
								dp[i] = Math.Max( dp[i], dp[j] + 1 );
							break;
						case MonotoneType.NonIncreasing:
							if ( sequence[i].CompareTo( sequence[j] ) <= 0 )
								dp[i] = Math.Max( dp[i], dp[j] + 1 );
							break;
						default:
							throw new System.Exception( "Unknown monotone type" );
					}
				}
				res = Math.Max( res, dp[i] );
			}
			return res;
		}
	}
}
