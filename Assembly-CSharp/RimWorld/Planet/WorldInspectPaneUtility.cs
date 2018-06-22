using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008EA RID: 2282
	public static class WorldInspectPaneUtility
	{
		// Token: 0x0600349F RID: 13471 RVA: 0x001C1BC4 File Offset: 0x001BFFC4
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

		// Token: 0x060034A0 RID: 13472 RVA: 0x001C1C34 File Offset: 0x001C0034
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
