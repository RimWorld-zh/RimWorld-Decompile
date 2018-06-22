using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC7 RID: 2759
	public class JobGiver_MoveToStandable : ThinkNode_JobGiver
	{
		// Token: 0x06003D56 RID: 15702 RVA: 0x00205A40 File Offset: 0x00203E40
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.Drafted)
			{
				result = null;
			}
			else if (!pawn.Position.Standable(pawn.Map))
			{
				result = this.FindBetterPosition(pawn);
			}
			else
			{
				List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn2 = thingList[i] as Pawn;
					if (pawn2 != null && pawn2.Faction == pawn.Faction && pawn2.Drafted && !pawn2.pather.MovingNow)
					{
						return this.FindBetterPosition(pawn);
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00205B04 File Offset: 0x00203F04
		private Job FindBetterPosition(Pawn pawn)
		{
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(pawn.Position, pawn);
			Job result;
			if (intVec.IsValid && intVec != pawn.Position)
			{
				result = new Job(JobDefOf.Goto, intVec);
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
