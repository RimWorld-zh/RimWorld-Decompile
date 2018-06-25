using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200051B RID: 1307
	public class Pawn_TraderTracker : IExposable
	{
		// Token: 0x04000DFF RID: 3583
		private Pawn pawn;

		// Token: 0x04000E00 RID: 3584
		public TraderKindDef traderKind;

		// Token: 0x04000E01 RID: 3585
		private List<Pawn> soldPrisoners = new List<Pawn>();

		// Token: 0x060017BB RID: 6075 RVA: 0x000CF631 File Offset: 0x000CDA31
		public Pawn_TraderTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x000CF64C File Offset: 0x000CDA4C
		public IEnumerable<Thing> Goods
		{
			get
			{
				Lord lord = this.pawn.GetLord();
				if (lord == null || !(lord.LordJob is LordJob_TradeWithColony))
				{
					for (int i = 0; i < this.pawn.inventory.innerContainer.Count; i++)
					{
						Thing t = this.pawn.inventory.innerContainer[i];
						if (!this.pawn.inventory.NotForSale(t))
						{
							yield return t;
						}
					}
				}
				if (lord != null)
				{
					for (int j = 0; j < lord.ownedPawns.Count; j++)
					{
						Pawn p = lord.ownedPawns[j];
						TraderCaravanRole role = p.GetTraderCaravanRole();
						if (role == TraderCaravanRole.Carrier)
						{
							for (int k = 0; k < p.inventory.innerContainer.Count; k++)
							{
								yield return p.inventory.innerContainer[k];
							}
						}
						else if (role == TraderCaravanRole.Chattel)
						{
							if (!this.soldPrisoners.Contains(p))
							{
								yield return p;
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x000CF678 File Offset: 0x000CDA78
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.pawn.thingIDNumber, 1149275593);
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x000CF6A4 File Offset: 0x000CDAA4
		public string TraderName
		{
			get
			{
				return this.pawn.LabelShort;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x000CF6C4 File Offset: 0x000CDAC4
		public bool CanTradeNow
		{
			get
			{
				return !this.pawn.Dead && this.pawn.Spawned && this.pawn.mindState.wantsToTradeWithColony && this.pawn.CanCasuallyInteractNow(false) && !this.pawn.Downed && !this.pawn.IsPrisoner && this.pawn.Faction != Faction.OfPlayer && (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer)) && this.Goods.Any((Thing x) => this.traderKind.WillTrade(x.def));
			}
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000CF798 File Offset: 0x000CDB98
		public void ExposeData()
		{
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000CF804 File Offset: 0x000CDC04
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> items = from x in this.pawn.Map.listerThings.AllThings
			where x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x) && !x.Position.Fogged(x.Map) && (this.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && this.ReachableForTrade(x)
			select x;
			foreach (Thing t in items)
			{
				yield return t;
			}
			bool hasLord = this.pawn.GetLord() != null;
			if (hasLord)
			{
				foreach (Pawn p in from x in TradeUtility.AllSellableColonyPawns(this.pawn.Map)
				where !x.Downed && this.ReachableForTrade(x)
				select x)
				{
					yield return p;
				}
			}
			yield break;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x000CF830 File Offset: 0x000CDC30
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.", false);
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

		// Token: 0x060017C3 RID: 6083 RVA: 0x000CF8E4 File Offset: 0x000CDCE4
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
				if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null, null))
				{
					Lord lord2 = this.pawn.GetLord();
					if (lord2 != null)
					{
						lord2.extraForbiddenThings.Add(thing);
					}
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not place bought thing ",
						thing,
						" at ",
						positionHeld
					}), false);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x000CF9E4 File Offset: 0x000CDDE4
		private void AddPawnToStock(Pawn newPawn)
		{
			if (!newPawn.Spawned)
			{
				GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map, WipeMode.Vanish);
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
				Log.Error(string.Concat(new object[]
				{
					"Tried to sell pawn ",
					newPawn,
					" to ",
					this.pawn,
					", but ",
					this.pawn,
					" has no lord. Traders without lord can't buy pawns."
				}), false);
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

		// Token: 0x060017C5 RID: 6085 RVA: 0x000CFAEC File Offset: 0x000CDEEC
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
			if (source.Any<Pawn>())
			{
				if (!source.RandomElement<Pawn>().inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			else if (!this.pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x000CFB98 File Offset: 0x000CDF98
		private bool ReachableForTrade(Thing thing)
		{
			return this.pawn.Map == thing.Map && this.pawn.Map.reachability.CanReach(this.pawn.Position, thing, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
		}
	}
}
