// ToadicusTools
//
// Layout.cs
//
// Copyright © 2015, toadicus
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
using UnityEngine;

namespace ToadicusTools.GUIUtils
{
	public static class Layout
	{
		public static bool Toggle(
			bool value,
			GUIContent content,
			bool expandWidth,
			GUIStyle toggleStyle,
			GUIStyle labelStyle
		)
		{
			if (toggleStyle == null)
			{
				toggleStyle = GUI.skin.toggle;
			}

			if (labelStyle == null)
			{
				labelStyle = GUI.skin.label;
			}

			GUILayout.BeginHorizontal(GUILayout.ExpandWidth(expandWidth));

			value = GUILayout.Toggle(value, GUIContent.none, GUILayout.ExpandWidth(false));

			GUILayout.Label(content, GUILayout.ExpandWidth(expandWidth));

			GUILayout.EndHorizontal();

			return value;
		}

		public static bool Toggle(
			bool value,
			string text,
			bool expandWidth,
			GUIStyle toggleStyle,
			GUIStyle labelStyle
		)
		{
			return Toggle(value, new GUIContent(text), expandWidth, toggleStyle, toggleStyle);
		}

		public static bool Toggle(
			bool value,
			GUIContent content,
			GUIStyle toggleStyle,
			GUIStyle labelStyle
		)
		{
			return Toggle(value, content, true, toggleStyle, toggleStyle);
		}

		public static bool Toggle(
			bool value,
			string text,
			GUIStyle toggleStyle,
			GUIStyle labelStyle
		)
		{
			return Toggle(value, new GUIContent(text), true, toggleStyle, toggleStyle);
		}

		public static bool Toggle(bool value, GUIContent content, bool expandWidth, GUIStyle labelStyle)
		{
			return Toggle(value, content, expandWidth, null, labelStyle);
		}

		public static bool Toggle(bool value, string text, bool expandWidth, GUIStyle labelStyle)
		{
			return Toggle(value, new GUIContent(text), expandWidth, labelStyle);
		}

		public static bool Toggle(bool value, GUIContent content, GUIStyle labelStyle)
		{
			return Toggle(value, content, true, null, labelStyle);
		}

		public static bool Toggle(bool value, string text, GUIStyle labelStyle)
		{
			return Toggle(value, new GUIContent(text), true, labelStyle);
		}

		public static bool Toggle(bool value, GUIContent content, bool expandWidth)
		{
			return Toggle(value, content, expandWidth, null, null);
		}

		public static bool Toggle(bool value, string text, bool expandWidth)
		{
			return Toggle(value, new GUIContent(text), expandWidth);
		}

		public static bool Toggle(bool value, GUIContent content)
		{
			return Toggle(value, content, true, null, null);
		}

		public static bool Toggle(bool value, string text)
		{
			return Toggle(value, new GUIContent(text), true);
		}
	}
}

