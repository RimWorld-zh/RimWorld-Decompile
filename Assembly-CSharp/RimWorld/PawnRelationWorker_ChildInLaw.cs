using System;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		public PawnRelationWorker_ChildInLaw()
		{
		}

		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (other.GetSpouse() == null)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
				result = (!worker.InRelation(me, other) && worker.InRelation(me, other.GetSpouse()));
			}
			return result;
		}
	}
}
