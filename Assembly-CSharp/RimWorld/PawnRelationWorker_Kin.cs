using System.Linq;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			return me.relations.FamilyByBlood.Contains(other);
		}
	}
}
