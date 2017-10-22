using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public abstract class Settlement_TraderTracker : IThingHolder, IExposable
	{
		public Settlement settlement;

		private ThingOwner<Thing> stock;

		private int lastStockGenerationTicks = -1;

		private const float DefaultTradePriceImprovement = 0.02f;

		private List<Pawn> tmpSavedPawns = new List<Pawn>();

		protected virtual int RegenerateStockEveryDays
		{
			get
			{
				return 30;
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return this.settlement;
			}
		}

		public List<Thing> StockListForReading
		{
			get
			{
				if (this.stock == null)
				{
					this.RegenerateStock();
				}
				return this.stock.InnerListForReading;
			}
		}

		public abstract TraderKindDef TraderKind
		{
			get;
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.settlement.ID, 1933327354);
			}
		}

		public virtual string TraderName
		{
			get
			{
				return (this.settlement.Faction != null) ? "SettlementTrader".Translate(this.settlement.LabelCap, this.settlement.Faction.Name) : this.settlement.LabelCap;
			}
		}

		public virtual bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && (this.stock == null || this.stock.InnerListForReading.Any((Predicate<Thing>)((Thing x) => this.TraderKind.WillTrade(x.def))));
			}
		}

		public virtual float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0.02f;
			}
		}

		public Settlement_TraderTracker(Settlement settlement)
		{
			this.settlement = settlement;
		}

		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpSavedPawns.Clear();
				if (this.stock != null)
				{
					for (int num = this.stock.Count - 1; num >= 0; num--)
					{
						Pawn pawn = this.stock[num] as Pawn;
						if (pawn != null)
						{
							this.stock.Remove(pawn);
							this.tmpSavedPawns.Add(pawn);
						}
					}
				}
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.stock, "stock", new object[0]);
			Scribe_Values.Look<int>(ref this.lastStockGenerationTicks, "lastStockGenerationTicks", 0, false);
			if (Scribe.mode != LoadSaveMode.PostLoadInit && Scribe.mode != LoadSaveMode.Saving)
				return;
			for (int i = 0; i < this.tmpSavedPawns.Count; i++)
			{
				this.stock.TryAdd(this.tmpSavedPawns[i], false);
			}
			this.tmpSavedPawns.Clear();
		}

		public virtual IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			using (List<Thing>.Enumerator enumerator = CaravanInventoryUtility.AllInventoryItems(caravan).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing item = enumerator.Current;
					yield return item;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			List<Pawn> pawns = caravan.PawnsListForReading;
			int i = 0;
			while (true)
			{
				if (i < pawns.Count)
				{
					if (caravan.IsOwner(pawns[i]))
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return (Thing)pawns[i];
			/*Error: Unable to find new state assignment for yield return*/;
			IL_015c:
			/*Error near IL_015d: Unexpected return in MoveNext()*/;
		}

		public virtual void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.settlement);
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				if (!pawn.RaceProps.Humanlike && !this.stock.TryAdd(pawn, false))
				{
					pawn.Destroy(DestroyMode.Vanish);
				}
			}
			else if (!this.stock.TryAdd(thing, false))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		public virtual void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.settlement);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				caravan.AddPawn(pawn, true);
			}
			else
			{
				Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
				if (pawn2 == null)
				{
					Log.Error("Could not find any pawn to give sold thing to.");
					thing.Destroy(DestroyMode.Vanish);
				}
				else if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error("Could not add sold thing to inventory.");
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		public virtual void TraderTrackerTick()
		{
			if (this.stock != null)
			{
				if (Find.TickManager.TicksGame - this.lastStockGenerationTicks > this.RegenerateStockEveryDays * 60000)
				{
					this.TryDestroyStock();
				}
				else
				{
					for (int num = this.stock.Count - 1; num >= 0; num--)
					{
						Pawn pawn = this.stock[num] as Pawn;
						if (pawn != null && pawn.Destroyed)
						{
							this.stock.Remove(pawn);
						}
					}
					for (int num2 = this.stock.Count - 1; num2 >= 0; num2--)
					{
						Pawn pawn2 = this.stock[num2] as Pawn;
						if (pawn2 != null && !pawn2.IsWorldPawn())
						{
							Log.Error("Faction base has non-world-pawns in its stock. Removing...");
							this.stock.Remove(pawn2);
						}
					}
				}
			}
		}

		public void TryDestroyStock()
		{
			if (this.stock != null)
			{
				for (int num = this.stock.Count - 1; num >= 0; num--)
				{
					Thing thing = this.stock[num];
					this.stock.Remove(thing);
					if (!(thing is Pawn) && !thing.Destroyed)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				this.stock = null;
			}
		}

		public bool ContainsPawn(Pawn p)
		{
			return this.stock != null && this.stock.Contains(p);
		}

		protected virtual void RegenerateStock()
		{
			this.TryDestroyStock();
			this.stock = new ThingOwner<Thing>(this);
			if (this.settlement.Faction == null || !this.settlement.Faction.IsPlayer)
			{
				ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
				{
					traderDef = this.TraderKind,
					tile = new int?(this.settlement.Tile),
					traderFaction = this.settlement.Faction
				};
				this.stock.TryAddRangeOrTransfer(ItemCollectionGeneratorDefOf.TraderStock.Worker.Generate(parms), true, false);
			}
			for (int i = 0; i < this.stock.Count; i++)
			{
				Pawn pawn = this.stock[i] as Pawn;
				if (pawn != null)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
			this.lastStockGenerationTicks = Find.TickManager.TicksGame;
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.stock;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
