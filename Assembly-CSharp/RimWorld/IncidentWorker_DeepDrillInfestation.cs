using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000326 RID: 806
	public class IncidentWorker_DeepDrillInfestation : IncidentWorker
	{
		// Token: 0x040008C6 RID: 2246
		private static List<Thing> tmpDrills = new List<Thing>();

		// Token: 0x040008C7 RID: 2247
		private const float PointsFactor = 0.32f;

		// Token: 0x040008C8 RID: 2248
		private const float MinPoints = 115f;

		// Token: 0x040008C9 RID: 2249
		private const float MaxPoints = 530f;

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00075E9C File Offset: 0x0007429C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				IncidentWorker_DeepDrillInfestation.tmpDrills.Clear();
				DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, IncidentWorker_DeepDrillInfestation.tmpDrills);
				result = IncidentWorker_DeepDrillInfestation.tmpDrills.Any<Thing>();
			}
			return result;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00075EF0 File Offset: 0x000742F0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IncidentWorker_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, IncidentWorker_DeepDrillInfestation.tmpDrills);
			Thing deepDrill;
			bool result;
			if (!IncidentWorker_DeepDrillInfestation.tmpDrills.TryRandomElement(out deepDrill))
			{
				result = false;
			}
			else
			{
				IntVec3 intVec = CellFinder.FindNoWipeSpawnLocNear(deepDrill.Position, map, ThingDefOf.TunnelHiveSpawner, Rot4.North, 2, (IntVec3 x) => x.Walkable(map) && x.GetFirstThing(map, deepDrill.def) == null && x.GetFirstThing(map) == null && x.GetFirstThing(map, ThingDefOf.Hive) == null && x.GetFirstThing(map, ThingDefOf.TunnelHiveSpawner) == null);
				if (intVec == deepDrill.Position)
				{
					result = false;
				}
				else
				{
					TunnelHiveSpawner tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
					tunnelHiveSpawner.spawnHive = false;
					tunnelHiveSpawner.insectsPoints = Mathf.Clamp(parms.points * 0.32f, 115f, 530f);
					GenSpawn.Spawn(tunnelHiveSpawner, intVec, map, WipeMode.FullRefund);
					deepDrill.TryGetComp<CompCreatesInfestations>().Notify_CreatedInfestation();
					base.SendStandardLetter(new TargetInfo(tunnelHiveSpawner.Position, map, false), null, new string[0]);
					result = true;
				}
			}
			return result;
		}
	}
}
