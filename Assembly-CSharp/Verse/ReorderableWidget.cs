using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public static class ReorderableWidget
	{
		private struct ReorderableGroup
		{
			public Action<int, int> reorderedAction;
		}

		private struct ReorderableInstance
		{
			public int groupID;

			public Rect rect;

			public Rect absRect;
		}

		private static List<ReorderableGroup> groups = new List<ReorderableGroup>();

		private static List<ReorderableInstance> reorderables = new List<ReorderableInstance>();

		private static int draggingReorderable = -1;

		private static bool clicked;

		private static bool released;

		private static bool dragBegun;

		private static Vector2 clickedAt;

		private static Rect clickedInRect;

		private static int lastInsertAt = -1;

		private const float MinMouseMoveToHighlightReorderable = 5f;

		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.3f);

		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		private const float LineWidth = 2f;

		public static void ReorderableWidgetOnGUI()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (ReorderableWidget.clicked)
				{
					ReorderableWidget.draggingReorderable = -1;
					for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
					{
						ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[i];
						if (reorderableInstance.rect == ReorderableWidget.clickedInRect)
						{
							ReorderableWidget.draggingReorderable = i;
							ReorderableWidget.dragBegun = false;
							break;
						}
					}
					ReorderableWidget.clicked = false;
				}
				if (ReorderableWidget.draggingReorderable >= ReorderableWidget.reorderables.Count)
				{
					ReorderableWidget.draggingReorderable = -1;
				}
				ReorderableWidget.lastInsertAt = ReorderableWidget.CurrentInsertAt();
				if (ReorderableWidget.released)
				{
					ReorderableWidget.released = false;
					if (ReorderableWidget.lastInsertAt >= 0 && ReorderableWidget.lastInsertAt != ReorderableWidget.draggingReorderable)
					{
						SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
						List<ReorderableGroup> list = ReorderableWidget.groups;
						ReorderableInstance reorderableInstance2 = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable];
						ReorderableGroup reorderableGroup = list[reorderableInstance2.groupID];
						reorderableGroup.reorderedAction(ReorderableWidget.draggingReorderable, ReorderableWidget.lastInsertAt);
					}
					ReorderableWidget.draggingReorderable = -1;
					ReorderableWidget.lastInsertAt = -1;
				}
				ReorderableWidget.groups.Clear();
				ReorderableWidget.reorderables.Clear();
			}
		}

		public static int NewGroup(Action<int, int> reorderedAction)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			ReorderableGroup item = default(ReorderableGroup);
			item.reorderedAction = reorderedAction;
			ReorderableWidget.groups.Add(item);
			return ReorderableWidget.groups.Count - 1;
		}

		public static bool Reorderable(int groupID, Rect rect)
		{
			if (Event.current.type == EventType.Repaint)
			{
				ReorderableInstance item = default(ReorderableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				ReorderableWidget.reorderables.Add(item);
				int num = ReorderableWidget.reorderables.Count - 1;
				if (ReorderableWidget.draggingReorderable != -1 && Vector2.Distance(ReorderableWidget.clickedAt, Event.current.mousePosition) > 5.0)
				{
					if (!ReorderableWidget.dragBegun)
					{
						SoundDefOf.TickTiny.PlayOneShotOnCamera(null);
						ReorderableWidget.dragBegun = true;
					}
					if (ReorderableWidget.draggingReorderable == num)
					{
						GUI.color = ReorderableWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
					if (ReorderableWidget.lastInsertAt == num)
					{
						ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[ReorderableWidget.lastInsertAt];
						Rect rect2 = reorderableInstance.rect;
						Vector2 mousePosition = Event.current.mousePosition;
						float y = mousePosition.y;
						Vector2 center = rect2.center;
						bool flag = y < center.y;
						GUI.color = ReorderableWidget.LineColor;
						if (flag)
						{
							Widgets.DrawLine(rect2.position, new Vector2(rect2.x + rect2.width, rect2.y), ReorderableWidget.LineColor, 2f);
						}
						else
						{
							Widgets.DrawLine(new Vector2(rect2.x, rect2.yMax), new Vector2(rect2.x + rect2.width, rect2.yMax), ReorderableWidget.LineColor, 2f);
						}
						GUI.color = Color.white;
					}
				}
				return ReorderableWidget.draggingReorderable == num && ReorderableWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				ReorderableWidget.clicked = true;
				ReorderableWidget.clickedAt = Event.current.mousePosition;
				ReorderableWidget.clickedInRect = rect;
			}
			return false;
		}

		private static int CurrentInsertAt()
		{
			if (ReorderableWidget.draggingReorderable < 0)
			{
				return -1;
			}
			ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable];
			int groupID = reorderableInstance.groupID;
			if (groupID >= 0 && groupID < ReorderableWidget.groups.Count)
			{
				int num = -1;
				int num2 = -1;
				for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
				{
					ReorderableInstance reorderableInstance2 = ReorderableWidget.reorderables[i];
					if (reorderableInstance2.groupID == groupID)
					{
						int num3 = (i > ReorderableWidget.draggingReorderable) ? num2 : i;
						Rect rect = reorderableInstance2.absRect.TopHalf();
						if (rect.yMin > 0.0)
						{
							rect.yMin = 0f;
						}
						if (rect.Contains(Event.current.mousePosition))
						{
							num = num3;
							break;
						}
						if (num2 >= 0)
						{
							float x = reorderableInstance2.absRect.x;
							ReorderableInstance reorderableInstance3 = ReorderableWidget.reorderables[num2];
							float num4 = Mathf.Min(x, reorderableInstance3.absRect.x);
							float x2 = num4;
							ReorderableInstance reorderableInstance4 = ReorderableWidget.reorderables[num2];
							Vector2 center = reorderableInstance4.absRect.center;
							float y = center.y;
							float xMax = reorderableInstance2.absRect.xMax;
							ReorderableInstance reorderableInstance5 = ReorderableWidget.reorderables[num2];
							float width = Mathf.Max(xMax, reorderableInstance5.absRect.xMax) - num4;
							Vector2 center2 = reorderableInstance2.absRect.center;
							float y2 = center2.y;
							ReorderableInstance reorderableInstance6 = ReorderableWidget.reorderables[num2];
							Vector2 center3 = reorderableInstance6.absRect.center;
							Rect rect2 = new Rect(x2, y, width, y2 - center3.y);
							if (rect2.Contains(Event.current.mousePosition))
							{
								num = num3;
								break;
							}
						}
						num2 = i;
					}
				}
				if (num < 0 && num2 >= 0)
				{
					ReorderableInstance reorderableInstance7 = ReorderableWidget.reorderables[num2];
					Rect rect3 = reorderableInstance7.absRect.BottomHalf();
					if (rect3.yMax < (float)UI.screenHeight)
					{
						rect3.yMax = (float)UI.screenHeight;
					}
					if (rect3.Contains(Event.current.mousePosition))
					{
						num = num2;
					}
				}
				return num;
			}
			Log.ErrorOnce("Reorderable used invalid group.", 1968375560);
			return -1;
		}
	}
}
