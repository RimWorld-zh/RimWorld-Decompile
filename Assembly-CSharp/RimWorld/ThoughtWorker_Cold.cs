using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Cold : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMin, true);
			float ambientTemperature = p.AmbientTemperature;
			float num = statValue - ambientTemperature;
			if (num <= 0.0)
			{
				return ThoughtState.Inactive;
			}
			if (num < 10.0)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num < 20.0)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.ActiveAtStage(2);
		}
	}
}
