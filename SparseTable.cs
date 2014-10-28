// ToadicusTools
//
// SparseTable.cs
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Write list-style sequential column and row classes.

public class SparseTable
{
	private Dictionary<Address, Cell> table;
	private Dictionary<UInt32, ColumnRange> columns;
	private Dictionary<UInt32, RowRange> rows;

	private UInt32 maxColumn;
	private UInt32 maxRow;

	public Cell this[UInt32 columnIdx, UInt32 rowIdx]
	{
		get
		{
			return this[new Address(columnIdx, rowIdx)];
		}
		set
		{
			Address hash = new Address(columnIdx, rowIdx);
			this[hash] = value;
			value.Hash = hash;
		}
	}

	public Cell this[Address address]
	{
		get
		{
			if (this.table.ContainsKey(address))
			{
				return this.table[address];
			}
			else
			{
				return null;
			}
		}
		set
		{
			this.table[address] = value;
			this.maxColumn = Math.Max(address.columnIdx, this.maxColumn);
			this.maxRow = Math.Max(address.rowIdx, this.maxRow);
		}
	}

	public ColumnRange GetColumn(UInt32 columnIdx, UInt32 offset, UInt32 range)
	{
		if (!this.columns.ContainsKey(columnIdx))
		{
			this.columns[columnIdx] = new ColumnRange(this, columnIdx, offset, range);
		}

		return this.columns[columnIdx];
	}

	public ColumnRange GetColumn(UInt32 columnIdx, UInt32 range)
	{
		return this.GetColumn(columnIdx, 0, range);
	}

	public ColumnRange GetColumn(UInt32 columnIdx)
	{
		return this.GetColumn(columnIdx, this.maxRow);
	}

	public RowRange GetRow(UInt32 rowIdx, UInt32 offset, UInt32 range)
	{
		if (!this.rows.ContainsKey(rowIdx))
		{
			this.rows[rowIdx] = new RowRange(this, rowIdx, offset, range);
		}

		return this.rows[rowIdx];
	}

	public RowRange GetRow(UInt32 rowIdx, UInt32 range)
	{
		return this.GetRow(rowIdx, 0, range);
	}

	public RowRange GetRow(UInt32 rowIdx)
	{
		return this.GetRow(rowIdx, this.maxColumn);
	}

	public SparseTable()
	{
		this.table = new Dictionary<Address, Cell>();
	}

	public class SequentialColumn : ColumnRange
	{
		public UInt32 Count
		{
			get
			{
				return this.Range;
			}
		}

		public void Add(object value)
		{
			this.Range++;
			this[this.Range - 1] = value;
		}

		public SequentialColumn(SparseTable table, UInt32 columnIdx, UInt32 offset) : base(table, columnIdx, offset, 0) {}

		public SequentialColumn(SparseTable table, UInt32 columnIdx) : this(table, columnIdx, 0) {}

		protected SequentialColumn() {}
	}

	public class SequentialRow : RowRange
	{
		public UInt32 Count
		{
			get
			{
				return this.Range;
			}
		}

		public void Add(object value)
		{
			this.Range++;
			this[this.Range - 1] = value;
		}

		public SequentialRow(SparseTable table, UInt32 rowIdx, UInt32 offset) : base(table, rowIdx, offset, 0) {}

		public SequentialRow(SparseTable table, UInt32 rowIdx) : this(table, rowIdx, 0) {}

		protected SequentialRow() {}
	}

	public class ColumnRange : Range1D
	{
		protected UInt32 columnIdx;

		public override object this[UInt32 idx]
		{
			get
			{
				if (idx >= this.Range)
				{
					throw new ArgumentOutOfRangeException();
				}

				return this.table[this.columnIdx, idx + this.Offset];
			}
			set
			{
				if (idx >= this.Range)
				{
					throw new ArgumentOutOfRangeException();
				}

				this.table[this.columnIdx, idx + this.Offset] = (Cell)value;
			}
		}

		public ColumnRange(SparseTable table, UInt32 columnIdx, UInt32 offset, UInt32 range) : base(table, offset, range)
		{
			this.columnIdx = columnIdx;
		}

