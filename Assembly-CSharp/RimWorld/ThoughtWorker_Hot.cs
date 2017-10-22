using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Hot : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMax, true);
			float ambientTemperature = p.AmbientTemperature;
			float num = ambientTemperature - statValue;
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
			if (num < 30.0)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
