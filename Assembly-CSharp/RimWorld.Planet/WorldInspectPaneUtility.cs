using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class WorldInspectPaneUtility
	{
		public static string AdjustedLabelFor(List<WorldObject> worldObjects, Rect rect)
		{
			return (worldObjects.Count != 1) ? ((!WorldInspectPaneUtility.AllLabelsAreSame(worldObjects)) ? "VariousLabel".Translate() : (worldObjects[0].LabelCap + " x" + worldObjects.Count)) : worldObjects[0].LabelCap;
		}

		private static bool AllLabelsAreSame(List<WorldObject> worldObjects)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < worldObjects.Count)
				{
					string labelCap = worldObjects[num].LabelCap;
					for (int i = num + 1; i < worldObjects.Count; i++)
					{
						if (labelCap != worldObjects[i].LabelCap)
							goto IL_0037;
					}
					num++;
					continue;
				}
				result = true;
				break;
				IL_0037:
				result = false;
				break;
			}
			return result;
		}
	}
}
