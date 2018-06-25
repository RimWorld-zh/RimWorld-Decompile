using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanTweenerUtility
	{
		private const float BaseRadius = 0.15f;

		private const float BaseDistToCollide = 0.2f;

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
	}
}
