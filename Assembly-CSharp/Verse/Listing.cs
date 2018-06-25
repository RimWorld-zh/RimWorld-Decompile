using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E7E RID: 3710
	public abstract class Listing
	{
		// Token: 0x040039E7 RID: 14823
		public float verticalSpacing = 2f;

		// Token: 0x040039E8 RID: 14824
		protected Rect listingRect;

		// Token: 0x040039E9 RID: 14825
		protected float curY = 0f;

		// Token: 0x040039EA RID: 14826
		protected float curX = 0f;

		// Token: 0x040039EB RID: 14827
		private float columnWidthInt;

		// Token: 0x040039EC RID: 14828
		private bool hasCustomColumnWidth;

		// Token: 0x040039ED RID: 14829
		public const float ColumnSpacing = 17f;

		// Token: 0x040039EE RID: 14830
		private const float DefaultGap = 12f;

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06005773 RID: 22387 RVA: 0x001B2E9C File Offset: 0x001B129C
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06005775 RID: 22389 RVA: 0x001B2EC8 File Offset: 0x001B12C8
		// (set) Token: 0x06005774 RID: 22388 RVA: 0x001B2EB7 File Offset: 0x001B12B7
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

		// Token: 0x06005776 RID: 22390 RVA: 0x001B2EE3 File Offset: 0x001B12E3
		public void NewColumn()
		{
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001B2F0A File Offset: 0x001B130A
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x001B2F2C File Offset: 0x001B132C
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x001B2F71 File Offset: 0x001B1371
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x001B2F84 File Offset: 0x001B1384
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x001B2FF4 File Offset: 0x001B13F4
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

		// Token: 0x0600577C RID: 22396 RVA: 0x001B30BB File Offset: 0x001B14BB
		public virtual void End()
		{
			GUI.EndGroup();
		}
	}
}
