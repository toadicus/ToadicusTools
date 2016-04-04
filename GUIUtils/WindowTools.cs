// ToadicusTools
//
// WindowTools.cs
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

using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToadicusTools.GUIUtils
{
	public static class WindowTools
	{
		public static Rect ClampRectToScreen(Rect window, int topMargin, int rgtMargin, int botMargin, int lftMargin)
		{
			window.x = Mathf.Clamp(window.x, lftMargin - window.width, Screen.width - rgtMargin);
			window.y = Mathf.Clamp(window.y, topMargin - window.height, Screen.height - botMargin);

			return window;
		}
		// This implementation is adapted from FARGUIUtils.ClampToScreen
		public static Rect ClampRectToScreen(Rect window, int xMargin, int yMargin)
		{
			return ClampRectToScreen(window, yMargin, xMargin, yMargin, xMargin);
		}

		public static Rect ClampRectToScreen(Rect window, int Margin)
		{
			return ClampRectToScreen(window, Margin, Margin);
		}

		public static Rect ClampRectToScreen(Rect window)
		{
			return ClampRectToScreen(window, 30);
		}

		public static Rect ClampRectToEditorPad(Rect window)
		{
			float lftMargin = window.width;

			// TODO: Find this stuff
			switch (EditorLogic.fetch.editorScreen)
			{
				case EditorScreen.Actions:
					// lftMargin += EditorPanels.Instance.panelManager;
					break;
				case EditorScreen.Crew:
					// lftMargin += EditorPanels.Instance.crewPanelWidth;
					break;
				case EditorScreen.Parts:
				default:
					// lftMargin += EditorPanels.Instance.partsPanelWidth;
					break;
			}

			return ClampRectToScreen(window, 30, 30, 30, (int)lftMargin);
		}

		public static Vector2 ClampV2ToScreen(Vector2 vec, uint xMargin, uint yMargin)
		{
			vec.x = Mathf.Clamp(vec.x, xMargin, Screen.width - xMargin);
			vec.y = Mathf.Clamp(vec.y, yMargin, Screen.height - yMargin);

			return vec;
		}

		public static Vector2 ClampV2ToScreen(Vector2 vec, uint Margin)
		{
			return ClampV2ToScreen(vec, Margin, Margin);
		}

		public static Vector2 ClampV2ToScreen(Vector2 vec)
		{
			return ClampV2ToScreen(vec, 15);
		}

		// UNDONE: This seems messy.  Can we clean it up?
		public static Rect DockToWindow(Rect icon, Rect window)
		{
			// We can't set the x and y of the center point directly, so build a new vector.
			Vector2 center = new Vector2();

			// If we are near the top or bottom of the screen...
			if (window.yMax > Screen.height - icon.height ||
				window.yMin < icon.height)
			{
				// If we are in a corner...
				if (window.xMax > Screen.width - icon.width ||
					window.xMin < icon.width)
				{
					// If it is a top corner, put the icon below the window.
					if (window.yMax < Screen.height / 2)
					{
						center.y = window.yMax + icon.height / 2;
					}
					// If it is a bottom corner, put the icon above the window.
					else
					{
						center.y = window.yMin - icon.height / 2;
					}
				}
				// If we are not in a corner...
				else
				{
					// If we are along the top edge, align the icon's top edge with the top edge of the window
					if (window.yMax > Screen.height / 2)
					{
						center.y = window.yMax - icon.height / 2;
					}
					// If we are along the bottom edge, align the icon's bottom edge with the bottom edge of the window
					else
					{
						center.y = window.yMin + icon.height / 2;
					}
				}

				// At the top or bottom, if we are towards the right, put the icon to the right of the window
				if (window.center.x < Screen.width / 2)
				{
					center.x = window.xMin - icon.width / 2;
				}
				// At the top or bottom, if we are towards the left, put the icon to the left of the window
				else
				{
					center.x = window.xMax + icon.width / 2;
				}

			}
			// If we are not along the top or bottom of the screen...
			else
			{
				// By default, center the icon above the window
				center.y = window.yMin - icon.height / 2;
				center.x = window.center.x;

				// If we are along a side...
				if (window.xMax > Screen.width - icon.width ||
					window.xMin < icon.width)
				{
					// UNDONE: I'm not sure I like the feel of this part.
					// If we are along a side towards the bottom, put the icon below the window
					if (window.center.y > Screen.height / 2)
					{
						center.y = window.yMax + icon.height / 2;
					}

					// Along the left side, align the left edge of the icon with the left edge of the window.
					if (window.xMax > Screen.width - icon.width)
					{
						center.x = window.xMax - icon.width / 2;
					}
					// Along the right side, align the right edge of the icon with the right edge of the window.
					else if (window.xMin < icon.width)
					{
						center.x = window.xMin + icon.width / 2;
					}
				}
			}

			// Assign the vector to the center of the rect.
			icon.center = center;

			// Return the icon's position.
			return icon;
		}
	}
}