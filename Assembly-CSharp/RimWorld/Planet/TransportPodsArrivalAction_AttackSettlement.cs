using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000611 RID: 1553
	public class TransportPodsArrivalAction_AttackSettlement : TransportPodsArrivalAction
	{
		// Token: 0x04001242 RID: 4674
		private Settlement settlement;

		// Token: 0x04001243 RID: 4675
		private PawnsArrivalModeDef arrivalMode;

		// Token: 0x06001F42 RID: 8002 RVA: 0x0010F5E3 File Offset: 0x0010D9E3
		public TransportPodsArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0010F5EC File Offset: 0x0010D9EC
		public TransportPodsArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0010F603 File Offset: 0x0010DA03
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0010F630 File Offset: 0x0010DA30
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
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
				result = TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, this.settlement);
			}
			return result;
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0010F694 File Offset: 0x0010DA94
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.settlement.HasMap;
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0010F6B8 File Offset: 0x0010DAB8
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.settlement.Tile, null);
			string label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			string text = "LetterTransportPodsLandedInEnemyBase".Translate(new object[]
			{
				this.settlement.Label
			}).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(this.settlement, ref text);
			if (flag)
			{
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, this.settlement.Faction, null);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0010F7AC File Offset: 0x0010DBAC
		public static FloatMenuAcceptanceReport CanAttack(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			FloatMenuAcceptanceReport result;
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				result = false;
			}
			else if (!TransportPodsArrivalActionUtility.AnyNonDownedColonist(pods))
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

		// Token: 0x06001F49 RID: 8009 RVA: 0x0010F844 File Offset: 0x0010DC44
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			foreach (FloatMenuOption f in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement), () => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop), "AttackAndDropAtEdge".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile))
			{
				yield return f;
			}
			foreach (FloatMenuOption f2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement), () => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop), "AttackAndDropInCenter".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile))
			{
				yield return f2;
			}
			yield break;
		}
	}
}
