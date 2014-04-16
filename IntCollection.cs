// ToadicusTools
//
// IntCollection.cs
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

using System;

namespace ToadicusTools
{
	public class IntCollection
	{
		public static implicit operator long(IntCollection c)
		{
			return c.collection;
		}

		protected long mask;

		public long collection { get; protected set; }
		public ushort maxCount { get; protected set; }
		public ushort wordLength { get; protected set; }

		public IntCollection (ushort wordLength = 4, long initialCollection = 0)
		{
			this.collection = initialCollection;
			this.wordLength = wordLength;
			this.maxCount = (ushort)((sizeof(long) * 8 - 1) / wordLength);
			this.mask = ((1 << this.wordLength) - 1);
		}

		public ushort this[int idx]
		{
			get {
				if (idx < 0) {
					idx += this.maxCount;
				}

				if (idx >= maxCount || idx < 0) {
					throw new IndexOutOfRangeException ();
				}

				idx *= wordLength;

				return (ushort)((this.collection & (this.mask << idx)) >> idx);
			}
			set {
				if (idx < 0) {
					idx += this.maxCount;
				}

				if (idx >= maxCount || idx < 0) {
					throw new IndexOutOfRangeException ();
				}

				idx *= wordLength;

				long packvalue = value & this.mask;

				this.collection &= ~(this.mask << idx);
				this.collection |= packvalue << idx;
			}
		}
	}
}

