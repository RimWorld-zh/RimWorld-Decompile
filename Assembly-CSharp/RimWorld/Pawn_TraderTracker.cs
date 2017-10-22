using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class Pawn_TraderTracker : IExposable
	{
		private Pawn pawn;

		public TraderKindDef traderKind;

		private List<Pawn> soldPrisoners = new List<Pawn>();

		public IEnumerable<Thing> Goods
		{
			get
			{
				Lord lord = this.pawn.GetLord();
				if (lord == null || !(lord.LordJob is LordJob_TradeWithColony))
				{
					for (int k = 0; k < this.pawn.inventory.innerContainer.Count; k++)
					{
						Thing t = this.pawn.inventory.innerContainer[k];
						if (!this.pawn.inventory.NotForSale(t))
						{
							yield return t;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				if (lord == null)
					yield break;
				int j = 0;
				Pawn p;
				while (true)
				{
					if (j < lord.ownedPawns.Count)
					{
						p = lord.ownedPawns[j];
						switch (p.GetTraderCaravanRole())
						{
						case TraderCaravanRole.Carrier:
						{
							int i = 0;
							if (i < p.inventory.innerContainer.Count)
							{
								yield return p.inventory.innerContainer[i];
								/*Error: Unable to find new state assignment for yield return*/;
							}
							break;
						}
						case TraderCaravanRole.Chattel:
						{
							if (!this.soldPrisoners.Contains(p))
								goto end_IL_023d;
							break;
						}
						}
						j++;
						continue;
					}
					yield break;
					continue;
					end_IL_023d:
					break;
				}
				yield return (Thing)p;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.pawn.thingIDNumber, 1149275593);
			}
		}

		public string TraderName
		{
			get
			{
				return this.pawn.LabelShort;
			}
		}

		public bool CanTradeNow
		{
			get
			{
				return !this.pawn.Dead && this.pawn.Spawned && this.pawn.mindState.wantsToTradeWithColony && this.pawn.CanCasuallyInteractNow(false) && !this.pawn.Downed && !this.pawn.IsPrisoner && this.pawn.Faction != Faction.OfPlayer && (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer)) && this.Goods.Any((Func<Thing, bool>)((Thing x) => this.traderKind.WillTrade(x.def)));
			}
		}

		public Pawn_TraderTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Predicate<Pawn>)((Pawn x) => x == null));
			}
		}

		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> items = from x in this.pawn.Map.listerThings.AllThings
			where TradeUtility.EverTradeable(x.def) && x.def.category == ThingCategory.Item && !x.Position.Fogged(x.Map) && (((Area)((_003CColonyThingsWillingToBuy_003Ec__Iterator1)/*Error near IL_0043: stateMachine*/)._0024this.pawn.Map.areaManager.Home)[x.Position] || x.IsInAnyStorage()) && TradeUtility.TradeableNow(x) && ((_003CColonyThingsWillingToBuy_003Ec__Iterator1)/*Error near IL_0043: stateMachine*/)._0024this.ReachableForTrade(x)
			select x;
			using (IEnumerator<Thing> enumerator = items.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing t = enumerator.Current;
					yield return t;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.pawn.GetLord() != null)
			{
				using (IEnumerator<Pawn> enumerator2 = (from x in TradeUtility.AllSellableColonyPawns(this.pawn.Map)
				where !x.Downed && ((_003CColonyThingsWillingToBuy_003Ec__Iterator1)/*Error near IL_0123: stateMachine*/)._0024this.ReachableForTrade(x)
				select x).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						Pawn p = enumerator2.Current;
						yield return (Thing)p;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_01c1:
			/*Error near IL_01c2: Unexpected return in MoveNext()*/;
		}

		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.");
			}
			else
			{
				Pawn pawn = toGive as Pawn;
				if (pawn != null)
				{
					pawn.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
					this.AddPawnToStock(pawn);
				}
				else
				{
					Thing thing = toGive.SplitOff(countToGive);
					thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
					Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this.pawn, thing);
					if (thing2 != null)
					{
						if (!thing2.TryAbsorbStack(thing, false))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
					else
					{
						this.AddThingToRandomInventory(thing);
					}
				}
			}
		}

		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.Undefined);
				}
				if (this.soldPrisoners.Contains(pawn))
				{
					this.soldPrisoners.Remove(pawn);
				}
			}
			else
			{
				IntVec3 positionHeld = toGive.PositionHeld;
				Map mapHeld = toGive.MapHeld;
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null))
				{
					Lord lord2 = this.pawn.GetLord();
					if (lord2 != null)
					{
						lord2.extraForbiddenThings.Add(thing);
					}
				}
				else
				{
					Log.Error("Could not place bought thing " + thing + " at " + positionHeld);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		private void AddPawnToStock(Pawn newPawn)
		{
			if (!newPawn.Spawned)
			{
				GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map);
			}
			if (newPawn.Faction != this.pawn.Faction)
			{
				newPawn.SetFaction(this.pawn.Faction, null);
			}
			if (newPawn.RaceProps.Humanlike)
			{
				newPawn.kindDef = PawnKindDefOf.Slave;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				newPawn.Destroy(DestroyMode.Vanish);
				Log.Error("Tried to sell pawn " + newPawn + " to " + this.pawn + ", but " + this.pawn + " has no lord. Traders without lord can't buy pawns.");
			}
			else
			{
				if (newPawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(newPawn);
				}
				lord.AddPawn(newPawn);
			}
		}

		private void AddThingToRandomInventory(Thing thing)
		{
			Lord lord = this.pawn.GetLord();
			IEnumerable<Pawn> source = Enumerable.Empty<Pawn>();
			if (lord != null)
			{
				source = from x in lord.ownedPawns
				where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
				select x;
			}
			if (source.Any())
			{
				if (!source.RandomElement().inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			else if (!this.pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		private bool ReachableForTrade(Thing thing)
		{
			return this.pawn.Map == thing.Map && this.pawn.Map.reachability.CanReach(this.pawn.Position, thing, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
		}
	}
}
