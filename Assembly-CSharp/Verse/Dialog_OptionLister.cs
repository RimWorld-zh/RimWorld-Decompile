using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBD RID: 3773
	public abstract class Dialog_OptionLister : Window
	{
		// Token: 0x06005955 RID: 22869 RVA: 0x002BB4DF File Offset: 0x002B98DF
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06005956 RID: 22870 RVA: 0x002BB514 File Offset: 0x002B9914
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06005957 RID: 22871 RVA: 0x002BB53C File Offset: 0x002B993C
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x002BB554 File Offset: 0x002B9954
		public override void DoWindowContents(Rect inRect)
		{
			this.filter = Widgets.TextField(new Rect(0f, 0f, 200f, 30f), this.filter);
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 35f;
			int num = (int)(this.InitialSize.x / 200f);
			float num2 = (this.totalOptionsHeight + 24f * (float)(num - 1)) / (float)num;
			if (num2 < outRect.height)
			{
				num2 = outRect.height;
			}
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, num2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			this.listing = new Listing_Standard();
			this.listing.ColumnWidth = (rect.width - 17f * (float)(num - 1)) / (float)num;
			this.listing.Begin(rect);
			this.DoListingItems();
			this.listing.End();
			Widgets.EndScrollView();
		}

		// Token: 0x06005959 RID: 22873 RVA: 0x002BB682 File Offset: 0x002B9A82
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x0600595A RID: 22874
		protected abstract void DoListingItems();

		// Token: 0x0600595B RID: 22875 RVA: 0x002BB690 File Offset: 0x002B9A90
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04003B95 RID: 15253
		protected Vector2 scrollPosition;

		// Token: 0x04003B96 RID: 15254
		protected string filter = "";

		// Token: 0x04003B97 RID: 15255
		protected float totalOptionsHeight = 0f;

		// Token: 0x04003B98 RID: 15256
		protected Listing_Standard listing;
	}
}
