// ToadicusTools
//
// SecurePartModule.cs
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
	/// <summary>
	/// "Secure" PartModule, allowing KSPField targets to be non-public.
	/// </summary>
	#if DEBUG
	public abstract class SecurePartModule : DebugPartModule
	#else
	public class SecurePartModule : PartModule
	#endif
	{
		#if PROP_KSPFIELDS
		private UIPartActionWindow actionWindow;
		#endif

		/// <summary>
		/// Runs during Unity's Awake() phase during prefab compile and clone passes.
		/// </summary>
		public override void OnAwake()
		{
			base.OnAwake();

			bool isClonePass;

			switch (HighLogic.LoadedScene)
			{
				case GameScenes.EDITOR:
				case GameScenes.FLIGHT:
				case GameScenes.SPH:
				case GameScenes.SPACECENTER:
				case GameScenes.TRACKSTATION:
					isClonePass = true;
					break;
				default:
					isClonePass = false;
					break;
			}

			if (isClonePass)
			{
				foreach (BaseField field in this.Fields)
				{
					int moduleIdx = this.part.Modules.IndexOf(this);

					PartModule prefabModule = PartLoader.getPartInfoByName(this.part.partInfo.name)
						.partPrefab.Modules[moduleIdx];
					object prefabVal = prefabModule.Fields[field.name].GetValue(prefabModule);

					field.SetValue(prefabVal, this);
				}
			}
			else
			{
				this.OnCompile();
			}
			#if PROP_KSPFIELDS
			GameEvents.onPartActionUICreate.Add(this.onPartActionUICreate);
			GameEvents.onPartActionUIDismiss.Add(this.onPartActionUIDismiss);
			#endif
		}

		/// <summary>
		/// Runs during Unity's Awake() phase during prefab compile passes (i.e. during startup).
		/// </summary>
		public virtual void OnCompile() {}

		/// <summary>
		/// Runs during prefab compile passes, clone passes, and when loading from persistence.
		/// </summary>
		/// <param name="node">The <see cref="KSP.ConfigNode/> containing static or persistent data></param>
		public override void OnLoad(ConfigNode node)
		{
			foreach (BaseField field in this.Fields)
			{
				this.LoadMemberByName(field.FieldInfo, node, field.name);
			}

			#if PROP_KSPFIELDS
			foreach (var prop in this.GetType().GetProperties())
			{
				foreach (var attr in prop.GetCustomAttributes(true))
				{
					if (attr is KSPField && prop.GetSetMethod(true) != null)
					{
						this.LoadMemberByName(prop, node, prop.Name);
					}
				}
			}
			#endif

			#if DEBUG
			this.DumpModule();
			#endif
		}

		#if PROP_KSPFIELDS
		public override void OnSave(ConfigNode node)
		{
			base.OnSave(node);

			foreach (var prop in this.GetType().GetProperties())
			{
				foreach (var attr in prop.GetCustomAttributes(true))
				{
					if (attr is KSPField && prop.GetGetMethod(true) != null)
					{
						if (!node.HasValue(prop.Name))
						{
							node.AddValue(prop.Name, string.Empty);
						}

						node.SetValue(prop.Name, (string)prop.GetValue(this, null));
					}
				}
			}
		}

		public virtual void Update()
		{
			if (this.actionWindow != null)
			{
				foreach (var prop in this.GetType().GetProperties())
				{
					foreach (var attr in prop.GetCustomAttributes(true))
					{
						if (attr is UI_Control && prop.GetGetMethod(true) != null)
						{
							// NYI
						}
					}
				}
			}
		}

		public virtual void Destroy()
		{
			GameEvents.onPartActionUICreate.Remove(this.onPartActionUICreate);
			GameEvents.onPartActionUIDismiss.Remove(this.onPartActionUIDismiss);
		}
		#endif

		private void LoadMemberByName(MemberInfo field, ConfigNode node, string name)
		{
			if (node.HasValue(name))
			{
				string valstring = node.GetValue(name);

				object value;

				if (field is FieldInfo)
				{
					value = (field as FieldInfo).GetValue(this);
				}
				else if (field is PropertyInfo)
				{
					value = (field as PropertyInfo).GetValue(this, null);
				}
				else
				{
					throw new NotImplementedException(
						"LoadMemberByName is only valid for FieldInfo and PropertyInfo objects");
				}

				if (typeof(int).IsInstanceOfType(value))
				{
					int v = (int)value;
					int.TryParse(valstring, out v);
					value = v;
				}
				else if (typeof(float).IsInstanceOfType(value))
				{
					float v = (float)value;
					float.TryParse(valstring, out v);
					value = v;
				}
				else if (typeof(double).IsInstanceOfType(value))
				{
					double v = (double)value;
					double.TryParse(valstring, out v);
					value = v;
				}
				else if (typeof(Vector3).IsInstanceOfType(value))
				{
					Vector3 v = (Vector3)value;
					try
					{
						v = ConfigNode.ParseVector3(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(Vector3d).IsInstanceOfType(value))
				{
					Vector3d v = (Vector3d)value;
					try
					{
						v = ConfigNode.ParseVector3D(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(Vector2).IsInstanceOfType(value))
				{
					Vector2 v = (Vector2)value;
					try
					{
						v = ConfigNode.ParseVector2(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(Vector4).IsInstanceOfType(value))
				{
					Vector4 v = (Vector4)value;
					try
					{
						v = ConfigNode.ParseVector4(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(Quaternion).IsInstanceOfType(value))
				{
					Quaternion v = (Quaternion)value;
					try
					{
						v = ConfigNode.ParseQuaternion(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(QuaternionD).IsInstanceOfType(value))
				{
					QuaternionD v = (QuaternionD)value;
					try
					{
						v = ConfigNode.ParseQuaternionD(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(Matrix4x4).IsInstanceOfType(value))
				{
					Matrix4x4 v = (Matrix4x4)value;
					try
					{
						v = ConfigNode.ParseMatrix4x4(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(Color).IsInstanceOfType(value))
				{
					Color v = (Color)value;
					try
					{
						v = ConfigNode.ParseColor(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(Color32).IsInstanceOfType(value))
				{
					Color32 v = (Color32)value;
					try
					{
						v = ConfigNode.ParseColor32(valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (value.GetType().IsEnum)
				{
					Enum v = value as Enum;
					try
					{
						v = ConfigNode.ParseEnum(value.GetType(), valstring);
					}
					catch { /* Do nothing */ }
					value = v;
				}
				else if (typeof(IConfigNode).IsInstanceOfType(value))
				{
					(value as IConfigNode).Load(node);
				}
				else
				{
					value = valstring;
				}

				if (field is FieldInfo)
				{
					(field as FieldInfo).SetValue(this, value);
				}
				else if (field is PropertyInfo)
				{
					(field as PropertyInfo).SetValue(this, value, null);
				}
			}
		}

		#if PROP_KSPFIELDS
		protected virtual void onPartActionUICreate(Part data)
		{
			if (this.part != null && data == this.part)
			{
				var actionWindows = MonoBehaviour.FindObjectsOfType<UIPartActionWindow>();

				foreach (var window in actionWindows)
				{
					if (window.part == this.part)
					{
						this.actionWindow = window;
					}
				}
			}
		}

		protected virtual void onPartActionUIDismiss(Part data)
		{
			if (this.part != null && data == this.part)
			{
				this.actionWindow = null;
			}
		}
		#endif
	}
}

