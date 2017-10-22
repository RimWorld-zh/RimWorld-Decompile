using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			if (painTotal < 9.9999997473787516E-05)
			{
				return ThoughtState.Inactive;
			}
			if (painTotal < 0.15000000596046448)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (painTotal < 0.40000000596046448)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (painTotal < 0.800000011920929)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
