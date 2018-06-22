using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D4 RID: 1236
	public class PawnRelationWorker_Stepchild : PawnRelationWorker
	{
		// Token: 0x06001608 RID: 5640 RVA: 0x000C3A18 File Offset: 0x000C1E18
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (me.GetSpouse() == null)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
				result = (!worker.InRelation(me, other) && worker.InRelation(me.GetSpouse(), other));
			}
			return result;
		}
	}
}
