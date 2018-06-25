using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7A RID: 3194
	public class PlaceWorker_NeedsFuelingPort : PlaceWorker
	{
		// Token: 0x060045FD RID: 17917 RVA: 0x0024E498 File Offset: 0x0024C898
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			AcceptanceReport result;
			if (FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(center, map) == null)
			{
				result = "MustPlaceNearFuelingPort".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x0024E4D8 File Offset: 0x0024C8D8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			List<Building> allBuildingsColonist = currentMap.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.building.hasFuelingPort && !Find.Selector.IsSelected(building) && FuelingPortUtility.GetFuelingPortCell(building).Standable(currentMap))
				{
					PlaceWorker_FuelingPort.DrawFuelingPortCell(building.Position, building.Rotation);
				}
			}
		}
	}
}
