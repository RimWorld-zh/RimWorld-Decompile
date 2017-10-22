using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	public class Dialog_LoadTransporters : Window
	{
		private enum Tab
		{
			Pawns = 0,
			Items = 1
		}

		private Map map;

		private List<CompTransporter> transporters;

		private List<TransferableOneWay> transferables;

		private TransferableOneWayWidget pawnsTransfer;

		private TransferableOneWayWidget itemsTransfer;

		private Tab tab = Tab.Pawns;

		private float lastMassFlashTime = -9999f;

		private bool massUsageDirty = true;

		private float cachedMassUsage;

		private bool daysWorthOfFoodDirty = true;

		private Pair<float, float> cachedDaysWorthOfFood;

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

		private string TransportersLabel
		{
			get
			{
				return Find.ActiveLanguageWorker.Pluralize(this.transporters[0].parent.Label, -1);
			}
		}

		private string TransportersLabelCap
		{
			get
			{
				return this.TransportersLabel.CapitalizeFirst();
			}
		}

		private bool EnvironmentAllowsEatingVirtualPlantsNow
		{
			get
			{
				return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsNowAt(this.map.Tile);
			}
		}

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

		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.EnvironmentAllowsEatingVirtualPlantsNow, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		public Dialog_LoadTransporters(Map map, List<CompTransporter> transporters)
		{
			this.map = map;
			this.transporters = new List<CompTransporter>();
			this.transporters.AddRange(transporters);
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
			Widgets.Label(rect, "LoadTransporters".Translate(this.TransportersLabel));
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Dialog_LoadTransporters.tabsList.Clear();
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("PawnsTab".Translate(), (Action)delegate
			{
				this.tab = Tab.Pawns;
			}, this.tab == Tab.Pawns));
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("ItemsTab".Translate(), (Action)delegate
			{
				this.tab = Tab.Items;
			}, this.tab == Tab.Items));
			inRect.yMin += 72f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_LoadTransporters.tabsList);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2;
			Rect rect3 = rect2 = inRect.AtZero();
			rect2.xMin += (float)(rect3.width - 515.0);
			rect2.y += 32f;
			TransferableUIUtility.DrawMassInfo(rect2, this.MassUsage, this.MassCapacity, "TransportersMassUsageTooltip".Translate(), this.lastMassFlashTime, true);
			CaravanUIUtility.DrawDaysWorthOfFoodInfo(new Rect(rect2.x, (float)(rect2.y + 19.0), rect2.width, rect2.height), this.DaysWorthOfFood.First, this.DaysWorthOfFood.Second, this.EnvironmentAllowsEatingVirtualPlantsNow, true, 3.40282347E+38f);
			this.DoBottomButtons(rect3);
			Rect inRect2 = rect3;
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
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, false, true) && this.TryAccept())
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
			if (Prefs.DevMode)
			{
				float width = 200f;
				Vector2 bottomButtonSize9 = this.BottomButtonSize;
				float num3 = (float)(bottomButtonSize9.y / 2.0);
				Rect rect5 = new Rect(0f, (float)(rect.height - 55.0), width, num3);
				if (Widgets.ButtonText(rect5, "Dev: Load instantly", true, false, true) && this.DebugTryLoadInstantly())
				{
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				Rect rect6 = new Rect(0f, (float)(rect.height - 55.0 + num3), width, num3);
				if (Widgets.ButtonText(rect6, "Dev: Select everything", true, false, true))
				{
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
					this.SetToLoadEverything();
				}
			}
		}

		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			this.pawnsTransfer = new TransferableOneWayWidget(null, Faction.OfPlayer.Name, this.TransportersLabelCap, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, (Func<float>)(() => this.MassCapacity - this.MassUsage), 24f, false, true, -1);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			this.itemsTransfer = new TransferableOneWayWidget(from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, Faction.OfPlayer.Name, this.TransportersLabelCap, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, (Func<float>)(() => this.MassCapacity - this.MassUsage), 24f, false, true, this.map.Tile);
			this.CountToTransferChanged();
		}

		private bool DebugTryLoadInstantly()
		{
			this.CreateAndAssignNewTransportersGroup();
			int i;
			for (i = 0; i < this.transferables.Count; i++)
			{
				TransferableUtility.Transfer(this.transferables[i].things, this.transferables[i].CountToTransfer, (Action<Thing, IThingHolder>)delegate(Thing splitPiece, IThingHolder originalThing)
				{
					this.transporters[i % this.transporters.Count].GetDirectlyHeldThings().TryAdd(splitPiece, true);
				});
			}
			return true;
		}

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
				if (enumerable.Any())
				{
					foreach (Pawn item in enumerable)
					{
						Lord lord = item.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(item, PawnLostCondition.ForcedToJoinOtherLord);
						}
					}
					LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_LoadAndEnterTransporters(transportersGroup), this.map, enumerable);
					foreach (Pawn item2 in enumerable)
					{
						if (item2.Spawned)
						{
							item2.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
				}
				Messages.Message("MessageTransportersLoadingProcessStarted".Translate(), (Thing)this.transporters[0].parent, MessageTypeDefOf.TaskCompletion);
				result = true;
			}
			return result;
		}

		private void AssignTransferablesToRandomTransporters()
		{
			TransferableOneWay transferableOneWay = this.transferables.MaxBy((Func<TransferableOneWay, int>)((TransferableOneWay x) => x.CountToTransfer));
			int num = 0;
			for (int i = 0; i < this.transferables.Count; i++)
			{
				if (this.transferables[i] != transferableOneWay && this.transferables[i].CountToTransfer > 0)
				{
					this.transporters[num % this.transporters.Count].AddToTheToLoadList(this.transferables[i], this.transferables[i].CountToTransfer);
					num++;
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

		private int CreateAndAssignNewTransportersGroup()
		{
			int nextTransporterGroupID = Find.UniqueIDsManager.GetNextTransporterGroupID();
			for (int i = 0; i < this.transporters.Count; i++)
			{
				this.transporters[i].groupID = nextTransporterGroupID;
			}
			return nextTransporterGroupID;
		}

		private bool CheckForErrors(List<Pawn> pawns)
		{
			bool result;
			int i;
			int countToTransfer;
			if (!this.transferables.Any((Predicate<TransferableOneWay>)((TransferableOneWay x) => x.CountToTransfer != 0)))
			{
				Messages.Message("CantSendEmptyTransportPods".Translate(), MessageTypeDefOf.RejectInput);
				result = false;
			}
			else if (this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigTransportersMassUsage".Translate(), MessageTypeDefOf.RejectInput);
				result = false;
			}
			else
			{
				Pawn pawn = pawns.Find((Predicate<Pawn>)((Pawn x) => !x.MapHeld.reachability.CanReach(x.PositionHeld, (Thing)this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false))));
				if (pawn != null)
				{
					Messages.Message("PawnCantReachTransporters".Translate(pawn.LabelShort).CapitalizeFirst(), MessageTypeDefOf.RejectInput);
					result = false;
				}
				else
				{
					Map map = this.transporters[0].parent.Map;
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
									Thing thing = this.transferables[i].things[j];
									if (map.reachability.CanReach(thing.Position, (Thing)this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
									{
										num += thing.stackCount;
										if (num >= countToTransfer)
											break;
									}
								}
								if (num < countToTransfer)
									goto IL_01c7;
							}
						}
					}
					result = true;
				}
			}
			goto IL_026a;
			IL_026a:
			return result;
			IL_01c7:
			if (countToTransfer == 1)
			{
				Messages.Message("TransporterItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput);
			}
			else
			{
				Messages.Message("TransporterItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput);
			}
			result = false;
			goto IL_026a;
		}

		private void AddPawnsToTransferables()
		{
			List<Pawn> list = CaravanFormingUtility.AllSendablePawns(this.map, false, false);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, false, false);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		private void SetToLoadEverything()
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
			this.daysWorthOfFoodDirty = true;
		}
	}
}
