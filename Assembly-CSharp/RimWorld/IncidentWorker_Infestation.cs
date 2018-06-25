using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000332 RID: 818
	public class IncidentWorker_Infestation : IncidentWorker
	{
		// Token: 0x040008D8 RID: 2264
		private const float HivePoints = 250f;

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00077280 File Offset: 0x00075680
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && HivesUtility.TotalSpawnedHivesCount(map) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x000772C8 File Offset: 0x000756C8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = null;
			int num;
			for (int i = Mathf.Max(GenMath.RoundRandom(parms.points / 250f), 1); i > 0; i -= num)
			{
				num = Mathf.Min(3, i);
				t = this.SpawnTunnel(num, map);
			}
			base.SendStandardLetter(t, null, new string[0]);
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0007734C File Offset: 0x0007574C
		private Thing SpawnTunnel(int hiveCount, Map map)
		{
			IntVec3 loc;
			Thing result;
			if (!InfestationCellFinder.TryFindCell(out loc, map))
			{
				result = null;
			}
			else
			{
				Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null), loc, map, WipeMode.FullRefund);
				for (int i = 0; i < hiveCount - 1; i++)
				{
					loc = CompSpawnerHives.FindChildHiveLocation(thing.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), false);
					if (loc.IsValid)
					{
						thing = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null), loc, map, WipeMode.FullRefund);
					}
				}
				result = thing;
			}
			return result;
		}
	}
}
