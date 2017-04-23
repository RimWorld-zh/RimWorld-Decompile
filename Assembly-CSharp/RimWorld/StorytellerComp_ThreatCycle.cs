using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_ThreatCycle : StorytellerComp
	{
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

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_ThreatCycle.<MakeIntervalIncidents>c__IteratorAE <MakeIntervalIncidents>c__IteratorAE = new StorytellerComp_ThreatCycle.<MakeIntervalIncidents>c__IteratorAE();
			<MakeIntervalIncidents>c__IteratorAE.target = target;
			<MakeIntervalIncidents>c__IteratorAE.<$>target = target;
			<MakeIntervalIncidents>c__IteratorAE.<>f__this = this;
			StorytellerComp_ThreatCycle.<MakeIntervalIncidents>c__IteratorAE expr_1C = <MakeIntervalIncidents>c__IteratorAE;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		private FiringIncident GenerateQueuedThreatSmall(IIncidentTarget target)
		{
			IncidentDef incidentDef;
			if (!this.UsableIncidentsInCategory(IncidentCategory.ThreatSmall, target).TryRandomElementByWeight(new Func<IncidentDef, float>(this.IncidentChanceAdjustedForPopulation), out incidentDef))
			{
				return null;
			}
			return new FiringIncident(incidentDef, this, null)
			{
				parms = this.GenerateParms(incidentDef.category, target)
			};
		}

		private FiringIncident GenerateQueuedThreatBig(IIncidentTarget target)
		{
			IncidentParms parms = this.GenerateParms(IncidentCategory.ThreatBig, target);
			IncidentDef raidEnemy;
			if (GenDate.DaysPassed < 20)
			{
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(target))
				{
					return null;
				}
				raidEnemy = IncidentDefOf.RaidEnemy;
			}
			else if (!(from def in DefDatabase<IncidentDef>.AllDefs
			where def.category == IncidentCategory.ThreatBig && parms.points >= def.minThreatPoints && def.Worker.CanFireNow(target)
			select def).TryRandomElementByWeight(new Func<IncidentDef, float>(this.IncidentChanceAdjustedForPopulation), out raidEnemy))
			{
				return null;
			}
			return new FiringIncident(raidEnemy, this, null)
			{
				parms = parms
			};
		}
	}
}
