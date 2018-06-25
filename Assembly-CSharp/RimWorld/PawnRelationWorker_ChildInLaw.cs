using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C1 RID: 1217
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x060015CC RID: 5580 RVA: 0x000C2764 File Offset: 0x000C0B64
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (other.GetSpouse() == null)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
				result = (!worker.InRelation(me, other) && worker.InRelation(me, other.GetSpouse()));
			}
			return result;
		}
	}
}
