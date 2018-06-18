using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CF RID: 1231
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x060015F1 RID: 5617 RVA: 0x000C2AB8 File Offset: 0x000C0EB8
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && ((me.GetMother() != null && me.GetMother() == other.GetMother()) || (me.GetFather() != null && me.GetFather() == other.GetFather()));
		}
	}
}
