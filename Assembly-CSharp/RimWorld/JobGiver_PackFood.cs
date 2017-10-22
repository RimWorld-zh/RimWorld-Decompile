using System;
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
			Job result;
			if (pawn.inventory == null)
			{
				result = null;
			}
			else
			{
				ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Thing thing = innerContainer[i];
					if (thing.def.ingestible != null && thing.def.ingestible.nutrition > 0.30000001192092896 && (int)thing.def.ingestible.preferability >= 6 && pawn.RaceProps.CanEverEat(thing))
						goto IL_00a1;
				}
				if (pawn.Map.resourceCounter.TotalHumanEdibleNutrition < (float)pawn.Map.mapPawns.ColonistsSpawnedCount * 1.5)
				{
					result = null;
				}
				else
				{
					Thing thing2 = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 20f, (Predicate<Thing>)delegate(Thing t)
					{
						bool result2;
						if (t.def.category != ThingCategory.Item || t.def.ingestible == null || t.def.ingestible.nutrition < 0.30000001192092896 || t.IsForbidden(pawn) || t is Corpse || !pawn.CanReserve(t, 1, -1, null, false) || !t.IsSociallyProper(pawn) || !pawn.RaceProps.CanEverEat(t))
						{
							result2 = false;
						}
						else
						{
							List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(pawn, t, FoodUtility.GetFinalIngestibleDef(t, false));
							for (int j = 0; j < list.Count; j++)
							{
								if (list[j].stages[0].baseMoodEffect < 0.0)
									goto IL_00de;
							}
							result2 = true;
						}
						goto IL_00fd;
						IL_00de:
						result2 = false;
						goto IL_00fd;
						IL_00fd:
						return result2;
					}, (Func<Thing, float>)((Thing x) => FoodUtility.FoodOptimality(pawn, x, FoodUtility.GetFinalIngestibleDef(x, false), 0f, false)), 24, 30);
					if (thing2 == null)
					{
						result = null;
					}
					else
					{
						Job job = new Job(JobDefOf.TakeInventory, thing2);
						job.count = 1;
						result = job;
					}
				}
			}
			goto IL_017b;
			IL_017b:
			return result;
			IL_00a1:
			result = null;
			goto IL_017b;
		}
	}
}
