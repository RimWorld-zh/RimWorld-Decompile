using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020007FC RID: 2044
	public class Dialog_LoadTransporters : Window
	{
		// Token: 0x04001816 RID: 6166
		private Map map;

		// Token: 0x04001817 RID: 6167
		private List<CompTransporter> transporters;

		// Token: 0x04001818 RID: 6168
		private List<TransferableOneWay> transferables;

		// Token: 0x04001819 RID: 6169
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x0400181A RID: 6170
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x0400181B RID: 6171
		private Dialog_LoadTransporters.Tab tab = Dialog_LoadTransporters.Tab.Pawns;

		// Token: 0x0400181C RID: 6172
		private float lastMassFlashTime = -9999f;

		// Token: 0x0400181D RID: 6173
		private bool massUsageDirty = true;

		// Token: 0x0400181E RID: 6174
		private float cachedMassUsage;

		// Token: 0x0400181F RID: 6175
		private bool tilesPerDayDirty = true;

		// Token: 0x04001820 RID: 6176
		private float cachedTilesPerDay;

		// Token: 0x04001821 RID: 6177
		private string cachedTilesPerDayExplanation;

		// Token: 0x04001822 RID: 6178
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04001823 RID: 6179
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x04001824 RID: 6180
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x04001825 RID: 6181
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x04001826 RID: 6182
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04001827 RID: 6183
		private bool visibilityDirty = true;

		// Token: 0x04001828 RID: 6184
		private float cachedVisibility;

		// Token: 0x04001829 RID: 6185
		private string cachedVisibilityExplanation;

		// Token: 0x0400182A RID: 6186
		private const float TitleRectHeight = 35f;

		// Token: 0x0400182B RID: 6187
		private const float BottomAreaHeight = 55f;

		// Token: 0x0400182C RID: 6188
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400182D RID: 6189
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x06002D89 RID: 11657 RVA: 0x0017F7A0 File Offset: 0x0017DBA0
		public Dialog_LoadTransporters(Map map, List<CompTransporter> transporters)
		{
			this.map = map;
			this.transporters = new List<CompTransporter>();
			this.transporters.AddRange(transporters);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002D8A RID: 11658 RVA: 0x0017F82C File Offset: 0x0017DC2C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002D8B RID: 11659 RVA: 0x0017F854 File Offset: 0x0017DC54
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002D8C RID: 11660 RVA: 0x0017F870 File Offset: 0x0017DC70
		private float MassCapacity
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.transporters.Count; i++)
				{
					num += this.transporters[i].Props.massCapacity;
				}
				return num;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002D8D RID: 11661 RVA: 0x0017F8C4 File Offset: 0x0017DCC4
		private string TransportersLabel
		{
			get
			{
				return Find.ActiveLanguageWorker.Pluralize(this.transporters[0].parent.Label, -1);
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002D8E RID: 11662 RVA: 0x0017F8FC File Offset: 0x0017DCFC
		private string TransportersLabelCap
		{
			get
			{
				return this.TransportersLabel.CapitalizeFirst();
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002D8F RID: 11663 RVA: 0x0017F91C File Offset: 0x0017DD1C
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002D90 RID: 11664 RVA: 0x0017F93C File Offset: 0x0017DD3C
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, false);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002D91 RID: 11665 RVA: 0x0017F980 File Offset: 0x0017DD80
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.map.Tile, -1, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002D92 RID: 11666 RVA: 0x0017F9EC File Offset: 0x0017DDEC
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, Faction.OfPlayer, null, 0f, 3500);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, null, 0f, 3500));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002D93 RID: 11667 RVA: 0x0017FA74 File Offset: 0x0017DE74
		private Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x0017FAD4 File Offset: 0x0017DED4
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x0017FB27 File Offset: 0x0017DF27
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x0017FB38 File Offset: 0x0017DF38
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "LoadTransporters".Translate(new object[]
			{
				this.TransportersLabel
			}));
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, "", this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation), null, this.map.Tile, null, this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), false, null, false);
			Dialog_LoadTransporters.tabsList.Clear();
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_LoadTransporters.Tab.Pawns;
			}, this.tab == Dialog_LoadTransporters.Tab.Pawns));
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_LoadTransporters.Tab.Items;
			}, this.tab == Dialog_LoadTransporters.Tab.Items));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_LoadTransporters.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			Dialog_LoadTransporters.Tab tab = this.tab;
			if (tab != Dialog_LoadTransporters.Tab.Pawns)
			{
				if (tab == Dialog_LoadTransporters.Tab.Items)
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

		// Token: 0x06002D97 RID: 11671 RVA: 0x0017FD48 File Offset: 0x0017E148
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x0017FD60 File Offset: 0x0017E160
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x0017FDA4 File Offset: 0x0017E1A4
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, false, true))
			{
				if (this.TryAccept())
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
			if (Prefs.DevMode)
			{
				float width = 200f;
				float num = this.BottomButtonSize.y / 2f;
				Rect rect5 = new Rect(0f, rect.height - 55f, width, num);
				if (Widgets.ButtonText(rect5, "Dev: Load instantly", true, false, true))
				{
					if (this.DebugTryLoadInstantly())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
				Rect rect6 = new Rect(0f, rect.height - 55f + num, width, num);
				if (Widgets.ButtonText(rect6, "Dev: Select everything", true, false, true))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToLoadEverything();
				}
			}
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x0017FFD0 File Offset: 0x0017E3D0
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			IEnumerable<TransferableOneWay> enumerable = null;
			string text = null;
			string destinationLabel = null;
			string text2 = "FormCaravanColonyThingCountTip".Translate();
			bool flag = true;
			IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload;
			bool flag2 = true;
			Func<float> availableMassGetter = () => this.MassCapacity - this.MassUsage;
			int tile = this.map.Tile;
			this.pawnsTransfer = new TransferableOneWayWidget(enumerable, text, destinationLabel, text2, flag, ignorePawnInventoryMass, flag2, availableMassGetter, 0f, false, tile, true, true, true, false, true, false, false);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			enumerable = from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x;
			text2 = null;
			destinationLabel = null;
			text = "FormCaravanColonyThingCountTip".Translate();
			flag2 = true;
			ignorePawnInventoryMass = IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload;
			flag = true;
			availableMassGetter = (() => this.MassCapacity - this.MassUsage);
			tile = this.map.Tile;
			this.itemsTransfer = new TransferableOneWayWidget(enumerable, text2, destinationLabel, text, flag2, ignorePawnInventoryMass, flag, availableMassGetter, 0f, false, tile, true, false, false, true, false, true, false);
			this.CountToTransferChanged();
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x001800EC File Offset: 0x0017E4EC
		private bool DebugTryLoadInstantly()
		{
			this.CreateAndAssignNewTransportersGroup();
			int i;
			for (i = 0; i < this.transferables.Count; i++)
			{
				TransferableUtility.Transfer(this.transferables[i].things, this.transferables[i].CountToTransfer, delegate(Thing splitPiece, IThingHolder originalThing)
				{
					this.transporters[i % this.transporters.Count].GetDirectlyHeldThings().TryAdd(splitPiece, true);
				});
			}
			return true;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x00180188 File Offset: 0x0017E588
		private bool TryAccept()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			bool result;
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				result = false;
			}
			else
			{
				int transportersGroup = this.CreateAndAssignNewTransportersGroup();
				this.AssignTransferablesToRandomTransporters();
				IEnumerable<Pawn> enumerable = from x in pawnsFromTransferables
				where x.IsColonist && !x.Downed
				select x;
				if (enumerable.Any<Pawn>())
				{
					foreach (Pawn pawn in enumerable)
					{
						Lord lord = pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord);
						}
					}
					LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_LoadAndEnterTransporters(transportersGroup), this.map, enumerable);
					foreach (Pawn pawn2 in enumerable)
					{
						if (pawn2.Spawned)
						{
							pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
				}
				Messages.Message("MessageTransportersLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				result = true;
			}
			return result;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x00180300 File Offset: 0x0017E700
		private void AssignTransferablesToRandomTransporters()
		{
			TransferableOneWay transferableOneWay = this.transferables.MaxBy((TransferableOneWay x) => x.CountToTransfer);
			int num = 0;
			for (int i = 0; i < this.transferables.Count; i++)
			{
				if (this.transferables[i] != transferableOneWay)
				{
					if (this.transferables[i].CountToTransfer > 0)
					{
						this.transporters[num % this.transporters.Count].AddToTheToLoadList(this.transferables[i], this.transferables[i].CountToTransfer);
						num++;
					}
				}
			}
			if (num < this.transporters.Count)
			{
				int num2 = transferableOneWay.CountToTransfer;
				int num3 = num2 / (this.transporters.Count - num);
				for (int j = num; j < this.transporters.Count; j++)
				{
					int num4 = (j != this.transporters.Count - 1) ? num3 : num2;
					if (num4 > 0)
					{
						this.transporters[j].AddToTheToLoadList(transferableOneWay, num4);
					}
					num2 -= num4;
				}
			}
			else
			{
				this.transporters[num % this.transporters.Count].AddToTheToLoadList(transferableOneWay, transferableOneWay.CountToTransfer);
			}
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x0018047C File Offset: 0x0017E87C
		private int CreateAndAssignNewTransportersGroup()
		{
			int nextTransporterGroupID = Find.UniqueIDsManager.GetNextTransporterGroupID();
			for (int i = 0; i < this.transporters.Count; i++)
			{
				this.transporters[i].groupID = nextTransporterGroupID;
			}
			return nextTransporterGroupID;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x001804D0 File Offset: 0x0017E8D0
		private bool CheckForErrors(List<Pawn> pawns)
		{
			bool result;
			if (!this.transferables.Any((TransferableOneWay x) => x.CountToTransfer != 0))
			{
				Messages.Message("CantSendEmptyTransportPods".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else if (this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigTransportersMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else
			{
				Pawn pawn = pawns.Find((Pawn x) => !x.MapHeld.reachability.CanReach(x.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)));
				if (pawn != null)
				{
					Messages.Message("PawnCantReachTransporters".Translate(new object[]
					{
						pawn.LabelShort
					}).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					result = false;
				}
				else
				{
					Map map = this.transporters[0].parent.Map;
					for (int i = 0; i < this.transferables.Count; i++)
					{
						if (this.transferables[i].ThingDef.category == ThingCategory.Item)
						{
							int countToTransfer = this.transferables[i].CountToTransfer;
							int num = 0;
							if (countToTransfer > 0)
							{
								for (int j = 0; j < this.transferables[i].things.Count; j++)
								{
									Thing thing = this.transferables[i].things[j];
									if (map.reachability.CanReach(thing.Position, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
									{
										num += thing.stackCount;
										if (num >= countToTransfer)
										{
											break;
										}
									}
								}
								if (num < countToTransfer)
								{
									if (countToTransfer == 1)
									{
										Messages.Message("TransporterItemIsUnreachableSingle".Translate(new object[]
										{
											this.transferables[i].ThingDef.label
										}), MessageTypeDefOf.RejectInput, false);
									}
									else
									{
										Messages.Message("TransporterItemIsUnreachableMulti".Translate(new object[]
										{
											countToTransfer,
											this.transferables[i].ThingDef.label
										}), MessageTypeDefOf.RejectInput, false);
									}
									return false;
								}
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x00180750 File Offset: 0x0017EB50
		private void AddPawnsToTransferables()
		{
			List<Pawn> list = CaravanFormingUtility.AllSendablePawns(this.map, false, false, false);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x00180794 File Offset: 0x0017EB94
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, false, false, false);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x001807D7 File Offset: 0x0017EBD7
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x001807E8 File Offset: 0x0017EBE8
		private void SetToLoadEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x0018083C File Offset: 0x0017EC3C
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x020007FD RID: 2045
		private enum Tab
		{
			// Token: 0x04001833 RID: 6195
			Pawns,
			// Token: 0x04001834 RID: 6196
			Items
		}
	}
}
