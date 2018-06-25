using System;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
		public PawnRelationWorker_NephewOrNiece()
		{
		}

		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Child.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Sibling.Worker;
				PawnRelationWorker worker2 = PawnRelationDefOf.HalfSibling.Worker;
				result = ((other.GetMother() != null && (worker.InRelation(me, other.GetMother()) || worker2.InRelation(me, other.GetMother()))) || (other.GetFather() != null && (worker.InRelation(me, other.GetFather()) || worker2.InRelation(me, other.GetFather()))));
			}
			return result;
		}
	}
}
