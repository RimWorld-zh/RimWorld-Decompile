using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008E5 RID: 2277
	public class WITab_Caravan_Needs : WITab
	{
		// Token: 0x04001C57 RID: 7255
		private Vector2 scrollPosition;

		// Token: 0x04001C58 RID: 7256
		private float scrollViewHeight;

		// Token: 0x04001C59 RID: 7257
		private Pawn specificNeedsTabForPawn;

		// Token: 0x04001C5A RID: 7258
		private Vector2 thoughtScrollPosition;

		// Token: 0x04001C5B RID: 7259
		private bool doNeeds;

		// Token: 0x0600345B RID: 13403 RVA: 0x001C033E File Offset: 0x001BE73E
		public WITab_Caravan_Needs()
		{
			this.labelKey = "TabCaravanNeeds";
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x0600345C RID: 13404 RVA: 0x001C0354 File Offset: 0x001BE754
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

		// Token: 0x0600345D RID: 13405 RVA: 0x001C03A2 File Offset: 0x001BE7A2
		protected override void FillTab()
		{
			CaravanNeedsTabUtility.DoRows(this.size, base.SelCaravan.PawnsListForReading, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, ref this.specificNeedsTabForPawn, this.doNeeds);
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x001C03DC File Offset: 0x001BE7DC
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

		// Token: 0x0600345F RID: 13407 RVA: 0x001C0484 File Offset: 0x001BE884
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
	}
}
