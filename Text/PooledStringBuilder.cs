// ToadicusTools
//
// PooledStringBuilder.cs
//
// Copyright © 2015, toadicus
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation and/or other
//    materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ToadicusTools.Text
{
	public class PooledStringBuilder : PooledObject<PooledStringBuilder>, IDisposable
	{
		public static explicit operator PooledStringBuilder(StringBuilder sb)
		{
			PooledStringBuilder psb = Get();

			psb.sb = sb;

			return psb;
		}

		// Returns the PooledStringBuilder's StringBuilder object.
		// The PooledStringBuilder allocates a new StringBuilder and is pushed back to the stack.
		public static explicit operator StringBuilder(PooledStringBuilder psb)
		{
			StringBuilder sb = psb.sb;

			psb.sb = new StringBuilder();

			Put(psb);

			return sb;
		}

		protected StringBuilder sb;

		public int Capacity
		{
			get
			{
				return sb.Capacity;
			}
			set
			{
				sb.Capacity = value;
			}
		}

		public int Length
		{
			get
			{
				return sb.Length;
			}
			set
			{
				sb.Length = value;
			}
		}

		public int MaxCapacity
		{
			get
			{
				return sb.MaxCapacity;
			}
		}

		//
		// Indexer
		//
		public char this [int index]
		{
			get
			{
				return sb[index];
			}
			set
			{
				sb[index] = value;
			}
		}

		[Obsolete("Use PooledStringBuilder.Get instead")]
		public PooledStringBuilder()
		{
			sb = new StringBuilder();
		}

		public PooledStringBuilder Append(sbyte value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(int value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(long value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(object value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(float value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(string value, int startIndex, int count)
		{
			this.sb.Append(value, startIndex, count);
			return this;
		}

		public PooledStringBuilder Append(ulong value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(char value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(char value, int repeatCount)
		{
			this.sb.Append(value, repeatCount);
			return this;
		}

		public PooledStringBuilder Append(char[] value, int startIndex, int charCount)
		{
			this.sb.Append(value, startIndex, charCount);
			return this;
		}

		public PooledStringBuilder Append(short value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(uint value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(double value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(ushort value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(byte value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(bool value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(string value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(char[] value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder Append(decimal value)
		{
			this.sb.Append(value);
			return this;
		}

		public PooledStringBuilder AppendFormat(string format, object arg0, object arg1)
		{
			this.sb.AppendFormat(SIFormatProvider.SIFormatter, format, arg0, arg1);
			return this;
		}

		public PooledStringBuilder AppendFormat(string format, object arg0)
		{
			this.sb.AppendFormat(SIFormatProvider.SIFormatter, format, arg0);
			return this;
		}

		public PooledStringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
		{
			this.sb.AppendFormat(provider, format, args);
			return this;
		}

		public PooledStringBuilder AppendFormat(string format, params object[] args)
		{
			this.sb.AppendFormat(SIFormatProvider.SIFormatter, format, args);
			return this;
		}

		public PooledStringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
		{
			this.sb.AppendFormat(SIFormatProvider.SIFormatter, format, arg0, arg1, arg2);
			return this;
		}

		[ComVisible(false)]
		public PooledStringBuilder AppendLine(string value)
		{
			this.sb.Append(value);
			return this;
		}

		[ComVisible(false)]
		public PooledStringBuilder AppendLine()
		{
			this.sb.AppendLine();
			return this;
		}

		public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
		{
			this.sb.CopyTo(sourceIndex, destination, destinationIndex, count);
		}

		public int EnsureCapacity(int capacity)
		{
			return this.sb.EnsureCapacity(capacity);
		}

		public bool Equals(StringBuilder sb)
		{
			return this.sb.Equals(sb);
		}

		public bool Equals(PooledStringBuilder psb)
		{
			return this.sb.Equals(psb.sb);
		}

		public PooledStringBuilder Insert(int index, short value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, char[] value, int startIndex, int charCount)
		{
			this.sb.Insert(index, value, startIndex, charCount);
			return this;
		}

		public PooledStringBuilder Insert(int index, string value, int count)
		{
			this.sb.Insert(index, value, count);
			return this;
		}

		public PooledStringBuilder Insert(int index, ulong value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, uint value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, ushort value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, float value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, sbyte value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, object value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, long value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, int value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, char value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, decimal value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, byte value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, bool value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, string value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, char[] value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Insert(int index, double value)
		{
			this.sb.Insert(index, value);
			return this;
		}

		public PooledStringBuilder Print()
		{
			this.sb.Print();
			return this;
		}

		public PooledStringBuilder Remove(int startIndex, int length)
		{
			this.sb.Remove(startIndex, length);
			return this;
		}

		public PooledStringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
		{
			this.sb.Replace(oldValue, newValue, startIndex, count);
			return this;
		}

		public PooledStringBuilder Replace(string oldValue, string newValue)
		{
			this.sb.Replace(oldValue, newValue);
			return this;
		}

		public PooledStringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
		{
			this.sb.Replace(oldChar, newChar, startIndex, count);
			return this;
		}

		public PooledStringBuilder Replace(char oldChar, char newChar)
		{
			this.sb.Replace(oldChar, newChar);
			return this;
		}

		public override string ToString()
		{
			return this.sb.ToString();
		}

		public string ToString(int startIndex, int length)
		{
			return this.sb.ToString(startIndex, length);
		}

		public void Dispose()
		{
			Put(this);
		}

		protected override void onGet()
		{
			this.sb.Length = 0;
		}
	}
}
