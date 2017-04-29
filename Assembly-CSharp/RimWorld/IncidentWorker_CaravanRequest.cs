using RimWorld.Planet;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_CaravanRequest : IncidentWorker
	{
		public IntRange offerDuration = new IntRange(10, 30);

		public float travelBufferMultiple = 0.1f;

		public float travelBufferAbsolute = 1f;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			return IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(((Map)target).Tile) != null && base.CanFireNowSub(target);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Settlement settlement = IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(parms.target.Tile);
			if (settlement == null)
			{
				return false;
			}
			CaravanRequestComp component = settlement.GetComponent<CaravanRequestComp>();
			if (!this.GenerateCaravanRequest(component, parms.target.Tile))
			{
				return false;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCaravanRequest".Translate(), "LetterCaravanRequest".Translate(new object[]
			{
				settlement.Label,
				GenLabel.ThingLabel(component.requestThingDef, null, component.requestCount).CapitalizeFirst(),
				component.rewards[0].LabelCap,
				(component.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F0")
			}), LetterType.Good, settlement, null);
			return true;
		}

		public bool GenerateCaravanRequest(CaravanRequestComp target, int tileFrom)
		{
			int num = this.RandomOfferDuration(tileFrom, target.parent.Tile);
			if (num < 1)
			{
				return false;
			}
			target.requestThingDef = IncidentWorker_CaravanRequest.RandomRequestedThingDef();
			if (target.requestThingDef == null)
			{
				Log.Error("Attempted to create a caravan request, but couldn't find a valid request object");
				return false;
			}
			target.requestCount = IncidentWorker_CaravanRequest.RandomRequestCount(target.requestThingDef);
			target.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			target.rewards.TryAdd(IncidentWorker_CaravanRequest.GenerateRewardFor(target.requestThingDef, target.requestCount, target.parent.Faction), true);
			target.expiration = Find.TickManager.TicksGame + num;
			return true;
		}

		private static Settlement RandomNearbyTradeableSettlement(int originTile)
		{
			return (from settlement in Find.WorldObjects.Settlements
			where settlement.Visitable && settlement.GetComponent<CaravanRequestComp>() != null && !settlement.GetComponent<CaravanRequestComp>().ActiveRequest && Find.WorldGrid.ApproxDistanceInTiles(originTile, settlement.Tile) < 50f && Find.WorldReachability.CanReach(originTile, settlement.Tile)
			select settlement).RandomElementWithFallback(null);
		}

		private static ThingDef RandomRequestedThingDef()
		{
			Func<ThingDef, bool> globalValidator = delegate(ThingDef td)
			{
				if (td.BaseMarketValue / td.BaseMass < 0.02f)
				{
					return false;
				}
				if (!td.alwaysHaulable)
				{
					return false;
				}
				CompProperties_Rottable compProperties = td.GetCompProperties<CompProperties_Rottable>();
				return (compProperties == null || compProperties.daysToRotStart >= 10f) && td != ThingDefOf.Silver && td.PlayerAcquirable;
			};
			if (Rand.Value < 0.8f)
			{
				ThingDef thingDef = null;
				(from td in DefDatabase<ThingDef>.AllDefs
				where (td.IsWithinCategory(ThingCategoryDefOf.FoodMeals) || td.IsWithinCategory(ThingCategoryDefOf.PlantFoodRaw) || td.IsWithinCategory(ThingCategoryDefOf.PlantMatter) || td.IsWithinCategory(ThingCategoryDefOf.StoneBlocks) || td.IsWithinCategory(ThingCategoryDefOf.Chunks) || (td.IsWithinCategory(ThingCategoryDefOf.ResourcesRaw) && td.BaseMarketValue < 4f)) && globalValidator(td)
				select td).TryRandomElement(out thingDef);
				if (thingDef != null)
				{
					return thingDef;
				}
			}
			return (from td in DefDatabase<ThingDef>.AllDefs
			where (td.IsWithinCategory(ThingCategoryDefOf.Medicine) || td.IsWithinCategory(ThingCategoryDefOf.Drugs) || td.IsWithinCategory(ThingCategoryDefOf.Weapons) || td.IsWithinCategory(ThingCategoryDefOf.Apparel) || (td.IsWithinCategory(ThingCategoryDefOf.ResourcesRaw) && td.BaseMarketValue >= 4f)) && globalValidator(td)
			select td).RandomElementWithFallback(null);
		}

		private static int RandomRequestCount(ThingDef thingDef)
		{
			return Mathf.Max(1, Mathf.RoundToInt(Rand.Range(400f, 3000f) / thingDef.BaseMarketValue));
		}

		private static Thing GenerateRewardFor(ThingDef thingDef, int quantity, Faction faction)
		{
			TechLevel techLevel = (faction != null) ? faction.def.techLevel : TechLevel.Spacer;
			ItemCollectionGeneratorParams parms = default(ItemCollectionGeneratorParams);
			parms.count = 1;
			parms.totalMarketValue = thingDef.BaseMarketValue * (float)quantity * Rand.Range(1f, 2f);
			parms.techLevel = techLevel;
			parms.validator = ((ThingDef td) => td != thingDef);
			return ItemCollectionGeneratorDefOf.CaravanRequestRewards.Worker.Generate(parms)[0];
		}

		private int RandomOfferDuration(int tileIdFrom, int tileIdTo)
		{
			int num = this.offerDuration.RandomInRange;
			int num2 = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(tileIdFrom, tileIdTo, null);
			float num3 = (float)num2 / 60000f;
			int b = Mathf.CeilToInt(Mathf.Max(num3 + this.travelBufferAbsolute, num3 * (1f + this.travelBufferMultiple)));
			num = Mathf.Max(num, b);
			if (num > this.offerDuration.max)
			{
				return -1;
			}
			return 60000 * num;
		}
	}
}
