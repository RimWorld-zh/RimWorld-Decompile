using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Sibling.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				if (me.GetMother() != null && me.GetMother() == other.GetMother())
				{
					goto IL_0064;
				}
				if (me.GetFather() != null && me.GetFather() == other.GetFather())
					goto IL_0064;
				result = false;
			}
			goto IL_0073;
			IL_0064:
			result = true;
			goto IL_0073;
			IL_0073:
			return result;
		}
	}
}
