using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public static class TooltipHandler
	{
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		private static int frame = 0;

		private static List<int> dyingTips = new List<int>(32);

		private static float TooltipDelay = 0.45f;

		private const float SpaceBetweenTooltips = 2f;

		[CompilerGenerated]
		private static Comparison<ActiveTip> _003C_003Ef__mg_0024cache0;

		public static void ClearTooltipsFrom(Rect rect)
		{
			if (Event.current.type == EventType.Repaint && Mouse.IsOver(rect))
			{
				TooltipHandler.dyingTips.Clear();
				foreach (KeyValuePair<int, ActiveTip> activeTip in TooltipHandler.activeTips)
				{
					if (activeTip.Value.lastTriggerFrame == TooltipHandler.frame)
					{
						TooltipHandler.dyingTips.Add(activeTip.Key);
					}
				}
				for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
				{
					TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
				}
			}
		}

		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		public static void TipRegion(Rect rect, TipSignal tip)
		{
			if (Event.current.type == EventType.Repaint && Mouse.IsOver(rect))
			{
				if (DebugViewSettings.drawTooltipEdges)
				{
					Widgets.DrawBox(rect, 1);
				}
				if (!TooltipHandler.activeTips.ContainsKey(tip.uniqueId))
				{
					ActiveTip value = new ActiveTip(tip);
					TooltipHandler.activeTips.Add(tip.uniqueId, value);
					TooltipHandler.activeTips[tip.uniqueId].firstTriggerTime = (double)Time.realtimeSinceStartup;
				}
				TooltipHandler.activeTips[tip.uniqueId].lastTriggerFrame = TooltipHandler.frame;
				TooltipHandler.activeTips[tip.uniqueId].signal.text = tip.text;
				TooltipHandler.activeTips[tip.uniqueId].signal.textGetter = tip.textGetter;
			}
		}

		public static void DoTooltipGUI()
		{
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		private static void DrawActiveTips()
		{
			List<ActiveTip> list = new List<ActiveTip>();
			foreach (KeyValuePair<int, ActiveTip> activeTip in TooltipHandler.activeTips)
			{
				if ((double)Time.realtimeSinceStartup > activeTip.Value.firstTriggerTime + (double)TooltipHandler.TooltipDelay)
				{
					list.Add(activeTip.Value);
				}
			}
			list.Sort(new Comparison<ActiveTip>(TooltipHandler.CompareTooltipsByPriority));
			Vector2 pos = TooltipHandler.CalculateInitialTipPosition(list);
			for (int i = 0; i < list.Count; i++)
			{
				pos.y += list[i].DrawTooltip(pos);
				pos.y += 2f;
			}
		}

		private static void CleanActiveTooltips()
		{
			TooltipHandler.dyingTips.Clear();
			foreach (KeyValuePair<int, ActiveTip> activeTip in TooltipHandler.activeTips)
			{
				if (activeTip.Value.lastTriggerFrame != TooltipHandler.frame)
				{
					TooltipHandler.dyingTips.Add(activeTip.Key);
				}
			}
			for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
			{
				TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
			}
		}

		private static Vector2 CalculateInitialTipPosition(List<ActiveTip> drawingTips)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < drawingTips.Count; i++)
			{
				Rect tipRect = drawingTips[i].TipRect;
				num += tipRect.height;
				num2 = Mathf.Max(num2, tipRect.width);
				if (i != drawingTips.Count - 1)
				{
					num = (float)(num + 2.0);
				}
			}
			Vector3 vector = Event.current.mousePosition;
			float num3 = 0f;
			num3 = (float)((!(vector.y + 14.0 + num < (float)UI.screenHeight)) ? ((!(vector.y - 5.0 - num >= 0.0)) ? 0.0 : (vector.y - 5.0 - num)) : (vector.y + 14.0));
			float num4 = 0f;
			num4 = (float)((!(vector.x + 16.0 + num2 < (float)UI.screenWidth)) ? (vector.x - 4.0 - num2) : (vector.x + 16.0));
			return new Vector2(num4, num3);
		}

		private static int CompareTooltipsByPriority(ActiveTip A, ActiveTip B)
		{
			return (A.signal.priority != B.signal.priority) ? ((A.signal.priority != TooltipPriority.Pawn) ? ((B.signal.priority == TooltipPriority.Pawn) ? 1 : 0) : (-1)) : 0;
		}
	}
}
