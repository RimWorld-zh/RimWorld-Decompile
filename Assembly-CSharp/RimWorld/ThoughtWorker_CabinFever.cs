using Verse;

namespace RimWorld
{
	public class ThoughtWorker_CabinFever : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.Downed)
			{
				result = ThoughtState.Inactive;
			}
			else if (p.HostFaction != null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				float num = (float)((float)p.needs.mood.recentMemory.TicksSinceOutdoors / 60000.0);
				result = ((!(num < 2.5)) ? ((!(num < 7.5)) ? ThoughtState.ActiveAtStage(1) : ThoughtState.ActiveAtStage(0)) : ThoughtState.Inactive);
			}
			return result;
		}
	}
}
