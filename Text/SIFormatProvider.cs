// ToadicusTools
//
// SIFormatProvider.cs
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
		public static SIFormatProvider SIFormatter;

		static SIFormatProvider()
		{
			SIFormatter = new SIFormatProvider();
		}

		public static string ToSI(double value, int sigFigs)
		{
			if (value == 0)
			{
				return "0.0";
			}

			if (double.IsNaN(value))
			{
				return "NaN";
			}

			if (double.IsInfinity(value))
			{
				if (double.IsNegativeInfinity(value))
				{
					return "-∞";
				}
				else
				{
					return "∞";
				}
			}

			string format;

			double absValue = Math.Abs(value);

			int magnitude = (int)Math.Log10(absValue);
			int significance;
			int divisorExp;
			int decimalPlaces;

			string prefix = string.Empty;

			if (magnitude < 0 || absValue < 1)
			{
				decimalPlaces = 1;

				significance = 1;
			}
			else
			{
				decimalPlaces = 0;

				significance = (sigFigs / 3) * 3;
			}

			if (Math.Abs(magnitude) >= significance)
			{
				divisorExp = magnitude - significance;
			}
			else
			{
				divisorExp = 0;
			}

			switch (divisorExp)
			{
				case 0:
					break;
				case 1:
				case 2:
				case 3:
					value /= 1e3;
					magnitude -= 3;
					prefix = "k";
					break;
				case 4:
				case 5:
				case 6:
					value /= 1e6;
					magnitude -= 6;
					prefix = "M";
					break;
				case 7:
				case 8:
				case 9:
					value /= 1e9;
					magnitude -= 9;
					prefix = "G";
					break;
				case 10:
				case 11:
				case 12:
					value /= 1e12;
					magnitude -= 12;
					prefix = "T";
					break;
				case 13:
				case 14:
				case 15:
					value /= 1e15;
					magnitude -= 15;
					prefix = "P";
					break;
				case 16:
				case 17:
				case 18:
					value /= 1e18;
					magnitude -= 18;
					prefix = "E";
					break;
				case 19:
				case 20:
				case 21:
					value /= 1e21;
					magnitude -= 21;
					prefix = "Z";
					break;
				case 22:
				case 23:
				case 24:
					value /= 1e24;
					magnitude -= 24;
					prefix = "Y";
					break;
				case -1:
				case -2:
				case -3:
					value *= 1e3;
					magnitude += 3;
					prefix = "m";
					break;
				case -4:
				case -5:
				case -6:
					value *= 1e6;
					magnitude += 6;
					prefix = "µ";
					break;
				case -7:
				case -8:
				case -9:
					value *= 1e9;
					magnitude += 9;
					prefix = "n";
					break;
				case -10:
				case -11:
				case -12:
					value *= 1e12;
					magnitude += 12;
					prefix = "p";
					break;
				case -13:
				case -14:
				case -15:
					value *= 1e15;
					magnitude += 15;
					prefix = "f";
					break;
				case -16:
				case -17:
				case -18:
					value *= 1e18;
					magnitude += 18;
					prefix = "a";
					break;
				case -19:
				case -20:
				case -21:
					value *= 1e21;
					magnitude += 21;
					prefix = "z";
					break;
				case -22:
				case -23:
				case -24:
					value *= 1e24;
					magnitude += 24;
					prefix = "y";
					break;
				default:
					if (divisorExp > 0)
					{
						value /= 1e24;
						magnitude -= 24;
						prefix = "Y";
					}
					else
					{
						value *= 1e24;
						magnitude += 24;
						prefix = "y";
					}

					format = string.Format("{{0:g{0}}}{1}", sigFigs < 5 ? 2 : sigFigs - 3, prefix);

					return string.Format(format, value, prefix);
			}

			decimalPlaces += sigFigs - magnitude - 1;

			if (decimalPlaces < 0)
			{
				double divisor = MathTools.Pow(10d, -decimalPlaces);
				value = ((int)value / divisor) * divisor;
				decimalPlaces = 0;
			}

			format = string.Format("{{0:f{0}}}{1}", decimalPlaces, prefix);

			return string.Format(format, value, prefix);
		}

		public static string ToSI(double value)
		{
			return ToSI(value, 3);
		}

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
			if (arg == null)
			{
				return "null";
			}

			if (format == null)
			{
				return arg.ToString();
			}

			if (format.Length == 0)
			{
				throw new FormatException("Format string is empty.");
			}

			if (formatProvider == null)
			{
				throw new ArgumentNullException("SIFormatProvider.Format called with null formatProvider");
			}

			if (!this.Equals(formatProvider))
			{
				return null;
			}

			if (arg is IFormattable && arg is IConvertible)
			{
				switch (format[0])
				{
					case 'S':
					case 's':
						string[] args = format.Substring(1).Split(new char[] { ',' }, 3);

						double d = Convert.ToDouble(arg);

						int digits = 3;
						int MinMagnitude = 0;
						int MaxMagnitude = int.MaxValue;

						if (args.Length > 0)
						{
							digits = int.Parse(args[0]);
						}

						if (args.Length == 1)
						{
							return ToSI(d, digits);
						}

						if (args.Length > 1)
						{
							MinMagnitude = int.Parse(args[1]);
						}
						if (args.Length > 2)
						{
							MaxMagnitude = int.Parse(args[2]);
						}

						return MuMechTools.MuMechTools.MuMech_ToSI(d, digits, MinMagnitude, MaxMagnitude);
					default:
						return ((IFormattable)arg).ToString(format, System.Globalization.CultureInfo.CurrentCulture);
						// TODO: Remember why I previously switched from ToString to Format.
						// return string.Format(System.Globalization.CultureInfo.CurrentCulture, format, arg);
				}
			}
			else
			{
				return arg.ToString();
			}
		}
	}
}
