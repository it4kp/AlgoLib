using System;
using System.Collections.Generic;
using System.Globalization;

namespace kp.Algo.Temp
{
	public class Point
	{
		public Point( double x, double y )
		{
			X = x;
			Y = y;
		}
		public double X, Y;
		public static Point operator +( Point a, Point b ) { return new Point( a.X + b.X, a.Y + b.Y ); }
		public static Point operator -( Point a, Point b ) { return new Point( a.X - b.X, a.Y - b.Y ); }
		public static double operator *( Point a, Point b ) { return a.X * b.Y - a.Y * b.X; }
		public static Point operator *( Point a, double k ) { return new Point( a.X * k, a.Y * k ); }
		public static Point operator /( Point a, double k ) { return new Point( a.X / k, a.Y / k ); }
		public double Dist() { return Math.Sqrt( X * X + Y * Y ); }
		public Point Norm() { return this / Dist(); }
		public override string ToString() { return string.Format( "{0}, {1}", X.ToString( CultureInfo.InvariantCulture ), Y.ToString( CultureInfo.InvariantCulture ) ); }
	}

	public class Circle
	{
		public double X, Y, R;
	}

	public class Line
	{
		public double A, B, C;

		public Line() : this( 0, 0, 0 ) { }

		public Line( double a, double b, double c )
		{
			A = a;
			B = b;
			C = c;
		}

		public Line( Point a, Point b )
		{
			if ( ( a - b ).Dist() < Geometry.Eps )
				throw new Exception( "Can't make line with two equal points" );
			A = a.Y - b.Y;
			B = b.X - b.Y;
			C = -A * a.X - B * a.Y;
		}
	}

	public static class Geometry
	{
		public static double Eps = 1e-9;

		public static Point Rotate( Point a, Point b, double alpha )
		{
			return new Point
							(
								( a.X - b.X ) * Math.Cos( alpha ) - ( a.Y - b.Y ) * Math.Sin( alpha ) + b.X,
								( a.X - b.X ) * Math.Sin( alpha ) + ( a.Y - b.Y ) * Math.Cos( alpha ) + b.Y
							);
		}

		public static double Square( Point a, Point b, Point c )
		{
			return Math.Abs( ( b - a ) * ( c - a ) ) / 2;
		}

		public static List<Point> CrossCircleAndLine( Circle c, Line l )
		{
			double C = l.A * c.X + l.B * c.Y + l.C;
			double x0 = -l.A * C / ( l.A * l.A + l.B * l.B ), y0 = -l.B * C / ( l.A * l.A + l.B * l.B );
			List<Point> res = new List<Point>();
			if ( C * C > c.R * c.R * ( l.A * l.A + l.B * l.B ) + Eps )
				return new List<Point>();
			else if ( Math.Abs( C * C - c.R * c.R * ( l.A * l.A + l.B * l.B ) ) < Eps )
			{
				res.Add( new Point( x0, y0 ) );
			}
			else
			{
				double d = c.R * c.R - C * C / ( l.A * l.A + l.B * l.B );
				double mult = Math.Sqrt( d / ( l.A * l.A + l.B * l.B ) );
				double ax, ay, bx, by;
				ax = x0 + l.B * mult;
				bx = x0 - l.B * mult;
				ay = y0 - l.A * mult;
				by = y0 + l.A * mult;
				res.Add( new Point( ax, ay ) );
				res.Add( new Point( bx, by ) );
			}
			return res;
		}

		/// <summary>
		/// Returns the list of common points for two circle on null if the two coinside
		/// </summary>
		public static List<Point> CrossCircleAndCircle( Circle c1, Circle c2 )
		{
			if ( Math.Abs( c1.X - c2.X ) < Eps && Math.Abs( c1.Y - c2.Y ) < Eps )
			{
				if ( Math.Abs( c1.R - c2.R ) < Eps ) return null;
				return new List<Point>();
			}
			double tx = c1.X, ty = c1.Y;
			c1.X -= tx;
			c1.Y -= ty;
			c2.X -= tx;
			c2.Y -= ty;

			List<Point> res = CrossCircleAndLine( c1, new Line( -2 * c2.X, -2 * c2.Y, c2.X * c2.X + c2.Y * c2.Y + c1.R * c1.R - c2.R * c2.R ) );
			for ( int i = 0; i < res.Count; ++i )
			{
				res[i].X += tx;
				res[i].Y += ty;
			}
			c1.X += tx;
			c1.Y += ty;
			c2.X += tx;
			c2.Y += ty;
			return res;
		}

		public static double IntersectionOfCircleAndCircle( Circle c1, Circle c2 )
		{
			if ( c1.R > c2.R )
			{
				Circle tmp = c1;
				c1 = c2;
				c2 = tmp;
			}
			double sqD = ( c1.X - c2.X ) * ( c1.X - c2.X ) + ( c1.Y - c2.Y ) * ( c1.Y - c2.Y );
			double D = Math.Sqrt( sqD );
			if ( sqD > ( c1.R + c2.R ) * ( c1.R + c2.R ) - Eps )
				return 0;
			if ( D + c1.R < c2.R + Eps )
				return Math.PI * c1.R * c1.R;
			return c1.R * c1.R / Math.Cos( ( sqD + c1.R * c1.R - c2.R * c2.R ) / ( 2 * D * c1.R ) ) +
				c2.R * c2.R / Math.Cos( ( sqD + c2.R * c2.R - c1.R * c1.R ) / ( 2 * D * c2.R ) ) -
				Math.Sqrt( ( c1.R + c2.R - D ) * ( D + c1.R - c2.R ) * ( D + c2.R - c1.R ) * ( D + c1.R + c2.R ) ) / 2;
		}
	}
}