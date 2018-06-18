using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008DE RID: 2270
	public static class FloatMenuMakerWorld
	{
		// Token: 0x06003404 RID: 13316 RVA: 0x001BC90C File Offset: 0x001BAD0C
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

		// Token: 0x06003405 RID: 13317 RVA: 0x001BC970 File Offset: 0x001BAD70
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

		// Token: 0x06003406 RID: 13318 RVA: 0x001BC9C0 File Offset: 0x001BADC0
		public static List<FloatMenuOption> ChoicesAtFor(int tile, Caravan caravan)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile)
				{
					list.AddRange(allWorldObjects[i].GetFloatMenuOptions(caravan));
				}
			}
			return list;
		}
	}
}
