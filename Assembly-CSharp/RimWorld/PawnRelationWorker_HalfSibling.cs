using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CD RID: 1229
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x000C2BF8 File Offset: 0x000C0FF8
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && ((me.GetMother() != null && me.GetMother() == other.GetMother()) || (me.GetFather() != null && me.GetFather() == other.GetFather()));
		}
	}
}
