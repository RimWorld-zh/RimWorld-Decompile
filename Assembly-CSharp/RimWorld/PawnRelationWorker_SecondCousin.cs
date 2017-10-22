using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_SecondCousin : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			Pawn mother = other.GetMother();
			if (mother != null)
			{
				if (mother.GetMother() != null && worker.InRelation(me, mother.GetMother()))
				{
					goto IL_005b;
				}
				if (mother.GetFather() != null && worker.InRelation(me, mother.GetFather()))
					goto IL_005b;
			}
			Pawn father = other.GetFather();
			if (father != null)
			{
				if (father.GetMother() != null && worker.InRelation(me, father.GetMother()))
				{
					goto IL_00a4;
				}
				if (father.GetFather() != null && worker.InRelation(me, father.GetFather()))
					goto IL_00a4;
			}
			return false;
			IL_005b:
			return true;
			IL_00a4:
			return true;
		}
	}
}
