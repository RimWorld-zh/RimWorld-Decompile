using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E93 RID: 3731
	public static class TooltipHandler
	{
		// Token: 0x04003A3C RID: 14908
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		// Token: 0x04003A3D RID: 14909
		private static int frame = 0;

		// Token: 0x04003A3E RID: 14910
		private static List<int> dyingTips = new List<int>(32);

		// Token: 0x04003A3F RID: 14911
		private static float TooltipDelay = 0.45f;

		// Token: 0x04003A40 RID: 14912
		private const float SpaceBetweenTooltips = 2f;

		// Token: 0x04003A41 RID: 14913
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		// Token: 0x04003A42 RID: 14914
		[CompilerGenerated]
		private static Comparison<ActiveTip> <>f__mg$cache0;

		// Token: 0x06005812 RID: 22546 RVA: 0x002D2AD4 File Offset: 0x002D0ED4
		public static void ClearTooltipsFrom(Rect rect)
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.dyingTips.Clear();
					foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
					{
						if (keyValuePair.Value.lastTriggerFrame == TooltipHandler.frame)
						{
							TooltipHandler.dyingTips.Add(keyValuePair.Key);
						}
					}
					for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
					{
						TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
					}
				}
			}
		}

		// Token: 0x06005813 RID: 22547 RVA: 0x002D2BB4 File Offset: 0x002D0FB4
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		// Token: 0x06005814 RID: 22548 RVA: 0x002D2BC4 File Offset: 0x002D0FC4
		public static void TipRegion(Rect rect, TipSignal tip)
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (tip.textGetter != null || !tip.text.NullOrEmpty())
				{
					if (Mouse.IsOver(rect))
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
			}
		}

		// Token: 0x06005815 RID: 22549 RVA: 0x002D2CD6 File Offset: 0x002D10D6
		public static void DoTooltipGUI()
		{
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x002D2D04 File Offset: 0x002D1104
		private static void DrawActiveTips()
		{
			TooltipHandler.drawingTips.Clear();
			foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
			{
				if ((double)Time.realtimeSinceStartup > keyValuePair.Value.firstTriggerTime + (double)TooltipHandler.TooltipDelay)
				{
					TooltipHandler.drawingTips.Add(keyValuePair.Value);
				}
			}
			List<ActiveTip> list = TooltipHandler.drawingTips;
			if (TooltipHandler.<>f__mg$cache0 == null)
			{
				TooltipHandler.<>f__mg$cache0 = new Comparison<ActiveTip>(TooltipHandler.CompareTooltipsByPriority);
			}
			list.Sort(TooltipHandler.<>f__mg$cache0);
			Vector2 pos = TooltipHandler.CalculateInitialTipPosition(TooltipHandler.drawingTips);
			for (int i = 0; i < TooltipHandler.drawingTips.Count; i++)
			{
				pos.y += TooltipHandler.drawingTips[i].DrawTooltip(pos);
				pos.y += 2f;
			}
			TooltipHandler.drawingTips.Clear();
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x002D2E20 File Offset: 0x002D1220
		private static void CleanActiveTooltips()
		{
			TooltipHandler.dyingTips.Clear();
			foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
			{
				if (keyValuePair.Value.lastTriggerFrame != TooltipHandler.frame)
				{
					TooltipHandler.dyingTips.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
			{
				TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
			}
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x002D2EDC File Offset: 0x002D12DC
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
					num += 2f;
				}
			}
			return GenUI.GetMouseAttachedWindowPos(num2, num);
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x002D2F5C File Offset: 0x002D135C
		private static int CompareTooltipsByPriority(ActiveTip A, ActiveTip B)
		{
			int result;
			if (A.signal.priority == B.signal.priority)
			{
				result = 0;
			}
			else if (A.signal.priority == TooltipPriority.Pawn)
			{
				result = -1;
			}
			else if (B.signal.priority == TooltipPriority.Pawn)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
