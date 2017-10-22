using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_ApparelDamaged : ThoughtWorker
	{
		public const float MinForFrayed = 0.5f;

		public const float MinForTattered = 0.2f;

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float num = 999f;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			int num2 = 0;
			ThoughtState result;
			while (true)
			{
				if (num2 < wornApparel.Count)
				{
					if (wornApparel[num2].def.useHitPoints)
					{
						float num3 = (float)wornApparel[num2].HitPoints / (float)wornApparel[num2].MaxHitPoints;
						if (num3 < num)
						{
							num = num3;
						}
						if (num < 0.20000000298023224)
						{
							result = ThoughtState.ActiveAtStage(1);
							break;
						}
					}
					num2++;
					continue;
				}
				result = ((!(num < 0.5)) ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(0));
				break;
			}
			return result;
		}
	}
}
