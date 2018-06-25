using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	public class JobGiver_MoveToStandable : ThinkNode_JobGiver
	{
		public JobGiver_MoveToStandable()
		{
		}

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
