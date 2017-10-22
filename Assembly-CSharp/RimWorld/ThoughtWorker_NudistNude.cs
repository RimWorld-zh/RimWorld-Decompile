using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_NudistNude : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			int num = 0;
			ThoughtState result;
			while (true)
			{
				if (num < wornApparel.Count)
				{
					Apparel apparel = wornApparel[num];
					for (int i = 0; i < apparel.def.apparel.bodyPartGroups.Count; i++)
					{
						if (apparel.def.apparel.bodyPartGroups[i] == BodyPartGroupDefOf.Torso)
							goto IL_0045;
						if (apparel.def.apparel.bodyPartGroups[i] == BodyPartGroupDefOf.Legs)
							goto IL_0072;
					}
					num++;
					continue;
				}
				result = true;
				break;
				IL_0045:
				result = false;
				break;
				IL_0072:
				result = false;
				break;
			}
			return result;
		}
	}
}
