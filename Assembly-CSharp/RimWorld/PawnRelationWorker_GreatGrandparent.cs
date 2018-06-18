using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CE RID: 1230
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x060015EF RID: 5615 RVA: 0x000C2A7C File Offset: 0x000C0E7C
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
