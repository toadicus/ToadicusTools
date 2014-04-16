// ToadicusTools
//
// EventSniffer.cs
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

#if DEBUG
using KSP;
using System;
using System.Text;
using UnityEngine;

namespace AntennaRange
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class EventSniffer : MonoBehaviour
	{
		public void Awake()
		{
			GameEvents.onSameVesselDock.Add(this.onSameVesselDockUndock);
			GameEvents.onSameVesselUndock.Add(this.onSameVesselDockUndock);
			GameEvents.onPartUndock.Add(this.onPartUndock);
			GameEvents.onUndock.Add(this.onReportEvent);
			GameEvents.onPartCouple.Add(this.onPartCouple);
			GameEvents.onPartJointBreak.Add(this.onPartJointBreak);
		}

		public void Destroy()
		{
			GameEvents.onSameVesselDock.Remove(this.onSameVesselDockUndock);
			GameEvents.onSameVesselUndock.Remove(this.onSameVesselDockUndock);
			GameEvents.onPartUndock.Remove(this.onPartUndock);
			GameEvents.onUndock.Remove(this.onReportEvent);
			GameEvents.onPartCouple.Remove(this.onPartCouple);
			GameEvents.onPartJointBreak.Remove(this.onPartJointBreak);
		}

		public void onSameVesselDockUndock(GameEvents.FromToAction<ModuleDockingNode, ModuleDockingNode> data)
		{
			this.FromModuleToModuleHelper(
				this.getStringBuilder(),
				new GameEvents.FromToAction<PartModule, PartModule>(data.from, data.to)
			);
		}

		public void onPartJointBreak(PartJoint joint)
		{
			this.PartJointHelper(this.getStringBuilder(), joint);
		}

		public void onPartUndock(Part part)
		{
			this.PartEventHelper(this.getStringBuilder(), part);
		}

		public void onReportEvent(EventReport data)
		{
			this.EventReportHelper(this.getStringBuilder(), data);
		}

		public void onPartCouple(GameEvents.FromToAction<Part, Part> data)
		{
			this.FromPartToPartHelper(this.getStringBuilder(), data);
		}

		internal void EventReportHelper(StringBuilder sb, EventReport data)
		{
			sb.Append("\n\tOrigin Part:");
			this.appendPartAncestry(sb, data.origin);

			sb.AppendFormat(
				"\n\tother: '{0}'" +
				"\n\tmsg: '{1}'" +
				"\n\tsender: '{2}'",
				data.other,
				data.msg,
				data.sender
			);

			Debug.Log(sb.ToString());
		}

		internal void PartEventHelper(StringBuilder sb, Part part)
		{
			this.appendPartAncestry(sb, part);

			Debug.Log(sb.ToString());
		}

		internal void FromPartToPartHelper(StringBuilder sb, GameEvents.FromToAction<Part, Part> data)
		{
			sb.Append("\n\tFrom:");

			this.appendPartAncestry(sb, data.from);

			sb.Append("\n\tTo:");

			this.appendPartAncestry(sb, data.to);

			Debug.Log(sb.ToString());
		}

		internal void FromModuleToModuleHelper(StringBuilder sb, GameEvents.FromToAction<PartModule, PartModule> data)
		{
			sb.Append("\n\tFrom:");

			this.appendModuleAncestry(sb, data.from);

			sb.Append("\n\tTo:");

			this.appendModuleAncestry(sb, data.to);

			Debug.Log(sb.ToString());
		}

		internal void PartJointHelper(StringBuilder sb, PartJoint joint)
		{
			sb.Append("PartJoint: ");
			if (joint != null)
			{
				sb.Append(joint);
				this.appendPartAncestry(sb, joint.Host);
			}
			else
			{
				sb.Append("null");
			}

			Debug.Log(sb.ToString());
		}

		internal StringBuilder appendModuleAncestry(StringBuilder sb, PartModule module, uint tabs = 1)
		{
			sb.Append('\n');
			for (ushort i=0; i < tabs; i++)
			{
				sb.Append('\t');
			}
			sb.Append("Module: ");

			if (module != null)
			{
				sb.Append(module.moduleName);
				this.appendPartAncestry(sb, module.part, tabs + 1u);
			}
			else
			{
				sb.Append("null");
			}

			return sb;
		}

		internal StringBuilder appendPartAncestry(StringBuilder sb, Part part, uint tabs = 1)
		{
			sb.Append('\n');
			for (ushort i=0; i < tabs; i++)
			{
				sb.Append('\t');
			}
			sb.Append("Part: ");

			if (part != null)
			{
				sb.AppendFormat("'{0}' ({1})", part.partInfo.title, part.partName);
				this.appendVessel(sb, part.vessel, tabs + 1u);
			}
			else
			{
				sb.Append("null");
			}

			return sb;
		}

		internal StringBuilder appendVessel(StringBuilder sb, Vessel vessel, uint tabs = 1)
		{
			sb.Append('\n');
			for (ushort i=0; i < tabs; i++)
			{
				sb.Append('\t');
			}
			sb.Append("Vessel: ");

			if (vessel != null)
			{
				sb.AppendFormat("'{0}' ({1})", vessel.vesselName, vessel.id);
			}
			else
			{
				sb.Append("null");
			}

			return sb;
		}

		internal StringBuilder getStringBuilder()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}: called {1} ",
				this.GetType().Name,
				new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name
			);
			return sb;
		}
	}
}
#endif