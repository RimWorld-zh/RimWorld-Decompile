using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000122 RID: 290
	public class WorkGiver_Tame : WorkGiver_InteractAnimal
	{
		// Token: 0x04000308 RID: 776
		public const int MinTameInterval = 30000;

		// Token: 0x060005FE RID: 1534 RVA: 0x0003FD34 File Offset: 0x0003E134
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Tame))
			{
				yield return des.target.Thing;
			}
			yield break;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0003FD60 File Offset: 0x0003E160
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Job result;
			if (pawn2 == null || !pawn2.NonHumanlikeOrWildMan())
			{
				result = null;
			}
			else if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Tame) == null)
			{
				result = null;
			}
			else if (Find.TickManager.TicksGame < pawn2.mindState.lastAssignedInteractTime + 30000)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				result = null;
			}
			else if (!this.CanInteractWithAnimal(pawn, pawn2, forced))
			{
				result = null;
			}
			else
			{
				if (pawn2.RaceProps.EatsFood)
				{
					if (!base.HasFoodToInteractAnimal(pawn, pawn2))
					{
						Job job = base.TakeFoodForAnimalInteractJob(pawn, pawn2);
						if (job == null)
						{
							JobFailReason.Is(WorkGiver_InteractAnimal.NoUsableFoodTrans, null);
						}
						return job;
					}
				}
				result = new Job(JobDefOf.Tame, t);
			}
			return result;
		}
	}
}
