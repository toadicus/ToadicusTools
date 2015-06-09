// ToadicusTools
//
// RollingAverage.cs
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

namespace ToadicusTools
{
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

