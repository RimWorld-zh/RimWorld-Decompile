namespace Verse
{
	public class CompHeatPusher : ThingComp
	{
		private const int HeatPushInterval = 60;

		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)base.props;
			}
		}

		protected virtual bool ShouldPushHeatNow
		{
			get
			{
				return true;
			}
		}

		public override void CompTick()
		{
			base.CompTick();
			if (base.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				CompProperties_HeatPusher props = this.Props;
				float ambientTemperature = base.parent.AmbientTemperature;
				if (ambientTemperature < props.heatPushMaxTemperature && ambientTemperature > props.heatPushMinTemperature)
				{
					GenTemperature.PushHeat(base.parent.Position, base.parent.Map, props.heatPerSecond);
				}
			}
		}
	}
}
