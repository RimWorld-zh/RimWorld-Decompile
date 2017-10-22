namespace Verse
{
	public class CompProperties_HeatPusher : CompProperties
	{
		public float heatPerSecond = 0f;

		public float heatPushMaxTemperature = 99999f;

		public float heatPushMinTemperature = -99999f;

		public CompProperties_HeatPusher()
		{
			base.compClass = typeof(CompHeatPusher);
		}
	}
}
