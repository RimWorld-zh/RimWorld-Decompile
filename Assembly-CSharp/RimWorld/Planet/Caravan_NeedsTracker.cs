using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class Caravan_NeedsTracker : IExposable
	{
		public Caravan caravan;

		private Dictionary<Pawn, Building_Bed> bedsUsedLastTick = new Dictionary<Pawn, Building_Bed>();

		private List<Pawn> tmpPawnsSaving;

		private List<Building_Bed> tmpBedsSaving;

		private static List<Building_Bed> tmpUsableBeds = new List<Building_Bed>();

		private static List<Thing> tmpInvFood = new List<Thing>();

		[CompilerGenerated]
		private static Predicate<KeyValuePair<Pawn, Building_Bed>> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<KeyValuePair<Pawn, Building_Bed>> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Building_Bed, float> <>f__am$cache2;

		public Caravan_NeedsTracker()
		{
		}

		public Caravan_NeedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.bedsUsedLastTick.RemoveAll((KeyValuePair<Pawn, Building_Bed> x) => x.Key.Destroyed || x.Value.DestroyedOrNull());
			}
			Scribe_Collections.Look<Pawn, Building_Bed>(ref this.bedsUsedLastTick, "bedsUsedLastTick", LookMode.Reference, LookMode.Reference, ref this.tmpPawnsSaving, ref this.tmpBedsSaving);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.bedsUsedLastTick.RemoveAll((KeyValuePair<Pawn, Building_Bed> x) => x.Value.DestroyedOrNull());
			}
		}

		public void NeedsTrackerTick()
		{
			this.TrySatisfyPawnsNeeds();
		}

		public void TrySatisfyPawnsNeeds()
		{
			this.bedsUsedLastTick.Clear();
			this.GetUsableBeds(Caravan_NeedsTracker.tmpUsableBeds, true);
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = pawnsListForReading.Count - 1; i >= 0; i--)
			{
				this.TrySatisfyPawnNeeds(pawnsListForReading[i], Caravan_NeedsTracker.tmpUsableBeds);
			}
		}

		private void TrySatisfyPawnNeeds(Pawn pawn, List<Building_Bed> usableBeds)
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
						this.TrySatisfyRestNeed(pawn, need_Rest, usableBeds);
					}
					else if (need_Food != null)
					{
						this.TrySatisfyFoodNeed(pawn, need_Food);
					}
					else if (need_Chemical != null)
					{
						this.TrySatisfyChemicalNeed(pawn, need_Chemical);
					}
					else if (need_Joy != null)
					{
						this.TrySatisfyJoyNeed(pawn, need_Joy);
					}
				}
			}
		}

		private void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest, List<Building_Bed> usableBeds)
		{
			if (this.caravan.Resting)
			{
				Building_Bed building_Bed = null;
				for (int i = 0; i < usableBeds.Count; i++)
				{
					if (RestUtility.CanUseBedEver(pawn, usableBeds[i].def))
					{
						building_Bed = usableBeds[i];
						this.bedsUsedLastTick.Add(pawn, building_Bed);
						usableBeds.RemoveAt(i);
						break;
					}
				}
				float restEffectiveness = (building_Bed == null) ? 0.8f : building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true);
				rest.TickResting(restEffectiveness);
			}
		}

		private void TrySatisfyFoodNeed(Pawn pawn, Need_Food food)
		{
			if (food.CurCategory >= HungerCategory.Hungry)
			{
				Thing thing;
				Pawn pawn2;
				if (VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					VirtualPlantsUtility.EatVirtualPlants(pawn);
				}
				else if (CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn2))
				{
					food.CurLevel += thing.Ingested(pawn, food.NutritionWanted);
					if (thing.Destroyed)
					{
						if (pawn2 != null)
						{
							pawn2.inventory.innerContainer.Remove(thing);
							this.caravan.RecacheImmobilizedNow();
							this.caravan.RecacheDaysWorthOfFood();
						}
						if (!this.caravan.notifiedOutOfFood && !CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn2))
						{
							Messages.Message("MessageCaravanRanOutOfFood".Translate(new object[]
							{
								this.caravan.LabelCap,
								pawn.Label
							}), this.caravan, MessageTypeDefOf.ThreatBig, true);
							this.caravan.notifiedOutOfFood = true;
						}
					}
				}
			}
		}

		private void TrySatisfyChemicalNeed(Pawn pawn, Need_Chemical chemical)
		{
			if (chemical.CurCategory < DrugDesireCategory.Satisfied)
			{
				Thing drug;
				Pawn drugOwner;
				if (CaravanInventoryUtility.TryGetDrugToSatisfyChemicalNeed(this.caravan, pawn, chemical, out drug, out drugOwner))
				{
					this.IngestDrug(pawn, drug, drugOwner);
				}
			}
		}

		public void IngestDrug(Pawn pawn, Thing drug, Pawn drugOwner)
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
				this.caravan.RecacheImmobilizedNow();
				this.caravan.RecacheDaysWorthOfFood();
			}
		}

		private void TrySatisfyJoyNeed(Pawn pawn, Need_Joy joy)
		{
			if (pawn.IsHashIntervalTick(1250))
			{
				float num = this.GetCurrentJoyGainPerTick(pawn);
				if (num > 0f)
				{
					num *= 1250f;
					int num2 = 0;
					for (int i = 0; i < this.caravan.pawns.Count; i++)
					{
						if (this.caravan.pawns[i].RaceProps.Humanlike && !this.caravan.pawns[i].Downed && !this.caravan.pawns[i].InMentalState)
						{
							num2++;
						}
					}
					JoyKindDef joyKind = (num2 < 2) ? JoyKindDefOf.Meditative : Rand.Element<JoyKindDef>(JoyKindDefOf.Social, JoyKindDefOf.Meditative);
					joy.GainJoy(num, joyKind);
				}
			}
		}

		public float GetCurrentJoyGainPerTick(Pawn pawn)
		{
			float result;
			if (this.caravan.pather.MovingNow)
			{
				result = 0f;
			}
			else
			{
				result = 3.2E-05f;
			}
			return result;
		}

		public Building_Bed GetBedUsedLastTickBy(Pawn p)
		{
			Building_Bed result;
			Building_Bed building_Bed;
			if (!this.caravan.pawns.Contains(p))
			{
				result = null;
			}
			else if (this.bedsUsedLastTick.TryGetValue(p, out building_Bed) && !building_Bed.DestroyedOrNull())
			{
				result = building_Bed;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public int GetBedCountUsedLastTick()
		{
			return this.bedsUsedLastTick.Count;
		}

		public bool AnyPawnOutOfFood(out string malnutritionHediff)
		{
			Caravan_NeedsTracker.tmpInvFood.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.IsNutritionGivingIngestible)
				{
					Caravan_NeedsTracker.tmpInvFood.Add(list[i]);
				}
			}
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int j = 0; j < pawnsListForReading.Count; j++)
			{
				Pawn pawn = pawnsListForReading[j];
				if (pawn.RaceProps.EatsFood && !VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					bool flag = false;
					for (int k = 0; k < Caravan_NeedsTracker.tmpInvFood.Count; k++)
					{
						if (CaravanPawnsNeedsUtility.CanEatForNutritionEver(Caravan_NeedsTracker.tmpInvFood[k].def, pawn))
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
						Caravan_NeedsTracker.tmpInvFood.Clear();
						return true;
					}
				}
			}
			malnutritionHediff = null;
			Caravan_NeedsTracker.tmpInvFood.Clear();
			return false;
		}

		private void GetUsableBeds(List<Building_Bed> outBeds, bool sort)
		{
			outBeds.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Bed building_Bed = list[i].GetInnerIfMinified() as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_caravansCanUse)
				{
					for (int j = 0; j < list[i].stackCount; j++)
					{
						for (int k = 0; k < building_Bed.SleepingSlotsCount; k++)
						{
							outBeds.Add(building_Bed);
						}
					}
				}
			}
			if (sort)
			{
				outBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.BedRestEffectiveness, true));
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Caravan_NeedsTracker()
		{
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(KeyValuePair<Pawn, Building_Bed> x)
		{
			return x.Key.Destroyed || x.Value.DestroyedOrNull();
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__1(KeyValuePair<Pawn, Building_Bed> x)
		{
			return x.Value.DestroyedOrNull();
		}

		[CompilerGenerated]
		private static float <GetUsableBeds>m__2(Building_Bed x)
		{
			return x.GetStatValue(StatDefOf.BedRestEffectiveness, true);
		}
	}
}
