using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_Tale : Thought_SituationalSocial
	{
		public override float OpinionOffset()
		{
			Tale latestTale = Find.TaleManager.GetLatestTale(base.def.taleDef, base.otherPawn);
			float result;
			if (latestTale != null)
			{
				float num = 1f;
				if (latestTale.def.type == TaleType.Expirable)
				{
					float value = (float)((float)latestTale.AgeTicks / (latestTale.def.expireDays * 60000.0));
					num = Mathf.InverseLerp(1f, base.def.lerpOpinionToZeroAfterDurationPct, value);
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
