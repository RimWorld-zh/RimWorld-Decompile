using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E6 RID: 1510
	[StaticConstructorOnStartup]
	public static class CaravanVisitUtility
	{
		// Token: 0x06001DE7 RID: 7655 RVA: 0x00101A54 File Offset: 0x000FFE54
		public static Settlement SettlementVisitedNow(Caravan caravan)
		{
			Settlement result;
			if (!caravan.Spawned || caravan.pather.Moving)
			{
				result = null;
			}
			else
			{
				List<Settlement> settlements = Find.WorldObjects.Settlements;
				for (int i = 0; i < settlements.Count; i++)
				{
					Settlement settlement = settlements[i];
					if (settlement.Tile == caravan.Tile && settlement.Faction != caravan.Faction && settlement.Visitable)
					{
						return settlement;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x00101AF0 File Offset: 0x000FFEF0
		public static Command TradeCommand(Caravan caravan)
		{
			Pawn bestNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandTrade".Translate();
			command_Action.defaultDesc = "CommandTradeDesc".Translate();
			command_Action.icon = CaravanVisitUtility.TradeCommandTex;
			command_Action.action = delegate()
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(caravan);
				if (settlement != null && settlement.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(bestNegotiator, settlement, false));
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(settlement.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithSettlement".Translate(new object[]
					{
						Faction.OfPlayer.def.pawnsPlural
					}), LetterDefOf.NeutralEvent, false, true);
				}
			};
			if (bestNegotiator == null)
			{
				command_Action.Disable("CommandTradeFailNoNegotiator".Translate());
			}
			if (bestNegotiator != null && bestNegotiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				command_Action.Disable("CommandTradeFailSocialDisabled".Translate());
			}
			return command_Action;
		}

		// Token: 0x040011B0 RID: 4528
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Trade", true);
	}
}
