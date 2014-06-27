﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace kp.Algo.Geometry
{
	/// <summary>
	/// Geometry utils
	/// </summary>
	public static class GeometryUtils
	{
		/// <summary>
		/// Gets doubled square of a simple polygon. Points must be given in the
		/// cw or ccw order
		/// </summary>
		public static long PolygonDoubledSquare( IList<PointInt2D> points )
		{
			if ( points.Count < 3 )
				return 0;
			long res = 0;

			for ( int i = 0; i < points.Count - 1; ++i )
			{
				res += points[i] * points[i + 1];
			}

			res += points[points.Count - 1] * points[0];

			return Math.Abs( res );
		}

		/// <summary>
		/// Gets doubled square of the given triangle
		/// </summary>
		public static long TriangleDoubledSquare( PointInt2D p1, PointInt2D p2, PointInt2D p3 )
		{
			return Math.Abs( ( p2 - p1 ) * ( p3 - p1 ) );
		}

		/// <summary>
		/// Gets convex hull of set of points.
		/// Time complexity O(N*logN)
		/// </summary>
		public static IList<PointInt2D> ConvexHull( IList<PointInt2D> points )
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sorts points by angle, relative to the center
		/// Time complexity O(N*logN)
		/// </summary>
		public static IList<PointInt2D> SortByAngle( IList<PointInt2D> points, PointInt2D center )
		{
			return points.OrderBy( p => p, Comparer<PointInt2D>.Create( ( a, b ) =>
			{
				if ( a.Y >= center.Y && b.Y < center.Y || a.Y > center.Y && b.Y <= center.Y )
					return -1;
				if ( b.Y >= center.Y && a.Y < center.Y || b.Y > center.Y && a.Y <= center.Y )
					return 1;
				if ( a.Y == center.Y && b.Y == center.Y )
					if ( a.X >= center.X )
					{
						if ( b.X >= center.X )
						{
							return a.X.CompareTo( b.X );
						}
						return -1;
					}
					else
					{
						if ( b.X >= center.X )
						{
							return 1;
						}
						return -a.X.CompareTo( b.X );
					}
				var cross = ( a - center ) * ( b - center );
				if ( cross > 0 )
					return -1;
				return cross < 0 ? 1 : ( a - center ).DistSq().CompareTo( ( b - center ).DistSq() );
			} ) ).ToList();
		}
	}
}
