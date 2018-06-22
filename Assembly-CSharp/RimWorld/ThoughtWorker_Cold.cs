using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020E RID: 526
	public class ThoughtWorker_Cold : ThoughtWorker
	{
		// Token: 0x060009E8 RID: 2536 RVA: 0x00058A5C File Offset: 0x00056E5C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMin, true);
			float ambientTemperature = p.AmbientTemperature;
			float num = statValue - ambientTemperature;
			ThoughtState result;
			if (num <= 0f)
			{
				result = ThoughtState.Inactive;
			}
			else if (num < 10f)
			{
				result = ThoughtState.ActiveAtStage(0);
			}
			else if (num < 20f)
			{
				result = ThoughtState.ActiveAtStage(1);
			}
			else if (num < 30f)
			{
				result = ThoughtState.ActiveAtStage(2);
			}
			else
			{
				result = ThoughtState.ActiveAtStage(3);
			}
			return result;
		}
	}
}
