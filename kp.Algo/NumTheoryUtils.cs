using System;
using System.Collections.Generic;

namespace kp.Algo
{
	public static class NumTheoryUtils
	{
		/// <summary>
		/// Computes gcd of two numbers
		/// </summary>
		public static int Gcd( int a, int b )
		{
			if ( a < 0 ) a = -a;
			if ( b < 0 ) b = -b;
			return b == 0 ? a : Gcd( b, a % b );
		}

		/// <summary>
		/// Computes gcd of two numbers
		/// </summary>
		public static long Gcd( long a, long b )
		{
			if ( a < 0 ) a = -a;
			if ( b < 0 ) b = -b;
			return b == 0 ? a : Gcd( b, a % b );
		}

		/// <summary>
		/// Creates prime number sieve. False means the number is prime.
		/// </summary>
		public static bool[] Sieve( int n )
		{
			bool[] res = new bool[n];
			if ( n > 0 ) res[0] = true;
			if ( n > 1 ) res[1] = true;
			for ( int i = 2; i * i < n; ++i )
				if ( !res[i] )
					for ( int j = i * i; j < n; j += i )
						res[j] = true;
			return res;
		}

		/// <summary>
		/// Let canonical factorization of n be n = p1^a1*p2^a2*...*pk^ak
		/// then the return array is [p1,a1,p2,a2,...,pk,ak].
		/// Complexity: O(sqrt(n))
		/// </summary>
		public static int[] Factorize( int n )
		{
			if ( n < 2 ) throw new Exception( "Minimum number to factorize is 2" );
			List<int> result = new List<int>();
			for ( int i = 2; i * i <= n; ++i )
				if ( n % i == 0 )
				{
					int cnt = 0;
					while ( n % i == 0 )
					{
						n /= i;
						++cnt;
					}
					result.Add( i );
					result.Add( cnt );
				}
			if ( n > 1 )
			{
				result.Add( n );
				result.Add( 1 );
			}
			return result.ToArray();
		}

		/// <summary>
		/// Let canonical factorization of n be n = p1^a1*p2^a2*...*pk^ak
		/// then the return array is [p1,a1,p2,a2,...,pk,ak].
		/// Complexity: O(sqrt(n))
		/// </summary>
		public static long[] Factorize( long n )
		{
			if ( n < 2 ) throw new Exception( "Minimum number to factorize is 2" );
			List<long> result = new List<long>();
			for ( long i = 2; i * i <= n; ++i )
				if ( n % i == 0 )
				{
					int cnt = 0;
					while ( n % i == 0 )
					{
						n /= i;
						++cnt;
					}
					result.Add( i );
					result.Add( cnt );
				}
			if ( n > 1 )
			{
				result.Add( n );
				result.Add( 1 );
			}
			return result.ToArray();
		}

		/// <summary>
		/// Primality test. Complexity O(sqrt(n))
		/// </summary>
		public static bool IsPrimeNaive( int n )
		{
			if ( n < 2 ) return false;
			for ( int i = 2; i * i <= n; ++i ) if ( n % i == 0 ) return false;
			return true;
		}

		/// <summary>
		/// Primality test
		/// </summary>
		public static bool IsPrime( long n )
		{
			if ( n < 100 ) return IsPrimeNaive( (int)n );
			if ( ( n & 1 ) == 0 ) return false;

			if ( n < 118670087467 )
			{
				if ( n == 3215031751 ) return false;
				if ( IsCompositeByWitness( n, 2 ) ) return false;
				if ( IsCompositeByWitness( n, 3 ) ) return false;
				if ( IsCompositeByWitness( n, 5 ) ) return false;
				if ( IsCompositeByWitness( n, 7 ) ) return false;
				return true;
			}
			else
			{
				if ( IsCompositeByWitness( n, 2 ) ) return false;
				if ( IsCompositeByWitness( n, 325 ) ) return false;
				if ( IsCompositeByWitness( n, 9375 ) ) return false;
				if ( IsCompositeByWitness( n, 28178 ) ) return false;
				if ( IsCompositeByWitness( n, 450775 ) ) return false;
				if ( IsCompositeByWitness( n, 9780504 ) ) return false;
				if ( IsCompositeByWitness( n, 1795265022 ) ) return false;
				return true;
			}
		}

