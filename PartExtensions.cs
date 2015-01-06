// ToadicusTools
//
// PartExtensions.cs
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
using System.Collections.Generic;
using UnityEngine;

namespace ToadicusTools
{
	public static partial class Tools
	{
		public static bool hasModuleType<T>(this Part part) where T : PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.hasModuleType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			#if DEBUG
			Debug.Log("[hasModuleType]: Falling back to linear search.");
			#endif

			if (part.Modules != null)
			{
				#if DEBUG
				Debug.Log("[hasModuleType]: Part.modules is defined; checking PartModule subtypes.");
				#endif

				foreach (PartModule module in part.Modules)
				{
					if (module is T)
					{
						return true;
					}
				}
			}
			else
			{
				#if DEBUG
				Debug.Log("[hasModuleType]: Part.modules is not defined; trying ModuleInfo search.");
				#endif

				foreach (var moduleInfo in part.partInfo.moduleInfos)
				{
					if (moduleInfo.moduleName == typeof(T).Name)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static bool hasModuleByName(this Part part, string moduleName)
		{
			if (part == null)
			{
				throw new ArgumentNullException("Part.hasModuleByName: 'part' argument must not be null");
			}

			#if DEBUG
			Debug.Log(string.Format("Checking if part {0} has module(s) named {1}", part.partInfo.name, moduleName));
			#endif

			Type moduleType = Type.GetType(moduleName);

			return (bool)typeof(Tools).GetMethod("hasModuleType").MakeGenericMethod(moduleType)
				.Invoke(null, new object[] {part});
		}

		public static List<T> getModulesOfType<T>(this Part part) where T : PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.getModulesOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			List<T> returnList = new List<T>();


			if (part.Modules != null)
			{
				foreach (PartModule module in part.Modules)
				{
					if (module is T)
					{
						returnList.Add(module as T);
					}
				}
			}
			else
			{
				part.LogWarning("Modules list is null during module search; returning empty list.");
			}

			return returnList;
		}

		public static T getFirstModuleOfType<T>(this Part part) where T: PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.getFirstModuleOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			if (part.Modules == null)
			{
				part.LogWarning("Modules list is null during module search; returning null.");

				return null;
			}

			foreach (PartModule module in part.Modules)
			{
				if (module is T)
				{
					return module as T;
				}
			}

			return null;
		}

		public static bool tryGetFirstModuleOfType<T>(this Part part, out T module) where T : PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.tryGetFirstModuleOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			module = part.getFirstModuleOfType<T>();

			if (module == null)
			{
				return false;
			}

			return true;
		}

		public static bool hasAncestorPart(this Part part, Part checkPart)
		{
			if (part == null)
			{
				Debug.LogError("Part.hasAncestorPart: 'part' argument should not be null.  Returning false.");
				return false;
			}
			if (checkPart == null)
			{
				Debug.LogError("Part.hasAncestorPart: 'checkPart' argument should not be null.  Returning false.");
				return false;
			}

			Part ancestorPart = part;

			do
			{
				if (ancestorPart == checkPart)
				{
					return true;
				}

				ancestorPart = ancestorPart.parent;
			}
			while (ancestorPart != null);

			return false;
		}

		public static bool isDecoupler(this Part part)
		{
			return part.hasModuleType<ModuleDecouple>() | part.hasModuleType<ModuleAnchoredDecoupler>();
		}

		public static bool isDockingNode(this Part part)
		{
			return hasModuleType<ModuleDockingNode>(part);
		}
	}
}