using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B3 RID: 1715
	public class Building_WorkTable : Building, IBillGiver, IBillGiverWithTickAction
	{
		// Token: 0x04001454 RID: 5204
		public BillStack billStack = null;

		// Token: 0x04001455 RID: 5205
		private CompPowerTrader powerComp = null;

		// Token: 0x04001456 RID: 5206
		private CompRefuelable refuelableComp = null;

		// Token: 0x04001457 RID: 5207
		private CompBreakdownable breakdownableComp = null;

		// Token: 0x060024D9 RID: 9433 RVA: 0x0013BD1E File Offset: 0x0013A11E
		public Building_WorkTable()
		{
			this.billStack = new BillStack(this);
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x0013BD50 File Offset: 0x0013A150
		public bool CanWorkWithoutPower
		{
			get
			{
				return this.powerComp == null || this.def.building.unpoweredWorkTableWorkSpeedFactor > 0f;
			}
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x0013BD99 File Offset: 0x0013A199
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<BillStack>(ref this.billStack, "billStack", new object[]
			{
				this
			});
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x0013BDBC File Offset: 0x0013A1BC
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

		// Token: 0x060024DD RID: 9437 RVA: 0x0013BE44 File Offset: 0x0013A244
		public virtual void UsedThisTick()
		{
			if (this.refuelableComp != null)
			{
				this.refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x0013BE60 File Offset: 0x0013A260
		public BillStack BillStack
		{
			get
			{
				return this.billStack;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x0013BE7C File Offset: 0x0013A27C
		public IntVec3 BillInteractionCell
		{
			get
			{
				return this.InteractionCell;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060024E0 RID: 9440 RVA: 0x0013BE98 File Offset: 0x0013A298
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				return GenAdj.CellsOccupiedBy(this);
			}
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0013BEB4 File Offset: 0x0013A2B4
		public bool CurrentlyUsableForBills()
		{
			return this.UsableForBillsAfterFueling() && (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn));
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0013BF0C File Offset: 0x0013A30C
		public bool UsableForBillsAfterFueling()
		{
			return (this.CanWorkWithoutPower || (this.powerComp != null && this.powerComp.PowerOn)) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
		}
	}
}
