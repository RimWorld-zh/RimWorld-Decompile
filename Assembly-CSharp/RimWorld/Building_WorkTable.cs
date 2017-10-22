using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		public BillStack billStack;

		private CompPowerTrader powerComp;

		private CompRefuelable refuelableComp;

		private CompBreakdownable breakdownableComp;

		public bool CanWorkWithoutPower
		{
			get
			{
				if (this.powerComp == null)
				{
					return true;
				}
				if (base.def.building.unpoweredWorkTableWorkSpeedFactor > 0.0)
				{
					return true;
				}
				return false;
			}
		}

		public virtual bool UsableNow
		{
			get
			{
				if (!this.CanWorkWithoutPower && (this.powerComp == null || !this.powerComp.PowerOn))
				{
					return false;
				}
				if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
				{
					return false;
				}
				if (this.breakdownableComp != null && this.breakdownableComp.BrokenDown)
				{
					return false;
				}
				return true;
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

		public bool CurrentlyUsable()
		{
			return this.UsableNow;
		}

		virtual Map get_Map()
		{
			return base.Map;
		}

		Map IBillGiver.get_Map()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_Map
			return this.get_Map();
		}
	}
}
