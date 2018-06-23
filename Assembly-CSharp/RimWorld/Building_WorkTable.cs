using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B1 RID: 1713
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x04001450 RID: 5200
		public BillStack billStack = null;

		// Token: 0x04001451 RID: 5201
		private CompPowerTrader powerComp = null;

		// Token: 0x04001452 RID: 5202
		private CompRefuelable refuelableComp = null;

		// Token: 0x04001453 RID: 5203
		private CompBreakdownable breakdownableComp = null;

		// Token: 0x060024D6 RID: 9430 RVA: 0x0013B966 File Offset: 0x00139D66
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x0013B998 File Offset: 0x00139D98
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x0013B9E1 File Offset: 0x00139DE1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0013BA04 File Offset: 0x00139E04
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

		// Token: 0x060024DA RID: 9434 RVA: 0x0013BA8C File Offset: 0x00139E8C
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x0013BAA8 File Offset: 0x00139EA8
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x0013BAC4 File Offset: 0x00139EC4
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x0013BAE0 File Offset: 0x00139EE0
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x0013BAFC File Offset: 0x00139EFC
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn));
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x0013BB54 File Offset: 0x00139F54
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}
	}
}
