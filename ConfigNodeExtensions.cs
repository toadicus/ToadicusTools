// ToadicusTools
//
// ConfigNodeExtensions.cs
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

namespace ToadicusTools
{
	public static partial class Tools
	{
		/// <summary>
		/// Gets the value of key "name" in ConfigNode "node" as a double, or returns a given default value if the key
		/// does not exist or cannot be parsed to a double.
		/// </summary>
		/// <returns>The value as a double</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		public static double GetValue(this ConfigNode node, string name, double defaultValue)
		{
			if (node.HasValue(name))
			{
				double result;
				if (double.TryParse(node.GetValue(name), out result))
				{
					return result;
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets the value of key "name" in ConfigNode "node" as a float, or returns a given default value if the key
		/// does not exist or cannot be parsed to a double.
		/// </summary>
		/// <returns>The value as a float</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		public static float GetValue(this ConfigNode node, string name, float defaultValue)
		{
			if (node.HasValue(name))
			{
				float result;
				if (float.TryParse(node.GetValue(name), out result))
				{
					return result;
				}
			}
			return defaultValue;
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
			if (node.HasValue(name))
			{
				int result;
				if (int.TryParse(node.GetValue(name), out 	result))
				{
					return result;
				}
			}
			return defaultValue;
		}
	}
}
