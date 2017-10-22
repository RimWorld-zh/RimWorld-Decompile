using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Tradeable : Transferable
	{
		public List<Thing> thingsColony = new List<Thing>();

		public List<Thing> thingsTrader = new List<Thing>();

		public string editBuffer = (string)null;

		private const float MinimumBuyPrice = 0.5f;

		private const float MinimumSellPrice = 0.01f;

		public const float PriceFactorBuy_Global = 1.5f;

		public const float PriceFactorSell_Global = 0.5f;

		private float pricePlayerBuy = -1f;

		private float pricePlayerSell = -1f;

		private float priceFactorBuy_TraderPriceType;

		private float priceFactorSell_TraderPriceType;

		private float priceFactorSell_ItemSellPriceFactor;

		private float priceGain_PlayerNegotiator;

		private float priceGain_FactionBase;

		public Thing FirstThingColony
		{
			get
			{
				return (this.thingsColony.Count != 0) ? this.thingsColony[0] : null;
			}
		}

		public Thing FirstThingTrader
		{
			get
			{
				return (this.thingsTrader.Count != 0) ? this.thingsTrader[0] : null;
			}
		}

		public override string Label
		{
			get
			{
				return this.AnyThing.LabelCapNoCount;
			}
		}

		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency;
			}
		}

		public bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.AnyThing.def);
			}
		}

		public override bool HasAnyThing
		{
			get
			{
				return this.FirstThingColony != null || this.FirstThingTrader != null;
			}
		}

		public override Thing AnyThing
		{
			get
			{
				Thing result;
				if (this.FirstThingColony != null)
				{
					result = this.FirstThingColony.GetInnerIfMinified();
				}
				else if (this.FirstThingTrader != null)
				{
					result = this.FirstThingTrader.GetInnerIfMinified();
				}
				else
				{
					Log.Error(base.GetType() + " lacks AnyThing.");
					result = null;
				}
				return result;
			}
		}

		public override ThingDef ThingDef
		{
			get
			{
				return this.HasAnyThing ? this.AnyThing.def : null;
			}
		}

		public ThingDef StuffDef
		{
			get
			{
				return this.AnyThing.Stuff;
			}
		}

		public override string TipDescription
		{
			get
			{
				return this.HasAnyThing ? this.AnyThing.GetDescription() : "";
			}
		}

		public TradeAction ActionToDo
		{
			get
			{
				return (TradeAction)((base.CountToTransfer != 0) ? ((base.CountToTransfer > 0) ? 1 : 2) : 0);
			}
		}

		public bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Source;
			}
		}

		public float CurTotalSilverCost
		{
			get
			{
				return (float)((this.ActionToDo != 0) ? ((float)base.CountToTransfer * this.GetPriceFor(this.ActionToDo)) : 0.0);
			}
		}

		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef);
			}
		}

		private bool Bugged
		{
			get
			{
				bool result;
				if (!this.HasAnyThing)
				{
					Log.ErrorOnce(this.ToString() + " is bugged. There will be no more logs about this.", 162112);
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		public Tradeable()
		{
		}

		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		public void AddThing(Thing t, Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				this.thingsColony.Add(t);
			}
			if (trans == Transactor.Trader)
			{
				this.thingsTrader.Add(t);
			}
		}

		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		private void InitPriceDataIfNeeded()
		{
			if (!(this.pricePlayerBuy > 0.0))
			{
				this.priceFactorBuy_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerBuys).PriceMultiplier();
				this.priceFactorSell_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerSells).PriceMultiplier();
				this.priceGain_PlayerNegotiator = TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true);
				this.priceGain_FactionBase = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
				this.pricePlayerBuy = (float)(this.BaseMarketValue * 1.5 * this.priceFactorBuy_TraderPriceType * (1.0 + Find.Storyteller.difficulty.tradePriceFactorLoss));
				this.pricePlayerBuy *= (float)(1.0 - this.priceGain_PlayerNegotiator - this.priceGain_FactionBase);
				this.pricePlayerBuy = Mathf.Max(this.pricePlayerBuy, 0.5f);
				if (this.pricePlayerBuy > 99.5)
				{
					this.pricePlayerBuy = Mathf.Round(this.pricePlayerBuy);
				}
				this.priceFactorSell_ItemSellPriceFactor = this.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true);
				this.pricePlayerSell = (float)(this.BaseMarketValue * 0.5 * this.priceFactorSell_TraderPriceType * this.priceFactorSell_ItemSellPriceFactor * (1.0 - Find.Storyteller.difficulty.tradePriceFactorLoss));
				this.pricePlayerSell *= (float)(1.0 + this.priceGain_PlayerNegotiator + this.priceGain_FactionBase);
				this.pricePlayerSell = Mathf.Max(this.pricePlayerSell, 0.01f);
				if (this.pricePlayerSell > 99.5)
				{
					this.pricePlayerSell = Mathf.Round(this.pricePlayerSell);
				}
				if (this.pricePlayerSell >= this.pricePlayerBuy)
				{
					Log.ErrorOnce("Trying to put player-sells price above player-buys price for " + this.AnyThing, 65387);
					this.pricePlayerSell = this.pricePlayerBuy;
				}
			}
		}

		public string GetPriceTooltip(TradeAction action)
		{
			string result;
			if (!this.HasAnyThing)
			{
				result = "";
				goto IL_04cd;
			}
			this.InitPriceDataIfNeeded();
			string str = (action != TradeAction.PlayerBuys) ? "SellPriceDesc".Translate() : "BuyPriceDesc".Translate();
			string text;
			str = (text = str + "\n\n");
			str = text + StatDefOf.MarketValue.LabelCap + ": " + this.BaseMarketValue;
			if (action == TradeAction.PlayerBuys)
			{
				text = str;
				str = text + "\n  x " + 1.5f.ToString("F2") + " (" + "Buying".Translate() + ")";
				if (this.priceFactorBuy_TraderPriceType != 1.0)
				{
					text = str;
					str = text + "\n  x " + this.priceFactorBuy_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0.0)
				{
					text = str;
					str = text + "\n  x " + ((float)(1.0 + Find.Storyteller.difficulty.tradePriceFactorLoss)).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				str = (text = str + "\n");
				str = text + "\n" + "YourNegotiatorBonus".Translate() + ": -" + this.priceGain_PlayerNegotiator.ToStringPercent();
				if (this.priceGain_FactionBase != 0.0)
				{
					text = str;
					str = text + "\n" + "TradeWithFactionBaseBonus".Translate() + ": -" + this.priceGain_FactionBase.ToStringPercent();
				}
			}
			else
			{
				text = str;
				str = text + "\n  x " + 0.5f.ToString("F2") + " (" + "Selling".Translate() + ")";
				if (this.priceFactorSell_TraderPriceType != 1.0)
				{
					text = str;
					str = text + "\n  x " + this.priceFactorSell_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (this.priceFactorSell_ItemSellPriceFactor != 1.0)
				{
					text = str;
					str = text + "\n  x " + this.priceFactorSell_ItemSellPriceFactor.ToString("F2") + " (" + "ItemSellPriceFactor".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0.0)
				{
					text = str;
					str = text + "\n  x " + ((float)(1.0 - Find.Storyteller.difficulty.tradePriceFactorLoss)).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				str = (text = str + "\n");
				str = text + "\n" + "YourNegotiatorBonus".Translate() + ": " + this.priceGain_PlayerNegotiator.ToStringPercent();
				if (this.priceGain_FactionBase != 0.0)
				{
					text = str;
					str = text + "\n" + "TradeWithFactionBaseBonus".Translate() + ": " + this.priceGain_FactionBase.ToStringPercent();
				}
			}
			str += "\n\n";
			float priceFor = this.GetPriceFor(action);
			str = str + "FinalPrice".Translate() + ": $" + priceFor.ToString("F2");
			if (action == TradeAction.PlayerBuys && priceFor <= 0.5)
			{
				goto IL_04ab;
			}
			if (action == TradeAction.PlayerBuys && priceFor <= 0.0099999997764825821)
				goto IL_04ab;
			goto IL_04c6;
			IL_04cd:
			return result;
			IL_04c6:
			result = str;
			goto IL_04cd;
			IL_04ab:
			str = str + " (" + "minimum".Translate() + ")";
			goto IL_04c6;
		}

		public float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			return (action != TradeAction.PlayerBuys) ? this.pricePlayerSell : this.pricePlayerBuy;
		}

		public override int GetMinimum()
		{
			return -this.CountHeldBy(Transactor.Colony);
		}

		public override int GetMaximum()
		{
			return this.CountHeldBy(Transactor.Trader);
		}

		public override AcceptanceReport UnderflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("TraderHasNoMore".Translate());
		}

		private List<Thing> TransactorThings(Transactor trans)
		{
			return (trans != 0) ? this.thingsTrader : this.thingsColony;
		}

		public int CountHeldBy(Transactor trans)
		{
			List<Thing> list = this.TransactorThings(trans);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].stackCount;
			}
			return num;
		}

		public int CountPostDealFor(Transactor trans)
		{
			return (trans != 0) ? (this.CountHeldBy(trans) - base.CountToTransfer) : (this.CountHeldBy(trans) + base.CountToTransfer);
		}

		public virtual void ResolveTrade()
		{
			if (this.ActionToDo == TradeAction.PlayerSells)
			{
				TransferableUtility.TransferNoSplit(this.thingsColony, -base.CountToTransfer, (Action<Thing, int>)delegate(Thing thing, int countToTransfer)
				{
					TradeSession.trader.GiveSoldThingToTrader(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
			}
			else if (this.ActionToDo == TradeAction.PlayerBuys)
			{
				TransferableUtility.TransferNoSplit(this.thingsTrader, base.CountToTransfer, (Action<Thing, int>)delegate(Thing thing, int countToTransfer)
				{
					this.CheckTeachOpportunity(thing, countToTransfer);
					TradeSession.trader.GiveSoldThingToPlayer(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
			}
		}

		private void CheckTeachOpportunity(Thing boughtThing, int boughtCount)
		{
			Building building = boughtThing as Building;
			if (building == null)
			{
				MinifiedThing minifiedThing = boughtThing as MinifiedThing;
				if (minifiedThing != null)
				{
					building = (minifiedThing.InnerThing as Building);
				}
			}
			if (building != null && building.def.building != null && building.def.building.boughtConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(building.def.building.boughtConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		public override string ToString()
		{
			return base.GetType() + "(" + this.ThingDef + ", countToDrop=" + base.CountToTransfer + ")";
		}

		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}
	}
}
