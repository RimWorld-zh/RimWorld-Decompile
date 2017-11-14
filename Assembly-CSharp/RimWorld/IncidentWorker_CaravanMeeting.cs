using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_CaravanMeeting : IncidentWorker
	{
		private const int MapSize = 100;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (target is Map)
			{
				return true;
			}
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(target.Tile);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (parms.target is Map)
			{
				return IncidentDefOf.TravelerGroup.Worker.TryExecute(parms);
			}
			Caravan caravan = (Caravan)parms.target;
			Faction faction;
			if (!(from x in Find.FactionManager.AllFactionsListForReading
			where !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.humanlikeFaction && x.def.caravanTraderKinds.Any()
			select x).TryRandomElement<Faction>(out faction))
			{
				return false;
			}
			CameraJumper.TryJumpAndSelect(caravan);
			List<Pawn> pawns = this.GenerateCaravanPawns(faction);
			Caravan metCaravan = CaravanMaker.MakeCaravan(pawns, faction, -1, false);
			string text = "CaravanMeeting".Translate(faction.Name, PawnUtility.PawnKindsToCommaList(metCaravan.PawnsListForReading));
			DiaNode diaNode = new DiaNode(text);
			Pawn bestPlayerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			if (metCaravan.CanTradeNow)
			{
				DiaOption diaOption = new DiaOption("CaravanMeeting_Trade".Translate());
				diaOption.action = delegate
				{
					Find.WindowStack.Add(new Dialog_Trade(bestPlayerNegotiator, metCaravan));
					string empty = string.Empty;
					string empty2 = string.Empty;
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(metCaravan.Goods.OfType<Pawn>(), ref empty, ref empty2, "LetterRelatedPawnsTradingWithOtherCaravan".Translate(), false, true);
					if (!empty2.NullOrEmpty())
					{
						Find.LetterStack.ReceiveLetter(empty, empty2, LetterDefOf.NeutralEvent, new GlobalTargetInfo(caravan.Tile), null);
					}
				};
				if (bestPlayerNegotiator == null)
				{
					diaOption.Disable("CaravanMeeting_TradeIncapable".Translate());
				}
				diaNode.options.Add(diaOption);
			}
			DiaOption diaOption2 = new DiaOption("CaravanMeeting_Attack".Translate());
			diaOption2.action = delegate
			{
				LongEventHandler.QueueLongEvent(delegate
				{
					if (!faction.HostileTo(Faction.OfPlayer))
					{
						faction.SetHostileTo(Faction.OfPlayer, true);
					}
					Pawn t = caravan.PawnsListForReading[0];
					Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(100, 1, 100), WorldObjectDefOf.AttackedCaravan);
					IntVec3 playerSpot;
					IntVec3 enemySpot;
					MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerSpot, out enemySpot);
					CaravanEnterMapUtility.Enter(caravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(playerSpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, true);
					List<Pawn> list = metCaravan.PawnsListForReading.ToList();
					CaravanEnterMapUtility.Enter(metCaravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(enemySpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, false);
					LordMaker.MakeNewLord(faction, new LordJob_DefendAttackedTraderCaravan(list[0].Position), map, list);
					Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					CameraJumper.TryJumpAndSelect(t);
					Messages.Message("MessageAttackedCaravanIsNowHostile".Translate(faction.Name), new GlobalTargetInfo(list[0].Position, list[0].Map, false), MessageTypeDefOf.ThreatBig);
				}, "GeneratingMapForNewEncounter", false, null);
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			DiaOption diaOption3 = new DiaOption("CaravanMeeting_MoveOn".Translate());
			diaOption3.action = delegate
			{
				this.RemoveAllPawnsAndPassToWorld(metCaravan);
			};
			diaOption3.resolveTree = true;
			diaNode.options.Add(diaOption3);
			string text2 = "CaravanMeetingTitle".Translate(caravan.Label);
			WindowStack windowStack = Find.WindowStack;
			DiaNode nodeRoot = diaNode;
			bool delayInteractivity = true;
			string title = text2;
			windowStack.Add(new Dialog_NodeTree(nodeRoot, delayInteractivity, false, title));
			return true;
		}

		private List<Pawn> GenerateCaravanPawns(Faction faction)
		{
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.faction = faction;
			pawnGroupMakerParms.points = TraderCaravanUtility.GenerateGuardPoints();
			return PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Trader, pawnGroupMakerParms, true).ToList();
		}

		private void RemoveAllPawnsAndPassToWorld(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Find.WorldPawns.PassToWorld(pawnsListForReading[i], PawnDiscardDecideMode.Discard);
			}
			caravan.RemoveAllPawns();
		}
	}
}
