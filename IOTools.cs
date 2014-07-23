// ToadicusTools
//
// TextureTools.cs
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
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ToadicusTools
{
	public static class IOTools
	{
		public static readonly string rootPath = KSPUtil.ApplicationRootPath;
		public static readonly char ds = Path.DirectorySeparatorChar;
		public static readonly string dirSepString = new string(ds, 1);
		public static readonly string gameDataPath = string.Format("{0}{1}GameData{1}", rootPath, ds);

		public static bool LoadTexture(
			out Texture2D texture,
			string path,
			int width,
			int height,
			TextureFormat format,
			bool mipmap,
			bool isGameDataRelative = true
		)
		{
			path.Replace("\\", dirSepString);
			path.Replace("/", dirSepString);

			if (isGameDataRelative)
			{
				path = string.Format("{0}{1}", gameDataPath, path);
			}

			if (File.Exists(path))
			{
				texture = new Texture2D(width, height, format, mipmap);

				if (texture.LoadImage(File.ReadAllBytes(path)))
				{
					return true;
				}
			}

			texture = null;
			return false;
		}

		public static bool LoadTexture(out Texture2D texture, string path, int width, int height)
		{
			return LoadTexture(out texture, path, width, height, TextureFormat.ARGB32, true, true);
		}
	}
}

