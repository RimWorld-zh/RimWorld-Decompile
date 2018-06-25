using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000219 RID: 537
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x060009FD RID: 2557 RVA: 0x00059150 File Offset: 0x00057550
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
