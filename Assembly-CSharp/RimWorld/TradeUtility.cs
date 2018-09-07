using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class TradeUtility
	{
		public const float MinimumBuyPrice = 0.5f;

		public const float MinimumSellPrice = 0.01f;

		public const float PriceFactorBuy_Global = 1.4f;

		public const float PriceFactorSell_Global = 0.6f;

		[CompilerGenerated]
		private static Func<Map, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Map, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Thing, int> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<Thing, int> <>f__am$cache5;

		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		public static bool PlayerSellableNow(Thing t)
		{
			t = t.GetInnerIfMinified();
			if (!TradeUtility.EverPlayerSellable(t.def))
			{
				return false;
			}
			if (t.IsNotFresh())
			{
				return false;
			}
			Apparel apparel = t as Apparel;
			return apparel == null || !apparel.WornByCorpse;
		}

		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveDropPodInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			});
		}

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

		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			if (thing is Pawn)
			{
				return null;
			}
			foreach (Thing thing2 in trader.Goods)
			{
				if (TransferableUtility.TransferAsOne(thing2, thing, TransferAsOneMode.Normal) && thing2.CanStackWith(thing) && thing2.def.stackLimit != 1)
				{
					return thing2;
				}
			}
			return null;
		}

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
								goto IL_CC;
							}
						}
					}
				}
				IL_CC:
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

		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
			}
		}

		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true);
			float num = thing.MarketValue * 0.6f * priceFactorSell_TraderPriceType * statValue * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_FactionBase;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.4f * priceFactorBuy_TraderPriceType * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		[CompilerGenerated]
		private static bool <PlayerHomeMapWithMostLaunchableSilver>m__0(Map x)
		{
			return x.IsPlayerHome;
		}

		[CompilerGenerated]
		private static int <PlayerHomeMapWithMostLaunchableSilver>m__1(Map x)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(x)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount);
		}

		[CompilerGenerated]
		private static bool <ColonyHasEnoughSilver>m__2(Thing t)
		{
			return t.def == ThingDefOf.Silver;
		}

		[CompilerGenerated]
		private static int <ColonyHasEnoughSilver>m__3(Thing t)
		{
			return t.stackCount;
		}

		[CompilerGenerated]
		private static bool <PlayerHomeMapWithMostLaunchableSilver>m__4(Thing t)
		{
			return t.def == ThingDefOf.Silver;
		}

		[CompilerGenerated]
		private static int <PlayerHomeMapWithMostLaunchableSilver>m__5(Thing t)
		{
			return t.stackCount;
		}

		[CompilerGenerated]
		private sealed class <AllLaunchableThingsForTrade>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal HashSet<Thing> <yieldedThings>__0;

			internal Map map;

			internal IEnumerator<Building_OrbitalTradeBeacon> $locvar0;

			internal Building_OrbitalTradeBeacon <beacon>__1;

			internal IEnumerator<IntVec3> $locvar1;

			internal IntVec3 <c>__2;

			internal List<Thing> <thingList>__3;

			internal int <i>__4;

			internal Thing <t>__5;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllLaunchableThingsForTrade>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					yieldedThings = new HashSet<Thing>();
					enumerator = Building_OrbitalTradeBeacon.AllPowered(map).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						Block_4:
						try
						{
							switch (num)
							{
							case 1u:
								IL_14E:
								i++;
								break;
							default:
								goto IL_172;
							}
							IL_15C:
							if (i < thingList.Count)
							{
								t = thingList[i];
								if (t.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(t) && !yieldedThings.Contains(t))
								{
									yieldedThings.Add(t);
									this.$current = t;
									if (!this.$disposing)
									{
										this.$PC = 1;
									}
									flag = true;
									return true;
								}
								goto IL_14E;
							}
							IL_172:
							if (enumerator2.MoveNext())
							{
								c = enumerator2.Current;
								thingList = c.GetThingList(map);
								i = 0;
								goto IL_15C;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						beacon = enumerator.Current;
						enumerator2 = beacon.TradeableCells.GetEnumerator();
						num = 4294967293u;
						goto Block_4;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TradeUtility.<AllLaunchableThingsForTrade>c__Iterator0 <AllLaunchableThingsForTrade>c__Iterator = new TradeUtility.<AllLaunchableThingsForTrade>c__Iterator0();
				<AllLaunchableThingsForTrade>c__Iterator.map = map;
				return <AllLaunchableThingsForTrade>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <AllSellableColonyPawns>c__Iterator1 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal Map map;

			internal List<Pawn>.Enumerator $locvar0;

			internal Pawn <p>__1;

			internal List<Pawn>.Enumerator $locvar1;

			internal Pawn <p>__2;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllSellableColonyPawns>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = map.mapPawns.PrisonersOfColonySpawned.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_EC;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.guest.PrisonerIsSecure)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				enumerator2 = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_EC:
					switch (num)
					{
					}
					while (enumerator2.MoveNext())
					{
						p2 = enumerator2.Current;
						if (p2.RaceProps.Animal && p2.HostFaction == null && !p2.InMentalState && !p2.Downed && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(p2.def))
						{
							this.$current = p2;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TradeUtility.<AllSellableColonyPawns>c__Iterator1 <AllSellableColonyPawns>c__Iterator = new TradeUtility.<AllSellableColonyPawns>c__Iterator1();
				<AllSellableColonyPawns>c__Iterator.map = map;
				return <AllSellableColonyPawns>c__Iterator;
			}
		}
	}
}
