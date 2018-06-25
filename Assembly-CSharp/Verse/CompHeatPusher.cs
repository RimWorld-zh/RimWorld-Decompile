using System;

namespace Verse
{
	public class CompHeatPusher : ThingComp
	{
		private const int HeatPushInterval = 60;

		public CompHeatPusher()
		{
		}

		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		protected virtual bool ShouldPushHeatNow
		{
			get
			{
				bool result;
				if (!this.parent.SpawnedOrAnyParentSpawned)
				{
					result = false;
				}
				else
				{
					CompProperties_HeatPusher props = this.Props;
					float ambientTemperature = this.parent.AmbientTemperature;
					result = (ambientTemperature < props.heatPushMaxTemperature && ambientTemperature > props.heatPushMinTemperature);
				}
				return result;
			}
		}

		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.16666651f);
			}
		}
	}
}
