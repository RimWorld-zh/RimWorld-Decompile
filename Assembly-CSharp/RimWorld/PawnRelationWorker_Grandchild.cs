using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C7 RID: 1223
	public class PawnRelationWorker_Grandchild : PawnRelationWorker
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x000C2974 File Offset: 0x000C0D74
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
				result = ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())));
			}
			return result;
		}
	}
}
