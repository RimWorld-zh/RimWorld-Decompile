using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D8 RID: 1240
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x000C3E70 File Offset: 0x000C2270
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
