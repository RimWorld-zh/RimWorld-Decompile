using Verse;

namespace RimWorld
{
	public class RoadDefGenStep_Bulldoze : RoadDefGenStep
	{
		public override void Place(Map map, IntVec3 tile, TerrainDef rockDef)
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
