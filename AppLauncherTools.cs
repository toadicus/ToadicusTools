// ToadicusTools
//
// AppLauncherTools.cs
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

using KSP;
using System;
using UnityEngine;

namespace ToadicusTools
{
	public static class AppLauncherTools
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
			RUIToggleButton.OnTrue onTrue,
			RUIToggleButton.OnFalse onFalse,
			RUIToggleButton.OnHover onHover,
			RUIToggleButton.OnHoverOut onHoverOut,
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
			RUIToggleButton.OnTrue onTrue,
			RUIToggleButton.OnFalse onFalse,
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
			RUIToggleButton.OnTrue onTrue,
			RUIToggleButton.OnFalse onFalse,
			RUIToggleButton.OnEnable onEnable,
			RUIToggleButton.OnDisable onDisable,
			ApplicationLauncher.AppScenes visibleInScenes,
			Texture texture
		)
		{
			return appLauncher.AddModApplication(
				onTrue, onFalse,
				Dummy, Dummy,
				onEnable, onDisable,
				visibleInScenes,
				texture
			);
		}
	}
}

