// ToadicusTools
//
// PrefabDBWrapper.cs
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
using System.Reflection;
using UnityEngine;

namespace ToadicusTools
{
	public class PrefabPartDB
	{
		public static bool prefabDBPresent
		{
			get
			{
				return (Instance != null);
			}
		}

		public static IPrefabPartDB Instance
		{
			get
			{
				if (PrefabDBType != null)
				{
					if (instanceProperty == null)
					{
						instanceProperty = PrefabDBType.GetProperty("Instance",
							BindingFlags.Public | BindingFlags.Static);
					}

					try
					{
						return instanceProperty.GetValue(null, null) as IPrefabPartDB;
					}
					catch (System.InvalidCastException ex)
					{
						Debug.LogError(string.Format("[PrefabDBWrapper]: Cast failed in Instance call." +
							"\n\tInstanceProperty: {0}" +
							"\n\tInstanceProperty.GetValue(null, null): {1}" +
							"\n\tInstanceProperty.GetValue(null, null).GetType().Name: {2}" +
							"\n\ttypeof(IPrefabPartDB).Name: {3}",
							instanceProperty,
							instanceProperty.GetValue(null, null),
							instanceProperty.GetValue(null, null).GetType().Name,
							typeof(IPrefabPartDB).Name
						));

						Debug.LogException(ex);
					}
				}

				return null;
			}
		}

		public static bool TryGetInstance(out IPrefabPartDB instance)
		{
			if (prefabDBPresent)
			{
				instance = Instance;
				return true;
			}
			else
			{
				instance = null;
				return false;
			}
		}

		private static Type PrefabDBType
		{
			get
			{
				if (prefabDBType == null && runOnce)
				{
					runOnce = false;

					foreach (AssemblyLoader.LoadedAssembly assy in AssemblyLoader.loadedAssemblies)
					{
						#if DEBUG
						Tools.PostDebugMessage(null, "[PrefabDBWrapper] Checking assembly {0} version {1}",
							assy.assembly.GetName().Name,
							assy.assembly.GetName().Version
						);
						#endif

						foreach (Type type in assy.assembly.GetExportedTypes())
						{
							#if DEBUG
							if (type.Namespace == "ModuleDB")
							{
								Tools.PostDebugMessage(null, "[PrefabDBWrapper] Checking ModuleDB type {0}", type.FullName);
							}
							#endif

							if (type.FullName == "ModuleDB.PrefabPartDB")
							{
								System.Version assyVersion = assy.assembly.GetName().Version;
								if (assyVersion > loadedVersion)
								{
									#if DEBUG
									Tools.PostDebugMessage(null, "[PrefabDBWrapper] Found type {0}", type.FullName);
									#endif

									prefabDBType = type;
									loadedVersion = (System.Version)assyVersion.Clone();
								}
								#if DEBUG
								else
								{
									Tools.PostDebugMessage(null,
										"[PrefabDBWrapper] Discarded type {0} because" +
										" assembly version {1}is not greather than loaded version {2}",
										type.FullName,
										assyVersion, loadedVersion
									);
								}
								#endif
							}
						}
					}

					#if DEBUG
					if (prefabDBType == null)
					{
						Tools.PostDebugMessage(null, "[PrefabDBWrapper]: ModuleDB not found.");
					}
					else
					{
						Tools.PostDebugMessage(null, "[PrefabDBWrapper]: ModuleDB loaded from assembly version {0}.",
							loadedVersion);
					}
					#endif
				}

				return prefabDBType;
			}
		}

		static PrefabPartDB()
		{
			loadedVersion = new System.Version(0, 0, 0, 0);
			runOnce = true;
		}

		private static Type prefabDBType;
		private static PropertyInfo instanceProperty;
		private static System.Version loadedVersion;

		private static bool runOnce;
	}
}