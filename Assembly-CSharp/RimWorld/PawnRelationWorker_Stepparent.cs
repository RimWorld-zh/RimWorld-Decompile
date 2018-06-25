using System;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		public PawnRelationWorker_Stepparent()
		{
		}

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
				result = ((me.GetMother() != null && worker.InRelation(me.GetMother(), other)) || (me.GetFather() != null && worker.InRelation(me.GetFather(), other)));
			}
			return result;
		}
	}
}
