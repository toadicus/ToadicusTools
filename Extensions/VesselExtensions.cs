// ToadicusTools
//
// VesselExtensions.cs
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
using UnityEngine;

namespace ToadicusTools.Extensions
{
	public static class VesselExtensions
	{
		/// <summary>
		/// Returns the distance between this Vessel and another Vessel.
		/// </summary>
		/// <param name="vesselOne">This <see cref="Vessel"/><see ></param>
		/// <param name="vesselTwo">Another <see cref="Vessel"/></param>
		public static double DistanceTo(this Vessel vesselOne, Vessel vesselTwo)
		{
			return (vesselOne.GetWorldPos3D() - vesselTwo.GetWorldPos3D()).magnitude;
		}

		/// <summary>
		/// Returns the distance between this Vessel and a CelestialBody
		/// </summary>
		/// <param name="vessel">This Vessel</param>
		/// <param name="body">A <see cref="CelestialBody"/></param>
		public static double DistanceTo(this Vessel vessel, CelestialBody body)
		{
			return (vessel.GetWorldPos3D() - body.position).magnitude;
		}

		/// <summary>
		/// Returns the square of the distance between this Vessel and another Vessel.
		/// </summary>
		/// <param name="vesselOne">This <see cref="Vessel"/><see ></param>
		/// <param name="vesselTwo">Another <see cref="Vessel"/></param>
		public static double sqrDistanceTo(this Vessel vesselOne, Vessel vesselTwo)
		{
			if (vesselOne == null || vesselTwo == null)
			{
				return double.PositiveInfinity;
			}

			return (vesselOne.GetWorldPos3D() - vesselTwo.GetWorldPos3D()).sqrMagnitude;
		}

		/// <summary>
		/// Returns the square of the distance between this Vessel and a CelestialBody
		/// </summary>
		/// <param name="vessel">This Vessel</param>
		/// <param name="body">A <see cref="CelestialBody"/></param>
		public static double sqrDistanceTo(this Vessel vessel, CelestialBody body)
		{
			if (vessel == null || body == null)
			{
				return double.PositiveInfinity;
			}

			return (vessel.GetWorldPos3D() - body.position).sqrMagnitude;
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target Vessel from this Vessel, false otherwise.
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel,
		/// <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="targetVessel">target Vessel</param>
		/// <param name="firstOccludingBody">Set to the first body found to be blocking line of sight,
		/// if any, otherwise null.</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			Vessel targetVessel,
			out CelestialBody firstOccludingBody,
			double sqrRatio = 1d
		)
		{
			return VectorTools.IsLineOfSightBetween(
				vessel.GetWorldPos3D(),
				targetVessel.GetWorldPos3D(),
				out firstOccludingBody,
				sqrRatio
			);
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target CelestialBody from this Vessel, false otherwise.
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="targetBody">target CelestialBody</param>
		/// <param name="firstOccludingBody">Set to the first body found to be blocking line of sight,
		/// if any, otherwise null.</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			CelestialBody targetBody,
			out CelestialBody firstOccludingBody,
			double sqrRatio = 1d
		)
		{
			return VectorTools.IsLineOfSightBetween(
				vessel.GetWorldPos3D(),
				targetBody.position,
				out firstOccludingBody,
				targetBody,
				sqrRatio
			);
		}

		/// <summary>
		/// Checks if this vessel has a properly-crewed manned command pod.
		/// </summary>
		/// <returns><c>true</c>, if this vessel is adequately crewed for control, <c>false</c> otherwise.</returns>
		/// <param name="vessel"></param>
		public static bool hasCrewCommand(this Vessel vessel)
		{
			return (vessel.CurrentCommand() & VesselCommand.Crew) == VesselCommand.Crew;
		}

		public static bool hasProbeCommand(this Vessel vessel)
		{
			return (vessel.CurrentCommand() & VesselCommand.Probe) == VesselCommand.Probe;
		}

		public static uint PartCrewCount(this Part part)
		{
			return part.PartCrewCount(false);
		}

