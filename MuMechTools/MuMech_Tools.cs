// ToadicusTools
//
// MuMech_Tools.cs
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
// 
// This software uses methods derived from MuMechLib © 2013-2014 r4m0n, sarbian, et al
// Used under the terms of the General Public License, version 3.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToadicusTools.MuMechTools
{
	public static class MuMechTools
	{
		/*
		* MuMechLib Methods
		* The methods below are adapted from MuMechLib, © 2013-2014 r4m0n
		* The following methods are a derivative work of the code from MuMechLib in the MechJeb project.
		* Used under license.
		* */

		// Derived from MechJeb2/VesselState.cs
		public static Quaternion getSurfaceRotation(this Vessel vessel)
		{
			Vector3 CoM;

			try
			{
				CoM = vessel.CoM;
			}
			catch
			{
				return new Quaternion();
			}

			Vector3 bodyPosition = vessel.mainBody.position;
			Vector3 bodyUp = vessel.mainBody.transform.up;

			Vector3 surfaceUp = (CoM - vessel.mainBody.position).normalized;
			Vector3 surfaceNorth = Vector3.ProjectOnPlane(
				(bodyPosition + bodyUp * (float)vessel.mainBody.Radius) - CoM,
				surfaceUp
			).normalized;

			Quaternion surfaceRotation = Quaternion.LookRotation(surfaceNorth, surfaceUp);

			return Quaternion.Inverse(
				Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(vessel.GetTransform().rotation) * surfaceRotation
			);
		}

		// Derived from MechJeb2/VesselState.cs
		public static double getSurfaceHeading(this Vessel vessel)
		{
			return vessel.getSurfaceRotation().eulerAngles.y;
		}

		// Derived from MechJeb2/VesselState.cs
		public static double getSurfacePitch(this Vessel vessel)
		{
			Quaternion vesselSurfaceRotation = vessel.getSurfaceRotation();

			return (vesselSurfaceRotation.eulerAngles.x > 180f) ?
				(360f - vesselSurfaceRotation.eulerAngles.x) :
				-vesselSurfaceRotation.eulerAngles.x;
		}

		// Derived from MechJeb2/MuUtils.cs
		public static string MuMech_ToSI(
			double d, int digits = 3, int MinMagnitude = 0, int MaxMagnitude = int.MaxValue
		)
		{
			float exponent = (float)Math.Log10(Math.Abs(d));
			exponent = Mathf.Clamp(exponent, (float)MinMagnitude, (float)MaxMagnitude);

			if (exponent >= 0)
			{
				switch ((int)Math.Floor(exponent))
				{
					case 0:
					case 1:
					case 2:
						return d.ToString("F" + digits);
					case 3:
					case 4:
					case 5:
						return (d / 1e3).ToString("F" + digits) + "k";
					case 6:
					case 7:
					case 8:
						return (d / 1e6).ToString("F" + digits) + "M";
					case 9:
					case 10:
					case 11:
						return (d / 1e9).ToString("F" + digits) + "G";
					case 12:
					case 13:
					case 14:
						return (d / 1e12).ToString("F" + digits) + "T";
					case 15:
					case 16:
					case 17:
						return (d / 1e15).ToString("F" + digits) + "P";
					case 18:
					case 19:
					case 20:
						return (d / 1e18).ToString("F" + digits) + "E";
					case 21:
					case 22:
					case 23:
						return (d / 1e21).ToString("F" + digits) + "Z";
					default:
						return (d / 1e24).ToString("F" + digits) + "Y";
				}
			}
			else if (exponent < 0)
			{
				switch ((int)Math.Floor(exponent))
				{
					case -1:
					case -2:
					case -3:
						return (d * 1e3).ToString("F" + digits) + "m";
					case -4:
					case -5:
					case -6:
						return (d * 1e6).ToString("F" + digits) + "μ";
					case -7:
					case -8:
					case -9:
						return (d * 1e9).ToString("F" + digits) + "n";
					case -10:
					case -11:
					case -12:
						return (d * 1e12).ToString("F" + digits) + "p";
					case -13:
					case -14:
					case -15:
						return (d * 1e15).ToString("F" + digits) + "f";
					case -16:
					case -17:
					case -18:
						return (d * 1e18).ToString("F" + digits) + "a";
					case -19:
					case -20:
					case -21:
						return (d * 1e21).ToString("F" + digits) + "z";
					default:
						return (d * 1e24).ToString("F" + digits) + "y";
				}
			}
			else
			{
				return "0";
			}
		}

		/*
		 * END MuMecLib METHODS
		 * */
	}
}