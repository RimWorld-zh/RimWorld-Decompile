using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D0 RID: 1232
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
		// Token: 0x060015F3 RID: 5619 RVA: 0x000C2F68 File Offset: 0x000C1368
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Child.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Sibling.Worker;
				PawnRelationWorker worker2 = PawnRelationDefOf.HalfSibling.Worker;
				result = ((other.GetMother() != null && (worker.InRelation(me, other.GetMother()) || worker2.InRelation(me, other.GetMother()))) || (other.GetFather() != null && (worker.InRelation(me, other.GetFather()) || worker2.InRelation(me, other.GetFather()))));
			}
			return result;
		}
	}
}
