using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008A2 RID: 2210
	public class ResourceReadout
	{
		// Token: 0x0600328F RID: 12943 RVA: 0x001B31D5 File Offset: 0x001B15D5
		public ResourceReadout()
		{
			this.RootThingCategories = (from cat in DefDatabase<ThingCategoryDef>.AllDefs
			where cat.resourceReadoutRoot
			select cat).ToList<ThingCategoryDef>();
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x001B3210 File Offset: 0x001B1610
		public void ResourceReadoutOnGUI()
		{
			if (Event.current.type != EventType.Layout)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu)
					{
						GenUI.DrawTextWinterShadow(new Rect(256f, 512f, -256f, -512f));
						Text.Font = GameFont.Small;
						Rect rect = (!Prefs.ResourceReadoutCategorized) ? new Rect(7f, 7f, 110f, (float)(UI.screenHeight - 7) - 200f) : new Rect(2f, 7f, 124f, (float)(UI.screenHeight - 7) - 200f);
						Rect rect2 = new Rect(0f, 0f, rect.width, this.lastDrawnHeight);
						bool flag = rect2.height > rect.height;
						if (flag)
						{
							Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, false);
						}
						else
						{
							this.scrollPosition = Vector2.zero;
							GUI.BeginGroup(rect);
						}
						if (!Prefs.ResourceReadoutCategorized)
						{
							this.DoReadoutSimple(rect2, rect.height);
						}
						else
						{
							this.DoReadoutCategorized(rect2);
						}
						if (flag)
						{
							Widgets.EndScrollView();
						}
						else
						{
							GUI.EndGroup();
						}
					}
				}
			}
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x001B3368 File Offset: 0x001B1768
		private void DoReadoutCategorized(Rect rect)
		{
			Listing_ResourceReadout listing_ResourceReadout = new Listing_ResourceReadout(Find.CurrentMap);
			listing_ResourceReadout.Begin(rect);
			listing_ResourceReadout.nestIndentWidth = 7f;
			listing_ResourceReadout.lineHeight = 24f;
			listing_ResourceReadout.verticalSpacing = 0f;
			for (int i = 0; i < this.RootThingCategories.Count; i++)
			{
				listing_ResourceReadout.DoCategory(this.RootThingCategories[i].treeNode, 0, 32);
			}
			listing_ResourceReadout.End();
			this.lastDrawnHeight = listing_ResourceReadout.CurHeight;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x001B33F8 File Offset: 0x001B17F8
		private void DoReadoutSimple(Rect rect, float outRectHeight)
		{
			GUI.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in Find.CurrentMap.resourceCounter.AllCountedAmounts)
			{
				if (keyValuePair.Value > 0 || keyValuePair.Key.resourceReadoutAlwaysShow)
				{
					Rect rect2 = new Rect(0f, num, 999f, 24f);
					if (rect2.yMax >= this.scrollPosition.y && rect2.y <= this.scrollPosition.y + outRectHeight)
					{
						this.DrawResourceSimple(rect2, keyValuePair.Key);
					}
					num += 24f;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.lastDrawnHeight = num;
			GUI.EndGroup();
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x001B3500 File Offset: 0x001B1900
		public void DrawResourceSimple(Rect rect, ThingDef thingDef)
		{
			this.DrawIcon(rect.x, rect.y, thingDef);
			rect.y += 2f;
			int count = Find.CurrentMap.resourceCounter.GetCount(thingDef);
			Rect rect2 = new Rect(34f, rect.y, rect.width - 34f, rect.height);
			Widgets.Label(rect2, count.ToStringCached());
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x001B357C File Offset: 0x001B197C
		private void DrawIcon(float x, float y, ThingDef thingDef)
		{
			Rect rect = new Rect(x, y, 27f, 27f);
			Color color = GUI.color;
			Widgets.ThingIcon(rect, thingDef);
			GUI.color = color;
			TooltipHandler.TipRegion(rect, new TipSignal(() => thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst(), thingDef.GetHashCode()));
		}

		// Token: 0x04001B08 RID: 6920
		private Vector2 scrollPosition;

		// Token: 0x04001B09 RID: 6921
		private float lastDrawnHeight;

		// Token: 0x04001B0A RID: 6922
		private readonly List<ThingCategoryDef> RootThingCategories;

		// Token: 0x04001B0B RID: 6923
		private const float LineHeightSimple = 24f;

		// Token: 0x04001B0C RID: 6924
		private const float LineHeightCategorized = 24f;

		// Token: 0x04001B0D RID: 6925
		private const float DistFromScreenBottom = 200f;
	}
}
