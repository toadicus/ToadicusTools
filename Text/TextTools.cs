// ToadicusTools
//
// Tools.cs
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

namespace ToadicusTools.Text
{
	public static class TextTools
	{
		/// <summary>
		/// <para>Replaces the format items in a specified string with the string representation of corresponding objects in a
		/// specified array.</para>
		/// <para>&#160;</para>
		/// <para>Uses the custom SIFormatter format provider, to facilitate SI formats for double and double-like numbers, as
		/// MuMech_ToSI.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		public static string Format(IFormatProvider provider, string format, params object[] args)
		{
			return string.Format(provider, format, args);
		}

		public static string Format(string format, params object[] args)
		{
			return string.Format(SIFormatProvider.SIFormatter, format, args);
		}

		public static string ToString(IFormattable arg, string format, IFormatProvider provider)
		{
			return arg.ToString(format, provider);
		}

		public static string ToString(IFormattable arg, string format)
		{
			return arg.ToString(format, SIFormatProvider.SIFormatter);
		}
	}
}
