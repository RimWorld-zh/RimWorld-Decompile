using RimWorld.Planet;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_CaravanRequest : IncidentWorker
	{
		private const float TravelBufferMultiple = 0.1f;

		private const float TravelBufferAbsolute = 1f;

		private const int MaxTileDistance = 36;

		private static readonly IntRange OfferDurationRange = new IntRange(10, 30);

		private static readonly IntRange BaseValueWantedRange = new IntRange(400, 3000);

		private static readonly SimpleCurve ValueFactorFromWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.5f),
				true
			},
			{
				new CurvePoint(50000f, 1f),
				true
			},
			{
				new CurvePoint(300000f, 2f),
				true
			}
		};

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(((Map)target).Tile) == null)
			{
				return false;
			}
			return base.CanFireNowSub(target);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Settlement settlement = IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(parms.target.Tile);
			if (settlement == null)
			{
				return false;
			}
			CaravanRequestComp component = ((WorldObject)settlement).GetComponent<CaravanRequestComp>();
			if (!this.GenerateCaravanRequest(component, (Map)parms.target))
			{
				return false;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCaravanRequest".Translate(), "LetterCaravanRequest".Translate(settlement.Label, GenLabel.ThingLabel(component.requestThingDef, null, component.requestCount).CapitalizeFirst(), component.rewards[0].LabelCap, (component.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F0")), LetterDefOf.Good, (WorldObject)settlement, (string)null);
			return true;
		}

		public bool GenerateCaravanRequest(CaravanRequestComp target, Map map)
		{
			int num = this.RandomOfferDuration(map.Tile, target.parent.Tile);
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
			target.requestCount = IncidentWorker_CaravanRequest.RandomRequestCount(target.requestThingDef, map);
			target.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			target.rewards.TryAdd(IncidentWorker_CaravanRequest.GenerateRewardFor(target.requestThingDef, target.requestCount, target.parent.Faction), true);
			target.expiration = Find.TickManager.TicksGame + num;
			return true;
		}

		public static Settlement RandomNearbyTradeableSettlement(int originTile)
		{
			return Find.WorldObjects.Settlements.Where((Func<Settlement, bool>)delegate(Settlement settlement)
			{
				if (settlement.Visitable && ((WorldObject)settlement).GetComponent<CaravanRequestComp>() != null && !((WorldObject)settlement).GetComponent<CaravanRequestComp>().ActiveRequest)
				{
					return Find.WorldGrid.ApproxDistanceInTiles(originTile, settlement.Tile) < 36.0 && Find.WorldReachability.CanReach(originTile, settlement.Tile);
				}
				return false;
			}).RandomElementWithFallback(null);
		}

		private static ThingDef RandomRequestedThingDef()
		{
			Func<ThingDef, bool> globalValidator = (Func<ThingDef, bool>)delegate(ThingDef td)
			{
				if (td.BaseMarketValue / td.BaseMass < 5.0)
				{
					return false;
				}
				if (!td.alwaysHaulable)
				{
					return false;
				}
				CompProperties_Rottable compProperties = td.GetCompProperties<CompProperties_Rottable>();
				if (compProperties != null && compProperties.daysToRotStart < 10.0)
				{
					return false;
				}
				if (td == ThingDefOf.Silver)
				{
					return false;
				}
				if (!td.PlayerAcquirable)
				{
					return false;
				}
				return true;
			};
			if (Rand.Value < 0.800000011920929)
			{
				ThingDef result = null;
				if (DefDatabase<ThingDef>.AllDefs.Where((Func<ThingDef, bool>)delegate(ThingDef td)
				{
					int result3;
					if ((td.IsWithinCategory(ThingCategoryDefOf.FoodMeals) || td.IsWithinCategory(ThingCategoryDefOf.PlantFoodRaw) || td.IsWithinCategory(ThingCategoryDefOf.PlantMatter) || td.IsWithinCategory(ThingCategoryDefOf.ResourcesRaw)) && td.BaseMarketValue < 4.0)
					{
						result3 = (globalValidator(td) ? 1 : 0);
						goto IL_005f;
					}
					result3 = 0;
					goto IL_005f;
					IL_005f:
					return (byte)result3 != 0;
				}).TryRandomElement<ThingDef>(out result))
				{
					return result;
				}
			}
			return DefDatabase<ThingDef>.AllDefs.Where((Func<ThingDef, bool>)delegate(ThingDef td)
			{
				int result2;
				if ((td.IsWithinCategory(ThingCategoryDefOf.Medicine) || td.IsWithinCategory(ThingCategoryDefOf.Drugs) || td.IsWithinCategory(ThingCategoryDefOf.Weapons) || td.IsWithinCategory(ThingCategoryDefOf.Apparel) || td.IsWithinCategory(ThingCategoryDefOf.ResourcesRaw)) && td.BaseMarketValue >= 4.0)
				{
					result2 = (globalValidator(td) ? 1 : 0);
					goto IL_006f;
				}
				result2 = 0;
				goto IL_006f;
				IL_006f:
				return (byte)result2 != 0;
			}).RandomElementWithFallback(null);
		}

		private static int RandomRequestCount(ThingDef thingDef, Map map)
		{
			float num = (float)IncidentWorker_CaravanRequest.BaseValueWantedRange.RandomInRange;
			float wealthTotal = map.wealthWatcher.WealthTotal;
			num *= IncidentWorker_CaravanRequest.ValueFactorFromWealthCurve.Evaluate(wealthTotal);
			return Mathf.Max(1, Mathf.RoundToInt(num / thingDef.BaseMarketValue));
		}

		private static Thing GenerateRewardFor(ThingDef thingDef, int quantity, Faction faction)
		{
			TechLevel techLevel = (faction != null) ? faction.def.techLevel : TechLevel.Spacer;
			ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
			{
				count = 1,
				totalMarketValue = thingDef.BaseMarketValue * (float)quantity * Rand.Range(1f, 2f),
				techLevel = techLevel,
				validator = (Predicate<ThingDef>)((ThingDef td) => td != thingDef)
			};
			return ItemCollectionGeneratorDefOf.CaravanRequestRewards.Worker.Generate(parms)[0];
		}

		private int RandomOfferDuration(int tileIdFrom, int tileIdTo)
		{
			int randomInRange = IncidentWorker_CaravanRequest.OfferDurationRange.RandomInRange;
			int num = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(tileIdFrom, tileIdTo, null);
			float num2 = (float)((float)num / 60000.0);
			int b = Mathf.CeilToInt(Mathf.Max((float)(num2 + 1.0), (float)(num2 * 1.1000000238418579)));
			randomInRange = Mathf.Max(randomInRange, b);
			int num3 = randomInRange;
			IntRange offerDurationRange = IncidentWorker_CaravanRequest.OfferDurationRange;
			if (num3 > offerDurationRange.max)
			{
				return -1;
			}
			return 60000 * randomInRange;
		}
	}
}
