// ToadicusTools
//
// Tools.cs
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
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ToadicusTools
{
	public static class Tools
	{
		[Obsolete("Deprecated; use field from ToadicusTools.Text.SIFormatProvider instead")]
		#if HAS_SIFORMMATER
		public static readonly SIFormatProvider SIFormatter = SIFormatProvider.SIFormatter;
		#else
		public static readonly IFormatProvider SIFormatter = System.Globalization.CultureInfo.CurrentCulture;
		#endif

		#region Logging
		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostLogMessage(LogChannel channel, string msg)
		{
			Logging.PostLogMessage(channel, msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostLogMessage(LogChannel channel, string Format, params object[] args)
		{
			Logging.PostLogMessage(channel, Format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostLogMessage(string Msg)
		{
			Logging.PostLogMessage(Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostLogMessage(string Format, params object[] args)
		{
			Logging.PostLogMessage(Format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostWarningMessage(string Msg)
		{
			Logging.PostWarningMessage(Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostWarningMessage(string Format, params object[] args)
		{
			Logging.PostWarningMessage(Format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostErrorMessage(string Msg)
		{
			Logging.PostErrorMessage(Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostErrorMessage(string Format, params object[] args)
		{
			Logging.PostErrorMessage(Format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		[System.Diagnostics.Conditional("DEBUG")]
		public static void PostDebugMessage(string Msg)
		{
			Logging.PostDebugMessage(Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		[System.Diagnostics.Conditional("DEBUG")]
		public static void PostDebugMessage(object Sender, params object[] args)
		{
			Logging.PostDebugMessage(Sender, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		[System.Diagnostics.Conditional("DEBUG")]
		public static void PostDebugMessage(object Sender, string Format, params object[] args)
		{
			Logging.PostDebugMessage(Sender, Format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void PostMessageWithScreenMsg(string Msg)
		{
			Logging.PostMessageWithScreenMsg(Msg);
		}

		[Obsolete("Deprecated; use ToadicusTools.DebugLogger instead")]
		public class DebugLogger : IDisposable
		{
			public static DebugLogger New(object caller)
			{
				return new DebugLogger(caller.GetType());
			}

			public static DebugLogger New(Type callingType)
			{
				return new DebugLogger(callingType);
			}

			private StringBuilder stringBuilder;

			private DebugLogger() {}

			private DebugLogger(Type caller)
			{
				this.stringBuilder = GetStringBuilder();

				this.stringBuilder.Append(caller.Name);
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
					PostMessageWithScreenMsg(this.stringBuilder.ToString());
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
				PostMessageWithScreenMsg(this.stringBuilder.ToString());
			}
			[System.Diagnostics.Conditional("DEBUG")]
			public void Clear()
			{
				this.stringBuilder.Length = 0;
			}

			public void Dispose()
			{
				PutStringBuilder(this.stringBuilder);
			}

			~DebugLogger()
			{
				this.Dispose();
			}
		}
		#endregion

		#region ComponentExtensions
		[Obsolete("Deprecated; using method from Logging instead")]
		public static void Log(this Component component, LogChannel channel, string Msg)
		{
			Extensions.ComponentExtensions.Log(component, channel, Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void Log(this Component component, string Msg)
		{
			Extensions.ComponentExtensions.Log(component, Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void Log(this Component component, string format, params object[] args)
		{
			Extensions.ComponentExtensions.Log(component, format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void LogWarning(this Component component, string Msg)
		{
			Extensions.ComponentExtensions.LogWarning(component, Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void LogWarning(this Component component, string format, params object[] args)
		{
			Extensions.ComponentExtensions.LogWarning(component, format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void LogError(this Component component, string Msg)
		{
			Extensions.ComponentExtensions.LogError(component, Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		public static void LogError(this Component component, string format, params object[] args)
		{
			Extensions.ComponentExtensions.LogError(component, format, args);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		[System.Diagnostics.Conditional("DEBUG")]
		public static void LogDebug(this Component component, string Msg)
		{
			Extensions.ComponentExtensions.LogDebug(component, Msg);
		}

		[Obsolete("Deprecated; using method from Logging instead")]
		[System.Diagnostics.Conditional("DEBUG")]
		public static void LogDebug(this Component component, string format, params object[] args)
		{
			Extensions.ComponentExtensions.LogDebug(component, format, args);
		}
		#endregion

		#region PartModuleExtensions
		public static void DebugFieldsActivate(this PartModule partModule)
		{
			Extensions.PartModuleExtensions.DebugFieldsActivate(partModule);
		}
		#endregion

		#region PooledStringBuilder
		[Obsolete("Deprecated, use PooledStringBuilder instead")]
		public static StringBuilder GetStringBuilder()
		{
			return (StringBuilder)Text.PooledStringBuilder.Get();
		}

		[Obsolete("Deprecated, use PooledStringBuilder instead")]
		public static void PutStringBuilder(Text.PooledStringBuilder sb)
		{
			Text.PooledStringBuilder.Put(sb);
		}

		[Obsolete("Deprecated, use PooledStringBuilder instead")]
		public static void PutStringBuilder(StringBuilder sb) {}
		#endregion

		#region ArrayExtensions
		[Obsolete("Deprecated, use method from Extensions.ArrayExtensions instead")]
		public static bool Contains(this GameScenes[] haystack, GameScenes needle)
		{
			return Extensions.CollectionExtensions.Contains(haystack, needle);
		}

		[Obsolete("Deprecated, use method from Extensions.ArrayExtensions instead")]
		public static bool Contains(this CelestialBody[] haystack, CelestialBody needle)
		{
			return Extensions.CollectionExtensions.Contains(haystack, needle);
		}

		[Obsolete("Deprecated, use method from Extensions.ArrayExtensions instead")]
		public static bool Contains<T>(this T[] haystack, T needle)
		{
			return Extensions.CollectionExtensions.Contains(haystack, needle);
		}
		#endregion

		#region TextExtensions
		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this T[] array, string delimiter, Func<T, string> stringFunc)
		{
			return Text.Extensions.SPrint(array, delimiter, stringFunc);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this T[] array, Func<T, string> stringFunc, string delimiter = ", ")
		{
			return Text.Extensions.SPrint(array, delimiter, stringFunc);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this T[] array, string delimiter = ", ")
		{
			return Text.Extensions.SPrint(array, delimiter, null);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this IList<T> list, string delimiter, Func<T, string> stringFunc)
		{
			return Text.Extensions.SPrint(list, delimiter, stringFunc);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this List<T> list, string delimiter, Func<T, string> stringFunc)
		{
			return Text.Extensions.SPrint(list as IList<T>, delimiter, stringFunc);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this List<T> list, Func<T, string> stringFunc, string delimiter = ", ")
		{
			return Text.Extensions.SPrint(list, delimiter, stringFunc);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this IList<T> list, Func<T, string> stringFunc, string delimiter = ", ")
		{
			return Text.Extensions.SPrint(list, delimiter, stringFunc);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this List<T> list, string delimiter = ", ")
		{
			return Text.Extensions.SPrint(list, delimiter, null);
		}

		[Obsolete("Deprecated; use method from Text.TextExtensions instead")]
		public static string SPrint<T>(this IList<T> list, string delimiter = ", ")
		{
			return Text.Extensions.SPrint(list, delimiter, null);
		}
		#endregion

		#region Enum_Tools
		[Obsolete("Deprecated; use method from EnumTools instead")]
		public static bool TryParse<enumType>(string value, out enumType result)
			where enumType : struct, IConvertible, IComparable, IFormattable
		{
			return EnumTools.TryParse(value, out result);
		}
		#endregion

		#region IComparable Extensions
		[Obsolete("Deprecated; use method from MathTools instead")]
		public static T Min<T>(params T[] values) where T : IComparable<T>
		{
			return MathTools.Min(values);
		}
		#endregion

		#region Stopwatch Extensions
		[Obsolete("Deprecated; use method from ToadicusTools.Extensions instead")]
		public static void Restart(this System.Diagnostics.Stopwatch stopwatch)
		{
			Extensions.StopwatchExtensions.Restart(stopwatch);
		}
		#endregion

		#region UI_Control Extensions
		[Obsolete("Deprecated; use method from ToadicusTools.Extensions instead")]
		public static UI_Control uiControlCurrent(this BaseField field)
		{
			return Extensions.BaseFieldExtensions.uiControlCurrent(field);
		}
		#endregion

		#region ConfigNodeExtensions
		/// <summary>
		/// Gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="string"/>,
		/// or returns a given default value if the key does not exist or cannot be parsed.
		/// </summary>
		/// <returns>The value as a <see cref="string"/></returns>
		/// <param name="node">The <see cref="ConfigNode"/> being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static string GetValue(this ConfigNode node, string name, string defaultValue)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.GetValue(node, name, defaultValue);
		}

		/// <summary>
		/// Gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="double"/>,
		/// or returns a given default value if the key does not exist or cannot be parsed.
		/// </summary>
		/// <returns>The value as a <see cref="double"/></returns>
		/// <param name="node">The <see cref="ConfigNode"/> being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static double GetValue(this ConfigNode node, string name, double defaultValue)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.GetValue(node, name, defaultValue);
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="double"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The <see cref="ConfigNode"/>  being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static bool TryGetValue(this ConfigNode node, string name, out double value)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.TryGetValue(node, name, out value);
		}

		/// <summary>
		/// Gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="float"/>,
		/// or returns a given default value if the key does not exist or cannot be parsed.
		/// </summary>
		/// <returns>The value as a float</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static float GetValue(this ConfigNode node, string name, float defaultValue)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.GetValue(node, name, defaultValue);
		}

		/// <summary>
		/// Gets the value of key "name" in ConfigNode "node" as an int, or returns a given default value if the key
		/// does not exist or cannot be parsed to a double.
		/// </summary>
		/// <returns>The value as an int</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static int GetValue(this ConfigNode node, string name, int defaultValue)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.GetValue(node, name, defaultValue);
		}

		/// <summary>
		/// Gets the value of key "name" in ConfigNode "node" as a bool, or returns a given default value if the key
		/// does not exist or cannot be parsed to a bool.
		/// </summary>
		/// <returns>The value as a bool</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="defaultValue">The default value to return in fallback conditions</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static bool GetValue(this ConfigNode node, string name, bool defaultValue)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.GetValue(node, name, defaultValue);
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="float"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static bool TryGetValue(this ConfigNode node, string name, out float value)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.TryGetValue(node, name, out value);
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as an <see cref="int"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static bool TryGetValue(this ConfigNode node, string name, out int value)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.TryGetValue(node, name, out value);
		}

		/// <summary>
		/// Trys to gets the value of key "name" in <see cref="ConfigNode"/> "node" as a <see cref="bool"/>,
		/// placing it in the output value
		/// </summary>
		/// <returns><c>true</c>, if get value was retrieved successfully, <c>false</c> otherwise.</returns>
		/// <param name="node">The ConfigNode being referenced</param>
		/// <param name="name">The name of the key being referenced</param>
		/// <param name="value">The output value as parsed</param>
		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static bool TryGetValue(this ConfigNode node, string name, out bool value)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.TryGetValue(node, name, out value);
		}

		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static bool TryGetValue(this ConfigNode node, string name, out string value)
		{
			return ToadicusTools.Extensions.ConfigNodeExtensions.TryGetValue(node, name, out value);
		}

		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static void SafeSetValue(this ConfigNode node, string name, string value)
		{
			ToadicusTools.Extensions.ConfigNodeExtensions.SafeSetValue(node, name, value);
		}

		[Obsolete("Use methods from ToadicusTools.Extensions.ConfigNodeExtensions instead.")]
		public static void SafeSetValue(this ConfigNode node, string name, bool value)
		{
			ToadicusTools.Extensions.ConfigNodeExtensions.SafeSetValue(node, name, value);
		}
		#endregion

		#region WindowTools
		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Rect ClampRectToScreen(Rect window, int topMargin, int rgtMargin, int botMargin, int lftMargin)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampRectToScreen(window, topMargin, rgtMargin, botMargin, lftMargin);
		}

		// This implementation is adapted from FARGUIUtils.ClampToScreen
		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Rect ClampRectToScreen(Rect window, int xMargin, int yMargin)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampRectToScreen(window, yMargin, xMargin, yMargin, xMargin);
		}

		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Rect ClampRectToScreen(Rect window, int Margin)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampRectToScreen(window, Margin, Margin);
		}

		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Rect ClampRectToScreen(Rect window)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampRectToScreen(window, 30);
		}

		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Rect ClampRectToEditorPad(Rect window)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampRectToEditorPad(window);
		}

		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Vector2 ClampV2ToScreen(Vector2 vec, uint xMargin, uint yMargin)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampV2ToScreen(vec, xMargin, yMargin);
		}

		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Vector2 ClampV2ToScreen(Vector2 vec, uint Margin)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampV2ToScreen(vec, Margin, Margin);
		}

		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Vector2 ClampV2ToScreen(Vector2 vec)
		{
			return ToadicusTools.GUIUtils.WindowTools.ClampV2ToScreen(vec, 15);
		}

		// UNDONE: This seems messy.  Can we clean it up?
		[Obsolete("Use method from ToadicusTools.GUIUtils instead")]
		public static Rect DockToWindow(Rect icon, Rect window)
		{
			return ToadicusTools.GUIUtils.WindowTools.DockToWindow(icon, window);
		}
		#endregion

		#region MuMech Tools
		// Derived from MechJeb2/VesselState.cs
		[Obsolete("Use method from ToadicusTools.MuMechTools instead")]
		public static Quaternion getSurfaceRotation(this Vessel vessel)
		{
			return MuMechTools.MuMechTools.getSurfaceRotation(vessel);
		}

		// Derived from MechJeb2/VesselState.cs
		[Obsolete("Use method from ToadicusTools.MuMechTools instead")]
		public static double getSurfaceHeading(this Vessel vessel)
		{
			return MuMechTools.MuMechTools.getSurfaceHeading(vessel);
		}

		// Derived from MechJeb2/VesselState.cs
		[Obsolete("Use method from ToadicusTools.MuMechTools instead")]
		public static double getSurfacePitch(this Vessel vessel)
		{
			return MuMechTools.MuMechTools.getSurfacePitch(vessel);
		}

		// Derived from MechJeb2/MuUtils.cs
		[Obsolete("Use method from ToadicusTools.MuMechTools instead")]
		public static string MuMech_ToSI(
			double d, int digits = 3, int MinMagnitude = 0, int MaxMagnitude = int.MaxValue
		)
		{
			return MuMechTools.MuMechTools.MuMech_ToSI(d, digits, MinMagnitude, MaxMagnitude);
		}
		#endregion

		#region PartExtensions
		[Obsolete("Deprecated, please use hasModuleByType from ToadicusTools.Extensions")]
		public static bool hasModuleType<T>(this Part part) where T : PartModule
		{
			return Extensions.PartExtensions.hasModuleType<T>(part);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool hasModuleByType<T>(this Part part) where T : PartModule
		{
			return Extensions.PartExtensions.hasModuleByType<T>(part);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool hasModuleByType(this Part part, Type type)
		{
			return Extensions.PartExtensions.hasModuleByType(part, type);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool hasModuleByName(this Part part, string moduleName)
		{
			return Extensions.PartExtensions.hasModuleByName(part, moduleName);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static List<T> getModulesOfType<T>(this Part part) where T : PartModule
		{
			return Extensions.PartExtensions.getModulesOfType<T>(part);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static T getFirstModuleOfType<T>(this Part part) where T: PartModule
		{
			return Extensions.PartExtensions.getFirstModuleOfType<T>(part);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool tryGetFirstModuleOfType<T>(this Part part, out T module) where T : PartModule
		{
			return Extensions.PartExtensions.tryGetFirstModuleOfType<T>(part, out module);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static PartModule getFirstModuleByName(this Part part, string moduleName)
		{
			return Extensions.PartExtensions.getFirstModuleByName(part, moduleName);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool tryGetFirstModuleByName(this Part part, string moduleName, out PartModule module)
		{
			return Extensions.PartExtensions.tryGetFirstModuleByName(part, moduleName, out module);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool hasAncestorPart(this Part part, Part checkPart)
		{
			return Extensions.PartExtensions.hasAncestorPart(part, checkPart);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool isDecoupler(this Part part)
		{
			return Extensions.PartExtensions.isDecoupler(part);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool isDockingNode(this Part part)
		{
			return Extensions.PartExtensions.isDockingNode(part);
		}

		[Obsolete("Deprecated, please use method from ToadicusTools.Extensions")]
		public static bool isInStagingList(this Part part)
		{
			return Extensions.PartExtensions.isInStagingList(part);
		}
		#endregion

		#region VesselExtensions
		/// <summary>
		/// Returns the distance between this Vessel and another Vessel.
		/// </summary>
		/// <param name="vesselOne">This <see cref="Vessel"/><see ></param>
		/// <param name="vesselTwo">Another <see cref="Vessel"/></param>
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static double DistanceTo(this Vessel vesselOne, Vessel vesselTwo)
		{
			return Extensions.VesselExtensions.DistanceTo(vesselOne, vesselTwo);
		}

		/// <summary>
		/// Returns the distance between this Vessel and a CelestialBody
		/// </summary>
		/// <param name="vessel">This Vessel</param>
		/// <param name="body">A <see cref="CelestialBody"/></param>
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static double DistanceTo(this Vessel vessel, CelestialBody body)
		{
			return Extensions.VesselExtensions.DistanceTo(vessel, body);
		}

		/// <summary>
		/// Returns the square of the distance between this Vessel and another Vessel.
		/// </summary>
		/// <param name="vesselOne">This <see cref="Vessel"/><see ></param>
		/// <param name="vesselTwo">Another <see cref="Vessel"/></param>
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static double sqrDistanceTo(this Vessel vesselOne, Vessel vesselTwo)
		{
			return Extensions.VesselExtensions.sqrDistanceTo(vesselOne, vesselTwo);
		}

		/// <summary>
		/// Returns the square of the distance between this Vessel and a CelestialBody
		/// </summary>
		/// <param name="vessel">This Vessel</param>
		/// <param name="body">A <see cref="CelestialBody"/></param>
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static double sqrDistanceTo(this Vessel vessel, CelestialBody body)
		{
			return Extensions.VesselExtensions.sqrDistanceTo(vessel, body);
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
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			Vessel targetVessel,
			out CelestialBody firstOccludingBody,
			double sqrRatio = 1d
		)
		{
			return Extensions.VesselExtensions.hasLineOfSightTo(vessel, targetVessel, out firstOccludingBody, sqrRatio);
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
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			CelestialBody targetBody,
			out CelestialBody firstOccludingBody,
			double sqrRatio = 1d
		)
		{
			return Extensions.VesselExtensions.hasLineOfSightTo(vessel, targetBody, out firstOccludingBody, sqrRatio);
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target Vessel from this Vessel, false otherwise.
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="targetVessel">target Vessel</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		[Obsolete("This overload has been removed", true)]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			Vessel targetVessel,
			double sqrRatio = 1d
		)
		{
			throw new NotImplementedException("This overload has been removed.");
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target point from this Vessel, false otherwise.
		/// Includes a 5% "fudge factor".
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="distantPoint">target point</param>
		/// <param name="firstOccludingBody">Set to the first body found to be blocking line of sight,
		/// if any, otherwise null.</param>
		/// <param name="excludedBody">CelestialBody to exclude from the LOS check</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		[Obsolete("This overload has been removed", true)]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			Vector3d distantPoint,
			out CelestialBody firstOccludingBody,
			CelestialBody excludedBody,
			double sqrRatio = 1d
		)
		{
			throw new NotImplementedException("This overload has been removed.");
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target point from this Vessel, false otherwise.
		/// Includes a 5% "fudge factor".
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="distantPoint">target point</param>
		/// <param name="firstOccludingBody">Set to the first body found to be blocking line of sight,
		/// if any, otherwise null.</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		[Obsolete("This overload has been removed", true)]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			Vector3d distantPoint,
			out CelestialBody firstOccludingBody,
			double sqrRatio = 1d
		)
		{
			throw new NotImplementedException("This overload has been removed.");
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target CelestialBody from this Vessel, false otherwise.
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="targetBody">target CelestialBody</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		[Obsolete("This overload has been removed", true)]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			CelestialBody targetBody,
			double sqrRatio = 1d
		)
		{
			throw new NotImplementedException("This overload has been removed.");
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target point from this Vessel, false otherwise.
		/// Includes a 5% "fudge factor".
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="distantPoint">target point</param>
		/// <param name="firstOccludingBody">Set to the first body found to be blocking line of sight,
		/// if any, otherwise null.</param>
		/// <param name="excludedBody">Array of CelestialBodies to exclude from the LOS check</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		[Obsolete("This overload has been removed", true)]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			Vector3d distantPoint,
			out CelestialBody firstOccludingBody,
			CelestialBody[] excludedBodies = null,
			double sqrRatio = 1d
		)
		{
			throw new NotImplementedException("This overload has been removed.");
		}

		/// <summary>
		/// Returns true if no CelestialBody occludes the target point from this Vessel, false otherwise.
		/// Includes a 5% "fudge factor".
		/// </summary>
		/// <returns><c>true</c>, if this Vessel has line of sight to the target Vessel, <c>false</c> otherwise.</returns>
		/// <param name="vessel">this Vessel</param>
		/// <param name="distantPoint">target point</param>
		/// <param name="excludedBody">Array of CelestialBodies to exclude from the LOS check</param>
		/// <param name="sqrRatio">The square of the "grace" ratio to apply
		/// to the radius of potentially excluding bodies.</param>
		[Obsolete("This overload has been removed", true)]
		public static bool hasLineOfSightTo(
			this Vessel vessel,
			Vector3d distantPoint,
			CelestialBody[] excludedBodies = null,
			double sqrRatio = 1d
		)
		{
			throw new NotImplementedException("This overload has been removed.");
		}

		/// <summary>
		/// Checks if this vessel has a properly-crewed manned command pod.
		/// </summary>
		/// <returns><c>true</c>, if this vessel is adequately crewed for control, <c>false</c> otherwise.</returns>
		/// <param name="vessel"></param>
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static bool hasCrewCommand(this Vessel vessel)
		{
			return Extensions.VesselExtensions.hasCrewCommand(vessel);
		}

		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static bool hasProbeCommand(this Vessel vessel)
		{
			return Extensions.VesselExtensions.hasProbeCommand(vessel);
		}

		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static VesselCommand CurrentCommand(this Vessel vessel)
		{
			return (VesselCommand)Extensions.VesselExtensions.CurrentCommand(vessel);
		}

		/// <summary>
		/// Gets a list of PartModules of type T within this vessel.
		/// </summary>
		/// <returns>a list of PartModules of type T within this vessel, or an empty list if none</returns>
		/// <param name="vessel"></param>
		/// <typeparam name="T">PartModule type paramter</typeparam>
		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static IList<T> getModulesOfType<T>(this Vessel vessel) where T : PartModule
		{
			return Extensions.VesselExtensions.getModulesOfType<T>(vessel);
		}

		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static bool tryGetFirstModuleOfType<T>(this Vessel vessel, out T module) where T : PartModule
		{
			return Extensions.VesselExtensions.tryGetFirstModuleOfType<T>(vessel, out module);
		}

		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static T getFirstModuleOfType<T>(this Vessel vessel) where T : PartModule
		{
			return Extensions.VesselExtensions.getFirstModuleOfType<T>(vessel);
		}

		[Obsolete("Deprecated, please use module from Extensions.VesselExtensions instead")]
		public static bool hasModuleOfType<T>(this Vessel vessel) where T : PartModule
		{
			return Extensions.VesselExtensions.hasModuleOfType<T>(vessel);
		}
		#endregion

		#region Math
		/// <summary>
		/// <para>Exponentiation function optimized for arbitrary integer exponents.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		[Obsolete("Deprecated, please use module from MathTools.Pow instead")]
		public static double Pow(double b, int n)
		{
			return MathTools.Pow(b, n);
		}

		/// <summary>
		/// <para>Exponentiation function which uses an integer-optimized algorithm when possible,
		/// or falls back to <see cref="Math.Pow"/>.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		[Obsolete("Deprecated, please use module from MathTools.Pow instead")]
		public static double Pow(double b, double n)
		{
			return MathTools.Pow(b, n);
		}

		/// <summary>
		/// <para>Exponentiation function optimized for arbitrary integer exponents
		/// using single-precision floats.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		[Obsolete("Deprecated, please use module from MathTools.Pow instead")]
		public static float Pow(float b, int n)
		{
			return MathTools.Pow(b, n);
		}

		/// <summary>
		/// <para>Exponentiation function which uses an integer-optimized algorithm when possible,
		/// or falls back to <see cref="Math.Pow"/>.  Uses single-precision floats in the integer case.</para>
		/// <para>Returns b to the nth power</para>
		/// </summary>
		/// <param name="b">The base of the exponential</param>
		/// <param name="n">The exponent</param>
		[Obsolete("Deprecated, please use module from MathTools.Pow instead")]
		public static float Pow(float b, float n)
		{
			return MathTools.Pow(b, n);
		}
		#endregion

		#region Misc
		// TODO: Find this guy a home
		public static bool SetIfDefault<T>(this T o, T val)
		{
			if (System.Object.Equals(o, default(T)))
			{
				o = val;
				return true;
			}

			return false;
		}
		
		
		public static Part GetSceneRootPart()
		{
			Part rootPart;
			switch (HighLogic.LoadedScene)
			{
				case GameScenes.EDITOR:
					rootPart = EditorLogic.RootPart;
					break;
				case GameScenes.FLIGHT:
					rootPart = FlightGlobals.ActiveVessel != null ? FlightGlobals.ActiveVessel.rootPart : null;
					break;
				default:
					rootPart = null;
					break;
			}

			return rootPart;
		}
		#endregion
	}

	public enum VesselCommand
	{
		None = 0,
		Probe = 1,
		Crew = 2
	}
}