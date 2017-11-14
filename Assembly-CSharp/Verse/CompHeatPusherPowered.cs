using RimWorld;

namespace Verse
{
	public class CompHeatPusherPowered : CompHeatPusher
	{
		protected CompPowerTrader powerComp;

		protected CompRefuelable refuelableComp;

		protected CompBreakdownable breakdownableComp;

		protected override bool ShouldPushHeatNow
		{
			get
			{
				if (FlickUtility.WantsToBeOn(base.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown))
				{
					return true;
				}
				return false;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = base.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = base.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = base.parent.GetComp<CompBreakdownable>();
		}
	}
}
