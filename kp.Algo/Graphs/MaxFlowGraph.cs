using System;
using System.Collections.Generic;

namespace kp.Algo.Graphs
{
	/// <summary>
	/// Graph to compute max flow in O(N^3) (push-relabel)
	/// </summary>
	public class MaxFlowGraph
	{
		public const int Inf = 1000000000;
		public readonly int N;
		public readonly int[,] Mates;
		public readonly int[,] Capacity;
		public int[,] Flow;

		public MaxFlowGraph( int numVertices )
		{
			N = numVertices;
			Mates = new int[N, N + 1];
			Capacity = new int[N, N];
			Flow = new int[N, N];
		}

		public void AddUndirectedEdge( int u, int v, int capacity )
		{
			Capacity[u, v] += capacity;
			Capacity[v, u] += capacity;
			Mates[u, ++Mates[u, 0]] = v;
			Mates[v, ++Mates[v, 0]] = u;
		}

		public void AddDirectedEdge( int u, int v, int capacity )
		{
			Capacity[u, v] += capacity;
			Mates[u, ++Mates[u, 0]] = v;
			Mates[v, ++Mates[v, 0]] = u;
		}

		private int[] _h, _e, _cur;
		private Queue<int> _q;
		public int MaxFlow( int from, int to )
		{
			_h = new int[N]; _e = new int[N]; _cur = new int[N];
			Flow = new int[N, N];
			_h[from] = N;
			for ( int i = 0; i < N; ++i ) _cur[i] = 1;
			_q = new Queue<int>();
			for ( int i = 1; i <= Mates[from, 0]; ++i )
			{
				int v = Mates[from, i];
				Flow[from, v] = Capacity[from, v];
				Flow[v, from] = -Flow[from, v];
				_e[v] = Capacity[from, v];
				_q.Enqueue( v );
			}
			while ( _q.Count > 0 )
			{
				int u = _q.Dequeue();
				if ( u == from || u == to ) continue;
				Relax( u );
			}
			return _e[to];
		}

		void Lift( int u )
		{
			int minh = Inf;
			for ( int i = 1; i <= Mates[u, 0]; ++i )
			{
				int v = Mates[u, i];
				if ( Capacity[u, v] - Flow[u, v] > 0 ) minh = Math.Min( minh, _h[v] );
			}
			_h[u] = minh + 1;
		}

		void Push( int u, int v )
		{
			int d = Math.Min( Capacity[u, v] - Flow[u, v], _e[u] );
			_e[u] -= d; Flow[u, v] += d; Flow[v, u] = -Flow[u, v]; _e[v] += d;
		}

		void Relax( int u )
		{
			while ( _e[u] > 0 )
			{
				int v = Mates[u, _cur[u]];
				if ( _cur[u] > Mates[u, 0] )
				{
					Lift( u );
					_cur[u] = 1;
				}
				else if ( Capacity[u, v] - Flow[u, v] > 0 && _h[u] == _h[v] + 1 )
				{
					Push( u, v );
					_q.Enqueue( v );
					++_cur[u];
				}
				else ++_cur[u];
			}
		}
	}
}