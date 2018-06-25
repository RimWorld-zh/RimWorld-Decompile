using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077B RID: 1915
	public class TradeDeal
	{
		// Token: 0x040016C8 RID: 5832
		private List<Tradeable> tradeables = new List<Tradeable>();

		// Token: 0x040016C9 RID: 5833
		public List<string> cannotSellReasons = new List<string>();

		// Token: 0x06002A3C RID: 10812 RVA: 0x001663B7 File Offset: 0x001647B7
		public TradeDeal()
		{
			this.Reset();
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002A3D RID: 10813 RVA: 0x001663DC File Offset: 0x001647DC
		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x001663FC File Offset: 0x001647FC
		public Tradeable SilverTradeable
		{
			get
			{
				for (int i = 0; i < this.tradeables.Count; i++)
				{
					if (this.tradeables[i].ThingDef == ThingDefOf.Silver)
					{
						return this.tradeables[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002A3F RID: 10815 RVA: 0x00166460 File Offset: 0x00164860
		public List<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x0016647B File Offset: 0x0016487B
		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x0016649C File Offset: 0x0016489C
		private void AddAllTradeables()
		{
			foreach (Thing t in TradeSession.trader.ColonyThingsWillingToBuy(TradeSession.playerNegotiator))
			{
				if (TradeUtility.PlayerSellableNow(t))
				{
					string text;
					if (!TradeSession.playerNegotiator.IsWorldPawn() && !this.InSellablePosition(t, out text))
					{
						if (text != null && !this.cannotSellReasons.Contains(text))
						{
							this.cannotSellReasons.Add(text);
						}
					}
					else
					{
						this.AddToTradeables(t, Transactor.Colony);
					}
				}
			}
			if (!TradeSession.giftMode)
			{
				foreach (Thing t2 in TradeSession.trader.Goods)
				{
					this.AddToTradeables(t2, Transactor.Trader);
				}
			}
			if (!TradeSession.giftMode)
			{
				if (this.tradeables.Find((Tradeable x) => x.IsCurrency) == null)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
					thing.stackCount = 0;
					this.AddToTradeables(thing, Transactor.Trader);
				}
			}
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x00166614 File Offset: 0x00164A14
		private bool InSellablePosition(Thing t, out string reason)
		{
			bool result;
			if (!t.Spawned)
			{
				reason = null;
				result = false;
			}
			else if (t.Position.Fogged(t.Map))
			{
				reason = null;
				result = false;
			}
			else
			{
				Room room = t.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					int num = GenRadial.NumCellsInRadius(6.9f);
					for (int i = 0; i < num; i++)
					{
						IntVec3 intVec = t.Position + GenRadial.RadialPattern[i];
						if (intVec.InBounds(t.Map) && intVec.GetRoom(t.Map, RegionType.Set_Passable) == room)
						{
							List<Thing> thingList = intVec.GetThingList(t.Map);
							for (int j = 0; j < thingList.Count; j++)
							{
								if (thingList[j].PreventPlayerSellingThingsNearby(out reason))
								{
									return false;
								}
							}
						}
					}
				}
				reason = null;
				result = true;
			}
			return result;
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x00166724 File Offset: 0x00164B24
		private void AddToTradeables(Thing t, Transactor trans)
		{
			Tradeable tradeable = TransferableUtility.TradeableMatching(t, this.tradeables);
			if (tradeable == null)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null)
				{
					tradeable = new Tradeable_Pawn();
				}
				else
				{
					tradeable = new Tradeable();
				}
				this.tradeables.Add(tradeable);
			}
			tradeable.AddThing(t, trans);
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x0016677C File Offset: 0x00164B7C
		public void UpdateCurrencyCount()
		{
			if (this.SilverTradeable != null && !TradeSession.giftMode)
			{
				float num = 0f;
				for (int i = 0; i < this.tradeables.Count; i++)
				{
					Tradeable tradeable = this.tradeables[i];
					if (!tradeable.IsCurrency)
					{
						num += tradeable.CurTotalSilverCostForSource;
					}
				}
				this.SilverTradeable.ForceToSource(-Mathf.RoundToInt(num));
			}
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x00166800 File Offset: 0x00164C00
		public bool TryExecute(out bool actuallyTraded)
		{
			bool result;
			if (TradeSession.giftMode)
			{
				this.UpdateCurrencyCount();
				this.LimitCurrencyCountToFunds();
				int goodwillChange = FactionGiftUtility.GetGoodwillChange(this.tradeables, TradeSession.trader.Faction);
				FactionGiftUtility.GiveGift(this.tradeables, TradeSession.trader.Faction, TradeSession.playerNegotiator);
				actuallyTraded = ((float)goodwillChange > 0f);
				result = true;
			}
			else if (this.SilverTradeable == null || this.SilverTradeable.CountPostDealFor(Transactor.Colony) < 0)
			{
				Find.WindowStack.WindowOfType<Dialog_Trade>().FlashSilver();
				Messages.Message("MessageColonyCannotAfford".Translate(), MessageTypeDefOf.RejectInput, false);
				actuallyTraded = false;
				result = false;
			}
			else
			{
				this.UpdateCurrencyCount();
				this.LimitCurrencyCountToFunds();
				actuallyTraded = false;
				float num = 0f;
				foreach (Tradeable tradeable in this.tradeables)
				{
					if (tradeable.ActionToDo != TradeAction.None)
					{
						actuallyTraded = true;
					}
					if (tradeable.ActionToDo == TradeAction.PlayerSells)
					{
						num += tradeable.CurTotalSilverCostForDestination;
					}
					tradeable.ResolveTrade();
				}
				this.Reset();
				if (TradeSession.trader.Faction != null)
				{
					TradeSession.trader.Faction.Notify_PlayerTraded(num, TradeSession.playerNegotiator);
				}
				Pawn pawn = TradeSession.trader as Pawn;
				if (pawn != null)
				{
					TaleRecorder.RecordTale(TaleDefOf.TradedWith, new object[]
					{
						TradeSession.playerNegotiator,
						pawn
					});
				}
				TradeSession.playerNegotiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Trade);
				result = true;
			}
			return result;
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x001669C0 File Offset: 0x00164DC0
		public bool DoesTraderHaveEnoughSilver()
		{
			return TradeSession.giftMode || (this.SilverTradeable != null && this.SilverTradeable.CountPostDealFor(Transactor.Trader) >= 0);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x00166A0C File Offset: 0x00164E0C
		private void LimitCurrencyCountToFunds()
		{
			if (this.SilverTradeable != null)
			{
				if (this.SilverTradeable.CountToTransferToSource > this.SilverTradeable.CountHeldBy(Transactor.Trader))
				{
					this.SilverTradeable.ForceToSource(this.SilverTradeable.CountHeldBy(Transactor.Trader));
				}
				if (this.SilverTradeable.CountToTransferToDestination > this.SilverTradeable.CountHeldBy(Transactor.Colony))
				{
					this.SilverTradeable.ForceToDestination(this.SilverTradeable.CountHeldBy(Transactor.Colony));
				}
			}
		}
	}
}
