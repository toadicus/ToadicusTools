// ToadicusTools
//
// DebugPartModule.cs
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
using System.Text;
using UnityEngine;

namespace ToadicusTools
{
	public class DebugPartModule : PartModule
	{
		public override void OnAwake()
		{
			foreach (BaseField field in this.Fields)
			{
				field.guiActive = field.guiActiveEditor = true;
			}

			this.Events["DumpModule"].guiName = string.Format("Dump {0}", this.GetType().Name);

			base.OnAwake();
		}

		[KSPEvent(guiActive = true, guiActiveEditor = true)]
		public void DumpPart()
		{
			if (this.part != null)
			{
				DumpClassObject(this.part);
			}

			if (this.part.Modules != null)
			{
				foreach (var module in this.part.Modules)
				{
					DumpClassObject(module);
				}
			}
		}

		[KSPEvent(guiActive = true, guiActiveEditor = true)]
		public void DumpModule()
		{
			DumpClassObject(this);
		}

		public static void DumpClassObject(object obj)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(obj.GetType().Name);
			sb.Append(":\n");

			foreach (var fieldInfo in obj.GetType().GetFields(
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic |
				System.Reflection.BindingFlags.Instance |
				System.Reflection.BindingFlags.FlattenHierarchy
			))
			{
				try
				{
					sb.AppendFormat("{0}: {1}\n", fieldInfo.Name, fieldInfo.GetValue(obj));
				}
				catch
				{
					// Do nothing
				}
			}

			foreach (var propInfo in obj.GetType().GetProperties(
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic |
				System.Reflection.BindingFlags.Instance |
				System.Reflection.BindingFlags.FlattenHierarchy
			))
			{
				try
				{
					sb.AppendFormat("{0}: {1}\n", propInfo.Name, propInfo.GetValue(obj, null));
				}
				catch
				{
					// Do nothing
				}
			}

			foreach (var methodInfo in obj.GetType().GetMethods(
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic |
				System.Reflection.BindingFlags.Instance |
				System.Reflection.BindingFlags.FlattenHierarchy
			))
			{
				try
				{
					if (methodInfo.ReturnType != typeof(void) && methodInfo.GetParameters().Length == 0)
					{
						sb.AppendFormat("{0} returns: '''{1}'''\n", methodInfo.Name, methodInfo.Invoke(obj, null));
					}
				}
				catch
				{
					// Do nothing
				}
			}

			Debug.Log(sb.ToString());
		}
	}
}

