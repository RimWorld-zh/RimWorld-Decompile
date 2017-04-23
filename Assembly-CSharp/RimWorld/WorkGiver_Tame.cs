using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Tame : WorkGiver_InteractAnimal
	{
		public const int MinTameInterval = 30000;

		[DebuggerHidden]
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Tame.<PotentialWorkThingsGlobal>c__Iterator5B <PotentialWorkThingsGlobal>c__Iterator5B = new WorkGiver_Tame.<PotentialWorkThingsGlobal>c__Iterator5B();
			<PotentialWorkThingsGlobal>c__Iterator5B.pawn = pawn;
			<PotentialWorkThingsGlobal>c__Iterator5B.<$>pawn = pawn;
			WorkGiver_Tame.<PotentialWorkThingsGlobal>c__Iterator5B expr_15 = <PotentialWorkThingsGlobal>c__Iterator5B;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2.RaceProps.Humanlike)
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Tame) == null)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < pawn2.mindState.lastAssignedInteractTime + 30000)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans);
				return null;
			}
			if (!this.CanInteractWithAnimal(pawn, pawn2))
			{
				return null;
			}
			if (pawn2.RaceProps.EatsFood && !base.HasFoodToInteractAnimal(pawn, pawn2))
			{
				Job job = base.TakeFoodForAnimalInteractJob(pawn, pawn2);
				if (job == null)
				{
					JobFailReason.Is(WorkGiver_InteractAnimal.NoUsableFoodTrans);
				}
				return job;
			}
			return new Job(JobDefOf.Tame, t);
		}
	}
}
