using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000127 RID: 295
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00040894 File Offset: 0x0003EC94
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000408B0 File Offset: 0x0003ECB0
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return new Job(JobDefOf.RemoveFloor, c);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000408D8 File Offset: 0x0003ECD8
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00040938 File Offset: 0x0003ED38
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
