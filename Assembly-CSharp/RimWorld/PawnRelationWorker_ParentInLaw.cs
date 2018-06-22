using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D0 RID: 1232
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x060015F7 RID: 5623 RVA: 0x000C2F5C File Offset: 0x000C135C
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
