using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x020008A4 RID: 2212
	public class Dialog_SellableItems : Window
	{
		// Token: 0x0600329D RID: 12957 RVA: 0x001B4458 File Offset: 0x001B2858
		public Dialog_SellableItems(TraderKindDef trader)
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.CalculateSellableItems(trader);
			this.CalculateTabs();
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x001B44C0 File Offset: 0x001B28C0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(650f, (float)Mathf.Min(UI.screenHeight, 1000));
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x001B44F0 File Offset: 0x001B28F0
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x001B450C File Offset: 0x001B290C
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 60f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SellableItemsTitle".Translate().CapitalizeFirst());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			inRect.yMin += 124f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, this.tabs, 2);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect outRect = rect2;
			outRect.yMax -= 65f;
			List<ThingDef> sellableItemsInCategory = this.GetSellableItemsInCategory(this.currentCategory, this.pawnsTabOpen);
			if (sellableItemsInCategory.Any<ThingDef>())
			{
				float height = (float)sellableItemsInCategory.Count * 24f;
				float num = 0f;
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
				Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
				float num2 = this.scrollPosition.y - 24f;
				float num3 = this.scrollPosition.y + outRect.height;
				for (int i = 0; i < sellableItemsInCategory.Count; i++)
				{
					if (num > num2 && num < num3)
					{
						Rect rect3 = new Rect(0f, num, viewRect.width, 24f);
						Profiler.BeginSample("DoRow()");
						this.DoRow(rect3, sellableItemsInCategory[i], i);
						Profiler.EndSample();
					}
					num += 24f;
				}
				Widgets.EndScrollView();
			}
			else
			{
				Widgets.NoneLabel(0f, outRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x001B46EC File Offset: 0x001B2AEC
		private void DoRow(Rect rect, ThingDef thingDef, int index)
		{
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, thingDef.description);
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(4f, (rect.height - 20f) / 2f, 20f, 20f);
			Widgets.ThingIcon(rect2, thingDef);
			Rect rect3 = new Rect(rect2.xMax + 4f, 0f, rect.width, 24f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect3, thingDef.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.EndGroup();
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x001B4798 File Offset: 0x001B2B98
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "CloseButton".Translate(), true, false, true))
			{
				this.Close(true);
			}
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x001B4818 File Offset: 0x001B2C18
		private void CalculateSellableItems(TraderKindDef trader)
		{
			this.sellableItems.Clear();
			this.cachedSellableItemsByCategory.Clear();
			this.cachedSellablePawns = null;
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].PlayerAcquirable && !allDefsListForReading[i].IsCorpse && !typeof(MinifiedThing).IsAssignableFrom(allDefsListForReading[i].thingClass) && trader.WillTrade(allDefsListForReading[i]))
				{
					this.sellableItems.Add(allDefsListForReading[i]);
				}
			}
			this.sellableItems.SortBy((ThingDef x) => x.label);
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x001B48F8 File Offset: 0x001B2CF8
		private void CalculateTabs()
		{
			this.tabs.Clear();
			List<ThingCategoryDef> allDefsListForReading = DefDatabase<ThingCategoryDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingCategoryDef category = allDefsListForReading[i];
				if (category.parent == ThingCategoryDefOf.Root && this.AnyTraderWillEverTrade(category))
				{
					if (this.currentCategory == null)
					{
						this.currentCategory = category;
					}
					this.tabs.Add(new TabRecord(category.LabelCap, delegate()
					{
						this.currentCategory = category;
						this.pawnsTabOpen = false;
					}, () => this.currentCategory == category));
				}
			}
			this.tabs.Add(new TabRecord("PawnsTabShort".Translate(), delegate()
			{
				this.currentCategory = null;
				this.pawnsTabOpen = true;
			}, () => this.pawnsTabOpen));
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x001B49F4 File Offset: 0x001B2DF4
		private List<ThingDef> GetSellableItemsInCategory(ThingCategoryDef category, bool pawns)
		{
			List<ThingDef> result;
			List<ThingDef> list;
			if (pawns)
			{
				if (this.cachedSellablePawns == null)
				{
					this.cachedSellablePawns = new List<ThingDef>();
					for (int i = 0; i < this.sellableItems.Count; i++)
					{
						if (this.sellableItems[i].category == ThingCategory.Pawn)
						{
							this.cachedSellablePawns.Add(this.sellableItems[i]);
						}
					}
				}
				result = this.cachedSellablePawns;
			}
			else if (this.cachedSellableItemsByCategory.TryGetValue(category, out list))
			{
				result = list;
			}
			else
			{
				list = new List<ThingDef>();
				for (int j = 0; j < this.sellableItems.Count; j++)
				{
					if (this.sellableItems[j].IsWithinCategory(category))
					{
						list.Add(this.sellableItems[j]);
					}
				}
				this.cachedSellableItemsByCategory.Add(category, list);
				result = list;
			}
			return result;
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x001B4AF8 File Offset: 0x001B2EF8
		private bool AnyTraderWillEverTrade(ThingCategoryDef thingCategory)
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].IsWithinCategory(thingCategory))
				{
					List<TraderKindDef> allDefsListForReading2 = DefDatabase<TraderKindDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading2.Count; j++)
					{
						if (allDefsListForReading2[j].WillTrade(allDefsListForReading[i]))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04001B30 RID: 6960
		private ThingCategoryDef currentCategory;

		// Token: 0x04001B31 RID: 6961
		private bool pawnsTabOpen;

		// Token: 0x04001B32 RID: 6962
		private List<ThingDef> sellableItems = new List<ThingDef>();

		// Token: 0x04001B33 RID: 6963
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x04001B34 RID: 6964
		private Vector2 scrollPosition;

		// Token: 0x04001B35 RID: 6965
		private List<ThingDef> cachedSellablePawns;

		// Token: 0x04001B36 RID: 6966
		private Dictionary<ThingCategoryDef, List<ThingDef>> cachedSellableItemsByCategory = new Dictionary<ThingCategoryDef, List<ThingDef>>();

		// Token: 0x04001B37 RID: 6967
		private const float RowHeight = 24f;

		// Token: 0x04001B38 RID: 6968
		private const float IconMargin = 4f;

		// Token: 0x04001B39 RID: 6969
		private const float IconSize = 20f;

		// Token: 0x04001B3A RID: 6970
		private const float TitleRectHeight = 60f;

		// Token: 0x04001B3B RID: 6971
		private const float BottomAreaHeight = 55f;

		// Token: 0x04001B3C RID: 6972
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);
	}
}
