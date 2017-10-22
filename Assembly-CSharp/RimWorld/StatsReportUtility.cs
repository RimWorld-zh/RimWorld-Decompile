using RimWorld.Planet;
using System;
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
				StatsReportUtility.cachedDrawEntries.RemoveAll((Predicate<StatDrawEntry>)((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract));
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
				StatsReportUtility.cachedDrawEntries.RemoveAll((Predicate<StatDrawEntry>)((StatDrawEntry de) => de.stat != null && !de.stat.showNonAbstract));
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, worldObject);
		}

		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			BuildableDef eDef = def as BuildableDef;
			if (eDef != null)
			{
				foreach (StatDef item in from st in DefDatabase<StatDef>.AllDefs
				where st.Worker.ShouldShowFor(((_003CStatsToDraw_003Ec__Iterator199)/*Error near IL_0066: stateMachine*/)._003CeDef_003E__0)
				select st)
				{
					yield return new StatDrawEntry(item.category, item, eDef.GetStatValueAbstract(item, stuff), StatRequest.For(eDef, stuff, QualityCategory.Normal), ToStringNumberSense.Undefined);
				}
			}
		}

		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			yield return StatsReportUtility.DescriptionEntry(thing);
			StatDrawEntry qe = StatsReportUtility.QualityEntry(thing);
			if (qe != null)
			{
				yield return qe;
			}
			foreach (StatDef item in from st in DefDatabase<StatDef>.AllDefs
			where st.Worker.ShouldShowFor(((_003CStatsToDraw_003Ec__Iterator19A)/*Error near IL_0096: stateMachine*/).thing.def)
			select st)
			{
				yield return new StatDrawEntry(item.category, item, thing.GetStatValue(item, true), StatRequest.For(thing), ToStringNumberSense.Undefined);
			}
			if (thing.def.useHitPoints)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "HitPointsBasic".Translate().CapitalizeFirst(), thing.HitPoints.ToString() + " / " + thing.MaxHitPoints.ToString(), 0)
				{
					overrideReportText = "HitPointsBasic".Translate().CapitalizeFirst() + ":\n\n" + thing.HitPoints.ToString() + "\n\n" + StatDefOf.MaxHitPoints.LabelCap + ":\n\n" + StatDefOf.MaxHitPoints.Worker.GetExplanation(StatRequest.For(thing), ToStringNumberSense.Absolute)
				};
			}
			foreach (StatDrawEntry specialDisplayStat in thing.SpecialDisplayStats)
			{
				yield return specialDisplayStat;
			}
			if (!thing.def.equippedStatOffsets.NullOrEmpty())
			{
				for (int k = 0; k < thing.def.equippedStatOffsets.Count; k++)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, thing.def.equippedStatOffsets[k].stat, thing.def.equippedStatOffsets[k].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset);
				}
			}
			if (thing.def.IsStuff)
			{
				if (!thing.def.stuffProps.statFactors.NullOrEmpty())
				{
					for (int j = 0; j < thing.def.stuffProps.statFactors.Count; j++)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, thing.def.stuffProps.statFactors[j].stat, thing.def.stuffProps.statFactors[j].value, StatRequest.ForEmpty(), ToStringNumberSense.Factor);
					}
				}
				if (!thing.def.stuffProps.statOffsets.NullOrEmpty())
				{
					for (int i = 0; i < thing.def.stuffProps.statOffsets.Count; i++)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, thing.def.stuffProps.statOffsets[i].stat, thing.def.stuffProps.statOffsets[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset);
					}
				}
			}
		}

		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			yield return StatsReportUtility.DescriptionEntry(worldObject);
			foreach (StatDrawEntry specialDisplayStat in worldObject.SpecialDisplayStats)
			{
				yield return specialDisplayStat;
			}
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
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999);
			statDrawEntry.overrideReportText = def.description;
			return statDrawEntry;
		}

		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999);
			statDrawEntry.overrideReportText = thing.GetDescription();
			return statDrawEntry;
		}

		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), string.Empty, 99999);
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
			StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Basics, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), 99999);
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
			Rect rect2 = new Rect(rect)
			{
				x = outRect.xMax
			};
			rect2.width = rect.xMax - rect2.x;
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, (float)(outRect.width - 16.0), StatsReportUtility.listHeight);
			Widgets.BeginScrollView(outRect, ref StatsReportUtility.scrollPosition, viewRect, true);
			float num = 0f;
			string b = (string)null;
			List<StatDrawEntry>.Enumerator enumerator = StatsReportUtility.cachedDrawEntries.GetEnumerator();
			try
			{
				StatDrawEntry ent;
				while (enumerator.MoveNext())
				{
					ent = enumerator.Current;
					if (ent.category.LabelCap != b)
					{
						Widgets.ListSeparator(ref num, viewRect.width, ent.category.LabelCap);
						b = ent.category.LabelCap;
					}
					num += ent.Draw(8f, num, (float)(viewRect.width - 8.0), StatsReportUtility.selectedEntry == ent, (Action)delegate
					{
						StatsReportUtility.SelectEntry(ent, true);
					});
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
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
