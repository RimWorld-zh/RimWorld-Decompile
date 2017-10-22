using RimWorld.Planet;
using System;
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
				int num = 0;
				Tradeable result;
				while (true)
				{
					if (num < this.tradeables.Count)
					{
						if (this.tradeables[num].ThingDef == ThingDefOf.Silver)
						{
							result = this.tradeables[num];
							break;
						}
						num++;
						continue;
					}
					result = null;
					break;
				}
				return result;
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
			if (this.tradeables.Find((Predicate<Tradeable>)((Tradeable x) => x.IsCurrency)) == null)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
				thing.stackCount = 0;
				this.AddToTradeables(thing, Transactor.Trader);
			}
		}

		private bool InSellablePosition(Thing t, out string reason)
		{
			bool result;
			if (!t.Spawned)
			{
				reason = (string)null;
				result = false;
			}
			else if (t.Position.Fogged(t.Map))
			{
				reason = (string)null;
				result = false;
			}
			else
			{
				Room room = t.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					int num = GenRadial.NumCellsInRadius(6.9f);
					for (int num2 = 0; num2 < num; num2++)
					{
						IntVec3 intVec = t.Position + GenRadial.RadialPattern[num2];
						if (intVec.InBounds(t.Map) && intVec.GetRoom(t.Map, RegionType.Set_Passable) == room)
						{
							List<Thing> thingList = intVec.GetThingList(t.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								if (thingList[i].PreventPlayerSellingThingsNearby(out reason))
									goto IL_00ce;
							}
						}
					}
				}
				reason = (string)null;
				result = true;
			}
			goto IL_0101;
			IL_0101:
			return result;
			IL_00ce:
			result = false;
			goto IL_0101;
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
			bool result;
			if (this.SilverTradeable.CountPostDealFor(Transactor.Colony) < 0)
			{
				Find.WindowStack.WindowOfType<Dialog_Trade>().FlashSilver();
				Messages.Message("MessageColonyCannotAfford".Translate(), MessageTypeDefOf.RejectInput);
				actuallyTraded = false;
				result = false;
			}
			else
			{
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
				result = true;
			}
			return result;
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
