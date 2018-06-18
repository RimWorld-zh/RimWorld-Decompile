using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008EE RID: 2286
	public static class WorldInspectPaneUtility
	{
		// Token: 0x060034A6 RID: 13478 RVA: 0x001C19DC File Offset: 0x001BFDDC
		public static string AdjustedLabelFor(List<WorldObject> worldObjects, Rect rect)
		{
			string result;
			if (worldObjects.Count == 1)
			{
				result = worldObjects[0].LabelCap;
			}
			else if (WorldInspectPaneUtility.AllLabelsAreSame(worldObjects))
			{
				result = worldObjects[0].LabelCap + " x" + worldObjects.Count;
			}
			else
			{
				result = "VariousLabel".Translate();
			}
			return result;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x001C1A4C File Offset: 0x001BFE4C
		private static bool AllLabelsAreSame(List<WorldObject> worldObjects)
		{
			for (int i = 0; i < worldObjects.Count; i++)
			{
				string labelCap = worldObjects[i].LabelCap;
				for (int j = i + 1; j < worldObjects.Count; j++)
				{
					if (labelCap != worldObjects[j].LabelCap)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
