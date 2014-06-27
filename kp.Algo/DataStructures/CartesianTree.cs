namespace kp.Algo.DataStructures
{
	/// <summary>
	/// Cartesian tree
	/// </summary>
	public class CartesianTree
	{
		private int _key, _priority;
		private CartesianTree _left, _right;

		/// <summary>
		/// Splits the tree into two trees.
		/// The left tree contains all nodes with keys less that <param name="key"></param>
		/// </summary>
		public static void Split( CartesianTree tree, int key, out CartesianTree left, out CartesianTree right )
		{
			left = null;
			right = null;
			if ( tree == null ) return;
			if ( tree._key < key )
			{
				Split( tree._right, key, out tree._right, out right );
				left = tree;
			}
			else
			{
				Split( tree._left, key, out left, out tree._left );
				right = tree;
			}
			RefreshAdditionalData( tree );
		}

		/// <summary>
		/// Inserts the node in tree
		/// </summary>
		public static void Insert( ref CartesianTree tree, int key, int priority )
		{
			if ( tree == null )
			{
				tree = new CartesianTree { _key = key, _priority = priority };
			}
			else
			{
				if ( tree._priority > priority )
				{
					if ( tree._key < key ) Insert( ref tree._right, key, priority );
					else Insert( ref tree._left, key, priority );
				}
				else
				{
					CartesianTree tL, tR;
					Split( tree, key, out tL, out tR );
					tree = new CartesianTree { _key = key, _priority = priority, _left = tL, _right = tR };
				}
			}
			RefreshAdditionalData( tree );
		}

		/// <summary>
		/// Finds the node by key
		/// </summary>
		public static CartesianTree Find( CartesianTree tree, int key )
		{
			if ( tree == null ) return null;
			if ( tree._key == key ) return tree;
			if ( tree._key < key ) return Find( tree._right, key );
			return Find( tree._left, key );
		}

		/// <summary>
		/// Merges two trees into one
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static CartesianTree Merge( CartesianTree left, CartesianTree right )
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

		private static void RefreshAdditionalData( CartesianTree tree )
		{
			if ( tree == null )
				return;

		}
	}
}
