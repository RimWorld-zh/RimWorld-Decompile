using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D7 RID: 1239
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		// Token: 0x0600160E RID: 5646 RVA: 0x000C3BD4 File Offset: 0x000C1FD4
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Spouse.Worker;
				result = ((me.GetMother() != null && worker.InRelation(me.GetMother(), other)) || (me.GetFather() != null && worker.InRelation(me.GetFather(), other)));
			}
			return result;
		}
	}
}
