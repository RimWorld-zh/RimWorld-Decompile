using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class RoadDefGenStep_Place : RoadDefGenStep_Bulldoze
	{
		public BuildableDef place;

		public int proximitySpacing;

		private List<BuildableDef> placeAsList;

		public override void Place(Map map, IntVec3 position, TerrainDef rockDef)
		{
			base.Place(map, position, rockDef);
			if (this.place is TerrainDef)
			{
				if (this.proximitySpacing != 0)
				{
					Log.ErrorOnce("Proximity spacing used for road terrain placement; not yet supported", 60936625);
				}
				TerrainDef terrainDef = this.place as TerrainDef;
				if (terrainDef == TerrainDefOf.FlagstoneSandstone)
				{
					terrainDef = rockDef;
				}
				map.terrainGrid.SetTerrain(position, terrainDef);
				map.roadInfo.roadEdgeTiles.Add(position);
			}
			else if (this.place is ThingDef)
			{
				if (this.proximitySpacing > 0 && GenClosest.ClosestThing_Global(position, map.listerThings.ThingsOfDef(this.place as ThingDef), (float)this.proximitySpacing, null) != null)
				{
					return;
				}
				while (position.GetThingList(map).Count > 0)
				{
					position.GetThingList(map)[0].Destroy(DestroyMode.Vanish);
				}
				RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, TerrainDefOf.Gravel);
				GenSpawn.Spawn(ThingMaker.MakeThing(this.place as ThingDef, null), position, map);
			}
			else
			{
				Log.ErrorOnce(string.Format("Can't figure out how to place object {0} while building road", this.place), 10785584);
			}
		}
	}
}
