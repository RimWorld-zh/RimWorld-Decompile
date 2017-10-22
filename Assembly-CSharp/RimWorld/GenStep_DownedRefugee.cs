using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map);
		}

		protected override void ScatterAt(IntVec3 loc, Map map, int count = 1)
		{
			DownedRefugeeComp component = ((WorldObject)map.info.parent).GetComponent<DownedRefugeeComp>();
			Pawn newThing = (component == null || !component.pawn.Any) ? DownedRefugeeQuestUtility.GenerateRefugee(map.Tile) : component.pawn.Take(component.pawn[0]);
			GenSpawn.Spawn(newThing, loc, map);
			MapGenerator.rootsToUnfog.Add(loc);
		}
	}
}
