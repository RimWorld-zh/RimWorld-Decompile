using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C5 RID: 1221
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x000C24F0 File Offset: 0x000C08F0
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
				if ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())))
				{
					result = true;
				}
				else
				{
					PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
					result = ((other.GetMother() != null && worker2.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker2.InRelation(me, other.GetFather())));
				}
			}
			return result;
		}
	}
}
