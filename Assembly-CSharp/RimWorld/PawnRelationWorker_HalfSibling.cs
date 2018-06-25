using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CD RID: 1229
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x060015EB RID: 5611 RVA: 0x000C2DF8 File Offset: 0x000C11F8
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && ((me.GetMother() != null && me.GetMother() == other.GetMother()) || (me.GetFather() != null && me.GetFather() == other.GetFather()));
		}
	}
}
