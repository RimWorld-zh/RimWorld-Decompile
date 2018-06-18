using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E7C RID: 3708
	public abstract class Listing
	{
		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x0600574F RID: 22351 RVA: 0x001B290C File Offset: 0x001B0D0C
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06005751 RID: 22353 RVA: 0x001B2938 File Offset: 0x001B0D38
		// (set) Token: 0x06005750 RID: 22352 RVA: 0x001B2927 File Offset: 0x001B0D27
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

		// Token: 0x06005752 RID: 22354 RVA: 0x001B2953 File Offset: 0x001B0D53
		public void NewColumn()
		{
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x001B297A File Offset: 0x001B0D7A
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x001B299C File Offset: 0x001B0D9C
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x001B29E1 File Offset: 0x001B0DE1
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x001B29F4 File Offset: 0x001B0DF4
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x001B2A64 File Offset: 0x001B0E64
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

		// Token: 0x06005758 RID: 22360 RVA: 0x001B2B2B File Offset: 0x001B0F2B
		public virtual void End()
		{
			GUI.EndGroup();
		}

		// Token: 0x040039CF RID: 14799
		public float verticalSpacing = 2f;

		// Token: 0x040039D0 RID: 14800
		protected Rect listingRect;

		// Token: 0x040039D1 RID: 14801
		protected float curY = 0f;

		// Token: 0x040039D2 RID: 14802
		protected float curX = 0f;

		// Token: 0x040039D3 RID: 14803
		private float columnWidthInt;

		// Token: 0x040039D4 RID: 14804
		private bool hasCustomColumnWidth;

		// Token: 0x040039D5 RID: 14805
		public const float ColumnSpacing = 17f;

		// Token: 0x040039D6 RID: 14806
		private const float DefaultGap = 12f;
	}
}
