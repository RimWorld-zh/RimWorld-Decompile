using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020008A6 RID: 2214
	[StaticConstructorOnStartup]
	public class Dialog_Trade : Window
	{
		// Token: 0x060032AA RID: 12970 RVA: 0x001B4C20 File Offset: 0x001B3020
		public Dialog_Trade(Pawn playerNegotiator, ITrader trader, bool giftsOnly = false)
		{
			this.giftsOnly = giftsOnly;
			TradeSession.SetupWith(trader, playerNegotiator, giftsOnly);
			this.SetupPlayerCaravanVariables();
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (trader is PassingShip)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x001B4CDC File Offset: 0x001B30DC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x001B4D04 File Offset: 0x001B3104
		private int Tile
		{
			get
			{
				return TradeSession.playerNegotiator.Tile;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x001B4D24 File Offset: 0x001B3124
		private BiomeDef Biome
		{
			get
			{
				return Find.WorldGrid[this.Tile].biome;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x001B4D50 File Offset: 0x001B3150
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
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x001B4DCC File Offset: 0x001B31CC
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
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassCapacity;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x001B4E58 File Offset: 0x001B3258
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.MassUsage, this.MassCapacity, this.Tile, (caravan == null || !caravan.pather.Moving) ? -1 : caravan.pather.nextTile, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x001B4EF8 File Offset: 0x001B32F8
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore, Faction.OfPlayer);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x001B4F6C File Offset: 0x001B336C
		private Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x001B4FD0 File Offset: 0x001B33D0
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x001B502C File Offset: 0x001B342C
		public override void PostOpen()
		{
			base.PostOpen();
			if (!this.giftsOnly)
			{
				Pawn playerNegotiator = TradeSession.playerNegotiator;
				float level = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Talking);
				float level2 = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Hearing);
				if (level < 0.95f || level2 < 0.95f)
				{
					string text;
					if (level < 0.95f)
					{
						text = "NegotiatorTalkingImpaired".Translate(new object[]
						{
							playerNegotiator.LabelShort
						});
					}
					else
					{
						text = "NegotiatorHearingImpaired".Translate(new object[]
						{
							playerNegotiator.LabelShort
						});
					}
					text = text + "\n\n" + "NegotiatorCapacityImpaired".Translate();
					Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
				}
			}
			this.CacheTradeables();
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x001B5110 File Offset: 0x001B3510
		private void CacheTradeables()
		{
			this.cachedCurrencyTradeable = (from x in TradeSession.deal.AllTradeables
			where x.IsCurrency
			select x).FirstOrDefault<Tradeable>();
			this.cachedTradeables = (from tr in TradeSession.deal.AllTradeables
			where !tr.IsCurrency
			orderby (!tr.TraderWillTrade) ? -1 : 0 descending
			select tr).ThenBy((Tradeable tr) => tr, this.sorter1.Comparer).ThenBy((Tradeable tr) => tr, this.sorter2.Comparer).ThenBy((Tradeable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ThenBy((Tradeable tr) => tr.ThingDef.label).ThenBy(delegate(Tradeable tr)
			{
				QualityCategory qualityCategory;
				int result;
				if (tr.AnyThing.TryGetQuality(out qualityCategory))
				{
					result = (int)qualityCategory;
				}
				else
				{
					result = -1;
				}
				return result;
			}).ThenBy((Tradeable tr) => tr.AnyThing.HitPoints).ToList<Tradeable>();
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x001B5290 File Offset: 0x001B3690
		public override void DoWindowContents(Rect inRect)
		{
			if (this.playerIsCaravan)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation), null, this.Tile, null, -9999f, new Rect(12f, 0f, inRect.width - 24f, 40f), true, null, false);
				inRect.yMin += 52f;
			}
			TradeSession.deal.UpdateCurrencyCount();
			GUI.BeginGroup(inRect);
			inRect = inRect.AtZero();
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTradeables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTradeables();
			});
			float num = inRect.width - 590f;
			Rect position = new Rect(num, 0f, inRect.width - num, 58f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(0f, 0f, position.width / 2f, position.height);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, Faction.OfPlayer.Name);
			Rect rect2 = new Rect(position.width / 2f, 0f, position.width / 2f, position.height);
			Text.Anchor = TextAnchor.UpperRight;
			string text = TradeSession.trader.TraderName;
			if (Text.CalcSize(text).x > rect2.width)
			{
				Text.Font = GameFont.Small;
				text = text.Truncate(rect2.width, null);
			}
			Widgets.Label(rect2, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(0f, 27f, position.width / 2f, position.height / 2f);
			Widgets.Label(rect3, "Negotiator".Translate() + ": " + TradeSession.playerNegotiator.LabelShort);
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect4 = new Rect(position.width / 2f, 27f, position.width / 2f, position.height / 2f);
			Widgets.Label(rect4, TradeSession.trader.TraderKind.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!TradeSession.giftMode)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.6f);
				Text.Font = GameFont.Tiny;
				Rect rect5 = new Rect(position.width / 2f - 100f - 30f, 0f, 200f, position.height);
				Text.Anchor = TextAnchor.LowerCenter;
				Widgets.Label(rect5, "PositiveBuysNegativeSells".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			GUI.EndGroup();
			float num2 = 0f;
			if (this.cachedCurrencyTradeable != null)
			{
				float num3 = inRect.width - 16f;
				Rect rect6 = new Rect(0f, 58f, num3, 30f);
				TradeUI.DrawTradeableRow(rect6, this.cachedCurrencyTradeable, 1);
				GUI.color = Color.gray;
				Widgets.DrawLineHorizontal(0f, 87f, num3);
				GUI.color = Color.white;
				num2 = 30f;
			}
			Rect mainRect = new Rect(0f, 58f + num2, inRect.width, inRect.height - 58f - 38f - num2 - 20f);
			this.FillMainRect(mainRect);
			Rect rect7 = new Rect(inRect.width / 2f - Dialog_Trade.AcceptButtonSize.x / 2f, inRect.height - 55f, Dialog_Trade.AcceptButtonSize.x, Dialog_Trade.AcceptButtonSize.y);
			if (Widgets.ButtonText(rect7, (!TradeSession.giftMode) ? "AcceptButton".Translate() : ("OfferGifts".Translate() + " (" + FactionGiftUtility.GetGoodwillChange(TradeSession.deal.AllTradeables, TradeSession.trader.Faction).ToStringWithSign() + ")"), true, false, true))
			{
				Action action = delegate()
				{
					bool flag;
					if (TradeSession.deal.TryExecute(out flag))
					{
						if (flag)
						{
							SoundDefOf.ExecuteTrade.PlayOneShotOnCamera(null);
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
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmTraderShortFunds".Translate(), action, false, null));
				}
				Event.current.Use();
			}
			Rect rect8 = new Rect(rect7.x - 10f - Dialog_Trade.OtherBottomButtonSize.x, rect7.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y);
			if (Widgets.ButtonText(rect8, "ResetButton".Translate(), true, false, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				TradeSession.deal.Reset();
				this.CacheTradeables();
				this.CountToTransferChanged();
			}
			Rect rect9 = new Rect(rect7.xMax + 10f, rect7.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y);
			if (Widgets.ButtonText(rect9, "CancelButton".Translate(), true, false, true))
			{
				this.Close(true);
				Event.current.Use();
			}
			float y = Dialog_Trade.OtherBottomButtonSize.y;
			Rect rect10 = new Rect(inRect.width - y, rect7.y, y, y);
			if (Widgets.ButtonImageWithBG(rect10, Dialog_Trade.ShowSellableItemsIcon, new Vector2?(new Vector2(32f, 32f))))
			{
				Find.WindowStack.Add(new Dialog_SellableItems(TradeSession.trader.TraderKind));
			}
			TooltipHandler.TipRegion(rect10, "CommandShowSellableItemsDesc".Translate());
			Faction faction = TradeSession.trader.Faction;
			if (faction != null && !this.giftsOnly && !faction.def.permanentEnemy)
			{
				Rect rect11 = new Rect(rect10.x - y - 4f, rect7.y, y, y);
				if (TradeSession.giftMode)
				{
					if (Widgets.ButtonImageWithBG(rect11, Dialog_Trade.TradeModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = false;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegion(rect11, "TradeModeTip".Translate());
				}
				else
				{
					if (Widgets.ButtonImageWithBG(rect11, Dialog_Trade.GiftModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = true;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegion(rect11, "GiftModeTip".Translate(new object[]
					{
						faction.Name
					}));
				}
			}
			GUI.EndGroup();
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x001B5A2E File Offset: 0x001B3E2E
		public override void Close(bool doCloseSound = true)
		{
			DragSliderManager.ForceStop();
			base.Close(doCloseSound);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x001B5A40 File Offset: 0x001B3E40
		private void FillMainRect(Rect mainRect)
		{
			Text.Font = GameFont.Small;
			float height = 6f + (float)this.cachedTradeables.Count * 30f;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, height);
			Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
			float num = 6f;
			float num2 = this.scrollPosition.y - 30f;
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
				num += 30f;
				num4++;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x001B5B74 File Offset: 0x001B3F74
		public void FlashSilver()
		{
			Dialog_Trade.lastCurrencyFlashTime = Time.time;
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x001B5B84 File Offset: 0x001B3F84
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x001B5B9C File Offset: 0x001B3F9C
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
				caravan.Notify_StartedTrading();
			}
			else
			{
				this.playerIsCaravan = false;
			}
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x001B5C24 File Offset: 0x001B4024
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x04001B41 RID: 6977
		private bool giftsOnly;

		// Token: 0x04001B42 RID: 6978
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001B43 RID: 6979
		public static float lastCurrencyFlashTime = -100f;

		// Token: 0x04001B44 RID: 6980
		private List<Tradeable> cachedTradeables = null;

		// Token: 0x04001B45 RID: 6981
		private Tradeable cachedCurrencyTradeable = null;

		// Token: 0x04001B46 RID: 6982
		private TransferableSorterDef sorter1;

		// Token: 0x04001B47 RID: 6983
		private TransferableSorterDef sorter2;

		// Token: 0x04001B48 RID: 6984
		private bool playerIsCaravan;

		// Token: 0x04001B49 RID: 6985
		private List<Thing> playerCaravanAllPawnsAndItems;

		// Token: 0x04001B4A RID: 6986
		private bool massUsageDirty = true;

		// Token: 0x04001B4B RID: 6987
		private float cachedMassUsage;

		// Token: 0x04001B4C RID: 6988
		private bool massCapacityDirty = true;

		// Token: 0x04001B4D RID: 6989
		private float cachedMassCapacity;

		// Token: 0x04001B4E RID: 6990
		private string cachedMassCapacityExplanation;

		// Token: 0x04001B4F RID: 6991
		private bool tilesPerDayDirty = true;

		// Token: 0x04001B50 RID: 6992
		private float cachedTilesPerDay;

		// Token: 0x04001B51 RID: 6993
		private string cachedTilesPerDayExplanation;

		// Token: 0x04001B52 RID: 6994
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04001B53 RID: 6995
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x04001B54 RID: 6996
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x04001B55 RID: 6997
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x04001B56 RID: 6998
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04001B57 RID: 6999
		private bool visibilityDirty = true;

		// Token: 0x04001B58 RID: 7000
		private float cachedVisibility;

		// Token: 0x04001B59 RID: 7001
		private string cachedVisibilityExplanation;

		// Token: 0x04001B5A RID: 7002
		private const float TitleAreaHeight = 45f;

		// Token: 0x04001B5B RID: 7003
		private const float TopAreaHeight = 58f;

		// Token: 0x04001B5C RID: 7004
		private const float ColumnWidth = 120f;

		// Token: 0x04001B5D RID: 7005
		private const float FirstCommodityY = 6f;

		// Token: 0x04001B5E RID: 7006
		private const float RowInterval = 30f;

		// Token: 0x04001B5F RID: 7007
		private const float SpaceBetweenTraderNameAndTraderKind = 27f;

		// Token: 0x04001B60 RID: 7008
		private const float ShowSellableItemsIconSize = 32f;

		// Token: 0x04001B61 RID: 7009
		private const float GiftModeIconSize = 32f;

		// Token: 0x04001B62 RID: 7010
		private const float TradeModeIconSize = 32f;

		// Token: 0x04001B63 RID: 7011
		protected static readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		// Token: 0x04001B64 RID: 7012
		protected static readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04001B65 RID: 7013
		private static readonly Texture2D ShowSellableItemsIcon = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x04001B66 RID: 7014
		private static readonly Texture2D GiftModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/GiftMode", true);

		// Token: 0x04001B67 RID: 7015
		private static readonly Texture2D TradeModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/TradeMode", true);
	}
}
