using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000818 RID: 2072
	public static class InteractionCardUtility
	{
		// Token: 0x040018BC RID: 6332
		private static Vector2 logScrollPosition = Vector2.zero;

		// Token: 0x040018BD RID: 6333
		public const float ImageSize = 26f;

		// Token: 0x040018BE RID: 6334
		public const float ImagePadRight = 3f;

		// Token: 0x040018BF RID: 6335
		public const float TextOffset = 29f;

		// Token: 0x040018C0 RID: 6336
		private static List<Pair<string, int>> logStrings = new List<Pair<string, int>>();

		// Token: 0x06002E4F RID: 11855 RVA: 0x0018A4B0 File Offset: 0x001888B0
		public static void DrawInteractionsLog(Rect rect, Pawn pawn, List<LogEntry> entries, int maxEntries)
		{
			float width = rect.width - 29f - 16f - 10f;
			InteractionCardUtility.logStrings.Clear();
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].Concerns(pawn))
				{
					string text = entries[i].ToGameStringFromPOV(pawn, false);
					InteractionCardUtility.logStrings.Add(new Pair<string, int>(text, i));
					num += Mathf.Max(26f, Text.CalcHeight(text, width));
					num2++;
					if (num2 >= maxEntries)
					{
						break;
					}
				}
			}
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, num);
			Widgets.BeginScrollView(rect, ref InteractionCardUtility.logScrollPosition, viewRect, true);
			float num3 = 0f;
			for (int j = 0; j < InteractionCardUtility.logStrings.Count; j++)
			{
				string first = InteractionCardUtility.logStrings[j].First;
				LogEntry entry = entries[InteractionCardUtility.logStrings[j].Second];
				if (entry.Age > 7500)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.5f);
				}
				float num4 = Mathf.Max(26f, Text.CalcHeight(first, width));
				Texture2D texture2D = entry.IconFromPOV(pawn);
				if (texture2D != null)
				{
					Rect position = new Rect(0f, num3, 26f, 26f);
					GUI.DrawTexture(position, texture2D);
				}
				Rect rect2 = new Rect(29f, num3, width, num4);
				Widgets.DrawHighlightIfMouseover(rect2);
				Widgets.Label(rect2, first);
				TooltipHandler.TipRegion(rect2, () => entry.GetTipString(), 613261 + j * 611);
				if (Widgets.ButtonInvisible(rect2, false))
				{
					entry.ClickedFromPOV(pawn);
				}
				GUI.color = Color.white;
				num3 += num4;
			}
			Widgets.EndScrollView();
		}
	}
}
