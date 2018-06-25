using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A88 RID: 2696
	public static class MurderousRageMentalStateUtility
	{
		// Token: 0x04002585 RID: 9605
		private static List<Pawn> tmpTargets = new List<Pawn>();

		// Token: 0x06003BD8 RID: 15320 RVA: 0x001F8B80 File Offset: 0x001F6F80
		public static Pawn FindPawnToKill(Pawn pawn)
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
					if (pawn2.Faction == pawn.Faction || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction))
					{
						if (pawn2.RaceProps.Humanlike && pawn2 != pawn && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && (pawn2.CurJob == null || !pawn2.CurJob.exitMapOnArrival))
						{
							MurderousRageMentalStateUtility.tmpTargets.Add(pawn2);
						}
					}
				}
				if (!MurderousRageMentalStateUtility.tmpTargets.Any<Pawn>())
				{
					result = null;
				}
				else
				{
					Pawn pawn3 = MurderousRageMentalStateUtility.tmpTargets.RandomElement<Pawn>();
					MurderousRageMentalStateUtility.tmpTargets.Clear();
					result = pawn3;
				}
			}
			return result;
		}
	}
}
