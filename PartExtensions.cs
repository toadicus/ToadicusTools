// ToadicusTools © 2014 toadicus
//
// This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. To view a
// copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/

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

			foreach (PartModule module in part.Modules)
			{
				if (module is T)
				{
					return true;
				}
			}

			return false;
		}

		public static IEnumerable<T> getModulesOfType<T>(this Part part) where T : PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.getModulesOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
			}

			List<T> returnList = new List<T>();

			foreach (PartModule module in part.Modules)
			{
				if (module is T)
				{
					returnList.Add(module as T);
				}
			}

			return returnList as IEnumerable<T>;
		}

		public static T getFirstModuleOfType<T>(this Part part) where T: PartModule
		{
			if (part == null)
			{
				throw new ArgumentNullException(
					string.Format("Part.getFirstModuleOfType<{0}>: 'part' argument must not be null", typeof(T).Name)
				);
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

		public static bool isDockingNode(this Part part)
		{
			return hasModuleType<ModuleDockingNode>(part);
		}
	}
}