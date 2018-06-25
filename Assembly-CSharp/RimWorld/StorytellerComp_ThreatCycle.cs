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
	public class StorytellerComp_ThreatCycle : StorytellerComp
	{
		public StorytellerComp_ThreatCycle()
		{
		}

		protected StorytellerCompProperties_ThreatCycle Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatCycle)this.props;
			}
		}

		protected int QueueIntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float curCycleDays = (GenDate.DaysPassedFloat - this.Props.minDaysPassed) % this.Props.ThreatCycleTotalDays;
			if (curCycleDays > this.Props.threatOffDays)
			{
				float daysSinceThreatBig = (float)(Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick) / 60000f;
				if (daysSinceThreatBig > this.Props.minDaysBetweenThreatBigs)
				{
					if ((daysSinceThreatBig > this.Props.ThreatCycleTotalDays * 0.9f && curCycleDays > this.Props.ThreatCycleTotalDays * 0.95f) || Rand.MTBEventOccurs(this.Props.mtbDaysThreatBig, 60000f, 1000f))
					{
						FiringIncident bt = this.GenerateQueuedThreatBig(target);
						if (bt != null)
						{
							yield return bt;
						}
					}
				}
				if (Rand.MTBEventOccurs(this.Props.mtbDaysThreatSmall, 60000f, 1000f))
				{
					FiringIncident st = this.GenerateQueuedThreatSmall(target);
					if (st != null)
					{
						yield return st;
					}
				}
			}
			yield break;
		}

		private FiringIncident GenerateQueuedThreatSmall(IIncidentTarget target)
		{
			IncidentDef incidentDef;
			FiringIncident result;
			if (!base.UsableIncidentsInCategory(this.Props.threatSmallCategory, target).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incidentDef))
			{
				result = null;
			}
			else
			{
				result = new FiringIncident(incidentDef, this, null)
				{
					parms = this.GenerateParms(incidentDef.category, target)
				};
			}
			return result;
		}

		private FiringIncident GenerateQueuedThreatBig(IIncidentTarget target)
		{
			IncidentParms parms = this.GenerateParms(this.Props.threatBigCategory, target);
			IncidentDef raidEnemy;
			if ((float)GenDate.DaysPassed < this.Props.minDaysBeforeNonRaidThreatBig)
			{
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
				{
					return null;
				}
				raidEnemy = IncidentDefOf.RaidEnemy;
			}
			else if (!(from def in base.UsableIncidentsInCategory(this.Props.threatBigCategory, parms)
			where parms.points >= def.minThreatPoints
			select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out raidEnemy))
			{
				return null;
			}
			return new FiringIncident(raidEnemy, this, null)
			{
				parms = parms
			};
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal float <curCycleDays>__0;

			internal IIncidentTarget target;

			internal float <daysSinceThreatBig>__1;

			internal bool <mustThreat>__2;

			internal FiringIncident <bt>__3;

			internal FiringIncident <st>__4;

			internal StorytellerComp_ThreatCycle $this;

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
					curCycleDays = (GenDate.DaysPassedFloat - base.Props.minDaysPassed) % base.Props.ThreatCycleTotalDays;
					if (curCycleDays <= base.Props.threatOffDays)
					{
						goto IL_1D8;
					}
					daysSinceThreatBig = (float)(Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick) / 60000f;
					if (daysSinceThreatBig > base.Props.minDaysBetweenThreatBigs)
					{
						bool mustThreat = daysSinceThreatBig > base.Props.ThreatCycleTotalDays * 0.9f && curCycleDays > base.Props.ThreatCycleTotalDays * 0.95f;
						if (mustThreat || Rand.MTBEventOccurs(base.Props.mtbDaysThreatBig, 60000f, 1000f))
						{
							bt = base.GenerateQueuedThreatBig(target);
							if (bt != null)
							{
								this.$current = bt;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								return true;
							}
						}
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_1D7;
				default:
					return false;
				}
				if (Rand.MTBEventOccurs(base.Props.mtbDaysThreatSmall, 60000f, 1000f))
				{
					st = base.GenerateQueuedThreatSmall(target);
					if (st != null)
					{
						this.$current = st;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
				}
				IL_1D7:
				IL_1D8:
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
				StorytellerComp_ThreatCycle.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_ThreatCycle.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateQueuedThreatBig>c__AnonStorey1
		{
			internal IncidentParms parms;

			public <GenerateQueuedThreatBig>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IncidentDef def)
			{
				return this.parms.points >= def.minThreatPoints;
			}
		}
	}
}
