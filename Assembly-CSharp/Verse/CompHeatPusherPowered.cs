using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E06 RID: 3590
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x04003557 RID: 13655
		protected CompPowerTrader powerComp;

		// Token: 0x04003558 RID: 13656
		protected CompRefuelable refuelableComp;

		// Token: 0x04003559 RID: 13657
		protected CompBreakdownable breakdownableComp;

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x0600515A RID: 20826 RVA: 0x0029C3C8 File Offset: 0x0029A7C8
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x0029C452 File Offset: 0x0029A852
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}
	}
}
