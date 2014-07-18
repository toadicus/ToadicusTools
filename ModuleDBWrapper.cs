// ToadicusTools
//
// ModuleDBWrapper.cs
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
using System.Reflection;
using UnityEngine;

namespace ToadicusTools
{
	public class ModuleDB<T> : IModuleDB<T>
		where T : PartModule
	{
		public static IModuleDB<T> Instance
		{
			get
			{
				if (_instance == null && DBPresent)
				{
					_instance = new ModuleDB<T>();
				}

				return _instance;
			}
		}
		private static IModuleDB<T> _instance;

		public static bool DBPresent
		{
			get
			{
				return (realDBInstance != null);
			}
		}

		private static object realDBInstance
		{
			get
			{
				if (_realDBInstance != null)
				{
					#if DEBUG
					Debug.Log("[ModuleDBWrapper]: Returning cached instance.");
					#endif
					return _realDBInstance;
				}

				if (InstanceProperty != null)
				{
					_realDBInstance = InstanceProperty.GetValue(null, null);;

					#if DEBUG
					Debug.Log(string.Format("[ModuleDBWrapper]: Caching instance {0}.", _realDBInstance));
					#endif

					return _realDBInstance;
				}
				#if DEBUG
				else
				{
					Debug.Log("[ModuleDBWrapper]: InstanceProperty not set.");
				}
				#endif

				return null;
			}
		}

		public static bool TryGetInstance(out IModuleDB<T> instance)
		{
			if (DBPresent)
			{
				instance = null;
				return false;
			}
			else
			{
				instance = Instance;
				return true;
			}
		}

		private static PropertyInfo InstanceProperty
		{
			get
			{
				if (ModuleDBWrapper.moduleDBPresent)
				{
					if (genericDBType == null)
					{
						genericDBType = ModuleDBWrapper.ModuleDBType.MakeGenericType(typeof(T));
					}

					if (genericInstanceProperty == null)
					{
						genericInstanceProperty = genericDBType.GetProperty("Instance",
							BindingFlags.Public | BindingFlags.Static);

						#if DEBUG
						Debug.Log(string.Format("[ModuleDBWrapper]: Found genericInstanceProperty: {0}.",
							genericInstanceProperty));
						#endif
					}

					if (genericGetModules_Vessel == null)
					{
						genericGetModules_Vessel = genericDBType.GetMethod("getModules", new Type[] { typeof(Vessel) });
						#if DEBUG
						Debug.Log(string.Format("[ModuleDBWrapper]: Found genericGetModules_Vessel {0}.",
							genericGetModules_Vessel));
						#endif
					}

					if (genericGetModules_Part == null)
					{
						genericGetModules_Part = genericDBType.GetMethod("getModules", new Type[] { typeof(Part) });
						#if DEBUG
						Debug.Log(string.Format("[ModuleDBWrapper]: Found genericGetModules_Part {0}.",
							genericGetModules_Part));
						#endif
					}

					if (genericGetModules == null)
					{
						genericGetModules = genericDBType.GetMethod("getModules", new Type[] { typeof(System.Object) });
						#if DEBUG
						Debug.Log(string.Format("[ModuleDBWrapper]: Found genericGetModules {0}.",
							genericGetModules));
						#endif
					}

					if (genericInDeepCache == null)
					{
						genericInDeepCache = genericDBType.GetMethod("inDeepCache", new Type[] { typeof(System.Object) });
						#if DEBUG
						Debug.Log(string.Format("[ModuleDBWrapper]: Found genericInDeepCache {0}.",
							genericInDeepCache));
						#endif
					}

					if (genericInShallowCache == null)
					{
						genericInShallowCache = genericDBType.GetMethod("inShallowCache", new Type[] { typeof(System.Object) });
						#if DEBUG
						Debug.Log(string.Format("[ModuleDBWrapper]: Found genericInShallowCache {0}.",
							genericInShallowCache));
						#endif
					}
				}

				return genericInstanceProperty;
			}
		}

		private static Type genericDBType;
		private static PropertyInfo genericInstanceProperty;

		private static MethodInfo genericGetModules_Vessel;
		private static MethodInfo genericGetModules_Part;
		private static MethodInfo genericGetModules;
		private static MethodInfo genericInDeepCache;
		private static MethodInfo genericInShallowCache;

		private static System.Object _realDBInstance;

		/// <summary>
		/// Gets a flat list of all modules of type T in the given Vessel.  Returns an empty list if none exist.
		/// </summary>
		/// <returns>The list modules of type T</returns>
		/// <param name="vessel">The Vessel being queried</param>
		public List<T> getModules(Vessel vessel)
		{
			return genericGetModules.Invoke(_realDBInstance, new System.Object[] { vessel }) as List<T>;
		}

		/// <summary>
		/// Gets a flat list of all modules of type T in the given Part.  Returns an empty list if none exist.
		/// </summary>
		/// <returns>The list of modules of type T</returns>
		/// <param name="part">The Part being queried</param>
		public List<T> getModules(Part part)
		{
			return genericGetModules.Invoke(_realDBInstance, new System.Object[] { part }) as List<T>;
		}

		/// <summary>
		/// Returns true if the given Vessel exists in the deep cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Vessel exists in the deep cache, false otherwise</returns>
		/// <param name="vessel">The Vessel being queried</param>
		public bool inDeepCache(Vessel vessel)
		{
			return (bool)genericInDeepCache.Invoke(_realDBInstance, new System.Object[] { vessel });
		}

		/// <summary>
		/// Returns true if the given Part exists in the deep cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Part exists in the deep cache, false otherwise</returns>
		/// <param name="part">The Part being queried</param>
		public bool inDeepCache(Part part)
		{
			return (bool)genericInDeepCache.Invoke(_realDBInstance, new System.Object[] { part });
		}

		/// <summary>
		/// Returns true if the given Vessel exists in the shallow cache, false otherwise.
		/// </summary>
		/// <returns>true if the given Vessel exists in the shallow cache, false otherwise</returns>
		/// <param name="vessel">The Vessel being queried</param>
		public bool inShallowCache(Vessel vessel)
		{
			return (bool)genericInShallowCache.Invoke(_realDBInstance, new System.Object[] { vessel });
		}
	}

