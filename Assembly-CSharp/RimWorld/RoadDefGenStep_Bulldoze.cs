using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F0 RID: 1008
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		// Token: 0x06001163 RID: 4451 RVA: 0x00096A28 File Offset: 0x00094E28
		public override void Place(Map map, IntVec3 tile, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			while (tile.Impassable(map))
			{
				foreach (Thing thing in tile.GetThingList(map))
				{
					if (thing.def.passability == Traversability.Impassable)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
				}
			}
		}
	}
}
