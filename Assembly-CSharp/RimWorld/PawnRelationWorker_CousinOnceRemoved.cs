using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
			if (other.GetMother() != null && worker.InRelation(me, other.GetMother()))
			{
				goto IL_004e;
			}
			if (other.GetFather() != null && worker.InRelation(me, other.GetFather()))
				goto IL_004e;
			PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			if (other.GetMother() != null && worker2.InRelation(me, other.GetMother()))
			{
				goto IL_0095;
			}
			if (other.GetFather() != null && worker2.InRelation(me, other.GetFather()))
				goto IL_0095;
			return false;
			IL_0095:
			return true;
			IL_004e:
			return true;
		}
	}
}
