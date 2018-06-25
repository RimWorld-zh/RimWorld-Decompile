using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D6 RID: 1238
	public class PawnRelationWorker_Stepchild : PawnRelationWorker
	{
		// Token: 0x0600160C RID: 5644 RVA: 0x000C3B68 File Offset: 0x000C1F68
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
