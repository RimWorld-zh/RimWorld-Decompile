using System;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_GranduncleOrGrandaunt : PawnRelationWorker
	{
		public PawnRelationWorker_GranduncleOrGrandaunt()
		{
		}

		public override bool InRelation(Pawn me, Pawn other)
		{
			bool result;
			if (me == other)
			{
				result = false;
			}
			else if (PawnRelationDefOf.Grandparent.Worker.InRelation(me, other))
			{
				result = false;
			}
			else
			{
				PawnRelationWorker worker = PawnRelationDefOf.GreatGrandparent.Worker;
				result = ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())));
			}
			return result;
		}
	}
}
