using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C0 RID: 1216
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		// Token: 0x060015CB RID: 5579 RVA: 0x000C2480 File Offset: 0x000C0880
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
