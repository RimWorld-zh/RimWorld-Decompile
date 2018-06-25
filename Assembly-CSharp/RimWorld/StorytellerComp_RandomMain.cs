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
				List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
				IEnumerable<IncidentDef> options;
				for (;;)
				{
					if (triedCategories.Count >= this.Props.categoryWeights.Count)
					{
						break;
					}
					IncidentCategoryDef incidentCategoryDef = this.DecideCategory(target, triedCategories);
					triedCategories.Add(incidentCategoryDef);
					IncidentParms parms = this.GenerateParms(incidentCategoryDef, target);
					options = from d in base.UsableIncidentsInCategory(incidentCategoryDef, target)
					where !d.NeedsParmsPoints || d.minThreatPoints <= parms.points
					select d;
					if (options.Any<IncidentDef>())
					{
						goto Block_3;
					}
				}
				yield break;
				Block_3:
				IncidentDef incDef;
				if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
				{
					yield return new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
				}
			}
			yield break;
		}

		private IncidentCategoryDef DecideCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig))
			{
				int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
				if ((float)num > 60000f * this.Props.maxThreatBigIntervalDays)
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
			incidentParms.points *= Rand.Range(0.5f, 1.5f);
			return incidentParms;
		}

		[CompilerGenerated]
		private static float <DecideCategory>m__0(IncidentCategoryEntry cw)
		{
			return cw.weight;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal List<IncidentCategoryDef> <triedCategories>__1;

			internal IIncidentTarget target;

			internal IEnumerable<IncidentDef> <options>__2;

			internal IncidentDef <incDef>__1;

			internal StorytellerComp_RandomMain $this;

			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

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
						triedCategories = new List<IncidentCategoryDef>();
						for (;;)
						{
							if (triedCategories.Count >= base.Props.categoryWeights.Count)
							{
								break;
							}
							IncidentCategoryDef incidentCategoryDef = base.DecideCategory(target, triedCategories);
							triedCategories.Add(incidentCategoryDef);
							IncidentParms parms = this.GenerateParms(incidentCategoryDef, target);
							options = from d in base.UsableIncidentsInCategory(incidentCategoryDef, target)
							where !d.NeedsParmsPoints || d.minThreatPoints <= parms.points
							select d;
							if (options.Any<IncidentDef>())
							{
								goto Block_4;
							}
						}
						return false;
						Block_4:
						if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
						{
							this.$current = new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
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
					return !d.NeedsParmsPoints || d.minThreatPoints <= this.parms.points;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DecideCategory>c__AnonStorey2
		{
			internal List<IncidentCategoryDef> skipCategories;

			public <DecideCategory>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IncidentCategoryEntry cw)
			{
				return !this.skipCategories.Contains(cw.category);
			}
		}
	}
}
