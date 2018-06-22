using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008DA RID: 2266
	public static class FloatMenuMakerWorld
	{
		// Token: 0x060033FD RID: 13309 RVA: 0x001BCAF4 File Offset: 0x001BAEF4
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

		// Token: 0x060033FE RID: 13310 RVA: 0x001BCB58 File Offset: 0x001BAF58
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

		// Token: 0x060033FF RID: 13311 RVA: 0x001BCBA8 File Offset: 0x001BAFA8
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
