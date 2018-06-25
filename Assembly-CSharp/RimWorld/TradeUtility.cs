using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000780 RID: 1920
	public static class TradeUtility
	{
		// Token: 0x040016DF RID: 5855
		public const float MinimumBuyPrice = 0.5f;

		// Token: 0x040016E0 RID: 5856
		public const float MinimumSellPrice = 0.01f;

		// Token: 0x040016E1 RID: 5857
		public const float PriceFactorBuy_Global = 1.5f;

		// Token: 0x040016E2 RID: 5858
		public const float PriceFactorSell_Global = 0.5f;

		// Token: 0x06002A6B RID: 10859 RVA: 0x00167934 File Offset: 0x00165D34
		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x001679C8 File Offset: 0x00165DC8
		public static bool PlayerSellableNow(Thing t)
		{
			t = t.GetInnerIfMinified();
			bool result;
			if (!TradeUtility.EverPlayerSellable(t.def))
			{
				result = false;
			}
			else if (t.IsNotFresh())
			{
				result = false;
			}
			else
			{
				Apparel apparel = t as Apparel;
				result = (apparel == null || !apparel.WornByCorpse);
			}
			return result;
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x00167A30 File Offset: 0x00165E30
		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveDropPodInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			}, false);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00167A5C File Offset: 0x00165E5C
		public static IEnumerable<Thing> AllLaunchableThingsForTrade(Map map)
		{
			HashSet<Thing> yieldedThings = new HashSet<Thing>();
			foreach (Building_OrbitalTradeBeacon beacon in Building_OrbitalTradeBeacon.AllPowered(map))
			{
				foreach (IntVec3 c in beacon.TradeableCells)
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing t = thingList[i];
						if (t.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(t) && !yieldedThings.Contains(t))
						{
							yieldedThings.Add(t);
							yield return t;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x00167A88 File Offset: 0x00165E88
		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map)
		{
			foreach (Pawn p in map.mapPawns.PrisonersOfColonySpawned)
			{
				if (p.guest.PrisonerIsSecure)
				{
					yield return p;
				}
			}
			foreach (Pawn p2 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (p2.RaceProps.Animal && p2.HostFaction == null && !p2.InMentalState && !p2.Downed && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(p2.def))
				{
					yield return p2;
				}
			}
			yield break;
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x00167AB4 File Offset: 0x00165EB4
		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			Thing result;
			if (thing is Pawn)
			{
				result = null;
			}
			else
			{
				foreach (Thing thing2 in trader.Goods)
				{
					if (TransferableUtility.TransferAsOne(thing2, thing, TransferAsOneMode.Normal) && thing2.CanStackWith(thing) && thing2.def.stackLimit != 1)
					{
						return thing2;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x00167B58 File Offset: 0x00165F58
		public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
		{
			while (debt > 0)
			{
				Thing thing = null;
				foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
				{
					foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
					{
						foreach (Thing thing2 in map.thingGrid.ThingsAt(c))
						{
							if (thing2.def == resDef)
							{
								thing = thing2;
								goto IL_D8;
							}
						}
					}
				}
				IL_D8:
				if (thing == null)
				{
					Log.Error("Could not find any " + resDef + " to transfer to trader.", false);
					break;
				}
				int num = Math.Min(debt, thing.stackCount);
				if (trader != null)
				{
					trader.GiveSoldThingToTrader(thing, num, TradeSession.playerNegotiator);
				}
				else
				{
					thing.SplitOff(num).Destroy(DestroyMode.Vanish);
				}
				debt -= num;
			}
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x00167CCC File Offset: 0x001660CC
		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x00167CDC File Offset: 0x001660DC
		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x00167D3C File Offset: 0x0016613C
		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x00167DA4 File Offset: 0x001661A4
		public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
		{
			if (!pawn.Dead)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
				}
			}
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x00167DF0 File Offset: 0x001661F0
		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true);
			float num = thing.MarketValue * 0.5f * priceFactorSell_TraderPriceType * statValue * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_FactionBase;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x00167E64 File Offset: 0x00166264
		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.5f * priceFactorBuy_TraderPriceType * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}
	}
}