		public static uint PartCrewCount(this Part part, bool countTourists)
		{
			if (part == null)
			{
				throw new ArgumentNullException("part.PartCrewCount: part must not be null");
			}

			if (part.protoModuleCrew == null)
			{
				return 0u;
			}

			uint count = 0u;
			ProtoCrewMember member;

			for (int idx = 0; idx < part.protoModuleCrew.Count; idx++)
			{
				member = part.protoModuleCrew[idx];

				if (countTourists || member.type != ProtoCrewMember.KerbalType.Tourist)
				{
					count++;
				}
			}

			return count;
		}

		public static VesselCommand CurrentCommand(this Vessel vessel)
		{
			VesselCommand currentCommand = VesselCommand.None;

			IList<PartModule> modules = vessel.getModulesOfType<PartModule>();
			PartModule module;
			for (int idx  = 0; idx < modules.Count; idx++)
			{
				module = modules[idx];
				if (module is ModuleCommand)
				{
					ModuleCommand commandModule = module as ModuleCommand;

					if (commandModule.part.PartCrewCount() >= commandModule.minimumCrew)
					{
						if (
							commandModule.minimumCrew > 0 ||
							(commandModule.part.CrewCapacity > 0 &&	commandModule.part.protoModuleCrew.Count > 0)
						)
						{
							currentCommand |= VesselCommand.Crew;
						}
						if (commandModule.minimumCrew == 0)
						{
							currentCommand |= VesselCommand.Probe;
						}
					}
				}

				if (module is KerbalSeat)
				{
					KerbalSeat seatModule = module as KerbalSeat;

					if (seatModule.Occupant != null)
					{
						currentCommand |= VesselCommand.Crew;
					}
				}

				if (currentCommand == (VesselCommand.Crew | VesselCommand.Probe))
				{
					break;
				}
			}

			return currentCommand;
		}

		/// <summary>
		/// Gets a list of PartModules of type T within this vessel.
		/// </summary>
		/// <returns>a list of PartModules of type T within this vessel, or an empty list if none</returns>
		/// <param name="vessel"></param>
		/// <typeparam name="T">PartModule type paramter</typeparam>
		public static IList<T> getModulesOfType<T>(this Vessel vessel) where T : PartModule
		{
			if (vessel == null)
			{
				throw new ArgumentNullException(
					string.Format(
						"Vessel.getModulesOfType<{0}>: 'vessel' argument cannot be null.",
						typeof(T).Name
					)
				);
			}

			List<T> modulesInVessel = new List<T>();

			Part part;
			PartModule module;

			for (int pIdx = 0; pIdx < vessel.Parts.Count; pIdx++)
			{
				part = vessel.Parts[pIdx];

				for (int mIdx = 0; mIdx < part.Modules.Count; mIdx++)
				{
					module = part.Modules[mIdx];

					if (module is T)
					{
						modulesInVessel.Add((T)module);
					}
				}
			}

			return modulesInVessel.AsReadOnly();
		}

		public static bool tryGetFirstModuleOfType<T>(this Vessel vessel, out T module) where T : PartModule
		{
			if (vessel == null)
			{
				throw new ArgumentNullException(
					string.Format(
						"Vessel.getModulesOfType<{0}>: 'vessel' argument cannot be null.",
						typeof(T).Name
					)
				);
			}

			module = null;

			if (vessel.Parts == null)
			{
				return false;
			}

			Part part;
			for (int idx = 0; idx < vessel.Parts.Count; idx++)
			{
				part = vessel.Parts[idx];
				if (part.tryGetFirstModuleOfType<T>(out module))
				{
					return true;
				}
			}

			return false;
		}

		public static T getFirstModuleOfType<T>(this Vessel vessel) where T : PartModule
		{
			if (vessel == null)
			{
				throw new ArgumentNullException(
					string.Format(
						"Vessel.getModulesOfType<{0}>: 'vessel' argument cannot be null.",
						typeof(T).Name
					)
				);
			}

			T module;

			if (vessel.tryGetFirstModuleOfType<T>(out module))
			{
				return module;
			}

			return null;
		}

		public static bool hasModuleOfType<T>(this Vessel vessel) where T : PartModule
		{
			if (vessel == null)
			{
				throw new ArgumentNullException(
					string.Format(
						"Vessel.getModulesOfType<{0}>: 'vessel' argument cannot be null.",
						typeof(T).Name
					)
				);
			}

			T _;

			return vessel.tryGetFirstModuleOfType(out _);
		}
	}
}
