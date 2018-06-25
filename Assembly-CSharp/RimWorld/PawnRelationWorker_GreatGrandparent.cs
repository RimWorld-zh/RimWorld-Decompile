using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CC RID: 1228
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x060015E9 RID: 5609 RVA: 0x000C2DBC File Offset: 0x000C11BC
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
