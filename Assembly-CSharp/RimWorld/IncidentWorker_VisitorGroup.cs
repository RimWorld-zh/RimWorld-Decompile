using System;
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
			bool result;
			if (!base.TryResolveParms(parms))
			{
				result = false;
			}
			else
			{
				List<Pawn> list = base.SpawnPawns(parms);
				if (list.Count == 0)
				{
					result = false;
				}
				else
				{
					IntVec3 chillSpot = default(IntVec3);
					RCellFinder.TryFindRandomSpotJustOutsideColony(list[0], out chillSpot);
					LordJob_VisitColony lordJob = new LordJob_VisitColony(parms.faction, chillSpot);
					LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
					bool flag = false;
					if (Rand.Value < 0.75)
					{
						flag = this.TryConvertOnePawnToSmallTrader(list, parms.faction, map);
					}
					Pawn pawn = list.Find((Predicate<Pawn>)((Pawn x) => parms.faction.leader == x));
					string label;
					string text3;
					if (list.Count == 1)
					{
						string text = (!flag) ? "" : "SingleVisitorArrivesTraderInfo".Translate();
						string text2 = (pawn == null) ? "" : "SingleVisitorArrivesLeaderInfo".Translate();
						label = "LetterLabelSingleVisitorArrives".Translate();
						text3 = "SingleVisitorArrives".Translate(list[0].story.Title.ToLower(), parms.faction.Name, list[0].Name, text, text2);
						text3 = text3.AdjustedFor(list[0]);
					}
					else
					{
						string text4 = (!flag) ? "" : "GroupVisitorsArriveTraderInfo".Translate();
						string text5 = (pawn == null) ? "" : "GroupVisitorsArriveLeaderInfo".Translate(pawn.LabelShort);
						label = "LetterLabelGroupVisitorsArrive".Translate();
						text3 = "GroupVisitorsArrive".Translate(parms.faction.Name, text4, text5);
					}
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text3, "LetterRelatedPawnsNeutralGroup".Translate(), true, true);
					Find.LetterStack.ReceiveLetter(label, text3, LetterDefOf.NeutralEvent, (Thing)list[0], (string)null);
					result = true;
				}
			}
			return result;
		}

		private bool TryConvertOnePawnToSmallTrader(List<Pawn> pawns, Faction faction, Map map)
		{
			bool result;
			if (faction.def.visitorTraderKinds.NullOrEmpty())
			{
				result = false;
			}
			else
			{
				Pawn pawn = pawns.RandomElement();
				Lord lord = pawn.GetLord();
				pawn.mindState.wantsToTradeWithColony = true;
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
				List<TraderKindDef> visitorTraderKinds = faction.def.visitorTraderKinds;
				TraderKindDef traderDef2 = pawn.trader.traderKind = visitorTraderKinds.RandomElementByWeight((Func<TraderKindDef, float>)((TraderKindDef traderDef) => traderDef.commonality));
				pawn.inventory.DestroyAll(DestroyMode.Vanish);
				ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
				{
					traderDef = traderDef2,
					tile = new int?(map.Tile),
					traderFaction = faction
				};
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
				result = true;
			}
			return result;
		}
	}
}
