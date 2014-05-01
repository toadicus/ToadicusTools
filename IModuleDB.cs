// ToadicusTools
//
// IModuleDB.cs
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
	/// <summary>
	/// Interface for a generic, caching database of PartModule derivatives keyed by Vessel and Part.
	/// </summary>
	public interface IModuleDB<T>
		where T : PartModule
	{
		/// <summary>
		/// Gets a flat list of all modules of type T in the given Vessel.  Returns an empty list if none exist.
		/// </summary>
		/// <returns>The list modules of type T</returns>
		/// <param name="vessel">The Vessel being queried</param>
		List<T> getModules(Vessel vessel);

		/// <summary>
		/// Gets a flat list of all modules of type T in the given Part.  Returns an empty list if none exist.
		/// </summary>
		/// <returns>The list of modules of type T</returns>
		/// <param name="part">The Part being queried</param>
		List<T> getModules(Part part);

		/// <summary>
		/// Returns true if the given Vessel exists in the deep cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Vessel exists in the deep cache, false otherwise</returns>
		/// <param name="vessel">The Vessel being queried</param>
		bool inDeepCache(Vessel vessel);

		/// <summary>
		/// Returns true if the given Part exists in the deep cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Part exists in the deep cache, false otherwise</returns>
		/// <param name="part">The Part being queried</param>
		bool inDeepCache(Part part);

		/// <summary>
		/// Returns true if the given Vessel exists in the shallow cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Vessel exists in the shallow cache, false otherwise</returns>
		/// <param name="vessel">The Vessel being queried</param>
		bool inShallowCache(Vessel vessel);
	}

	/// <summary>
	/// Interface for a caching database of ModuleInfo objects keyed by part name and module name.
	/// </summary>
	public interface IPrefabPartDB
	{
		/// <summary>
		/// Gets a moduleName-keyed table of ModuleInfo objects in the named part prefab.
		/// </summary>
		/// <returns>a moduleName-keyed table of ModuleInfo objects</returns>
		/// <param name="partName">the part name</param>
		Dictionary<string, AvailablePart.ModuleInfo> getPrefabModuleDB(string partName);
	}
}
