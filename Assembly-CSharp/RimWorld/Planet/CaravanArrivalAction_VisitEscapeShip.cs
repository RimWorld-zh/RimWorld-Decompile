using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D0 RID: 1488
	public class CaravanArrivalAction_VisitEscapeShip : CaravanArrivalAction
	{
		// Token: 0x06001CE5 RID: 7397 RVA: 0x000F7967 File Offset: 0x000F5D67
		public CaravanArrivalAction_VisitEscapeShip()
		{
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000F7970 File Offset: 0x000F5D70
		public CaravanArrivalAction_VisitEscapeShip(EscapeShipComp escapeShip)
		{
			this.target = (MapParent)escapeShip.parent;
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000F798C File Offset: 0x000F5D8C
		public override string Label
		{
			get
			{
				return "VisitEscapeShip".Translate(new object[]
				{
					this.target.Label
				});
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x000F79C0 File Offset: 0x000F5DC0
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(new object[]
				{
					this.target.Label
				});
			}
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x000F79F4 File Offset: 0x000F5DF4
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.target != null && this.target.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, this.target);
			}
			return result;
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000F7A58 File Offset: 0x000F5E58
		public override void Arrived(Caravan caravan)
		{
			if (!this.target.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.target.SetFaction(Faction.OfPlayer);
					this.DoArrivalAction(caravan);
					Find.LetterStack.ReceiveLetter("EscapeShipFoundLabel".Translate(), "EscapeShipFound".Translate(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.target.Map.Center, this.target.Map, false), null, null);
				}, "GeneratingMapForNewEncounter", false, null);
			}
			else
			{
				this.DoArrivalAction(caravan);
			}
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000F7AB5 File Offset: 0x000F5EB5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.target, "target", false);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000F7AD0 File Offset: 0x000F5ED0
		private void DoArrivalAction(Caravan caravan)
		{
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.target.Tile, null);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.UnloadIndividually, false, null);
			Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000F7B08 File Offset: 0x000F5F08
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, MapParent escapeShip)
		{
			FloatMenuAcceptanceReport result;
			if (escapeShip == null || !escapeShip.Spawned || escapeShip.GetComponent<EscapeShipComp>() == null)
			{
				result = false;
			}
			else if (escapeShip.EnterCooldownBlocksEntering())
			{
				result = FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
				{
					escapeShip.EnterCooldownDaysLeft().ToString("0.#")
				}));
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000F7B88 File Offset: 0x000F5F88
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent escapeShip)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitEscapeShip>(() => CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, escapeShip), () => new CaravanArrivalAction_VisitEscapeShip(escapeShip.GetComponent<EscapeShipComp>()), "VisitEscapeShip".Translate(new object[]
			{
				escapeShip.Label
			}), caravan, escapeShip.Tile, escapeShip);
		}

		// Token: 0x04001156 RID: 4438
		private MapParent target;
	}
}
