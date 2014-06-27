using System;
using System.Collections.Generic;

namespace kp.Algo.Graphs
{
	/// <summary>
	/// Class for solving 2-SAT problem
	/// </summary>
	public class SAT2Graph
	{
		private readonly int _n;
		private readonly List<int>[] _mates, _backMates;

		public SAT2Graph( int numVariables )
		{
			if ( numVariables <= 0 )
				throw new ArgumentException( "The number of parameters must be greater than one", "numVariables" );
			_n = numVariables;
			_mates = new List<int>[2 * _n];
			_backMates = new List<int>[2 * _n];
			for ( int i = 0; i < 2 * _n; ++i )
			{
				_mates[i] = new List<int>();
				_backMates[i] = new List<int>();
			}
		}

		/// <summary>
		/// Adds implication (var1==var1Value) => (var2==var2Value)
		/// </summary>
		public void AddImplication( int var1, int var2, bool var1Value, bool var2Value )
		{
			if ( var1 < 0 || var1 >= _n )
				throw new ArgumentOutOfRangeException( "var1" );
			if ( var2 < 0 || var2 >= _n )
				throw new ArgumentOutOfRangeException( "var2" );
			int u = var1Value ? var1 : var1 + _n;
			int v = var2Value ? var2 : var2 + _n;
			_mates[u].Add( v );
			_backMates[v].Add( u );
		}

		/// <summary>
		/// Adds clause of type (var1 or var2) where variables can be negated.
		/// Such clause is equivalent to two implications.
		/// </summary>
		public void AddClause( int var1, int var2, bool negateVar1, bool negateVar2 )
		{
			if ( var1 < 0 || var1 >= _n )
				throw new ArgumentOutOfRangeException( "var1" );
			if ( var2 < 0 || var2 >= _n )
				throw new ArgumentOutOfRangeException( "var2" );
			AddImplication( var1, var2, negateVar1, !negateVar2 );
			AddImplication( var2, var1, negateVar2, !negateVar1 );
		}

		/// <summary>
		/// Gets solution of 2-SAT problem. The list of values for each variable that satisfies
		/// all clauses and implications.
		/// Returns null if solution doesn't exist.
		/// Time and memory complexity is O(numVertices + numImplications)
		/// </summary>
		public bool[] GetSolution()
		{
			_visited = new bool[_n * 2];
			_top = new List<int>();
			for ( int i = 0; i < 2 * _n; ++i )
				if ( !_visited[i] )
					Dfs( i );
			_visited = new bool[_n * 2];
			int components = 0;
			_component = new int[2 * _n];
			_compVertices = new List<int>[2 * _n];
			for ( int i = 0; i < 2 * _n; ++i ) _compVertices[i] = new List<int>();
			for ( int i = 2 * _n - 1; i >= 0; --i )
			{
				int cur = _top[i];
				if ( !_visited[cur] ) DfsBack( cur, components++ );
			}
			for ( int i = 0; i < _n; ++i )
				if ( _component[i] == _component[i + _n] )
					return null;
			_compMates = new List<int>[components];
			for ( int i = 0; i < components; ++i ) _compMates[i] = new List<int>();
			for ( int i = 0; i < 2 * _n; ++i )
				foreach ( int u in _mates[i] )
				{
					if ( _component[i] != _component[u] ) _compMates[_component[i]].Add( _component[u] );
				}
			_compTop = new List<int>();
			_visited = new bool[components];
			for ( int i = 0; i < components; ++i )
				if ( !_visited[i] )
					DfsComp( i );
			bool[] ans = new bool[_n], got = new bool[_n];
			for ( int i = 0; i < components; ++i )
			{
				int cur = _compTop[i];
				foreach ( var u in _compVertices[cur] )
				{
					int real = u >= _n ? u - _n : u;
					if ( !got[real] )
					{
						got[real] = true;
						ans[real] = u < _n;
					}
				}
			}
			return ans;
		}

		private void DfsComp( int u )
		{
			_visited[u] = true;
			foreach ( int mate in _compMates[u] )
			{
				if ( !_visited[mate] ) DfsComp( mate );
			}
			_compTop.Add( u );
		}

		private void DfsBack( int u, int component )
		{
			_visited[u] = true;
			_component[u] = component;
			_compVertices[component].Add( u );
			foreach ( int v in _backMates[u] )
			{
				if ( !_visited[v] ) DfsBack( v, component );
			}
		}

		private void Dfs( int u )
		{
			_visited[u] = true;
			foreach ( int mate in _mates[u] )
			{
				if ( !_visited[mate] ) Dfs( mate );
			}
			_top.Add( u );
		}

		private bool[] _visited;
		private List<int> _top, _compTop;
		private int[] _component;
		private List<int>[] _compMates, _compVertices;
	}
}
