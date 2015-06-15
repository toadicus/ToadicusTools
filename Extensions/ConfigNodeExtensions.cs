// ToadicusTools
//
// ConfigNodeExtensions.cs
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

namespace ToadicusTools.Extensions
{
	public static class ConfigNodeExtensions
	{
		/// <summary>
		/// Gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="string"/>,
		/// or returns a given default value if the key does not exist or cannot be parsed.
		/// </summary>
		/// <returns>The value as a <see cref="string"/></returns>
		/// <param name="node">The <see cref="ConfigNode"/> being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		public static string GetValue(this ConfigNode node, string name, string defaultValue)
		{
			string value;

			if (node.TryGetValue(name, out value))
			{
				return value;
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="double"/>,
		/// or returns a given default value if the key does not exist or cannot be parsed.
		/// </summary>
		/// <returns>The value as a <see cref="double"/></returns>
		/// <param name="node">The <see cref="ConfigNode"/> being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		public static double GetValue(this ConfigNode node, string name, double defaultValue)
		{
			double value;

			if (node.TryGetValue(name, out value))
			{
				return value;
			}

			return defaultValue;
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="double"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The <see cref="ConfigNode"/>  being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		public static bool TryGetValue(this ConfigNode node, string name, out double value)
		{
			string result;

			if (node.TryGetValue(name, out result))
			{
				return double.TryParse(result, out value);
			}

			value = default(double);
			return false;
		}

		/// <summary>
		/// Gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="float"/>,
		/// or returns a given default value if the key does not exist or cannot be parsed.
		/// </summary>
		/// <returns>The value as a float</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		public static float GetValue(this ConfigNode node, string name, float defaultValue)
		{
			float value;

			if (node.TryGetValue(name, out value))
			{
				return value;
			}

			return defaultValue;
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="float"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		public static bool TryGetValue(this ConfigNode node, string name, out float value)
		{
			string result;

			if (node.TryGetValue(name, out result))
			{
				return float.TryParse(result, out value);
			}

			value = default(float);
			return false;
		}

		/// <summary>
		/// Gets the value of key "name" in ConfigNode "node" as an int, or returns a given default value if the key
		/// does not exist or cannot be parsed to a double.
		/// </summary>
		/// <returns>The value as an int</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		public static int GetValue(this ConfigNode node, string name, int defaultValue)
		{
			int value;

			if (node.TryGetValue(name, out value))
			{
				return value;
			}

			return defaultValue;
		}

		/// <summary>
		/// Gets the value of key "name" in ConfigNode "node" as a bool, or returns a given default value if the key
		/// does not exist or cannot be parsed to a bool.
		/// </summary>
		/// <returns>The value as a bool</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		public static bool GetValue(this ConfigNode node, string name, bool defaultValue)
		{
			bool value;

			if (node.TryGetValue(name, out value))
			{
				return value;
			}

			return defaultValue;
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as an <see cref="int"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		public static bool TryGetValue(this ConfigNode node, string name, out int value)
		{
			string result;

			if (node.TryGetValue(name, out result))
			{
				return int.TryParse(result, out value);
			}

			value = default(int);
			return false;
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="bool"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		public static bool TryGetValue(this ConfigNode node, string name, out bool value)
		{
			string result;

			if (node.TryGetValue(name, out result))
			{
				return bool.TryParse(result, out value);
			}

			value = default(bool);
			return false;
		}

		public static bool TryGetValue(this ConfigNode node, string name, out string value)
		{
			if (node.HasValue(name))
			{
				value = node.GetValue(name);
				return true;
			}

			value = string.Empty;
			return false;
		}

		public static void SafeSetValue(this ConfigNode node, string name, string value)
		{
			if (node.HasValue(name))
			{
				node.SetValue(name, value);
			}
			else
			{
				node.AddValue(name, value);
			}
		}

		public static void SafeSetValue(this ConfigNode node, string name, bool value)
		{
			string saveValue = value ? bool.TrueString : bool.FalseString;
			if (node.HasValue(name))
			{
				node.SetValue(name, saveValue);
			}
			else
			{
				node.AddValue(name, saveValue);
			}
		}
	}
}
