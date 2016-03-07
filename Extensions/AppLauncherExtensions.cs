// ToadicusTools
//
// AppLauncherTools.cs
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

using KSP;
using KSP.UI.Screens;
using System;
using UnityEngine;

namespace ToadicusTools.Extensions
{
	public static class AppLauncherExtensions
	{
		private static void Dummy() {}

		/*
		 * Squad's method signature for reference.
		 * public static ApplicationLauncherButton AddModApplication(this ApplicationLauncher appLauncher,
			RUIToggleButton.OnTrue onTrue,
			RUIToggleButton.OnFalse onFalse,
			RUIToggleButton.OnHover onHover,
			RUIToggleButton.OnHoverOut onHoverOut,
			RUIToggleButton.OnEnable onEnable,
			RUIToggleButton.OnDisable onDisable,
			ApplicationLauncher.AppScenes visibleInScenes,
			Texture texture
			)
		*/

		public static ApplicationLauncherButton AddModApplication(this ApplicationLauncher appLauncher,
			Callback onTrue,
			Callback onFalse,
			Callback onHover,
			Callback onHoverOut,
			ApplicationLauncher.AppScenes visibleInScenes,
			Texture texture
		)
		{
			return appLauncher.AddModApplication(
				onTrue, onFalse,
				onHover, onHoverOut,
				Dummy, Dummy,
				visibleInScenes,
				texture
			);
		}

		public static ApplicationLauncherButton AddModApplication(this ApplicationLauncher appLauncher,
			Callback onTrue,
			Callback onFalse,
			ApplicationLauncher.AppScenes visibleInScenes,
			Texture texture
		)
		{
			return appLauncher.AddModApplication(
				onTrue, onFalse,
				Dummy, Dummy,
				Dummy, Dummy,
				visibleInScenes,
				texture
			);
		}

		public static ApplicationLauncherButton AddModApplication(this ApplicationLauncher appLauncher,
			ApplicationLauncher.AppScenes visibleInScenes,
			Texture texture
		)
		{
			return appLauncher.AddModApplication(
				Dummy, Dummy,
				Dummy, Dummy,
				Dummy, Dummy,
				visibleInScenes,
				texture
			);
		}

		public static ApplicationLauncher.AppScenes ToAppScenes(this GameScenes gameScene)
		{
			/*
			 * 	NEVER = 0,
			 *	ALWAYS = -1,
			 *	SPACECENTER = 1,
			 *	FLIGHT = 2,
			 *	MAPVIEW = 4,
			 *	VAB = 8,
			 *	SPH = 16,
			 *	TRACKSTATION = 32
			 */

			switch (gameScene)
			{
				case GameScenes.EDITOR:
					return ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH;
				case GameScenes.FLIGHT:
					return ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW;
				case GameScenes.SPACECENTER:
					return ApplicationLauncher.AppScenes.SPACECENTER;
				case GameScenes.TRACKSTATION:
					return ApplicationLauncher.AppScenes.TRACKSTATION;
				case GameScenes.MAINMENU:
				case GameScenes.CREDITS:
				case GameScenes.LOADING:
				case GameScenes.LOADINGBUFFER:
				case GameScenes.PSYSTEM:
				case GameScenes.SETTINGS:
				default:
					Logging.PostLogMessage(LogChannel.Warning,
						"Cannot convert GameScenes.{0}: no acceptable AppScenes analog.",
						Enum.GetName(typeof(GameScenes), gameScene)
					);
					
					return ApplicationLauncher.AppScenes.NEVER;
			}
		}
	}
}

