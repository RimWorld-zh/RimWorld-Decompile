using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_InteractAnimal : WorkGiver_Scanner
	{
		protected static string NoUsableFoodTrans;

		protected static string AnimalInteractedTooRecentlyTrans;

		private static string AnimalsSkillTooLowTrans;

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		public static void Reset()
		{
			WorkGiver_InteractAnimal.NoUsableFoodTrans = "NoUsableFood".Translate();
			WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans = "AnimalInteractedTooRecently".Translate();
			WorkGiver_InteractAnimal.AnimalsSkillTooLowTrans = "AnimalsSkillTooLow".Translate();
		}

		protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal)
		{
			bool result;
			if (!pawn.CanReserve((Thing)animal, 1, -1, null, false))
			{
				result = false;
			}
			else if (animal.Downed)
			{
				result = false;
			}
			else if (!animal.CanCasuallyInteractNow(false))
			{
				result = false;
			}
			else if (Mathf.RoundToInt(animal.GetStatValue(StatDefOf.MinimumHandlingSkill, true)) > pawn.skills.GetSkill(SkillDefOf.Animals).Level)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalsSkillTooLowTrans);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		protected bool HasFoodToInteractAnimal(Pawn pawn, Pawn tamee)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			float num2 = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee);
			float num3 = 0f;
			int num4 = 0;
			bool result;
			while (true)
			{
				if (num4 < innerContainer.Count)
				{
					Thing thing = innerContainer[num4];
					if (tamee.RaceProps.CanEverEat(thing) && (int)thing.def.ingestible.preferability <= 5 && !thing.def.IsDrug)
					{
						for (int i = 0; i < thing.stackCount; i++)
						{
							num3 += thing.def.ingestible.nutrition;
							if (num3 >= num2)
							{
								num++;
								num3 = 0f;
							}
							if (num >= 2)
								goto IL_00a1;
						}
					}
					num4++;
					continue;
				}
				result = false;
				break;
				IL_00a1:
				result = true;
				break;
			}
			return result;
		}

		protected Job TakeFoodForAnimalInteractJob(Pawn pawn, Pawn tamee)
		{
			float num = (float)(JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2.0 * 4.0);
			ThingDef thingDef = default(ThingDef);
			Thing thing = FoodUtility.BestFoodSourceOnMap(pawn, tamee, false, out thingDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false);
			Job result;
			if (thing == null)
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.TakeInventory, thing);
				job.count = Mathf.CeilToInt(num / thingDef.ingestible.nutrition);
				result = job;
			}
			return result;
		}
	}
}
