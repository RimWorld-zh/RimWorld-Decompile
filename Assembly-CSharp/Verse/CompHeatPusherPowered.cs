using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E06 RID: 3590
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06005142 RID: 20802 RVA: 0x0029A9E0 File Offset: 0x00298DE0
		protected override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x0029AA6A File Offset: 0x00298E6A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x04003549 RID: 13641
		protected CompPowerTrader powerComp;

		// Token: 0x0400354A RID: 13642
		protected CompRefuelable refuelableComp;

		// Token: 0x0400354B RID: 13643
		protected CompBreakdownable breakdownableComp;
	}
}
