using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	public class WITab_Caravan_Needs : WITab
	{
		private Vector2 scrollPosition;

		private float scrollViewHeight;

		private static List<Thing> tmpThings = new List<Thing>();

		private Pawn specificNeedsTabForPawn;

		private Vector2 thoughtScrollPosition;

		private bool doNeeds;

		private float SpecificNeedsTabWidth
		{
			get
			{
				if (this.specificNeedsTabForPawn != null && !this.specificNeedsTabForPawn.Destroyed)
				{
					Vector2 size = NeedsCardUtility.GetSize(this.specificNeedsTabForPawn);
					return size.x;
				}
				return 0f;
			}
		}

		public WITab_Caravan_Needs()
		{
			base.labelKey = "TabCaravanNeeds";
		}

		protected override void FillTab()
		{
			this.AddPawnsToTmpThings();
			CaravanPeopleAndItemsTabUtility.DoRows(base.size, WITab_Caravan_Needs.tmpThings, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, false, ref this.specificNeedsTabForPawn, this.doNeeds);
			WITab_Caravan_Needs.tmpThings.Clear();
		}

		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.AddPawnsToTmpThings();
			base.size = CaravanPeopleAndItemsTabUtility.GetSize(WITab_Caravan_Needs.tmpThings, this.PaneTopY, true);
			if (base.size.x + this.SpecificNeedsTabWidth > (float)UI.screenWidth)
			{
				this.doNeeds = false;
				base.size = CaravanPeopleAndItemsTabUtility.GetSize(WITab_Caravan_Needs.tmpThings, this.PaneTopY, false);
			}
			else
			{
				this.doNeeds = true;
			}
			ref Vector2 val = ref base.size;
			float y = base.size.y;
			Vector2 fullSize = NeedsCardUtility.FullSize;
			val.y = Mathf.Max(y, fullSize.y);
			WITab_Caravan_Needs.tmpThings.Clear();
		}

		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			Pawn localSpecificNeedsTabForPawn = this.specificNeedsTabForPawn;
			if (localSpecificNeedsTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificNeedsTabWidth = this.SpecificNeedsTabWidth;
				Rect rect = new Rect((float)(tabRect.xMax - 1.0), tabRect.yMin, specificNeedsTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, (Action)delegate
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

		private void AddPawnsToTmpThings()
		{
			WITab_Caravan_Needs.tmpThings.Clear();
			Caravan selCaravan = base.SelCaravan;
			List<Pawn> pawnsListForReading = selCaravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				WITab_Caravan_Needs.tmpThings.Add(pawnsListForReading[i]);
			}
		}
	}
}
