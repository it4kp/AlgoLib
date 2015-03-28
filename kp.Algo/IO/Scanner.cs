using System;

namespace kp.Algo.IO
{
	public class Scanner : IDisposable
	{
		#region Fields

		readonly System.IO.TextReader _reader;
		readonly int _bufferSize;
		readonly bool _closeReader;
		readonly char[] _buffer;
		int _length, _pos;

		#endregion

		#region .ctors

		public Scanner( System.IO.TextReader reader, int bufferSize, bool closeReader )
		{
			_reader = reader;
			_bufferSize = bufferSize;
			_closeReader = closeReader;
			_buffer = new char[_bufferSize];
			FillBuffer( false );
		}

		public Scanner( System.IO.TextReader reader, bool closeReader ) : this( reader, 1 << 16, closeReader ) { }

		public Scanner( string fileName ) : this( new System.IO.StreamReader( fileName, System.Text.Encoding.Default ), true ) { }

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if ( _closeReader )
			{
				_reader.Close();
			}
		}

		#endregion

		#region Properties

		public bool Eof
		{
			get
			{
				if ( _pos < _length ) return false;
				FillBuffer( false );
				return _pos >= _length;
			}
		}

		#endregion

		#region Methods

		private char NextChar()
		{
			if ( _pos < _length ) return _buffer[_pos++];
			FillBuffer( true );
			return _buffer[_pos++];
		}

		private char PeekNextChar()
		{
			if ( _pos < _length ) return _buffer[_pos];
			FillBuffer( true );
			return _buffer[_pos];
		}

		private void FillBuffer( bool throwOnEof )
		{
			_length = _reader.Read( _buffer, 0, _bufferSize );
			if ( throwOnEof && Eof )
			{
				throw new System.IO.IOException( "Can't read beyond the end of file" );
			}
			_pos = 0;
		}

		public int NextInt()
		{
			var neg = false;
			int res = 0;
			SkipWhitespaces();
			if ( !Eof && PeekNextChar() == '-' )
			{
				neg = true;
				_pos++;
			}
			while ( !Eof && !IsWhitespace( PeekNextChar() ) )
			{
				var c = NextChar();
				if ( c < '0' || c > '9' ) throw new ArgumentException( "Illegal character" );
				res = 10 * res + c - '0';
			}
			return neg ? -res : res;
		}

		public int[] NextIntArray( int n )
		{
			var res = new int[n];
			for ( int i = 0; i < n; i++ )
			{
				res[i] = NextInt();
			}
			return res;
		}

		public long NextLong()
		{
			var neg = false;
			long res = 0;
			SkipWhitespaces();
			if ( !Eof && PeekNextChar() == '-' )
			{
				neg = true;
				_pos++;
			}
			while ( !Eof && !IsWhitespace( PeekNextChar() ) )
			{
				var c = NextChar();
				if ( c < '0' || c > '9' ) throw new ArgumentException( "Illegal character" );
				res = 10 * res + c - '0';
			}
			return neg ? -res : res;
		}

		public long[] NextLongArray( int n )
		{
			var res = new long[n];
			for ( int i = 0; i < n; i++ )
			{
				res[i] = NextLong();
			}
			return res;
		}

		public string NextLine()
		{
			SkipUntilNextLine();
			if ( Eof ) return "";
			var builder = new System.Text.StringBuilder();
			while ( !Eof && !IsEndOfLine( PeekNextChar() ) )
			{
				builder.Append( NextChar() );
			}
			return builder.ToString();
		}

		public double NextDouble()
		{
			SkipWhitespaces();
			var builder = new System.Text.StringBuilder();
			while ( !Eof && !IsWhitespace( PeekNextChar() ) )
			{
				builder.Append( NextChar() );
			}
			return double.Parse( builder.ToString(), System.Globalization.CultureInfo.InvariantCulture );
		}

		public double[] NextDoubleArray( int n )
		{
			var res = new double[n];
			for ( int i = 0; i < n; i++ )
			{
				res[i] = NextDouble();
			}
			return res;
		}

		public string NextToken()
		{
			SkipWhitespaces();
			var builder = new System.Text.StringBuilder();
			while ( !Eof && !IsWhitespace( PeekNextChar() ) )
			{
				builder.Append( NextChar() );
			}
			return builder.ToString();
		}

		private void SkipWhitespaces()
		{
			while ( !Eof && IsWhitespace( PeekNextChar() ) )
			{
				++_pos;
			}
		}

		private void SkipUntilNextLine()
		{
			while ( !Eof && IsEndOfLine( PeekNextChar() ) )
			{
				++_pos;
			}
		}

		private static bool IsWhitespace( char c )
		{
			return c == ' ' || c == '\t' || c == '\n' || c == '\r';
		}

		private static bool IsEndOfLine( char c )
		{
			return c == '\n' || c == '\r';
		}

		#endregion
	}
}
