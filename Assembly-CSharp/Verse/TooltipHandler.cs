using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E94 RID: 3732
	public static class TooltipHandler
	{
		// Token: 0x04003A44 RID: 14916
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		// Token: 0x04003A45 RID: 14917
		private static int frame = 0;

		// Token: 0x04003A46 RID: 14918
		private static List<int> dyingTips = new List<int>(32);

		// Token: 0x04003A47 RID: 14919
		private static float TooltipDelay = 0.45f;

		// Token: 0x04003A48 RID: 14920
		private const float SpaceBetweenTooltips = 2f;

		// Token: 0x04003A49 RID: 14921
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		// Token: 0x04003A4A RID: 14922
		[CompilerGenerated]
		private static Comparison<ActiveTip> <>f__mg$cache0;

		// Token: 0x06005812 RID: 22546 RVA: 0x002D2CC0 File Offset: 0x002D10C0
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

		// Token: 0x06005813 RID: 22547 RVA: 0x002D2DA0 File Offset: 0x002D11A0
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		// Token: 0x06005814 RID: 22548 RVA: 0x002D2DB0 File Offset: 0x002D11B0
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

		// Token: 0x06005815 RID: 22549 RVA: 0x002D2EC2 File Offset: 0x002D12C2
		public static void DoTooltipGUI()
		{
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x002D2EF0 File Offset: 0x002D12F0
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

		// Token: 0x06005817 RID: 22551 RVA: 0x002D300C File Offset: 0x002D140C
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

		// Token: 0x06005818 RID: 22552 RVA: 0x002D30C8 File Offset: 0x002D14C8
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

		// Token: 0x06005819 RID: 22553 RVA: 0x002D3148 File Offset: 0x002D1548
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
