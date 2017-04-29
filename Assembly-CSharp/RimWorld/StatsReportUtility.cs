using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats());
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def, stuff)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries, def);
			}
			StatsReportUtility.DrawStatsWorker(rect, null);
		}

		public static void DrawStatsReport(Rect rect, Thing thing)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(thing.def.SpecialDisplayStats());
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(thing)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.cachedDrawEntries.RemoveAll((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries, thing.def);
			}
			StatsReportUtility.DrawStatsWorker(rect, thing);
		}

		[DebuggerHidden]
		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			StatsReportUtility.<StatsToDraw>c__Iterator195 <StatsToDraw>c__Iterator = new StatsReportUtility.<StatsToDraw>c__Iterator195();
			<StatsToDraw>c__Iterator.def = def;
			<StatsToDraw>c__Iterator.stuff = stuff;
			<StatsToDraw>c__Iterator.<$>def = def;
			<StatsToDraw>c__Iterator.<$>stuff = stuff;
			StatsReportUtility.<StatsToDraw>c__Iterator195 expr_23 = <StatsToDraw>c__Iterator;
			expr_23.$PC = -2;
			return expr_23;
		}

		[DebuggerHidden]
		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			StatsReportUtility.<StatsToDraw>c__Iterator196 <StatsToDraw>c__Iterator = new StatsReportUtility.<StatsToDraw>c__Iterator196();
			<StatsToDraw>c__Iterator.thing = thing;
			<StatsToDraw>c__Iterator.<$>thing = thing;
			StatsReportUtility.<StatsToDraw>c__Iterator196 expr_15 = <StatsToDraw>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original, Def def)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
			if (StatsReportUtility.cachedDrawEntries.Count > 0)
			{
				StatsReportUtility.SelectEntry(StatsReportUtility.cachedDrawEntries[0], false);
			}
		}

		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999)
			{
				overrideReportText = def.description
			};
		}

		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999)
			{
				overrideReportText = thing.GetDescription()
			};
		}

		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat;
			if (!t.TryGetQuality(out cat))
			{
				return null;
			}
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), 99999)
			{
				overrideReportText = "QualityDescription".Translate()
			};
		}

		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
			}
		}

		private static void DrawStatsWorker(Rect rect, Thing optionalThing)
		{
			Rect outRect = new Rect(rect);
			outRect.width *= 0.5f;
			Rect rect2 = new Rect(rect);
			rect2.x = outRect.xMax;
			rect2.width = rect.xMax - rect2.x;
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, StatsReportUtility.listHeight);
			Widgets.BeginScrollView(outRect, ref StatsReportUtility.scrollPosition, viewRect);
			float num = 0f;
			string b = null;
			foreach (StatDrawEntry ent in StatsReportUtility.cachedDrawEntries)
			{
				if (ent.category.LabelCap != b)
				{
					Widgets.ListSeparator(ref num, viewRect.width, ent.category.LabelCap);
					b = ent.category.LabelCap;
				}
				num += ent.Draw(8f, num, viewRect.width - 8f, StatsReportUtility.selectedEntry == ent, delegate
				{
					StatsReportUtility.SelectEntry(ent, true);
				});
			}
			StatsReportUtility.listHeight = num + 100f;
			Widgets.EndScrollView();
			Rect rect3 = rect2.ContractedBy(10f);
			GUI.BeginGroup(rect3);
			if (StatsReportUtility.selectedEntry != null)
			{
				StatRequest optionalReq;
				if (StatsReportUtility.selectedEntry.hasOptionalReq)
				{
					optionalReq = StatsReportUtility.selectedEntry.optionalReq;
				}
				else if (optionalThing != null)
				{
					optionalReq = StatRequest.For(optionalThing);
				}
				else
				{
					optionalReq = StatRequest.ForEmpty();
				}
				string explanationText = StatsReportUtility.selectedEntry.GetExplanationText(optionalReq);
				Rect rect4 = rect3.AtZero();
				Widgets.Label(rect4, explanationText);
			}
			GUI.EndGroup();
		}
	}
}
