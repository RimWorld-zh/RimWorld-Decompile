using System;
using System.Collections.Generic;
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
				return (StorytellerCompProperties_ThreatCycle)base.props;
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
				float daysSinceThreatBig = (float)((float)(Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick) / 60000.0);
				if (daysSinceThreatBig > this.Props.minDaysBetweenThreatBigs && ((daysSinceThreatBig > this.Props.ThreatCycleTotalDays * 0.89999997615814209 && curCycleDays > this.Props.ThreatCycleTotalDays * 0.949999988079071) || Rand.MTBEventOccurs(this.Props.mtbDaysThreatBig, 60000f, 1000f)))
				{
					FiringIncident bt = this.GenerateQueuedThreatBig(target);
					if (bt != null)
					{
						yield return bt;
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
		}

		private FiringIncident GenerateQueuedThreatSmall(IIncidentTarget target)
		{
			IncidentDef incidentDef = default(IncidentDef);
			if (!this.UsableIncidentsInCategory(IncidentCategory.ThreatSmall, target).TryRandomElementByWeight<IncidentDef>(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incidentDef))
			{
				return null;
			}
			FiringIncident firingIncident = new FiringIncident(incidentDef, this, null);
			firingIncident.parms = this.GenerateParms(incidentDef.category, target);
			return firingIncident;
		}

		private FiringIncident GenerateQueuedThreatBig(IIncidentTarget target)
		{
			IncidentParms parms = this.GenerateParms(IncidentCategory.ThreatBig, target);
			IncidentDef raidEnemy = default(IncidentDef);
			if (GenDate.DaysPassed < 20)
			{
				if (IncidentDefOf.RaidEnemy.Worker.CanFireNow(target))
				{
					raidEnemy = IncidentDefOf.RaidEnemy;
					goto IL_0088;
				}
				return null;
			}
			if (!(from def in DefDatabase<IncidentDef>.AllDefs
			where def.category == IncidentCategory.ThreatBig && parms.points >= def.minThreatPoints && def.Worker.CanFireNow(target)
			select def).TryRandomElementByWeight<IncidentDef>(new Func<IncidentDef, float>(base.IncidentChanceFinal), out raidEnemy))
			{
				return null;
			}
			goto IL_0088;
			IL_0088:
			FiringIncident firingIncident = new FiringIncident(raidEnemy, this, null);
			firingIncident.parms = parms;
			return firingIncident;
		}
	}
}