		private static bool IsCompositeByWitness( long n, long witness )
		{
			var d = n - 1;
			var s = 0;
			while ( ( d & 1 ) == 0 )
			{
				d >>= 1;
				++s;
			}
			var x = n < int.MaxValue ? Pow( witness, d, n ) : PowWith64Overflow( witness, d, n );
			if ( x == 1 ) return false;
			if ( x == n - 1 ) return false;
			for ( int times = 0; times < s - 1; ++times )
			{
				x = ( n < int.MaxValue ? Mul( (int)x, (int)x, (int)n ) : Mul( x, x, n ) );
				if ( x == n - 1 ) return false;
			}
			return true;
		}

		/// <summary>
		/// Gets array [0..n] that for each number contains its minimum prime divisor greater than 1
		/// The time complexity is O(n*log(n))
		/// </summary>
		public static int[] GetMinDivisorArray( int n )
		{
			var res = new int[n + 1];

			for ( int i = 2; i * i <= n; ++i )
				if ( res[i] == 0 )
					for ( int j = i; j <= n; j += i )
						if ( res[j] == 0 )
							res[j] = i;

			for ( int i = 2; i <= n; ++i )
				if ( res[i] == 0 ) res[i] = i;

			return res;
		}

		/// <summary>
		/// Gets array [0..n] that for each number contains its maximum prime divisor
		/// The time complexity is O(n*log(n))
		/// </summary>
		public static int[] GetMaxDivisorArray( int n )
		{
			var res = new int[n + 1];

			for ( int i = 2; i <= n; ++i )
				if ( res[i] == 0 )
					for ( int j = i; j <= n; j += i )
						res[j] = i;

			return res;
		}

		/// <summary>
		/// Returns all divisors of integer n in ascending order.
		/// Complexity: O(number of divisors)
		/// </summary>
		public static int[] Divisors( int n, int[] minDivisorArray )
		{
			if ( n < 2 ) throw new Exception( "n must be greater than 1" );
			int t = n, sz = 0;
			while ( t > 1 )
			{
				++sz;
				t /= minDivisorArray[t];
			}
			var primeDivs = new int[sz];
			t = n;
			sz = 0;
			while ( t > 1 )
			{
				primeDivs[sz++] = minDivisorArray[t];
				t /= minDivisorArray[t];
			}
			int cnt = 1;
			for ( int i = 0; i < primeDivs.Length; )
			{
				int j = i + 1;
				while ( j < primeDivs.Length && primeDivs[j] == primeDivs[i] ) ++j;

				cnt *= ( j - i + 1 );

				i = j;
			}
			var result = new int[cnt];
			int resPos = 0;
			GetDivs( 0, primeDivs, 1, result, false, ref resPos );
			Array.Sort( result );
			return result;
		}

		private static void GetDivs( int pos, int[] primeDivs, int curMul, int[] result, bool added, ref int resPos )
		{
			if ( !added )
			{
				result[resPos++] = curMul;
			}
			if ( pos == primeDivs.Length )
				return;
			int j = pos + 1;
			while ( j < primeDivs.Length && primeDivs[j] == primeDivs[pos] ) ++j;
			GetDivs( j, primeDivs, curMul, result, true, ref resPos );
			for ( int i = pos; i < j; ++i )
			{
				curMul *= primeDivs[i];
				GetDivs( j, primeDivs, curMul, result, false, ref resPos );
			}
		}

		/// <summary>
		/// Returns all divisors of integer n in ascending order.
		/// Complexity: O(sqrt(n))
		/// </summary>
		public static int[] Divisors( int n )
		{
			if ( n < 2 ) throw new Exception( "n must be greater than 1" );
			List<int> result = new List<int>();
			for ( int i = 1; i * i <= n; ++i )
				if ( n % i == 0 )
				{
					result.Add( i );
					int x = n / i;
					if ( x != i ) result.Add( x );
				}
			result.Sort();
			return result.ToArray();
		}

		/// <summary>
		/// Returns all divisors of long n in ascending order.
		/// Complexity: O(sqrt(n))
		/// </summary>
		public static long[] Divisors( long n )
		{
			if ( n < 2 ) throw new Exception( "n must be greater than 1" );
			List<long> result = new List<long>();
			for ( long i = 1; i * i <= n; ++i )
				if ( n % i == 0 )
				{
					result.Add( i );
					long x = n / i;
					if ( x != i ) result.Add( x );
				}
			result.Sort();
			return result.ToArray();
		}

