using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		public BillStack billStack = null;

		private CompPowerTrader powerComp = null;

		private CompRefuelable refuelableComp = null;

		private CompBreakdownable breakdownableComp = null;

		public bool CanWorkWithoutPower
		{
			get
			{
				return (byte)((this.powerComp == null) ? 1 : ((base.def.building.unpoweredWorkTableWorkSpeedFactor > 0.0) ? 1 : 0)) != 0;
			}
		}

		public virtual bool UsableNow
		{
			get
			{
				return (byte)((this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) ? ((this.refuelableComp == null || this.refuelableComp.HasFuel) ? ((this.breakdownableComp == null || !this.breakdownableComp.BrokenDown) ? 1 : 0) : 0) : 0) != 0;
			}
		}

		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[1]
			{
				this
			});
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.breakdownableComp = base.GetComp<CompBreakdownable>();
		}

		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		public bool CurrentlyUsableForBills()
		{
			return this.UsableNow;
		}
	}
}
