using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		public StorytellerComp_FactionInteraction()
		{
		}

		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (this.Props.minDanger != StoryDanger.None)
			{
				Map map = target as Map;
				if (map == null || map.dangerWatcher.DangerRating < this.Props.minDanger)
				{
					yield break;
				}
			}
			float allyIncidentFraction = StorytellerUtility.AllyIncidentFraction(this.Props.fullAlliesOnly);
			if (allyIncidentFraction <= 0f)
			{
				yield break;
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, 60f, 0f, this.Props.minSpacingDays, this.Props.baseIncidentsPerYear, this.Props.baseIncidentsPerYear, allyIncidentFraction);
			for (int i = 0; i < incCount; i++)
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		public override string ToString()
		{
			return base.ToString() + " (" + this.Props.incident.defName + ")";
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal IIncidentTarget target;

			internal float <allyIncidentFraction>__0;

			internal int <incCount>__0;

			internal int <i>__1;

			internal IncidentParms <parms>__2;

			internal StorytellerComp_FactionInteraction $this;

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
					if (base.Props.minDanger != StoryDanger.None)
					{
						Map map = target as Map;
						if (map == null || map.dangerWatcher.DangerRating < base.Props.minDanger)
						{
							return false;
						}
					}
					allyIncidentFraction = StorytellerUtility.AllyIncidentFraction(base.Props.fullAlliesOnly);
					if (allyIncidentFraction <= 0f)
					{
						return false;
					}
					incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), base.Props.minDaysPassed, 60f, 0f, base.Props.minSpacingDays, base.Props.baseIncidentsPerYear, base.Props.baseIncidentsPerYear, allyIncidentFraction);
					i = 0;
					break;
				case 1u:
					IL_1AC:
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
					parms = this.GenerateParms(base.Props.incident.category, target);
					if (base.Props.incident.Worker.CanFireNow(parms, false))
					{
						this.$current = new FiringIncident(base.Props.incident, this, parms);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_1AC;
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
				StorytellerComp_FactionInteraction.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_FactionInteraction.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}
		}
	}
}
