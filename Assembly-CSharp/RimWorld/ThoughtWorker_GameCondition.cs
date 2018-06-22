using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000217 RID: 535
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x060009FA RID: 2554 RVA: 0x00058FD4 File Offset: 0x000573D4
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
