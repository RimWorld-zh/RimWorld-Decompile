using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CB RID: 1483
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x04001154 RID: 4436
		private Settlement settlement;

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000F74E0 File Offset: 0x000F58E0
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000F74E9 File Offset: 0x000F58E9
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x000F74FC File Offset: 0x000F58FC
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
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x000F7530 File Offset: 0x000F5930
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

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000F7564 File Offset: 0x000F5964
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

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000F75C8 File Offset: 0x000F59C8
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000F75D7 File Offset: 0x000F59D7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000F75F4 File Offset: 0x000F59F4
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

		// Token: 0x06001CCB RID: 7371 RVA: 0x000F7674 File Offset: 0x000F5A74
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_AttackSettlement>(() => CaravanArrivalAction_AttackSettlement.CanAttack(caravan, settlement), () => new CaravanArrivalAction_AttackSettlement(settlement), "AttackSettlement".Translate(new object[]
			{
				settlement.Label
			}), caravan, settlement.Tile, settlement);
		}
	}
}
