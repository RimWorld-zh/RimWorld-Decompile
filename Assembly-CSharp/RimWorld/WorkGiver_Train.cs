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
			Job result;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				result = null;
			}
			else if (pawn2.Faction != pawn.Faction)
			{
				result = null;
			}
			else if (Find.TickManager.TicksGame < pawn2.mindState.lastAssignedInteractTime + 15000)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans);
				result = null;
			}
			else if (pawn2.training == null)
			{
				result = null;
			}
			else
			{
				TrainableDef trainableDef = pawn2.training.NextTrainableToTrain();
				if (trainableDef == null)
				{
					result = null;
				}
				else if (!this.CanInteractWithAnimal(pawn, pawn2))
				{
					result = null;
				}
				else if (pawn2.RaceProps.EatsFood && !base.HasFoodToInteractAnimal(pawn, pawn2))
				{
					Job job = base.TakeFoodForAnimalInteractJob(pawn, pawn2);
					if (job == null)
					{
						JobFailReason.Is(WorkGiver_InteractAnimal.NoUsableFoodTrans);
					}
					result = job;
				}
				else
				{
					result = new Job(JobDefOf.Train, t);
				}
			}
			return result;
		}
	}
}
