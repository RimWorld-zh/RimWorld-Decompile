using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E7D RID: 3709
	public abstract class Listing
	{
		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06005751 RID: 22353 RVA: 0x001B2844 File Offset: 0x001B0C44
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06005753 RID: 22355 RVA: 0x001B2870 File Offset: 0x001B0C70
		// (set) Token: 0x06005752 RID: 22354 RVA: 0x001B285F File Offset: 0x001B0C5F
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

		// Token: 0x06005754 RID: 22356 RVA: 0x001B288B File Offset: 0x001B0C8B
		public void NewColumn()
		{
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x001B28B2 File Offset: 0x001B0CB2
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x001B28D4 File Offset: 0x001B0CD4
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x001B2919 File Offset: 0x001B0D19
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x001B292C File Offset: 0x001B0D2C
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001B299C File Offset: 0x001B0D9C
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

		// Token: 0x0600575A RID: 22362 RVA: 0x001B2A63 File Offset: 0x001B0E63
		public virtual void End()
		{
			GUI.EndGroup();
		}

		// Token: 0x040039D1 RID: 14801
		public float verticalSpacing = 2f;

		// Token: 0x040039D2 RID: 14802
		protected Rect listingRect;

		// Token: 0x040039D3 RID: 14803
		protected float curY = 0f;

		// Token: 0x040039D4 RID: 14804
		protected float curX = 0f;

		// Token: 0x040039D5 RID: 14805
		private float columnWidthInt;

		// Token: 0x040039D6 RID: 14806
		private bool hasCustomColumnWidth;

		// Token: 0x040039D7 RID: 14807
		public const float ColumnSpacing = 17f;

		// Token: 0x040039D8 RID: 14808
		private const float DefaultGap = 12f;
	}
}
