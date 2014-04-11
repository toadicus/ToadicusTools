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
	public class ModuleDB<T>
		where T : PartModule
	{
		private static ModuleDB<T> _instance;

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

		private ModuleDB()
		{
			vesselPartModuleDB = new Dictionary<Guid, Dictionary<uint, List<T>>>();
			vesselModuleTable = new Dictionary<Guid, List<T>>();

			GameEvents.onVesselWasModified.Add(onVesselEvent);
			GameEvents.onVesselChange.Add(onVesselEvent);
			GameEvents.onVesselDestroy.Add(onVesselEvent);
			GameEvents.onGameSceneLoadRequested.Add(onSceneChange);
			GameEvents.onPartUndock.Add(onPartEvent);
			GameEvents.onPartCouple.Add(onFromPartToPartEvent);
		}

		private void onVesselEvent(Vessel vessel)
		{
			if (vesselPartModuleDB.ContainsKey(vessel.id))
			{
				vesselPartModuleDB.Remove(vessel.id);
			}

			if (vesselModuleTable.ContainsKey(vessel.id))
			{
				vesselModuleTable.Remove(vessel.id);
			}
		}

		private void onSceneChange(GameScenes scene)
		{
			if (FlightGlobals.ActiveVessel != null)
			{
				onVesselEvent(FlightGlobals.ActiveVessel);
			}
		}

		private void onPartEvent(Part part)
		{
			if (part.vessel != null)
			{
				onVesselEvent(part.vessel);
			}
		}

		private void onFromPartToPartEvent(GameEvents.FromToAction<Part, Part> data)
		{
			onPartEvent(data.from);
			onPartEvent(data.to);
		}

		public List<T> getModules(Vessel vessel)
		{
			Tools.DebugLogger log = Tools.DebugLogger.New(typeof(ModuleDB<T>));

			if (vessel.Parts != null && vessel.Parts.Count > 0)
			{
				if (!vesselModuleTable.ContainsKey(vessel.id))
				{
					log.AppendFormat("Vessel '{0}' not in shallow cache, building new entry.", vessel);

					List<T> modulesInVessel = new List<T>();

					foreach (Part part in vessel.Parts)
					{
						foreach (T module in getModules(part))
						{
							modulesInVessel.Add(module);
						}
					}

					vesselModuleTable[vessel.id] = modulesInVessel;
				}

				log.AppendFormat("Returning {0} modules of type '{1}' for Vessel '{2}'",
					vesselModuleTable[vessel.id].Count, typeof(T).Name, vessel);

				log.Print();

				return vesselModuleTable[vessel.id];
			}

			return new List<T>();
		}

		public List<T> getModules(Part part)
		{
			Tools.DebugLogger log = Tools.DebugLogger.New(typeof(ModuleDB<T>));

			if (part.vessel != null)
			{
				if (!vesselPartModuleDB.ContainsKey(part.vessel.id))
				{
					log.AppendFormat("Vessel '{0}' not in deep cache, building new entry.", part.vessel);

					vesselPartModuleDB[part.vessel.id] = new Dictionary<uint, List<T>>();
				}

				if (!vesselPartModuleDB[part.vessel.id].ContainsKey(part.uid))
				{
					log.AppendFormat("Part '{0}' not in cache for Vessel '{1}', building new entry.",
						part, part.vessel);

					List<T> modulesInPart = new List<T>();

					foreach (PartModule module in part.Modules)
					{
						if (module is T)
						{
							modulesInPart.Add((T)module);
						}
					}

					vesselPartModuleDB[part.vessel.id][part.uid] = modulesInPart;
				}

				log.AppendFormat("Returning {2} modules of type '{3}' in Part '{0}' for Vessel '{1}'.",
					part, part.vessel, vesselPartModuleDB[part.vessel.id][part.uid].Count, typeof(T).Name);

				log.Print();

				return vesselPartModuleDB[part.vessel.id][part.uid];
			}

			return new List<T>();
		}

		public bool inDeepCache(Vessel vessel)
		{
			return vesselPartModuleDB.ContainsKey(vessel.id);
		}

		public bool inDeepCache(Part part)
		{
			if (part.vessel == null)
			{
				return false;
			}

			return inDeepCache(part.vessel) && vesselPartModuleDB[part.vessel.id].ContainsKey(part.uid);
		}

		public bool inShallowCache(Vessel vessel)
		{
			return vesselModuleTable.ContainsKey(vessel.id);
		}
	}
}