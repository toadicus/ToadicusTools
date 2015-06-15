// ToadicusTools
//
// DebugLogger.cs
//
// Copyright © 2015, toadicus
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

using System;
using System.Text;
using UnityEngine;

namespace ToadicusTools.DebugTools
{
	public class PooledDebugLogger : PooledObject<PooledDebugLogger>, IDisposable
	{
		public static PooledDebugLogger New(object caller)
		{
			return Get();
		}

		public static PooledDebugLogger New(Type callingType)
		{
			PooledDebugLogger logger = Get();
			logger.sb.Append(callingType.Name);
			logger.sb.Append(": ");
			return logger;
		}

		private StringBuilder sb;

		protected override void onGet()
		{
			this.sb.Length = 0;
		}

		[Obsolete("Use DebugLogger.Get instead", true)]
		public PooledDebugLogger()
		{
			this.sb = new StringBuilder();
		}

		[Obsolete("Use DebugLogger.Get instead", true)]
		private PooledDebugLogger(Type caller) : this() {}

		[System.Diagnostics.Conditional("DEBUG")]
		public void Append(object value)
		{
			this.sb.Append(value);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void AppendFormat(string format, params object[] args)
		{
			this.sb.AppendFormat(format, args);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void AppendLine(string value)
		{
			this.sb.Append(value);
			this.sb.Append('\n');
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void Print(bool postToScreen)
		{
			if (postToScreen)
			{
				Logging.PostMessageWithScreenMsg(this.sb.ToString());
			}
			else
			{
				Logging.PostLogMessage(this.sb.ToString());
			}

			this.sb.Length = 0;
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void Print()
		{
			Logging.PostMessageWithScreenMsg(this.sb.ToString());
		}
		[System.Diagnostics.Conditional("DEBUG")]
		public void Clear()
		{
			this.sb.Length = 0;
		}

		public void Dispose()
		{
			Put(this);
		}
	}

}

