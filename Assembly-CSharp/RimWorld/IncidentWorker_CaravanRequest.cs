using RimWorld.Planet;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_CaravanRequest : IncidentWorker
	{
		private static readonly IntRange OfferDurationRange = new IntRange(10, 30);

		private const float TravelBufferMultiple = 0.1f;

		private const float TravelBufferAbsolute = 1f;

		private const int MaxTileDistance = 36;

		public static readonly IntRange BaseValueWantedRange = new IntRange(400, 3000);

		public static readonly FloatRange RewardMarketValueFactorRange = new FloatRange(1f, 2f);

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
			return IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(((Map)target).Tile) != null && base.CanFireNowSub(target);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Settlement settlement = IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(parms.target.Tile);
			bool result;
			if (settlement == null)
			{
				result = false;
			}
			else
			{
				CaravanRequestComp component = ((WorldObject)settlement).GetComponent<CaravanRequestComp>();
				if (!this.GenerateCaravanRequest(component, (Map)parms.target))
				{
					result = false;
				}
				else
				{
					Find.LetterStack.ReceiveLetter("LetterLabelCaravanRequest".Translate(), "LetterCaravanRequest".Translate(settlement.Label, GenLabel.ThingLabel(component.requestThingDef, null, component.requestCount).CapitalizeFirst(), component.rewards[0].LabelCap, (component.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F0")), LetterDefOf.PositiveEvent, (WorldObject)settlement, (string)null);
					result = true;
				}
			}
			return result;
		}

		public bool GenerateCaravanRequest(CaravanRequestComp target, Map map)
		{
			int num = this.RandomOfferDuration(map.Tile, target.parent.Tile);
			bool result;
			if (num < 1)
			{
				result = false;
			}
			else
			{
				target.requestThingDef = IncidentWorker_CaravanRequest.RandomRequestedThingDef();
				if (target.requestThingDef == null)
				{
					Log.Error("Attempted to create a caravan request, but couldn't find a valid request object");
					result = false;
				}
				else
				{
					target.requestCount = IncidentWorker_CaravanRequest.RandomRequestCount(target.requestThingDef, map);
					target.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
					target.rewards.TryAdd(IncidentWorker_CaravanRequest.GenerateRewardFor(target.requestThingDef, target.requestCount, target.parent.Faction), true);
					target.expiration = Find.TickManager.TicksGame + num;
					result = true;
				}
			}
			return result;
		}

		public static Settlement RandomNearbyTradeableSettlement(int originTile)
		{
			return (from settlement in Find.WorldObjects.Settlements
			where settlement.Visitable && ((WorldObject)settlement).GetComponent<CaravanRequestComp>() != null && !((WorldObject)settlement).GetComponent<CaravanRequestComp>().ActiveRequest && Find.WorldGrid.ApproxDistanceInTiles(originTile, settlement.Tile) < 36.0 && Find.WorldReachability.CanReach(originTile, settlement.Tile)
			select settlement).RandomElementWithFallback(null);
		}

		private static ThingDef RandomRequestedThingDef()
		{
			Func<ThingDef, bool> globalValidator = (Func<ThingDef, bool>)delegate(ThingDef td)
			{
				bool result;
				if (td.BaseMarketValue / td.BaseMass < 5.0)
				{
					result = false;
				}
				else if (!td.alwaysHaulable)
				{
					result = false;
				}
				else
				{
					CompProperties_Rottable compProperties = td.GetCompProperties<CompProperties_Rottable>();
					result = ((byte)((compProperties == null || !(compProperties.daysToRotStart < 10.0)) ? ((td.ingestible == null || !td.ingestible.HumanEdible) ? ((td != ThingDefOf.Silver) ? (td.PlayerAcquirable ? 1 : 0) : 0) : 0) : 0) != 0);
				}
				return result;
			};
			return (from td in DefDatabase<ThingDef>.AllDefs
			where globalValidator(td)
			select td).RandomElementWithFallback(null);
		}

		private static int RandomRequestCount(ThingDef thingDef, Map map)
		{
			float num = (float)IncidentWorker_CaravanRequest.BaseValueWantedRange.RandomInRange;
			float wealthTotal = map.wealthWatcher.WealthTotal;
			num *= IncidentWorker_CaravanRequest.ValueFactorFromWealthCurve.Evaluate(wealthTotal);
			return ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(num / thingDef.BaseMarketValue)));
		}

		private static Thing GenerateRewardFor(ThingDef thingDef, int quantity, Faction faction)
		{
			TechLevel value = (faction != null) ? faction.def.techLevel : TechLevel.Spacer;
			ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
			{
				totalMarketValue = new float?(thingDef.BaseMarketValue * (float)quantity * IncidentWorker_CaravanRequest.RewardMarketValueFactorRange.RandomInRange),
				techLevel = new TechLevel?(value),
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
			return (num3 <= offerDurationRange.max) ? (60000 * randomInRange) : (-1);
		}
	}
}
