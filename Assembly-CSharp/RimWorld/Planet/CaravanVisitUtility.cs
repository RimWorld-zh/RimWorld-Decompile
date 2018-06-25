using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E8 RID: 1512
	[StaticConstructorOnStartup]
	public static class CaravanVisitUtility
	{
		// Token: 0x040011B0 RID: 4528
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Trade", true);

		// Token: 0x06001DEB RID: 7659 RVA: 0x00101BA4 File Offset: 0x000FFFA4
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

		// Token: 0x06001DEC RID: 7660 RVA: 0x00101C40 File Offset: 0x00100040
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
	}
}
