using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005EA RID: 1514
	[StaticConstructorOnStartup]
	public static class CaravanVisitUtility
	{
		// Token: 0x06001DEE RID: 7662 RVA: 0x00101988 File Offset: 0x000FFD88
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

		// Token: 0x06001DEF RID: 7663 RVA: 0x00101A24 File Offset: 0x000FFE24
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

		// Token: 0x040011B3 RID: 4531
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Trade", true);
	}
}
