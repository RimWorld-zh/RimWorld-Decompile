using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E92 RID: 3730
	public static class TooltipHandler
	{
		// Token: 0x060057EE RID: 22510 RVA: 0x002D0D98 File Offset: 0x002CF198
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

		// Token: 0x060057EF RID: 22511 RVA: 0x002D0E78 File Offset: 0x002CF278
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		// Token: 0x060057F0 RID: 22512 RVA: 0x002D0E88 File Offset: 0x002CF288
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

		// Token: 0x060057F1 RID: 22513 RVA: 0x002D0F9A File Offset: 0x002CF39A
		public static void DoTooltipGUI()
		{
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x002D0FC8 File Offset: 0x002CF3C8
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

		// Token: 0x060057F3 RID: 22515 RVA: 0x002D10E4 File Offset: 0x002CF4E4
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

		// Token: 0x060057F4 RID: 22516 RVA: 0x002D11A0 File Offset: 0x002CF5A0
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

		// Token: 0x060057F5 RID: 22517 RVA: 0x002D1220 File Offset: 0x002CF620
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

		// Token: 0x04003A2C RID: 14892
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		// Token: 0x04003A2D RID: 14893
		private static int frame = 0;

		// Token: 0x04003A2E RID: 14894
		private static List<int> dyingTips = new List<int>(32);

		// Token: 0x04003A2F RID: 14895
		private static float TooltipDelay = 0.45f;

		// Token: 0x04003A30 RID: 14896
		private const float SpaceBetweenTooltips = 2f;

		// Token: 0x04003A31 RID: 14897
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		// Token: 0x04003A32 RID: 14898
		[CompilerGenerated]
		private static Comparison<ActiveTip> <>f__mg$cache0;
	}
}
