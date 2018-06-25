using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C2 RID: 1218
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		// Token: 0x060015CF RID: 5583 RVA: 0x000C25D0 File Offset: 0x000C09D0
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.UncleOrAunt.Worker;
				result = ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())));
			}
			return result;
		}
	}
}
