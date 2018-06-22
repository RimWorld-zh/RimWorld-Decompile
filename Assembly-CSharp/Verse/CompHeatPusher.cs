using System;

namespace Verse
{
	// Token: 0x02000E02 RID: 3586
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06005151 RID: 20817 RVA: 0x0029BE98 File Offset: 0x0029A298
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x0029BEB8 File Offset: 0x0029A2B8
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

		// Token: 0x06005153 RID: 20819 RVA: 0x0029BF10 File Offset: 0x0029A310
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x0029BF68 File Offset: 0x0029A368
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.16666651f);
			}
		}

		// Token: 0x0400354F RID: 13647
		private const int HeatPushInterval = 60;
	}
}
