// ToadicusTools © 2014 toadicus
//
// This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/.
//
// This software uses the methods derived from Ferram Aeropace Research, © 2013-2014 Michael Ferrara AKA ferram4
// Used under a Creative Commons Attribution-ShareAlike 3.0 Unported License.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToadicusTools
{
	public static partial class Tools
	{
		// This implementation is adapted from FARGUIUtils.ClampToScreen
		public static Rect ClampRectToScreen(Rect window, int xMargin, int yMargin)
		{
			window.x = Mathf.Clamp(window.x, xMargin - window.width, Screen.width - xMargin);
			window.y = Mathf.Clamp(window.y, yMargin - window.height, Screen.height - yMargin);

			return window;
		}

		public static Rect ClampRectToScreen(Rect window, int Margin)
		{
			return ClampRectToScreen(window, Margin, Margin);
		}

		public static Rect ClampRectToScreen(Rect window)
		{
			return ClampRectToScreen(window, 30);
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