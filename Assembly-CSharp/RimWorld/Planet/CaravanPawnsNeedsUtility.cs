using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E3 RID: 1507
	public static class CaravanPawnsNeedsUtility
	{
		// Token: 0x06001DBC RID: 7612 RVA: 0x00100338 File Offset: 0x000FE738
		public static void TrySatisfyPawnsNeeds(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = pawnsListForReading.Count - 1; i >= 0; i--)
			{
				CaravanPawnsNeedsUtility.TrySatisfyPawnNeeds(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00100378 File Offset: 0x000FE778
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
			for (int j = 0; j < pawnsListForReading.Count; j++)
			{
				Pawn pawn = pawnsListForReading[j];
				if (pawn.RaceProps.EatsFood && !VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					bool flag = false;
					for (int k = 0; k < CaravanPawnsNeedsUtility.tmpInvFood.Count; k++)
					{
						if (CaravanPawnsNeedsUtility.CanEverEatForNutrition(CaravanPawnsNeedsUtility.tmpInvFood[k].def, pawn))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num = -1;
						string text = null;
						for (int l = 0; l < pawnsListForReading.Count; l++)
						{
							Hediff firstHediffOfDef = pawnsListForReading[l].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
							if (firstHediffOfDef != null && (text == null || firstHediffOfDef.CurStageIndex > num))
							{
								num = firstHediffOfDef.CurStageIndex;
								text = firstHediffOfDef.LabelCap;
							}
						}
						malnutritionHediff = text;
						CaravanPawnsNeedsUtility.tmpInvFood.Clear();
						return true;
					}
				}
			}
			malnutritionHediff = null;
			CaravanPawnsNeedsUtility.tmpInvFood.Clear();
			return false;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x00100514 File Offset: 0x000FE914
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
					Need_Joy need_Joy = need as Need_Joy;
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
					else if (need_Joy != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyJoyNeed(pawn, need_Joy, caravan);
					}
				}
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x001005CB File Offset: 0x000FE9CB
		private static void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest, Caravan caravan)
		{
			if (caravan.Resting)
			{
				rest.TickResting(1f);
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x001005E4 File Offset: 0x000FE9E4
		private static void TrySatisfyFoodNeed(Pawn pawn, Need_Food food, Caravan caravan)
		{
			if (food.CurCategory >= HungerCategory.Hungry)
			{
				Thing thing;
				Pawn pawn2;
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
						if (!caravan.notifiedOutOfFood && !CaravanInventoryUtility.TryGetBestFood(caravan, pawn, out thing, out pawn2))
						{
							Messages.Message("MessageCaravanRanOutOfFood".Translate(new object[]
							{
								caravan.LabelCap,
								pawn.Label
							}), caravan, MessageTypeDefOf.ThreatBig, true);
							caravan.notifiedOutOfFood = true;
						}
					}
				}
			}
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x001006D4 File Offset: 0x000FEAD4
		private static void TrySatisfyChemicalNeed(Pawn pawn, Need_Chemical chemical, Caravan caravan)
		{
			if (chemical.CurCategory < DrugDesireCategory.Satisfied)
			{
				Thing drug;
				Pawn drugOwner;
				if (CaravanInventoryUtility.TryGetBestDrug(caravan, pawn, chemical, out drug, out drugOwner))
				{
					CaravanPawnsNeedsUtility.IngestDrug(pawn, drug, drugOwner, caravan);
				}
			}
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00100710 File Offset: 0x000FEB10
		private static void TrySatisfyJoyNeed(Pawn pawn, Need_Joy joy, Caravan caravan)
		{
			float caravanNotMovingJoyGainPerTick = CaravanPawnsNeedsUtility.GetCaravanNotMovingJoyGainPerTick(pawn, caravan);
			if (caravanNotMovingJoyGainPerTick > 0f)
			{
				joy.GainJoy(caravanNotMovingJoyGainPerTick, JoyKindDefOf.Social);
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00100744 File Offset: 0x000FEB44
		public static float GetCaravanNotMovingJoyGainPerTick(Pawn pawn, Caravan caravan)
		{
			float result;
			if (caravan.pather.MovingNow)
			{
				result = 0f;
			}
			else
			{
				Pawn pawn2 = BestCaravanPawnUtility.FindBestEntertainingPawnFor(caravan, pawn);
				if (pawn2 == null)
				{
					result = 0f;
				}
				else
				{
					float statValue = pawn2.GetStatValue(StatDefOf.SocialImpact, true);
					result = statValue * 0.031f / 2500f;
				}
			}
			return result;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x001007A8 File Offset: 0x000FEBA8
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

		// Token: 0x06001DC5 RID: 7621 RVA: 0x00100814 File Offset: 0x000FEC14
		public static bool CanEverEatForNutrition(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.RaceProps.CanEverEat(food) && food.ingestible.preferability > FoodPreferability.NeverForNutrition && (!food.IsDrug || !pawn.IsTeetotaler());
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00100870 File Offset: 0x000FEC70
		public static bool CanNowEatForNutrition(ThingDef food, Pawn pawn)
		{
			return CaravanPawnsNeedsUtility.CanEverEatForNutrition(food, pawn) && (!pawn.RaceProps.Humanlike || pawn.needs.food.CurCategory >= HungerCategory.Starving || food.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x001008D8 File Offset: 0x000FECD8
		public static bool CanNowEatForNutrition(Thing food, Pawn pawn)
		{
			return food.IngestibleNow && CaravanPawnsNeedsUtility.CanNowEatForNutrition(food.def, pawn);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00100918 File Offset: 0x000FED18
		public static float GetFoodScore(Thing food, Pawn pawn)
		{
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true));
			if (pawn.RaceProps.Humanlike)
			{
				CompRottable compRottable = food.TryGetComp<CompRottable>();
				int a = (compRottable == null) ? int.MaxValue : compRottable.TicksUntilRotAtCurrentTemp;
				float a2 = 1f - (float)Mathf.Min(a, 3600000) / 3600000f;
				num += Mathf.Min(a2, 0.999f);
			}
			return num;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x001009A0 File Offset: 0x000FEDA0
		public static float GetFoodScore(ThingDef food, Pawn pawn, float singleFoodNutrition)
		{
			float result;
			if (pawn.RaceProps.Humanlike)
			{
				result = (float)food.ingestible.preferability;
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
				else if (food.ingestible.preferability < FoodPreferability.MealAwful)
				{
					num = 1f;
				}
				result = num + Mathf.Min(singleFoodNutrition / 100f, 0.999f);
			}
			return result;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00100A80 File Offset: 0x000FEE80
		public static void Notify_CaravanMemberIngestedFood(Pawn p, float nutritionIngested)
		{
			if (!p.Dead && p.needs.joy != null)
			{
				if (nutritionIngested > 0f)
				{
					Pawn pawn = BestCaravanPawnUtility.FindBestEntertainingPawnFor(p.GetCaravan(), p);
					JoyKindDef joyKind = (pawn == null) ? JoyKindDefOf.Meditative : Rand.Element<JoyKindDef>(JoyKindDefOf.Meditative, JoyKindDefOf.Social);
					float amount = 0.2f * Mathf.Min(nutritionIngested / p.needs.food.MaxLevel, 1f);
					p.needs.joy.GainJoy(amount, joyKind);
				}
			}
		}

		// Token: 0x040011A0 RID: 4512
		private const float AutoRefillMiscNeedsIfBelowLevel = 0.3f;

		// Token: 0x040011A1 RID: 4513
		private const float ExtraJoyFromEatingFoodPerNutritionPct = 0.2f;

		// Token: 0x040011A2 RID: 4514
		private const float BaseJoyGainPerHour = 0.031f;

		// Token: 0x040011A3 RID: 4515
		private static List<Thing> tmpInvFood = new List<Thing>();
	}
}
