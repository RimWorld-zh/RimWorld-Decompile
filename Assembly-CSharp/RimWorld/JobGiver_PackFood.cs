using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PackFood : ThinkNode_JobGiver
	{
		private const float MinMealNutrition = 0.3f;

		private const float MinNutritionPerColonistToDo = 1.5f;

		public const FoodPreferability MinFoodPreferability = FoodPreferability.MealAwful;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.inventory == null)
			{
				return null;
			}
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (thing.def.ingestible != null && thing.def.ingestible.nutrition > 0.30000001192092896 && (int)thing.def.ingestible.preferability >= 6 && pawn.RaceProps.CanEverEat(thing))
				{
					return null;
				}
			}
			if (pawn.Map.resourceCounter.TotalHumanEdibleNutrition < (float)pawn.Map.mapPawns.ColonistsSpawnedCount * 1.5)
			{
				return null;
			}
			Thing thing2 = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 20f, delegate(Thing t)
			{
				if (t.def.category == ThingCategory.Item && t.def.ingestible != null && !(t.def.ingestible.nutrition < 0.30000001192092896) && !t.IsForbidden(pawn) && !(t is Corpse) && pawn.CanReserve(t, 1, -1, null, false) && t.IsSociallyProper(pawn) && pawn.RaceProps.CanEverEat(t))
				{
					List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(pawn, t, FoodUtility.GetFinalIngestibleDef(t, false));
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].stages[0].baseMoodEffect < 0.0)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}, (Thing x) => FoodUtility.FoodOptimality(pawn, x, FoodUtility.GetFinalIngestibleDef(x, false), 0f, false), 24, 30);
			if (thing2 == null)
			{
				return null;
			}
			Job job = new Job(JobDefOf.TakeInventory, thing2);
			job.count = 1;
			return job;
		}
	}
}
