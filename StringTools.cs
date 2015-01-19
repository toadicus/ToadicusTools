// ToadicusTools
//
// StringTools.cs
//
// Copyright © 2014, toadicus
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

using KSP;
using System;
using UnityEngine;

namespace ToadicusTools
{
	public static partial class Tools
	{
		public static readonly SIFormatProvider SIFormatter = new SIFormatProvider();

		/// <summary>
		/// <para>Replaces the format items in a specified string with the string representation of corresponding objects in a
		/// specified array.</para>
		/// <para>&#160;</para>
		/// <para>Uses the custom SIFormatter format provider, to facilitate SI formats for double and double-like numbers, as
		/// MuMech_ToSI.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		public static string Format(string format, params object[] args)
		{
			return string.Format(SIFormatter, format, args);
		}
	}

	/// <summary>
	/// <para>Facilitates a new "SI prefixed" string format for doubles and double-like numbers, "S[x[,y[,z]]]" where:</para>
	/// <list type="bullet">
	/// <item><description>x:  Number of digits to display after the decimal point,</description></item>
	/// <item><description>y:  Minimum magnitude to be used for an SI prefix, e.g. -3 for "milli", and</description></item>
	/// <item><description>z:  Maximum magnitude to be used for an SI prefix, e.g. 9 for "giga".</description></item>
	/// </list>
	/// <para>When used thus:</para>
	/// <example>string.Format("{0:x,y,z}", d"),</example>
	/// <para>the formatter will invoke Tools.MuMech_ToSI(d, x, y, z).</para>
	/// </summary>
	public class SIFormatProvider : IFormatProvider, ICustomFormatter
	{
		public object GetFormat(Type type)
		{
			if (type == typeof(ICustomFormatter))
			{
				return this;
			}
			else
			{
				return null;
			}
		}

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (!this.Equals(formatProvider))
			{
				return null;
			}

			switch (format[0])
			{
				case 'S':
				case 's':
					string[] args = format.Substring(1).Split(new char[] {','}, 3);

					double d = Convert.ToDouble(arg);

					int digits = 3;
					int MinMagnitude = 0;
					int MaxMagnitude = int.MaxValue;

					if (args.Length > 0)
					{
						digits = int.Parse(args[0]);
					}
					if (args.Length > 1)
					{
						MinMagnitude = int.Parse(args[1]);
					}
					if (args.Length > 2)
					{
						MaxMagnitude = int.Parse(args[2]);
					}

					return Tools.MuMech_ToSI(d, digits, MinMagnitude, MaxMagnitude);
				default:
					if (arg is IFormattable)
					{
						return ((IFormattable)arg).ToString(format, System.Globalization.CultureInfo.CurrentCulture);
					}
					else if (arg != null)
					{
						return arg.ToString();
					}
					else
					{
						return string.Empty;
					}
			}
		}
	}

}