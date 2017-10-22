using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Dialog_Trade : Window
	{
		private const float TitleAreaHeight = 45f;

		private const float BaseTopAreaHeight = 55f;

		private const float ColumnWidth = 120f;

		private const float FirstCommodityY = 6f;

		private const float RowInterval = 30f;

		private const float SpaceBetweenTraderNameAndTraderKind = 27f;

		private Vector2 scrollPosition = Vector2.zero;

		public static float lastCurrencyFlashTime = -100f;

		private List<Tradeable> cachedTradeables;

		private Tradeable cachedCurrencyTradeable;

		private TransferableSorterDef sorter1;

		private TransferableSorterDef sorter2;

		private bool playerIsCaravan;

		private List<Thing> playerCaravanAllPawnsAndItems;

		private bool massUsageDirty = true;

		private float cachedMassUsage;

		private bool massCapacityDirty = true;

		private float cachedMassCapacity;

		private bool daysWorthOfFoodDirty = true;

		private Pair<float, float> cachedDaysWorthOfFood;

		protected readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		protected readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		private int Tile
		{
			get
			{
				return TradeSession.playerNegotiator.Tile;
			}
		}

		private bool EnvironmentAllowsEatingVirtualPlantsNow
		{
			get
			{
				return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsNowAt(this.Tile);
			}
		}

		private float TopAreaHeight
		{
			get
			{
				float num = 55f;
				if (this.playerIsCaravan)
				{
					num = (float)(num + 28.0);
				}
				return num;
			}
		}

		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, IgnorePawnsInventoryMode.Ignore, false, false);
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast();
					}
				}
				return this.cachedMassUsage;
			}
		}

		private float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables);
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast();
					}
				}
				return this.cachedMassCapacity;
			}
		}

		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.EnvironmentAllowsEatingVirtualPlantsNow, IgnorePawnsInventoryMode.Ignore);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		public Dialog_Trade(Pawn playerNegotiator, ITrader trader)
		{
			TradeSession.SetupWith(trader, playerNegotiator);
			this.SetupPlayerCaravanVariables();
			base.closeOnEscapeKey = true;
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
			base.soundAppear = SoundDefOf.CommsWindow_Open;
			base.soundClose = SoundDefOf.CommsWindow_Close;
			if (!(trader is Pawn))
			{
				base.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		public override void PostOpen()
		{
			base.PostOpen();
			if (TradeSession.playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Talking) < 0.99000000953674316)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("NegotiatorTalkingImpaired".Translate(TradeSession.playerNegotiator.LabelShort), (string)null, null, (string)null, null, (string)null, false));
			}
			this.CacheTradeables();
		}

		private void CacheTradeables()
		{
			this.cachedCurrencyTradeable = (from x in TradeSession.deal.AllTradeables
			where x.IsCurrency
			select x).FirstOrDefault();
			this.cachedTradeables = (from tr in TradeSession.deal.AllTradeables
			where !tr.IsCurrency
			orderby (!tr.TraderWillTrade) ? (-1) : 0 descending
			select tr).ThenBy((Func<Tradeable, Transferable>)((Tradeable tr) => tr), this.sorter1.Comparer).ThenBy((Func<Tradeable, Transferable>)((Tradeable tr) => tr), this.sorter2.Comparer).ThenBy((Func<Tradeable, float>)((Tradeable tr) => TransferableUIUtility.DefaultListOrderPriority(tr))).ThenBy((Func<Tradeable, string>)((Tradeable tr) => tr.ThingDef.label)).ThenBy((Func<Tradeable, int>)delegate(Tradeable tr)
			{
				QualityCategory result = default(QualityCategory);
				if (tr.AnyThing.TryGetQuality(out result))
				{
					return (int)result;
				}
				return -1;
			}).ThenBy((Func<Tradeable, int>)((Tradeable tr) => tr.AnyThing.HitPoints)).ToList();
		}

		public override void DoWindowContents(Rect inRect)
		{
			TradeSession.deal.UpdateCurrencyCount();
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, (Action<TransferableSorterDef>)delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTradeables();
			}, (Action<TransferableSorterDef>)delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTradeables();
			});
			float num = (float)(inRect.width - 590.0);
			Rect rect = new Rect(num, 0f, inRect.width - num, this.TopAreaHeight);
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Medium;
			Rect rect2 = new Rect(0f, 0f, (float)(rect.width / 2.0), rect.height);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect2, Faction.OfPlayer.Name);
			Rect rect3 = new Rect((float)(rect.width / 2.0), 0f, (float)(rect.width / 2.0), rect.height);
			Text.Anchor = TextAnchor.UpperRight;
			string text = TradeSession.trader.TraderName;
			Vector2 vector = Text.CalcSize(text);
			if (vector.x > rect3.width)
			{
				Text.Font = GameFont.Small;
				text = text.Truncate(rect3.width, null);
			}
			Widgets.Label(rect3, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect4 = new Rect(0f, 27f, (float)(rect.width / 2.0), (float)(rect.height / 2.0));
			Widgets.Label(rect4, "Negotiator".Translate() + ": " + TradeSession.playerNegotiator.LabelShort);
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect5 = new Rect((float)(rect.width / 2.0), 27f, (float)(rect.width / 2.0), (float)(rect.height / 2.0));
			Widgets.Label(rect5, TradeSession.trader.TraderKind.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.6f);
			Text.Font = GameFont.Tiny;
			Rect rect6 = new Rect((float)(rect.width / 2.0 - 100.0 - 30.0), 0f, 200f, rect.height);
			Text.Anchor = TextAnchor.LowerCenter;
			Widgets.Label(rect6, "PositiveBuysNegativeSells".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (this.playerIsCaravan)
			{
				Text.Font = GameFont.Small;
				float massUsage = this.MassUsage;
				float massCapacity = this.MassCapacity;
				Rect rect7 = rect.AtZero();
				rect7.y = 45f;
				TransferableUIUtility.DrawMassInfo(rect7, massUsage, massCapacity, "TradeMassUsageTooltip".Translate(), -9999f, false);
				CaravanUIUtility.DrawDaysWorthOfFoodInfo(new Rect(rect7.x, (float)(rect7.y + 19.0), rect7.width, rect7.height), this.DaysWorthOfFood.First, this.DaysWorthOfFood.Second, this.EnvironmentAllowsEatingVirtualPlantsNow, false, 200f);
			}
			GUI.EndGroup();
			float num2 = 0f;
			if (this.cachedCurrencyTradeable != null)
			{
				float num3 = (float)(inRect.width - 16.0);
				Rect rect8 = new Rect(0f, this.TopAreaHeight, num3, 30f);
				TradeUI.DrawTradeableRow(rect8, this.cachedCurrencyTradeable, 1);
				GUI.color = Color.gray;
				Widgets.DrawLineHorizontal(0f, (float)(this.TopAreaHeight + 30.0 - 1.0), num3);
				GUI.color = Color.white;
				num2 = 30f;
			}
			Rect mainRect = new Rect(0f, this.TopAreaHeight + num2, inRect.width, (float)(inRect.height - this.TopAreaHeight - 38.0 - num2 - 20.0));
			this.FillMainRect(mainRect);
			double num4 = inRect.width / 2.0;
			Vector2 acceptButtonSize = this.AcceptButtonSize;
			double x2 = num4 - acceptButtonSize.x / 2.0;
			double y = inRect.height - 55.0;
			Vector2 acceptButtonSize2 = this.AcceptButtonSize;
			float x3 = acceptButtonSize2.x;
			Vector2 acceptButtonSize3 = this.AcceptButtonSize;
			Rect rect9 = new Rect((float)x2, (float)y, x3, acceptButtonSize3.y);
			if (Widgets.ButtonText(rect9, "AcceptButton".Translate(), true, false, true))
			{
				Action action = (Action)delegate
				{
					bool flag = default(bool);
					if (TradeSession.deal.TryExecute(out flag))
					{
						if (flag)
						{
							SoundDefOf.ExecuteTrade.PlayOneShotOnCamera(null);
							Pawn pawn = TradeSession.trader as Pawn;
							if (pawn != null)
							{
								TaleRecorder.RecordTale(TaleDefOf.TradedWith, TradeSession.playerNegotiator, pawn);
							}
							this.Close(false);
						}
						else
						{
							this.Close(true);
						}
					}
				};
				if (TradeSession.deal.DoesTraderHaveEnoughSilver())
				{
					action();
				}
				else
				{
					this.FlashSilver();
					SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmTraderShortFunds".Translate(), action, false, (string)null));
				}
				Event.current.Use();
			}
			double num5 = rect9.x - 10.0;
			Vector2 otherBottomButtonSize = this.OtherBottomButtonSize;
			double x4 = num5 - otherBottomButtonSize.x;
			float y2 = rect9.y;
			Vector2 otherBottomButtonSize2 = this.OtherBottomButtonSize;
			float x5 = otherBottomButtonSize2.x;
			Vector2 otherBottomButtonSize3 = this.OtherBottomButtonSize;
			Rect rect10 = new Rect((float)x4, y2, x5, otherBottomButtonSize3.y);
			if (Widgets.ButtonText(rect10, "ResetButton".Translate(), true, false, true))
			{
				SoundDefOf.TickLow.PlayOneShotOnCamera(null);
				TradeSession.deal.Reset();
				this.CacheTradeables();
				this.CountToTransferChanged();
				Event.current.Use();
			}
			double x6 = rect9.xMax + 10.0;
			float y3 = rect9.y;
			Vector2 otherBottomButtonSize4 = this.OtherBottomButtonSize;
			float x7 = otherBottomButtonSize4.x;
			Vector2 otherBottomButtonSize5 = this.OtherBottomButtonSize;
			Rect rect11 = new Rect((float)x6, y3, x7, otherBottomButtonSize5.y);
			if (Widgets.ButtonText(rect11, "CancelButton".Translate(), true, false, true))
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		public override void Close(bool doCloseSound = true)
		{
			DragSliderManager.ForceStop();
			base.Close(doCloseSound);
		}

		private void FillMainRect(Rect mainRect)
		{
			Text.Font = GameFont.Small;
			float height = (float)(6.0 + (float)this.cachedTradeables.Count * 30.0);
			Rect viewRect = new Rect(0f, 0f, (float)(mainRect.width - 16.0), height);
			Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
			float num = 6f;
			float num2 = (float)(this.scrollPosition.y - 30.0);
			float num3 = this.scrollPosition.y + mainRect.height;
			int num4 = 0;
			for (int i = 0; i < this.cachedTradeables.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					Rect rect = new Rect(0f, num, viewRect.width, 30f);
					int countToTransfer = this.cachedTradeables[i].CountToTransfer;
					TradeUI.DrawTradeableRow(rect, this.cachedTradeables[i], num4);
					if (countToTransfer != this.cachedTradeables[i].CountToTransfer)
					{
						this.CountToTransferChanged();
					}
				}
				num = (float)(num + 30.0);
				num4++;
			}
			Widgets.EndScrollView();
		}

		public void FlashSilver()
		{
			Dialog_Trade.lastCurrencyFlashTime = Time.time;
		}

		public override bool CausesMessageBackground()
		{
			return true;
		}

		private void SetupPlayerCaravanVariables()
		{
			Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
			if (caravan != null)
			{
				this.playerIsCaravan = true;
				this.playerCaravanAllPawnsAndItems = new List<Thing>();
				List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					this.playerCaravanAllPawnsAndItems.Add(pawnsListForReading[i]);
				}
				this.playerCaravanAllPawnsAndItems.AddRange(CaravanInventoryUtility.AllInventoryItems(caravan));
			}
			else
			{
				this.playerIsCaravan = false;
			}
		}

		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.daysWorthOfFoodDirty = true;
		}
	}
}