		/// <summary>
		/// Extended euclid's algorithm
		/// </summary>
		public static long GcdLongExt( long a, long b, out long x, out long y )
		{
			if ( b == 0 )
			{
				x = 1; y = 0; return a;
			}
			else
			{
				long x1, y1, d;
				d = GcdLongExt( b, a % b, out x1, out y1 );
				x = y1; y = x1 - ( a / b ) * y1;
				return d;
			}
		}

		/// <summary>
		/// Multiplicative inverse of a modulo n
		/// such 0 &lt; x &lt; n that a * x == 1 (mod n) or 0, if not exist
		/// </summary>
		public static long Inverse( long a, long n )
		{
			long x, y, d = GcdLongExt( a, n, out x, out y );
			if ( d != 1 ) return 0;
			else if ( x >= 0 ) return x;
			else return n - ( ( -x ) % n );
		}

		/// <summary>
		/// Modular exponentiation
		/// a^b mod n
		/// </summary>
		public static long Pow( long a, long b, long n )
		{
			if ( b == 0 ) return 1;
			else
			{
				long tmp = Pow( a, b / 2, n );
				tmp *= tmp; tmp %= n;
				if ( ( b & 1 ) != 0 ) tmp *= a;
				return tmp % n;
			}
		}

		/// <summary>
		/// Modular exponentiation when 64 bit overflow possible
		/// a^b mod n
		/// </summary>
		public static long PowWith64Overflow( long a, long b, long n )
		{
			if ( b == 0 ) return 1;
			else
			{
				long tmp = PowWith64Overflow( a, b / 2, n );
				tmp = Mul( tmp, tmp, n );
				if ( ( b & 1 ) != 0 ) tmp = Mul( tmp, a, n ); ;
				return tmp % n;
			}
		}

		/// <summary>
		/// Jacobi symbol (Legendre symbol if n is prime)    
		/// (should be: n - odd and n &gt;= 3)
		/// </summary>
		public static long Jacobi( long a, long n )
		{
			if ( a < 2 ) return a;
			long e = 0, a1 = a, s;
			while ( ( a1 & 1 ) == 0 ) { a1 >>= 1; ++e; }
			if ( ( e & 1 ) == 0 ) s = 1;
			else
			{
				long m = n % 8;
				if ( m == 1 || m == 7 ) s = 1; else s = -1;
			}
			if ( ( n % 4 ) == 3 && ( a1 % 4 ) == 3 ) s = -s;
			if ( a1 == 1 ) return s;
			else return ( s * Jacobi( n % a1, a1 ) );
		}

		/// <summary>
		/// Square root of a modulo p
		/// such x that x*x == a (mod p) or 0, if not exist        
		/// p should be an odd prime and 0 &lt; a &lt; p 
		/// </summary>
		public static long SquareRoot( long a, long p )
		{
			if ( Jacobi( a, p ) == -1 ) return 0;
			long b = 1;
			while ( Jacobi( b, p ) == 1 ) ++b;
			long t = p - 1;
			int s = 0;
			while ( ( t & 1 ) == 0 ) { t >>= 1; ++s; }
			long a1 = Inverse( a, p ), c = Pow( b, t, p ), r = Pow( a, ( t + 1 ) / 2, p );
			for ( int i = 1; i <= s - 1; ++i )
			{
				long d = ( ( ( r * r ) % p ) * a1 ) % p;
				d = Pow( d, 1 << ( s - i - 1 ), p );
				if ( ( d % p ) == p - 1 ) r = ( r * c ) % p;
				c = ( c * c ) % p;
			}
			return r;
		}

		/// <summary>
		/// Multiplies two integers using modulo. Stores intermediate result in long to avoid 32 bit overflow.
		/// Returns integer in [0,mod)
		/// </summary>
		public static int Mul( int a, int b, int mod )
		{
			int res = (int)( ( (long)a * b ) % mod );
			if ( res < 0 ) res += mod;
			return res;
		}

