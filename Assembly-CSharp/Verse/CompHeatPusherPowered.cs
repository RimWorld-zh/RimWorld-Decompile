using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E05 RID: 3589
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x04003550 RID: 13648
		protected CompPowerTrader powerComp;

		// Token: 0x04003551 RID: 13649
		protected CompRefuelable refuelableComp;

		// Token: 0x04003552 RID: 13650
		protected CompBreakdownable breakdownableComp;

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x0600515A RID: 20826 RVA: 0x0029C0E8 File Offset: 0x0029A4E8
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x0029C172 File Offset: 0x0029A572
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}
	}
}
