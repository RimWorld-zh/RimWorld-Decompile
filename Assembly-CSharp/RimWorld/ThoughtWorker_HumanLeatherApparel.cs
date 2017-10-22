using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_HumanLeatherApparel : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			string text = (string)null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].Stuff == ThingDefOf.Human.race.leatherDef)
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
