using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	internal static class BeautyDrawer
	{
		private static List<Thing> beautyCountedThings = new List<Thing>();

		private static Color ColorUgly = Color.red;

		private static Color ColorBeautiful = Color.green;

		public static void DrawBeautyAroundMouse()
		{
			BeautyUtility.FillBeautyRelevantCells(UI.MouseCell(), Find.VisibleMap);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				IntVec3 intVec = BeautyUtility.beautyRelevantCells[i];
				float num = BeautyUtility.CellBeauty(intVec, Find.VisibleMap, BeautyDrawer.beautyCountedThings);
				if (num != 0.0)
				{
					Vector3 v = GenMapUI.LabelDrawPosFor(intVec);
					GenMapUI.DrawThingLabel(v, Mathf.RoundToInt(num).ToStringCached(), BeautyDrawer.BeautyColor(num, 8f));
				}
			}
			BeautyDrawer.beautyCountedThings.Clear();
		}

		public static Color BeautyColor(float beauty, float scale)
		{
			float value = Mathf.InverseLerp((float)(0.0 - scale), scale, beauty);
			value = Mathf.Clamp01(value);
			Color a = Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, value);
			return Color.Lerp(a, Color.white, 0.5f);
		}
	}
}
