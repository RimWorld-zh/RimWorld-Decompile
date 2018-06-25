using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000123 RID: 291
	public class WorkGiver_Train : WorkGiver_InteractAnimal
	{
		// Token: 0x0400030A RID: 778
		public const int MinTrainInterval = 15000;

		// Token: 0x06000600 RID: 1536 RVA: 0x00040044 File Offset: 0x0003E444
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Pawn> pawnList = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < pawnList.Count; i++)
			{
				yield return pawnList[i];
			}
			yield break;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00040070 File Offset: 0x0003E470
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
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				result = null;
			}
			else if (pawn2.training == null)
			{
				result = null;
			}
			else if (pawn2.training.NextTrainableToTrain() == null)
			{
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
				result = new Job(JobDefOf.Train, t);
			}
			return result;
		}
	}
}
