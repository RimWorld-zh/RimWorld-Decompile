using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008E0 RID: 2272
	public class WITab_Caravan_Gear : WITab
	{
		// Token: 0x04001C24 RID: 7204
		private Vector2 leftPaneScrollPosition;

		// Token: 0x04001C25 RID: 7205
		private float leftPaneScrollViewHeight;

		// Token: 0x04001C26 RID: 7206
		private Vector2 rightPaneScrollPosition;

		// Token: 0x04001C27 RID: 7207
		private float rightPaneScrollViewHeight;

		// Token: 0x04001C28 RID: 7208
		private Thing draggedItem;

		// Token: 0x04001C29 RID: 7209
		private Vector2 draggedItemPosOffset;

		// Token: 0x04001C2A RID: 7210
		private bool droppedDraggedItem;

		// Token: 0x04001C2B RID: 7211
		private float leftPaneWidth;

		// Token: 0x04001C2C RID: 7212
		private float rightPaneWidth;

		// Token: 0x04001C2D RID: 7213
		private const float PawnRowHeight = 50f;

		// Token: 0x04001C2E RID: 7214
		private const float ItemRowHeight = 30f;

		// Token: 0x04001C2F RID: 7215
		private const float PawnLabelHeight = 18f;

		// Token: 0x04001C30 RID: 7216
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04001C31 RID: 7217
		private const float GearLabelColumnWidth = 250f;

		// Token: 0x04001C32 RID: 7218
		private const float SpaceAroundIcon = 4f;

		// Token: 0x04001C33 RID: 7219
		private const float EquippedGearColumnWidth = 250f;

		// Token: 0x04001C34 RID: 7220
		private const float EquippedGearIconSize = 32f;

		// Token: 0x04001C35 RID: 7221
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x04001C36 RID: 7222
		private static List<ThingWithComps> tmpExistingEquipment = new List<ThingWithComps>();

		// Token: 0x04001C37 RID: 7223
		private static List<Apparel> tmpExistingApparel = new List<Apparel>();

		// Token: 0x06003424 RID: 13348 RVA: 0x001BE066 File Offset: 0x001BC466
		public WITab_Caravan_Gear()
		{
			this.labelKey = "TabCaravanGear";
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x001BE07C File Offset: 0x001BC47C
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x001BE09C File Offset: 0x001BC49C
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.leftPaneWidth = 469f;
			this.rightPaneWidth = 345f;
			this.size.x = this.leftPaneWidth + this.rightPaneWidth;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x001BE0FF File Offset: 0x001BC4FF
		public override void OnOpen()
		{
			base.OnOpen();
			this.draggedItem = null;
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x001BE110 File Offset: 0x001BC510
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			this.CheckDraggedItemStillValid();
			this.CheckDropDraggedItem();
			Rect position = new Rect(0f, 0f, this.leftPaneWidth, this.size.y);
			GUI.BeginGroup(position);
			this.DoLeftPane();
			GUI.EndGroup();
			Rect position2 = new Rect(position.xMax, 0f, this.rightPaneWidth, this.size.y);
			GUI.BeginGroup(position2);
			this.DoRightPane();
			GUI.EndGroup();
			if (this.draggedItem != null && this.droppedDraggedItem)
			{
				this.droppedDraggedItem = false;
				this.draggedItem = null;
			}
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x001BE1C4 File Offset: 0x001BC5C4
		private void DoLeftPane()
		{
			Rect rect = new Rect(0f, 0f, this.leftPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.leftPaneScrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.leftPaneScrollPosition, rect2, true);
			this.DoPawnRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.leftPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x001BE264 File Offset: 0x001BC664
		private void DoRightPane()
		{
			Rect rect = new Rect(0f, 0f, this.rightPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.rightPaneScrollViewHeight);
			bool flag = this.draggedItem != null && rect.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != null;
			if (flag)
			{
				Widgets.DrawHighlight(rect);
				if (this.droppedDraggedItem)
				{
					this.MoveDraggedItemToInventory();
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
			}
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.rightPaneScrollPosition, rect2, true);
			this.DoInventoryRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.rightPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x001BE368 File Offset: 0x001BC768
		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.draggedItem != null)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Rect rect = new Rect(mousePosition.x - this.draggedItemPosOffset.x, mousePosition.y - this.draggedItemPosOffset.y, 32f, 32f);
				Find.WindowStack.ImmediateWindow(1283641090, rect, WindowLayer.Super, delegate
				{
					if (this.draggedItem != null)
					{
						Widgets.ThingIcon(rect.AtZero(), this.draggedItem, 1f);
					}
				}, false, false, 0f);
			}
			this.CheckDropDraggedItem();
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x001BE40C File Offset: 0x001BC80C
		private void DoPawnRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(135f, curY + 6f, 200f, 100f), "DragToRearrange".Translate());
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (pawn2.IsPrisoner)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisoners".Translate());
						flag = true;
					}
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x001BE51C File Offset: 0x001BC91C
		private void DoPawnRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.leftPaneScrollPosition.y - 50f;
			float num2 = this.leftPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoPawnRow(new Rect(0f, curY, viewRect.width, 50f), p);
			}
			curY += 50f;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x001BE58C File Offset: 0x001BC98C
		private void DoPawnRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			bool flag = this.draggedItem != null && rect2.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != p;
			if ((Mouse.IsOver(rect2) && this.draggedItem == null) || flag)
			{
				Widgets.DrawHighlight(rect2);
			}
			if (flag && this.droppedDraggedItem)
			{
				this.TryEquipDraggedItem(p);
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
			Rect bgRect = new Rect(rect3.xMax + 4f, 16f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float xMax = bgRect.xMax;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					this.DoEquippedGear(allEquipmentListForReading[i], p, ref xMax);
				}
			}
			if (p.apparel != null)
			{
				WITab_Caravan_Gear.tmpApparel.Clear();
				WITab_Caravan_Gear.tmpApparel.AddRange(p.apparel.WornApparel);
				WITab_Caravan_Gear.tmpApparel.SortBy((Apparel x) => x.def.apparel.LastLayer.drawOrder, (Apparel x) => -x.def.apparel.HumanBodyCoverage);
				for (int j = 0; j < WITab_Caravan_Gear.tmpApparel.Count; j++)
				{
					this.DoEquippedGear(WITab_Caravan_Gear.tmpApparel[j], p, ref xMax);
				}
			}
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x001BE840 File Offset: 0x001BCC40
		private void DoInventoryRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanWeaponsAndApparel".Translate());
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (this.IsVisibleWeapon(thing.def))
				{
					if (!flag)
					{
						flag = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < list.Count; j++)
			{
				Thing thing2 = list[j];
				if (thing2.def.IsApparel)
				{
					if (!flag2)
					{
						flag2 = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing2);
				}
			}
			if (!flag && !flag2)
			{
				Widgets.NoneLabel(ref curY, scrollViewRect.width, null);
			}
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x001BE928 File Offset: 0x001BCD28
		private void DoInventoryRow(ref float curY, Rect viewRect, Rect scrollOutRect, Thing t)
		{
			float num = this.rightPaneScrollPosition.y - 30f;
			float num2 = this.rightPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoInventoryRow(new Rect(0f, curY, viewRect.width, 30f), t);
			}
			curY += 30f;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x001BE998 File Offset: 0x001BCD98
		private void DoInventoryRow(Rect rect, Thing t)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, t);
			rect2.width -= 24f;
			if (this.draggedItem == null && Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			float num = (t != this.draggedItem) ? 1f : 0.5f;
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, t, num);
			GUI.color = new Color(1f, 1f, 1f, num);
			Rect rect4 = new Rect(rect3.xMax + 4f, 0f, 250f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect4, t.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect2))
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = new Vector2(16f, 16f);
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			GUI.EndGroup();
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x001BEB2C File Offset: 0x001BCF2C
		private void DoEquippedGear(Thing t, Pawn p, ref float curX)
		{
			Rect rect = new Rect(curX, 9f, 32f, 32f);
			bool flag = Mouse.IsOver(rect);
			float alpha;
			if (t == this.draggedItem)
			{
				alpha = 0.2f;
			}
			else if (flag && this.draggedItem == null)
			{
				alpha = 0.75f;
			}
			else
			{
				alpha = 1f;
			}
			Widgets.ThingIcon(rect, t, alpha);
			curX += 32f;
			TooltipHandler.TipRegion(rect, t.LabelCap);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && flag)
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = Event.current.mousePosition - rect.position;
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x001BEC1C File Offset: 0x001BD01C
		private void CheckDraggedItemStillValid()
		{
			if (this.draggedItem != null)
			{
				if (this.draggedItem.Destroyed)
				{
					this.draggedItem = null;
				}
				else if (this.CurrentWearerOf(this.draggedItem) == null)
				{
					List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
					if (!list.Contains(this.draggedItem))
					{
						this.draggedItem = null;
					}
				}
			}
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x001BEC96 File Offset: 0x001BD096
		private void CheckDropDraggedItem()
		{
			if (this.draggedItem != null)
			{
				if (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseUp)
				{
					this.droppedDraggedItem = true;
				}
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x001BECD0 File Offset: 0x001BD0D0
		private bool IsVisibleWeapon(ThingDef t)
		{
			return t.IsWeapon && t != ThingDefOf.WoodLog && t != ThingDefOf.Beer;
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x001BED0C File Offset: 0x001BD10C
		private Pawn CurrentWearerOf(Thing t)
		{
			IThingHolder parentHolder = t.ParentHolder;
			Pawn result;
			if (parentHolder is Pawn_EquipmentTracker || parentHolder is Pawn_ApparelTracker)
			{
				result = (Pawn)parentHolder.ParentHolder;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x001BED50 File Offset: 0x001BD150
		private void MoveDraggedItemToInventory()
		{
			this.droppedDraggedItem = false;
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(this.draggedItem, this.Pawns, null, null);
			if (pawn != null)
			{
				this.draggedItem.holdingOwner.TryTransferToContainer(this.draggedItem, pawn.inventory.innerContainer, 1, true);
			}
			else
			{
				Log.Warning("Could not find any pawn to move " + this.draggedItem + " to.", false);
			}
			this.draggedItem = null;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x001BEDCC File Offset: 0x001BD1CC
		private void TryEquipDraggedItem(Pawn p)
		{
			this.droppedDraggedItem = false;
			if (this.draggedItem.def.IsWeapon)
			{
				if (p.story != null && p.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					Messages.Message("MessageCantEquipIncapableOfViolence".Translate(new object[]
					{
						p.LabelShort
					}), p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					Messages.Message("MessageCantEquipIncapableOfManipulation".Translate(), p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
			}
			Apparel apparel = this.draggedItem as Apparel;
			ThingWithComps thingWithComps = this.draggedItem as ThingWithComps;
			if (apparel != null && p.apparel != null)
			{
				WITab_Caravan_Gear.tmpExistingApparel.Clear();
				WITab_Caravan_Gear.tmpExistingApparel.AddRange(p.apparel.WornApparel);
				for (int i = 0; i < WITab_Caravan_Gear.tmpExistingApparel.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(apparel.def, WITab_Caravan_Gear.tmpExistingApparel[i].def, p.RaceProps.body))
					{
						p.apparel.Remove(WITab_Caravan_Gear.tmpExistingApparel[i]);
						Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(WITab_Caravan_Gear.tmpExistingApparel[i], this.Pawns, null, null);
						if (pawn != null)
						{
							pawn.inventory.innerContainer.TryAdd(WITab_Caravan_Gear.tmpExistingApparel[i], true);
						}
						else
						{
							Log.Warning("Could not find any pawn to move " + WITab_Caravan_Gear.tmpExistingApparel[i] + " to.", false);
							WITab_Caravan_Gear.tmpExistingApparel[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				p.apparel.Wear((Apparel)apparel.SplitOff(1), false);
				if (p.outfits != null)
				{
					p.outfits.forcedHandler.SetForced(apparel, true);
				}
			}
			else if (thingWithComps != null && p.equipment != null)
			{
				WITab_Caravan_Gear.tmpExistingEquipment.Clear();
				WITab_Caravan_Gear.tmpExistingEquipment.AddRange(p.equipment.AllEquipmentListForReading);
				for (int j = 0; j < WITab_Caravan_Gear.tmpExistingEquipment.Count; j++)
				{
					p.equipment.Remove(WITab_Caravan_Gear.tmpExistingEquipment[j]);
					Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(WITab_Caravan_Gear.tmpExistingEquipment[j], this.Pawns, null, null);
					if (pawn2 != null)
					{
						pawn2.inventory.innerContainer.TryAdd(WITab_Caravan_Gear.tmpExistingEquipment[j], true);
					}
					else
					{
						Log.Warning("Could not find any pawn to move " + WITab_Caravan_Gear.tmpExistingEquipment[j] + " to.", false);
						WITab_Caravan_Gear.tmpExistingEquipment[j].Destroy(DestroyMode.Vanish);
					}
				}
				p.equipment.AddEquipment((ThingWithComps)thingWithComps.SplitOff(1));
			}
			else
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not make ",
					p,
					" equip or wear ",
					this.draggedItem
				}), false);
			}
			this.draggedItem = null;
		}
	}
}
