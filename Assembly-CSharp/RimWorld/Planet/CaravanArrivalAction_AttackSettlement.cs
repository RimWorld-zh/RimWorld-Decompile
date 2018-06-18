using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CD RID: 1485
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x06001CC9 RID: 7369 RVA: 0x000F70D4 File Offset: 0x000F54D4
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000F70DD File Offset: 0x000F54DD
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x000F70F0 File Offset: 0x000F54F0
		public override string Label
		{
			get
			{
				return "AttackSettlement".Translate(new object[]
				{
					this.settlement.Label
				});
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x000F7124 File Offset: 0x000F5524
		public override string ReportString
		{
			get
			{
				return "CaravanAttacking".Translate(new object[]
				{
					this.settlement.Label
				});
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000F7158 File Offset: 0x000F5558
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = CaravanArrivalAction_AttackSettlement.CanAttack(caravan, this.settlement);
			}
			return result;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000F71BC File Offset: 0x000F55BC
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000F71CB File Offset: 0x000F55CB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000F71E8 File Offset: 0x000F55E8
		public static FloatMenuAcceptanceReport CanAttack(Caravan caravan, Settlement settlement)
		{
			FloatMenuAcceptanceReport result;
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				result = false;
			}
			else if (settlement.EnterCooldownBlocksEntering())
			{
				result = FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
				{
					settlement.EnterCooldownDaysLeft().ToString("0.#")
				}));
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000F7268 File Offset: 0x000F5668
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_AttackSettlement>(() => CaravanArrivalAction_AttackSettlement.CanAttack(caravan, settlement), () => new CaravanArrivalAction_AttackSettlement(settlement), "AttackSettlement".Translate(new object[]
			{
				settlement.Label
			}), caravan, settlement.Tile, settlement);
		}

		// Token: 0x04001153 RID: 4435
		private Settlement settlement;
	}
}
