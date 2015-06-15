// ToadicusTools
//
// Math.cs
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

namespace ToadicusTools
{
	public static class MathTools
	{
		/// <summary>
		/// Returns the minimum from among the given values
		/// </summary>
		/// <param name="values">Values</param>
		/// <typeparam name="T">Any type implementing IComparable<T></typeparam>
		public static T Min<T>(params T[] values) where T : IComparable<T>
		{
			if (values.Length < 2)
			{
				throw new ArgumentException("Min must be called with at least two arguments.");
			}

			IComparable<T> minValue = values[0];

			for (long i = 1; i < values.LongLength; i++)
			{
				IComparable<T> value = values[i];

				if (value.CompareTo((T)minValue) < 0)
				{
					minValue = value;
				}
			}

			return (T)minValue;
		}

		/// <summary>
		/// <para>Exponentiation function optimized for arbitrary integer exponents.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		public static double Pow(double b, int n)
		{
			switch (n)
			{
				case -4:
					return 1d / (b * b * b * b);
				case -3:
					return 1d / (b * b * b);
				case -2:
					return 1d / (b * b);
				case -1:
					return 1d / b;
				case 0:
					return 1d;
				case 1:
					return b;
				case 2:
					return b * b;
				case 3:
					return b * b * b;
				case 4:
					return b * b * b * b;
				default:
					double r = 1d;

					if (n < 0)
					{
						n = -n;

						while (n != 0)
						{
							if ((n & 1) != 0)
							{
								r *= b;
							}

							n >>= 1;

							b *= b;
						}

						return 1d / r;
					}
					else
					{
						while (n != 0)
						{
							if ((n & 1) != 0)
							{
								r *= b;
							}

							n >>= 1;

							b *= b;
						}

						return r;
					}
			}
		}

		/// <summary>
		/// <para>Exponentiation function which uses an integer-optimized algorithm when possible,
		/// or falls back to <see cref="Math.Pow"/>.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		public static double Pow(double b, double n)
		{
			int x = (int)n;

			if (n == x)
			{
				return Pow(b, x);
			}
			else
			{
				return Math.Pow(b, n);
			}
		}

		/// <summary>
		/// <para>Exponentiation function optimized for arbitrary integer exponents
		/// using single-precision floats.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		public static float Pow(float b, int n)
		{
			switch (n)
			{
				case -4:
					return 1f / (b * b * b * b);
				case -3:
					return 1f / (b * b * b);
				case -2:
					return 1f / (b * b);
				case -1:
					return 1f / b;
				case 0:
					return 1f;
				case 1:
					return b;
				case 2:
					return b * b;
				case 3:
					return b * b * b;
				case 4:
					return b * b * b * b;
				default:
					float r = 1f;

					if (n < 0)
					{
						n = -n;

						while (n != 0)
						{
							if ((n & 1) != 0)
							{
								r *= b;
							}

							n >>= 1;

							b *= b;
						}

						return 1f / r;
					}
					else
					{
						while (n != 0)
						{
							if ((n & 1) != 0)
							{
								r *= b;
							}

							n >>= 1;

							b *= b;
						}

						return r;
					}
			}
		}

		/// <summary>
		/// <para>Exponentiation function which uses an integer-optimized algorithm when possible,
		/// or falls back to <see cref="Math.Pow"/>.  Uses single-precision floats in the integer case.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		public static float Pow(float b, float n)
		{
			int x = (int)n;

			if (n == x)
			{
				return Pow(b, x);
			}
			else
			{
				return (float)Math.Pow(b, n);
			}
		}
	}
}

