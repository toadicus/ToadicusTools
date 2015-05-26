// ToadicusTools
//
// IntCollection.cs
//
// Copyright © 2014-2015, toadicus
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
	/// <summary>
	/// A collection of small, unsigned integers stored in a single, 64-bit signed container.  This is useful for saving
	/// sets of small numbers as a single large number in the KSP IConfigValue system.
	/// </summary>
	public class IntCollection
	{
		/// <param name="c">The IntCollection object to be returned as a long.</param>
		public static implicit operator long(IntCollection c)
		{
			return c.collection;
		}

		protected long mask;

		/// <summary>
		/// Gets the collection container
		/// </summary>
		/// <value>The collection container</value>
		public long collection { get; protected set; }

		/// <summary>
		/// Gets the maximum number of indexes in the collection
		/// </summary>
		/// <value>the maximum number of indexes in the collection</value>
		public ushort maxCount { get; protected set; }

		/// <summary>
		/// Gets the bit length of unsigned integer members of the collection
		/// </summary>
		/// <value>The bit length of the members</value>
		public ushort wordLength { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ToadicusTools.IntCollection"/> class.
		/// </summary>
		/// <param name="wordLength">Bit length of the individual members to be stored</param>
		/// <param name="initialCollection">Optional initial collection set for loading existing collections</param>
		public IntCollection (ushort wordLength = 4, long initialCollection = 0)
		{
			this.collection = initialCollection;
			this.wordLength = wordLength;
			this.maxCount = (ushort)((sizeof(long) * 8 - 1) / wordLength);
			this.mask = ((1 << this.wordLength) - 1);
		}

		/// <summary>
		/// Gets or sets the <see cref="ToadicusTools.IntCollection"/> member with the specified index.
		/// </summary>
		/// <param name="idx">Index</param>
		public ushort this[int idx]
		{
			get {
				if (idx >= maxCount || idx < 0) {
					throw new IndexOutOfRangeException ();
				}

				idx *= wordLength;

				return (ushort)((this.collection & (this.mask << idx)) >> idx);
			}
			set
			{
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

