using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	public class Dialog_SplitCaravan : Window
	{
		private enum Tab
		{
			Pawns,
			Items
		}

		private Caravan caravan;

		private List<TransferableOneWay> transferables;

		private TransferableOneWayWidget pawnsTransfer;

		private TransferableOneWayWidget itemsTransfer;

		private Tab tab;

		private float lastSourceMassFlashTime = -9999f;

		private float lastDestMassFlashTime = -9999f;

		private bool sourceMassUsageDirty = true;

		private float cachedSourceMassUsage;

		private bool sourceMassCapacityDirty = true;

		private float cachedSourceMassCapacity;

		private bool sourceDaysWorthOfFoodDirty = true;

		private Pair<float, float> cachedSourceDaysWorthOfFood;

		private bool destMassUsageDirty = true;

		private float cachedDestMassUsage;

		private bool destMassCapacityDirty = true;

		private float cachedDestMassCapacity;

		private bool destDaysWorthOfFoodDirty = true;

		private Pair<float, float> cachedDestDaysWorthOfFood;

		private const float TitleRectHeight = 40f;

		private const float BottomAreaHeight = 55f;

		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		private static List<TabRecord> tabsList = new List<TabRecord>();

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		private bool EnvironmentAllowsEatingVirtualPlantsNow
		{
			get
			{
				return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsNowAt(this.caravan.Tile);
			}
		}

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

		private float SourceMassCapacity
		{
			get
			{
				if (this.sourceMassCapacityDirty)
				{
					this.sourceMassCapacityDirty = false;
					this.cachedSourceMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTransfer(this.transferables);
				}
				return this.cachedSourceMassCapacity;
			}
		}

		private Pair<float, float> SourceDaysWorthOfFood
		{
			get
			{
				if (this.sourceDaysWorthOfFoodDirty)
				{
					this.sourceDaysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.EnvironmentAllowsEatingVirtualPlantsNow, IgnorePawnsInventoryMode.Ignore);
					this.cachedSourceDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedSourceDaysWorthOfFood;
			}
		}

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

		private float DestMassCapacity
		{
			get
			{
				if (this.destMassCapacityDirty)
				{
					this.destMassCapacityDirty = false;
					this.cachedDestMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables);
				}
				return this.cachedDestMassCapacity;
			}
		}

		private Pair<float, float> DestDaysWorthOfFood
		{
			get
			{
				if (this.destDaysWorthOfFoodDirty)
				{
					this.destDaysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.EnvironmentAllowsEatingVirtualPlantsNow, IgnorePawnsInventoryMode.Ignore);
					this.cachedDestDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedDestDaysWorthOfFood;
			}
		}

		public Dialog_SplitCaravan(Caravan caravan)
		{
			this.caravan = caravan;
			base.closeOnEscapeKey = true;
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
		}

		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 40f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SplitCaravan".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Dialog_SplitCaravan.tabsList.Clear();
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate
			{
				this.tab = Tab.Pawns;
			}, this.tab == Tab.Pawns));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate
			{
				this.tab = Tab.Items;
			}, this.tab == Tab.Items));
			inRect.yMin += 72f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_SplitCaravan.tabsList);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			Rect rect3 = rect2;
			rect3.y += 32f;
			rect3.xMin += (float)(rect2.width - 515.0);
			this.DrawMassAndFoodInfo(rect3);
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			switch (this.tab)
			{
			case Tab.Pawns:
				this.pawnsTransfer.OnGUI(inRect2, out flag);
				break;
			case Tab.Items:
				this.itemsTransfer.OnGUI(inRect2, out flag);
				break;
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			GUI.EndGroup();
		}

		public override bool CausesMessageBackground()
		{
			return true;
		}

		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching(t, this.transferables);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			transferableOneWay.things.Add(t);
		}

		private void DrawMassAndFoodInfo(Rect rect)
		{
			TransferableUIUtility.DrawMassInfo(rect, this.SourceMassUsage, this.SourceMassCapacity, "SplitCaravanMassUsageTooltip".Translate(), this.lastSourceMassFlashTime, false);
			CaravanUIUtility.DrawDaysWorthOfFoodInfo(new Rect(rect.x, (float)(rect.y + 19.0), rect.width, rect.height), this.SourceDaysWorthOfFood.First, this.SourceDaysWorthOfFood.Second, this.EnvironmentAllowsEatingVirtualPlantsNow, false, 3.40282347E+38f);
			TransferableUIUtility.DrawMassInfo(rect, this.DestMassUsage, this.DestMassCapacity, "SplitCaravanMassUsageTooltip".Translate(), this.lastDestMassFlashTime, true);
			CaravanUIUtility.DrawDaysWorthOfFoodInfo(new Rect(rect.x, (float)(rect.y + 19.0), rect.width, rect.height), this.DestDaysWorthOfFood.First, this.DestDaysWorthOfFood.Second, this.EnvironmentAllowsEatingVirtualPlantsNow, true, 3.40282347E+38f);
		}

		private void DoBottomButtons(Rect rect)
		{
			double num = rect.width / 2.0;
			Vector2 bottomButtonSize = this.BottomButtonSize;
			double x = num - bottomButtonSize.x / 2.0;
			double y = rect.height - 55.0;
			Vector2 bottomButtonSize2 = this.BottomButtonSize;
			float x2 = bottomButtonSize2.x;
			Vector2 bottomButtonSize3 = this.BottomButtonSize;
			Rect rect2 = new Rect((float)x, (float)y, x2, bottomButtonSize3.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, false, true) && this.TrySplitCaravan())
			{
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				this.Close(false);
			}
			double num2 = rect2.x - 10.0;
			Vector2 bottomButtonSize4 = this.BottomButtonSize;
			double x3 = num2 - bottomButtonSize4.x;
			float y2 = rect2.y;
			Vector2 bottomButtonSize5 = this.BottomButtonSize;
			float x4 = bottomButtonSize5.x;
			Vector2 bottomButtonSize6 = this.BottomButtonSize;
			Rect rect3 = new Rect((float)x3, y2, x4, bottomButtonSize6.y);
			if (Widgets.ButtonText(rect3, "ResetButton".Translate(), true, false, true))
			{
				SoundDefOf.TickLow.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			double x5 = rect2.xMax + 10.0;
			float y3 = rect2.y;
			Vector2 bottomButtonSize7 = this.BottomButtonSize;
			float x6 = bottomButtonSize7.x;
			Vector2 bottomButtonSize8 = this.BottomButtonSize;
			Rect rect4 = new Rect((float)x5, y3, x6, bottomButtonSize8.y);
			if (Widgets.ButtonText(rect4, "CancelButton".Translate(), true, false, true))
			{
				this.Close(true);
			}
		}

		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, "CaravanSplitSourceLabel".Translate(), "CaravanSplitDestLabel".Translate(), "SplitCaravanThingCountTip".Translate(), IgnorePawnsInventoryMode.Ignore, (Func<float>)(() => this.DestMassCapacity - this.DestMassUsage), false, this.caravan.Tile);
			this.CountToTransferChanged();
		}

		private bool TrySplitCaravan()
		{
			List<Pawn> pawns = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawns))
			{
				return false;
			}
			for (int i = 0; i < pawns.Count; i++)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawns[i], this.caravan.PawnsListForReading, pawns);
			}
			Caravan caravan = (Caravan)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Caravan);
			caravan.Tile = this.caravan.Tile;
			caravan.SetFaction(this.caravan.Faction);
			caravan.Name = CaravanNameGenerator.GenerateCaravanName(caravan);
			Find.WorldObjects.Add(caravan);
			for (int j = 0; j < pawns.Count; j++)
			{
				this.caravan.RemovePawn(pawns[j]);
				caravan.AddPawn(pawns[j], true);
			}
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			for (int k = 0; k < this.transferables.Count; k++)
			{
				TransferableUtility.TransferNoSplit(this.transferables[k].things, this.transferables[k].CountToTransfer, delegate(Thing thing, int numToTake)
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Error while splitting a caravan: Thing " + thing + " has no owner. Where did it come from then?");
					}
					else
					{
						CaravanInventoryUtility.MoveInventoryToSomeoneElse(ownerOf, thing, pawns, null, numToTake);
					}
				}, true, true);
			}
			return true;
		}

		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput);
				return false;
			}
			if (!this.AnyNonDownedColonistLeftInSourceCaravan(pawns))
			{
				Messages.Message("SourceCaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput);
				return false;
			}
			return true;
		}

		private void AddPawnsToTransferables()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				this.AddToTransferables(pawnsListForReading[i]);
			}
		}

		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		private void FlashSourceMass()
		{
			this.lastSourceMassFlashTime = Time.time;
		}

		private void FlashDestMass()
		{
			this.lastDestMassFlashTime = Time.time;
		}

		private bool AnyNonDownedColonistLeftInSourceCaravan(List<Pawn> pawnsToTransfer)
		{
			return this.transferables.Any((TransferableOneWay x) => x.things.Any(delegate(Thing y)
			{
				Pawn pawn = y as Pawn;
				return pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.Downed && !pawnsToTransfer.Contains(pawn);
			}));
		}

		private void CountToTransferChanged()
		{
			this.sourceMassUsageDirty = true;
			this.sourceMassCapacityDirty = true;
			this.sourceDaysWorthOfFoodDirty = true;
			this.destMassUsageDirty = true;
			this.destMassCapacityDirty = true;
			this.destDaysWorthOfFoodDirty = true;
		}
	}
}
