using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class TradeDeal
	{
		private List<Tradeable> tradeables = new List<Tradeable>();

		public List<string> cannotSellReasons = new List<string>();

		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

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

		public IEnumerable<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		public TradeDeal()
		{
			this.Reset();
		}

		public IEnumerator<Tradeable> GetEnumerator()
		{
			return (IEnumerator<Tradeable>)(object)this.tradeables.GetEnumerator();
		}

		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		private void AddAllTradeables()
		{
			foreach (Thing item in TradeSession.trader.ColonyThingsWillingToBuy(TradeSession.playerNegotiator))
			{
				string text = default(string);
				if (!TradeSession.playerNegotiator.IsWorldPawn() && !this.InSellablePosition(item, out text))
				{
					if (text != null && !this.cannotSellReasons.Contains(text))
					{
						this.cannotSellReasons.Add(text);
					}
				}
				else
				{
					this.AddToTradeables(item, Transactor.Colony);
				}
			}
			foreach (Thing good in TradeSession.trader.Goods)
			{
				this.AddToTradeables(good, Transactor.Trader);
			}
			if (this.tradeables.Find((Tradeable x) => x.IsCurrency) == null)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
				thing.stackCount = 0;
				this.AddToTradeables(thing, Transactor.Trader);
			}
		}

		private bool InSellablePosition(Thing t, out string reason)
		{
			if (!t.Spawned)
			{
				reason = null;
				return false;
			}
			if (t.Position.Fogged(t.Map))
			{
				reason = null;
				return false;
			}
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
			return true;
		}

		private void AddToTradeables(Thing t, Transactor trans)
		{
			Tradeable tradeable = TransferableUtility.TransferableMatching(t, this.tradeables);
			if (tradeable == null)
			{
				Pawn pawn = t as Pawn;
				tradeable = ((pawn == null) ? new Tradeable() : new Tradeable_Pawn());
				this.tradeables.Add(tradeable);
			}
			tradeable.AddThing(t, trans);
		}

		public void UpdateCurrencyCount()
		{
			float num = 0f;
			foreach (Tradeable tradeable in this.tradeables)
			{
				if (!tradeable.IsCurrency)
				{
					num += tradeable.CurTotalSilverCost;
				}
			}
			this.SilverTradeable.ForceTo(-Mathf.RoundToInt(num));
		}

		public bool TryExecute(out bool actuallyTraded)
		{
			if (this.SilverTradeable.CountPostDealFor(Transactor.Colony) < 0)
			{
				Find.WindowStack.WindowOfType<Dialog_Trade>().FlashSilver();
				Messages.Message("MessageColonyCannotAfford".Translate(), MessageTypeDefOf.RejectInput);
				actuallyTraded = false;
				return false;
			}
			this.UpdateCurrencyCount();
			this.LimitCurrencyCountToTraderFunds();
			actuallyTraded = false;
			foreach (Tradeable tradeable in this.tradeables)
			{
				if (tradeable.ActionToDo != 0)
				{
					actuallyTraded = true;
				}
				tradeable.ResolveTrade();
			}
			this.Reset();
			return true;
		}

		public bool DoesTraderHaveEnoughSilver()
		{
			return this.SilverTradeable.CountPostDealFor(Transactor.Trader) >= 0;
		}

		private void LimitCurrencyCountToTraderFunds()
		{
			if (this.SilverTradeable.CountToTransfer > this.SilverTradeable.CountHeldBy(Transactor.Trader))
			{
				this.SilverTradeable.ForceTo(this.SilverTradeable.CountHeldBy(Transactor.Trader));
			}
		}
	}
}
