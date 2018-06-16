using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C3 RID: 1219
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x060015D2 RID: 5586 RVA: 0x000C2408 File Offset: 0x000C0808
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
