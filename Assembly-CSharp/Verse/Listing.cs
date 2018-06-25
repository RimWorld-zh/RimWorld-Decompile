using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E7D RID: 3709
	public abstract class Listing
	{
		// Token: 0x040039DF RID: 14815
		public float verticalSpacing = 2f;

		// Token: 0x040039E0 RID: 14816
		protected Rect listingRect;

		// Token: 0x040039E1 RID: 14817
		protected float curY = 0f;

		// Token: 0x040039E2 RID: 14818
		protected float curX = 0f;

		// Token: 0x040039E3 RID: 14819
		private float columnWidthInt;

		// Token: 0x040039E4 RID: 14820
		private bool hasCustomColumnWidth;

		// Token: 0x040039E5 RID: 14821
		public const float ColumnSpacing = 17f;

		// Token: 0x040039E6 RID: 14822
		private const float DefaultGap = 12f;

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06005773 RID: 22387 RVA: 0x001B2C34 File Offset: 0x001B1034
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06005775 RID: 22389 RVA: 0x001B2C60 File Offset: 0x001B1060
		// (set) Token: 0x06005774 RID: 22388 RVA: 0x001B2C4F File Offset: 0x001B104F
		public float ColumnWidth
		{
			get
			{
				return this.columnWidthInt;
			}
			set
			{
				this.columnWidthInt = value;
				this.hasCustomColumnWidth = true;
			}
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x001B2C7B File Offset: 0x001B107B
		public void NewColumn()
		{
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001B2CA2 File Offset: 0x001B10A2
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x001B2CC4 File Offset: 0x001B10C4
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x001B2D09 File Offset: 0x001B1109
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x001B2D1C File Offset: 0x001B111C
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x001B2D8C File Offset: 0x001B118C
		public virtual void Begin(Rect rect)
		{
			this.listingRect = rect;
			if (this.hasCustomColumnWidth)
			{
				if (this.columnWidthInt > this.listingRect.width)
				{
					Log.Error(string.Concat(new object[]
					{
						"Listing set ColumnWith to ",
						this.columnWidthInt,
						" which is more than the whole listing rect width of ",
						this.listingRect.width,
						". Clamping."
					}), false);
					this.columnWidthInt = this.listingRect.width;
				}
			}
			else
			{
				this.columnWidthInt = this.listingRect.width;
			}
			this.curX = 0f;
			this.curY = 0f;
			GUI.BeginGroup(rect);
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x001B2E53 File Offset: 0x001B1253
		public virtual void End()
		{
			GUI.EndGroup();
		}
	}
}
