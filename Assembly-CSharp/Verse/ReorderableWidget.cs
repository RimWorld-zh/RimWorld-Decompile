using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EA4 RID: 3748
	public static class ReorderableWidget
	{
		// Token: 0x04003A84 RID: 14980
		private static List<ReorderableWidget.ReorderableGroup> groups = new List<ReorderableWidget.ReorderableGroup>();

		// Token: 0x04003A85 RID: 14981
		private static List<ReorderableWidget.ReorderableInstance> reorderables = new List<ReorderableWidget.ReorderableInstance>();

		// Token: 0x04003A86 RID: 14982
		private static int draggingReorderable = -1;

		// Token: 0x04003A87 RID: 14983
		private static Vector2 dragStartPos;

		// Token: 0x04003A88 RID: 14984
		private static bool clicked;

		// Token: 0x04003A89 RID: 14985
		private static bool released;

		// Token: 0x04003A8A RID: 14986
		private static bool dragBegun;

		// Token: 0x04003A8B RID: 14987
		private static Vector2 clickedAt;

		// Token: 0x04003A8C RID: 14988
		private static Rect clickedInRect;

		// Token: 0x04003A8D RID: 14989
		private static int lastInsertNear = -1;

		// Token: 0x04003A8E RID: 14990
		private static bool lastInsertNearLeft;

		// Token: 0x04003A8F RID: 14991
		private static int lastFrameReorderableCount = -1;

		// Token: 0x04003A90 RID: 14992
		private const float MinMouseMoveToHighlightReorderable = 5f;

		// Token: 0x04003A91 RID: 14993
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x04003A92 RID: 14994
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04003A93 RID: 14995
		private const float LineWidth = 2f;

		// Token: 0x06005870 RID: 22640 RVA: 0x002D54EC File Offset: 0x002D38EC
		public static void ReorderableWidgetOnGUI_BeforeWindowStack()
		{
			if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.draggingReorderable < ReorderableWidget.reorderables.Count)
			{
				int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
				if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					ReorderableWidget.groups[groupID].extraDraggedItemOnGUI(ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable), ReorderableWidget.dragStartPos);
				}
			}
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x002D5594 File Offset: 0x002D3994
		public static void ReorderableWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (ReorderableWidget.clicked)
				{
					ReorderableWidget.StopDragging();
					for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
					{
						if (ReorderableWidget.reorderables[i].rect == ReorderableWidget.clickedInRect)
						{
							ReorderableWidget.draggingReorderable = i;
							ReorderableWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					ReorderableWidget.clicked = false;
				}
				if (ReorderableWidget.draggingReorderable >= ReorderableWidget.reorderables.Count)
				{
					ReorderableWidget.StopDragging();
				}
				if (ReorderableWidget.reorderables.Count != ReorderableWidget.lastFrameReorderableCount)
				{
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastInsertNear = ReorderableWidget.CurrentInsertNear(out ReorderableWidget.lastInsertNearLeft);
				if (ReorderableWidget.released)
				{
					ReorderableWidget.released = false;
					if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0)
					{
						int indexWithinGroup = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
						int num;
						if (ReorderableWidget.lastInsertNear == ReorderableWidget.draggingReorderable)
						{
							num = indexWithinGroup;
						}
						else if (ReorderableWidget.lastInsertNearLeft)
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear);
						}
						else
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear) + 1;
						}
						if (num >= 0 && num != indexWithinGroup && num != indexWithinGroup + 1)
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							try
							{
								ReorderableWidget.groups[ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID].reorderedAction(indexWithinGroup, num);
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Could not reorder elements (from ",
									indexWithinGroup,
									" to ",
									num,
									"): ",
									ex
								}), false);
							}
						}
					}
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastFrameReorderableCount = ReorderableWidget.reorderables.Count;
				ReorderableWidget.groups.Clear();
				ReorderableWidget.reorderables.Clear();
			}
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x002D57CC File Offset: 0x002D3BCC
		public static int NewGroup(Action<int, int> reorderedAction, ReorderableDirection direction, float drawLineExactlyBetween_space = -1f, Action<int, Vector2> extraDraggedItemOnGUI = null)
		{
			int result;
			if (Event.current.type != EventType.Repaint)
			{
				result = -1;
			}
			else
			{
				ReorderableWidget.ReorderableGroup item = default(ReorderableWidget.ReorderableGroup);
				item.reorderedAction = reorderedAction;
				item.direction = direction;
				item.drawLineExactlyBetween_space = drawLineExactlyBetween_space;
				item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
				ReorderableWidget.groups.Add(item);
				result = ReorderableWidget.groups.Count - 1;
			}
			return result;
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x002D5838 File Offset: 0x002D3C38
		public static bool Reorderable(int groupID, Rect rect, bool useRightButton = false)
		{
			bool result;
			if (Event.current.type == EventType.Repaint)
			{
				ReorderableWidget.ReorderableInstance item = default(ReorderableWidget.ReorderableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				ReorderableWidget.reorderables.Add(item);
				int num = ReorderableWidget.reorderables.Count - 1;
				if (ReorderableWidget.draggingReorderable != -1 && (ReorderableWidget.dragBegun || Vector2.Distance(ReorderableWidget.clickedAt, Event.current.mousePosition) > 5f))
				{
					if (!ReorderableWidget.dragBegun)
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						ReorderableWidget.dragBegun = true;
					}
					if (ReorderableWidget.draggingReorderable == num)
					{
						GUI.color = ReorderableWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
					if (ReorderableWidget.lastInsertNear == num && groupID >= 0 && groupID < ReorderableWidget.groups.Count)
					{
						Rect rect2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].rect;
						ReorderableWidget.ReorderableGroup reorderableGroup = ReorderableWidget.groups[groupID];
						if (reorderableGroup.DrawLineExactlyBetween)
						{
							if (reorderableGroup.direction == ReorderableDirection.Horizontal)
							{
								rect2.xMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
								rect2.xMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
							}
							else
							{
								rect2.yMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
								rect2.yMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
							}
						}
						GUI.color = ReorderableWidget.LineColor;
						if (reorderableGroup.direction == ReorderableDirection.Horizontal)
						{
							if (ReorderableWidget.lastInsertNearLeft)
							{
								Widgets.DrawLine(rect2.position, new Vector2(rect2.x, rect2.yMax), ReorderableWidget.LineColor, 2f);
							}
							else
							{
								Widgets.DrawLine(new Vector2(rect2.xMax, rect2.y), new Vector2(rect2.xMax, rect2.yMax), ReorderableWidget.LineColor, 2f);
							}
						}
						else if (ReorderableWidget.lastInsertNearLeft)
						{
							Widgets.DrawLine(rect2.position, new Vector2(rect2.xMax, rect2.y), ReorderableWidget.LineColor, 2f);
						}
						else
						{
							Widgets.DrawLine(new Vector2(rect2.x, rect2.yMax), new Vector2(rect2.xMax, rect2.yMax), ReorderableWidget.LineColor, 2f);
						}
						GUI.color = Color.white;
					}
				}
				result = (ReorderableWidget.draggingReorderable == num && ReorderableWidget.dragBegun);
			}
			else
			{
				if (Event.current.rawType == EventType.MouseUp)
				{
					ReorderableWidget.released = true;
				}
				if (Event.current.type == EventType.MouseDown && ((useRightButton && Event.current.button == 1) || (!useRightButton && Event.current.button == 0)) && Mouse.IsOver(rect))
				{
					ReorderableWidget.clicked = true;
					ReorderableWidget.clickedAt = Event.current.mousePosition;
					ReorderableWidget.clickedInRect = rect;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x002D5B98 File Offset: 0x002D3F98
		private static int CurrentInsertNear(out bool toTheLeft)
		{
			toTheLeft = false;
			int result;
			if (ReorderableWidget.draggingReorderable < 0)
			{
				result = -1;
			}
			else
			{
				int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
				if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
				{
					Log.ErrorOnce("Reorderable used invalid group.", 1968375560, false);
					result = -1;
				}
				else
				{
					int num = -1;
					for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
					{
						ReorderableWidget.ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[i];
						if (reorderableInstance.groupID == groupID)
						{
							if (num == -1 || Event.current.mousePosition.DistanceToRect(reorderableInstance.absRect) < Event.current.mousePosition.DistanceToRect(ReorderableWidget.reorderables[num].absRect))
							{
								num = i;
							}
						}
					}
					if (num >= 0)
					{
						ReorderableWidget.ReorderableInstance reorderableInstance2 = ReorderableWidget.reorderables[num];
						if (ReorderableWidget.groups[reorderableInstance2.groupID].direction == ReorderableDirection.Horizontal)
						{
							toTheLeft = (Event.current.mousePosition.x < reorderableInstance2.absRect.center.x);
						}
						else
						{
							toTheLeft = (Event.current.mousePosition.y < reorderableInstance2.absRect.center.y);
						}
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x002D5D2C File Offset: 0x002D412C
		private static int GetIndexWithinGroup(int index)
		{
			int result;
			if (index < 0 || index >= ReorderableWidget.reorderables.Count)
			{
				result = -1;
			}
			else
			{
				int num = -1;
				for (int i = 0; i <= index; i++)
				{
					if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.reorderables[index].groupID)
					{
						num++;
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x002D5DA8 File Offset: 0x002D41A8
		private static void StopDragging()
		{
			ReorderableWidget.draggingReorderable = -1;
			ReorderableWidget.dragStartPos = default(Vector2);
			ReorderableWidget.lastInsertNear = -1;
			ReorderableWidget.dragBegun = false;
		}

		// Token: 0x02000EA5 RID: 3749
		private struct ReorderableGroup
		{
			// Token: 0x04003A94 RID: 14996
			public Action<int, int> reorderedAction;

			// Token: 0x04003A95 RID: 14997
			public ReorderableDirection direction;

			// Token: 0x04003A96 RID: 14998
			public float drawLineExactlyBetween_space;

			// Token: 0x04003A97 RID: 14999
			public Action<int, Vector2> extraDraggedItemOnGUI;

			// Token: 0x17000DF4 RID: 3572
			// (get) Token: 0x06005878 RID: 22648 RVA: 0x002D5E48 File Offset: 0x002D4248
			public bool DrawLineExactlyBetween
			{
				get
				{
					return this.drawLineExactlyBetween_space > 0f;
				}
			}
		}

		// Token: 0x02000EA6 RID: 3750
		private struct ReorderableInstance
		{
			// Token: 0x04003A98 RID: 15000
			public int groupID;

			// Token: 0x04003A99 RID: 15001
			public Rect rect;

			// Token: 0x04003A9A RID: 15002
			public Rect absRect;
		}
	}
}
