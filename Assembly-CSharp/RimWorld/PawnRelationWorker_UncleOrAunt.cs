using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
				if (other.GetMother() != null && worker.InRelation(me, other.GetMother()))
				{
					goto IL_0071;
				}
				if (other.GetFather() != null && worker.InRelation(me, other.GetFather()))
					goto IL_0071;
				result = false;
			}
			goto IL_0080;
			IL_0071:
			result = true;
			goto IL_0080;
			IL_0080:
			return result;
		}
	}
}
