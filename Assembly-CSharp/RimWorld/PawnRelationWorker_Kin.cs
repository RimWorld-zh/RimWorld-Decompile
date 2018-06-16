using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D0 RID: 1232
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		// Token: 0x060015F3 RID: 5619 RVA: 0x000C2B28 File Offset: 0x000C0F28
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && me.relations.FamilyByBlood.Contains(other);
		}
	}
}
