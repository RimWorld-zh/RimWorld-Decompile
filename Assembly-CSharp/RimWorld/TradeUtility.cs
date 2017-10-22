using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class TradeUtility
	{
		public static bool EverTradeable(ThingDef def)
		{
			bool result;
			if (def.tradeability == Tradeability.Never)
			{
				result = false;
			}
			else
			{
				if ((def.category == ThingCategory.Item || def.category == ThingCategory.Pawn) && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0.0)
				{
					result = true;
					goto IL_0051;
				}
				result = false;
			}
			goto IL_0051;
			IL_0051:
			return result;
		}

		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
			activeDropPodInfo.SingleContainedThing = t;
			activeDropPodInfo.leaveSlag = false;
			DropPodUtility.MakeDropPodAt(dropSpot, map, activeDropPodInfo, false);
		}

		public static IEnumerable<Thing> AllLaunchableThings(Map map)
		{
			HashSet<Thing> yieldedThings = new HashSet<Thing>();
			foreach (Building_OrbitalTradeBeacon item in Building_OrbitalTradeBeacon.AllPowered(map))
			{
				foreach (IntVec3 tradeableCell in item.TradeableCells)
				{
					List<Thing> thingList = tradeableCell.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing t = thingList[i];
						if (TradeUtility.EverTradeable(t.def) && t.def.category == ThingCategory.Item && !yieldedThings.Contains(t) && TradeUtility.TradeableNow(t))
						{
							yieldedThings.Add(t);
							yield return t;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_01fb:
			/*Error near IL_01fc: Unexpected return in MoveNext()*/;
		}

		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map)
		{
			foreach (Pawn item in map.mapPawns.PrisonersOfColonySpawned)
			{
				if (item.guest.PrisonerIsSecure)
				{
					yield return item;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (List<Pawn>.Enumerator enumerator2 = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).GetEnumerator())
			{
				Pawn p;
				while (true)
				{
					if (enumerator2.MoveNext())
					{
						p = enumerator2.Current;
						if (p.RaceProps.Animal && p.HostFaction == null && !p.InMentalState && !p.Downed && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(p.def))
							break;
						continue;
					}
					yield break;
				}
				yield return p;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_01d0:
			/*Error near IL_01d1: Unexpected return in MoveNext()*/;
		}

		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			Thing result;
			if (thing is Pawn)
			{
				result = null;
			}
			else
			{
				foreach (Thing good in trader.Goods)
				{
					if (TransferableUtility.TransferAsOne(good, thing))
					{
						return good;
					}
				}
				result = null;
			}
			return result;
		}

		public static bool TradeableNow(Thing t)
		{
			return (byte)((!t.IsNotFresh()) ? 1 : 0) != 0;
		}

		public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
		{
			while (true)
			{
				Thing thing;
				if (debt > 0)
				{
					thing = null;
					foreach (Building_OrbitalTradeBeacon item in Building_OrbitalTradeBeacon.AllPowered(map))
					{
						foreach (IntVec3 tradeableCell in item.TradeableCells)
						{
							foreach (Thing item2 in map.thingGrid.ThingsAt(tradeableCell))
							{
								if (item2.def == resDef)
								{
									thing = item2;
									goto IL_00d8;
								}
							}
						}
					}
					goto IL_00d8;
				}
				return;
				IL_00d8:
				if (thing != null)
				{
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
					continue;
				}
				break;
			}
			Log.Error("Could not find any " + resDef + " to transfer to trader.");
		}

		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Func<Map, int>)((Map x) => (from t in TradeUtility.AllLaunchableThings(x)
			where t.def == ThingDefOf.Silver
			select t).Sum((Func<Thing, int>)((Thing t) => t.stackCount))));
		}

		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThings(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Func<Thing, int>)((Thing t) => t.stackCount)) >= fee;
		}

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
	}
}
