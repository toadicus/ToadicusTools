// ToadicusTools
//
// VesselExtensions.cs
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
			return (vesselOne.GetWorldPos3D() - vesselTwo.GetWorldPos3D()).sqrMagnitude;
		}

		/// <summary>
		/// Returns the square of the distance between this Vessel and a CelestialBody
		/// </summary>
		/// <param name="vessel">This Vessel</param>
		/// <param name="body">A <see cref="CelestialBody"/></param>
		public static double sqrDistanceTo(this Vessel vessel, CelestialBody body)
		{
			return (vessel.GetWorldPos3D() - body.position).sqrMagnitude;
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target point from this Vessel, false otherwise.
		/// Includes a 5% "fudge factor".
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="distantPoint">target point</param>
		public static bool hasLineOfSightTo(this Vessel vessel, Vector3d distantPoint, CelestialBody[] excludedBodies = null)
		{
			// Line X = A + tN
			Vector3d a = vessel.GetWorldPos3D();
			Vector3d n = (distantPoint - a).normalized;

			if (FlightGlobals.Bodies != null)
			{
				foreach (CelestialBody body in FlightGlobals.Bodies)
				{
					if (excludedBodies != null && excludedBodies.Contains(body))
					{
						continue;
					}

					// Point p
					Vector3d p = body.position;

					// Shortest distance d from point p to line X
					Vector3d d = (a - p) - (Vector3d.Dot((a - p), n)) * n;

					if (d.sqrMagnitude < (body.Radius * body.Radius * .9025d))
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target Vessel from this Vessel, false otherwise.
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="targetVessel">target Vessel</param>
		public static bool hasLineOfSightTo(this Vessel vessel, Vessel targetVessel)
		{
			return vessel.hasLineOfSightTo(targetVessel.GetWorldPos3D());
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target CelestialBody from this Vessel, false otherwise.
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="targetBody">target CelestialBody</param>
		public static bool hasLineOfSightTo(this Vessel vessel, CelestialBody targetBody)
		{
			return vessel.hasLineOfSightTo(targetBody.position, new CelestialBody[] {targetBody});
		}

		/// <summary>
		/// Checks if this vessel has a properly-crewed manned command pod.
		/// </summary>
		/// <returns><c>true</c>, if this vessel is adequately crewed for control, <c>false</c> otherwise.</returns>
		/// <param name="vessel"></param>
		public static bool hasCrewCommand(this Vessel vessel)
		{
			List<ModuleCommand> commandModules = vessel.getModulesOfType<ModuleCommand>();

			foreach (ModuleCommand commandModule in commandModules)
			{
				if (
					commandModule.part != null &&
					commandModule.part.protoModuleCrew != null &&
					commandModule.part.protoModuleCrew.Count > commandModule.minimumCrew
				)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets a list of PartModules of type T within this vessel.
		/// </summary>
		/// <returns>a list of PartModules of type T within this vessel, or an empty list if none</returns>
		/// <param name="vessel"></param>
		/// <typeparam name="T">PartModule type paramter</typeparam>
		public static List<T> getModulesOfType<T>(this Vessel vessel) where T : PartModule
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

			if (ModuleDB<T>.DBPresent)
			{
				return ModuleDB<T>.Instance.getModules(vessel);
			}
			else
			{
				List<T> modulesInVessel = new List<T>();

				foreach (Part part in vessel.Parts)
				{
					foreach (PartModule module in part.Modules)
					{
						if (module is T)
						{
							modulesInVessel.Add((T)module);
						}
					}
				}

				return modulesInVessel;
			}
		}
	}
}
