using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ResourceReadout
	{
		private Vector2 scrollPosition;

		private float lastDrawnHeight;

		private readonly List<ThingCategoryDef> RootThingCategories;

		private const float LineHeightSimple = 24f;

		private const float LineHeightCategorized = 24f;

		private const float DistFromScreenBottom = 200f;

		[CompilerGenerated]
		private static Func<ThingCategoryDef, bool> <>f__am$cache0;

		public ResourceReadout()
		{
			this.RootThingCategories = (from cat in DefDatabase<ThingCategoryDef>.AllDefs
			where cat.resourceReadoutRoot
			select cat).ToList<ThingCategoryDef>();
		}

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

		public void DrawResourceSimple(Rect rect, ThingDef thingDef)
		{
			this.DrawIcon(rect.x, rect.y, thingDef);
			rect.y += 2f;
			int count = Find.CurrentMap.resourceCounter.GetCount(thingDef);
			Rect rect2 = new Rect(34f, rect.y, rect.width - 34f, rect.height);
			Widgets.Label(rect2, count.ToStringCached());
		}

		private void DrawIcon(float x, float y, ThingDef thingDef)
		{
			Rect rect = new Rect(x, y, 27f, 27f);
			Color color = GUI.color;
			Widgets.ThingIcon(rect, thingDef);
			GUI.color = color;
			TooltipHandler.TipRegion(rect, new TipSignal(() => thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst(), thingDef.GetHashCode()));
		}

		[CompilerGenerated]
		private static bool <ResourceReadout>m__0(ThingCategoryDef cat)
		{
			return cat.resourceReadoutRoot;
		}

		[CompilerGenerated]
		private sealed class <DrawIcon>c__AnonStorey0
		{
			internal ThingDef thingDef;

			public <DrawIcon>c__AnonStorey0()
			{
			}

			internal string <>m__0()
			{
				return this.thingDef.LabelCap + ": " + this.thingDef.description.CapitalizeFirst();
			}
		}
	}
}
