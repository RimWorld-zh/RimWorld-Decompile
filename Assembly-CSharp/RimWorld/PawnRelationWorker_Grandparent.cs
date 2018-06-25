using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004C9 RID: 1225
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x060015E4 RID: 5604 RVA: 0x000C2A6C File Offset: 0x000C0E6C
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
