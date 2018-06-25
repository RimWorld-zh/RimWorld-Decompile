using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000ACA RID: 2762
	public class JobGiver_MoveToStandable : ThinkNode_JobGiver
	{
		// Token: 0x06003D5A RID: 15706 RVA: 0x00205E4C File Offset: 0x0020424C
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

		// Token: 0x06003D5B RID: 15707 RVA: 0x00205F10 File Offset: 0x00204310
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
