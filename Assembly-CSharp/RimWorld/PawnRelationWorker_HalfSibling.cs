using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Sibling.Worker.InRelation(me, other))
			{
				return false;
			}
			if (me.GetMother() != null && me.GetMother() == other.GetMother())
			{
				goto IL_0059;
			}
			if (me.GetFather() != null && me.GetFather() == other.GetFather())
				goto IL_0059;
			return false;
			IL_0059:
			return true;
		}
	}
}
