using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
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
				PawnRelationWorker worker = PawnRelationDefOf.Spouse.Worker;
				result = ((byte)((me.GetMother() != null && worker.InRelation(me.GetMother(), other)) ? 1 : ((me.GetFather() != null && worker.InRelation(me.GetFather(), other)) ? 1 : 0)) != 0);
			}
			return result;
		}
	}
}
