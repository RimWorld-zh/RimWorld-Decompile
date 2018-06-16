using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200034A RID: 842
	public class IncidentWorker_TraderCaravanArrival : IncidentWorker_NeutralGroup
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0007AEAC File Offset: 0x000792AC
		protected override PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Trader;
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0007AEC8 File Offset: 0x000792C8
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.def.caravanTraderKinds.Any<TraderKindDef>();
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0007AF00 File Offset: 0x00079300
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool result;
			if (!base.TryResolveParms(parms))
			{
				result = false;
			}
			else if (parms.faction.HostileTo(Faction.OfPlayer))
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
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].needs != null && list[i].needs.food != null)
						{
							list[i].needs.food.CurLevel = list[i].needs.food.MaxLevel;
						}
					}
					TraderKindDef traderKindDef = null;
					for (int j = 0; j < list.Count; j++)
					{
						Pawn pawn = list[j];
						if (pawn.TraderKind != null)
						{
							traderKindDef = pawn.TraderKind;
							break;
						}
					}
					string label = "LetterLabelTraderCaravanArrival".Translate(new object[]
					{
						parms.faction.Name,
						traderKindDef.label
					}).CapitalizeFirst();
					string text = "LetterTraderCaravanArrival".Translate(new object[]
					{
						parms.faction.Name,
						traderKindDef.label
					}).CapitalizeFirst();
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text, "LetterRelatedPawnsNeutralGroup".Translate(new object[]
					{
						Faction.OfPlayer.def.pawnsPlural
					}), true, true);
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, list[0], parms.faction, null);
					IntVec3 chillSpot;
					RCellFinder.TryFindRandomSpotJustOutsideColony(list[0], out chillSpot);
					LordJob_TradeWithColony lordJob = new LordJob_TradeWithColony(parms.faction, chillSpot);
					LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0007B0FC File Offset: 0x000794FC
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			parms.points = TraderCaravanUtility.GenerateGuardPoints();
		}
	}
}