	internal class ModuleDBWrapper
	{
		public static bool moduleDBPresent
		{
			get
			{
				return (ModuleDBType != null);
			}
		}

		public static Type ModuleDBType
		{
			get
			{
				if (moduleDBType == null && runOnce)
				{
					runOnce = false;

					foreach (AssemblyLoader.LoadedAssembly assy in AssemblyLoader.loadedAssemblies)
					{
						#if DEBUG
						Tools.PostDebugMessage(null, "[ModuleDBWrapper] Checking assembly {0} version {1}",
							assy.assembly.GetName().Name,
							assy.assembly.GetName().Version
						);
						#endif

						foreach (Type type in assy.assembly.GetExportedTypes())
						{
							#if DEBUG
							if (type.Namespace == "ModuleDB")
							{
								Tools.PostDebugMessage(null, "[ModuleDBWrapper] Checking ModuleDB type {0}", type.FullName);
							}
							#endif

							if (type.FullName == "ModuleDB.ModuleDB`1")
							{
								System.Version assyVersion = assy.assembly.GetName().Version;
								if (assyVersion > loadedVersion)
								{
									#if DEBUG
									Tools.PostDebugMessage(null, "[ModuleDBWrapper] Found type {0}", type.FullName);
									#endif

									moduleDBType = type;
									loadedVersion = (System.Version)assyVersion.Clone();
								}
								#if DEBUG
								else
								{
									Tools.PostDebugMessage(null,
										"[ModuleDBWrapper] Discarded type {0} because" +
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
					if (moduleDBType == null)
					{
						Tools.PostDebugMessage(null, "[ModuleDBWrapper]: ModuleDB not found.");
					}
					else
					{
						Tools.PostDebugMessage(null, "[ModuleDBWrapper]: ModuleDB loaded from assembly version {0}.",
							loadedVersion);
					}
					#endif
				}

				return moduleDBType;
			}
		}

		static ModuleDBWrapper()
		{
			loadedVersion = new System.Version(0, 0, 0, 0);
			runOnce = true;
		}

		private static Type moduleDBType;
		private static System.Version loadedVersion;
		private static bool runOnce;
	}
}