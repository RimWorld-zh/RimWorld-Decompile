using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class CaravanVisitUtility
	{
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Trade", true);

		public static SettlementBase SettlementVisitedNow(Caravan caravan)
		{
			SettlementBase result;
			if (!caravan.Spawned || caravan.pather.Moving)
			{
				result = null;
			}
			else
			{
				List<SettlementBase> settlementBases = Find.WorldObjects.SettlementBases;
				for (int i = 0; i < settlementBases.Count; i++)
				{
					SettlementBase settlementBase = settlementBases[i];
					if (settlementBase.Tile == caravan.Tile && settlementBase.Faction != caravan.Faction && settlementBase.Visitable)
					{
						return settlementBase;
					}
				}
				result = null;
			}
			return result;
		}

		public static Command TradeCommand(Caravan caravan)
		{
			Pawn bestNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandTrade".Translate();
			command_Action.defaultDesc = "CommandTradeDesc".Translate();
			command_Action.icon = CaravanVisitUtility.TradeCommandTex;
			command_Action.action = delegate()
			{
				SettlementBase settlementBase = CaravanVisitUtility.SettlementVisitedNow(caravan);
				if (settlementBase != null && settlementBase.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(bestNegotiator, settlementBase, false));
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(settlementBase.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithSettlement".Translate(new object[]
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

		// Note: this type is marked as 'beforefieldinit'.
		static CaravanVisitUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <TradeCommand>c__AnonStorey0
		{
			internal Caravan caravan;

			internal Pawn bestNegotiator;

			public <TradeCommand>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				SettlementBase settlementBase = CaravanVisitUtility.SettlementVisitedNow(this.caravan);
				if (settlementBase != null && settlementBase.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(this.bestNegotiator, settlementBase, false));
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(settlementBase.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithSettlement".Translate(new object[]
					{
						Faction.OfPlayer.def.pawnsPlural
					}), LetterDefOf.NeutralEvent, false, true);
				}
			}
		}
	}
}
