using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5E RID: 3678
	internal static class BeautyDrawer
	{
		// Token: 0x04003964 RID: 14692
		private static List<Thing> beautyCountedThings = new List<Thing>();

		// Token: 0x04003965 RID: 14693
		private static Color ColorUgly = Color.red;

		// Token: 0x04003966 RID: 14694
		private static Color ColorBeautiful = Color.green;

		// Token: 0x060056AF RID: 22191 RVA: 0x002CAF59 File Offset: 0x002C9359
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type == EventType.Repaint && BeautyDrawer.ShouldShow())
			{
				BeautyDrawer.DrawBeautyAroundMouse();
			}
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x002CAF80 File Offset: 0x002C9380
		private static bool ShouldShow()
		{
			return Find.PlaySettings.showBeauty && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x002CAFEC File Offset: 0x002C93EC
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

		// Token: 0x060056B2 RID: 22194 RVA: 0x002CB0A0 File Offset: 0x002C94A0
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			Color a = Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num);
			return Color.Lerp(a, Color.white, 0.5f);
		}
	}
}
