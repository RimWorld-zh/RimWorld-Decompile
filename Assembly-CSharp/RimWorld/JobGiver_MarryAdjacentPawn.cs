using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000CB RID: 203
	public class JobGiver_MarryAdjacentPawn : ThinkNode_JobGiver
	{
		// Token: 0x060004A3 RID: 1187 RVA: 0x00034C2C File Offset: 0x0003302C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
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
						Thing thing = c.GetThingList(pawn.Map).Find((Thing x) => x is Pawn && this.CanMarry(pawn, (Pawn)x));
						if (thing != null)
						{
							return new Job(JobDefOf.MarryAdjacentPawn, thing);
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00034D04 File Offset: 0x00033104
		private bool CanMarry(Pawn pawn, Pawn toMarry)
		{
			return !toMarry.Drafted && pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, toMarry);
		}
	}
}
