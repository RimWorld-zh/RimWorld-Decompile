using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Train : WorkGiver_InteractAnimal
	{
		public const int MinTrainInterval = 15000;

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Pawn> pawnList = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			int i = 0;
			if (i < pawnList.Count)
			{
				yield return (Thing)pawnList[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2.RaceProps.Animal)
			{
				if (pawn2.Faction != pawn.Faction)
				{
					return null;
				}
				if (Find.TickManager.TicksGame < pawn2.mindState.lastAssignedInteractTime + 15000)
				{
					JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans);
					return null;
				}
				if (pawn2.training == null)
				{
					return null;
				}
				TrainableDef trainableDef = pawn2.training.NextTrainableToTrain();
				if (trainableDef == null)
				{
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
				return new Job(JobDefOf.Train, t);
			}
			return null;
		}
	}
}
