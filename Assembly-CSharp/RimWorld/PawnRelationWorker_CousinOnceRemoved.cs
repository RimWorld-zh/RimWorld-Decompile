using System;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		public PawnRelationWorker_CousinOnceRemoved()
		{
		}

		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
				if ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())))
				{
					result = true;
				}
				else
				{
					PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
					result = ((other.GetMother() != null && worker2.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker2.InRelation(me, other.GetFather())));
				}
			}
			return result;
		}
	}
}
