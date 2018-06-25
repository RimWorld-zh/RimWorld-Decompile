using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Tradeable : Transferable
	{
		public List<Thing> thingsColony = new List<Thing>();

		public List<Thing> thingsTrader = new List<Thing>();

		private int countToTransfer;

		private float pricePlayerBuy = -1f;

		private float pricePlayerSell = -1f;

		private float priceFactorBuy_TraderPriceType;

		private float priceFactorSell_TraderPriceType;

		private float priceFactorSell_ItemSellPriceFactor;

		private float priceGain_PlayerNegotiator;

		private float priceGain_FactionBase;

		[CompilerGenerated]
		private static Action<Thing, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache4;

		public Tradeable()
		{
		}

		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		public Thing FirstThingColony
		{
			get
			{
				Thing result;
				if (this.thingsColony.Count == 0)
				{
					result = null;
				}
				else
				{
					result = this.thingsColony[0];
				}
				return result;
			}
		}

		public Thing FirstThingTrader
		{
			get
			{
				Thing result;
				if (this.thingsTrader.Count == 0)
				{
					result = null;
				}
				else
				{
					result = this.thingsTrader[0];
				}
				return result;
			}
		}

		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
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
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		public bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
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
					Log.Error(base.GetType() + " lacks AnyThing.", false);
					result = null;
				}
				return result;
			}
		}

		public override ThingDef ThingDef
		{
			get
			{
				ThingDef result;
				if (!this.HasAnyThing)
				{
					result = null;
				}
				else
				{
					result = this.AnyThing.def;
				}
				return result;
			}
		}

		public ThingDef StuffDef
		{
			get
			{
				ThingDef result;
				if (!this.HasAnyThing)
				{
					result = null;
				}
				else
				{
					result = this.AnyThing.Stuff;
				}
				return result;
			}
		}

		public override string TipDescription
		{
			get
			{
				string result;
				if (!this.HasAnyThing)
				{
					result = "";
				}
				else
				{
					result = this.AnyThing.DescriptionDetailed;
				}
				return result;
			}
		}

		public TradeAction ActionToDo
		{
			get
			{
				TradeAction result;
				if (this.CountToTransfer == 0)
				{
					result = TradeAction.None;
				}
				else if (base.CountToTransferToDestination > 0)
				{
					result = TradeAction.PlayerSells;
				}
				else
				{
					result = TradeAction.PlayerBuys;
				}
				return result;
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
				TransferablePositiveCountDirection result;
				if (TradeSession.Active && TradeSession.giftMode)
				{
					result = TransferablePositiveCountDirection.Destination;
				}
				else
				{
					result = TransferablePositiveCountDirection.Source;
				}
				return result;
			}
		}

		public float CurTotalSilverCostForSource
		{
			get
			{
				float result;
				if (this.ActionToDo == TradeAction.None)
				{
					result = 0f;
				}
				else
				{
					result = (float)base.CountToTransferToSource * this.GetPriceFor(this.ActionToDo);
				}
				return result;
			}
		}

		public float CurTotalSilverCostForDestination
		{
			get
			{
				float result;
				if (this.ActionToDo == TradeAction.None)
				{
					result = 0f;
				}
				else
				{
					result = (float)base.CountToTransferToDestination * this.GetPriceFor(this.ActionToDo);
				}
				return result;
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
					Log.ErrorOnce(this.ToString() + " is bugged. There will be no more logs about this.", 162112, false);
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
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
			if (this.pricePlayerBuy <= 0f)
			{
				if (this.IsCurrency)
				{
					this.pricePlayerBuy = this.BaseMarketValue;
					this.pricePlayerSell = this.BaseMarketValue;
				}
				else
				{
					this.priceFactorBuy_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerBuys).PriceMultiplier();
					this.priceFactorSell_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerSells).PriceMultiplier();
					this.priceGain_PlayerNegotiator = TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true);
					this.priceGain_FactionBase = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
					this.priceFactorSell_ItemSellPriceFactor = this.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true);
					this.pricePlayerBuy = TradeUtility.GetPricePlayerBuy(this.AnyThing, this.priceFactorBuy_TraderPriceType, this.priceGain_PlayerNegotiator, this.priceGain_FactionBase);
					this.pricePlayerSell = TradeUtility.GetPricePlayerSell(this.AnyThing, this.priceFactorSell_TraderPriceType, this.priceGain_PlayerNegotiator, this.priceGain_FactionBase);
					if (this.pricePlayerSell >= this.pricePlayerBuy)
					{
						Log.ErrorOnce("Trying to put player-sells price above player-buys price for " + this.AnyThing, 65387, false);
						this.pricePlayerSell = this.pricePlayerBuy;
					}
				}
			}
		}

		public string GetPriceTooltip(TradeAction action)
		{
			string result;
			if (!this.HasAnyThing)
			{
				result = "";
			}
			else
			{
				this.InitPriceDataIfNeeded();
				string text = (action != TradeAction.PlayerBuys) ? "SellPriceDesc".Translate() : "BuyPriceDesc".Translate();
				text += "\n\n";
				text = text + StatDefOf.MarketValue.LabelCap + ": " + this.BaseMarketValue.ToStringMoney("F2");
				if (action == TradeAction.PlayerBuys)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n  x ",
						1.5f.ToString("F2"),
						" (",
						"Buying".Translate(),
						")"
					});
					if (this.priceFactorBuy_TraderPriceType != 1f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n  x ",
							this.priceFactorBuy_TraderPriceType.ToString("F2"),
							" (",
							"TraderTypePrice".Translate(),
							")"
						});
					}
					if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n  x ",
							(1f + Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2"),
							" (",
							"DifficultyLevel".Translate(),
							")"
						});
					}
					text += "\n";
					text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n",
						"YourNegotiatorBonus".Translate(),
						": -",
						this.priceGain_PlayerNegotiator.ToStringPercent()
					});
					if (this.priceGain_FactionBase != 0f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n",
							"TradeWithFactionBaseBonus".Translate(),
							": -",
							this.priceGain_FactionBase.ToStringPercent()
						});
					}
				}
				else
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n  x ",
						0.5f.ToString("F2"),
						" (",
						"Selling".Translate(),
						")"
					});
					if (this.priceFactorSell_TraderPriceType != 1f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n  x ",
							this.priceFactorSell_TraderPriceType.ToString("F2"),
							" (",
							"TraderTypePrice".Translate(),
							")"
						});
					}
					if (this.priceFactorSell_ItemSellPriceFactor != 1f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n  x ",
							this.priceFactorSell_ItemSellPriceFactor.ToString("F2"),
							" (",
							"ItemSellPriceFactor".Translate(),
							")"
						});
					}
					if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n  x ",
							(1f - Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2"),
							" (",
							"DifficultyLevel".Translate(),
							")"
						});
					}
					text += "\n";
					text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n",
						"YourNegotiatorBonus".Translate(),
						": ",
						this.priceGain_PlayerNegotiator.ToStringPercent()
					});
					if (this.priceGain_FactionBase != 0f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n",
							"TradeWithFactionBaseBonus".Translate(),
							": ",
							this.priceGain_FactionBase.ToStringPercent()
						});
					}
				}
				text += "\n\n";
				float priceFor = this.GetPriceFor(action);
				text = text + "FinalPrice".Translate() + ": $" + priceFor.ToString("F2");
				if ((action == TradeAction.PlayerBuys && priceFor <= 0.5f) || (action == TradeAction.PlayerBuys && priceFor <= 0.01f))
				{
					text = text + " (" + "minimum".Translate() + ")";
				}
				result = text;
			}
			return result;
		}

		public float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			float result;
			if (action == TradeAction.PlayerBuys)
			{
				result = this.pricePlayerBuy;
			}
			else
			{
				result = this.pricePlayerSell;
			}
			return result;
		}

		public override int GetMinimumToTransfer()
		{
			int result;
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				result = -this.CountHeldBy(Transactor.Trader);
			}
			else
			{
				result = -this.CountHeldBy(Transactor.Colony);
			}
			return result;
		}

		public override int GetMaximumToTransfer()
		{
			int result;
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				result = this.CountHeldBy(Transactor.Colony);
			}
			else
			{
				result = this.CountHeldBy(Transactor.Trader);
			}
			return result;
		}

		public override AcceptanceReport UnderflowReport()
		{
			AcceptanceReport result;
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				result = new AcceptanceReport("TraderHasNoMore".Translate());
			}
			else
			{
				result = new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			return result;
		}

		public override AcceptanceReport OverflowReport()
		{
			AcceptanceReport result;
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				result = new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			else
			{
				result = new AcceptanceReport("TraderHasNoMore".Translate());
			}
			return result;
		}

		private List<Thing> TransactorThings(Transactor trans)
		{
			List<Thing> result;
			if (trans == Transactor.Colony)
			{
				result = this.thingsColony;
			}
			else
			{
				result = this.thingsTrader;
			}
			return result;
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
			int result;
			if (trans == Transactor.Colony)
			{
				result = this.CountHeldBy(trans) + base.CountToTransferToSource;
			}
			else
			{
				result = this.CountHeldBy(trans) + base.CountToTransferToDestination;
			}
			return result;
		}

		public virtual void ResolveTrade()
		{
			if (this.ActionToDo == TradeAction.PlayerSells)
			{
				TransferableUtility.TransferNoSplit(this.thingsColony, base.CountToTransferToDestination, delegate(Thing thing, int countToTransfer)
				{
					TradeSession.trader.GiveSoldThingToTrader(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
			}
			else if (this.ActionToDo == TradeAction.PlayerBuys)
			{
				TransferableUtility.TransferNoSplit(this.thingsTrader, base.CountToTransferToSource, delegate(Thing thing, int countToTransfer)
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
			if (building != null)
			{
				if (building.def.building != null && building.def.building.boughtConceptLearnOpportunity != null)
				{
					LessonAutoActivator.TeachOpportunity(building.def.building.boughtConceptLearnOpportunity, OpportunityType.GoodToKnow);
				}
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType(),
				"(",
				this.ThingDef,
				", countToTransfer=",
				this.CountToTransfer,
				")"
			});
		}

		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.thingsColony.RemoveAll((Thing x) => x.Destroyed);
				this.thingsTrader.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.thingsColony, "thingsColony", LookMode.Reference, new object[0]);
			Scribe_Collections.Look<Thing>(ref this.thingsTrader, "thingsTrader", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsColony.RemoveAll((Thing x) => x == null) == 0)
				{
					if (this.thingsTrader.RemoveAll((Thing x) => x == null) == 0)
					{
						goto IL_133;
					}
				}
				Log.Warning("Some of the things were null after loading.", false);
				IL_133:;
			}
		}

		[CompilerGenerated]
		private static void <ResolveTrade>m__0(Thing thing, int countToTransfer)
		{
			TradeSession.trader.GiveSoldThingToTrader(thing, countToTransfer, TradeSession.playerNegotiator);
		}

		[CompilerGenerated]
		private void <ResolveTrade>m__1(Thing thing, int countToTransfer)
		{
			this.CheckTeachOpportunity(thing, countToTransfer);
			TradeSession.trader.GiveSoldThingToPlayer(thing, countToTransfer, TradeSession.playerNegotiator);
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__2(Thing x)
		{
			return x.Destroyed;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__3(Thing x)
		{
			return x.Destroyed;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__4(Thing x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__5(Thing x)
		{
			return x == null;
		}
	}
}
