using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D8 RID: 1240
	public class PawnRelationWorker_Stepchild : PawnRelationWorker
	{
		// Token: 0x06001611 RID: 5649 RVA: 0x000C3A28 File Offset: 0x000C1E28
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
