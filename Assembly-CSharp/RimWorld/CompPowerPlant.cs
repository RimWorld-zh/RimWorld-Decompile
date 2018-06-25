using System;

namespace RimWorld
{
	// Token: 0x0200041B RID: 1051
	public class CompPowerPlant : CompPowerTrader
	{
		// Token: 0x04000B0A RID: 2826
		protected CompRefuelable refuelableComp;

		// Token: 0x04000B0B RID: 2827
		protected CompBreakdownable breakdownableComp;

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x0009E324 File Offset: 0x0009C724
		protected virtual float DesiredPowerOutput
		{
			get
			{
				return -base.Props.basePowerConsumption;
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0009E348 File Offset: 0x0009C748
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
			if (base.Props.basePowerConsumption < 0f && !this.parent.IsBrokenDown() && FlickUtility.WantsToBeOn(this.parent))
			{
				base.PowerOn = true;
			}
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0009E3BD File Offset: 0x0009C7BD
		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0009E3CC File Offset: 0x0009C7CC
		public void UpdateDesiredPowerOutput()
		{
			if ((this.breakdownableComp != null && this.breakdownableComp.BrokenDown) || (this.refuelableComp != null && !this.refuelableComp.HasFuel) || (this.flickableComp != null && !this.flickableComp.SwitchIsOn) || !base.PowerOn)
			{
				base.PowerOutput = 0f;
			}
			else
			{
				base.PowerOutput = this.DesiredPowerOutput;
			}
		}
	}
}
