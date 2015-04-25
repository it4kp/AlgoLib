using System;

namespace kp.Algo.Misc
{
	/// <summary>
	/// Custom mplementation of arbitrary length arithmetics.
	/// 
	/// WARNING: Length of numbers is bounded by <see cref="MyBigInteger.MaxDigits"/>
	/// </summary>
	public class MyBigInteger : ICloneable, IComparable<MyBigInteger>
	{
		public static int MaxDigits = 120;
		public static int Base = 10000;
		public static int BaseDigits = 4;
		public static string BaseFormat = "0000";

		private int[] _data = new int[( MaxDigits + BaseDigits - 1 ) / BaseDigits];
		private int _sz = 1;
		private int _sign = 1;

		public static MyBigInteger operator /( MyBigInteger a, int b )
		{
			if ( b == 0 )
				throw new DivideByZeroException();
			if ( a.IsZero() ) return new MyBigInteger();
			if ( b < 0 ) return ( -a ) / ( -b );
			MyBigInteger res = new MyBigInteger();
			res._sz = a._sz;
			res._sign = a._sign;
			int carry = 0;
			for ( int i = a._sz - 1; i >= 0; --i )
			{
				carry = carry * Base + a._data[i];
				res._data[i] = carry / b;
				carry %= b;
			}
			res.Norm();
			return res;
		}

		public static MyBigInteger operator *( MyBigInteger a, MyBigInteger b )
		{
			if ( a.IsZero() || b.IsZero() ) return new MyBigInteger();

			MyBigInteger res = new MyBigInteger();

			for ( int i = 0; i < a._sz; ++i )
				for ( int j = 0; j < b._sz; ++j )
				{
					res._data[i + j] += a._data[i] * b._data[j];
					if ( res._data[i + j] >= Base )
					{
						res._data[i + j + 1] += res._data[i + j] / Base;
						res._data[i + j] %= Base;
					}
				}
			int carry = 0;
			res._sz = a._sz + b._sz + 1;
			for ( int i = 0; i < res._sz; ++i )
			{
				carry += res._data[i];
				res._data[i] = carry % Base;
				carry /= Base;
			}
			res._sign = a._sign * b._sign;
			res.Norm();
			return res;
		}

		public static MyBigInteger operator *( MyBigInteger a, int b )
		{
			if ( b == 0 || a.IsZero() ) return new MyBigInteger();
			MyBigInteger res = new MyBigInteger();
			int carry = 0;
			int bsig = b < 0 ? -1 : 1;
			if ( b < 0 ) b = -b;
			for ( int i = 0; i < a._sz || carry > 0; ++i )
			{
				carry += b * a._data[i];
				res._data[i] = carry % Base;
				carry /= Base;
				res._sz = i + 1;
			}

			res._sign = a._sign * bsig;
			res.Norm();
			return res;
		}

		public static MyBigInteger operator -( MyBigInteger o )
		{
			MyBigInteger res = (MyBigInteger)o.Clone();
			res._sign *= -1;
			res.Norm();
			return res;
		}

		public static MyBigInteger operator -( MyBigInteger a, MyBigInteger b )
		{
			if ( a.IsZero() ) return (MyBigInteger)( -b ).Clone();
			if ( b.IsZero() ) return (MyBigInteger)a.Clone();
			if ( a._sign == -1 )
			{
				if ( b._sign == -1 )
				{
					return ( -b ) - ( -a );
				}
				else
				{
					return -( ( -a ) + b );
				}
			}
			else if ( b._sign == -1 ) return a + ( -b );
			if ( a.CompareTo( b ) < 0 ) return -( b - a );
			MyBigInteger res = (MyBigInteger)a.Clone();

			for ( int i = 0; i < a._sz; ++i )
			{
				res._data[i] -= b._data[i];
				if ( res._data[i] < 0 )
				{
					res._data[i] += Base;
					res._data[i + 1]--;
				}
			}

			res.Norm();
			return res;
		}

		public static MyBigInteger operator +( MyBigInteger a, MyBigInteger b )
		{
			if ( a.IsZero() ) return (MyBigInteger)b.Clone();
			if ( b.IsZero() ) return (MyBigInteger)a.Clone();
			if ( a._sign == -1 )
			{
				if ( b._sign == -1 ) return -( ( -a ) + ( -b ) );
				else return b - ( -a );
			}
			else
			{
				if ( b._sign == -1 ) return a - ( -b );
			}
			MyBigInteger res = new MyBigInteger();
			int carry = 0;
			for ( int i = 0; i < a._sz || i < b._sz || carry > 0; ++i )
			{
				carry += a._data[i] + b._data[i];
				res._data[i] = carry % Base;
				carry /= Base;
				res._sz = i + 1;
			}
			res.Norm();
			return res;
		}

