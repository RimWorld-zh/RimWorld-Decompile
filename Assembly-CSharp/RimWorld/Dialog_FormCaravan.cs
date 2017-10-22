using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class Dialog_FormCaravan : Window
	{
		private enum Tab
		{
			Pawns = 0,
			Items = 1,
			Config = 2
		}

		private Map map;

		private bool reform;

		private Action onClosed;

		private bool showEstTimeToDestinationButton;

		private bool cancellingWillAbandon;

		private bool thisWindowInstanceEverOpened;

		public List<TransferableOneWay> transferables;

		private TransferableOneWayWidget pawnsTransfer;

		private TransferableOneWayWidget itemsTransfer;

		private Tab tab = Tab.Pawns;

		private float lastMassFlashTime = -9999f;

		private int startingTile = -1;

		private bool massUsageDirty = true;

		private float cachedMassUsage;

		private bool massCapacityDirty = true;

		private float cachedMassCapacity;

		private bool daysWorthOfFoodDirty = true;

		private Pair<float, float> cachedDaysWorthOfFood;

		private const float TitleRectHeight = 40f;

		private const float BottomAreaHeight = 55f;

		private const float ExitDirectionTitleHeight = 30f;

		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		private readonly Vector2 ExitDirectionRadioSize = new Vector2(250f, 30f);

		private const float MaxDaysWorthOfFoodToShowWarningDialog = 5f;

		public const float MassLabelYOffset = 32f;

		private static List<TabRecord> tabsList = new List<TabRecord>();

		private static List<Thing> tmpPackingSpots = new List<Thing>();

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

		private bool AutoStripCorpses
		{
			get
			{
				return this.reform;
			}
		}

		private bool EnvironmentAllowsEatingVirtualPlantsNow
		{
			get
			{
				return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsNowAt(this.CurrentTile);
			}
		}

		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					List<TransferableOneWay> list = this.transferables;
					IgnorePawnsInventoryMode ignoreInventory = IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload;
					bool autoStripCorpses = this.AutoStripCorpses;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(list, ignoreInventory, false, autoStripCorpses);
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
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables);
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
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.EnvironmentAllowsEatingVirtualPlantsNow, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload));
				}
				return this.cachedDaysWorthOfFood;
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
					if (transferableOneWay.HasAnyThing && transferableOneWay.CountToTransfer > 0 && transferableOneWay.ThingDef.IsNutritionGivingIngestible)
					{
						float num3 = 1000f;
						CompRottable compRottable = transferableOneWay.AnyThing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							num3 = (float)((float)compRottable.ApproxTicksUntilRotWhenAtTempOfTile(this.CurrentTile) / 60000.0);
						}
						float num4 = transferableOneWay.ThingDef.ingestible.nutrition * (float)transferableOneWay.CountToTransfer;
						if (num3 < 5.0)
						{
							num += num4;
						}
						else
						{
							num2 += num4;
						}
					}
				}
				return (num != 0.0 || num2 != 0.0) && num / (num + num2) >= 0.75;
			}
		}

		public Dialog_FormCaravan(Map map, bool reform = false, Action onClosed = null, bool showEstTimeToDestinationButton = true, bool cancellingWillAbandon = false)
		{
			this.map = map;
			this.reform = reform;
			this.onClosed = onClosed;
			this.showEstTimeToDestinationButton = showEstTimeToDestinationButton;
			this.cancellingWillAbandon = cancellingWillAbandon;
			base.closeOnEscapeKey = !reform;
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
		}

		public override void PostOpen()
		{
			base.PostOpen();
			if (!this.thisWindowInstanceEverOpened)
			{
				this.thisWindowInstanceEverOpened = true;
				this.CalculateAndRecacheTransferables();
				this.startingTile = CaravanExitMapUtility.RandomBestExitTileFrom(this.map);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.FormCaravan, KnowledgeAmount.Total);
			}
		}

		public override void PostClose()
		{
			base.PostClose();
			if ((object)this.onClosed != null)
			{
				this.onClosed();
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 40f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, ((!this.reform) ? "FormCaravan" : "ReformCaravan").Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Dialog_FormCaravan.tabsList.Clear();
			Dialog_FormCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), (Action)delegate
			{
				this.tab = Tab.Pawns;
			}, this.tab == Tab.Pawns));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), (Action)delegate
			{
				this.tab = Tab.Items;
			}, this.tab == Tab.Items));
			if (!this.reform)
			{
				Dialog_FormCaravan.tabsList.Add(new TabRecord("CaravanConfigTab".Translate(), (Action)delegate
				{
					this.tab = Tab.Config;
				}, this.tab == Tab.Config));
			}
			inRect.yMin += 72f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_FormCaravan.tabsList);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			if (this.tab != Tab.Config)
			{
				Rect rect3 = rect2;
				rect3.xMin += (float)(rect2.width - 515.0);
				rect3.y += 32f;
				TransferableUIUtility.DrawMassInfo(rect3, this.MassUsage, this.MassCapacity, "CaravanMassUsageTooltip".Translate(), this.lastMassFlashTime, true);
				CaravanUIUtility.DrawDaysWorthOfFoodInfo(new Rect(rect3.x, (float)(rect3.y + 19.0), rect3.width, rect3.height), this.DaysWorthOfFood.First, this.DaysWorthOfFood.Second, this.EnvironmentAllowsEatingVirtualPlantsNow, true, 3.40282347E+38f);
			}
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			switch (this.tab)
			{
			case Tab.Pawns:
			{
				this.pawnsTransfer.OnGUI(inRect2, out flag);
				break;
			}
			case Tab.Items:
			{
				this.itemsTransfer.OnGUI(inRect2, out flag);
				break;
			}
			case Tab.Config:
			{
				this.DrawConfig(rect2);
				break;
			}
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

		private void AddToTransferables(Thing t, bool setToTransferMax = false)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching(t, this.transferables);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			transferableOneWay.things.Add(t);
			if (setToTransferMax)
			{
				transferableOneWay.AdjustTo(transferableOneWay.MaxCount);
			}
		}

		private void DoBottomButtons(Rect rect)
		{
			double num = rect.width / 2.0;
			Vector2 bottomButtonSize = this.BottomButtonSize;
			double x2 = num - bottomButtonSize.x / 2.0;
			double y = rect.height - 55.0;
			Vector2 bottomButtonSize2 = this.BottomButtonSize;
			float x3 = bottomButtonSize2.x;
			Vector2 bottomButtonSize3 = this.BottomButtonSize;
			Rect rect2 = new Rect((float)x2, (float)y, x3, bottomButtonSize3.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, false, true))
			{
				if (this.reform)
				{
					if (this.TryReformCaravan())
					{
						SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
				else
				{
					List<string> list = new List<string>();
					Pair<float, float> daysWorthOfFood = this.DaysWorthOfFood;
					if (daysWorthOfFood.First < 5.0)
					{
						list.Add((!(daysWorthOfFood.First < 0.10000000149011612)) ? "DaysWorthOfFoodWarningDialog".Translate(daysWorthOfFood.First.ToString("0.#")) : "DaysWorthOfFoodWarningDialog_NoFood".Translate());
					}
					else if (this.MostFoodWillRotSoon)
					{
						list.Add("CaravanFoodWillRotSoonWarningDialog".Translate());
					}
					if (!TransferableUtility.GetPawnsFromTransferables(this.transferables).Any((Predicate<Pawn>)((Pawn pawn) => CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)))
					{
						list.Add("CaravanIncapableOfSocial".Translate());
					}
					if (list.Count > 0)
					{
						if (this.CheckForErrors(TransferableUtility.GetPawnsFromTransferables(this.transferables)))
						{
							string text = string.Concat((from str in list
							select str + "\n\n").ToArray()) + "CaravanAreYouSure".Translate();
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, (Action)delegate
							{
								if (this.TryFormAndSendCaravan())
								{
									this.Close(false);
								}
							}, false, (string)null));
						}
					}
					else if (this.TryFormAndSendCaravan())
					{
						SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
			}
			double num2 = rect2.x - 10.0;
			Vector2 bottomButtonSize4 = this.BottomButtonSize;
			double x4 = num2 - bottomButtonSize4.x;
			float y2 = rect2.y;
			Vector2 bottomButtonSize5 = this.BottomButtonSize;
			float x5 = bottomButtonSize5.x;
			Vector2 bottomButtonSize6 = this.BottomButtonSize;
			Rect rect3 = new Rect((float)x4, y2, x5, bottomButtonSize6.y);
			if (Widgets.ButtonText(rect3, "ResetButton".Translate(), true, false, true))
			{
				SoundDefOf.TickLow.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			double x6 = rect2.xMax + 10.0;
			float y3 = rect2.y;
			Vector2 bottomButtonSize7 = this.BottomButtonSize;
			float x7 = bottomButtonSize7.x;
			Vector2 bottomButtonSize8 = this.BottomButtonSize;
			Rect rect4 = new Rect((float)x6, y3, x7, bottomButtonSize8.y);
			if (Widgets.ButtonText(rect4, (!this.cancellingWillAbandon) ? "CancelButton".Translate() : "AbandonButton".Translate(), true, false, true))
			{
				this.Close(true);
			}
			if (this.showEstTimeToDestinationButton)
			{
				float width = rect.width;
				Vector2 bottomButtonSize9 = this.BottomButtonSize;
				float x8 = width - bottomButtonSize9.x;
				float y4 = rect2.y;
				Vector2 bottomButtonSize10 = this.BottomButtonSize;
				float x9 = bottomButtonSize10.x;
				Vector2 bottomButtonSize11 = this.BottomButtonSize;
				Rect rect5 = new Rect(x8, y4, x9, bottomButtonSize11.y);
				if (Widgets.ButtonText(rect5, "EstimatedTimeToDestinationButton".Translate(), true, false, true))
				{
					List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
					if (!pawnsFromTransferables.Any((Predicate<Pawn>)((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed)))
					{
						Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput);
					}
					else
					{
						Find.WorldRoutePlanner.Start(this);
					}
				}
			}
			if (Prefs.DevMode)
			{
				float width2 = 200f;
				Vector2 bottomButtonSize12 = this.BottomButtonSize;
				float num3 = (float)(bottomButtonSize12.y / 2.0);
				Rect rect6 = new Rect(0f, (float)(rect.height - 55.0), width2, num3);
				if (Widgets.ButtonText(rect6, "Dev: Send instantly", true, false, true) && this.DebugTryFormCaravanInstantly())
				{
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				Rect rect7 = new Rect(0f, (float)(rect.height - 55.0 + num3), width2, num3);
				if (Widgets.ButtonText(rect7, "Dev: Select everything", true, false, true))
				{
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
					this.SetToSendEverything();
				}
			}
		}

		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			string sourceLabel = (!this.reform) ? Faction.OfPlayer.Name : this.map.info.parent.LabelCap;
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, sourceLabel, WorldObjectDefOf.Caravan.LabelCap, "FormCaravanColonyThingCountTip".Translate(), IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, (Func<float>)(() => this.MassCapacity - this.MassUsage), this.AutoStripCorpses, this.CurrentTile);
			this.CountToTransferChanged();
		}

		private void DrawConfig(Rect rect)
		{
			Rect rect2 = new Rect(0f, rect.y, rect.width, 30f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "ExitDirection".Translate());
			Text.Font = GameFont.Small;
			List<int> list = CaravanExitMapUtility.AvailableExitTilesAt(this.map);
			if (list.Any())
			{
				for (int i = 0; i < list.Count; i++)
				{
					Direction8Way direction8WayFromTo = Find.WorldGrid.GetDirection8WayFromTo(this.CurrentTile, list[i]);
					float y = rect.y;
					float num = (float)i;
					Vector2 exitDirectionRadioSize = this.ExitDirectionRadioSize;
					float num2 = (float)(y + num * exitDirectionRadioSize.y + 30.0 + 4.0);
					float x = rect.x;
					float y2 = num2;
					Vector2 exitDirectionRadioSize2 = this.ExitDirectionRadioSize;
					float x2 = exitDirectionRadioSize2.x;
					Vector2 exitDirectionRadioSize3 = this.ExitDirectionRadioSize;
					Rect rect3 = new Rect(x, y2, x2, exitDirectionRadioSize3.y);
					Vector2 vector = Find.WorldGrid.LongLatOf(list[i]);
					string labelText = "ExitDirectionRadioButtonLabel".Translate(direction8WayFromTo.LabelShort(), vector.y.ToStringLatitude(), vector.x.ToStringLongitude());
					if (Widgets.RadioButtonLabeled(rect3, labelText, this.startingTile == list[i]))
					{
						this.startingTile = list[i];
					}
				}
			}
			else
			{
				GUI.color = Color.gray;
				Widgets.Label(new Rect(rect.x, (float)(rect.y + 30.0 + 4.0), rect.width, 100f), "NoCaravanExitDirectionAvailable".Translate());
				GUI.color = Color.white;
			}
		}

		private bool DebugTryFormCaravanInstantly()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			bool result;
			if (!pawnsFromTransferables.Any((Predicate<Pawn>)((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer))))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput);
				result = false;
			}
			else
			{
				this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
				int currentTile = this.startingTile;
				if (currentTile < 0)
				{
					currentTile = this.CurrentTile;
				}
				CaravanFormingUtility.FormAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, currentTile);
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
				IntVec3 intVec = default(IntVec3);
				if (!this.TryFindExitSpot(pawnsFromTransferables, true, out intVec))
				{
					if (!this.TryFindExitSpot(pawnsFromTransferables, false, out intVec))
					{
						Messages.Message("CaravanCouldNotFindExitSpot".Translate(direction8WayFromTo.LabelShort()), MessageTypeDefOf.RejectInput);
						result = false;
						goto IL_013b;
					}
					Messages.Message("CaravanCouldNotFindReachableExitSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.CautionInput);
				}
				IntVec3 meetingPoint = default(IntVec3);
				if (!this.TryFindRandomPackingSpot(intVec, out meetingPoint))
				{
					Messages.Message("CaravanCouldNotFindPackingSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.RejectInput);
					result = false;
				}
				else
				{
					CaravanFormingUtility.StartFormingCaravan(pawnsFromTransferables, Faction.OfPlayer, this.transferables, meetingPoint, intVec, this.startingTile);
					Messages.Message("CaravanFormationProcessStarted".Translate(), (Thing)pawnsFromTransferables[0], MessageTypeDefOf.PositiveEvent);
					result = true;
				}
			}
			goto IL_013b;
			IL_013b:
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
				Caravan o = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile);
				this.map.info.parent.CheckRemoveMapNow();
				Messages.Message("MessageReformedCaravan".Translate(), (WorldObject)o, MessageTypeDefOf.TaskCompletion);
				result = true;
			}
			return result;
		}

		private void AddItemsFromTransferablesToRandomInventories(List<Pawn> pawns)
		{
			this.transferables.RemoveAll((Predicate<TransferableOneWay>)((TransferableOneWay x) => x.AnyThing is Pawn));
			for (int i = 0; i < this.transferables.Count; i++)
			{
				TransferableUtility.TransferNoSplit(this.transferables[i].things, this.transferables[i].CountToTransfer, (Action<Thing, int>)delegate(Thing originalThing, int numToTake)
				{
					Corpse corpse = originalThing as Corpse;
					if (corpse != null && corpse.SpawnedOrAnyParentSpawned)
					{
						corpse.Strip();
					}
					Thing thing = originalThing.SplitOff(numToTake);
					this.RemoveFromCorpseIfPossible(thing);
					CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, pawns, null, null).inventory.innerContainer.TryAdd(thing, true);
				}, true, true);
			}
		}

		private void RemoveFromCorpseIfPossible(Thing thing)
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				for (int j = 0; j < this.transferables[i].things.Count; j++)
				{
					Corpse corpse = this.transferables[i].things[j] as Corpse;
					if (corpse != null)
					{
						Pawn innerPawn = corpse.InnerPawn;
						Apparel apparel = thing as Apparel;
						ThingWithComps thingWithComps = thing as ThingWithComps;
						if (innerPawn.inventory.innerContainer.Contains(thing))
						{
							innerPawn.inventory.innerContainer.Remove(thing);
						}
						if (apparel != null && innerPawn.apparel != null && innerPawn.apparel.WornApparel.Contains(apparel))
						{
							innerPawn.apparel.Remove(apparel);
						}
						if (thingWithComps != null && innerPawn.equipment != null && innerPawn.equipment.AllEquipmentListForReading.Contains(thingWithComps))
						{
							innerPawn.equipment.Remove(thingWithComps);
						}
					}
				}
			}
		}

		private bool CheckForErrors(List<Pawn> pawns)
		{
			bool result;
			int i;
			int countToTransfer;
			if (!this.reform && this.startingTile < 0)
			{
				Messages.Message("NoExitDirectionForCaravanChosen".Translate(), MessageTypeDefOf.RejectInput);
				result = false;
			}
			else if (!pawns.Any((Predicate<Pawn>)((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed)))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput);
				result = false;
			}
			else if (!this.reform && this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigCaravanMassUsage".Translate(), MessageTypeDefOf.RejectInput);
				result = false;
			}
			else
			{
				Pawn pawn = pawns.Find((Predicate<Pawn>)((Pawn x) => !x.IsColonist && !pawns.Any((Predicate<Pawn>)((Pawn y) => y.IsColonist && y.CanReach((Thing)x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)))));
				if (pawn != null)
				{
					Messages.Message("CaravanPawnIsUnreachable".Translate(pawn.LabelShort).CapitalizeFirst(), (Thing)pawn, MessageTypeDefOf.RejectInput);
					result = false;
				}
				else
				{
					for (i = 0; i < this.transferables.Count; i++)
					{
						if (this.transferables[i].ThingDef.category == ThingCategory.Item)
						{
							countToTransfer = this.transferables[i].CountToTransfer;
							int num = 0;
							if (countToTransfer > 0)
							{
								for (int j = 0; j < this.transferables[i].things.Count; j++)
								{
									Thing t = this.transferables[i].things[j];
									if (!t.Spawned || pawns.Any((Predicate<Pawn>)((Pawn x) => x.IsColonist && x.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))))
									{
										num += t.stackCount;
										if (num >= countToTransfer)
											break;
									}
								}
								if (num < countToTransfer)
									goto IL_020f;
							}
						}
					}
					result = true;
				}
			}
			goto IL_02b2;
			IL_020f:
			if (countToTransfer == 1)
			{
				Messages.Message("CaravanItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput);
			}
			else
			{
				Messages.Message("CaravanItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput);
			}
			result = false;
			goto IL_02b2;
			IL_02b2:
			return result;
		}

		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, out IntVec3 spot)
		{
			bool result;
			if (this.startingTile < 0)
			{
				Log.Error("Can't find exit spot because startingTile is not set.");
				spot = IntVec3.Invalid;
				result = false;
			}
			else
			{
				Predicate<IntVec3> validator = (Predicate<IntVec3>)((IntVec3 x) => !x.Fogged(this.map) && x.Standable(this.map));
				Rot4 rotFromTo = Find.WorldGrid.GetRotFromTo(this.CurrentTile, this.startingTile);
				if (reachableForEveryColonist)
				{
					result = CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)delegate(IntVec3 x)
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
									goto IL_0057;
							}
							result2 = true;
						}
						goto IL_007b;
						IL_007b:
						return result2;
						IL_0057:
						result2 = false;
						goto IL_007b;
					}, this.map, rotFromTo, CellFinder.EdgeRoadChance_Always, out spot);
				}
				else
				{
					IntVec3 intVec = IntVec3.Invalid;
					int num = -1;
					foreach (IntVec3 item in CellRect.WholeMap(this.map).GetEdgeCells(rotFromTo).InRandomOrder(null))
					{
						if (validator(item))
						{
							int num2 = 0;
							for (int i = 0; i < pawns.Count; i++)
							{
								if (pawns[i].IsColonist && pawns[i].CanReach(item, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
								{
									num2++;
								}
							}
							if (num2 > num)
							{
								num = num2;
								intVec = item;
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
			if (Dialog_FormCaravan.tmpPackingSpots.Any())
			{
				Thing thing = Dialog_FormCaravan.tmpPackingSpots.RandomElement();
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
				this.AddToTransferables(list[i], this.reform);
			}
		}

		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, this.reform, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i], false);
			}
			if (this.AutoStripCorpses)
			{
				for (int j = 0; j < list.Count; j++)
				{
					Corpse corpse = list[j] as Corpse;
					if (corpse != null)
					{
						this.AddCorpseInventoryAndGearToTransferables(corpse);
					}
				}
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

		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		private void SetToSendEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximum());
			}
			this.CountToTransferChanged();
		}

		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.daysWorthOfFoodDirty = true;
		}

		public static List<Pawn> AllSendablePawns(Map map, bool reform)
		{
			return CaravanFormingUtility.AllSendablePawns(map, reform, reform);
		}
	}
}
