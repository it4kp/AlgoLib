namespace kp.Algo.DataStructures
{
	/// <summary>
	/// Aho-Corasick tree
	/// </summary>
	public class AhoCorasickTree
	{
		public readonly int AlphabetSize;
		public readonly char MinChar;
		private readonly Node[] _nodes;
		private int _nodesCount;

		/// <summary>
		/// Creates the tree
		/// </summary>
		public AhoCorasickTree( int maxStates, int alphabetSize, char minChar )
		{
			AlphabetSize = alphabetSize;
			MinChar = minChar;
			_nodes = new Node[maxStates + 1];
			_nodes[0] = new Node( alphabetSize );
			_nodesCount = 1;
		}

		public int Size
		{
			get { return _nodesCount; }
		}

		public bool IsLeaf( int n )
		{
			return _nodes[n].Leaf;
		}

		/// <summary>
		/// Adds string to the tree
		/// Characters must be in [_minChar.._alphabetSize+_minChar)
		/// </summary>
		public void AddString( char[] s )
		{
			int v = 0;
			for ( int i = 0; i < s.Length; ++i )
			{
				var c = (char)( s[i] - MinChar );
				if ( _nodes[v].NextNode[c] == -1 )
				{
					_nodes[_nodesCount] = new Node( AlphabetSize ) { Parent = v, CharFromParent = c };
					_nodes[v].NextNode[c] = _nodesCount++;
				}
				v = _nodes[v].NextNode[c];
			}
			_nodes[v].Leaf = true;
		}

		/// <summary>
		/// Gets suffix link for node (with memoization)
		/// </summary>
		public int GetSuffixLink( int v )
		{
			if ( _nodes[v].SuffixLink == -1 )
				if ( v == 0 || _nodes[v].Parent == 0 )
					_nodes[v].SuffixLink = 0;
				else
					_nodes[v].SuffixLink = GetNextState( GetSuffixLink( _nodes[v].Parent ), _nodes[v].CharFromParent );
			return _nodes[v].SuffixLink;
		}

		/// <summary>
		/// Gets next state for node and character
		/// </summary>
		public int GetNextState( int v, char c )
		{
			if ( _nodes[v].NextState[c] == -1 )
				if ( _nodes[v].NextNode[c] != -1 )
					_nodes[v].NextState[c] = _nodes[v].NextNode[c];
				else
					_nodes[v].NextState[c] = v == 0 ? 0 : GetNextState( GetSuffixLink( v ), c );
			return _nodes[v].NextState[c];
		}

		private class Node
		{
			public readonly int[] NextNode;
			public bool Leaf;
			public int Parent;
			public char CharFromParent;
			public int SuffixLink;
			public readonly int[] NextState;

			public Node( int alphabetSize )
			{
				Parent = -1;
				SuffixLink = -1;
				NextNode = new int[alphabetSize];
				NextState = new int[alphabetSize];
				ArrayUtils.Fill( NextNode, -1 );
				ArrayUtils.Fill( NextState, -1 );
			}
		}
	}
}
