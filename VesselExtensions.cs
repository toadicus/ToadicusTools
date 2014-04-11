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
		public static List<T> getModulesOfType<T>(this Vessel vessel) where T : PartModule
		{
			#if MODULE_DB_AVAILABLE
			return ModuleDB<T>.Instance.getModules(vessel);
			#else
			throw new NotImplementedException("Vessel.getModulesOfType<T> is not implemented without ModuleDB.");
			#endif
		}
	}
}
