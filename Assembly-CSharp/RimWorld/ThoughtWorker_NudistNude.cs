using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021B RID: 539
	public class ThoughtWorker_NudistNude : ThoughtWorker
	{
		// Token: 0x06000A01 RID: 2561 RVA: 0x00059248 File Offset: 0x00057648
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
