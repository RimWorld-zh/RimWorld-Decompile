using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6D RID: 3181
	public class PlaceWorker_ShowDeepResources : PlaceWorker
	{
		// Token: 0x060045DE RID: 17886 RVA: 0x0024DCB0 File Offset: 0x0024C0B0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			List<Building> allBuildingsColonist = currentMap.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building thing = allBuildingsColonist[i];
				CompDeepScanner compDeepScanner = thing.TryGetComp<CompDeepScanner>();
				if (compDeepScanner != null)
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						currentMap.deepResourceGrid.MarkForDraw();
					}
				}
			}
		}
	}
}
