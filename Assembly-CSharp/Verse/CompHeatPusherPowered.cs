using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E07 RID: 3591
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06005144 RID: 20804 RVA: 0x0029AA00 File Offset: 0x00298E00
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x0029AA8A File Offset: 0x00298E8A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x0400354B RID: 13643
		protected CompPowerTrader powerComp;

		// Token: 0x0400354C RID: 13644
		protected CompRefuelable refuelableComp;

		// Token: 0x0400354D RID: 13645
		protected CompBreakdownable breakdownableComp;
	}
}
