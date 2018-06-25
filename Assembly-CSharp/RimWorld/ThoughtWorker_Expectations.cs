using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		public ThoughtWorker_Expectations()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = ThoughtState.Inactive;
			}
			else if (p.Faction != Faction.OfPlayer)
			{
				result = ThoughtState.ActiveAtStage(5);
			}
			else if (p.IsCaravanMember())
			{
				result = ThoughtState.ActiveAtStage(4);
			}
			else if (p.MapHeld == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				float wealthTotal = p.MapHeld.wealthWatcher.WealthTotal;
				if (wealthTotal < 10000f)
				{
					result = ThoughtState.ActiveAtStage(5);
				}
				else if (wealthTotal < 25000f)
				{
					result = ThoughtState.ActiveAtStage(4);
				}
				else if (wealthTotal < 75000f)
				{
					result = ThoughtState.ActiveAtStage(3);
				}
				else if (wealthTotal < 150000f)
				{
					result = ThoughtState.ActiveAtStage(2);
				}
				else if (wealthTotal < 300000f)
				{
					result = ThoughtState.ActiveAtStage(1);
				}
				else
				{
					result = ThoughtState.ActiveAtStage(0);
				}
			}
			return result;
		}
	}
}
