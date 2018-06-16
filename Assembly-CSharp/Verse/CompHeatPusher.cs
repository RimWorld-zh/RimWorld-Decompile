using System;

namespace Verse
{
	// Token: 0x02000E06 RID: 3590
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x0600513F RID: 20799 RVA: 0x0029A8DC File Offset: 0x00298CDC
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06005140 RID: 20800 RVA: 0x0029A8FC File Offset: 0x00298CFC
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

		// Token: 0x06005141 RID: 20801 RVA: 0x0029A954 File Offset: 0x00298D54
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x0029A9AC File Offset: 0x00298DAC
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.16666651f);
			}
		}

		// Token: 0x0400354A RID: 13642
		private const int HeatPushInterval = 60;
	}
}
