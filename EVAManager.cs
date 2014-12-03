// TweakableEVA, a TweakableEverything module
//
// TweakableEVAManager.cs
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
// 3. Neither the name of the copyright holder nor the names of its contributors may be used
//    to endorse or promote products derived from this software without specific prior written permission.
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
using System.Linq;
using ToadicusTools;
using UnityEngine;

namespace EVAManager
{
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class EVAManager : MonoBehaviour
	{
		private bool runOnce = true;

		public void Update()
		{
			if (runOnce && PartLoader.Instance.IsReady() && PartResourceLibrary.Instance != null)
			{
				Tools.PostDebugMessage(this, "Looking for kerbalEVA.");
				{
					foreach (var loadedPart in PartLoader.LoadedPartsList)
					{
						if (loadedPart.name.ToLower() == "kerbaleva")
						{
							Tools.PostDebugMessage(this, "Found {0}", loadedPart.name);

							Part evaPart = loadedPart.partPrefab;

							#if DEBUG
							var log = Tools.DebugLogger.New(this);

							log.AppendLine("Modules before run:");

							foreach (var m in evaPart.GetComponents<PartModule>())
							{
								log.Append('\t');
								log.Append(m.GetType().Name);
								log.Append('\n');
							}

							log.AppendLine("Resources before run:");

							foreach (var r in evaPart.GetComponents<PartResource>())
							{
								log.Append('\t');
								log.AppendFormat("Name: {0}, amount: {1}, maxAmount: {2}",
									r.resourceName, r.amount, r.maxAmount);
								log.Append('\n');
							}

							log.Print();
							#endif

							ConfigNode[] evaModuleNodes = GameDatabase.Instance.GetConfigNodes("EVA_MODULE");

							Tools.PostDebugMessage(this, "Checking {0} EVA_MODULE nodes.", evaModuleNodes.Length);

							foreach (ConfigNode evaModuleNode in evaModuleNodes)
							{
								string moduleName;

								if (evaModuleNode.TryGetValue("name", out moduleName))
								{
									if (evaPart.GetComponents<PartModule>().Any(m => m.GetType().Name == moduleName))
									{
										Debug.LogWarning(string.Format(
											"[{0}]: Skipping module {1}: already present in kerbalEVA",
											this.GetType().Name,
											moduleName
										));
										continue;
									}

									Type moduleClass = AssemblyLoader.GetClassByName(typeof(PartModule), moduleName);

									if (moduleClass == null)
									{
										Debug.LogWarning(string.Format(
											"[{0}]: Skipping module {1}: class not found in loaded assemblies.",
											this.GetType().Name,
											moduleName
										));
										continue;
									}
									
									try
									{
										PartModule evaModule = evaPart.gameObject.AddComponent(moduleClass)
											as PartModule;

										var awakeMethod = typeof(PartModule).GetMethod("Awake",
											System.Reflection.BindingFlags.NonPublic |
											System.Reflection.BindingFlags.Instance
										);

										awakeMethod.Invoke(evaModule, new object[] {});

										evaModule.Load(evaModuleNode);
									}
									catch (Exception ex)
									{
										Debug.Log(string.Format(
											"TweakableEVAManager handled exception {0} while adding modules to kerbalEVA.",
											ex.GetType().Name
										));

										#if DEBUG
										Debug.LogException(ex);
										#endif
									}

									if (evaPart.GetComponents<PartModule>().Any(m => m.GetType().Name == moduleName))
									{
										Debug.Log(string.Format("EVAManager added {0} to kerbalEVA part.", moduleName));
									}
									else
									{
										Debug.LogWarning(string.Format(
											"EVAManager failed to add {0} to kerbalEVA part.",
											moduleName
										));
									}
								}
								else
								{
									Debug.Log(string.Format(
										"[{0}]: Skipping malformed EVA_MODULE node: missing 'name' field.",
										this.GetType().Name
									));
									continue;
								}
							}

							ConfigNode[] evaResourceNodes = GameDatabase.Instance.GetConfigNodes("EVA_RESOURCE");

							Tools.PostDebugMessage(this, "Checking {0} EVA_RESOURCE nodes.", evaResourceNodes.Length);

							foreach (ConfigNode evaResourceNode in evaResourceNodes)
							{
								string resourceName;

								if (evaResourceNode.TryGetValue("name", out resourceName))
								{
									Tools.PostDebugMessage(this, "Adding resource '{0}'", resourceName);

									PartResourceDefinition resourceInfo =
										PartResourceLibrary.Instance.GetDefinition(resourceName);

									if (resourceInfo == null)
									{
										Debug.LogWarning(string.Format(
											"[{0}]: Skipping resource {1}: definition not present in library.",
											this.GetType().Name,
											resourceName
										));

										continue;
									}

									Tools.PostDebugMessage(this, "Resource '{0}' is in library.", resourceName);

									if (evaPart.GetComponents<PartResource>().Any(r => r.resourceName == resourceName))
									{
										Debug.LogWarning(string.Format(
											"[{0}]: Skipping resource {1}: already present in kerbalEVA.",
											this.GetType().Name,
											resourceName
										));

										continue;
									}

									Tools.PostDebugMessage(this, "Resource '{0}' is not present.", resourceName);

									PartResource resource = evaPart.gameObject.AddComponent<PartResource>();

									Tools.PostDebugMessage(this, "Resource '{0}' component built.", resourceName);

									resource.SetInfo(resourceInfo);
									resource.Load(evaResourceNode);

									Tools.PostDebugMessage(this, "Resource '{0}' loaded.", resourceName);
								}
								else
								{
									Debug.Log(string.Format(
										"[{0}]: Skipping malformed EVA_RESOURCE node: missing 'name' field.",
										this.GetType().Name
									));
									continue;
								}
							}
							
							#if DEBUG
							log = Tools.DebugLogger.New(this);

							log.AppendLine("Modules after run:");

							foreach (var m in evaPart.GetComponents<PartModule>())
							{
								log.Append('\t');
								log.Append(m.GetType().Name);
								log.Append('\n');
							}

							log.AppendLine("Resources after run:");

							foreach (var r in evaPart.GetComponents<PartResource>())
							{
								log.Append('\t');
								log.AppendFormat("Name: {0}, amount: {1}, maxAmount: {2}",
									r.resourceName, r.amount, r.maxAmount);
								log.Append('\n');
							}

							log.Print();
							#endif

							this.runOnce = false;

							GameObject.Destroy(this);

							Tools.PostDebugMessage(this, "Destruction Requested.");

							break;
						}
					}
				}
			}
		}

		#if DEBUG
		public void OnDestroy()
		{
			Tools.PostDebugMessage(this, "Destroyed.");
		}
		#endif
	}
}

