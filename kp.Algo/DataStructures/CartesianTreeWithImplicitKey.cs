using System;

namespace kp.Algo.DataStructures
{
	/// <summary>
	/// Cartesian tree without explicit keys.
	/// Key is defined as position of node in the sorted array
	/// Each node store only one int field of data.
	/// </summary>
	public class CartesianTreeWithImplicitKey
	{
		private CartesianTreeWithImplicitKey _left, _right;
		private int _size, _priority, _data;

		public int Data
		{
			get { return _data; }
		}

		public int Size
		{
			get { return _size; }
		}

		/// <summary>
		/// Splits tree into two trees. The left tree contains exactly
		/// <param name="leftSize"></param> nodes.
		/// Time complexity: O(logN)
		/// </summary>
		public static void Split( CartesianTreeWithImplicitKey tree, int leftSize,
			out CartesianTreeWithImplicitKey left, out CartesianTreeWithImplicitKey right )
		{
			left = null;
			right = null;
			if ( tree == null )
			{
				return;
			}
			int curLeftSize = tree._left == null ? 0 : tree._left._size;
			if ( curLeftSize >= leftSize )
			{
				Split( tree._left, leftSize, out left, out tree._left );
				right = tree;
			}
			else
			{
				Split( tree._right, leftSize - curLeftSize - 1, out tree._right, out right );
				left = tree;
			}

			RefreshAdditionalData( tree );
		}

		/// <summary>
		/// Merges two trees
		/// Time complexity: O(logN)
		/// </summary>
		public static CartesianTreeWithImplicitKey Merge( CartesianTreeWithImplicitKey left, CartesianTreeWithImplicitKey right )
		{
			if ( left == null )
				return right;
			if ( right == null )
				return left;
			if ( left._priority > right._priority )
			{
				left._right = Merge( left._right, right );
				RefreshAdditionalData( left );
				return left;
			}
			else
			{
				right._left = Merge( left, right._left );
				RefreshAdditionalData( right );
				return right;
			}
		}

		/// <summary>
		/// Inserts new node such that it will have exactly
		/// <param name="itemsToTheLeft"></param> nodes less than it
		/// Time complexity: O(logN)
		/// </summary>
		public static void Insert( ref CartesianTreeWithImplicitKey tree, int data, int priority, int itemsToTheLeft = 0 )
		{
			if ( tree == null )
			{
				tree = new CartesianTreeWithImplicitKey { _priority = priority, _data = data };
				RefreshAdditionalData( tree );
			}
			else
			{
				if ( tree._priority > priority )
				{
					int curLeftCnt = tree._left == null ? 0 : tree._left._size;
					if ( curLeftCnt >= itemsToTheLeft )
					{
						Insert( ref tree._left, data, priority, itemsToTheLeft );
					}
					else
					{
						Insert( ref tree._right, data, priority, itemsToTheLeft - curLeftCnt - 1 );
					}
					RefreshAdditionalData( tree );
				}
				else
				{
					CartesianTreeWithImplicitKey tL, tR;
					Split( tree, itemsToTheLeft, out tL, out tR );
					tree = new CartesianTreeWithImplicitKey
					{
						_priority = priority,
						_data = data,
						_left = tL,
						_right = tR
					};

					RefreshAdditionalData( tree );
				}
			}
		}

		/// <summary>
		/// Erases the node such that it has exactly <param name="itemsToTheLeft"></param>
		/// items to the left of it. It is equivalent to erasing an item that has position
		/// <param name="itemsToTheLeft"></param> in zero-based numeration.
		/// Time complexity: O(logN)
		/// </summary>
		public static void Erase( ref CartesianTreeWithImplicitKey tree, int itemsToTheLeft )
		{
			int curLeftCnt = tree._left == null ? 0 : tree._left._size;
			if ( curLeftCnt == itemsToTheLeft )
			{
				tree = Merge( tree._left, tree._right );
			}
			else if ( curLeftCnt > itemsToTheLeft )
			{
				Erase( ref tree._left, itemsToTheLeft );
			}
			else
			{
				Erase( ref tree._right, itemsToTheLeft - curLeftCnt - 1 );
			}
			RefreshAdditionalData( tree );
		}

		/// <summary>
		/// Traverses the tree and calls <param name="action"></param>.
		/// The calls are in increasing order of nodes
		/// </summary>
		public static void Dfs( CartesianTreeWithImplicitKey tree, Action<CartesianTreeWithImplicitKey> action )
		{
			if ( tree == null ) return;
			Dfs( tree._left, action );
			action( tree );
			Dfs( tree._right, action );
		}

		/// <summary>
		/// Finds node such that it has exactly <param name="itemsToTheLeft"></param>
		/// nodes to the left of it.
		/// Time complexity: O(logN)
		/// </summary>
		public static CartesianTreeWithImplicitKey Find( CartesianTreeWithImplicitKey tree, int itemsToTheLeft )
		{
			if ( tree == null )
				return null;
			int curLeftCnt = tree._left == null ? 0 : tree._left._size;
			if ( curLeftCnt == itemsToTheLeft )
			{
				return tree;
			}
			else if ( curLeftCnt > itemsToTheLeft )
			{
				return Find( tree._left, itemsToTheLeft );
			}
			else
			{
				return Find( tree._right, itemsToTheLeft - curLeftCnt - 1 );
			}
		}

		private static void RefreshAdditionalData( CartesianTreeWithImplicitKey tree )
		{
			if ( tree == null )
				return;
			tree._size = ( tree._left == null ? 0 : tree._left._size ) +
				( tree._right == null ? 0 : tree._right._size ) + 1;
		}
	}
}
