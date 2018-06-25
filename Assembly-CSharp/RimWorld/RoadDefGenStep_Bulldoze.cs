using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F2 RID: 1010
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		// Token: 0x06001166 RID: 4454 RVA: 0x00096D74 File Offset: 0x00095174
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
