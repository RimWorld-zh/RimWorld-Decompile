using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DA RID: 1242
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		// Token: 0x06001615 RID: 5653 RVA: 0x000C3B14 File Offset: 0x000C1F14
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
				result = ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())));
			}
			return result;
		}
	}
}
