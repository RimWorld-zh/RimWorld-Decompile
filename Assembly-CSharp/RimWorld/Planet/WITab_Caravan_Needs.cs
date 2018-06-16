using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008E7 RID: 2279
	public class WITab_Caravan_Needs : WITab
	{
		// Token: 0x0600345C RID: 13404 RVA: 0x001BFC7A File Offset: 0x001BE07A
		public WITab_Caravan_Needs()
		{
			this.labelKey = "TabCaravanNeeds";
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x0600345D RID: 13405 RVA: 0x001BFC90 File Offset: 0x001BE090
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

		// Token: 0x0600345E RID: 13406 RVA: 0x001BFCDE File Offset: 0x001BE0DE
		protected override void FillTab()
		{
			CaravanNeedsTabUtility.DoRows(this.size, base.SelCaravan.PawnsListForReading, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, ref this.specificNeedsTabForPawn, this.doNeeds);
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x001BFD18 File Offset: 0x001BE118
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

		// Token: 0x06003460 RID: 13408 RVA: 0x001BFDC0 File Offset: 0x001BE1C0
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

		// Token: 0x04001C53 RID: 7251
		private Vector2 scrollPosition;

		// Token: 0x04001C54 RID: 7252
		private float scrollViewHeight;

		// Token: 0x04001C55 RID: 7253
		private Pawn specificNeedsTabForPawn;

		// Token: 0x04001C56 RID: 7254
		private Vector2 thoughtScrollPosition;

		// Token: 0x04001C57 RID: 7255
		private bool doNeeds;
	}
}
