using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008DD RID: 2269
	public static class GenWorldUI
	{
		// Token: 0x04001C0E RID: 7182
		private static List<Caravan> clickedCaravans = new List<Caravan>();

		// Token: 0x04001C0F RID: 7183
		private static List<WorldObject> clickedDynamicallyDrawnObjects = new List<WorldObject>();

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x001BCD50 File Offset: 0x001BB150
		public static float CaravanDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x001BCD78 File Offset: 0x001BB178
		private static float CaravanWideClickRadius
		{
			get
			{
				return 0.75f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x001BCDA0 File Offset: 0x001BB1A0
		private static float DynamicallyDrawnObjectDirectClickRadius
		{
			get
			{
				return 0.35f * Find.WorldGrid.averageTileSize;
			}
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x001BCDC8 File Offset: 0x001BB1C8
		public static List<WorldObject> WorldObjectsUnderMouse(Vector2 mousePos)
		{
			List<WorldObject> list = new List<WorldObject>();
			ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(mousePos, list);
			float caravanDirectClickRadius = GenWorldUI.CaravanDirectClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.DistanceToMouse(mousePos) < caravanDirectClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int j = 0; j < GenWorldUI.clickedCaravans.Count; j++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[j]))
				{
					list.Add(GenWorldUI.clickedCaravans[j]);
				}
			}
			float dynamicallyDrawnObjectDirectClickRadius = GenWorldUI.DynamicallyDrawnObjectDirectClickRadius;
			GenWorldUI.clickedDynamicallyDrawnObjects.Clear();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int k = 0; k < allWorldObjects.Count; k++)
			{
				WorldObject worldObject = allWorldObjects[k];
				if (worldObject.def.useDynamicDrawer && worldObject.DistanceToMouse(mousePos) < dynamicallyDrawnObjectDirectClickRadius)
				{
					GenWorldUI.clickedDynamicallyDrawnObjects.Add(worldObject);
				}
			}
			GenWorldUI.clickedDynamicallyDrawnObjects.SortBy((WorldObject x) => x.DistanceToMouse(mousePos));
			for (int l = 0; l < GenWorldUI.clickedDynamicallyDrawnObjects.Count; l++)
			{
				if (!list.Contains(GenWorldUI.clickedDynamicallyDrawnObjects[l]))
				{
					list.Add(GenWorldUI.clickedDynamicallyDrawnObjects[l]);
				}
			}
			int num = GenWorld.TileAt(mousePos, false);
			List<WorldObject> allWorldObjects2 = Find.WorldObjects.AllWorldObjects;
			for (int m = 0; m < allWorldObjects2.Count; m++)
			{
				if (allWorldObjects2[m].Tile == num && !list.Contains(allWorldObjects2[m]))
				{
					list.Add(allWorldObjects2[m]);
				}
			}
			float caravanWideClickRadius = GenWorldUI.CaravanWideClickRadius;
			GenWorldUI.clickedCaravans.Clear();
			List<Caravan> caravans2 = Find.WorldObjects.Caravans;
			for (int n = 0; n < caravans2.Count; n++)
			{
				Caravan caravan2 = caravans2[n];
				if (caravan2.DistanceToMouse(mousePos) < caravanWideClickRadius)
				{
					GenWorldUI.clickedCaravans.Add(caravan2);
				}
			}
			GenWorldUI.clickedCaravans.SortBy((Caravan x) => x.DistanceToMouse(mousePos));
			for (int num2 = 0; num2 < GenWorldUI.clickedCaravans.Count; num2++)
			{
				if (!list.Contains(GenWorldUI.clickedCaravans[num2]))
				{
					list.Add(GenWorldUI.clickedCaravans[num2]);
				}
			}
			GenWorldUI.clickedCaravans.Clear();
			return list;
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x001BD0E0 File Offset: 0x001BB4E0
		public static Vector2 WorldToUIPosition(Vector3 worldLoc)
		{
			Vector3 vector = Find.WorldCamera.WorldToScreenPoint(worldLoc) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}
	}
}
