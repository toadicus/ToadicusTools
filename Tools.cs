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

		[System.Diagnostics.Conditional("DEBUG")]
		public static void PostDebugMessage(object Sender, string Format, params object[] args)
		{
			StringBuilder sb = new StringBuilder();

			if (Sender != null)
			{
				Type type = (Sender is Type) ? Sender as Type : Sender.GetType();
				sb.Append(type.Name);
				sb.Append(": ");
			}

			sb.AppendFormat(Format, args);

			PostDebugMessage(sb.ToString());
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
			public void AppendLine(string value)
			{
				this.stringBuilder.Append(value);
				this.stringBuilder.Append('\n');
			}

			[System.Diagnostics.Conditional("DEBUG")]
			public void Print(bool postToScreen)
			{
				if (postToScreen)
				{
					PostDebugMessage(this.stringBuilder.ToString());
				}
				else
				{
					Debug.Log(this.stringBuilder.ToString());
				}

				this.stringBuilder.Length = 0;
			}

			[System.Diagnostics.Conditional("DEBUG")]
			public void Print()
			{
				this.Print(true);
			}
		}
		#endregion

		#region Array_Tools
		public static bool Contains(this GameScenes[] haystack, GameScenes needle)
		{
			foreach (GameScenes item in haystack)
			{
				if (item == needle)
				{
					return true;
				}
			}

			return false;
		}

		public static bool Contains(this CelestialBody[] haystack, CelestialBody needle)
		{
			foreach (CelestialBody item in haystack)
			{
				if (item == needle)
				{
					return true;
				}
			}

			return false;
		}
		#endregion

		#region Enum_Tools
		public static bool TryParse<enumType>(string value, out enumType result)
			where enumType : struct, IConvertible, IComparable, IFormattable
		{
			try
			{
				if (!typeof(enumType).IsEnum)
				{
					throw new ArgumentException("result must be of an enum type");
				}

				result = (enumType)Enum.Parse(typeof(enumType), value);
				return true;
			}
			catch (Exception e)
			{
				Debug.LogWarning(string.Format("[{0}] failed to parse value '{1}': {2}",
					typeof(enumType).Name,
					value,
					e.Message
				));

				result = (enumType)Enum.GetValues(typeof(enumType)).GetValue(0);
				return false;
			}
		}
		#endregion

		#region IComparable Extensions
		public static T Min<T>(params T[] values) where T : IComparable<T>
		{
			if (values.Length < 2)
			{
				throw new ArgumentException("Min must be called with at least two arguments.");
			}

			IComparable<T> minValue = values[0];

			for (long i = 1; i < values.LongLength; i++)
			{
				IComparable<T> value = values[i];

				if (value.CompareTo((T)minValue) < 0)
				{
					minValue = value;
				}
			}

			return (T)minValue;
		}
		#endregion

		#region Stopwatch Extensions
		public static void Restart(this System.Diagnostics.Stopwatch stopwatch)
		{
			stopwatch.Reset();
			stopwatch.Start();
		}
		#endregion

		#region UI_Control Extensions
		public static UI_Control uiControlCurrent(this BaseField field)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				return field.uiControlFlight;
			}
			else if (HighLogic.LoadedSceneIsEditor)
			{
				return field.uiControlEditor;
			}
			else
			{
				return null;
			}
		}
		#endregion

		public static bool SetIfDefault<T>(this T o, T val)
		{
			if (System.Object.Equals(o, default(T)))
			{
				o = val;
				return true;
			}

			return false;
		}
	}
}