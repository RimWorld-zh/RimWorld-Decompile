using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_SecondCousin : PawnRelationWorker
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
				PawnRelationWorker worker = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
				Pawn mother = other.GetMother();
				if (mother != null)
				{
					if (mother.GetMother() != null && worker.InRelation(me, mother.GetMother()))
					{
						goto IL_0062;
					}
					if (mother.GetFather() != null && worker.InRelation(me, mother.GetFather()))
						goto IL_0062;
				}
				Pawn father = other.GetFather();
				if (father != null)
				{
					if (father.GetMother() != null && worker.InRelation(me, father.GetMother()))
					{
						goto IL_00b3;
					}
					if (father.GetFather() != null && worker.InRelation(me, father.GetFather()))
						goto IL_00b3;
				}
				result = false;
			}
			goto IL_00c3;
			IL_00b3:
			result = true;
			goto IL_00c3;
			IL_00c3:
			return result;
			IL_0062:
			result = true;
			goto IL_00c3;
		}
	}
}
