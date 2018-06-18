using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F0 RID: 1520
	public static class CaravanTweenerUtility
	{
		// Token: 0x06001E2E RID: 7726 RVA: 0x001038B8 File Offset: 0x00101CB8
		public static Vector3 PatherTweenedPosRoot(Caravan caravan)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 result;
			if (!caravan.Spawned)
			{
				result = worldGrid.GetTileCenter(caravan.Tile);
			}
			else if (caravan.pather.Moving)
			{
				float num;
				if (!caravan.pather.IsNextTilePassable())
				{
					num = 0f;
				}
				else
				{
					num = 1f - caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				}
				int tileID;
				if (caravan.pather.nextTile == caravan.Tile && caravan.pather.previousTileForDrawingIfInDoubt != -1)
				{
					tileID = caravan.pather.previousTileForDrawingIfInDoubt;
				}
				else
				{
					tileID = caravan.Tile;
				}
				result = worldGrid.GetTileCenter(caravan.pather.nextTile) * num + worldGrid.GetTileCenter(tileID) * (1f - num);
			}
			else
			{
				result = worldGrid.GetTileCenter(caravan.Tile);
			}
			return result;
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x001039BC File Offset: 0x00101DBC
		public static Vector3 CaravanCollisionPosOffsetFor(Caravan caravan)
		{
			Vector3 result;
			if (!caravan.Spawned)
			{
				result = Vector3.zero;
			}
			else
			{
				bool flag = caravan.Spawned && caravan.pather.Moving;
				float d = 0.15f * Find.WorldGrid.averageTileSize;
				if (!flag || caravan.pather.nextTile == caravan.pather.Destination)
				{
					int num;
					if (flag)
					{
						num = caravan.pather.nextTile;
					}
					else
					{
						num = caravan.Tile;
					}
					int num2 = 0;
					int vertexIndex = 0;
					CaravanTweenerUtility.GetCaravansStandingAtOrAboutToStandAt(num, out num2, out vertexIndex, caravan);
					if (num2 == 0)
					{
						result = Vector3.zero;
					}
					else
					{
						result = WorldRendererUtility.ProjectOnQuadTangentialToPlanet(Find.WorldGrid.GetTileCenter(num), GenGeo.RegularPolygonVertexPosition(num2, vertexIndex) * d);
					}
				}
				else if (CaravanTweenerUtility.DrawPosCollides(caravan))
				{
					Rand.PushState();
					Rand.Seed = caravan.ID;
					float f = Rand.Range(0f, 360f);
					Rand.PopState();
					Vector2 point = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * d;
					result = WorldRendererUtility.ProjectOnQuadTangentialToPlanet(CaravanTweenerUtility.PatherTweenedPosRoot(caravan), point);
				}
				else
				{
					result = Vector3.zero;
				}
			}
			return result;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x00103B00 File Offset: 0x00101F00
		private static void GetCaravansStandingAtOrAboutToStandAt(int tile, out int caravansCount, out int caravansWithLowerIdCount, Caravan forCaravan)
		{
			caravansCount = 0;
			caravansWithLowerIdCount = 0;
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			int i = 0;
			while (i < caravans.Count)
			{
				Caravan caravan = caravans[i];
				if (caravan.Tile != tile)
				{
					if (caravan.pather.Moving && caravan.pather.nextTile == caravan.pather.Destination && caravan.pather.Destination == tile)
					{
						goto IL_8B;
					}
				}
				else if (!caravan.pather.Moving)
				{
					goto IL_8B;
				}
				IL_A9:
				i++;
				continue;
				IL_8B:
				caravansCount++;
				if (caravan.ID < forCaravan.ID)
				{
					caravansWithLowerIdCount++;
				}
				goto IL_A9;
			}
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x00103BC8 File Offset: 0x00101FC8
		private static bool DrawPosCollides(Caravan caravan)
		{
			Vector3 a = CaravanTweenerUtility.PatherTweenedPosRoot(caravan);
			float num = Find.WorldGrid.averageTileSize * 0.2f;
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (caravan2 != caravan)
				{
					if (Vector3.Distance(a, CaravanTweenerUtility.PatherTweenedPosRoot(caravan2)) < num)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040011D6 RID: 4566
		private const float BaseRadius = 0.15f;

		// Token: 0x040011D7 RID: 4567
		private const float BaseDistToCollide = 0.2f;
	}
}
