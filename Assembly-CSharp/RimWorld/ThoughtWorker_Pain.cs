using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000221 RID: 545
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		// Token: 0x06000A0E RID: 2574 RVA: 0x000594FC File Offset: 0x000578FC
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
