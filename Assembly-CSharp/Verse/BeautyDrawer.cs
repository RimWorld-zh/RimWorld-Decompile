using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5F RID: 3679
	internal static class BeautyDrawer
	{
		// Token: 0x0400396C RID: 14700
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x0400396D RID: 14701
		private static Color ColorUgly = Color.red;

		// Token: 0x0400396E RID: 14702
		private static Color ColorBeautiful = Color.green;

		// Token: 0x060056AF RID: 22191 RVA: 0x002CB145 File Offset: 0x002C9545
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type == EventType.Repaint && BeautyDrawer.ShouldShow())
			{
				BeautyDrawer.DrawBeautyAroundMouse();
			}
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x002CB16C File Offset: 0x002C956C
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x002CB1D8 File Offset: 0x002C95D8
		private static void DrawBeautyAroundMouse()
		{
			if (Find.PlaySettings.showBeauty)
			{
				BeautyUtility.FillBeautyRelevantCells(UI.MouseCell(), Find.CurrentMap);
				for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
				{
					IntVec3 intVec = BeautyUtility.beautyRelevantCells[i];
					float num = BeautyUtility.CellBeauty(intVec, Find.CurrentMap, BeautyDrawer.beautyCountedThings);
					if (num != 0f)
					{
						Vector3 v = GenMapUI.LabelDrawPosFor(intVec);
						GenMapUI.DrawThingLabel(v, Mathf.RoundToInt(num).ToStringCached(), BeautyDrawer.BeautyColor(num, 8f));
					}
				}
				BeautyDrawer.beautyCountedThings.Clear();
			}
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x002CB28C File Offset: 0x002C968C
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			Color a = Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num);
			return Color.Lerp(a, Color.white, 0.5f);
		}
	}
}
