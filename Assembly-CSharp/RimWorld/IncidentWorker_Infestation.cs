using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_Infestation : IncidentWorker
	{
		private const float HivePoints = 400f;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			IntVec3 intVec = default(IntVec3);
			return base.CanFireNowSub(target) && HivesUtility.TotalSpawnedHivesCount(map) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Hive t = null;
			int num2;
			for (int num = Mathf.Max(GenMath.RoundRandom((float)(parms.points / 400.0)), 1); num > 0; num -= num2)
			{
				num2 = Mathf.Min(3, num);
				t = this.SpawnHiveCluster(num2, map);
			}
			base.SendStandardLetter((Thing)t);
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		private Hive SpawnHiveCluster(int hiveCount, Map map)
		{
			IntVec3 loc = default(IntVec3);
			Hive result;
			if (!InfestationCellFinder.TryFindCell(out loc, map))
			{
				result = null;
			}
			else
			{
				Hive hive = (Hive)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Hive, null), loc, map);
				hive.SetFaction(Faction.OfInsects, null);
				IncidentWorker_Infestation.SpawnInsectJellyInstantly(hive);
				for (int i = 0; i < hiveCount - 1; i++)
				{
					Hive hive2 = default(Hive);
					if (hive.GetComp<CompSpawnerHives>().TrySpawnChildHive(false, out hive2))
					{
						IncidentWorker_Infestation.SpawnInsectJellyInstantly(hive2);
						hive = hive2;
					}
				}
				result = hive;
			}
			return result;
		}

		private static void SpawnInsectJellyInstantly(Hive hive)
		{
			using (IEnumerator<CompSpawner> enumerator = hive.GetComps<CompSpawner>().GetEnumerator())
			{
				CompSpawner current;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						current = enumerator.Current;
						if (current.PropsSpawner.thingToSpawn == ThingDefOf.InsectJelly)
							break;
						continue;
					}
					return;
				}
				current.TryDoSpawn();
			}
		}
	}
}
