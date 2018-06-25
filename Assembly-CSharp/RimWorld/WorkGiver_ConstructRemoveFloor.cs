using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		public WorkGiver_ConstructRemoveFloor()
		{
		}

		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return new Job(JobDefOf.RemoveFloor, c);
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		public static bool AnyBuildingBlockingFloorRemoval(IntVec3 c, Map map)
		{
			bool result;
			if (!map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				result = false;
			}
			else
			{
				Building firstBuilding = c.GetFirstBuilding(map);
				result = (firstBuilding != null && firstBuilding.def.terrainAffordanceNeeded != null && !map.terrainGrid.UnderTerrainAt(c).affordances.Contains(firstBuilding.def.terrainAffordanceNeeded));
			}
			return result;
		}
	}
}
