using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_DeadMansApparel : ThoughtWorker
	{
		public ThoughtWorker_DeadMansApparel()
		{
		}

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
