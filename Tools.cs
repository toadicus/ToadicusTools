// ToadicusTools
//
// Tools.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ToadicusTools
{
	public static partial class Tools
	{
		#region DEBUG_TOOLS
		private static ScreenMessage debugmsg = new ScreenMessage("", 4f, ScreenMessageStyle.UPPER_RIGHT);

		[System.Diagnostics.Conditional("DEBUG")]
		public static void PostDebugMessage(string Msg)
		{
			if (HighLogic.LoadedScene > GameScenes.SPACECENTER)
			{
				debugmsg.message = Msg;
				ScreenMessages.PostScreenMessage(debugmsg, true);
			}

			KSPLog.print(Msg);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void PostDebugMessage(object Sender, params object[] args)
		{
			string Msg;

			Msg = string.Format(
				"{0}:\n\t{1}",
				Sender.GetType().Name,
				string.Join("\n\t", args.Select(a => (string)a).ToArray())
			);

			PostDebugMessage(Msg);
		}

		[System.Diagnostics.Conditional("VERBOSE")]
		public static void PostVerboseMessage(object Sender, params object[] args)
		{
			PostDebugMessage(Sender, args);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void DebugFieldsActivate(this PartModule partModule)
		{
			foreach (BaseField field in partModule.Fields)
			{
				field.guiActive = field.guiActiveEditor = true;
			}
		}

		public class DebugLogger
		{
			public static DebugLogger New(object caller)
			{
				#if DEBUG
				return new DebugLogger(caller.GetType());
				#else
				return null;
				#endif
			}

			public static DebugLogger New(Type callingType)
			{
				return new DebugLogger(callingType);
			}

			private StringBuilder stringBuilder;

			private DebugLogger() {}

			private DebugLogger(Type caller)
			{
				this.stringBuilder = new StringBuilder(caller.Name);
				this.stringBuilder.Append(": ");
			}

			[System.Diagnostics.Conditional("DEBUG")]
			public void Append(object value)
			{
				this.stringBuilder.Append(value);
			}

			[System.Diagnostics.Conditional("DEBUG")]
			public void AppendFormat(string format, params object[] args)
			{
				this.stringBuilder.AppendFormat(format, args);
			}

			[System.Diagnostics.Conditional("DEBUG")]
			public void AppendFormat(string value)
			{
				this.stringBuilder.AppendLine(value);
			}

			[System.Diagnostics.Conditional("DEBUG")]
			public void Print()
			{
				PostDebugMessage(this.stringBuilder.ToString());
			}
		}
		#endregion
	}
}