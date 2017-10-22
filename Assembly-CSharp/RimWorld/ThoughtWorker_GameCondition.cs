using Verse;

namespace RimWorld
{
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return (!p.SpawnedOrAnyParentSpawned || !p.MapHeld.gameConditionManager.ConditionIsActive(base.def.gameCondition)) ? ((!Find.World.gameConditionManager.ConditionIsActive(base.def.gameCondition)) ? false : true) : true;
		}
	}
}
