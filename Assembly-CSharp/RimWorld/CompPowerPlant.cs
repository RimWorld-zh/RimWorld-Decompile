using System;

namespace RimWorld
{
	// Token: 0x02000419 RID: 1049
	public class CompPowerPlant : CompPowerTrader
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x0009DFE0 File Offset: 0x0009C3E0
		protected virtual float DesiredPowerOutput
		{
			get
			{
				return -base.Props.basePowerConsumption;
			}
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0009E004 File Offset: 0x0009C404
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

		// Token: 0x06001234 RID: 4660 RVA: 0x0009E079 File Offset: 0x0009C479
		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0009E088 File Offset: 0x0009C488
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

		// Token: 0x04000B06 RID: 2822
		protected CompRefuelable refuelableComp;

		// Token: 0x04000B07 RID: 2823
		protected CompBreakdownable breakdownableComp;
	}
}
