using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000217 RID: 535
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x00058F90 File Offset: 0x00057390
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
