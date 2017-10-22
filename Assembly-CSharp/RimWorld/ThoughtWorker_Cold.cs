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
			return (!(num <= 0.0)) ? ((!(num < 10.0)) ? ((!(num < 20.0)) ? ThoughtState.ActiveAtStage(2) : ThoughtState.ActiveAtStage(1)) : ThoughtState.ActiveAtStage(0)) : ThoughtState.Inactive;
		}
	}
}
