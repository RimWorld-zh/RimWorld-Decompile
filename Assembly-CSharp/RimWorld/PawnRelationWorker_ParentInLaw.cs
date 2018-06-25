using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D2 RID: 1234
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x060015FA RID: 5626 RVA: 0x000C32AC File Offset: 0x000C16AC
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (me.GetSpouse() == null)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Parent.Worker;
				result = (!worker.InRelation(me, other) && worker.InRelation(me.GetSpouse(), other));
			}
			return result;
		}
	}
}
