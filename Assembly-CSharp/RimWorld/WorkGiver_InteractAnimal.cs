using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200011E RID: 286
	public abstract class WorkGiver_InteractAnimal : WorkGiver_Scanner
	{
		// Token: 0x04000303 RID: 771
		protected static string NoUsableFoodTrans;

		// Token: 0x04000304 RID: 772
		protected static string AnimalInteractedTooRecentlyTrans;

		// Token: 0x04000305 RID: 773
		private static string CantInteractAnimalDownedTrans;

		// Token: 0x04000306 RID: 774
		private static string CantInteractAnimalAsleepTrans;

		// Token: 0x04000307 RID: 775
		private static string CantInteractAnimalBusyTrans;

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0003F6B4 File Offset: 0x0003DAB4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0003F6CC File Offset: 0x0003DACC
		public static void ResetStaticData()
		{
			WorkGiver_InteractAnimal.NoUsableFoodTrans = "NoUsableFood".Translate();
			WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans = "AnimalInteractedTooRecently".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans = "CantInteractAnimalDowned".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans = "CantInteractAnimalAsleep".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans = "CantInteractAnimalBusy".Translate();
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0003F728 File Offset: 0x0003DB28
		protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal, bool forced)
		{
			LocalTargetInfo target = animal;
			bool result;
			if (!pawn.CanReserve(target, 1, -1, null, forced))
			{
				result = false;
			}
			else if (animal.Downed)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans, null);
				result = false;
			}
			else if (!animal.Awake())
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans, null);
				result = false;
			}
			else if (!animal.CanCasuallyInteractNow(false))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans, null);
				result = false;
			}
			else
			{
				int num = TrainableUtility.MinimumHandlingSkill(animal);
				if (num > pawn.skills.GetSkill(SkillDefOf.Animals).Level)
				{
					JobFailReason.Is("AnimalsSkillTooLow".Translate(new object[]
					{
						num
					}), null);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0003F808 File Offset: 0x0003DC08
		protected bool HasFoodToInteractAnimal(Pawn pawn, Pawn tamee)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			float num2 = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee);
			float num3 = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (tamee.RaceProps.CanEverEat(thing) && thing.def.ingestible.preferability <= FoodPreferability.RawTasty && !thing.def.IsDrug)
				{
					for (int j = 0; j < thing.stackCount; j++)
					{
						num3 += thing.GetStatValue(StatDefOf.Nutrition, true);
						if (num3 >= num2)
						{
							num++;
							num3 = 0f;
						}
						if (num >= 2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0003F8F0 File Offset: 0x0003DCF0
		protected Job TakeFoodForAnimalInteractJob(Pawn pawn, Pawn tamee)
		{
			float num = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f;
			ThingDef foodDef;
			Thing thing = FoodUtility.BestFoodSourceOnMap(pawn, tamee, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false);
			Job result;
			if (thing == null)
			{
				result = null;
			}
			else
			{
				float nutrition = FoodUtility.GetNutrition(thing, foodDef);
				result = new Job(JobDefOf.TakeInventory, thing)
				{
					count = Mathf.CeilToInt(num / nutrition)
				};
			}
			return result;
		}
	}
}
