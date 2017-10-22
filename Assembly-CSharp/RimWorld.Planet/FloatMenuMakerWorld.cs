using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class FloatMenuMakerWorld
	{
		public static bool TryMakeFloatMenu(Caravan caravan)
		{
			bool result;
			if (!caravan.IsPlayerControlled)
			{
				result = false;
			}
			else
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(mousePositionOnUI, caravan);
				if (list.Count == 0)
				{
					result = false;
				}
				else
				{
					FloatMenuWorld window = new FloatMenuWorld(list, caravan.LabelCap, mousePositionOnUI);
					Find.WindowStack.Add(window);
					result = true;
				}
			}
			return result;
		}

		public static List<FloatMenuOption> ChoicesAtFor(Vector2 mousePos, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> list2 = GenWorldUI.WorldObjectsUnderMouse(mousePos);
			for (int i = 0; i < list2.Count; i++)
			{
				list.AddRange(list2[i].GetFloatMenuOptions(caravan));
			}
			return list;
		}
	}
}
