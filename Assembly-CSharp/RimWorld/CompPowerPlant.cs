namespace RimWorld
{
	public class CompPowerPlant : CompPowerTrader
	{
		protected CompRefuelable refuelableComp;

		protected CompBreakdownable breakdownableComp;

		protected virtual float DesiredPowerOutput
		{
			get
			{
				return (float)(0.0 - base.Props.basePowerConsumption);
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = base.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = base.parent.GetComp<CompBreakdownable>();
			if (base.Props.basePowerConsumption < 0.0 && !base.parent.IsBrokenDown())
			{
				base.PowerOn = true;
			}
		}

		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
		}

		public void UpdateDesiredPowerOutput()
		{
			if (this.breakdownableComp != null && this.breakdownableComp.BrokenDown)
			{
				goto IL_005c;
			}
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				goto IL_005c;
			}
			if (base.flickableComp != null && !base.flickableComp.SwitchIsOn)
			{
				goto IL_005c;
			}
			if (!base.PowerOn)
				goto IL_005c;
			base.PowerOutput = this.DesiredPowerOutput;
			return;
			IL_005c:
			base.PowerOutput = 0f;
		}
	}
}
