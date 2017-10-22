using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class ThingFilterUI
	{
		private static float viewHeight;

		private const float ExtraViewHeight = 90f;

		private const float RangeLabelTab = 10f;

		private const float RangeLabelHeight = 19f;

		private const float SliderHeight = 28f;

		private const float SliderTab = 20f;

		public static void DoThingFilterConfigWindow(Rect rect, ref Vector2 scrollPosition, ThingFilter filter, ThingFilter parentFilter = null, int openMask = 1, IEnumerable<ThingDef> forceHiddenDefs = null, IEnumerable<SpecialThingFilterDef> forceHiddenFilters = null, List<ThingDef> suppressSmallVolumeTags = null)
		{
			Widgets.DrawMenuSection(rect);
			Text.Font = GameFont.Tiny;
			float num = (float)(rect.width - 2.0);
			Rect rect2 = new Rect((float)(rect.x + 1.0), (float)(rect.y + 1.0), (float)(num / 2.0), 24f);
			if (Widgets.ButtonText(rect2, "ClearAll".Translate(), true, false, true))
			{
				filter.SetDisallowAll(forceHiddenDefs, forceHiddenFilters);
			}
			Rect rect3 = new Rect((float)(rect2.xMax + 1.0), rect2.y, (float)(rect.xMax - 1.0 - (rect2.xMax + 1.0)), 24f);
			if (Widgets.ButtonText(rect3, "AllowAll".Translate(), true, false, true))
			{
				filter.SetAllowAll(parentFilter);
			}
			Text.Font = GameFont.Small;
			rect.yMin = rect2.yMax;
			TreeNode_ThingCategory node = ThingCategoryNodeDatabase.RootNode;
			bool flag = true;
			bool flag2 = true;
			if (parentFilter != null)
			{
				node = parentFilter.DisplayRootCategory;
				flag = parentFilter.allowedHitPointsConfigurable;
				flag2 = parentFilter.allowedQualitiesConfigurable;
			}
			Rect viewRect = new Rect(0f, 0f, (float)(rect.width - 16.0), ThingFilterUI.viewHeight);
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			float num2 = 2f;
			if (flag)
			{
				ThingFilterUI.DrawHitPointsFilterConfig(ref num2, viewRect.width, filter);
			}
			if (flag2)
			{
				ThingFilterUI.DrawQualityFilterConfig(ref num2, viewRect.width, filter);
			}
			float num3 = num2;
			Rect rect4 = new Rect(0f, num2, viewRect.width, 9999f);
			Listing_TreeThingFilter listing_TreeThingFilter = new Listing_TreeThingFilter(filter, parentFilter, forceHiddenDefs, forceHiddenFilters, suppressSmallVolumeTags);
			listing_TreeThingFilter.Begin(rect4);
			listing_TreeThingFilter.DoCategoryChildren(node, 0, openMask, true);
			listing_TreeThingFilter.End();
			if (Event.current.type == EventType.Layout)
			{
				ThingFilterUI.viewHeight = (float)(num3 + listing_TreeThingFilter.CurHeight + 90.0);
			}
			Widgets.EndScrollView();
		}

		private static void DrawHitPointsFilterConfig(ref float y, float width, ThingFilter filter)
		{
			Rect rect = new Rect(20f, y, (float)(width - 20.0), 28f);
			FloatRange allowedHitPointsPercents = filter.AllowedHitPointsPercents;
			Widgets.FloatRange(rect, 1, ref allowedHitPointsPercents, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
			filter.AllowedHitPointsPercents = allowedHitPointsPercents;
			y += 28f;
			y += 5f;
			Text.Font = GameFont.Small;
		}

		private static void DrawQualityFilterConfig(ref float y, float width, ThingFilter filter)
		{
			Rect rect = new Rect(20f, y, (float)(width - 20.0), 28f);
			QualityRange allowedQualityLevels = filter.AllowedQualityLevels;
			Widgets.QualityRange(rect, 2, ref allowedQualityLevels);
			filter.AllowedQualityLevels = allowedQualityLevels;
			y += 28f;
			y += 5f;
			Text.Font = GameFont.Small;
		}
	}
}
