using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C6 RID: 1222
	public class PawnRelationWorker_GrandnephewOrGrandniece : PawnRelationWorker
	{
		// Token: 0x060015DE RID: 5598 RVA: 0x000C28A0 File Offset: 0x000C0CA0
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.NephewOrNiece.Worker;
				result = ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())));
			}
			return result;
		}
	}
}
