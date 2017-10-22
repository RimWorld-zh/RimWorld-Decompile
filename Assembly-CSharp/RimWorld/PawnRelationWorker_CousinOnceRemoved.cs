using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
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
				PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
				if (other.GetMother() != null && worker.InRelation(me, other.GetMother()))
				{
					goto IL_0054;
				}
				if (other.GetFather() != null && worker.InRelation(me, other.GetFather()))
					goto IL_0054;
				PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
				if (other.GetMother() != null && worker2.InRelation(me, other.GetMother()))
				{
					goto IL_00a1;
				}
				if (other.GetFather() != null && worker2.InRelation(me, other.GetFather()))
					goto IL_00a1;
				result = false;
			}
			goto IL_00b0;
			IL_00b0:
			return result;
			IL_0054:
			result = true;
			goto IL_00b0;
			IL_00a1:
			result = true;
			goto IL_00b0;
		}
	}
}
