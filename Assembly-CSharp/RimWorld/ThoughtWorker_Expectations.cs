using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = ThoughtState.Inactive;
			}
			else if (p.Faction != Faction.OfPlayer)
			{
				result = ThoughtState.ActiveAtStage(3);
			}
			else if (p.IsCaravanMember())
			{
				result = ThoughtState.ActiveAtStage(2);
			}
			else if (p.MapHeld == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				float wealthTotal = p.MapHeld.wealthWatcher.WealthTotal;
				result = ((!(wealthTotal < 10000.0)) ? ((!(wealthTotal < 50000.0)) ? ((!(wealthTotal < 150000.0)) ? ((!(wealthTotal < 300000.0)) ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(0)) : ThoughtState.ActiveAtStage(1)) : ThoughtState.ActiveAtStage(2)) : ThoughtState.ActiveAtStage(3));
			}
			return result;
		}
	}
}
