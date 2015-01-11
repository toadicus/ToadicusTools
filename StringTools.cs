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
		public static readonly IFormatProvider mySIFormatter = new SIFormatter() as IFormatProvider;

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
			return string.Format(mySIFormatter, format, args);
		}
	}

	public class SIFormatter : IFormatProvider, ICustomFormatter
	{
		private readonly static int[] defaultArgs;

		static SIFormatter()
		{
			defaultArgs = new int[3];

			var SIMethod = typeof(Tools).GetMethod("MuMech_ToSI");

			var parameters = SIMethod.GetParameters();

			int loopLimit = Math.Min(parameters.Length, defaultArgs.Length + 1);

			for (int i = 1; i < loopLimit; i++)
			{
				defaultArgs[i - 1] = (int)parameters[i].DefaultValue;
			}
		}

		public object GetFormat(Type service)
		{
			if (service == typeof(ICustomFormatter))
			{
				return this;
			}
			else
			{
				return null;
			}
		}

		/*
		 * Facilitates a new "SI prefixed" string format for doubles and double-like numbers, "S[x[,y[,z]]]" where:
		 * 
		 *  x:  Number of digits to display after the decimal point,
		 *  y:  Minimum magnitude to be used for an SI prefix, e.g. -3 for "milli", and
		 *  z:  Maximum magnitude to be used for an SI prefix, e.g. 9 for "giga".
		 * 
		 * When used thus:
		 * 
		 *  string.Format("{0:x,y,z}", d"),
		 * 
		 * the formatter will invoke Tools.MuMech_ToSI(d, x, y, z).
		 * */
		public string Format(string format, object arg, IFormatProvider provider)
		{
			if (format == null)
			{
				return string.Format("{0}", arg);
			}

			if (!(arg is IConvertible) ||
				!format.StartsWith("S", true, System.Globalization.CultureInfo.CurrentCulture))
			{
				if (arg is IFormattable)
				{
					return ((IFormattable)arg).ToString(format, provider);
				}
				else if (arg != null)
				{
					return arg.ToString();
				}
			}

			string[] formatArgs = format.Trim('S', 's').Split(',');
			int[] SIArgs = new int[defaultArgs.Length];

			Array.Copy(defaultArgs, SIArgs, defaultArgs.Length);

			for (int i = 0; i<formatArgs.Length; i++)
			{
				int SIarg;

				if (!Int32.TryParse(formatArgs[i], out SIarg))
				{
					Debug.LogError(
						string.Format("[SIFormatter] Ignoring invalid format argument at index {0}: '{1}'.",
							i,
							formatArgs[i]
						));
				}

				SIArgs[i] = SIarg;
			}

			double dArg = ((IConvertible)arg).ToDouble(System.Globalization.CultureInfo.CurrentCulture);

			return Tools.MuMech_ToSI(dArg, SIArgs[0], SIArgs[1], SIArgs[2]);
		}
	}
}