using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020004CE RID: 1230
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		// Token: 0x060015EE RID: 5614 RVA: 0x000C2C84 File Offset: 0x000C1084
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && me.relations.FamilyByBlood.Contains(other);
		}
	}
}
