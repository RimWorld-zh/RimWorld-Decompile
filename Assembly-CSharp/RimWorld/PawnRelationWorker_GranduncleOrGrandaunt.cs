using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C8 RID: 1224
	public class PawnRelationWorker_GranduncleOrGrandaunt : PawnRelationWorker
	{
		// Token: 0x060015E2 RID: 5602 RVA: 0x000C2958 File Offset: 0x000C0D58
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Grandparent.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.GreatGrandparent.Worker;
				result = ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())));
			}
			return result;
		}
	}
}
