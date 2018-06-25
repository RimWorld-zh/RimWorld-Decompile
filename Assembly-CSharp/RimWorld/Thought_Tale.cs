using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020F RID: 527
	public class Thought_Tale : Thought_SituationalSocial
	{
		// Token: 0x060009E9 RID: 2537 RVA: 0x00058B34 File Offset: 0x00056F34
		public override float OpinionOffset()
		{
			Tale latestTale = Find.TaleManager.GetLatestTale(this.def.taleDef, this.otherPawn);
			float result;
			if (latestTale != null)
			{
				float num = 1f;
				if (latestTale.def.type == TaleType.Expirable)
				{
					float value = (float)latestTale.AgeTicks / (latestTale.def.expireDays * 60000f);
					num = Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, value);
				}
				result = base.CurStage.baseOpinionOffset * num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
