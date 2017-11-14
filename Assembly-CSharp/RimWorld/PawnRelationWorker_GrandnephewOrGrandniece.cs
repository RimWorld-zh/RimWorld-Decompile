using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_GrandnephewOrGrandniece : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.NephewOrNiece.Worker;
			if (other.GetMother() != null && worker.InRelation(me, other.GetMother()))
			{
				goto IL_004e;
			}
			if (other.GetFather() != null && worker.InRelation(me, other.GetFather()))
				goto IL_004e;
			return false;
			IL_004e:
			return true;
		}
	}
}
