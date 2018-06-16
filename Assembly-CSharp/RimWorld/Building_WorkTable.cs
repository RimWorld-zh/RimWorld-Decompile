using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B5 RID: 1717
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x060024DC RID: 9436 RVA: 0x0013B7A6 File Offset: 0x00139BA6
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x0013B7D8 File Offset: 0x00139BD8
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x0013B821 File Offset: 0x00139C21
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x0013B844 File Offset: 0x00139C44
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

		// Token: 0x060024E0 RID: 9440 RVA: 0x0013B8CC File Offset: 0x00139CCC
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060024E1 RID: 9441 RVA: 0x0013B8E8 File Offset: 0x00139CE8
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x0013B904 File Offset: 0x00139D04
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x0013B920 File Offset: 0x00139D20
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x0013B93C File Offset: 0x00139D3C
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn));
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x0013B994 File Offset: 0x00139D94
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
