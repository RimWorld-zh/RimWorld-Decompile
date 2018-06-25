using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000328 RID: 808
	public class IncidentWorker_DeepDrillInfestation : IncidentWorker
	{
		// Token: 0x040008C9 RID: 2249
		private static List<Thing> tmpDrills = new List<Thing>();

		// Token: 0x040008CA RID: 2250
		private const float PointsFactor = 0.32f;

		// Token: 0x040008CB RID: 2251
		private const float MinPoints = 115f;

		// Token: 0x040008CC RID: 2252
		private const float MaxPoints = 530f;

		// Token: 0x06000DCA RID: 3530 RVA: 0x00075FF4 File Offset: 0x000743F4
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

		// Token: 0x06000DCB RID: 3531 RVA: 0x00076048 File Offset: 0x00074448
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
