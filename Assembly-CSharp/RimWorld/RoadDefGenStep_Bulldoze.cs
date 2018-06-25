using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F2 RID: 1010
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		// Token: 0x06001167 RID: 4455 RVA: 0x00096D64 File Offset: 0x00095164
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
