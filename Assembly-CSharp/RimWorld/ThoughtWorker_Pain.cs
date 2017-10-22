using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			return (!(painTotal < 9.9999997473787516E-05)) ? ((!(painTotal < 0.15000000596046448)) ? ((!(painTotal < 0.40000000596046448)) ? ((!(painTotal < 0.800000011920929)) ? ThoughtState.ActiveAtStage(3) : ThoughtState.ActiveAtStage(2)) : ThoughtState.ActiveAtStage(1)) : ThoughtState.ActiveAtStage(0)) : ThoughtState.Inactive;
		}
	}
}
