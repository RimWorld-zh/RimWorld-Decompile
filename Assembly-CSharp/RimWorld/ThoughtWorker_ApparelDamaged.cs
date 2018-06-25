using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000216 RID: 534
	public class ThoughtWorker_ApparelDamaged : ThoughtWorker
	{
		// Token: 0x040003E9 RID: 1001
		public const float MinForFrayed = 0.5f;

		// Token: 0x040003EA RID: 1002
		public const float MinForTattered = 0.2f;

		// Token: 0x060009F7 RID: 2551 RVA: 0x00058FBC File Offset: 0x000573BC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float num = 999f;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].def.useHitPoints)
				{
					float num2 = (float)wornApparel[i].HitPoints / (float)wornApparel[i].MaxHitPoints;
					if (num2 < num)
					{
						num = num2;
					}
					if (num < 0.2f)
					{
						return ThoughtState.ActiveAtStage(1);
					}
				}
			}
			if (num < 0.5f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}
	}
}
