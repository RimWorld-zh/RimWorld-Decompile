using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008DC RID: 2268
	public class Dialog_SplitCaravan : Window
	{
		// Token: 0x060033E1 RID: 13281 RVA: 0x001BB800 File Offset: 0x001B9C00
		public Dialog_SplitCaravan(Caravan caravan)
		{
			this.caravan = caravan;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060033E2 RID: 13282 RVA: 0x001BB8A0 File Offset: 0x001B9CA0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x001BB8C8 File Offset: 0x001B9CC8
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060033E4 RID: 13284 RVA: 0x001BB8E4 File Offset: 0x001B9CE4
		private BiomeDef Biome
		{
			get
			{
				return this.caravan.Biome;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060033E5 RID: 13285 RVA: 0x001BB904 File Offset: 0x001B9D04
		private float SourceMassUsage
		{
			get
			{
				if (this.sourceMassUsageDirty)
				{
					this.sourceMassUsageDirty = false;
					this.cachedSourceMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTransfer(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedSourceMassUsage;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060033E6 RID: 13286 RVA: 0x001BB948 File Offset: 0x001B9D48
		private float SourceMassCapacity
		{
			get
			{
				if (this.sourceMassCapacityDirty)
				{
					this.sourceMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceMassCapacity;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060033E7 RID: 13287 RVA: 0x001BB99C File Offset: 0x001B9D9C
		private float SourceTilesPerDay
		{
			get
			{
				if (this.sourceTilesPerDayDirty)
				{
					this.sourceTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTransfer(this.transferables, this.SourceMassUsage, this.SourceMassCapacity, this.caravan.Tile, (!this.caravan.pather.Moving) ? -1 : this.caravan.pather.nextTile, stringBuilder);
					this.cachedSourceTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceTilesPerDay;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060033E8 RID: 13288 RVA: 0x001BBA34 File Offset: 0x001B9E34
		private Pair<float, float> SourceDaysWorthOfFood
		{
			get
			{
				if (this.sourceDaysWorthOfFoodDirty)
				{
					this.sourceDaysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.caravan.pather.Moving)
					{
						using (Find.WorldPathFinder.FindPath(this.caravan.Tile, this.caravan.pather.Destination, null, null))
						{
							first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							second = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
						}
					}
					else
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3500);
						second = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3500);
					}
					this.cachedSourceDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedSourceDaysWorthOfFood;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x001BBBBC File Offset: 0x001B9FBC
		private Pair<ThingDef, float> SourceForagedFoodPerDay
		{
			get
			{
				if (this.sourceForagedFoodPerDayDirty)
				{
					this.sourceForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTransfer(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedSourceForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceForagedFoodPerDay;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x001BBC1C File Offset: 0x001BA01C
		private float SourceVisibility
		{
			get
			{
				if (this.sourceVisibilityDirty)
				{
					this.sourceVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceVisibility;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x001BBC70 File Offset: 0x001BA070
		private float DestMassUsage
		{
			get
			{
				if (this.destMassUsageDirty)
				{
					this.destMassUsageDirty = false;
					this.cachedDestMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedDestMassUsage;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x001BBCB4 File Offset: 0x001BA0B4
		private float DestMassCapacity
		{
			get
			{
				if (this.destMassCapacityDirty)
				{
					this.destMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedDestMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestMassCapacity;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x001BBD08 File Offset: 0x001BA108
		private float DestTilesPerDay
		{
			get
			{
				if (this.destTilesPerDayDirty)
				{
					this.destTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.DestMassUsage, this.DestMassCapacity, this.caravan.Tile, (!this.caravan.pather.Moving) ? -1 : this.caravan.pather.nextTile, stringBuilder);
					this.cachedDestTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestTilesPerDay;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060033EE RID: 13294 RVA: 0x001BBDA0 File Offset: 0x001BA1A0
		private Pair<float, float> DestDaysWorthOfFood
		{
			get
			{
				if (this.destDaysWorthOfFoodDirty)
				{
					this.destDaysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.caravan.pather.Moving)
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
						second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
					}
					else
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3500);
						second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3500);
					}
					this.cachedDestDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedDestDaysWorthOfFood;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x001BBEDC File Offset: 0x001BA2DC
		private Pair<ThingDef, float> DestForagedFoodPerDay
		{
			get
			{
				if (this.destForagedFoodPerDayDirty)
				{
					this.destForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedDestForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestForagedFoodPerDay;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x060033F0 RID: 13296 RVA: 0x001BBF3C File Offset: 0x001BA33C
		private float DestVisibility
		{
			get
			{
				if (this.destVisibilityDirty)
				{
					this.destVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedDestVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestVisibility;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060033F1 RID: 13297 RVA: 0x001BBF90 File Offset: 0x001BA390
		private int TicksToArrive
		{
			get
			{
				int result;
				if (!this.caravan.pather.Moving)
				{
					result = 0;
				}
				else
				{
					if (this.ticksToArriveDirty)
					{
						this.ticksToArriveDirty = false;
						this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, false);
					}
					result = this.cachedTicksToArrive;
				}
				return result;
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x001BBFED File Offset: 0x001BA3ED
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x001BBFFC File Offset: 0x001BA3FC
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SplitCaravan".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.SourceMassUsage, this.SourceMassCapacity, this.cachedSourceMassCapacityExplanation, this.SourceTilesPerDay, this.cachedSourceTilesPerDayExplanation, this.SourceDaysWorthOfFood, this.SourceForagedFoodPerDay, this.cachedSourceForagedFoodPerDayExplanation, this.SourceVisibility, this.cachedSourceVisibilityExplanation), new CaravanUIUtility.CaravanInfo?(new CaravanUIUtility.CaravanInfo(this.DestMassUsage, this.DestMassCapacity, this.cachedDestMassCapacityExplanation, this.DestTilesPerDay, this.cachedDestTilesPerDayExplanation, this.DestDaysWorthOfFood, this.DestForagedFoodPerDay, this.cachedDestForagedFoodPerDayExplanation, this.DestVisibility, this.cachedDestVisibilityExplanation)), this.caravan.Tile, (!this.caravan.pather.Moving) ? null : new int?(this.TicksToArrive), -9999f, new Rect(12f, 35f, inRect.width - 24f, 40f), true, null, false);
			Dialog_SplitCaravan.tabsList.Clear();
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Pawns;
			}, this.tab == Dialog_SplitCaravan.Tab.Pawns));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Items;
			}, this.tab == Dialog_SplitCaravan.Tab.Items));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_SplitCaravan.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			Dialog_SplitCaravan.Tab tab = this.tab;
			if (tab != Dialog_SplitCaravan.Tab.Pawns)
			{
				if (tab == Dialog_SplitCaravan.Tab.Items)
				{
					this.itemsTransfer.OnGUI(inRect2, out flag);
				}
			}
			else
			{
				this.pawnsTransfer.OnGUI(inRect2, out flag);
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			GUI.EndGroup();
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x001BC25C File Offset: 0x001BA65C
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x001BC274 File Offset: 0x001BA674
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.Normal);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x001BC2B8 File Offset: 0x001BA6B8
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, false, true))
			{
				if (this.TrySplitCaravan())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
			}
			Rect rect3 = new Rect(rect2.x - 10f - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect3, "ResetButton".Translate(), true, false, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			Rect rect4 = new Rect(rect2.xMax + 10f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect4, "CancelButton".Translate(), true, false, true))
			{
				this.Close(true);
			}
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x001BC420 File Offset: 0x001BA820
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, "SplitCaravanThingCountTip".Translate(), IgnorePawnsInventoryMode.Ignore, () => this.DestMassCapacity - this.DestMassUsage, false, this.caravan.Tile, false);
			this.CountToTransferChanged();
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x001BC488 File Offset: 0x001BA888
		private bool TrySplitCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			bool result;
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < pawnsFromTransferables.Count; i++)
				{
					CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawnsFromTransferables[i], this.caravan.PawnsListForReading, pawnsFromTransferables);
				}
				for (int j = 0; j < pawnsFromTransferables.Count; j++)
				{
					this.caravan.RemovePawn(pawnsFromTransferables[j]);
				}
				Caravan newCaravan = CaravanMaker.MakeCaravan(pawnsFromTransferables, this.caravan.Faction, this.caravan.Tile, true);
				this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
				for (int k = 0; k < this.transferables.Count; k++)
				{
					TransferableUtility.TransferNoSplit(this.transferables[k].things, this.transferables[k].CountToTransfer, delegate(Thing thing, int numToTake)
					{
						Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
						if (ownerOf == null)
						{
							Log.Error("Error while splitting a caravan: Thing " + thing + " has no owner. Where did it come from then?", false);
						}
						else
						{
							CaravanInventoryUtility.MoveInventoryToSomeoneElse(ownerOf, thing, newCaravan.PawnsListForReading, null, numToTake);
						}
					}, true, true);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x001BC5D0 File Offset: 0x001BA9D0
		private bool CheckForErrors(List<Pawn> pawns)
		{
			bool result;
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else if (!this.AnyNonDownedColonistLeftInSourceCaravan(pawns))
			{
				Messages.Message("SourceCaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x001BC66C File Offset: 0x001BAA6C
		private void AddPawnsToTransferables()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				this.AddToTransferables(pawnsListForReading[i]);
			}
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x001BC6AC File Offset: 0x001BAAAC
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x001BC6EC File Offset: 0x001BAAEC
		private bool AnyNonDownedColonistLeftInSourceCaravan(List<Pawn> pawnsToTransfer)
		{
			return this.transferables.Any((TransferableOneWay x) => x.things.Any(delegate(Thing y)
			{
				Pawn pawn = y as Pawn;
				return pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.Downed && !pawnsToTransfer.Contains(pawn);
			}));
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x001BC728 File Offset: 0x001BAB28
		private void CountToTransferChanged()
		{
			this.sourceMassUsageDirty = true;
			this.sourceMassCapacityDirty = true;
			this.sourceTilesPerDayDirty = true;
			this.sourceDaysWorthOfFoodDirty = true;
			this.sourceForagedFoodPerDayDirty = true;
			this.sourceVisibilityDirty = true;
			this.destMassUsageDirty = true;
			this.destMassCapacityDirty = true;
			this.destTilesPerDayDirty = true;
			this.destDaysWorthOfFoodDirty = true;
			this.destForagedFoodPerDayDirty = true;
			this.destVisibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		// Token: 0x04001BE0 RID: 7136
		private Caravan caravan;

		// Token: 0x04001BE1 RID: 7137
		private List<TransferableOneWay> transferables;

		// Token: 0x04001BE2 RID: 7138
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04001BE3 RID: 7139
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04001BE4 RID: 7140
		private Dialog_SplitCaravan.Tab tab = Dialog_SplitCaravan.Tab.Pawns;

		// Token: 0x04001BE5 RID: 7141
		private bool sourceMassUsageDirty = true;

		// Token: 0x04001BE6 RID: 7142
		private float cachedSourceMassUsage;

		// Token: 0x04001BE7 RID: 7143
		private bool sourceMassCapacityDirty = true;

		// Token: 0x04001BE8 RID: 7144
		private float cachedSourceMassCapacity;

		// Token: 0x04001BE9 RID: 7145
		private string cachedSourceMassCapacityExplanation;

		// Token: 0x04001BEA RID: 7146
		private bool sourceTilesPerDayDirty = true;

		// Token: 0x04001BEB RID: 7147
		private float cachedSourceTilesPerDay;

		// Token: 0x04001BEC RID: 7148
		private string cachedSourceTilesPerDayExplanation;

		// Token: 0x04001BED RID: 7149
		private bool sourceDaysWorthOfFoodDirty = true;

		// Token: 0x04001BEE RID: 7150
		private Pair<float, float> cachedSourceDaysWorthOfFood;

		// Token: 0x04001BEF RID: 7151
		private bool sourceForagedFoodPerDayDirty = true;

		// Token: 0x04001BF0 RID: 7152
		private Pair<ThingDef, float> cachedSourceForagedFoodPerDay;

		// Token: 0x04001BF1 RID: 7153
		private string cachedSourceForagedFoodPerDayExplanation;

		// Token: 0x04001BF2 RID: 7154
		private bool sourceVisibilityDirty = true;

		// Token: 0x04001BF3 RID: 7155
		private float cachedSourceVisibility;

		// Token: 0x04001BF4 RID: 7156
		private string cachedSourceVisibilityExplanation;

		// Token: 0x04001BF5 RID: 7157
		private bool destMassUsageDirty = true;

		// Token: 0x04001BF6 RID: 7158
		private float cachedDestMassUsage;

		// Token: 0x04001BF7 RID: 7159
		private bool destMassCapacityDirty = true;

		// Token: 0x04001BF8 RID: 7160
		private float cachedDestMassCapacity;

		// Token: 0x04001BF9 RID: 7161
		private string cachedDestMassCapacityExplanation;

		// Token: 0x04001BFA RID: 7162
		private bool destTilesPerDayDirty = true;

		// Token: 0x04001BFB RID: 7163
		private float cachedDestTilesPerDay;

		// Token: 0x04001BFC RID: 7164
		private string cachedDestTilesPerDayExplanation;

		// Token: 0x04001BFD RID: 7165
		private bool destDaysWorthOfFoodDirty = true;

		// Token: 0x04001BFE RID: 7166
		private Pair<float, float> cachedDestDaysWorthOfFood;

		// Token: 0x04001BFF RID: 7167
		private bool destForagedFoodPerDayDirty = true;

		// Token: 0x04001C00 RID: 7168
		private Pair<ThingDef, float> cachedDestForagedFoodPerDay;

		// Token: 0x04001C01 RID: 7169
		private string cachedDestForagedFoodPerDayExplanation;

		// Token: 0x04001C02 RID: 7170
		private bool destVisibilityDirty = true;

		// Token: 0x04001C03 RID: 7171
		private float cachedDestVisibility;

		// Token: 0x04001C04 RID: 7172
		private string cachedDestVisibilityExplanation;

		// Token: 0x04001C05 RID: 7173
		private bool ticksToArriveDirty = true;

		// Token: 0x04001C06 RID: 7174
		private int cachedTicksToArrive;

		// Token: 0x04001C07 RID: 7175
		private const float TitleRectHeight = 35f;

		// Token: 0x04001C08 RID: 7176
		private const float BottomAreaHeight = 55f;

		// Token: 0x04001C09 RID: 7177
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04001C0A RID: 7178
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x020008DD RID: 2269
		private enum Tab
		{
			// Token: 0x04001C0E RID: 7182
			Pawns,
			// Token: 0x04001C0F RID: 7183
			Items
		}
	}
}
