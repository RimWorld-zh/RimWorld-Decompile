using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F1 RID: 1009
	public class RoadDefGenStep_Place : RoadDefGenStep_Bulldoze
	{
		// Token: 0x06001165 RID: 4453 RVA: 0x00096CA8 File Offset: 0x000950A8
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			if (this.onlyIfOriginAllows)
			{
				if (!GenConstruct.CanBuildOnTerrain(this.place, origin, map, Rot4.North, null) && origin.GetTerrain(map) != this.place)
				{
					return;
				}
				bool flag = false;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 c = position + GenAdj.CardinalDirections[i];
					if (c.InBounds(map) && this.chancePerPositionCurve.Evaluate(distance[c.x, c.z].fromRoad) > 0f && (GenConstruct.CanBuildOnTerrain(this.place, c, map, Rot4.North, null) || c.GetTerrain(map) == this.place) && (GenConstruct.CanBuildOnTerrain(this.place, distance[c.x, c.z].origin, map, Rot4.North, null) || distance[c.x, c.z].origin.GetTerrain(map) == this.place))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			if (!this.suppressOnTerrainTag.NullOrEmpty())
			{
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
				if (terrainDef.HasTag(this.suppressOnTerrainTag))
				{
					return;
				}
			}
			base.Place(map, position, rockDef, origin, distance);
			if (this.place is TerrainDef)
			{
				if (this.proximitySpacing != 0)
				{
					Log.ErrorOnce("Proximity spacing used for road terrain placement; not yet supported", 60936625, false);
				}
				TerrainDef terrainDef2 = map.terrainGrid.TerrainAt(position);
				TerrainDef terrainDef3 = (TerrainDef)this.place;
				if (terrainDef3 == TerrainDefOf.FlagstoneSandstone)
				{
					terrainDef3 = rockDef;
				}
				if (terrainDef3 == TerrainDefOf.Bridge)
				{
					if (terrainDef2 == TerrainDefOf.WaterDeep)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.WaterShallow);
					}
					if (terrainDef2 == TerrainDefOf.WaterOceanDeep)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.WaterOceanShallow);
					}
				}
				if (GenConstruct.CanBuildOnTerrain(terrainDef3, position, map, Rot4.North, null) && (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, position, map, Rot4.North, null) || terrainDef3 == TerrainDefOf.Bridge) && terrainDef2 != TerrainDefOf.Bridge)
				{
					if (terrainDef2.HasTag("Road") && !terrainDef2.Removable)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.Gravel);
					}
					map.terrainGrid.SetTerrain(position, terrainDef3);
				}
				if (position.OnEdge(map) && !map.roadInfo.roadEdgeTiles.Contains(position))
				{
					map.roadInfo.roadEdgeTiles.Add(position);
				}
			}
			else if (this.place is ThingDef)
			{
				if (GenConstruct.CanBuildOnTerrain(this.place, position, map, Rot4.North, null))
				{
					if (this.proximitySpacing <= 0 || GenClosest.ClosestThing_Global(position, map.listerThings.ThingsOfDef((ThingDef)this.place), (float)this.proximitySpacing, null, null) == null)
					{
						while (position.GetThingList(map).Count > 0)
						{
							position.GetThingList(map)[0].Destroy(DestroyMode.Vanish);
						}
						RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, TerrainDefOf.Gravel);
						GenSpawn.Spawn(ThingMaker.MakeThing((ThingDef)this.place, null), position, map, WipeMode.Vanish);
					}
				}
			}
			else
			{
				Log.ErrorOnce(string.Format("Can't figure out how to place object {0} while building road", this.place), 10785584, false);
			}
		}

		// Token: 0x04000A97 RID: 2711
		public BuildableDef place;

		// Token: 0x04000A98 RID: 2712
		public int proximitySpacing;

		// Token: 0x04000A99 RID: 2713
		public bool onlyIfOriginAllows;

		// Token: 0x04000A9A RID: 2714
		public string suppressOnTerrainTag;
	}
}
