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
	public class StorytellerComp_OnOffCycle : StorytellerComp
	{
		public StorytellerComp_OnOffCycle()
		{
		}

		protected StorytellerCompProperties_OnOffCycle Props
		{
			get
			{
				return (StorytellerCompProperties_OnOffCycle)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float difficultyFactor = (!this.Props.applyRaidBeaconThreatMtbFactor) ? 1f : Find.Storyteller.difficulty.raidBeaconThreatCountFactor;
			float acceptFraction = 1f;
			if (this.Props.acceptFractionByDaysPassedCurve != null)
			{
				acceptFraction *= this.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (this.Props.acceptPercentFactorPerThreatPointsCurve != null)
			{
				acceptFraction *= this.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, this.Props.onDays, this.Props.offDays, this.Props.minSpacingDays, this.Props.numIncidentsRange.min * difficultyFactor, this.Props.numIncidentsRange.max * difficultyFactor, acceptFraction);
			for (int i = 0; i < incCount; i++)
			{
				FiringIncident fi = this.GenerateIncident(target);
				if (fi != null)
				{
					yield return fi;
				}
			}
			yield break;
		}

		private FiringIncident GenerateIncident(IIncidentTarget target)
		{
			IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);
			IncidentDef def2;
			if ((float)GenDate.DaysPassed < this.Props.forceRaidEnemyBeforeDaysPassed)
			{
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms, false))
				{
					return null;
				}
				def2 = IncidentDefOf.RaidEnemy;
			}
			else if (this.Props.incident != null)
			{
				if (!this.Props.incident.Worker.CanFireNow(parms, false))
				{
					return null;
				}
				def2 = this.Props.incident;
			}
			else if (!(from def in base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms)
			where parms.points >= def.minThreatPoints
			select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out def2))
			{
				return null;
			}
			return new FiringIncident(def2, this, null)
			{
				parms = parms
			};
		}

		public override string ToString()
		{
			return base.ToString() + " (" + ((this.Props.incident == null) ? this.Props.IncidentCategory.defName : this.Props.incident.defName) + ")";
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal float <difficultyFactor>__0;

			internal float <acceptFraction>__0;

			internal IIncidentTarget target;

			internal int <incCount>__0;

			internal int <i>__1;

			internal FiringIncident <fi>__2;

			internal StorytellerComp_OnOffCycle $this;

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
					difficultyFactor = ((!base.Props.applyRaidBeaconThreatMtbFactor) ? 1f : Find.Storyteller.difficulty.raidBeaconThreatCountFactor);
					acceptFraction = 1f;
					if (base.Props.acceptFractionByDaysPassedCurve != null)
					{
						acceptFraction *= base.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
					}
					if (base.Props.acceptPercentFactorPerThreatPointsCurve != null)
					{
						acceptFraction *= base.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
					}
					incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), base.Props.minDaysPassed, base.Props.onDays, base.Props.offDays, base.Props.minSpacingDays, base.Props.numIncidentsRange.min * difficultyFactor, base.Props.numIncidentsRange.max * difficultyFactor, acceptFraction);
					i = 0;
					break;
				case 1u:
					IL_1D0:
					i++;
					break;
				default:
					return false;
				}
				if (i >= incCount)
				{
					this.$PC = -1;
				}
				else
				{
					fi = base.GenerateIncident(target);
					if (fi != null)
					{
						this.$current = fi;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_1D0;
				}
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
				StorytellerComp_OnOffCycle.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_OnOffCycle.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateIncident>c__AnonStorey1
		{
			internal IncidentParms parms;

			public <GenerateIncident>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IncidentDef def)
			{
				return this.parms.points >= def.minThreatPoints;
			}
		}
	}
}
