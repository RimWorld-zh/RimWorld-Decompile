using System.Collections.Generic;

namespace Verse.AI
{
	public static class MurderousRageMentalStateUtility
	{
		private static List<Pawn> tmpTargets = new List<Pawn>();

		public static Pawn FindPawn(Pawn pawn)
		{
			Pawn result;
			if (!pawn.Spawned)
			{
				result = null;
			}
			else
			{
				MurderousRageMentalStateUtility.tmpTargets.Clear();
				List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if ((pawn2.Faction == pawn.Faction || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction)) && pawn2.RaceProps.Humanlike && pawn2 != pawn && pawn.CanReach((Thing)pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && (pawn2.CurJob == null || !pawn2.CurJob.exitMapOnArrival))
					{
						MurderousRageMentalStateUtility.tmpTargets.Add(pawn2);
					}
				}
				if (!MurderousRageMentalStateUtility.tmpTargets.Any())
				{
					result = null;
				}
				else
				{
					Pawn pawn3 = MurderousRageMentalStateUtility.tmpTargets.RandomElement();
					MurderousRageMentalStateUtility.tmpTargets.Clear();
					result = pawn3;
				}
			}
			return result;
		}
	}
}
