using System;
using System.Collections.Generic;

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
		public static int GetLongestIncreasingSubsequenceLength<T>( IEnumerable<T> sequence ) where T : IComparable<T>
		{
			var minElements = new List<T>();
			foreach ( var element in sequence )
			{
				int l = -1, r = minElements.Count;
				while ( l + 1 < r )
				{
					int m = ( l + r ) / 2;
					if ( minElements[m].CompareTo( element ) > 0 )
						r = m;
					else
						l = m;
				}
				if ( r == minElements.Count )
					minElements.Add( element );
				else
					minElements[r] = element;
			}
			return minElements.Count;
		}

		/// <summary>
		/// Gets the longest increasing subsequence
		/// Returns not the subsequence itself, but the sequence of positions of elements.
		/// Time complexity: O(NlogN)
		/// 
		/// WARNING: Allocates additional O(N) space
		/// </summary>
		public static int[] GetLongestIncreasingSubsequence<T>( IList<T> sequence ) where T : IComparable<T>
		{
			var prev = new int[sequence.Count];
			var minElements = new List<T>();
			var minElementsPositions = new List<int>();
			for ( int i = 0; i < sequence.Count; ++i )
			{
				var element = sequence[i];
				int l = -1, r = minElements.Count;
				while ( l + 1 < r )
				{
					int m = ( l + r ) / 2;
					if ( minElements[m].CompareTo( element ) > 0 )
						r = m;
					else
						l = m;
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
}
