using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C1 RID: 1217
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		// Token: 0x060015CD RID: 5581 RVA: 0x000C24FC File Offset: 0x000C08FC
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
