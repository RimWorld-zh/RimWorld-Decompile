using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CB RID: 1227
	public class PawnRelationWorker_GreatGrandchild : PawnRelationWorker
	{
		// Token: 0x060015E8 RID: 5608 RVA: 0x000C2B40 File Offset: 0x000C0F40
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
