using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
			if (other.GetMother() != null && worker.InRelation(me, other.GetMother()))
			{
				goto IL_0066;
			}
			if (other.GetFather() != null && worker.InRelation(me, other.GetFather()))
				goto IL_0066;
			return false;
			IL_0066:
			return true;
		}
	}
}
