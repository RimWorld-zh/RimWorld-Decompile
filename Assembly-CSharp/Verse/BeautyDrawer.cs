using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5C RID: 3676
	internal static class BeautyDrawer
	{
		// Token: 0x060056AB RID: 22187 RVA: 0x002CAE2D File Offset: 0x002C922D
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type == EventType.Repaint && BeautyDrawer.ShouldShow())
			{
				BeautyDrawer.DrawBeautyAroundMouse();
			}
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x002CAE54 File Offset: 0x002C9254
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x002CAEC0 File Offset: 0x002C92C0
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

		// Token: 0x060056AE RID: 22190 RVA: 0x002CAF74 File Offset: 0x002C9374
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			Color a = Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num);
			return Color.Lerp(a, Color.white, 0.5f);
		}

		// Token: 0x04003964 RID: 14692
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x04003965 RID: 14693
		private static Color ColorUgly = Color.red;

		// Token: 0x04003966 RID: 14694
		private static Color ColorBeautiful = Color.green;
	}
}
