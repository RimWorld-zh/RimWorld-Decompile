using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld.Planet
{
	public class Settlement : MapParent, ITrader
	{
		public Settlement_TraderTracker trader;

		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		public virtual bool Abandonable
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer));
			}
		}

		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		public TraderKindDef TraderKind
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.TraderKind;
			}
		}

		public IEnumerable<Thing> Goods
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.StockListForReading;
			}
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				if (this.trader == null)
				{
					return 0;
				}
				return this.trader.RandomPriceFactorSeed;
			}
		}

		public string TraderName
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.TraderName;
			}
		}

		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				if (this.trader == null)
				{
					return 0f;
				}
				return this.trader.TradePriceImprovementOffsetForPlayer;
			}
		}

		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			if (this.trader == null)
			{
				return null;
			}
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		public void AddToStock(Thing thing, Pawn playerNegotiator)
		{
			this.trader.AddToStock(thing, playerNegotiator);
		}

		public void GiveSoldThingToPlayer(Thing toGive, Thing originalThingFromStock, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, originalThingFromStock, playerNegotiator);
		}

		public bool IsPawnPurchasedAsPrisoner(Pawn pawn)
		{
			return this.trader.IsPawnPurchasedAsPrisoner(pawn);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.SettlementPostLoadInit(this);
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
		}

		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int i = this.previouslyGeneratedInhabitants.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[i];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(i);
				}
			}
		}

		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Settlement.<GetGizmos>c__Iterator104 <GetGizmos>c__Iterator = new Settlement.<GetGizmos>c__Iterator104();
			<GetGizmos>c__Iterator.<>f__this = this;
			Settlement.<GetGizmos>c__Iterator104 expr_0E = <GetGizmos>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			Settlement.<GetFloatMenuOptions>c__Iterator105 <GetFloatMenuOptions>c__Iterator = new Settlement.<GetFloatMenuOptions>c__Iterator105();
			<GetFloatMenuOptions>c__Iterator.caravan = caravan;
			<GetFloatMenuOptions>c__Iterator.<$>caravan = caravan;
			<GetFloatMenuOptions>c__Iterator.<>f__this = this;
			Settlement.<GetFloatMenuOptions>c__Iterator105 expr_1C = <GetFloatMenuOptions>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		virtual Faction get_Faction()
		{
			return base.Faction;
		}
	}
}
