using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		public ThoughtWorker_GameCondition()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.SpawnedOrAnyParentSpawned && p.MapHeld.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				result = true;
			}
			else if (Find.World.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
