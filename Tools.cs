// ToadicusTools © 2014 toadicus
//
// This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. To view a
// copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ToadicusTools
{
	public static partial class Tools
	{
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
	}
}