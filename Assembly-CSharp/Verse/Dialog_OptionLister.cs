using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBE RID: 3774
	public abstract class Dialog_OptionLister : Window
	{
		// Token: 0x06005934 RID: 22836 RVA: 0x002B9927 File Offset: 0x002B7D27
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06005935 RID: 22837 RVA: 0x002B995C File Offset: 0x002B7D5C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06005936 RID: 22838 RVA: 0x002B9984 File Offset: 0x002B7D84
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x002B999C File Offset: 0x002B7D9C
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

		// Token: 0x06005938 RID: 22840 RVA: 0x002B9ACA File Offset: 0x002B7ECA
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x06005939 RID: 22841
		protected abstract void DoListingItems();

		// Token: 0x0600593A RID: 22842 RVA: 0x002B9AD8 File Offset: 0x002B7ED8
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04003B85 RID: 15237
		protected Vector2 scrollPosition;

		// Token: 0x04003B86 RID: 15238
		protected string filter = "";

		// Token: 0x04003B87 RID: 15239
		protected float totalOptionsHeight = 0f;

		// Token: 0x04003B88 RID: 15240
		protected Listing_Standard listing;
	}
}
