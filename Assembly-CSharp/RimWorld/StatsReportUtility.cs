using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class StatsReportUtility
	{
		private static StatDrawEntry selectedEntry;

		private static Vector2 scrollPosition;

		private static float listHeight;

		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();

		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.cachedDrawEntries.Clear();
		}

		public static void DrawStatsReport(Rect rect, Def def, ThingDef stuff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats());
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def, stuff)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		public static void DrawStatsReport(Rect rect, Thing thing)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(thing.def.SpecialDisplayStats());
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(thing)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, thing, null);
		}

		public static void DrawStatsReport(Rect rect, WorldObject worldObject)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(worldObject.def.SpecialDisplayStats());
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(worldObject)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, worldObject);
		}

		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			_003CStatsToDraw_003Ec__Iterator0 _003CStatsToDraw_003Ec__Iterator = (_003CStatsToDraw_003Ec__Iterator0)/*Error near IL_0038: stateMachine*/;
			yield return StatsReportUtility.DescriptionEntry(def);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			yield return StatsReportUtility.DescriptionEntry(thing);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			yield return StatsReportUtility.DescriptionEntry(worldObject);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList();
			if (StatsReportUtility.cachedDrawEntries.Count > 0)
			{
				StatsReportUtility.SelectEntry(StatsReportUtility.cachedDrawEntries[0], false);
			}
		}

		private static StatDrawEntry DescriptionEntry(Def def)
		{
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999, string.Empty);
			statDrawEntry.overrideReportText = def.description;
			return statDrawEntry;
		}

		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999, string.Empty);
			statDrawEntry.overrideReportText = thing.GetDescription();
			return statDrawEntry;
		}

		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999, string.Empty);
			statDrawEntry.overrideReportText = worldObject.GetDescription();
			return statDrawEntry;
		}

		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat = default(QualityCategory);
			if (!t.TryGetQuality(out cat))
			{
				return null;
			}
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), 99999, string.Empty);
			statDrawEntry.overrideReportText = "QualityDescription".Translate();
			return statDrawEntry;
		}

		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
			}
		}

		private static void DrawStatsWorker(Rect rect, Thing optionalThing, WorldObject optionalWorldObject)
		{
			Rect outRect = new Rect(rect);
			outRect.width *= 0.5f;
			Rect rect2 = new Rect(rect);
			rect2.x = outRect.xMax;
			rect2.width = rect.xMax - rect2.x;
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, (float)(outRect.width - 16.0), StatsReportUtility.listHeight);
			Widgets.BeginScrollView(outRect, ref StatsReportUtility.scrollPosition, viewRect, true);
			float num = 0f;
			string b = null;
			foreach (StatDrawEntry cachedDrawEntry in StatsReportUtility.cachedDrawEntries)
			{
				if (cachedDrawEntry.category.LabelCap != b)
				{
					Widgets.ListSeparator(ref num, viewRect.width, cachedDrawEntry.category.LabelCap);
					b = cachedDrawEntry.category.LabelCap;
				}
				num += cachedDrawEntry.Draw(8f, num, (float)(viewRect.width - 8.0), StatsReportUtility.selectedEntry == cachedDrawEntry, delegate
				{
					StatsReportUtility.SelectEntry(cachedDrawEntry, true);
				});
			}
			StatsReportUtility.listHeight = (float)(num + 100.0);
			Widgets.EndScrollView();
			Rect rect3 = rect2.ContractedBy(10f);
			GUI.BeginGroup(rect3);
			if (StatsReportUtility.selectedEntry != null)
			{
				StatRequest optionalReq = (!StatsReportUtility.selectedEntry.hasOptionalReq) ? ((optionalThing == null) ? StatRequest.ForEmpty() : StatRequest.For(optionalThing)) : StatsReportUtility.selectedEntry.optionalReq;
				string explanationText = StatsReportUtility.selectedEntry.GetExplanationText(optionalReq);
				Rect rect4 = rect3.AtZero();
				Widgets.Label(rect4, explanationText);
			}
			GUI.EndGroup();
		}
	}
}
