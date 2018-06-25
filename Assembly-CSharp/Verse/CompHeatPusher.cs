using System;

namespace Verse
{
	// Token: 0x02000E05 RID: 3589
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x04003556 RID: 13654
		private const int HeatPushInterval = 60;

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x0029C2A4 File Offset: 0x0029A6A4
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06005156 RID: 20822 RVA: 0x0029C2C4 File Offset: 0x0029A6C4
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

		// Token: 0x06005157 RID: 20823 RVA: 0x0029C31C File Offset: 0x0029A71C
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x0029C374 File Offset: 0x0029A774
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
