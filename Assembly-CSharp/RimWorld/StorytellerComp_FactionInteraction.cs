using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200036D RID: 877
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		// Token: 0x04000952 RID: 2386
		private const int ForceChooseTraderAfterTicks = 780000;

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x00080C8C File Offset: 0x0007F08C
		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00080CAC File Offset: 0x0007F0AC
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float mtb = this.Props.baseMtbDays * StorytellerUtility.AllyIncidentMTBMultiplier(true);
			if (mtb < 0f)
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
			{
				IncidentDef incDef;
				if (this.TryChooseIncident(target, out incDef))
				{
					yield return new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
				}
			}
			yield break;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00080CE0 File Offset: 0x0007F0E0
		private bool TryChooseIncident(IIncidentTarget target, out IncidentDef result)
		{
			if (IncidentDefOf.TraderCaravanArrival.TargetAllowed(target))
			{
				int num = 0;
				if (!target.StoryState.lastFireTicks.TryGetValue(IncidentDefOf.TraderCaravanArrival, out num))
				{
					num = (int)(this.props.minDaysPassed * 60000f);
				}
				if (Find.TickManager.TicksGame > num + 780000)
				{
					result = IncidentDefOf.TraderCaravanArrival;
					return true;
				}
			}
			return base.UsableIncidentsInCategory(IncidentCategoryDefOf.FactionArrival, target).TryRandomElementByWeight((IncidentDef d) => d.baseChance, out result);
		}
	}
}
