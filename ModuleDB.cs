// ToadicusTools
//
// ModuleDB.cs
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
	/// A generic, caching database of PartModule derivatives keyed by Vessel and Part.
	/// </summary>
	public class ModuleDB<T>
		where T : PartModule
	{
		private static ModuleDB<T> _instance;

		/// <summary>
		/// Gets the ModuleDB instance for the specified type
		/// </summary>
		/// <value>The ModuleDB instance for the specified type</value>
		public static ModuleDB<T> Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ModuleDB<T>();
				}

				return _instance;
			}
		}

		private Dictionary<Guid, Dictionary<uint, List<T>>> vesselPartModuleDB;

		private Dictionary<Guid, List<T>> vesselModuleTable;

		private Guid editorVesselID;

		private ModuleDB()
		{
			// Initialize the caches.
			vesselPartModuleDB = new Dictionary<Guid, Dictionary<uint, List<T>>>();
			vesselModuleTable = new Dictionary<Guid, List<T>>();

			editorVesselID = new Guid(
				new byte[] { 209, 101, 40, 249, 114, 36, 27, 95, 191, 207, 7, 197, 51, 109, 223, 212 }
			);

			// Subscribe to events to keep the cache fresh.
			GameEvents.onVesselWasModified.Add(onVesselEvent);
			GameEvents.onVesselChange.Add(onVesselEvent);
			GameEvents.onVesselDestroy.Add(onVesselEvent);
			GameEvents.onGameSceneLoadRequested.Add(onSceneChange);
			GameEvents.onPartUndock.Add(onPartEvent);
			GameEvents.onPartCouple.Add(onFromPartToPartEvent);
		}

		// Called for a subject Vessel when destroyed, changed, or modified
		private void onVesselEvent(Vessel vessel)
		{
			// If the deep cache contains the Vessel...
			if (vesselPartModuleDB.ContainsKey(vessel.id))
			{
				// ...remove it from the deep cache.
				vesselPartModuleDB.Remove(vessel.id);
			}

			// If the shallow cache contains the Vessel...
			if (vesselModuleTable.ContainsKey(vessel.id))
			{
				// ..remove it fromt he shallow cache.
				vesselModuleTable.Remove(vessel.id);
			}
		}

		// Called when a scene changed is requested
		private void onSceneChange(GameScenes scene)
		{
			// If there is an active vessel when changing scenes...
			if (FlightGlobals.ActiveVessel != null)
			{
				// ...remove it from the caches.
				onVesselEvent(FlightGlobals.ActiveVessel);
			}

			if (HighLogic.LoadedSceneIsEditor)
			{
				if (vesselPartModuleDB.ContainsKey(this.editorVesselID))
				{
					vesselPartModuleDB.Remove(this.editorVesselID);
				}

				if (vesselModuleTable.ContainsKey(this.editorVesselID))
				{
					vesselModuleTable.Remove(this.editorVesselID);
				}
			}
		}

		// Called for a subject Part when undocked
		private void onPartEvent(Part part)
		{
			// If the Part's Vessel is defined...
			if (part.vessel != null)
			{
				// ...remove it from the caches.
				onVesselEvent(part.vessel);
			}
		}

		// Called for two subject Parts when coupled
		private void onFromPartToPartEvent(GameEvents.FromToAction<Part, Part> data)
		{
			// Remove both the from and the to Parts fromt he caches.
			onPartEvent(data.from);
			onPartEvent(data.to);
		}

		/// <summary>
		/// Gets a flat list of all modules of type T in the given Vessel.  Returns an empty list if none exist.
		/// </summary>
		/// <returns>The list modules of type T</returns>
		/// <param name="vessel">The Vessel being queried</param>
		public List<T> getModules(Vessel vessel)
		{
			// If the vessel's Parts list is defined and includes any Parts...
			if (vessel.Parts != null && vessel.Parts.Count > 0)
			{
				// ...and if the vessel is not in the shallow cache...
				if (!vesselModuleTable.ContainsKey(vessel.id))
				{
					// ...create a list flat list of modules
					List<T> modulesInVessel = new List<T>();

					// ...loop through the Vessel's Parts...
					foreach (Part part in vessel.Parts)
					{
						// ...loop through each Part's Modules...
						foreach (T module in getModules(part))
						{
							// ...and add each matching Module to the new list
							modulesInVessel.Add(module);
						}
					}

					// ...set the shallow cache entry to the new list
					vesselModuleTable[vessel.id] = modulesInVessel;
				}

				// ...return the shallow cache for the queried Vessel
				return vesselModuleTable[vessel.id];
			}

			// Otherwise, return an empty list.
			return new List<T>();
		}

		/// <summary>
		/// Gets a flat list of all modules of type T in the given Part.  Returns an empty list if none exist.
		/// </summary>
		/// <returns>The list of modules of type T</returns>
		/// <param name="part">The Part being queried</param>
		public List<T> getModules(Part part)
		{
			Guid id;

			if (HighLogic.LoadedSceneIsEditor)
			{
				id = this.editorVesselID;
			}
			// If the Part's Vessel is defined...
			else if (part.vessel != null)
			{
				id = part.vessel.id;
			}
			else
			{
				// Otherwise, return an empty list
				return new List<T>();
			}

			// ...and if the Vessel is not in the deep cache...
			if (!vesselPartModuleDB.ContainsKey(id))
			{
				// ...create a new table for the Vessel in the deep cache.
				vesselPartModuleDB[id] = new Dictionary<uint, List<T>>();
			}

			// ...and if the Part is not in the Vessel's table in the deep cache...
			if (!vesselPartModuleDB[id].ContainsKey(part.uid))
			{
				// ...create a flat list of modules
				List<T> modulesInPart = new List<T>();

				// ...loop through the Part's modules...
				foreach (PartModule module in part.Modules)
				{
					// ...if any module matches...
					if (module is T)
					{
						// ...add it to the list
						modulesInPart.Add((T)module);
					}
				}

				// ...set the deep cache entry to the new list
				vesselPartModuleDB[id][part.uid] = modulesInPart;
			}

			// ...return the deep cache entry for the queried Part.
			return vesselPartModuleDB[id][part.uid];
		}

		/// <summary>
		/// Returns true if the given Vessel exists in the deep cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Vessel exists in the deep cache, false otherwise</returns>
		/// <param name="vessel">The Vessel being queried</param>
		public bool inDeepCache(Vessel vessel)
		{
			return vesselPartModuleDB.ContainsKey(HighLogic.LoadedSceneIsEditor ? this.editorVesselID : vessel.id);
		}

		/// <summary>
		/// Returns true if the given Part exists in the deep cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Part exists in the deep cache, false otherwise</returns>
		/// <param name="part">The Part being queried</param>
		public bool inDeepCache(Part part)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				return vesselPartModuleDB.ContainsKey(this.editorVesselID) &&
					vesselPartModuleDB[this.editorVesselID].ContainsKey(part.uid);
			}
			else
			{
				if (part.vessel == null)
				{
					return false;
				}

				return inDeepCache(part.vessel) && vesselPartModuleDB[part.vessel.id].ContainsKey(part.uid);
			}
		}

		/// <summary>
		/// Returns true if the given Vessel exists in the shallow cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Vessel exists in the shallow cache, false otherwise</returns>
		/// <param name="vessel">The Vessel being queried</param>
		public bool inShallowCache(Vessel vessel)
		{
			return vesselModuleTable.ContainsKey(HighLogic.LoadedSceneIsEditor ? this.editorVesselID : vessel.id);
		}
	}
}