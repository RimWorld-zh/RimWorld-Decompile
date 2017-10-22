using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_DeadMansApparel : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			string text = (string)null;
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
			return (num != 0) ? ((num < 5) ? ThoughtState.ActiveAtStage(num - 1, text) : ThoughtState.ActiveAtStage(4, text)) : ThoughtState.Inactive;
		}
	}
}
