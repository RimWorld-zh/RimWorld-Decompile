using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class WorldInspectPaneUtility
	{
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