		/// <summary>
		/// Multiplies two longs using modulo. Uses logarithmic addition
		/// so complexity is O(log(max(a,b)))
		/// Returns long in [0,mod)
		/// </summary>
		public static long Mul( long a, long b, long mod )
		{
			if ( a == 0 || b == 0 ) return 0;
			if ( Math.Abs( a ) <= long.MaxValue / Math.Abs( b ) )
			{
				long result = ( a * b ) % mod;
				if ( result < 0 ) result += mod;
				return result;
			}
			if ( ( a & 1 ) == 0 )
			{
				long result = 2 * Mul( a / 2, b, mod );
				if ( result >= mod ) result -= mod;
				return result;
			}
			else if ( ( b & 1 ) == 0 )
			{
				long result = 2 * Mul( a, b / 2, mod );
				if ( result >= mod ) result -= mod;
				return result;
			}
			else
			{
				long result = Mul( a - 1, b, mod ) + b % mod;
				if ( result >= mod ) result -= mod;
				if ( result < 0 ) result += mod;
				return result;
			}
		}

		/// <summary>
		/// Computer euler phi function or the number of relativly prime (with n) numbers up to n.
		/// Time complexity O(sqrt(N)).
		/// </summary>
		public static int EulerPhi( int n )
		{
			int result = n;
			for ( int i = 2; i * i <= n; ++i )
				if ( n % i == 0 )
				{
					while ( n % i == 0 ) n /= i;
					result -= result / i;
				}
			if ( n > 1 ) result -= result / n;
			return result;
		}

		/// <summary>
		/// Computer euler phi function or the number of relativly prime (with n) numbers up to n.
		/// Time complexity O(sqrt(N)).
		/// </summary>
		public static long EulerPhi( long n )
		{
			long result = n;
			for ( int i = 2; (long)i * i <= n; ++i )
				if ( n % i == 0 )
				{
					while ( n % i == 0 ) n /= i;
					result -= result / i;
				}
			if ( n > 1 ) result -= result / n;
			return result;
		}

		/// <summary>
		/// Returns array [n+1,n+1] of binomials (using modulo)
		/// Complexity is O(n^2)
		/// </summary>
		public static int[,] BinomialArray( int n, int mod )
		{
			int[,] bin = new int[n + 1, n + 1];
			for ( int i = 0; i <= n; ++i )
			{
				bin[i, 0] = bin[i, i] = 1;
				for ( int j = 1; j < i; ++j )
				{
					bin[i, j] = bin[i - 1, j] + bin[i - 1, j - 1];
					if ( bin[i, j] >= mod )
						bin[i, j] -= mod;
				}
			}
			return bin;
		}

		/// <summary>
		/// Returns array [n+1,n+1] of binomials (using modulo)
		/// Complexity is O(n^2)
		/// </summary>
		public static long[,] BinomialArray( int n, long mod )
		{
			long[,] bin = new long[n + 1, n + 1];
			for ( int i = 0; i <= n; ++i )
			{
				bin[i, 0] = bin[i, i] = 1;
				for ( int j = 1; j < i; ++j )
				{
					bin[i, j] = bin[i - 1, j] + bin[i - 1, j - 1];
					if ( bin[i, j] >= mod )
						bin[i, j] -= mod;
				}
			}
			return bin;
		}

		/// <summary>
		/// Returns array [0..n] of Catalan numbers modulo prime
		/// </summary>
		/// <returns></returns>
		public static long[] CatalanModuloPrimeArray( int n, long p )
		{
			var res = new long[n + 1];
			res[0] = 1;
			for ( int i = 1; i <= n; ++i )
			{
				res[i] = Mul( Mul( res[i - 1], 4 * i - 2, p ), Inverse( i + 1, p ), p );
			}
			return res;
		}

		/// <summary>
		/// Gets array [n+1] of factorials, using modulo
		/// </summary>
		public static int[] FactorialArray( int n, int mod )
		{
			int[] f = new int[n + 1];
			f[0] = 1;
			for ( int i = 1; i <= n; ++i ) f[i] = Mul( i, f[i - 1], mod );
			return f;
		}

		/// <summary>
		/// Gets array [n+1] of factorials, using modulo
		/// </summary>
		public static long[] FactorialArray( int n, long mod )
		{
			long[] f = new long[n + 1];
			f[0] = 1;
			for ( int i = 1; i <= n; ++i ) f[i] = Mul( i, f[i - 1], mod );
			return f;
		}
	}
}
