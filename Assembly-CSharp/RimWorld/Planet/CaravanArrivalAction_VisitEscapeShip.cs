using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CC RID: 1484
	public class CaravanArrivalAction_VisitEscapeShip : CaravanArrivalAction
	{
		// Token: 0x04001153 RID: 4435
		private MapParent target;

		// Token: 0x06001CDC RID: 7388 RVA: 0x000F79BB File Offset: 0x000F5DBB
		public CaravanArrivalAction_VisitEscapeShip()
		{
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000F79C4 File Offset: 0x000F5DC4
		public CaravanArrivalAction_VisitEscapeShip(EscapeShipComp escapeShip)
		{
			this.target = (MapParent)escapeShip.parent;
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001CDE RID: 7390 RVA: 0x000F79E0 File Offset: 0x000F5DE0
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
		// (get) Token: 0x06001CDF RID: 7391 RVA: 0x000F7A14 File Offset: 0x000F5E14
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

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000F7A48 File Offset: 0x000F5E48
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

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000F7AAC File Offset: 0x000F5EAC
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

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000F7B09 File Offset: 0x000F5F09
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.target, "target", false);
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000F7B24 File Offset: 0x000F5F24
		private void DoArrivalAction(Caravan caravan)
		{
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.target.Tile, null);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.UnloadIndividually, false, null);
			Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000F7B5C File Offset: 0x000F5F5C
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

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000F7BDC File Offset: 0x000F5FDC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent escapeShip)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitEscapeShip>(() => CaravanArrivalAction_VisitEscapeShip.CanVisit(caravan, escapeShip), () => new CaravanArrivalAction_VisitEscapeShip(escapeShip.GetComponent<EscapeShipComp>()), "VisitEscapeShip".Translate(new object[]
			{
				escapeShip.Label
			}), caravan, escapeShip.Tile, escapeShip);
		}
	}
}
