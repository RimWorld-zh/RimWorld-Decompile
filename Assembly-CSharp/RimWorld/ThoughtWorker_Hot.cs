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
			return (!(num <= 0.0)) ? ((!(num < 10.0)) ? ((!(num < 20.0)) ? ((!(num < 30.0)) ? ThoughtState.ActiveAtStage(3) : ThoughtState.ActiveAtStage(2)) : ThoughtState.ActiveAtStage(1)) : ThoughtState.ActiveAtStage(0)) : ThoughtState.Inactive;
		}
	}
}
