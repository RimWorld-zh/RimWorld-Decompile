using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC0 RID: 3776
	public abstract class Dialog_OptionLister : Window
	{
		// Token: 0x04003B9D RID: 15261
		protected Vector2 scrollPosition;

		// Token: 0x04003B9E RID: 15262
		protected string filter = "";

		// Token: 0x04003B9F RID: 15263
		protected float totalOptionsHeight = 0f;

		// Token: 0x04003BA0 RID: 15264
		protected Listing_Standard listing;

		// Token: 0x06005958 RID: 22872 RVA: 0x002BB7F7 File Offset: 0x002B9BF7
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06005959 RID: 22873 RVA: 0x002BB82C File Offset: 0x002B9C2C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x0600595A RID: 22874 RVA: 0x002BB854 File Offset: 0x002B9C54
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x002BB86C File Offset: 0x002B9C6C
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

		// Token: 0x0600595C RID: 22876 RVA: 0x002BB99A File Offset: 0x002B9D9A
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x0600595D RID: 22877
		protected abstract void DoListingItems();

		// Token: 0x0600595E RID: 22878 RVA: 0x002BB9A8 File Offset: 0x002B9DA8
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}
	}
}
