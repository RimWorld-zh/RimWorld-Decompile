using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_MaintainHives : JobGiver_AIFightEnemies
	{
		private static readonly float CellsInScanRadius = (float)GenRadial.NumCellsInRadius(7.9f);

		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			int num = 0;
			Job result;
			while (true)
			{
				if ((float)num < JobGiver_MaintainHives.CellsInScanRadius)
				{
					IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num];
					if (intVec.InBounds(pawn.Map) && intVec.GetRoom(pawn.Map, RegionType.Set_Passable) == room)
					{
						Hive hive = (Hive)pawn.Map.thingGrid.ThingAt(intVec, ThingDefOf.Hive);
						if (hive != null && pawn.CanReserve((Thing)hive, 1, -1, null, false))
						{
							CompMaintainable compMaintainable = hive.TryGetComp<CompMaintainable>();
							if (compMaintainable.CurStage != 0)
							{
								result = new Job(JobDefOf.Maintain, (Thing)hive);
								break;
							}
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
