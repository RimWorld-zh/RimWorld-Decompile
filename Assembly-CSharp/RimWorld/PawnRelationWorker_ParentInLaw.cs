using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D4 RID: 1236
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x06001600 RID: 5632 RVA: 0x000C2F6C File Offset: 0x000C136C
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
				PawnRelationWorker worker = PawnRelationDefOf.Parent.Worker;
				result = (!worker.InRelation(me, other) && worker.InRelation(me.GetSpouse(), other));
			}
			return result;
		}
	}
}
