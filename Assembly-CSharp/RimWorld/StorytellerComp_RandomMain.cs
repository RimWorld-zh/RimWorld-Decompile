using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		[CompilerGenerated]
		private static Func<IncidentCategoryEntry, float> <>f__am$cache0;

		public StorytellerComp_RandomMain()
		{
		}

		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				bool targetIsRaidBeacon = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
				List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
				IncidentDef incDef;
				for (;;)
				{
					IncidentCategoryDef category = this.ChooseRandomCategory(target, triedCategories);
					IncidentParms parms = this.GenerateParms(category, target);
					IEnumerable<IncidentDef> options = from d in base.UsableIncidentsInCategory(category, target)
					where !d.NeedsParmsPoints || parms.points >= d.minThreatPoints
					select d;
					if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
					{
						break;
					}
					triedCategories.Add(category);
					if (triedCategories.Count >= this.Props.categoryWeights.Count)
					{
						goto Block_6;
					}
				}
				if (!this.Props.skipThreatBigIfRaidBeacon || !targetIsRaidBeacon || incDef.category != IncidentCategoryDefOf.ThreatBig)
				{
					yield return new FiringIncident(incDef, this, <MakeIntervalIncidents>c__AnonStorey.parms);
				}
				Block_6:;
			}
			yield break;
		}

		private IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig))
			{
				int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
				if (target.StoryState.LastThreatBigTick >= 0 && (float)num > 60000f * this.Props.maxThreatBigIntervalDays)
				{
					return IncidentCategoryDefOf.ThreatBig;
				}
			}
			return (from cw in this.Props.categoryWeights
			where !skipCategories.Contains(cw.category)
			select cw).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
		}

		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
			incidentParms.points *= this.Props.randomPointsFactorRange.RandomInRange;
			return incidentParms;
		}

		[CompilerGenerated]
		private static float <ChooseRandomCategory>m__0(IncidentCategoryEntry cw)
		{
			return cw.weight;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal IIncidentTarget target;

			internal bool <targetIsRaidBeacon>__1;

			internal List<IncidentCategoryDef> <triedCategories>__1;

			internal IncidentCategoryDef <category>__2;

			internal IEnumerable<IncidentDef> <options>__2;

			internal IncidentDef <incDef>__2;

			internal StorytellerComp_RandomMain $this;

			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

			private StorytellerComp_RandomMain.<MakeIntervalIncidents>c__Iterator0.<MakeIntervalIncidents>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <MakeIntervalIncidents>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (Rand.MTBEventOccurs(base.Props.mtbDays, 60000f, 1000f))
					{
						targetIsRaidBeacon = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
						triedCategories = new List<IncidentCategoryDef>();
						for (;;)
						{
							category = base.ChooseRandomCategory(target, triedCategories);
							IncidentParms parms = this.GenerateParms(category, target);
							options = from d in base.UsableIncidentsInCategory(category, target)
							where !d.NeedsParmsPoints || parms.points >= d.minThreatPoints
							select d;
							if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
							{
								break;
							}
							triedCategories.Add(category);
							if (triedCategories.Count >= base.Props.categoryWeights.Count)
							{
								goto Block_8;
							}
						}
						if (!base.Props.skipThreatBigIfRaidBeacon || !targetIsRaidBeacon || incDef.category != IncidentCategoryDefOf.ThreatBig)
						{
							this.$current = new FiringIncident(incDef, this, <MakeIntervalIncidents>c__AnonStorey.parms);
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
						Block_8:;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			FiringIncident IEnumerator<FiringIncident>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.FiringIncident>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StorytellerComp_RandomMain.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_RandomMain.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}

			private sealed class <MakeIntervalIncidents>c__AnonStorey1
			{
				internal IncidentParms parms;

				internal StorytellerComp_RandomMain.<MakeIntervalIncidents>c__Iterator0 <>f__ref$0;

				public <MakeIntervalIncidents>c__AnonStorey1()
				{
				}

				internal bool <>m__0(IncidentDef d)
				{
					return !d.NeedsParmsPoints || this.parms.points >= d.minThreatPoints;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ChooseRandomCategory>c__AnonStorey2
		{
			internal List<IncidentCategoryDef> skipCategories;

			public <ChooseRandomCategory>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IncidentCategoryEntry cw)
			{
				return !this.skipCategories.Contains(cw.category);
			}
		}
	}
}
