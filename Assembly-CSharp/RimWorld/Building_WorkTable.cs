using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B5 RID: 1717
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x060024DE RID: 9438 RVA: 0x0013B81E File Offset: 0x00139C1E
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x0013B850 File Offset: 0x00139C50
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x0013B899 File Offset: 0x00139C99
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0013B8BC File Offset: 0x00139CBC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.breakdownableComp = base.GetComp<CompBreakdownable>();
			foreach (Bill bill in this.billStack)
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0013B944 File Offset: 0x00139D44
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x0013B960 File Offset: 0x00139D60
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x0013B97C File Offset: 0x00139D7C
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060024E5 RID: 9445 RVA: 0x0013B998 File Offset: 0x00139D98
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x0013B9B4 File Offset: 0x00139DB4
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn));
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x0013BA0C File Offset: 0x00139E0C
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}

		// Token: 0x04001452 RID: 5202
		public BillStack billStack = null;

		// Token: 0x04001453 RID: 5203
		private CompPowerTrader powerComp = null;

		// Token: 0x04001454 RID: 5204
		private CompRefuelable refuelableComp = null;

		// Token: 0x04001455 RID: 5205
		private CompBreakdownable breakdownableComp = null;
	}
}
