using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E7B RID: 3707
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

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x0600576F RID: 22383 RVA: 0x001B2AF4 File Offset: 0x001B0EF4
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06005771 RID: 22385 RVA: 0x001B2B20 File Offset: 0x001B0F20
		// (set) Token: 0x06005770 RID: 22384 RVA: 0x001B2B0F File Offset: 0x001B0F0F
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

		// Token: 0x06005772 RID: 22386 RVA: 0x001B2B3B File Offset: 0x001B0F3B
		public void NewColumn()
		{
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x001B2B62 File Offset: 0x001B0F62
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x001B2B84 File Offset: 0x001B0F84
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x001B2BC9 File Offset: 0x001B0FC9
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x001B2BDC File Offset: 0x001B0FDC
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001B2C4C File Offset: 0x001B104C
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

		// Token: 0x06005778 RID: 22392 RVA: 0x001B2D13 File Offset: 0x001B1113
		public virtual void End()
		{
			GUI.EndGroup();
		}
	}
}
