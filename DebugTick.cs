// ToadicusTools
//
// DebugTick.cs
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

#if DEBUG && VERBOSE

using KSP;
using System;
using UnityEngine;

namespace ToadicusTools
{
	[KSPAddon(KSPAddon.Startup.EveryScene, false)]
	public class DebugTick : MonoBehaviour
	{
		private DateTime? timeAwake;
		private DateTime? timeStart;

		private DateTime? timeLastUpdate;
		private DateTime? timeThisUpdate;
		private RollingAverage avgUpdate;

		private DateTime? timeLastLateUpdate;
		private DateTime? timeThisLateUpdate;
		private RollingAverage avgLateUpdate;

		private DateTime? timeLastFixedUpdate;
		private DateTime? timeThisFixedUpdate;
		private RollingAverage avgFixedUpdate;

		public void Awake()
		{
			this.timeAwake = System.DateTime.Now;
			this.Log("Awake at {0}", this.timeAwake);

			this.avgUpdate = new RollingAverage();
			this.avgLateUpdate = new RollingAverage();
			this.avgFixedUpdate = new RollingAverage();
		}

		public void Start()
		{
			this.timeStart = System.DateTime.Now;
			this.Log("Started at {0} ({1} since Awake)", this.timeStart, this.timeStart - this.timeAwake);
		}

		public void Update()
		{
			this.timeThisUpdate = System.DateTime.Now;

			if (this.timeLastUpdate == null)
			{
				this.Log("First Update at {0} ({1} since Start)", this.timeThisUpdate, this.timeThisUpdate - this.timeStart);
			}
			else
			{
				TimeSpan delta = (TimeSpan)(this.timeThisUpdate - this.timeLastUpdate);
				double deltams = delta.TotalMilliseconds;

				if (deltams > this.avgUpdate.Average + this.avgUpdate.StdDev * 4)
				{
					this.Log("Long Update at {0} ({1} ms since last Update)", this.timeThisUpdate, deltams);
				}

				this.avgUpdate.AddItem(deltams);
			}

			this.timeLastUpdate = this.timeThisUpdate;
		}

		public void LateUpdate()
		{
			this.timeThisLateUpdate = System.DateTime.Now;

			if (this.timeLastLateUpdate == null)
			{
				this.Log("First LateUpdate at {0} ({1} since Start)", this.timeThisLateUpdate, this.timeThisLateUpdate - this.timeStart);
			}
			else
			{
				TimeSpan delta = (TimeSpan)(this.timeThisLateUpdate - this.timeLastLateUpdate);
				double deltams = delta.TotalMilliseconds;

				if (deltams > this.avgLateUpdate.Average + this.avgLateUpdate.StdDev * 4)
				{
					//this.Log("Long LateUpdate at {0} ({1} ms since last LateUpdate; Avg: {2}; StdDev: {3})", this.timeThisLateUpdate, deltams, this.avgLateUpdate.Average, this.avgLateUpdate.StdDev);
					this.Log("Long LateUpdate at {0} ({1} ms since last LateUpdate", this.timeThisLateUpdate, deltams);
				}

				this.avgLateUpdate.AddItem(deltams);
			}

			this.timeLastLateUpdate = this.timeThisLateUpdate;
		}

		public void FixedUpdate()
		{
			this.timeThisFixedUpdate = System.DateTime.Now;

			if (this.timeLastFixedUpdate == null)
			{
				this.Log("First FixedUpdate at {0} ({1} since Start)", this.timeThisFixedUpdate, this.timeThisFixedUpdate - this.timeStart);
			}
			else
			{
				TimeSpan delta = (TimeSpan)(this.timeThisFixedUpdate - this.timeLastFixedUpdate);
				double deltams = delta.TotalMilliseconds;

				if (deltams > this.avgFixedUpdate.Average + this.avgFixedUpdate.StdDev * 4)
				{
					this.Log("Long FixedUpdate at {0} ({1} ms since last FixedUpdate)", this.timeThisFixedUpdate, deltams);
				}

				this.avgFixedUpdate.AddItem(deltams);
			}

			this.timeLastFixedUpdate = this.timeThisFixedUpdate;
		}
	}

	public class RollingAverage
	{
		private double[] bin;
		private UInt16 curIdx;
		private UInt16 length;

		public RollingAverage() : this(32) {}

		public RollingAverage(UInt16 length)
		{
			this.length = length;
			this.bin = new double[length];
			this.curIdx = 0;
		}

		public double Average
		{
			get;
			private set;
		}

		public double StdDev
		{
			get;
			private set;
		}

		public void AddItem(double item)
		{
			this.bin[curIdx % this.length] = item;
			this.curIdx += 1;

			this.CalcAverage();
			this.CalcStdDev();
		}

		public void CalcAverage()
		{
			double sum = 0d;

			int realLen = Math.Min(this.length, curIdx);

			for (int i = 0; i < realLen; i++)
			{
				double item = this.bin[i];
				sum += item;
			}

			this.Average = sum / (double)realLen;
		}

		public void CalcStdDev()
		{
			double sumSqrDiffs = 0d;

			int realLen = Math.Min(this.length, curIdx);

			for (int i = 0; i < realLen; i++)
			{
				double item = this.bin[i];

				double diff = item - this.Average;

				sumSqrDiffs += diff * diff;
			}

			this.StdDev = Math.Sqrt(sumSqrDiffs / (double)realLen);
		}
	}
}

#endif