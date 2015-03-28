using System;

namespace kp.Algo
{
	/// <summary>
	/// Provides various utils for sequences
	/// </summary>
	public static class SequenceUtils
	{
		/// <summary>
		/// Gets the length of the longest increasing subsequence
		/// Time complexity: O(NlogN)
		/// </summary>
		public static int GetLongestIncreasingSubsequenceLength<T>( System.Collections.Generic.IEnumerable<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequenceLength( sequence, MonotoneType.Increasing );
		}

		/// <summary>
		/// Gets the longest increasing subsequence
		/// Returns not the subsequence itself, but the sequence of positions of elements.
		/// Time complexity: O(NlogN)
		/// 
		/// WARNING: Allocates additional O(N) space
		/// </summary>
		public static int[] GetLongestIncreasingSubsequence<T>( System.Collections.Generic.IList<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequence( sequence, MonotoneType.Increasing );
		}

		/// <summary>
		/// Gets the length of the longest decreasing subsequence
		/// Time complexity: O(NlogN)
		/// </summary>
		public static int GetLongestDecreasingSubsequenceLength<T>( System.Collections.Generic.IEnumerable<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequenceLength( sequence, MonotoneType.Decreasing );
		}

		/// <summary>
		/// Gets the longest decreasing subsequence
		/// Returns not the subsequence itself, but the sequence of positions of elements.
		/// Time complexity: O(NlogN)
		/// 
		/// WARNING: Allocates additional O(N) space
		/// </summary>
		public static int[] GetLongestDecreasingSubsequence<T>( System.Collections.Generic.IList<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequence( sequence, MonotoneType.Decreasing );
		}

		/// <summary>
		/// Gets the length of the longest non increasing subsequence
		/// Time complexity: O(NlogN)
		/// </summary>
		public static int GetLongestNonIncreasingSubsequenceLength<T>( System.Collections.Generic.IEnumerable<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequenceLength( sequence, MonotoneType.NonIncreasing );
		}

		/// <summary>
		/// Gets the longest non increasing subsequence
		/// Returns not the subsequence itself, but the sequence of positions of elements.
		/// Time complexity: O(NlogN)
		/// 
		/// WARNING: Allocates additional O(N) space
		/// </summary>
		public static int[] GetLongestNonIncreasingSubsequence<T>( System.Collections.Generic.IList<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequence( sequence, MonotoneType.NonIncreasing );
		}

		/// <summary>
		/// Gets the length of the longest non decreasing subsequence
		/// Time complexity: O(NlogN)
		/// </summary>
		public static int GetLongestNonDecreasingSubsequenceLength<T>( System.Collections.Generic.IEnumerable<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequenceLength( sequence, MonotoneType.NonDecreasing );
		}

		/// <summary>
		/// Gets the longest non decreasing subsequence
		/// Returns not the subsequence itself, but the sequence of positions of elements.
		/// Time complexity: O(NlogN)
		/// 
		/// WARNING: Allocates additional O(N) space
		/// </summary>
		public static int[] GetLongestNonDecreasingSubsequence<T>( System.Collections.Generic.IList<T> sequence ) where T : IComparable<T>
		{
			return GetLongestMonotoneSubsequence( sequence, MonotoneType.NonDecreasing );
		}

		/// <summary>
		/// Gets the length of the longest monotone subsequence with a given type of monotonicity
		/// Time complexity: O(NlogN)
		/// </summary>
		public static int GetLongestMonotoneSubsequenceLength<T>( System.Collections.Generic.IEnumerable<T> sequence, MonotoneType monotoneType ) where T : IComparable<T>
		{
			var minElements = new System.Collections.Generic.List<T>();
			foreach ( var element in sequence )
			{
				int l = -1, r = minElements.Count;
				while ( l + 1 < r )
				{
					int m = ( l + r ) / 2;

					switch ( monotoneType )
					{
						case MonotoneType.Increasing:
							if ( minElements[m].CompareTo( element ) < 0 )
								l = m;
							else
								r = m;
							break;
						case MonotoneType.Decreasing:
							if ( minElements[m].CompareTo( element ) > 0 )
								l = m;
							else
								r = m;
							break;
						case MonotoneType.NonDecreasing:
							if ( minElements[m].CompareTo( element ) > 0 )
								r = m;
							else
								l = m;
							break;
						case MonotoneType.NonIncreasing:
							if ( minElements[m].CompareTo( element ) < 0 )
								r = m;
							else
								l = m;
							break;
						default:
							throw new System.Exception( "Unknown monotone type" );
					}
				}
				if ( r == minElements.Count )
					minElements.Add( element );
				else
					minElements[r] = element;
			}
			return minElements.Count;
		}

		/// <summary>
		/// Gets the longest monotone subsequence with a given type of monotonicity
		/// Returns not the subsequence itself, but the sequence of positions of elements.
		/// Time complexity: O(NlogN)
		/// 
		/// WARNING: Allocates additional O(N) space
		/// </summary>
		public static int[] GetLongestMonotoneSubsequence<T>( System.Collections.Generic.IList<T> sequence, MonotoneType monotoneType ) where T : IComparable<T>
		{
			var prev = new int[sequence.Count];
			var minElements = new System.Collections.Generic.List<T>();
			var minElementsPositions = new System.Collections.Generic.List<int>();
			for ( int i = 0; i < sequence.Count; ++i )
			{
				var element = sequence[i];
				int l = -1, r = minElements.Count;
				while ( l + 1 < r )
				{
					int m = ( l + r ) / 2;
					switch ( monotoneType )
					{
						case MonotoneType.Increasing:
							if ( minElements[m].CompareTo( element ) < 0 )
								l = m;
							else
								r = m;
							break;
						case MonotoneType.Decreasing:
							if ( minElements[m].CompareTo( element ) > 0 )
								l = m;
							else
								r = m;
							break;
						case MonotoneType.NonDecreasing:
							if ( minElements[m].CompareTo( element ) > 0 )
								r = m;
							else
								l = m;
							break;
						case MonotoneType.NonIncreasing:
							if ( minElements[m].CompareTo( element ) < 0 )
								r = m;
							else
								l = m;
							break;
						default:
							throw new System.Exception( "Unknown monotone type" );
					}
				}
				if ( r == minElements.Count )
				{
					minElements.Add( element );
					minElementsPositions.Add( i );
				}
				else
				{
					minElements[r] = element;
					minElementsPositions[r] = i;
				}
				if ( r > 0 )
					prev[i] = minElementsPositions[r - 1];
				else
					prev[i] = -1;
			}

			var ans = new int[minElements.Count];
			int curPosition = minElementsPositions[minElementsPositions.Count - 1], curLength = minElements.Count;
			while ( curPosition != -1 )
			{
				ans[--curLength] = curPosition;
				curPosition = prev[curPosition];
			}
			return ans;
		}
	}

	public enum MonotoneType
	{
		Increasing,
		Decreasing,
		NonIncreasing,
		NonDecreasing
	}
}
