using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.UncleOrAunt.Worker;
				if (other.GetMother() != null && worker.InRelation(me, other.GetMother()))
				{
					goto IL_0054;
				}
				if (other.GetFather() != null && worker.InRelation(me, other.GetFather()))
					goto IL_0054;
				result = false;
			}
			goto IL_0063;
			IL_0063:
			return result;
			IL_0054:
			result = true;
			goto IL_0063;
		}
	}
}
