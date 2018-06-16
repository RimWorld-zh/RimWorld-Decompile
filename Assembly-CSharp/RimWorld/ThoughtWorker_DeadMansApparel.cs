using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000212 RID: 530
	public class ThoughtWorker_DeadMansApparel : ThoughtWorker
	{
		// Token: 0x060009F2 RID: 2546 RVA: 0x00058C9C File Offset: 0x0005709C
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
