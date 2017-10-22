using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanPawnsNeedsUtility
	{
		private const float AutoRefillMiscNeedsIfBelowLevel = 0.3f;

		private const float ExtraJoyFromEatingFood = 0.5f;

		private static List<Thing> tmpInvFood = new List<Thing>();

		public static void TrySatisfyPawnsNeeds(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int num = pawnsListForReading.Count - 1; num >= 0; num--)
			{
				CaravanPawnsNeedsUtility.TrySatisfyPawnNeeds(pawnsListForReading[num], caravan);
			}
		}

		public static bool AnyPawnOutOfFood(Caravan c, out string malnutritionHediff)
		{
			CaravanPawnsNeedsUtility.tmpInvFood.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.IsNutritionGivingIngestible)
				{
					CaravanPawnsNeedsUtility.tmpInvFood.Add(list[i]);
				}
			}
			List<Pawn> pawnsListForReading = c.PawnsListForReading;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < pawnsListForReading.Count)
				{
					Pawn pawn = pawnsListForReading[num];
					if (pawn.RaceProps.EatsFood && !VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
					{
						bool flag = false;
						int num2 = 0;
						while (num2 < CaravanPawnsNeedsUtility.tmpInvFood.Count)
						{
							if (!CaravanPawnsNeedsUtility.CanEverEatForNutrition(CaravanPawnsNeedsUtility.tmpInvFood[num2].def, pawn))
							{
								num2++;
								continue;
							}
							flag = true;
							break;
						}
						if (!flag)
						{
							int num3 = -1;
							string text = (string)null;
							for (int j = 0; j < pawnsListForReading.Count; j++)
							{
								Hediff firstHediffOfDef = pawnsListForReading[j].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
								if (firstHediffOfDef != null && (text == null || firstHediffOfDef.CurStageIndex > num3))
								{
									num3 = firstHediffOfDef.CurStageIndex;
									text = firstHediffOfDef.LabelCap;
								}
							}
							malnutritionHediff = text;
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				malnutritionHediff = (string)null;
				result = false;
				break;
			}
			return result;
		}

		private static void TrySatisfyPawnNeeds(Pawn pawn, Caravan caravan)
		{
			if (!pawn.Dead)
			{
				List<Need> allNeeds = pawn.needs.AllNeeds;
				for (int i = 0; i < allNeeds.Count; i++)
				{
					Need need = allNeeds[i];
					Need_Rest need_Rest = need as Need_Rest;
					Need_Food need_Food = need as Need_Food;
					Need_Chemical need_Chemical = need as Need_Chemical;
					if (need_Rest != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyRestNeed(pawn, need_Rest, caravan);
					}
					else if (need_Food != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyFoodNeed(pawn, need_Food, caravan);
					}
					else if (need_Chemical != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyChemicalNeed(pawn, need_Chemical, caravan);
					}
				}
			}
		}

		private static void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest, Caravan caravan)
		{
			if (caravan.Resting)
			{
				float restEffectiveness = RestUtility.PawnHealthRestEffectivenessFactor(pawn);
				rest.TickResting(restEffectiveness);
			}
		}

		private static void TrySatisfyFoodNeed(Pawn pawn, Need_Food food, Caravan caravan)
		{
			if ((int)food.CurCategory >= 1)
			{
				Thing thing = default(Thing);
				Pawn pawn2 = default(Pawn);
				if (VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					VirtualPlantsUtility.EatVirtualPlants(pawn);
				}
				else if (CaravanInventoryUtility.TryGetBestFood(caravan, pawn, out thing, out pawn2))
				{
					food.CurLevel += thing.Ingested(pawn, food.NutritionWanted);
					if (thing.Destroyed)
					{
						if (pawn2 != null)
						{
							pawn2.inventory.innerContainer.Remove(thing);
							caravan.RecacheImmobilizedNow();
							caravan.RecacheDaysWorthOfFood();
						}
						if (!CaravanInventoryUtility.TryGetBestFood(caravan, pawn, out thing, out pawn2))
						{
							Messages.Message("MessageCaravanRunOutOfFood".Translate(caravan.LabelCap, pawn.Label), (WorldObject)caravan, MessageTypeDefOf.ThreatBig);
						}
					}
				}
			}
		}

		private static void TrySatisfyChemicalNeed(Pawn pawn, Need_Chemical chemical, Caravan caravan)
		{
			Thing drug = default(Thing);
			Pawn drugOwner = default(Pawn);
			if ((int)chemical.CurCategory < 2 && CaravanInventoryUtility.TryGetBestDrug(caravan, pawn, chemical, out drug, out drugOwner))
			{
				CaravanPawnsNeedsUtility.IngestDrug(pawn, drug, drugOwner, caravan);
			}
		}

		public static void IngestDrug(Pawn pawn, Thing drug, Pawn drugOwner, Caravan caravan)
		{
			float num = drug.Ingested(pawn, 0f);
			Need_Food food = pawn.needs.food;
			if (food != null)
			{
				food.CurLevel += num;
			}
			if (drug.Destroyed && drugOwner != null)
			{
				drugOwner.inventory.innerContainer.Remove(drug);
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}
		}

		public static bool CanEverEatForNutrition(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.RaceProps.CanEverEat(food) && (int)food.ingestible.preferability > 1 && (!pawn.IsTeetotaler() || !food.IsDrug);
		}

		public static bool CanNowEatForNutrition(ThingDef food, Pawn pawn)
		{
			return (byte)(CaravanPawnsNeedsUtility.CanEverEatForNutrition(food, pawn) ? ((!pawn.RaceProps.Humanlike || (int)pawn.needs.food.CurCategory >= 3 || (int)food.ingestible.preferability > 3) ? 1 : 0) : 0) != 0;
		}

		public static bool CanNowEatForNutrition(Thing food, Pawn pawn)
		{
			return (byte)(food.IngestibleNow ? (CaravanPawnsNeedsUtility.CanNowEatForNutrition(food.def, pawn) ? 1 : 0) : 0) != 0;
		}

		public static float GetFoodScore(Thing food, Pawn pawn)
		{
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn);
			if (pawn.RaceProps.Humanlike)
			{
				CompRottable compRottable = food.TryGetComp<CompRottable>();
				int a = (compRottable == null) ? 2147483647 : compRottable.TicksUntilRotAtCurrentTemp;
				float a2 = (float)(1.0 - (float)Mathf.Min(a, 3600000) / 3600000.0);
				num += Mathf.Min(a2, 0.999f);
			}
			return num;
		}

		public static float GetFoodScore(ThingDef food, Pawn pawn)
		{
			float result;
			if (pawn.RaceProps.Humanlike)
			{
				result = (float)(int)food.ingestible.preferability;
			}
			else
			{
				float num = 0f;
				if (food == ThingDefOf.Kibble || food == ThingDefOf.Hay)
				{
					num = 5f;
				}
				else if (food.ingestible.preferability == FoodPreferability.DesperateOnlyForHumanlikes)
				{
					num = 4f;
				}
				else if (food.ingestible.preferability == FoodPreferability.RawBad)
				{
					num = 3f;
				}
				else if (food.ingestible.preferability == FoodPreferability.RawTasty)
				{
					num = 2f;
				}
				else if ((int)food.ingestible.preferability < 6)
				{
					num = 1f;
				}
				result = num + Mathf.Min((float)(food.ingestible.nutrition / 100.0), 0.999f);
			}
			return result;
		}

		public static void Notify_CaravanMemberIngestedFood(Pawn p, ThingDef foodDef)
		{
			if (!p.Dead && p.needs.joy != null && !(foodDef.ingestible.nutrition <= 0.0))
			{
				bool flag = false;
				List<Pawn> pawnsListForReading = p.GetCaravan().PawnsListForReading;
				int num = 0;
				while (num < pawnsListForReading.Count)
				{
					if (pawnsListForReading[num] == p || !pawnsListForReading[num].RaceProps.Humanlike || pawnsListForReading[num].Downed || pawnsListForReading[num].InMentalState || p.IsPrisoner != pawnsListForReading[num].IsPrisoner)
					{
						num++;
						continue;
					}
					flag = true;
					break;
				}
				JoyKindDef joyKind = (!flag) ? JoyKindDefOf.Meditative : Rand.Element(JoyKindDefOf.Meditative, JoyKindDefOf.Social);
				p.needs.joy.GainJoy(0.5f, joyKind);
			}
		}
	}
}
