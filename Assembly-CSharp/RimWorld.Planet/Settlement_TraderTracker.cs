using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld.Planet
{
	public abstract class Settlement_TraderTracker : IExposable, IThingHolder
	{
		private const float DefaultTradePriceImprovement = 0.02f;

		public Settlement settlement;

		private ThingOwner<Thing> stock;

		private int lastStockGenerationTicks = -1;

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
				if (this.settlement.Faction == null)
				{
					return this.settlement.LabelCap;
				}
				return "SettlementTrader".Translate(new object[]
				{
					this.settlement.LabelCap,
					this.settlement.Faction.Name
				});
			}
		}

		public virtual bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && (this.stock == null || this.stock.InnerListForReading.Any((Thing x) => this.TraderKind.WillTrade(x.def)));
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
					for (int i = this.stock.Count - 1; i >= 0; i--)
					{
						Pawn pawn = this.stock[i] as Pawn;
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
			if (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving)
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.stock.TryAdd(this.tmpSavedPawns[j], false);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		[DebuggerHidden]
		public virtual IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Settlement_TraderTracker.<ColonyThingsWillingToBuy>c__Iterator106 <ColonyThingsWillingToBuy>c__Iterator = new Settlement_TraderTracker.<ColonyThingsWillingToBuy>c__Iterator106();
			<ColonyThingsWillingToBuy>c__Iterator.playerNegotiator = playerNegotiator;
			<ColonyThingsWillingToBuy>c__Iterator.<$>playerNegotiator = playerNegotiator;
			Settlement_TraderTracker.<ColonyThingsWillingToBuy>c__Iterator106 expr_15 = <ColonyThingsWillingToBuy>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public virtual void AddToStock(Thing thing, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				caravan.RemovePawn(pawn);
				if (pawn.RaceProps.Humanlike)
				{
					Find.WorldPawns.DiscardIfUnimportant(pawn);
					return;
				}
			}
			else
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				if (ownerOf != null)
				{
					ownerOf.inventory.innerContainer.Remove(thing);
				}
			}
			this.stock.TryAdd(thing, false);
		}

		public virtual void GiveSoldThingToPlayer(Thing toGive, Thing originalThingFromStock, Pawn playerNegotiator)
		{
			if (toGive == originalThingFromStock)
			{
				this.stock.Remove(originalThingFromStock);
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				caravan.AddPawn(pawn, true);
			}
			else
			{
				Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(toGive, caravan.PawnsListForReading, null, null);
				if (pawn2 != null)
				{
					pawn2.inventory.innerContainer.TryAdd(toGive, true);
				}
				else
				{
					Log.Error("Could not find any pawn to give sold thing to.");
				}
			}
		}

		public bool IsPawnPurchasedAsPrisoner(Pawn pawn)
		{
			return false;
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
					for (int i = this.stock.Count - 1; i >= 0; i--)
					{
						Pawn pawn = this.stock[i] as Pawn;
						if (pawn != null && pawn.Destroyed)
						{
							this.stock.Remove(pawn);
							Find.WorldPawns.DiscardIfUnimportant(pawn);
						}
					}
					for (int j = this.stock.Count - 1; j >= 0; j--)
					{
						Pawn pawn2 = this.stock[j] as Pawn;
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
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Thing thing = this.stock[i];
					this.stock.Remove(thing);
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						Find.WorldPawns.DiscardIfUnimportant(pawn);
					}
					else if (!thing.Destroyed)
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
				ItemCollectionGeneratorParams parms = default(ItemCollectionGeneratorParams);
				parms.traderDef = this.TraderKind;
				parms.forTile = this.settlement.Tile;
				parms.forFaction = this.settlement.Faction;
				this.stock.TryAddRange(ItemCollectionGeneratorDefOf.TraderStock.Worker.Generate(parms));
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
