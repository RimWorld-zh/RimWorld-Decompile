using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_MarryAdjacentPawn : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			Thing thing;
			if (!pawn.RaceProps.IsFlesh)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 c = pawn.Position + GenAdj.CardinalDirections[i];
					if (c.InBounds(pawn.Map))
					{
						thing = c.GetThingList(pawn.Map).Find((Predicate<Thing>)((Thing x) => x is Pawn && this.CanMarry(pawn, (Pawn)x)));
						if (thing != null)
							goto IL_00a0;
					}
				}
				result = null;
			}
			goto IL_00ca;
			IL_00ca:
			return result;
			IL_00a0:
			result = new Job(JobDefOf.MarryAdjacentPawn, thing);
			goto IL_00ca;
		}

		private bool CanMarry(Pawn pawn, Pawn toMarry)
		{
			return !toMarry.Drafted && pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, toMarry);
		}
	}
}
