using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021B RID: 539
	public class ThoughtWorker_NudistNude : ThoughtWorker
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0005924C File Offset: 0x0005764C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						return false;
					}
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
