// ToadicusTools
//
// ArrayExtensions.cs
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

namespace ToadicusTools.Extensions
{
	public static class CollectionExtensions
	{
		public static bool Contains(this GameScenes[] haystack, GameScenes needle)
		{
			GameScenes item;
			for (int idx = 0; idx < haystack.Length; idx++)
			{
				item = haystack[idx];
				if (item == needle)
				{
					return true;
				}
			}

			return false;
		}

		public static bool Contains(this CelestialBody[] haystack, CelestialBody needle)
		{
			CelestialBody item;
			for (int idx = 0; idx < haystack.Length; idx++)
			{
				item = haystack[idx];
				if (item == needle)
				{
					return true;
				}
			}
			return false;
		}

		public static bool Contains<T>(this T[] haystack, T needle)
		{
			T item;
			for (int idx = 0; idx < haystack.Length; idx++)
			{
				item = haystack[idx];
				if (object.Equals(item, needle))
				{
					return true;
				}
			}
			return false;
		}
	}
}

