using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077D RID: 1917
	public class TradeDeal
	{
		// Token: 0x06002A3D RID: 10813 RVA: 0x00165FFB File Offset: 0x001643FB
		public TradeDeal()
		{
			this.Reset();
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x00166020 File Offset: 0x00164420
		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002A3F RID: 10815 RVA: 0x00166040 File Offset: 0x00164440
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

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x001660A4 File Offset: 0x001644A4
		public List<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x001660BF File Offset: 0x001644BF
		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x001660E0 File Offset: 0x001644E0
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

		// Token: 0x06002A43 RID: 10819 RVA: 0x00166258 File Offset: 0x00164658
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

		// Token: 0x06002A44 RID: 10820 RVA: 0x00166368 File Offset: 0x00164768
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

		// Token: 0x06002A45 RID: 10821 RVA: 0x001663C0 File Offset: 0x001647C0
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

		// Token: 0x06002A46 RID: 10822 RVA: 0x00166444 File Offset: 0x00164844
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

		// Token: 0x06002A47 RID: 10823 RVA: 0x00166604 File Offset: 0x00164A04
		public bool DoesTraderHaveEnoughSilver()
		{
			return TradeSession.giftMode || (this.SilverTradeable != null && this.SilverTradeable.CountPostDealFor(Transactor.Trader) >= 0);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x00166650 File Offset: 0x00164A50
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

		// Token: 0x040016CA RID: 5834
		private List<Tradeable> tradeables = new List<Tradeable>();

		// Token: 0x040016CB RID: 5835
		public List<string> cannotSellReasons = new List<string>();
	}
}
