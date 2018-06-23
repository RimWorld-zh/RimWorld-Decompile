using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6F RID: 3695
	public class GridLayout
	{
		// Token: 0x040039B3 RID: 14771
		public Rect container;

		// Token: 0x040039B4 RID: 14772
		private int cols;

		// Token: 0x040039B5 RID: 14773
		private float outerPadding;

		// Token: 0x040039B6 RID: 14774
		private float innerPadding;

		// Token: 0x040039B7 RID: 14775
		private float colStride;

		// Token: 0x040039B8 RID: 14776
		private float rowStride;

		// Token: 0x040039B9 RID: 14777
		private float colWidth;

		// Token: 0x040039BA RID: 14778
		private float rowHeight;

		// Token: 0x06005707 RID: 22279 RVA: 0x002CD0C8 File Offset: 0x002CB4C8
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

		// Token: 0x06005708 RID: 22280 RVA: 0x002CD164 File Offset: 0x002CB564
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

		// Token: 0x06005709 RID: 22281 RVA: 0x002CD1FC File Offset: 0x002CB5FC
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x002CD230 File Offset: 0x002CB630
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}
	}
}
