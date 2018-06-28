using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class StatsReportUtility
	{
		private static StatDrawEntry selectedEntry;

		private static StatDrawEntry mousedOverEntry;

		private static Vector2 scrollPosition;

		private static float listHeight;

		private static List<StatDrawEntry> cachedDrawEntries = new List<StatDrawEntry>();

		[CompilerGenerated]
		private static Func<StatDrawEntry, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<StatDrawEntry, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<StatDrawEntry> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<StatDrawEntry, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<StatDrawEntry> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<StatDrawEntry, int> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<StatDrawEntry, int> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<StatDrawEntry, string> <>f__am$cache7;

		public static void Reset()
		{
			StatsReportUtility.scrollPosition = default(Vector2);
			StatsReportUtility.selectedEntry = null;
			StatsReportUtility.mousedOverEntry = null;
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
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, null, null);
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
				StatsReportUtility.FinalizeCachedDrawEntries(StatsReportUtility.cachedDrawEntries);
			}
			StatsReportUtility.DrawStatsWorker(rect, thing, null);
		}

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
			foreach (StatDrawEntry stat2 in thing.SpecialDisplayStats())
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

		private static IEnumerable<StatDrawEntry> StatsToDraw(WorldObject worldObject)
		{
			yield return StatsReportUtility.DescriptionEntry(worldObject);
			foreach (StatDrawEntry stat in worldObject.SpecialDisplayStats)
			{
				yield return stat;
			}
			yield break;
		}

		private static void FinalizeCachedDrawEntries(IEnumerable<StatDrawEntry> original)
		{
			StatsReportUtility.cachedDrawEntries = (from sd in original
			orderby sd.category.displayOrder, sd.DisplayPriorityWithinCategory descending, sd.LabelCap
			select sd).ToList<StatDrawEntry>();
		}

		private static StatDrawEntry DescriptionEntry(Def def)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), "", 99999, "")
			{
				overrideReportText = def.description
			};
		}

		private static StatDrawEntry DescriptionEntry(Thing thing)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), "", 99999, "")
			{
				overrideReportText = thing.DescriptionFlavor
			};
		}

		private static StatDrawEntry DescriptionEntry(WorldObject worldObject)
		{
			return new StatDrawEntry(StatCategoryDefOf.Basics, "Description".Translate(), "", 99999, "")
			{
				overrideReportText = worldObject.GetDescription()
			};
		}

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

		private static void SelectEntry(StatDrawEntry rec, bool playSound = true)
		{
			StatsReportUtility.selectedEntry = rec;
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static StatsReportUtility()
		{
		}

		[CompilerGenerated]
		private static bool <DrawStatsReport>m__0(StatDrawEntry r)
		{
			return r.ShouldDisplay;
		}

		[CompilerGenerated]
		private static bool <DrawStatsReport>m__1(StatDrawEntry r)
		{
			return r.ShouldDisplay;
		}

		[CompilerGenerated]
		private static bool <DrawStatsReport>m__2(StatDrawEntry de)
		{
			return de.stat != null && !de.stat.showNonAbstract;
		}

		[CompilerGenerated]
		private static bool <DrawStatsReport>m__3(StatDrawEntry r)
		{
			return r.ShouldDisplay;
		}

		[CompilerGenerated]
		private static bool <DrawStatsReport>m__4(StatDrawEntry de)
		{
			return de.stat != null && !de.stat.showNonAbstract;
		}

		[CompilerGenerated]
		private static int <FinalizeCachedDrawEntries>m__5(StatDrawEntry sd)
		{
			return sd.category.displayOrder;
		}

		[CompilerGenerated]
		private static int <FinalizeCachedDrawEntries>m__6(StatDrawEntry sd)
		{
			return sd.DisplayPriorityWithinCategory;
		}

		[CompilerGenerated]
		private static string <FinalizeCachedDrawEntries>m__7(StatDrawEntry sd)
		{
			return sd.LabelCap;
		}

		[CompilerGenerated]
		private sealed class <StatsToDraw>c__Iterator0 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal Def def;

			internal BuildableDef <eDef>__0;

			internal ThingDef stuff;

			internal IEnumerator<StatDef> $locvar0;

			internal StatDef <stat>__2;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			private StatsReportUtility.<StatsToDraw>c__Iterator0.<StatsToDraw>c__AnonStorey3 $locvar1;

			[DebuggerHidden]
			public <StatsToDraw>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					this.$current = StatsReportUtility.DescriptionEntry(def);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					eDef = (def as BuildableDef);
					if (eDef == null)
					{
						goto IL_179;
					}
					StatRequest statRequest = StatRequest.For(eDef, stuff, QualityCategory.Normal);
					enumerator = (from st in DefDatabase<StatDef>.AllDefs
					where st.Worker.ShouldShowFor(statRequest)
					select st).GetEnumerator();
					num = 4294967293u;
					break;
				}
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						stat = enumerator.Current;
						this.$current = new StatDrawEntry(stat.category, stat, eDef.GetStatValueAbstract(stat, stuff), StatRequest.For(eDef, stuff, QualityCategory.Normal), ToStringNumberSense.Undefined);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				IL_179:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StatsReportUtility.<StatsToDraw>c__Iterator0 <StatsToDraw>c__Iterator = new StatsReportUtility.<StatsToDraw>c__Iterator0();
				<StatsToDraw>c__Iterator.def = def;
				<StatsToDraw>c__Iterator.stuff = stuff;
				return <StatsToDraw>c__Iterator;
			}

			private sealed class <StatsToDraw>c__AnonStorey3
			{
				internal StatRequest statRequest;

				internal StatsReportUtility.<StatsToDraw>c__Iterator0 <>f__ref$0;

				public <StatsToDraw>c__AnonStorey3()
				{
				}

				internal bool <>m__0(StatDef st)
				{
					return st.Worker.ShouldShowFor(this.statRequest);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <StatsToDraw>c__Iterator1 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal Thing thing;

			internal StatDrawEntry <qe>__0;

			internal IEnumerator<StatDef> $locvar0;

			internal StatDef <stat>__1;

			internal StatDrawEntry <hpe>__2;

			internal IEnumerator<StatDrawEntry> $locvar1;

			internal StatDrawEntry <stat>__3;

			internal int <i>__4;

			internal int <i>__5;

			internal int <i>__6;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			private StatsReportUtility.<StatsToDraw>c__Iterator1.<StatsToDraw>c__AnonStorey4 $locvar2;

			[DebuggerHidden]
			public <StatsToDraw>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					this.$current = StatsReportUtility.DescriptionEntry(thing);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					qe = StatsReportUtility.QualityEntry(<StatsToDraw>c__AnonStorey.thing);
					if (qe != null)
					{
						this.$current = qe;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2u:
					break;
				case 3u:
				case 4u:
					goto IL_F5;
				case 5u:
					goto IL_33B;
				case 6u:
					Block_8:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							stat2 = enumerator2.Current;
							this.$current = stat2;
							if (!this.$disposing)
							{
								this.$PC = 6;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					if (!<StatsToDraw>c__AnonStorey.thing.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
					{
						i = 0;
						goto IL_480;
					}
					goto IL_4A6;
				case 7u:
					i++;
					goto IL_480;
				case 8u:
					j++;
					goto IL_580;
				case 9u:
					k++;
					goto IL_66B;
				default:
					return false;
				}
				enumerator = (from st in DefDatabase<StatDef>.AllDefs
				where st.Worker.ShouldShowFor(StatRequest.For(<StatsToDraw>c__AnonStorey.thing))
				select st).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_F5:
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						stat = enumerator.Current;
						if (!stat.Worker.IsDisabledFor(<StatsToDraw>c__AnonStorey.thing))
						{
							this.$current = new StatDrawEntry(stat.category, stat, <StatsToDraw>c__AnonStorey.thing.GetStatValue(stat, true), StatRequest.For(<StatsToDraw>c__AnonStorey.thing), ToStringNumberSense.Undefined);
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
						this.$current = new StatDrawEntry(stat.category, stat);
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (<StatsToDraw>c__AnonStorey.thing.def.useHitPoints)
				{
					StatDrawEntry hpe = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "HitPointsBasic".Translate().CapitalizeFirst(), <StatsToDraw>c__AnonStorey.thing.HitPoints.ToString() + " / " + <StatsToDraw>c__AnonStorey.thing.MaxHitPoints.ToString(), 0, "");
					hpe.overrideReportText = string.Concat(new string[]
					{
						"HitPointsBasic".Translate().CapitalizeFirst(),
						":\n\n",
						<StatsToDraw>c__AnonStorey.thing.HitPoints.ToString(),
						"\n\n",
						StatDefOf.MaxHitPoints.LabelCap,
						":\n\n",
						StatDefOf.MaxHitPoints.Worker.GetExplanationUnfinalized(StatRequest.For(<StatsToDraw>c__AnonStorey.thing), ToStringNumberSense.Absolute)
					});
					this.$current = hpe;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_33B:
				enumerator2 = <StatsToDraw>c__AnonStorey.thing.SpecialDisplayStats().GetEnumerator();
				num = 4294967293u;
				goto Block_8;
				IL_480:
				if (i < <StatsToDraw>c__AnonStorey.thing.def.equippedStatOffsets.Count)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, <StatsToDraw>c__AnonStorey.thing.def.equippedStatOffsets[i].stat, <StatsToDraw>c__AnonStorey.thing.def.equippedStatOffsets[i].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_4A6:
				if (!<StatsToDraw>c__AnonStorey.thing.def.IsStuff)
				{
					goto IL_697;
				}
				if (<StatsToDraw>c__AnonStorey.thing.def.stuffProps.statFactors.NullOrEmpty<StatModifier>())
				{
					goto IL_5AB;
				}
				j = 0;
				IL_580:
				if (j < <StatsToDraw>c__AnonStorey.thing.def.stuffProps.statFactors.Count)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.StuffStatFactors, <StatsToDraw>c__AnonStorey.thing.def.stuffProps.statFactors[j].stat, <StatsToDraw>c__AnonStorey.thing.def.stuffProps.statFactors[j].value, StatRequest.ForEmpty(), ToStringNumberSense.Factor);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				IL_5AB:
				if (<StatsToDraw>c__AnonStorey.thing.def.stuffProps.statOffsets.NullOrEmpty<StatModifier>())
				{
					goto IL_696;
				}
				k = 0;
				IL_66B:
				if (k < <StatsToDraw>c__AnonStorey.thing.def.stuffProps.statOffsets.Count)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.StuffStatOffsets, <StatsToDraw>c__AnonStorey.thing.def.stuffProps.statOffsets[k].stat, <StatsToDraw>c__AnonStorey.thing.def.stuffProps.statOffsets[k].value, StatRequest.ForEmpty(), ToStringNumberSense.Offset);
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				}
				IL_696:
				IL_697:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 3u:
				case 4u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 6u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StatsReportUtility.<StatsToDraw>c__Iterator1 <StatsToDraw>c__Iterator = new StatsReportUtility.<StatsToDraw>c__Iterator1();
				<StatsToDraw>c__Iterator.thing = thing;
				return <StatsToDraw>c__Iterator;
			}

			private sealed class <StatsToDraw>c__AnonStorey4
			{
				internal Thing thing;

				public <StatsToDraw>c__AnonStorey4()
				{
				}

				internal bool <>m__0(StatDef st)
				{
					return st.Worker.ShouldShowFor(StatRequest.For(this.thing));
				}
			}
		}

		[CompilerGenerated]
		private sealed class <StatsToDraw>c__Iterator2 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal WorldObject worldObject;

			internal IEnumerator<StatDrawEntry> $locvar0;

			internal StatDrawEntry <stat>__1;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <StatsToDraw>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					this.$current = StatsReportUtility.DescriptionEntry(worldObject);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					enumerator = worldObject.SpecialDisplayStats.GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						stat = enumerator.Current;
						this.$current = stat;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StatsReportUtility.<StatsToDraw>c__Iterator2 <StatsToDraw>c__Iterator = new StatsReportUtility.<StatsToDraw>c__Iterator2();
				<StatsToDraw>c__Iterator.worldObject = worldObject;
				return <StatsToDraw>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <DrawStatsWorker>c__AnonStorey5
		{
			internal StatDrawEntry ent;

			public <DrawStatsWorker>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				StatsReportUtility.SelectEntry(this.ent, true);
			}

			internal void <>m__1()
			{
				StatsReportUtility.mousedOverEntry = this.ent;
			}
		}
	}
}
