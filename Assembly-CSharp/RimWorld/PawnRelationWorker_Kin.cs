using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CC RID: 1228
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		// Token: 0x060015EA RID: 5610 RVA: 0x000C2B34 File Offset: 0x000C0F34
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && me.relations.FamilyByBlood.Contains(other);
		}
	}
}
