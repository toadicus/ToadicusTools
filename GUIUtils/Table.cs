// ToadicusTools
//
// Table.cs
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
using System.Collections.Generic;
using ToadicusTools.Text;
using UnityEngine;

namespace ToadicusTools.GUIUtils
{
	public class Table
	{
		private List<Column> columns;

		public Column this [int idx]
		{
			get
			{
				return this.columns[idx];
			}
			set
			{
				this.columns[idx] = value;
			}
		}

		public float Width
		{
			get
			{
				float width = 0f;

				Column column;
				for (int idx = 0; idx < this.columns.Count; idx++)
				{
					column = this.columns[idx];
					width += column.Width;
				}

				return width;
			}
		}

		public void Add(Column column)
		{
			this.columns.Add(column);
		}

		public void ApplyCellStyle(GUIStyle style)
		{
			Column column;
			for (int idx = 0; idx < this.columns.Count; idx++)
			{
				column = this.columns[idx];
				column.CellStyle = style;
			}
		}

		public void ApplyHeaderStyle(GUIStyle style)
		{
			Column column;
			for (int idx = 0; idx < this.columns.Count; idx++)
			{
				column = this.columns[idx];
				column.HeaderStyle = style;
			}
		}

		public void ClearTable()
		{
			this.ClearColumns();
			this.columns.Clear();
		}

		public void ClearColumns()
		{
			Column column;
			for (int idx = 0; idx < this.columns.Count; idx++)
			{
				column = this.columns[idx];
				column.Clear();
			}
		}

		public void Render(bool renderHeader)
		{
			GUILayout.BeginHorizontal(GUILayout.MinWidth(this.Width));

			Column column;
			for (int idx = 0; idx < this.columns.Count; idx++)
			{
				column = this.columns[idx];
				column.Render(renderHeader);
			}

			GUILayout.EndHorizontal();
		}

		public void Render()
		{
			this.Render(true);
		}

		public void RenderHeader(bool doVertical)
		{
			GUILayout.BeginHorizontal(GUILayout.MinWidth(this.Width));

			Column column;
			for (int idx = 0; idx < this.columns.Count; idx++)
			{
				column = this.columns[idx];
				column.RenderHeader(doVertical);
			}

			GUILayout.EndHorizontal();
		}

		public void RenderHeader()
		{
			this.RenderHeader(true);
		}

		public Table()
		{
			this.columns = new List<Column>();
		}

		public class Column<T> : Column, IEnumerable<T>
		{
			private List<T> cells;
			private string format;
			public float defWidth;

			public string Header { get; set; }

			public GUIStyle CellStyle { get; set; }

			public GUIStyle HeaderStyle { get; set; }

			public float Width { get; set; }

			object Column.this [int idx]
			{
				get
				{
					return (object)this[idx];
				}
			}

			public T this [int idx]
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

			public void Render(bool renderHeader)
			{
				GUILayout.BeginVertical(
					GUILayout.MinWidth(this.Width),
					GUILayout.ExpandWidth(true),
					GUILayout.ExpandHeight(true)
				);

				if (renderHeader)
				{
					this.RenderHeader();
				}

				T cell;
				for (int idx = 0; idx < this.cells.Count; idx++)
				{
					cell = this.cells[idx];

					string cellContents = Text.SIFormatProvider.SIFormatter.Format(this.format, cell, Text.SIFormatProvider.SIFormatter);

					Vector2 cellSize = this.CellStyle.CalcSize(new GUIContent(cellContents));

					this.Width = Mathf.Max(cellSize.x, this.Width);

					GUILayout.Label(
						cellContents,
						this.CellStyle,
						GUILayout.ExpandWidth(true),
						GUILayout.MinWidth(this.Width),
						GUILayout.Height(((float)this.HeaderStyle.fontSize) * .8f)
					);
				}

				GUILayout.EndVertical();
			}

			public void Render()
			{
				this.Render(true);
			}

			public void RenderHeader(bool doVertical)
			{
				if (doVertical)
				{
					GUILayout.BeginVertical(
						GUILayout.MinWidth(this.Width),
						GUILayout.ExpandWidth(true),
						GUILayout.ExpandHeight(true)
					);
				}

				Vector2 cellSize = this.HeaderStyle.CalcSize(new GUIContent(this.Header));

				this.Width = Mathf.Max(cellSize.x, this.Width);

				GUILayout.Label(
					this.Header,
					this.HeaderStyle,
					GUILayout.ExpandWidth(true),
					GUILayout.MinWidth(this.Width),
					GUILayout.Height(((float)this.HeaderStyle.fontSize) * .8f)
				);

				if (doVertical)
				{
					GUILayout.EndVertical();
				}
			}

			public void RenderHeader()
			{
				this.RenderHeader(false);
			}

			public IEnumerator<T> GetEnumerator()
			{
				return this.cells.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public Column(string header, float width, GUIStyle cellStyle, GUIStyle headerStyle)
			{
				this.Header = header;
				this.cells = new List<T>();
				this.defWidth = this.Width = width;
				this.CellStyle = cellStyle;
				this.HeaderStyle = headerStyle;
			}

			public Column(string header, float width, GUIStyle style) : this(header, width, style, style)
			{
			}

			public Column(string header, float width) : this(header, width, GUI.skin.label)
			{
			}

			public Column(float width) : this(null, width, GUI.skin.label)
			{
			}

			public Column(GUIStyle style) : this(null, 60f, style)
			{
			}

			public Column() : this(60f)
			{
			}
		}

		public interface Column : System.Collections.IEnumerable
		{
			GUIStyle CellStyle { set; }

			GUIStyle HeaderStyle { set; }

			object this [int idx] { get; }

			int Count { get; }

			string Format { get; set; }

			float Width { get; }

			void Clear();

			void Render(bool renderHeader);

			void Render();

			void RenderHeader(bool doVertical);

			void RenderHeader();
		}
	}
}
