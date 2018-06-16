using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004D5 RID: 1237
	public class PawnRelationWorker_SecondCousin : PawnRelationWorker
	{
		// Token: 0x06001602 RID: 5634 RVA: 0x000C2FBC File Offset: 0x000C13BC
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
				Pawn mother = other.GetMother();
				if (mother != null)
				{
					if ((mother.GetMother() != null && worker.InRelation(me, mother.GetMother())) || (mother.GetFather() != null && worker.InRelation(me, mother.GetFather())))
					{
						return true;
					}
				}
				Pawn father = other.GetFather();
				if (father != null)
				{
					if ((father.GetMother() != null && worker.InRelation(me, father.GetMother())) || (father.GetFather() != null && worker.InRelation(me, father.GetFather())))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
