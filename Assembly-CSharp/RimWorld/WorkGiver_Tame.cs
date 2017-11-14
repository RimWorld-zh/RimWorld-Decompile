using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Tame : WorkGiver_InteractAnimal
	{
		public const int MinTameInterval = 30000;

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Tame).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Designation des = enumerator.Current;
					yield return des.target.Thing;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00d2:
			/*Error near IL_00d3: Unexpected return in MoveNext()*/;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2.NonHumanlikeOrWildMan())
			{
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
			return null;
		}
	}
}
