using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CA RID: 1226
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x060015E6 RID: 5606 RVA: 0x000C2A6C File Offset: 0x000C0E6C
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
