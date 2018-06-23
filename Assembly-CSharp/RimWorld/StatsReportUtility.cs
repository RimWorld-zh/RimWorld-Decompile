using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200081E RID: 2078
	public static class StatsReportUtility
	{
		// Token: 0x040018ED RID: 6381
		private static StatDrawEntry selectedEntry;

		// Token: 0x040018EE RID: 6382
		private static StatDrawEntry mousedOverEntry;

		// Token: 0x040018EF RID: 6383
		private static Vector2 scrollPosition;

		// Token: 0x040018F0 RID: 6384
		private static float listHeight;

		// Token: 0x040018F1 RID: 6385
		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();

		// Token: 0x06002E83 RID: 11907 RVA: 0x0018D6C0 File Offset: 0x0018BAC0
		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.mousedOverEntry = null;
			StatsReportUtility.cachedDrawEntries.Clear();
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x0018D6F4 File Offset: 0x0018BAF4
		public static void DrawStatsReport(Rect rect, Def def, ThingDef stuff)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
			{
				StatsReportUtility.cachedDrawEntries.AddRange(def.SpecialDisplayStats());
				StatsReportUtility.cachedDrawEntries.AddRange(from r in StatsReportUtility.StatsToDraw(def, stuff)
				where r.ShouldDisplay
				select r);
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x0018D768 File Offset: 0x0018BB68
		public static void DrawStatsReport(Rect rect, Thing thing)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
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

		// Token: 0x06002E86 RID: 11910 RVA: 0x0018D808 File Offset: 0x0018BC08
		public static void DrawStatsReport(Rect rect, WorldObject worldObject)
		{
			if (StatsReportUtility.cachedDrawEntries.NullOrEmpty<StatDrawEntry>())
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

		// Token: 0x06002E87 RID: 11911 RVA: 0x0018D8A8 File Offset: 0x0018BCA8
		private static IEnumerable<StatDrawEntry> StatsToDraw(Def def, ThingDef stuff)
		{
			yield return StatsReportUtility.DescriptionEntry(def);
			BuildableDef eDef = def as BuildableDef;
			if (eDef != null)
			{
				StatRequest statRequest = StatRequest.For(eDef, stuff, QualityCategory.Normal);
				foreach (StatDef stat in from st in DefDatabase<StatDef>.AllDefs
				where st.Worker.ShouldShowFor(statRequest)
				select st)
				{
					yield return new StatDrawEntry(stat.category, stat, eDef.GetStatValueAbstract(stat, stuff), StatRequest.For(eDef, stuff, QualityCategory.Normal), ToStringNumberSense.Undefined);
				}
			}
			yield break;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x0018D8DC File Offset: 0x0018BCDC
		private static IEnumerable<StatDrawEntry> StatsToDraw(Thing thing)
		{
			yield return StatsReportUtility.DescriptionEntry(thing);
			StatDrawEntry qe = StatsReportUtility.QualityEntry(thing);
			if (qe != null)
			{
				yield return qe;
			}
			foreach (StatDef stat in from st in DefDatabase<StatDef>.AllDefs
			where st.Worker.ShouldShowFor(StatRequest.For(thing))
			select st)
			{
				if (!stat.Worker.IsDisabledFor(thing))
				{
					yield return new StatDrawEntry(stat.category, stat, thing.GetStatValue(stat, true), StatRequest.For(thing), ToStringNumberSense.Undefined);
				}
				else
				{
					yield return new StatDrawEntry(stat.category, stat);
				}
			}
			if (thing.def.useHitPoints)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "HitPointsBasic".Translate().CapitalizeFirst(), thing.HitPoints.ToString() + " / " + thing.MaxHitPoints.ToString(), 0, "")
				{
					overrideReportText = string.Concat(new string[]
					{
						"HitPointsBasic".Translate().CapitalizeFirst(),
						":\n\n",
						thing.HitPoints.ToString(),
						"\n\n",
						StatDefOf.MaxHitPoints.LabelCap,
						":\n\n",
						StatDefOf.MaxHitPoints.Worker.GetExplanationUnfinalized(StatRequest.For(thing), ToStringNumberSense.Absolute)
					})
				};
			}
			foreach (StatDrawEntry stat2 in thing.SpecialDisplayStats)
			{
				yield return stat2;
			}
			if (!thing.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				for (int i = 0; i < thing.def.equippedStatOffsets.Count; i++)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, thing.def.equippedStatOffsets[i].stat, thing.def.equippedStatOffsets[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset);
				}
			}
			if (thing.def.IsStuff)
			{
				if (!thing.def.stuffProps.statFactors.NullOrEmpty<StatModifier>())
				{
					for (int j = 0; j < thing.def.stuffProps.statFactors.Count; j++)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, thing.def.stuffProps.statFactors[j].stat, thing.def.stuffProps.statFactors[j].value, StatRequest.ForEmpty(), ToStringNumberSense.Factor);
					}
				}
				if (!thing.def.stuffProps.statOffsets.NullOrEmpty<StatModifier>())
				{
					for (int k = 0; k < thing.def.stuffProps.statOffsets.Count; k++)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, thing.def.stuffProps.statOffsets[k].stat, thing.def.stuffProps.statOffsets[k].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset);
					}
				}
			}
			yield break;
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x0018D908 File Offset: 0x0018BD08
		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			yield return StatsReportUtility.DescriptionEntry(worldObject);
			foreach (StatDrawEntry stat in worldObject.SpecialDisplayStats)
			{
				yield return stat;
			}
			yield break;
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x0018D934 File Offset: 0x0018BD34
		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x0018D9B4 File Offset: 0x0018BDB4
		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), "", 99999, "")
			{
				overrideReportText = def.description
			};
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x0018D9FC File Offset: 0x0018BDFC
		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), "", 99999, "")
			{
				overrideReportText = thing.DescriptionFlavor
			};
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x0018DA44 File Offset: 0x0018BE44
		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), "", 99999, "")
			{
				overrideReportText = worldObject.GetDescription()
			};
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x0018DA8C File Offset: 0x0018BE8C
		private static StatDrawEntry QualityEntry(Thing t)
		{
			QualityCategory cat;
			StatDrawEntry result;
			if (!t.TryGetQuality(out cat))
			{
				result = null;
			}
			else
			{
				result = new StatDrawEntry(StatCategoryDefOf.Basics, "Quality".Translate(), cat.GetLabel().CapitalizeFirst(), 99999, "")
				{
					overrideReportText = "QualityDescription".Translate()
				};
			}
			return result;
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x0018DAF0 File Offset: 0x0018BEF0
		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x0018DB0C File Offset: 0x0018BF0C
		private static void DrawStatsWorker(Rect rect, Thing optionalThing, WorldObject optionalWorldObject)
		{
			Rect rect2 = new Rect(rect);
			rect2.width *= 0.5f;
			Rect rect3 = new Rect(rect);
			rect3.x = rect2.xMax;
			rect3.width = rect.xMax - rect3.x;
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(0f, 0f, rect2.width - 16f, StatsReportUtility.listHeight);
			Widgets.BeginScrollView(rect2, ref StatsReportUtility.scrollPosition, viewRect, true);
			float num = 0f;
			string b = null;
			StatsReportUtility.mousedOverEntry = null;
			for (int i = 0; i < StatsReportUtility.cachedDrawEntries.Count; i++)
			{
				StatDrawEntry ent = StatsReportUtility.cachedDrawEntries[i];
				if (ent.category.LabelCap != b)
				{
					Widgets.ListSeparator(ref num, viewRect.width, ent.category.LabelCap);
					b = ent.category.LabelCap;
				}
				num += ent.Draw(8f, num, viewRect.width - 8f, StatsReportUtility.selectedEntry == ent, delegate
				{
					StatsReportUtility.SelectEntry(ent, true);
				}, delegate
				{
					StatsReportUtility.mousedOverEntry = ent;
				}, StatsReportUtility.scrollPosition, rect2);
			}
			StatsReportUtility.listHeight = num + 100f;
			Widgets.EndScrollView();
			Rect rect4 = rect3.ContractedBy(10f);
			GUI.BeginGroup(rect4);
			StatDrawEntry statDrawEntry;
			if ((statDrawEntry = StatsReportUtility.selectedEntry) == null)
			{
				statDrawEntry = (StatsReportUtility.mousedOverEntry ?? StatsReportUtility.cachedDrawEntries.FirstOrDefault<StatDrawEntry>());
			}
			StatDrawEntry statDrawEntry2 = statDrawEntry;
			if (statDrawEntry2 != null)
			{
				StatRequest optionalReq;
				if (statDrawEntry2.hasOptionalReq)
				{
					optionalReq = statDrawEntry2.optionalReq;
				}
				else if (optionalThing != null)
				{
					optionalReq = StatRequest.For(optionalThing);
				}
				else
				{
					optionalReq = StatRequest.ForEmpty();
				}
				string explanationText = statDrawEntry2.GetExplanationText(optionalReq);
				Rect rect5 = rect4.AtZero();
				Widgets.Label(rect5, explanationText);
			}
			GUI.EndGroup();
		}
	}
}