		public ColumnRange(SparseTable table, UInt32 columnIdx, UInt32 range) : this(table, columnIdx, 0, range) {}

		protected ColumnRange() {}
	}

	public class RowRange : Range1D
	{
		private UInt32 rowIdx;

		public override object this[UInt32 idx]
		{
			get
			{
				if (idx >= this.Range)
				{
					throw new ArgumentOutOfRangeException();
				}

				return this.table[idx + this.Offset, this.rowIdx];
			}
			set
			{
				if (idx >= this.Range)
				{
					throw new ArgumentOutOfRangeException();
				}

				this.table[idx + this.Offset, this.rowIdx] = (Cell)value;
			}
		}

		public RowRange(SparseTable table, UInt32 rowIdx, UInt32 offset, UInt32 range) : base(table, offset, range)
		{
			this.rowIdx = rowIdx;
		}

		public RowRange(SparseTable table, UInt32 rowIdx, UInt32 range) : this(table, rowIdx, 0, range) {}

		protected RowRange() {}
	}

	public abstract class Range1D : IEnumerable
	{
		protected SparseTable table;

		public virtual UInt32 Offset
		{
			get;
			protected set;
		}

		public virtual UInt32 Range
		{
			get;
			protected set;
		}

		public abstract object this [UInt32 idx]
		{
			get;
			set;
		}

		public void RemoveAt(UInt32 idx)
		{
			if (idx < this.Range)
			{
				this[idx] = null;
			}
		}

		public void Remove(object value)
		{
			for (UInt32 i = 0; i < this.Range; i++)
			{
				if (System.Object.Equals(this[i], value))
				{
					this.RemoveAt(i);
					break;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)this.GetEnumerator();
		}

		public virtual RangeEnumerator GetEnumerator()
		{
			return new RangeEnumerator(this);
		}

		protected Range1D(SparseTable table, UInt32 offset, UInt32 range)
		{
			this.table = table;
			this.Offset = offset;
			this.Range = range;
		}

		protected Range1D() {}

		public class RangeEnumerator : IEnumerator
		{
			private Range1D range1D;
			private UInt32 idx;

			public object Current
			{
				get
				{
					return range1D[idx];
				}
			}

			public bool MoveNext()
			{
				this.idx++;

				return (this.idx < this.range1D.Range);
			}

			public void Reset()
			{
				this.idx = 0;
			}

			public RangeEnumerator(Range1D range)
			{
				this.range1D = range;
			}

			private RangeEnumerator() {}
		}
	}

	public struct Cell<T>
		where T : struct
	{
		public Address Hash
		{
			get;
			set;
		}

		public T Value
		{
			get;
			set;
		}

		public Cell(Address hash, T value) : this()
		{
			this.Hash = hash;
			this.Value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj is Cell<T>)
			{
				Cell<T> cellObj = (Cell<T>)obj;

				return System.Object.Equals(cellObj.Hash, this.Hash) && System.Object.Equals(cellObj.Value, this.Value);
			}

			return false;
		}

		public static implicit operator T(Cell<T> cell)
		{
			return cell.Value;
		}

		public static explicit operator Cell<T>(T obj)
		{
			Cell<T> cell = new Cell<T>();

			cell.Value = obj;

			return cell;
		}
	}

	public interface Cell
	{
		Address Hash
		{
			get;
			set;
		}
	}

	public struct Address
	{
		public UInt64 Hash
		{
			get;
			private set;
		}

		public UInt32 columnIdx
		{
			get
			{
				return (UInt32)(Hash >> 32);
			}
			set
			{
				this.Hash &= (((UInt64)UInt32.MaxValue) << 32);

				this.Hash |= (((UInt64)value) << 32);
			}
		}

		public UInt32 rowIdx
		{
			get
			{
				return (UInt32)Hash;
			}
			set
			{
				this.Hash &= (UInt64)UInt32.MaxValue;

				this.Hash |= (UInt64)value;
			}
		}

		public Address(UInt32 columnIdx, UInt32 rowIdx) : this()
		{
			this.columnIdx = columnIdx;
			this.rowIdx = rowIdx;
		}
	}
}