		public static bool operator <( MyBigInteger a, MyBigInteger b )
		{
			return a.CompareTo( b ) < 0;
		}

		public static bool operator >( MyBigInteger a, MyBigInteger b )
		{
			return a.CompareTo( b ) > 0;
		}

		public static bool operator ==( MyBigInteger a, MyBigInteger b )
		{
			return a.CompareTo( b ) == 0;
		}

		public static bool operator !=( MyBigInteger a, MyBigInteger b )
		{
			return a.CompareTo( b ) != 0;
		}

		public static implicit operator MyBigInteger( int num )
		{
			MyBigInteger res = new MyBigInteger();
			if ( num < 0 )
			{
				res._sign = -1;
				num = -num;
			}
			res._sz = 0;
			while ( num != 0 )
			{
				res._data[res._sz++] = num % Base;
				num /= Base;
			}
			res.Norm();
			return res;
		}

		public static implicit operator MyBigInteger( string num )
		{
			if ( string.IsNullOrEmpty( num ) )
				throw new InvalidCastException();
			for ( int i = 0; i < num.Length; ++i )
			{
				if ( num[i] < '0' || num[i] > '9' )
				{
					if ( num[i] != '-' || i > 0 )
						throw new InvalidCastException();
				}
			}
			MyBigInteger res = new MyBigInteger();
			int pos = 0;
			if ( num[0] == '-' )
			{
				res._sign = -1;
				pos = 1;
			}

			for ( int i = num.Length - 1; i >= pos; i -= BaseDigits )
			{
				int d = 1;
				for ( int j = 0; j < BaseDigits && i - j >= pos; ++j )
				{
					res._data[res._sz - 1] += d * ( num[i - j] - '0' );
					d *= 10;
				}
				res._sz++;
			}

			res.Norm();
			return res;
		}

		public int Digits
		{
			get
			{
				if ( _sz == 0 )
					return 0;
				if ( _sz == 1 && _data[0] == 0 )
					return 1;
				int res = 0;
				int x = _data[_sz - 1];
				while ( x > 0 )
				{
					++res;
					x /= 10;
				}
				if ( _sz > 1 )
					res += BaseDigits * ( _sz - 1 );
				return res;
			}
		}

		public int CompareTo( MyBigInteger o )
		{
			if ( _sign == -1 )
			{
				if ( o._sign == 1 ) return -1;
			}
			else if ( o._sign == -1 ) return 1;
			int res = 0;

			if ( _sz > o._sz ) res = 1;
			else if ( _sz < o._sz ) res = -1;
			else
			{
				for ( int i = _sz - 1; i >= 0; --i )
					if ( _data[i] > o._data[i] )
					{
						res = 1;
						break;
					}
					else if ( _data[i] < o._data[i] )
					{
						res = -1;
						break;
					}
			}

			if ( _sign == -1 ) res = -res;
			return res;
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if ( _sign == -1 ) sb.Append( "-" );
			sb.Append( _data[_sz - 1] );
			for ( int i = _sz - 2; i >= 0; --i )
			{
				sb.Append( _data[i].ToString( BaseFormat ) );
			}
			return sb.ToString();
		}

		public object Clone()
		{
			MyBigInteger res = new MyBigInteger();
			res._data = (int[])_data.Clone();
			res._sign = _sign;
			res._sz = _sz;
			return res;
		}

		private void Norm()
		{
			while ( _sz > 1 && _data[_sz - 1] == 0 ) --_sz;
			if ( _sz == 1 && _data[0] == 0 ) _sign = 1;
		}

		private bool IsZero()
		{
			return _sz == 1 && _data[0] == 0;
		}

		protected bool Equals( MyBigInteger other )
		{
			return this == other;
		}

		public override bool Equals( object obj )
		{
			if ( ReferenceEquals( null, obj ) ) return false;
			if ( ReferenceEquals( this, obj ) ) return true;
			if ( obj.GetType() != this.GetType() ) return false;
			return Equals( (MyBigInteger)obj );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = ( _data != null ? _data.GetHashCode() : 0 );
				hashCode = ( hashCode * 397 ) ^ _sz;
				hashCode = ( hashCode * 397 ) ^ _sign;
				return hashCode;
			}
		}
	}
}
