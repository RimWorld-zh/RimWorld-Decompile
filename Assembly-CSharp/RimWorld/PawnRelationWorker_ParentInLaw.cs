using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
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
