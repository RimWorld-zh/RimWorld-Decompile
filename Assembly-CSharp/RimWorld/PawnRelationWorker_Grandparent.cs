using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CB RID: 1227
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x060015E9 RID: 5609 RVA: 0x000C292C File Offset: 0x000C0D2C
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
