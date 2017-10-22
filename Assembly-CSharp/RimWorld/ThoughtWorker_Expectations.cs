using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return ThoughtState.Inactive;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				return ThoughtState.ActiveAtStage(3);
			}
			if (p.IsCaravanMember())
			{
				return ThoughtState.ActiveAtStage(2);
			}
			if (p.MapHeld == null)
			{
				return ThoughtState.Inactive;
			}
			float wealthTotal = p.MapHeld.wealthWatcher.WealthTotal;
			if (wealthTotal < 10000.0)
			{
				return ThoughtState.ActiveAtStage(3);
			}
			if (wealthTotal < 50000.0)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			if (wealthTotal < 150000.0)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (wealthTotal < 300000.0)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}
	}
}
