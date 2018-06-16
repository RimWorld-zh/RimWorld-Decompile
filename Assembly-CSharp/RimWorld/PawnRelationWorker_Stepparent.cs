using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D9 RID: 1241
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		// Token: 0x06001613 RID: 5651 RVA: 0x000C3A78 File Offset: 0x000C1E78
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
