using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020F RID: 527
	public class ThoughtWorker_Hot : ThoughtWorker
	{
		// Token: 0x060009EC RID: 2540 RVA: 0x00058AB0 File Offset: 0x00056EB0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMax, true);
			float ambientTemperature = p.AmbientTemperature;
			float num = ambientTemperature - statValue;
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
