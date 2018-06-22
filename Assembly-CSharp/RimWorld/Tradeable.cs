using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000780 RID: 1920
	public class Tradeable : Transferable
	{
		// Token: 0x06002A7B RID: 10875 RVA: 0x001684FA File Offset: 0x001668FA
		public Tradeable()
		{
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x00168530 File Offset: 0x00166930
		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002A7D RID: 10877 RVA: 0x00168588 File Offset: 0x00166988
		// (set) Token: 0x06002A7E RID: 10878 RVA: 0x001685A3 File Offset: 0x001669A3
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

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x001685BC File Offset: 0x001669BC
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

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x001685F4 File Offset: 0x001669F4
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

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x0016862C File Offset: 0x00166A2C
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x0016864C File Offset: 0x00166A4C
		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x0016866C File Offset: 0x00166A6C
		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x001686A4 File Offset: 0x00166AA4
		public bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002A85 RID: 10885 RVA: 0x001686D0 File Offset: 0x00166AD0
		public override bool HasAnyThing
		{
			get
			{
				return this.FirstThingColony != null || this.FirstThingTrader != null;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x00168700 File Offset: 0x00166B00
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

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002A87 RID: 10887 RVA: 0x00168764 File Offset: 0x00166B64
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

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002A88 RID: 10888 RVA: 0x00168798 File Offset: 0x00166B98
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

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x001687CC File Offset: 0x00166BCC
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

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002A8A RID: 10890 RVA: 0x00168804 File Offset: 0x00166C04
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

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002A8B RID: 10891 RVA: 0x00168840 File Offset: 0x00166C40
		public bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002A8C RID: 10892 RVA: 0x00168874 File Offset: 0x00166C74
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

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002A8D RID: 10893 RVA: 0x001688A8 File Offset: 0x00166CA8
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

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002A8E RID: 10894 RVA: 0x001688E8 File Offset: 0x00166CE8
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

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002A8F RID: 10895 RVA: 0x00168928 File Offset: 0x00166D28
		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x00168948 File Offset: 0x00166D48
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

		// Token: 0x06002A91 RID: 10897 RVA: 0x0016898C File Offset: 0x00166D8C
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

		// Token: 0x06002A92 RID: 10898 RVA: 0x001689B4 File Offset: 0x00166DB4
		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x001689E0 File Offset: 0x00166DE0
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

		// Token: 0x06002A94 RID: 10900 RVA: 0x00168B14 File Offset: 0x00166F14
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
				text = text + StatDefOf.MarketValue.LabelCap + ": " + this.BaseMarketValue.ToStringMoney();
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

		// Token: 0x06002A95 RID: 10901 RVA: 0x00168FDC File Offset: 0x001673DC
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

		// Token: 0x06002A96 RID: 10902 RVA: 0x00169010 File Offset: 0x00167410
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

		// Token: 0x06002A97 RID: 10903 RVA: 0x00169048 File Offset: 0x00167448
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

		// Token: 0x06002A98 RID: 10904 RVA: 0x00169080 File Offset: 0x00167480
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

		// Token: 0x06002A99 RID: 10905 RVA: 0x001690C8 File Offset: 0x001674C8
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

		// Token: 0x06002A9A RID: 10906 RVA: 0x00169110 File Offset: 0x00167510
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

		// Token: 0x06002A9B RID: 10907 RVA: 0x00169140 File Offset: 0x00167540
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

		// Token: 0x06002A9C RID: 10908 RVA: 0x00169188 File Offset: 0x00167588
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

		// Token: 0x06002A9D RID: 10909 RVA: 0x001691C8 File Offset: 0x001675C8
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

		// Token: 0x06002A9E RID: 10910 RVA: 0x00169248 File Offset: 0x00167648
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

		// Token: 0x06002A9F RID: 10911 RVA: 0x001692C4 File Offset: 0x001676C4
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

		// Token: 0x06002AA0 RID: 10912 RVA: 0x0016931C File Offset: 0x0016771C
		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x0016933C File Offset: 0x0016773C
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

		// Token: 0x040016E9 RID: 5865
		public List<Thing> thingsColony = new List<Thing>();

		// Token: 0x040016EA RID: 5866
		public List<Thing> thingsTrader = new List<Thing>();

		// Token: 0x040016EB RID: 5867
		private int countToTransfer;

		// Token: 0x040016EC RID: 5868
		private float pricePlayerBuy = -1f;

		// Token: 0x040016ED RID: 5869
		private float pricePlayerSell = -1f;

		// Token: 0x040016EE RID: 5870
		private float priceFactorBuy_TraderPriceType;

		// Token: 0x040016EF RID: 5871
		private float priceFactorSell_TraderPriceType;

		// Token: 0x040016F0 RID: 5872
		private float priceFactorSell_ItemSellPriceFactor;

		// Token: 0x040016F1 RID: 5873
		private float priceGain_PlayerNegotiator;

		// Token: 0x040016F2 RID: 5874
		private float priceGain_FactionBase;
	}
}
