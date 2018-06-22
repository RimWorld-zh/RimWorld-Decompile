using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000330 RID: 816
	public class IncidentWorker_Infestation : IncidentWorker
	{
		// Token: 0x06000DF4 RID: 3572 RVA: 0x00077128 File Offset: 0x00075528
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && HivesUtility.TotalSpawnedHivesCount(map) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00077170 File Offset: 0x00075570
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

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000771F4 File Offset: 0x000755F4
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

		// Token: 0x040008D5 RID: 2261
		private const float HivePoints = 250f;
	}
}
