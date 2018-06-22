using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A1 RID: 161
	public class JobGiver_MaintainHives : JobGiver_AIFightEnemies
	{
		// Token: 0x06000409 RID: 1033 RVA: 0x00030620 File Offset: 0x0002EA20
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			int num = 0;
			while ((float)num < JobGiver_MaintainHives.CellsInScanRadius)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(pawn.Map))
				{
					if (intVec.GetRoom(pawn.Map, RegionType.Set_Passable) == room)
					{
						Hive hive = (Hive)pawn.Map.thingGrid.ThingAt(intVec, ThingDefOf.Hive);
						if (hive != null && pawn.CanReserve(hive, 1, -1, null, false))
						{
							CompMaintainable compMaintainable = hive.TryGetComp<CompMaintainable>();
							if (compMaintainable.CurStage != MaintainableStage.Healthy)
							{
								return new Job(JobDefOf.Maintain, hive);
							}
						}
					}
				}
				num++;
			}
			return null;
		}

		// Token: 0x0400026E RID: 622
		private static readonly float CellsInScanRadius = (float)GenRadial.NumCellsInRadius(7.9f);
	}
}
