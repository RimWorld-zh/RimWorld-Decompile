using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5D RID: 3677
	internal static class BeautyDrawer
	{
		// Token: 0x0600568B RID: 22155 RVA: 0x002C921D File Offset: 0x002C761D
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type == EventType.Repaint && BeautyDrawer.ShouldShow())
			{
				BeautyDrawer.DrawBeautyAroundMouse();
			}
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x002C9244 File Offset: 0x002C7644
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x002C92B0 File Offset: 0x002C76B0
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

		// Token: 0x0600568E RID: 22158 RVA: 0x002C9364 File Offset: 0x002C7764
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			Color a = Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num);
			return Color.Lerp(a, Color.white, 0.5f);
		}

		// Token: 0x04003955 RID: 14677
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x04003956 RID: 14678
		private static Color ColorUgly = Color.red;

		// Token: 0x04003957 RID: 14679
		private static Color ColorBeautiful = Color.green;
	}
}
