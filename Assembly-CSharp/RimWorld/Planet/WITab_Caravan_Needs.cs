using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008E3 RID: 2275
	public class WITab_Caravan_Needs : WITab
	{
		// Token: 0x06003457 RID: 13399 RVA: 0x001BFF2A File Offset: 0x001BE32A
		public WITab_Caravan_Needs()
		{
			this.labelKey = "TabCaravanNeeds";
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x001BFF40 File Offset: 0x001BE340
		private float SpecificNeedsTabWidth
		{
			get
			{
				float result;
				if (this.specificNeedsTabForPawn == null || this.specificNeedsTabForPawn.Destroyed)
				{
					result = 0f;
				}
				else
				{
					result = NeedsCardUtility.GetSize(this.specificNeedsTabForPawn).x;
				}
				return result;
			}
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x001BFF8E File Offset: 0x001BE38E
		protected override void FillTab()
		{
			CaravanNeedsTabUtility.DoRows(this.size, base.SelCaravan.PawnsListForReading, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, ref this.specificNeedsTabForPawn, this.doNeeds);
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x001BFFC8 File Offset: 0x001BE3C8
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, true);
			if (this.size.x + this.SpecificNeedsTabWidth > (float)UI.screenWidth)
			{
				this.doNeeds = false;
				this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, false);
			}
			else
			{
				this.doNeeds = true;
			}
			this.size.y = Mathf.Max(this.size.y, NeedsCardUtility.FullSize.y);
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x001C0070 File Offset: 0x001BE470
		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			Pawn localSpecificNeedsTabForPawn = this.specificNeedsTabForPawn;
			if (localSpecificNeedsTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificNeedsTabWidth = this.SpecificNeedsTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificNeedsTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (!localSpecificNeedsTabForPawn.DestroyedOrNull())
					{
						NeedsCardUtility.DoNeedsMoodAndThoughts(rect.AtZero(), localSpecificNeedsTabForPawn, ref this.thoughtScrollPosition);
						if (Widgets.CloseButtonFor(rect.AtZero()))
						{
							this.specificNeedsTabForPawn = null;
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				}, true, false, 1f);
			}
		}

		// Token: 0x04001C51 RID: 7249
		private Vector2 scrollPosition;

		// Token: 0x04001C52 RID: 7250
		private float scrollViewHeight;

		// Token: 0x04001C53 RID: 7251
		private Pawn specificNeedsTabForPawn;

		// Token: 0x04001C54 RID: 7252
		private Vector2 thoughtScrollPosition;

		// Token: 0x04001C55 RID: 7253
		private bool doNeeds;
	}
}
