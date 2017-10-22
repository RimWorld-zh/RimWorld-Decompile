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
				float num = (float)(caravan.pather.IsNextTilePassable() ? (1.0 - caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal) : 0.0);
				result = worldGrid.GetTileCenter(caravan.pather.nextTile) * num + worldGrid.GetTileCenter(caravan.Tile) * (float)(1.0 - num);
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
				float d = (float)(0.15000000596046448 * Find.WorldGrid.averageTileSize);
				if (!flag || caravan.pather.nextTile == caravan.pather.Destination)
				{
					int num = (!flag) ? caravan.Tile : caravan.pather.nextTile;
					int num2 = 0;
					int vertexIndex = 0;
					CaravanTweenerUtility.GetCaravansStandingAtOrAboutToStandAt(num, out num2, out vertexIndex, caravan);
					result = ((num2 != 0) ? WorldRendererUtility.ProjectOnQuadTangentialToPlanet(Find.WorldGrid.GetTileCenter(num), GenGeo.RegularPolygonVertexPosition(num2, vertexIndex) * d) : Vector3.zero);
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
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.Tile != tile)
				{
					if (caravan.pather.Moving && caravan.pather.nextTile == caravan.pather.Destination && caravan.pather.Destination == tile)
						goto IL_008b;
				}
				else if (!caravan.pather.Moving)
					goto IL_008b;
				continue;
				IL_008b:
				caravansCount++;
				if (caravan.ID < forCaravan.ID)
				{
					caravansWithLowerIdCount++;
				}
			}
		}

		private static bool DrawPosCollides(Caravan caravan)
		{
			Vector3 a = CaravanTweenerUtility.PatherTweenedPosRoot(caravan);
			float num = (float)(Find.WorldGrid.averageTileSize * 0.20000000298023224);
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			int num2 = 0;
			bool result;
			while (true)
			{
				if (num2 < caravans.Count)
				{
					Caravan caravan2 = caravans[num2];
					if (caravan2 != caravan && Vector3.Distance(a, CaravanTweenerUtility.PatherTweenedPosRoot(caravan2)) < num)
					{
						result = true;
						break;
					}
					num2++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
