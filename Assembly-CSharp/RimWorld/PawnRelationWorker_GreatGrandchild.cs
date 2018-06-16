using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CD RID: 1229
	public class PawnRelationWorker_GreatGrandchild : PawnRelationWorker
	{
		// Token: 0x060015ED RID: 5613 RVA: 0x000C29E4 File Offset: 0x000C0DE4
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Grandchild.Worker;
				result = ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())));
			}
			return result;
		}
	}
}
