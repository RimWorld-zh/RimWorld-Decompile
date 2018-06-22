using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005DF RID: 1503
	public static class CaravanPawnsNeedsUtility
	{
		// Token: 0x06001DB5 RID: 7605 RVA: 0x00100404 File Offset: 0x000FE804
		public static void TrySatisfyPawnsNeeds(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = pawnsListForReading.Count - 1; i >= 0; i--)
			{
				CaravanPawnsNeedsUtility.TrySatisfyPawnNeeds(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x00100444 File Offset: 0x000FE844
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

		// Token: 0x06001DB7 RID: 7607 RVA: 0x001005E0 File Offset: 0x000FE9E0
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

		// Token: 0x06001DB8 RID: 7608 RVA: 0x00100697 File Offset: 0x000FEA97
		private static void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest, Caravan caravan)
		{
			if (caravan.Resting)
			{
				rest.TickResting(1f);
			}
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x001006B0 File Offset: 0x000FEAB0
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

		// Token: 0x06001DBA RID: 7610 RVA: 0x001007A0 File Offset: 0x000FEBA0
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

		// Token: 0x06001DBB RID: 7611 RVA: 0x001007DC File Offset: 0x000FEBDC
		private static void TrySatisfyJoyNeed(Pawn pawn, Need_Joy joy, Caravan caravan)
		{
			float caravanNotMovingJoyGainPerTick = CaravanPawnsNeedsUtility.GetCaravanNotMovingJoyGainPerTick(pawn, caravan);
			if (caravanNotMovingJoyGainPerTick > 0f)
			{
				joy.GainJoy(caravanNotMovingJoyGainPerTick, JoyKindDefOf.Social);
			}
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x00100810 File Offset: 0x000FEC10
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
					result = statValue * 0.035f / 2500f;
				}
			}
			return result;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00100874 File Offset: 0x000FEC74
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

		// Token: 0x06001DBE RID: 7614 RVA: 0x001008E0 File Offset: 0x000FECE0
		public static bool CanEverEatForNutrition(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.RaceProps.CanEverEat(food) && food.ingestible.preferability > FoodPreferability.NeverForNutrition && (!food.IsDrug || !pawn.IsTeetotaler());
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0010093C File Offset: 0x000FED3C
		public static bool CanNowEatForNutrition(ThingDef food, Pawn pawn)
		{
			return CaravanPawnsNeedsUtility.CanEverEatForNutrition(food, pawn) && (!pawn.RaceProps.Humanlike || pawn.needs.food.CurCategory >= HungerCategory.Starving || food.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes);
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x001009A4 File Offset: 0x000FEDA4
		public static bool CanNowEatForNutrition(Thing food, Pawn pawn)
		{
			return food.IngestibleNow && CaravanPawnsNeedsUtility.CanNowEatForNutrition(food.def, pawn);
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x001009E4 File Offset: 0x000FEDE4
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

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00100A6C File Offset: 0x000FEE6C
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

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00100B4C File Offset: 0x000FEF4C
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

		// Token: 0x0400119D RID: 4509
		private const float AutoRefillMiscNeedsIfBelowLevel = 0.3f;

		// Token: 0x0400119E RID: 4510
		private const float ExtraJoyFromEatingFoodPerNutritionPct = 0.2f;

		// Token: 0x0400119F RID: 4511
		private const float BaseJoyGainPerHour = 0.035f;

		// Token: 0x040011A0 RID: 4512
		private static List<Thing> tmpInvFood = new List<Thing>();
	}
}
