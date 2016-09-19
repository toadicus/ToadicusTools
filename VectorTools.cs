// ToadicusTools
//
// VectorTools.cs
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

using System;
using ToadicusTools.Text;
using KSP;

namespace ToadicusTools
{
	public static class VectorTools
	{
		/// <summary>
		/// Returns true if no CelestialBody occludes the distant point from the local point, false otherwise.
		/// Includes a 5% "fudge factor".
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="localPoint">local point</param>
		/// <param name="distantPoint">target point</param>
		/// <param name="firstOccludingBody">Set to the first body found to be blocking line of sight,
		/// if any, otherwise null.</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		public static bool IsLineOfSightBetween(
			Vector3d localPoint,
			Vector3d distantPoint,
			out CelestialBody firstOccludingBody,
			CelestialBody excludedBody,
			double sqrRatio = 1d
		)
		{
			// Line X = A + tN
			Vector3d dFroma = distantPoint - localPoint;
			Vector3d n = dFroma.normalized;

			CelestialBody body;
			for (int idx = 0; idx < FlightGlobals.Bodies.Count; idx++)
			{
				body = FlightGlobals.Bodies[idx];

				if (excludedBody != null && body == excludedBody)
				{
					continue;
				}

				// Point p
				Vector3d p = body.position;

				Vector3d pFroma = localPoint - p;

				// If this body is closer than the point, it's a candidate, so keep looking
				if (pFroma.sqrMagnitude < dFroma.sqrMagnitude)
				{
					double pFromaDotn = Vector3d.Dot(pFroma, n);

					// 
					if (pFromaDotn < 0)
					{
						// Shortest distance d from point p to line X
						Vector3d d = pFroma - pFromaDotn * n;

						if (d.sqrMagnitude < (body.Radius * body.Radius * sqrRatio))
						{
							firstOccludingBody = body;
							return false;
						}
					}
				}
			}

			firstOccludingBody = null;
			return true;
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the distant point from the local point, false otherwise.
		/// Includes a 5% "fudge factor".
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="localPoint">local point</param>
		/// <param name="distantPoint">target point</param>
		/// <param name="firstOccludingBody">Set to the first body found to be blocking line of sight,
		/// if any, otherwise null.</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		public static bool IsLineOfSightBetween(
			Vector3d localPoint,
			Vector3d distantPoint,
			out CelestialBody firstOccludingBody,
			double sqrRatio = 1d
		)
		{
			// Line X = A + tN
			Vector3d dFroma = distantPoint - localPoint;
			Vector3d n = dFroma.normalized;

			CelestialBody body;
			for (int idx = 0; idx < FlightGlobals.Bodies.Count; idx++)
			{
				body = FlightGlobals.Bodies[idx];

				// Point p
				Vector3d p = body.position;

				Vector3d pFroma = localPoint - p;

				// If this body is closer than the point, it's a candidate, so keep looking
				if (pFroma.sqrMagnitude < dFroma.sqrMagnitude)
				{
					double pFromaDotn = Vector3d.Dot(pFroma, n);

					// If this body is not behind us, keep looking
					if (pFromaDotn < 0)
					{
						// Shortest distance d from point p to line X
						Vector3d d = pFroma - pFromaDotn * n;

						// If the shortest distance to our line is less than the radius, the planet is blocking us
						if (d.sqrMagnitude < (body.Radius * body.Radius * sqrRatio))
						{
							firstOccludingBody = body;
							return false;
						}
					}
				}
			}

			firstOccludingBody = null;
			return true;
		}

		[Obsolete("Call to obsolete IsLineOfSight overload with CelestialBody[] excludedBodies.", true)]
		public static bool IsLineOfSightBetween(
			Vector3d localPoint,
			Vector3d distantPoint,
			out CelestialBody firstOccludingBody,
			CelestialBody[] excludedBodies = null,
			double sqrRatio = 1d
		)
		{
			CelestialBody body;

			if (excludedBodies != null && excludedBodies.Length > 0)
			{
				body = excludedBodies[0];
			}
			else
			{
				body = null;
			}

			return IsLineOfSightBetween(localPoint, distantPoint, out firstOccludingBody, body, sqrRatio);
		}

		/// <summary>
		/// Finds the shortest distance from point "P" to line "X = A + tN".
		/// </summary>
		/// <returns>Distance from point "P" to line "A + tN"</returns></returns>
		/// <param name="A">Point on line "X"</param>
		/// <param name="N">Normalized direction of line "X"</param>
		/// <param name="P">Point "P" not on line "X"</param>
		public static Vector3d PointDistanceToLine(Vector3d A, Vector3d N, Vector3d P)
		{
			Vector3d PfromA = A - P;

			double PfromAdotN = Vector3d.Dot(PfromA, N);

			// Shortest distance d from point p to line X
			Vector3d D = PfromA - PfromAdotN * N;

			return D;
		}

		public static string ToString(Vector3d vector, string format)
		{
			return Text.Extensions.ToString(vector, format);
		}
	}
}

