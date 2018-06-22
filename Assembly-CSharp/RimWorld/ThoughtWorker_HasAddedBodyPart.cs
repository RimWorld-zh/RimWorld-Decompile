using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000225 RID: 549
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x00059718 File Offset: 0x00057B18
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = 0;
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i] is Hediff_AddedPart)
				{
					num++;
				}
			}
			ThoughtState result;
			if (num > 0)
			{
				result = ThoughtState.ActiveAtStage(num - 1);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
