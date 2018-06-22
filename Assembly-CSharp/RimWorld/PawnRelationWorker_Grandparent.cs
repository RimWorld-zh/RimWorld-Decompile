using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C7 RID: 1223
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x000C291C File Offset: 0x000C0D1C
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
