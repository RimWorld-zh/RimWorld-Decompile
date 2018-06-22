using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021F RID: 543
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		// Token: 0x06000A0A RID: 2570 RVA: 0x0005937C File Offset: 0x0005777C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			ThoughtState result;
			if (painTotal < 0.0001f)
			{
				result = ThoughtState.Inactive;
			}
			else if (painTotal < 0.15f)
			{
				result = ThoughtState.ActiveAtStage(0);
			}
			else if (painTotal < 0.4f)
			{
				result = ThoughtState.ActiveAtStage(1);
			}
			else if (painTotal < 0.8f)
			{
				result = ThoughtState.ActiveAtStage(2);
			}
			else
			{
				result = ThoughtState.ActiveAtStage(3);
			}
			return result;
		}
	}
}
