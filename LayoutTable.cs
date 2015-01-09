// ToadicusTools
//
// LayoutTable.cs
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

using KSP;
using System;
using System.Collections.Generic;
using ToadicusTools.SparseTable;
using UnityEngine;

namespace ToadicusTools
{
	public class LayoutTable : SparseTable<LayoutCell, LayoutColumn, LayoutRow>
	{
		public GUIStyle defaultStyle = GUI.skin.label;

		public LayoutTable() : base()
		{
		}

		public void ApplyStyle(GUIStyle style, UInt32 colOffset, UInt32 rowOffset)
		{
			this.defaultStyle = style;

			foreach (var cell in this.table.Values)
			{
				if (cell.Hash.columnIdx >= colOffset && cell.Hash.rowIdx >= rowOffset)
				{
					cell.Style = defaultStyle;
				}
			}
		}

		public void ApplyStyle(GUIStyle style)
		{
			this.ApplyStyle(style, 0u, 0u);
		}

		public void Render(UInt32 colOffset, UInt32 rowOffset, UInt32 colRange, UInt32 rowRange)
		{
			colRange = Math.Max(colRange, this.maxColumn - colOffset);
			rowRange = Math.Max(rowRange, this.maxRow - rowOffset);

			for (UInt32 colIdx = colOffset; colIdx < colOffset + colRange; colIdx++)
			{
				GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

				List<GUILayoutOption> options = new List<GUILayoutOption>();

				LayoutColumn column = this.GetColumn(colIdx);

				if (column.Width != null)
				{
					options.Add(GUILayout.Width((float)column.Width));
				}

				for (UInt32 rowIdx = rowOffset; rowIdx < rowOffset + rowRange; rowIdx++)
				{
					GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

					LayoutRow row = this.GetRow(rowIdx);

					if (row.Height != null)
					{
						options.Add(GUILayout.Height((float)row.Height));
					}

					LayoutCell cell = this[colIdx, rowIdx];

					cell.Render(options.ToArray());

					column.Width = Mathf.Max(column.Width ?? 0f, cell.Width);
					row.Height = Mathf.Max(row.Height ?? 0f, cell.Height);

					GUILayout.EndHorizontal();
				}

				GUILayout.EndVertical();
			}
		}

		public void Render()
		{
			this.Render(0u, 0u, this.maxColumn, this.maxRow);
		}
	}

	public class LayoutCell : Cell
	{
		public static IFormatProvider Formatter = new SIFormatter();

		public GUIStyle Style;

		public string Format;

		public float Width
		{
			get;
			private set;
		}

		public float Height
		{
			get;
			private set;
		}

		public LayoutCell() : base() {}

		public LayoutCell(
			object value,
			string format = default(string),
			GUIStyle style = null,
			float? width = null,
			float? height = null
		) : base()
		{
			this.Value = value;

			if (format != default(string))
			{
				this.Format = format;
			}

			if (style != null)
			{
				this.Style = style;
			}

			if (width != null)
			{
				this.Width = (float)width;
			}

			if (height != null)
			{
				this.Height = (float)height;
			}
		}

		public void Render(params GUILayoutOption[] options)
		{
			GUILayout.Label(this.Value.ToString(), this.Style, options);

			Rect labelRect = GUILayoutUtility.GetLastRect();

			this.Width = labelRect.width;
			this.Height = labelRect.height;
		}

		public override string ToString()
		{
			if (this.Value is IFormattable)
			{
				return ((IFormattable)this.Value).ToString(this.Format, Formatter);
			}
			else
			{
				return this.Value.ToString();
			}
		}
	}

	public class LayoutColumn : SequentialColumn<LayoutCell>
	{
		public float? Width = null;
		public GUIStyle defaultStyle = GUI.skin.label;

		public override void Add(LayoutCell value)
		{
			value.Style = defaultStyle;
			base.Add(value);
		}

		public void ApplyStyle(GUIStyle style)
		{
			this.defaultStyle = style;

			foreach (LayoutCell cell in this)
			{
				cell.Style = defaultStyle;
			}
		}
	}

	public class LayoutRow : SequentialRow<LayoutCell>
	{
		public float? Height = null;
		public GUIStyle defaultStyle = GUI.skin.label;

		public override void Add(LayoutCell value)
		{
			value.Style = defaultStyle;
			base.Add(value);
		}

		public void Reset(UInt32 newRange)
		{
			this.Range = newRange;
		}

		public void Reset()
		{
			this.Reset(0u);
		}

		public void ApplyStyle(GUIStyle style)
		{
			this.defaultStyle = style;

			foreach (LayoutCell cell in this)
			{
				cell.Style = defaultStyle;
			}
		}
	}
}

