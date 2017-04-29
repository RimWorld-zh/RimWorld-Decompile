using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return (StorytellerCompProperties_AllyInteraction)this.props;
			}
		}

		private float IncidentMTBDays
		{
			get
			{
				return this.Props.baseMtb * StorytellerUtility.AllyIncidentMTBMultiplier();
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_AllyInteraction.<MakeIntervalIncidents>c__IteratorA6 <MakeIntervalIncidents>c__IteratorA = new StorytellerComp_AllyInteraction.<MakeIntervalIncidents>c__IteratorA6();
			<MakeIntervalIncidents>c__IteratorA.target = target;
			<MakeIntervalIncidents>c__IteratorA.<$>target = target;
			<MakeIntervalIncidents>c__IteratorA.<>f__this = this;
			StorytellerComp_AllyInteraction.<MakeIntervalIncidents>c__IteratorA6 expr_1C = <MakeIntervalIncidents>c__IteratorA;
			expr_1C.$PC = -2;
			return expr_1C;
		}

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
			return this.UsableIncidentsInCategory(IncidentCategory.AllyArrival, target).TryRandomElementByWeight((IncidentDef d) => d.baseChance, out result);
		}
	}
}
