using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200031F RID: 799
	public class IncidentWorker_CaravanMeeting : IncidentWorker
	{
		// Token: 0x06000DA5 RID: 3493 RVA: 0x00074AB8 File Offset: 0x00072EB8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return parms.target is Map || (CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile) && this.TryFindFaction(out faction));
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00074B04 File Offset: 0x00072F04
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			bool result;
			if (parms.target is Map)
			{
				result = IncidentDefOf.TravelerGroup.Worker.TryExecute(parms);
			}
			else
			{
				Caravan caravan = (Caravan)parms.target;
				Faction faction;
				if (!this.TryFindFaction(out faction))
				{
					result = false;
				}
				else
				{
					CameraJumper.TryJumpAndSelect(caravan);
					List<Pawn> pawns = this.GenerateCaravanPawns(faction);
					Caravan metCaravan = CaravanMaker.MakeCaravan(pawns, faction, -1, false);
					string text = "CaravanMeeting".Translate(new object[]
					{
						caravan.Name,
						faction.Name,
						PawnUtility.PawnKindsToCommaList(metCaravan.PawnsListForReading, true)
					}).CapitalizeFirst();
					DiaNode diaNode = new DiaNode(text);
					Pawn bestPlayerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
					if (metCaravan.CanTradeNow)
					{
						DiaOption diaOption = new DiaOption("CaravanMeeting_Trade".Translate());
						diaOption.action = delegate()
						{
							Find.WindowStack.Add(new Dialog_Trade(bestPlayerNegotiator, metCaravan, false));
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(metCaravan.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithOtherCaravan".Translate(new object[]
							{
								Faction.OfPlayer.def.pawnsPlural
							}), LetterDefOf.NeutralEvent, false, true);
						};
						if (bestPlayerNegotiator == null)
						{
							diaOption.Disable("CaravanMeeting_TradeIncapable".Translate());
						}
						diaNode.options.Add(diaOption);
					}
					DiaOption diaOption2 = new DiaOption("CaravanMeeting_Attack".Translate());
					diaOption2.action = delegate()
					{
						LongEventHandler.QueueLongEvent(delegate()
						{
							Pawn t = caravan.PawnsListForReading[0];
							Faction faction2 = faction;
							Faction ofPlayer = Faction.OfPlayer;
							FactionRelationKind kind = FactionRelationKind.Hostile;
							string reason = "GoodwillChangedReason_AttackedCaravan".Translate();
							faction2.TrySetRelationKind(ofPlayer, kind, true, reason, new GlobalTargetInfo?(t));
							Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(100, 1, 100), WorldObjectDefOf.AttackedNonPlayerCaravan);
							IntVec3 playerSpot;
							IntVec3 enemySpot;
							MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerSpot, out enemySpot);
							CaravanEnterMapUtility.Enter(caravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(playerSpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, true);
							List<Pawn> list = metCaravan.PawnsListForReading.ToList<Pawn>();
							CaravanEnterMapUtility.Enter(metCaravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(enemySpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, false);
							LordMaker.MakeNewLord(faction, new LordJob_DefendAttackedTraderCaravan(list[0].Position), map, list);
							Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
							CameraJumper.TryJumpAndSelect(t);
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(list, "LetterRelatedPawnsGroupGeneric".Translate(new object[]
							{
								Faction.OfPlayer.def.pawnsPlural
							}), LetterDefOf.NeutralEvent, true, true);
						}, "GeneratingMapForNewEncounter", false, null);
					};
					diaOption2.resolveTree = true;
					diaNode.options.Add(diaOption2);
					DiaOption diaOption3 = new DiaOption("CaravanMeeting_MoveOn".Translate());
					diaOption3.action = delegate()
					{
						this.RemoveAllPawnsAndPassToWorld(metCaravan);
					};
					diaOption3.resolveTree = true;
					diaNode.options.Add(diaOption3);
					string text2 = "CaravanMeetingTitle".Translate(new object[]
					{
						caravan.Label
					});
					WindowStack windowStack = Find.WindowStack;
					DiaNode nodeRoot = diaNode;
					Faction faction3 = faction;
					bool delayInteractivity = true;
					string title = text2;
					windowStack.Add(new Dialog_NodeTreeWithFactionInfo(nodeRoot, faction3, delayInteractivity, false, title));
					Find.Archive.Add(new ArchivedDialog(diaNode.text, text2, faction));
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00074D48 File Offset: 0x00073148
		private bool TryFindFaction(out Faction faction)
		{
			return (from x in Find.FactionManager.AllFactionsListForReading
			where !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.humanlikeFaction && x.def.caravanTraderKinds.Any<TraderKindDef>()
			select x).TryRandomElement(out faction);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00074D90 File Offset: 0x00073190
		private List<Pawn> GenerateCaravanPawns(Faction faction)
		{
			return PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Trader,
				faction = faction,
				points = TraderCaravanUtility.GenerateGuardPoints(),
				dontUseSingleUseRocketLaunchers = true
			}, true).ToList<Pawn>();
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00074DDC File Offset: 0x000731DC
		private void RemoveAllPawnsAndPassToWorld(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Find.WorldPawns.PassToWorld(pawnsListForReading[i], PawnDiscardDecideMode.Decide);
			}
			caravan.RemoveAllPawns();
		}

		// Token: 0x040008B8 RID: 2232
		private const int MapSize = 100;
	}
}
