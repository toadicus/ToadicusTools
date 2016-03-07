// ToadicusTools
//
// StringTools.cs
//
// Copyright © 2014-2015, toadicus
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
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ToadicusTools;

namespace ToadicusTools.Text
{
	public static class Extensions
	{
		public static string ToMD5Hash(this string input, int outLength = 32)
		{
			var alg = System.Security.Cryptography.MD5.Create();
			var hash = alg.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			int byteLength = outLength / 2;

			for (int idx = 0; idx < byteLength; idx++)
			{
				var b = hash[idx];
				sb.Append(b.ToString("x2"));
			}

			return sb.ToString();
		}

		public static StringBuilder Print(this StringBuilder sb)
		{
			Logging.PostLogMessage(sb.ToString());

			return sb;
		}

		public static StringBuilder AddIntendedLine(this StringBuilder sb, string line, int indent = 0)
		{
			if (indent > 0) {
				sb.Append(' ', indent * 4);
			}

			sb.AppendLine(line);

			return sb;
		}

		public static string SPrint<T>(this T[] array, string delimiter, Func<T, string> stringFunc)
		{
			using (PooledStringBuilder sb = PooledStringBuilder.Get())
			{
				T item;
				string s;

				for (int idx = 0; idx < array.Length; idx++)
				{
					if (idx > 0)
					{
						sb.Append(delimiter);
					}

					item = array[idx];

					sb.Append(item == null ? "null" : stringFunc == null ? item.ToString() : stringFunc(item));
				}

				s = sb.ToString();

				return s;
			}
		}

		public static string SPrint<T>(this T[] array, Func<T, string> stringFunc, string delimiter = ", ")
		{
			return SPrint(array, delimiter, stringFunc);
		}

		public static string SPrint<T>(this T[] array, string delimiter = ", ")
		{
			return array.SPrint(delimiter, null);
		}

		public static string SPrint<T>(this IList<T> list, string delimiter, Func<T, string> stringFunc)
		{
			using (PooledStringBuilder sb = PooledStringBuilder.Get())
			{
				T item;
				string s;

				for (int idx = 0; idx < list.Count; idx++)
				{
					if (idx > 0)
					{
						sb.Append(delimiter);
					}

					item = list[idx];

					sb.Append(item == null ? "null" : stringFunc == null ? item.ToString() : stringFunc(item));
				}

				s = sb.ToString();

				return s;
			}
		}

		public static string SPrint<T>(this List<T> list, string delimiter, Func<T, string> stringFunc)
		{
			return SPrint<T>(list as IList<T>, delimiter, stringFunc);
		}

		public static string SPrint<T>(this List<T> list, Func<T, string> stringFunc, string delimiter = ", ")
		{
			return SPrint(list, delimiter, stringFunc);
		}

		public static string SPrint<T>(this IList<T> list, Func<T, string> stringFunc, string delimiter = ", ")
		{
			return SPrint(list, delimiter, stringFunc);
		}

		public static string SPrint<T>(this List<T> list, string delimiter = ", ")
		{
			return list.SPrint(delimiter, null);
		}

		public static string SPrint<T>(this IList<T> list, string delimiter = ", ")
		{
			return list.SPrint(delimiter, null);
		}

		public static string ToString(this Vector3 vector, string format)
		{
			return string.Format("{0}, {1}, {2}",
				vector.x.ToString(format, SIFormatProvider.SIFormatter),
				vector.y.ToString(format, SIFormatProvider.SIFormatter),
				vector.z.ToString(format, SIFormatProvider.SIFormatter)
			);
		}
	}
}