using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_Tale : Thought_SituationalSocial
	{
		public Thought_Tale()
		{
		}

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
