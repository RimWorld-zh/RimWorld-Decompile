using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004BF RID: 1215
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x060015C9 RID: 5577 RVA: 0x000C2414 File Offset: 0x000C0814
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
