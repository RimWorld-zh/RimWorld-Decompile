using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E03 RID: 3587
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x04003550 RID: 13648
		protected CompPowerTrader powerComp;

		// Token: 0x04003551 RID: 13649
		protected CompRefuelable refuelableComp;

		// Token: 0x04003552 RID: 13650
		protected CompBreakdownable breakdownableComp;

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06005156 RID: 20822 RVA: 0x0029BFBC File Offset: 0x0029A3BC
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x0029C046 File Offset: 0x0029A446
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}
	}
}
