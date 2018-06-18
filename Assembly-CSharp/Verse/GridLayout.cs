using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E70 RID: 3696
	public class GridLayout
	{
		// Token: 0x060056E7 RID: 22247 RVA: 0x002CB4B8 File Offset: 0x002C98B8
		public GridLayout(Rect container, int cols = 1, int rows = 1, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.container = new Rect(container);
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			float num = container.width - outerPadding * 2f - (float)(cols - 1) * innerPadding;
			float num2 = container.height - outerPadding * 2f - (float)(rows - 1) * innerPadding;
			this.colWidth = num / (float)cols;
			this.rowHeight = num2 / (float)rows;
			this.colStride = this.colWidth + innerPadding;
			this.rowStride = this.rowHeight + innerPadding;
		}

		// Token: 0x060056E8 RID: 22248 RVA: 0x002CB554 File Offset: 0x002C9954
		public GridLayout(float colWidth, float rowHeight, int cols, int rows, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.colWidth = colWidth;
			this.rowHeight = rowHeight;
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			this.colStride = colWidth + innerPadding;
			this.rowStride = rowHeight + innerPadding;
			this.container = new Rect(0f, 0f, outerPadding * 2f + colWidth * (float)cols + innerPadding * (float)cols - 1f, outerPadding * 2f + rowHeight * (float)rows + innerPadding * (float)rows - 1f);
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x002CB5EC File Offset: 0x002C99EC
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x002CB620 File Offset: 0x002C9A20
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}

		// Token: 0x040039A3 RID: 14755
		public Rect container;

		// Token: 0x040039A4 RID: 14756
		private int cols;

		// Token: 0x040039A5 RID: 14757
		private float outerPadding;

		// Token: 0x040039A6 RID: 14758
		private float innerPadding;

		// Token: 0x040039A7 RID: 14759
		private float colStride;

		// Token: 0x040039A8 RID: 14760
		private float rowStride;

		// Token: 0x040039A9 RID: 14761
		private float colWidth;

		// Token: 0x040039AA RID: 14762
		private float rowHeight;
	}
}
