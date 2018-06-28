using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class Dialog_FormCaravan : Window
	{
		private Map map;

		private bool reform;

		private Action onClosed;

		private bool canChooseRoute;

		private bool mapAboutToBeRemoved;

		public bool choosingRoute;

		private bool thisWindowInstanceEverOpened;

		public List<TransferableOneWay> transferables;

		private TransferableOneWayWidget pawnsTransfer;

		private TransferableOneWayWidget itemsTransfer;

		private Dialog_FormCaravan.Tab tab = Dialog_FormCaravan.Tab.Pawns;

		private float lastMassFlashTime = -9999f;

		private int startingTile = -1;

		private int destinationTile = -1;

		private bool massUsageDirty = true;

		private float cachedMassUsage;

		private bool massCapacityDirty = true;

		private float cachedMassCapacity;

		private string cachedMassCapacityExplanation;

		private bool tilesPerDayDirty = true;

		private float cachedTilesPerDay;

		private string cachedTilesPerDayExplanation;

		private bool daysWorthOfFoodDirty = true;

		private Pair<float, float> cachedDaysWorthOfFood;

		private bool foragedFoodPerDayDirty = true;

		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		private string cachedForagedFoodPerDayExplanation;

		private bool visibilityDirty = true;

		private float cachedVisibility;

		private string cachedVisibilityExplanation;

		private bool ticksToArriveDirty = true;

		private int cachedTicksToArrive;

		private const float TitleRectHeight = 35f;

		private const float BottomAreaHeight = 55f;

		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		private const float MaxDaysWorthOfFoodToShowWarningDialog = 5f;

		private static List<TabRecord> tabsList = new List<TabRecord>();

		private static List<Thing> tmpPackingSpots = new List<Thing>();

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<string, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<TransferableOneWay> <>f__am$cache4;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache6;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache7;

		public Dialog_FormCaravan(Map map, bool reform = false, Action onClosed = null, bool mapAboutToBeRemoved = false)
		{
			this.map = map;
			this.reform = reform;
			this.onClosed = onClosed;
			this.mapAboutToBeRemoved = mapAboutToBeRemoved;
			this.canChooseRoute = (!reform || !map.retainedCaravanData.HasDestinationTile);
			this.closeOnAccept = !reform;
			this.closeOnCancel = !reform;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		public int CurrentTile
		{
			get
			{
				return this.map.Tile;
			}
		}

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

		private bool AutoStripSpawnedCorpses
		{
			get
			{
				return this.reform;
			}
		}

		private bool ListPlayerPawnsInventorySeparately
		{
			get
			{
				return this.reform;
			}
		}

		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		private bool MustChooseRoute
		{
			get
			{
				return this.canChooseRoute && (!this.reform || this.map.Parent is Settlement);
			}
		}

		private bool ShowCancelButton
		{
			get
			{
				bool result;
				if (!this.mapAboutToBeRemoved)
				{
					result = true;
				}
				else
				{
					bool flag = false;
					for (int i = 0; i < this.transferables.Count; i++)
					{
						Pawn pawn = this.transferables[i].AnyThing as Pawn;
						if (pawn != null && pawn.IsColonist && !pawn.Downed)
						{
							flag = true;
							break;
						}
					}
					result = !flag;
				}
				return result;
			}
		}

		private IgnorePawnsInventoryMode IgnoreInventoryMode
		{
			get
			{
				return (!this.ListPlayerPawnsInventorySeparately) ? IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload : IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn;
			}
		}

		public float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					List<TransferableOneWay> list = this.transferables;
					IgnorePawnsInventoryMode ignoreInventoryMode = this.IgnoreInventoryMode;
					bool autoStripSpawnedCorpses = this.AutoStripSpawnedCorpses;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(list, ignoreInventoryMode, false, autoStripSpawnedCorpses);
				}
				return this.cachedMassUsage;
			}
		}

		public float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedMassCapacity;
			}
		}

		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.CurrentTile, this.startingTile, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.destinationTile != -1)
					{
						using (WorldPath worldPath = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null))
						{
							int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
							first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, worldPath, 0f, ticksPerMove);
							second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, worldPath, 0f, ticksPerMove);
						}
					}
					else
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, null, 0f, 3500);
						second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, null, 0f, 3500);
					}
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedDaysWorthOfFood;
			}
		}

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

		private int TicksToArrive
		{
			get
			{
				int result;
				if (this.destinationTile == -1)
				{
					result = 0;
				}
				else
				{
					if (this.ticksToArriveDirty)
					{
						this.ticksToArriveDirty = false;
						using (WorldPath worldPath = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null))
						{
							this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.CurrentTile, this.destinationTile, worldPath, 0f, CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null), Find.TickManager.TicksAbs);
						}
					}
					result = this.cachedTicksToArrive;
				}
				return result;
			}
		}

		private bool MostFoodWillRotSoon
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					TransferableOneWay transferableOneWay = this.transferables[i];
					if (transferableOneWay.HasAnyThing && transferableOneWay.CountToTransfer > 0 && transferableOneWay.ThingDef.IsNutritionGivingIngestible && !(transferableOneWay.AnyThing is Corpse))
					{
						float num3 = 600f;
						CompRottable compRottable = transferableOneWay.AnyThing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							num3 = (float)DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.CurrentTile, null) / 60000f;
						}
						float num4 = transferableOneWay.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)transferableOneWay.CountToTransfer;
						if (num3 < 5f)
						{
							num += num4;
						}
						else
						{
							num2 += num4;
						}
					}
				}
				return (num != 0f || num2 != 0f) && num / (num + num2) >= 0.75f;
			}
		}

		public override void PostOpen()
		{
			base.PostOpen();
			this.choosingRoute = false;
			if (!this.thisWindowInstanceEverOpened)
			{
				this.thisWindowInstanceEverOpened = true;
				this.CalculateAndRecacheTransferables();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.FormCaravan, KnowledgeAmount.Total);
			}
		}

		public override void PostClose()
		{
			base.PostClose();
			if (this.onClosed != null && !this.choosingRoute)
			{
				this.onClosed();
			}
		}

		public void Notify_NoLongerChoosingRoute()
		{
			this.choosingRoute = false;
			if (!Find.WindowStack.IsOpen(this) && this.onClosed != null)
			{
				this.onClosed();
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, ((!this.reform) ? "FormCaravan" : "ReformCaravan").Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.CaravanInfo info = new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation);
			CaravanUIUtility.CaravanInfo? info2 = null;
			int currentTile = this.CurrentTile;
			int? ticksToArrive = (this.destinationTile != -1) ? new int?(this.TicksToArrive) : null;
			float num = this.lastMassFlashTime;
			Rect rect2 = new Rect(12f, 35f, inRect.width - 24f, 40f);
			string extraDaysWorthOfFoodTipInfo = (this.destinationTile != -1) ? "DaysWorthOfFoodTooltip_OnlyFirstWaypoint".Translate() : null;
			CaravanUIUtility.DrawCaravanInfo(info, info2, currentTile, ticksToArrive, num, rect2, true, extraDaysWorthOfFoodTipInfo, false);
			Dialog_FormCaravan.tabsList.Clear();
			Dialog_FormCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Pawns;
			}, this.tab == Dialog_FormCaravan.Tab.Pawns));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Items;
			}, this.tab == Dialog_FormCaravan.Tab.Items));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_FormCaravan.tabsList, 200f);
			Dialog_FormCaravan.tabsList.Clear();
			inRect = inRect.ContractedBy(17f);
			inRect.height += 17f;
			GUI.BeginGroup(inRect);
			Rect rect3 = inRect.AtZero();
			this.DoBottomButtons(rect3);
			Rect inRect2 = rect3;
			inRect2.yMax -= 76f;
			bool flag = false;
			Dialog_FormCaravan.Tab tab = this.tab;
			if (tab != Dialog_FormCaravan.Tab.Pawns)
			{
				if (tab == Dialog_FormCaravan.Tab.Items)
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

		public override bool CausesMessageBackground()
		{
			return true;
		}

		public void Notify_ChoseRoute(int destinationTile)
		{
			this.destinationTile = destinationTile;
			this.startingTile = CaravanExitMapUtility.BestExitTileToGoTo(destinationTile, this.map);
			this.ticksToArriveDirty = true;
			this.daysWorthOfFoodDirty = true;
			Messages.Message("MessageChoseRoute".Translate(), MessageTypeDefOf.CautionInput, false);
		}

		private void AddToTransferables(Thing t, bool setToTransferMax = false)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			transferableOneWay.things.Add(t);
			if (setToTransferMax)
			{
				transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + t.stackCount);
			}
		}

		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, false, true))
			{
				if (this.reform)
				{
					if (this.TryReformCaravan())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
				else
				{
					List<string> list = new List<string>();
					Pair<float, float> daysWorthOfFood = this.DaysWorthOfFood;
					if (daysWorthOfFood.First < 5f)
					{
						list.Add((daysWorthOfFood.First >= 0.1f) ? "DaysWorthOfFoodWarningDialog".Translate(new object[]
						{
							daysWorthOfFood.First.ToString("0.#")
						}) : "DaysWorthOfFoodWarningDialog_NoFood".Translate());
					}
					else if (this.MostFoodWillRotSoon)
					{
						list.Add("CaravanFoodWillRotSoonWarningDialog".Translate());
					}
					if (!TransferableUtility.GetPawnsFromTransferables(this.transferables).Any((Pawn pawn) => CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled))
					{
						list.Add("CaravanIncapableOfSocial".Translate());
					}
					if (list.Count > 0)
					{
						if (this.CheckForErrors(TransferableUtility.GetPawnsFromTransferables(this.transferables)))
						{
							string text = string.Concat((from str in list
							select str + "\n\n").ToArray<string>()) + "CaravanAreYouSure".Translate();
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, delegate
							{
								if (this.TryFormAndSendCaravan())
								{
									this.Close(false);
								}
							}, false, null));
						}
					}
					else if (this.TryFormAndSendCaravan())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
			}
			Rect rect3 = new Rect(rect2.x - 10f - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect3, "ResetButton".Translate(), true, false, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (this.ShowCancelButton)
			{
				Rect rect4 = new Rect(rect2.xMax + 10f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
				if (Widgets.ButtonText(rect4, "CancelButton".Translate(), true, false, true))
				{
					this.Close(true);
				}
			}
			if (this.canChooseRoute)
			{
				Rect rect5 = new Rect(rect.width - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
				if (Widgets.ButtonText(rect5, "ChooseRouteButton".Translate(), true, false, true))
				{
					List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
					if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
					{
						Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Find.WorldRoutePlanner.Start(this);
					}
				}
				if (this.destinationTile != -1)
				{
					Rect rect6 = rect5;
					rect6.y += rect5.height + 4f;
					rect6.height = 200f;
					rect6.xMin -= 200f;
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect6, "CaravanEstimatedDaysToDestination".Translate(new object[]
					{
						((float)this.TicksToArrive / 60000f).ToString("0.#")
					}));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (Prefs.DevMode)
			{
				float width = 200f;
				float num = this.BottomButtonSize.y / 2f;
				Rect rect7 = new Rect(0f, rect.height - 55f - 17f, width, num);
				if (Widgets.ButtonText(rect7, "Dev: Send instantly", true, false, true))
				{
					if (this.DebugTryFormCaravanInstantly())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
				Rect rect8 = new Rect(0f, rect.height - 55f - 17f + num, width, num);
				if (Widgets.ButtonText(rect8, "Dev: Select everything", true, false, true))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToSendEverything();
				}
			}
		}

		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, "FormCaravanColonyThingCountTip".Translate(), this.IgnoreInventoryMode, () => this.MassCapacity - this.MassUsage, this.AutoStripSpawnedCorpses, this.CurrentTile, this.mapAboutToBeRemoved);
			this.CountToTransferChanged();
		}

		private bool DebugTryFormCaravanInstantly()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			bool result;
			if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else
			{
				this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
				int num = this.startingTile;
				if (num < 0)
				{
					num = CaravanExitMapUtility.RandomBestExitTileFrom(this.map);
				}
				if (num < 0)
				{
					num = this.CurrentTile;
				}
				CaravanFormingUtility.FormAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, num, this.destinationTile);
				result = true;
			}
			return result;
		}

		private bool TryFormAndSendCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			bool result;
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				result = false;
			}
			else
			{
				Direction8Way direction8WayFromTo = Find.WorldGrid.GetDirection8WayFromTo(this.CurrentTile, this.startingTile);
				IntVec3 intVec;
				if (!this.TryFindExitSpot(pawnsFromTransferables, true, out intVec))
				{
					if (!this.TryFindExitSpot(pawnsFromTransferables, false, out intVec))
					{
						Messages.Message("CaravanCouldNotFindExitSpot".Translate(new object[]
						{
							direction8WayFromTo.LabelShort()
						}), MessageTypeDefOf.RejectInput, false);
						return false;
					}
					Messages.Message("CaravanCouldNotFindReachableExitSpot".Translate(new object[]
					{
						direction8WayFromTo.LabelShort()
					}), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.CautionInput, false);
				}
				IntVec3 meetingPoint;
				if (!this.TryFindRandomPackingSpot(intVec, out meetingPoint))
				{
					Messages.Message("CaravanCouldNotFindPackingSpot".Translate(new object[]
					{
						direction8WayFromTo.LabelShort()
					}), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.RejectInput, false);
					result = false;
				}
				else
				{
					CaravanFormingUtility.StartFormingCaravan(pawnsFromTransferables, Faction.OfPlayer, this.transferables, meetingPoint, intVec, this.startingTile, this.destinationTile);
					Messages.Message("CaravanFormationProcessStarted".Translate(), pawnsFromTransferables[0], MessageTypeDefOf.PositiveEvent, false);
					result = true;
				}
			}
			return result;
		}

		private bool TryReformCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			bool result;
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				result = false;
			}
			else
			{
				this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
				Caravan caravan = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, this.CurrentTile, this.destinationTile, false);
				this.map.Parent.CheckRemoveMapNow();
				string text = "MessageReformedCaravan".Translate();
				if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						" ",
						"MessageFormedCaravan_Orders".Translate(),
						": ",
						caravan.pather.ArrivalAction.Label,
						"."
					});
				}
				Messages.Message(text, caravan, MessageTypeDefOf.TaskCompletion, false);
				result = true;
			}
			return result;
		}

		private void AddItemsFromTransferablesToRandomInventories(List<Pawn> pawns)
		{
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			if (this.ListPlayerPawnsInventorySeparately)
			{
				for (int i = 0; i < pawns.Count; i++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(pawns[i]))
					{
						ThingOwner<Thing> innerContainer = pawns[i].inventory.innerContainer;
						for (int j = innerContainer.Count - 1; j >= 0; j--)
						{
							this.RemoveCarriedItemFromTransferablesOrDrop(innerContainer[j], pawns[i], this.transferables);
						}
					}
				}
				for (int k = 0; k < this.transferables.Count; k++)
				{
					if (this.transferables[k].things.Any((Thing x) => !x.Spawned))
					{
						this.transferables[k].things.SortBy((Thing x) => x.Spawned);
					}
				}
			}
			for (int l = 0; l < this.transferables.Count; l++)
			{
				if (!(this.transferables[l].AnyThing is Corpse))
				{
					TransferableUtility.Transfer(this.transferables[l].things, this.transferables[l].CountToTransfer, delegate(Thing splitPiece, IThingHolder originalHolder)
					{
						Thing item = splitPiece.TryMakeMinified();
						CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.innerContainer.TryAdd(item, true);
					});
				}
			}
			for (int m = 0; m < this.transferables.Count; m++)
			{
				if (this.transferables[m].AnyThing is Corpse)
				{
					TransferableUtility.TransferNoSplit(this.transferables[m].things, this.transferables[m].CountToTransfer, delegate(Thing originalThing, int numToTake)
					{
						if (this.AutoStripSpawnedCorpses)
						{
							Corpse corpse = originalThing as Corpse;
							if (corpse != null && corpse.Spawned)
							{
								corpse.Strip();
							}
						}
						Thing item = originalThing.SplitOff(numToTake);
						CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.innerContainer.TryAdd(item, true);
					}, true, true);
				}
			}
		}

		private bool CheckForErrors(List<Pawn> pawns)
		{
			bool result;
			if (this.MustChooseRoute && this.destinationTile < 0)
			{
				Messages.Message("MessageMustChooseRouteFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else if (!this.reform && this.startingTile < 0)
			{
				Messages.Message("MessageNoValidExitTile".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else if (!this.reform && this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigCaravanMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else
			{
				Pawn pawn = pawns.Find((Pawn x) => !x.IsColonist && !pawns.Any((Pawn y) => y.IsColonist && y.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)));
				if (pawn != null)
				{
					Messages.Message("CaravanPawnIsUnreachable".Translate(new object[]
					{
						pawn.LabelShort
					}).CapitalizeFirst(), pawn, MessageTypeDefOf.RejectInput, false);
					result = false;
				}
				else
				{
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
									Thing t = this.transferables[i].things[j];
									if (!t.Spawned || pawns.Any((Pawn x) => x.IsColonist && x.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)))
									{
										num += t.stackCount;
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
										Messages.Message("CaravanItemIsUnreachableSingle".Translate(new object[]
										{
											this.transferables[i].ThingDef.label
										}), MessageTypeDefOf.RejectInput, false);
									}
									else
									{
										Messages.Message("CaravanItemIsUnreachableMulti".Translate(new object[]
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

		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, out IntVec3 spot)
		{
			bool result;
			if (this.startingTile < 0)
			{
				Log.Error("Can't find exit spot because startingTile is not set.", false);
				spot = IntVec3.Invalid;
				result = false;
			}
			else
			{
				Predicate<IntVec3> validator = (IntVec3 x) => !x.Fogged(this.map) && x.Standable(this.map);
				Rot4 rotFromTo = Find.WorldGrid.GetRotFromTo(this.CurrentTile, this.startingTile);
				if (reachableForEveryColonist)
				{
					result = CellFinder.TryFindRandomEdgeCellWith(delegate(IntVec3 x)
					{
						bool result2;
						if (!validator(x))
						{
							result2 = false;
						}
						else
						{
							for (int j = 0; j < pawns.Count; j++)
							{
								if (pawns[j].IsColonist && !pawns[j].CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									return false;
								}
							}
							result2 = true;
						}
						return result2;
					}, this.map, rotFromTo, CellFinder.EdgeRoadChance_Always, out spot);
				}
				else
				{
					IntVec3 intVec = IntVec3.Invalid;
					int num = -1;
					foreach (IntVec3 intVec2 in CellRect.WholeMap(this.map).GetEdgeCells(rotFromTo).InRandomOrder(null))
					{
						if (validator(intVec2))
						{
							int num2 = 0;
							for (int i = 0; i < pawns.Count; i++)
							{
								if (pawns[i].IsColonist && pawns[i].CanReach(intVec2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									num2++;
								}
							}
							if (num2 > num)
							{
								num = num2;
								intVec = intVec2;
							}
						}
					}
					spot = intVec;
					result = intVec.IsValid;
				}
			}
			return result;
		}

		private bool TryFindRandomPackingSpot(IntVec3 exitSpot, out IntVec3 packingSpot)
		{
			Dialog_FormCaravan.tmpPackingSpots.Clear();
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.CaravanPackingSpot);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.map.reachability.CanReach(exitSpot, list[i], PathEndMode.OnCell, traverseParams))
				{
					Dialog_FormCaravan.tmpPackingSpots.Add(list[i]);
				}
			}
			bool result;
			if (Dialog_FormCaravan.tmpPackingSpots.Any<Thing>())
			{
				Thing thing = Dialog_FormCaravan.tmpPackingSpots.RandomElement<Thing>();
				Dialog_FormCaravan.tmpPackingSpots.Clear();
				packingSpot = thing.Position;
				result = true;
			}
			else
			{
				result = RCellFinder.TryFindRandomSpotJustOutsideColony(exitSpot, this.map, out packingSpot);
			}
			return result;
		}

		private void AddPawnsToTransferables()
		{
			List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				bool setToTransferMax = (this.reform || this.mapAboutToBeRemoved) && !CaravanUtility.ShouldAutoCapture(list[i], Faction.OfPlayer);
				this.AddToTransferables(list[i], setToTransferMax);
			}
		}

		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, this.reform, this.reform, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i], false);
			}
			if (this.AutoStripSpawnedCorpses)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Spawned)
					{
						this.TryAddCorpseInventoryAndGearToTransferables(list[j]);
					}
				}
			}
			if (this.ListPlayerPawnsInventorySeparately)
			{
				List<Pawn> list2 = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
				for (int k = 0; k < list2.Count; k++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(list2[k]))
					{
						ThingOwner<Thing> innerContainer = list2[k].inventory.innerContainer;
						for (int l = 0; l < innerContainer.Count; l++)
						{
							this.AddToTransferables(innerContainer[l], true);
							if (this.AutoStripSpawnedCorpses && innerContainer[l].Spawned)
							{
								this.TryAddCorpseInventoryAndGearToTransferables(innerContainer[l]);
							}
						}
					}
				}
			}
		}

		private void TryAddCorpseInventoryAndGearToTransferables(Thing potentiallyCorpse)
		{
			Corpse corpse = potentiallyCorpse as Corpse;
			if (corpse != null)
			{
				this.AddCorpseInventoryAndGearToTransferables(corpse);
			}
		}

		private void AddCorpseInventoryAndGearToTransferables(Corpse corpse)
		{
			Pawn_InventoryTracker inventory = corpse.InnerPawn.inventory;
			Pawn_ApparelTracker apparel = corpse.InnerPawn.apparel;
			Pawn_EquipmentTracker equipment = corpse.InnerPawn.equipment;
			for (int i = 0; i < inventory.innerContainer.Count; i++)
			{
				this.AddToTransferables(inventory.innerContainer[i], false);
			}
			if (apparel != null)
			{
				List<Apparel> wornApparel = apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					this.AddToTransferables(wornApparel[j], false);
				}
			}
			if (equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = equipment.AllEquipmentListForReading;
				for (int k = 0; k < allEquipmentListForReading.Count; k++)
				{
					this.AddToTransferables(allEquipmentListForReading[k], false);
				}
			}
		}

		private void RemoveCarriedItemFromTransferablesOrDrop(Thing carried, Pawn carrier, List<TransferableOneWay> transferables)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(carried, transferables, TransferAsOneMode.PodsOrCaravanPacking);
			int num;
			if (transferableOneWay == null)
			{
				num = carried.stackCount;
			}
			else if (transferableOneWay.CountToTransfer >= carried.stackCount)
			{
				transferableOneWay.AdjustBy(-carried.stackCount);
				transferableOneWay.things.Remove(carried);
				num = 0;
			}
			else
			{
				num = carried.stackCount - transferableOneWay.CountToTransfer;
				transferableOneWay.AdjustTo(0);
			}
			if (num > 0)
			{
				Thing thing = carried.SplitOff(num);
				if (carrier.SpawnedOrAnyParentSpawned)
				{
					GenPlace.TryPlaceThing(thing, carrier.PositionHeld, carrier.MapHeld, ThingPlaceMode.Near, null, null);
				}
				else
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		public static bool CanListInventorySeparately(Pawn p)
		{
			return p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer;
		}

		private void SetToSendEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		public static List<Pawn> AllSendablePawns(Map map, bool reform)
		{
			return CaravanFormingUtility.AllSendablePawns(map, reform, reform, reform);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Dialog_FormCaravan()
		{
		}

		[CompilerGenerated]
		private void <DoWindowContents>m__0()
		{
			this.tab = Dialog_FormCaravan.Tab.Pawns;
		}

		[CompilerGenerated]
		private void <DoWindowContents>m__1()
		{
			this.tab = Dialog_FormCaravan.Tab.Items;
		}

		[CompilerGenerated]
		private static bool <DoBottomButtons>m__2(Pawn pawn)
		{
			return CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		[CompilerGenerated]
		private static string <DoBottomButtons>m__3(string str)
		{
			return str + "\n\n";
		}

		[CompilerGenerated]
		private void <DoBottomButtons>m__4()
		{
			if (this.TryFormAndSendCaravan())
			{
				this.Close(false);
			}
		}

		[CompilerGenerated]
		private static bool <DoBottomButtons>m__5(Pawn x)
		{
			return CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed;
		}

		[CompilerGenerated]
		private float <CalculateAndRecacheTransferables>m__6()
		{
			return this.MassCapacity - this.MassUsage;
		}

		[CompilerGenerated]
		private static bool <DebugTryFormCaravanInstantly>m__7(Pawn x)
		{
			return CaravanUtility.IsOwner(x, Faction.OfPlayer);
		}

		[CompilerGenerated]
		private static bool <AddItemsFromTransferablesToRandomInventories>m__8(TransferableOneWay x)
		{
			return x.AnyThing is Pawn;
		}

		[CompilerGenerated]
		private static bool <AddItemsFromTransferablesToRandomInventories>m__9(Thing x)
		{
			return !x.Spawned;
		}

		[CompilerGenerated]
		private static bool <AddItemsFromTransferablesToRandomInventories>m__A(Thing x)
		{
			return x.Spawned;
		}

		[CompilerGenerated]
		private static bool <CheckForErrors>m__B(Pawn x)
		{
			return CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed;
		}

		private enum Tab
		{
			Pawns,
			Items
		}

		[CompilerGenerated]
		private sealed class <AddItemsFromTransferablesToRandomInventories>c__AnonStorey0
		{
			internal List<Pawn> pawns;

			internal Dialog_FormCaravan $this;

			public <AddItemsFromTransferablesToRandomInventories>c__AnonStorey0()
			{
			}

			internal void <>m__0(Thing splitPiece, IThingHolder originalHolder)
			{
				Thing item = splitPiece.TryMakeMinified();
				CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, this.pawns, null, null).inventory.innerContainer.TryAdd(item, true);
			}

			internal void <>m__1(Thing originalThing, int numToTake)
			{
				if (this.$this.AutoStripSpawnedCorpses)
				{
					Corpse corpse = originalThing as Corpse;
					if (corpse != null && corpse.Spawned)
					{
						corpse.Strip();
					}
				}
				Thing item = originalThing.SplitOff(numToTake);
				CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, this.pawns, null, null).inventory.innerContainer.TryAdd(item, true);
			}
		}

		[CompilerGenerated]
		private sealed class <CheckForErrors>c__AnonStorey1
		{
			internal List<Pawn> pawns;

			public <CheckForErrors>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return !x.IsColonist && !this.pawns.Any((Pawn y) => y.IsColonist && y.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn));
			}

			private sealed class <CheckForErrors>c__AnonStorey2
			{
				internal Pawn x;

				internal Dialog_FormCaravan.<CheckForErrors>c__AnonStorey1 <>f__ref$1;

				public <CheckForErrors>c__AnonStorey2()
				{
				}

				internal bool <>m__0(Pawn y)
				{
					return y.IsColonist && y.CanReach(this.x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <CheckForErrors>c__AnonStorey3
		{
			internal Thing t;

			public <CheckForErrors>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return x.IsColonist && x.CanReach(this.t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindExitSpot>c__AnonStorey4
		{
			internal Predicate<IntVec3> validator;

			internal List<Pawn> pawns;

			internal Dialog_FormCaravan $this;

			public <TryFindExitSpot>c__AnonStorey4()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return !x.Fogged(this.$this.map) && x.Standable(this.$this.map);
			}

			internal bool <>m__1(IntVec3 x)
			{
				bool result;
				if (!this.validator(x))
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].IsColonist && !this.pawns[i].CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}
	}
}
