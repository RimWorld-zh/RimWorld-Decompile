using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CB RID: 1227
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x060015E8 RID: 5608 RVA: 0x000C2AA8 File Offset: 0x000C0EA8
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && ((me.GetMother() != null && me.GetMother() == other.GetMother()) || (me.GetFather() != null && me.GetFather() == other.GetFather()));
		}
	}
}
