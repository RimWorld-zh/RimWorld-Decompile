using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
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
				if (other.GetMother() != null && (worker.InRelation(me, other.GetMother()) || worker2.InRelation(me, other.GetMother())))
				{
					goto IL_00a0;
				}
				if (other.GetFather() != null && (worker.InRelation(me, other.GetFather()) || worker2.InRelation(me, other.GetFather())))
				{
					goto IL_00a0;
				}
				result = false;
			}
			goto IL_00af;
			IL_00a0:
			result = true;
			goto IL_00af;
			IL_00af:
			return result;
		}
	}
}
