using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_VisitorGroup : IncidentWorker_NeutralGroup
	{
		private const float TraderChance = 0.75f;

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!base.TryResolveParms(parms))
			{
				return false;
			}
			List<Pawn> list = base.SpawnPawns(parms);
			if (list.Count == 0)
			{
				return false;
			}
			IntVec3 chillSpot = default(IntVec3);
			RCellFinder.TryFindRandomSpotJustOutsideColony(list[0], out chillSpot);
			LordJob_VisitColony lordJob = new LordJob_VisitColony(parms.faction, chillSpot);
			LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
			bool flag = false;
			if (Rand.Value < 0.75)
			{
				flag = this.TryConvertOnePawnToSmallTrader(list, parms.faction, map);
			}
			Pawn pawn = list.Find((Pawn x) => parms.faction.leader == x);
			string label;
			string text3;
			if (list.Count == 1)
			{
				string text = (!flag) ? string.Empty : "SingleVisitorArrivesTraderInfo".Translate();
				string text2 = (pawn == null) ? string.Empty : "SingleVisitorArrivesLeaderInfo".Translate();
				label = "LetterLabelSingleVisitorArrives".Translate();
				text3 = "SingleVisitorArrives".Translate(list[0].story.Title.ToLower(), parms.faction.Name, list[0].Name, text, text2);
				text3 = text3.AdjustedFor(list[0]);
			}
			else
			{
				string text4 = (!flag) ? string.Empty : "GroupVisitorsArriveTraderInfo".Translate();
				string text5 = (pawn == null) ? string.Empty : "GroupVisitorsArriveLeaderInfo".Translate(pawn.LabelShort);
				label = "LetterLabelGroupVisitorsArrive".Translate();
				text3 = "GroupVisitorsArrive".Translate(parms.faction.Name, text4, text5);
			}
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text3, "LetterRelatedPawnsNeutralGroup".Translate(), true, true);
			Find.LetterStack.ReceiveLetter(label, text3, LetterDefOf.NeutralEvent, list[0], null);
			return true;
		}

		private bool TryConvertOnePawnToSmallTrader(List<Pawn> pawns, Faction faction, Map map)
		{
			if (faction.def.visitorTraderKinds.NullOrEmpty())
			{
				return false;
			}
			Pawn pawn = pawns.RandomElement();
			Lord lord = pawn.GetLord();
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			TraderKindDef traderKindDef = faction.def.visitorTraderKinds.RandomElementByWeight((TraderKindDef traderDef) => traderDef.commonality);
			pawn.trader.traderKind = traderKindDef;
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			ItemCollectionGeneratorParams parms = default(ItemCollectionGeneratorParams);
			parms.traderDef = traderKindDef;
			parms.tile = map.Tile;
			parms.traderFaction = faction;
			foreach (Thing item in ItemCollectionGeneratorDefOf.TraderStock.Worker.Generate(parms))
			{
				Pawn pawn2 = item as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.Faction != pawn.Faction)
					{
						pawn2.SetFaction(pawn.Faction, null);
					}
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(pawn.Position, map, 5, null);
					GenSpawn.Spawn(pawn2, loc, map);
					lord.AddPawn(pawn2);
				}
				else if (!pawn.inventory.innerContainer.TryAdd(item, true))
				{
					item.Destroy(DestroyMode.Vanish);
				}
			}
			PawnInventoryGenerator.GiveRandomFood(pawn);
			return true;
		}
	}
}
