using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000227 RID: 551
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x040003EB RID: 1003
		private const int NumPartsAllowedWithoutThought = 2;

		// Token: 0x06000A19 RID: 2585 RVA: 0x00059894 File Offset: 0x00057C94
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = p.health.hediffSet.CountAddedParts();
			ThoughtState result;
			if (num > 0)
			{
				result = ThoughtState.ActiveAtStage(num - 2);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
