using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_CaravanMeeting : IncidentWorker
	{
		private const int MapSize = 100;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache0;

		public IncidentWorker_CaravanMeeting()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return parms.target is Map || (CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile) && this.TryFindFaction(out faction));
		}

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

		private bool TryFindFaction(out Faction faction)
		{
			return (from x in Find.FactionManager.AllFactionsListForReading
			where !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.humanlikeFaction && x.def.caravanTraderKinds.Any<TraderKindDef>()
			select x).TryRandomElement(out faction);
		}

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

		private void RemoveAllPawnsAndPassToWorld(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Find.WorldPawns.PassToWorld(pawnsListForReading[i], PawnDiscardDecideMode.Decide);
			}
			caravan.RemoveAllPawns();
		}

		[CompilerGenerated]
		private static bool <TryFindFaction>m__0(Faction x)
		{
			return !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.humanlikeFaction && x.def.caravanTraderKinds.Any<TraderKindDef>();
		}

		[CompilerGenerated]
		private sealed class <TryExecuteWorker>c__AnonStorey0
		{
			internal Pawn bestPlayerNegotiator;

			internal Caravan metCaravan;

			internal Caravan caravan;

			internal Faction faction;

			internal IncidentWorker_CaravanMeeting $this;

			public <TryExecuteWorker>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Find.WindowStack.Add(new Dialog_Trade(this.bestPlayerNegotiator, this.metCaravan, false));
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(this.metCaravan.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithOtherCaravan".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), LetterDefOf.NeutralEvent, false, true);
			}

			internal void <>m__1()
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					Pawn t = this.caravan.PawnsListForReading[0];
					Faction faction = this.faction;
					Faction ofPlayer = Faction.OfPlayer;
					FactionRelationKind kind = FactionRelationKind.Hostile;
					string reason = "GoodwillChangedReason_AttackedCaravan".Translate();
					faction.TrySetRelationKind(ofPlayer, kind, true, reason, new GlobalTargetInfo?(t));
					Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(this.caravan, new IntVec3(100, 1, 100), WorldObjectDefOf.AttackedNonPlayerCaravan);
					IntVec3 playerSpot;
					IntVec3 enemySpot;
					MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerSpot, out enemySpot);
					CaravanEnterMapUtility.Enter(this.caravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(playerSpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, true);
					List<Pawn> list = this.metCaravan.PawnsListForReading.ToList<Pawn>();
					CaravanEnterMapUtility.Enter(this.metCaravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(enemySpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, false);
					LordMaker.MakeNewLord(this.faction, new LordJob_DefendAttackedTraderCaravan(list[0].Position), map, list);
					Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					CameraJumper.TryJumpAndSelect(t);
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(list, "LetterRelatedPawnsGroupGeneric".Translate(new object[]
					{
						Faction.OfPlayer.def.pawnsPlural
					}), LetterDefOf.NeutralEvent, true, true);
				}, "GeneratingMapForNewEncounter", false, null);
			}

			internal void <>m__2()
			{
				this.$this.RemoveAllPawnsAndPassToWorld(this.metCaravan);
			}

			internal void <>m__3()
			{
				Pawn t = this.caravan.PawnsListForReading[0];
				Faction faction = this.faction;
				Faction ofPlayer = Faction.OfPlayer;
				FactionRelationKind kind = FactionRelationKind.Hostile;
				string reason = "GoodwillChangedReason_AttackedCaravan".Translate();
				faction.TrySetRelationKind(ofPlayer, kind, true, reason, new GlobalTargetInfo?(t));
				Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(this.caravan, new IntVec3(100, 1, 100), WorldObjectDefOf.AttackedNonPlayerCaravan);
				IntVec3 playerSpot;
				IntVec3 enemySpot;
				MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerSpot, out enemySpot);
				CaravanEnterMapUtility.Enter(this.caravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(playerSpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, true);
				List<Pawn> list = this.metCaravan.PawnsListForReading.ToList<Pawn>();
				CaravanEnterMapUtility.Enter(this.metCaravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(enemySpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, false);
				LordMaker.MakeNewLord(this.faction, new LordJob_DefendAttackedTraderCaravan(list[0].Position), map, list);
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				CameraJumper.TryJumpAndSelect(t);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(list, "LetterRelatedPawnsGroupGeneric".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), LetterDefOf.NeutralEvent, true, true);
			}

			private sealed class <TryExecuteWorker>c__AnonStorey1
			{
				internal IntVec3 playerSpot;

				internal Map map;

				internal IntVec3 enemySpot;

				internal IncidentWorker_CaravanMeeting.<TryExecuteWorker>c__AnonStorey0 <>f__ref$0;

				public <TryExecuteWorker>c__AnonStorey1()
				{
				}

				internal IntVec3 <>m__0(Pawn p)
				{
					return CellFinder.RandomClosewalkCellNear(this.playerSpot, this.map, 12, null);
				}

				internal IntVec3 <>m__1(Pawn p)
				{
					return CellFinder.RandomClosewalkCellNear(this.enemySpot, this.map, 12, null);
				}
			}
		}
	}
}
