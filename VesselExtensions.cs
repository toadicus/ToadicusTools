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
		/// Returns true if no CelestialBody occludes the target Vessel from this Vessel, false otherwise.
		/// This code is adapted from the RemoteTech2 RangeModelExtensions.
		/// RemoteTech2 © Cilph 2013-2014.  Used under the GPLv2 license.
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vesselOne">this Vessel</param>
		/// <param name="vesselTwo">target Vessel</param>
		public static bool hasLineOfSightTo(this Vessel vesselOne, Vessel vesselTwo)
		{
			Vector3d v1Pos = vesselOne.GetWorldPos3D();
			Vector3d v2Pos = vesselTwo.GetWorldPos3D();

			foreach (CelestialBody referenceBody in FlightGlobals.Bodies)
			{
				Vector3d bodyFromV1 = referenceBody.position - v1Pos;
				Vector3d V2FromV1 = v2Pos - v1Pos;

				if (Vector3d.Dot(bodyFromV1, V2FromV1) <= 0)
				{
					continue;
				}

				Vector3d V2FromV1norm = V2FromV1.normalized;

				if (Vector3d.Dot(bodyFromV1, V2FromV1norm) >= V2FromV1.magnitude)
				{
					continue;
				}

				Vector3d lateralOffset = bodyFromV1 - Vector3d.Dot(bodyFromV1, V2FromV1norm) * V2FromV1norm;

				if (lateralOffset.magnitude < referenceBody.Radius - 5)
				{
					return false;
				}
			}
			return true;
		}

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

			#if MODULE_DB_AVAILABLE
			return ModuleDB<T>.Instance.getModules(vessel);
			#else
			throw new NotImplementedException("Vessel.getModulesOfType<T> is not implemented without ModuleDB.");
			#endif
		}
	}
}
