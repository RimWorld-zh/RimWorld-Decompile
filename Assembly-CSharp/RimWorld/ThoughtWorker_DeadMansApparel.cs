using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000214 RID: 532
	public class ThoughtWorker_DeadMansApparel : ThoughtWorker
	{
		// Token: 0x060009F3 RID: 2547 RVA: 0x00058E5C File Offset: 0x0005725C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].WornByCorpse)
				{
					if (text == null)
					{
						text = wornApparel[i].def.label;
					}
					num++;
				}
			}
			ThoughtState result;
			if (num == 0)
			{
				result = ThoughtState.Inactive;
			}
			else if (num >= 5)
			{
				result = ThoughtState.ActiveAtStage(4, text);
			}
			else
			{
				result = ThoughtState.ActiveAtStage(num - 1, text);
			}
			return result;
		}
	}
}
