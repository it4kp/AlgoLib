using System;
using System.Collections.Generic;

namespace kp.Algo.Graphs
{
	/// <summary>
	/// Graph (not required to be bipartite) to compute maximum matching in O(N^3)
	/// </summary>
	public class MaximumMatchingGeneralGraph
	{
		private readonly int _n;
		private readonly List<int>[] _mates;
		private int[] _p, _match, _base, _q;
		private bool[] _used, _blossom;

		public MaximumMatchingGeneralGraph( int numberOfVertices )
		{
			if ( numberOfVertices < 1 ) throw new Exception( "Wrong number of vertices" );
			_n = numberOfVertices;
			_mates = new List<int>[_n];
			for ( int i = 0; i < _n; ++i ) _mates[i] = new List<int>();
		}

		public void AddEdge( int u, int v )
		{
			if ( u < 0 || u >= _n || v < 0 || v >= _n || u == v ) throw new Exception( "Wrong edge" );
			_mates[u].Add( v );
			_mates[v].Add( u );
		}

		public int GetMaximumMatching( out int[] matching )
		{
			_match = new int[_n];
			for ( int i = 0; i < _n; ++i ) _match[i] = -1;
			_p = new int[_n];
			_used = new bool[_n];
			_base = new int[_n];
			_blossom = new bool[_n];
			_q = new int[_n];
			for ( int i = 0; i < _n; ++i )
				if ( _match[i] == -1 )
				{
					int v = FindPath( i );
					while ( v != -1 )
					{
						int pv = _p[v], ppv = _match[pv];
						_match[v] = pv; _match[pv] = v;
						v = ppv;
					}
				}
			matching = _match;
			int res = 0;
			for ( int i = 0; i < _n; ++i ) if ( _match[i] != -1 ) ++res;
			return res / 2;
		}

		private int FindPath( int root )
		{
			_used = new bool[_n];
			for ( int i = 0; i < _n; ++i ) { _p[i] = -1; _used[i] = false; _base[i] = i; }
			_used[root] = true;
			int qh = 0, qt = 0;
			_q[qt++] = root;
			while ( qh < qt )
			{
				int v = _q[qh++];
				foreach ( int t in _mates[v] )
				{
					int to = t;
					if ( _base[v] == _base[to] || _match[v] == to ) continue;
					if ( to == root || _match[to] != -1 && _p[_match[to]] != -1 )
					{
						int curbase = Lca( v, to );
						Array.Clear( _blossom, 0, _n );
						MarkPath( v, curbase, to );
						MarkPath( to, curbase, v );
						for ( int i = 0; i < _n; ++i )
							if ( _blossom[_base[i]] )
							{
								_base[i] = curbase;
								if ( !_used[i] )
								{
									_used[i] = true;
									_q[qt++] = i;
								}
							}
					}
					else if ( _p[to] == -1 )
					{
						_p[to] = v;
						if ( _match[to] == -1 )
							return to;
						to = _match[to];
						_used[to] = true;
						_q[qt++] = to;
					}
				}
			}
			return -1;
		}

		private int Lca( int a, int b )
		{
			bool[] used = new bool[_n];
			for ( ; ; )
			{
				a = _base[a];
				used[a] = true;
				if ( _match[a] == -1 ) break;
				a = _p[_match[a]];
			}
			for ( ; ; )
			{
				b = _base[b];
				if ( used[b] ) return b;
				b = _p[_match[b]];
			}
		}

		private void MarkPath( int v, int b, int children )
		{
			while ( _base[v] != b )
			{
				_blossom[_base[v]] = _blossom[_base[_match[v]]] = true;
				_p[v] = children;
				children = _match[v];
				v = _p[_match[v]];
			}
		}
	}
}