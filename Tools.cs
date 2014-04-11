// ToadicusTools © 2014 toadicus
//
// This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. To view a
// copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/

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