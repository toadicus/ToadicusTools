// ToadicusTools
//
// Table.cs
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
using System.Collections.Generic;
using UnityEngine;

public class Table
{
	private List<Column> columns;
	private float width;

	public Column this[int idx]
	{
		get
		{
			return this.columns[idx];
		}
		set
		{
			if (idx > 0 && idx < this.columns.Count)
			{
				this.width -= this.columns[idx].Width;
			}

			this.columns[idx] = value;

			this.width += value.Width;
		}
	}

	public float Width
	{
		get
		{
			return width;
		}
	}

	public void Add(Column column)
	{
		this.columns.Add(column);

		this.width += column.Width;
	}

	public void ClearColumns()
	{
		foreach (Column column in this.columns)
		{
			column.Clear();
		}
	}

	public void Render()
	{
		GUILayout.BeginHorizontal(GUILayout.MinWidth(this.width));

		foreach (Column column in this.columns)
		{
			column.Render();
		}

		GUILayout.EndHorizontal();
	}

	public Table()
	{
		this.columns = new List<Column>();
	}

	public class Column<T> : Column, IEnumerable<T>
	{
		private List<T> cells;
		private string format;

		public GUIStyle Style { get; set; }

		public float Width { get; set; }

		public T this[int idx]
		{
			get
			{
				if (idx < 0 || idx >= this.cells.Count)
				{
					return default(T);
				}

				return cells[idx];
			}
			set
			{
				if (idx < 0)
				{
					throw new ArgumentOutOfRangeException("Column index must be 0 or greater.");
				}

				this.cells[idx] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.cells.Count;
			}
		}

		public string Format
		{
			get
			{
				return this.format;
			}
			set
			{
				if (typeof(IFormattable).IsAssignableFrom(typeof(T)))
				{
					this.format = value;
				}
				else
				{
					throw new ArgumentException(string.Format(
						"Cannot assign format: Column type {0} is not formattable.",
						typeof(T).Name
					));
				}
			}
		}

		public void Add(T item)
		{
			this.cells.Add(item);
		}

		public void Clear()
		{
			this.cells.Clear();
		}

		public void Render()
		{
			GUILayout.BeginVertical(GUILayout.Width(this.Width), GUILayout.ExpandHeight(true));

			foreach (T cell in this.cells)
			{
				string cellContents;

				if (cell is IFormattable && this.format != null && this.format != string.Empty)
				{
					int digits;

					if (cell is IConvertible && this.format[0] == 'S' && int.TryParse(this.format.Substring(1), out digits))
					{
						cellContents = ToadicusTools.Tools.MuMech_ToSI(Convert.ToDouble(cell), digits);
					}
					else
					{
						cellContents = ((IFormattable)cell).ToString(
							this.format,
							System.Globalization.CultureInfo.CurrentUICulture
						);
					}
				}
				else
				{
					cellContents = cell.ToString();
				}

				GUILayout.Label(cellContents, this.Style, GUILayout.ExpandWidth(true));
			}

			GUILayout.EndVertical();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.cells.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public Column(float width, GUIStyle style)
		{
			this.cells = new List<T>();
			this.Width = width;
			this.Style = style;
		}

		public Column(float width) : this(width, GUI.skin.label) {}

		public Column(GUIStyle style) : this(60f, style) {}

		public Column() : this(60f) {}
	}

	public interface Column : System.Collections.IEnumerable
	{
		float Width { get; }

		int Count { get; }

		string Format { get; set; }

		void Clear();

		void Render();
	}
}