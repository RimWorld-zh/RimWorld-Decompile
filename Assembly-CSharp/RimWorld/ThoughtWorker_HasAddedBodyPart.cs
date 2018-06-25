using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000227 RID: 551
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x00059898 File Offset: 0x00057C98
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = p.health.hediffSet.CountAddedParts();
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
