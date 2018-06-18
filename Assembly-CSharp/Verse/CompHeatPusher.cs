using System;

namespace Verse
{
	// Token: 0x02000E05 RID: 3589
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x0600513D RID: 20797 RVA: 0x0029A8BC File Offset: 0x00298CBC
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x0029A8DC File Offset: 0x00298CDC
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

		// Token: 0x0600513F RID: 20799 RVA: 0x0029A934 File Offset: 0x00298D34
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x0029A98C File Offset: 0x00298D8C
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.16666651f);
			}
		}

		// Token: 0x04003548 RID: 13640
		private const int HeatPushInterval = 60;
	}
}
