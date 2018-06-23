using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C9 RID: 1481
	public class CaravanArrivalAction_AttackSettlement : CaravanArrivalAction
	{
		// Token: 0x04001150 RID: 4432
		private Settlement settlement;

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000F7128 File Offset: 0x000F5528
		public CaravanArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000F7131 File Offset: 0x000F5531
		public CaravanArrivalAction_AttackSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x000F7144 File Offset: 0x000F5544
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
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x000F7178 File Offset: 0x000F5578
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

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000F71AC File Offset: 0x000F55AC
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

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000F7210 File Offset: 0x000F5610
		public override void Arrived(Caravan caravan)
		{
			SettlementUtility.Attack(caravan, this.settlement);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000F721F File Offset: 0x000F561F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000F723C File Offset: 0x000F563C
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

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000F72BC File Offset: 0x000F56BC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_AttackSettlement>(() => CaravanArrivalAction_AttackSettlement.CanAttack(caravan, settlement), () => new CaravanArrivalAction_AttackSettlement(settlement), "AttackSettlement".Translate(new object[]
			{
				settlement.Label
			}), caravan, settlement.Tile, settlement);
		}
	}
}
