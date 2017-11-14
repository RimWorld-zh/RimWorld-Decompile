using Verse;

namespace RimWorld
{
	public class ThoughtWorker_CabinFever : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.Downed)
			{
				return ThoughtState.Inactive;
			}
			if (p.HostFaction != null)
			{
				return ThoughtState.Inactive;
			}
			float num = (float)((float)p.needs.mood.recentMemory.TicksSinceOutdoors / 60000.0);
			if (num < 2.5)
			{
				return ThoughtState.Inactive;
			}
			if (num < 7.5)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}
	}
}
