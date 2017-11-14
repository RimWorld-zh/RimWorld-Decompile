using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_AllyInteraction : StorytellerComp
	{
		private const int ForceChooseTraderAfterTicks = 780000;

		private StorytellerCompProperties_AllyInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_AllyInteraction)base.props;
			}
		}

		private float IncidentMTBDays
		{
			get
			{
				return this.Props.baseMtb * StorytellerUtility.AllyIncidentMTBMultiplier();
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float mtb = this.IncidentMTBDays;
			IncidentDef incDef;
			if (!(mtb < 0.0) && Rand.MTBEventOccurs(mtb, 60000f, 1000f) && this.TryChooseIncident(target, out incDef))
			{
				yield return new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		private bool TryChooseIncident(IIncidentTarget target, out IncidentDef result)
		{
			if (IncidentDefOf.TraderCaravanArrival.TargetAllowed(target))
			{
				int num = 0;
				if (!target.StoryState.lastFireTicks.TryGetValue(IncidentDefOf.TraderCaravanArrival, out num))
				{
					num = (int)(base.props.minDaysPassed * 60000.0);
				}
				if (Find.TickManager.TicksGame > num + 780000)
				{
					result = IncidentDefOf.TraderCaravanArrival;
					return true;
				}
			}
			return this.UsableIncidentsInCategory(IncidentCategory.AllyArrival, target).TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)((IncidentDef d) => d.baseChance), out result);
		}
	}
}
