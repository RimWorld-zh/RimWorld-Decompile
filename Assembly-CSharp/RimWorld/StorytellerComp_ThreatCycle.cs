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
			if (!(curCycleDays > this.Props.threatOffDays))
				yield break;
			float daysSinceThreatBig = (float)((float)(Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick) / 60000.0);
			if (daysSinceThreatBig > this.Props.minDaysBetweenThreatBigs && ((daysSinceThreatBig > this.Props.ThreatCycleTotalDays * 0.89999997615814209 && curCycleDays > this.Props.ThreatCycleTotalDays * 0.949999988079071) || Rand.MTBEventOccurs(this.Props.mtbDaysThreatBig, 60000f, 1000f)))
			{
				FiringIncident bt = this.GenerateQueuedThreatBig(target);
				if (bt != null)
				{
					yield return bt;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!Rand.MTBEventOccurs(this.Props.mtbDaysThreatSmall, 60000f, 1000f))
				yield break;
			FiringIncident st = this.GenerateQueuedThreatSmall(target);
			if (st == null)
				yield break;
			yield return st;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private FiringIncident GenerateQueuedThreatSmall(IIncidentTarget target)
		{
			IncidentDef incidentDef = default(IncidentDef);
			FiringIncident result;
			if (!this.UsableIncidentsInCategory(this.Props.threatSmallCategory, target).TryRandomElementByWeight<IncidentDef>(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incidentDef))
			{
				result = null;
			}
			else
			{
				FiringIncident firingIncident = new FiringIncident(incidentDef, this, null);
				firingIncident.parms = this.GenerateParms(incidentDef.category, target);
				result = firingIncident;
			}
			return result;
		}

		private FiringIncident GenerateQueuedThreatBig(IIncidentTarget target)
		{
			IncidentParms parms = this.GenerateParms(this.Props.threatBigCategory, target);
			IncidentDef raidEnemy = default(IncidentDef);
			FiringIncident result;
			if ((float)GenDate.DaysPassed < this.Props.minDaysBeforeNonRaidThreatBig)
			{
				if (IncidentDefOf.RaidEnemy.Worker.CanFireNow(target))
				{
					raidEnemy = IncidentDefOf.RaidEnemy;
					goto IL_00b3;
				}
				result = null;
				goto IL_00cf;
			}
			if (!(from def in DefDatabase<IncidentDef>.AllDefs
			where def.category == this.Props.threatBigCategory && parms.points >= def.minThreatPoints && def.Worker.CanFireNow(target)
			select def).TryRandomElementByWeight<IncidentDef>(new Func<IncidentDef, float>(base.IncidentChanceFinal), out raidEnemy))
			{
				result = null;
				goto IL_00cf;
			}
			goto IL_00b3;
			IL_00cf:
			return result;
			IL_00b3:
			FiringIncident firingIncident = new FiringIncident(raidEnemy, this, null);
			firingIncident.parms = parms;
			result = firingIncident;
			goto IL_00cf;
		}
	}
}
