using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000379 RID: 889
	public class StorytellerComp_ThreatCycle : StorytellerComp
	{
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x00081B60 File Offset: 0x0007FF60
		protected StorytellerCompProperties_ThreatCycle Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatCycle)this.props;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x00081B80 File Offset: 0x0007FF80
		protected int QueueIntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00081BA8 File Offset: 0x0007FFA8
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

		// Token: 0x06000F57 RID: 3927 RVA: 0x00081BDC File Offset: 0x0007FFDC
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

		// Token: 0x06000F58 RID: 3928 RVA: 0x00081C40 File Offset: 0x00080040
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
	}
}
