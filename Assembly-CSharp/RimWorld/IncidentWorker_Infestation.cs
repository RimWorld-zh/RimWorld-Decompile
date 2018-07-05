using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_Infestation : IncidentWorker
	{
		private const float HivePoints = 250f;

		public IncidentWorker_Infestation()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && HivesUtility.TotalSpawnedHivesCount(map) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

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
