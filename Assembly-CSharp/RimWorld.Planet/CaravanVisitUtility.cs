using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class CaravanVisitUtility
	{
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Trade", true);

		public static Settlement SettlementVisitedNow(Caravan caravan)
		{
			Settlement result;
			Settlement settlement;
			if (!caravan.Spawned || caravan.pather.Moving)
			{
				result = null;
			}
			else
			{
				List<Settlement> settlements = Find.WorldObjects.Settlements;
				for (int i = 0; i < settlements.Count; i++)
				{
					settlement = settlements[i];
					if (settlement.Tile == caravan.Tile && settlement.Faction != caravan.Faction && settlement.Visitable)
						goto IL_006b;
				}
				result = null;
			}
			goto IL_008b;
			IL_006b:
			result = settlement;
			goto IL_008b;
			IL_008b:
			return result;
		}

		public static Command TradeCommand(Caravan caravan)
		{
			Pawn bestNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandTrade".Translate();
			command_Action.defaultDesc = "CommandTradeDesc".Translate();
			command_Action.icon = CaravanVisitUtility.TradeCommandTex;
			command_Action.action = (Action)delegate()
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(caravan);
				if (settlement != null && settlement.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(bestNegotiator, settlement));
					string label = "";
					string text = "";
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(settlement.Goods.OfType<Pawn>(), ref label, ref text, "LetterRelatedPawnsTradingWithSettlement".Translate(), false, true);
					if (!text.NullOrEmpty())
					{
						Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, (WorldObject)settlement, (string)null);
					}
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

		public static Command FulfillRequestCommand(Caravan caravan)
		{
			Func<Thing, bool> validator = (Func<Thing, bool>)((Thing thing) => (byte)((thing.GetRotStage() == RotStage.Fresh) ? 1 : 0) != 0);
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandFulfillTradeOffer".Translate();
			command_Action.defaultDesc = "CommandFulfillTradeOfferDesc".Translate();
			command_Action.icon = CaravanVisitUtility.TradeCommandTex;
			command_Action.action = (Action)delegate()
			{
				Settlement settlement2 = CaravanVisitUtility.SettlementVisitedNow(caravan);
				CaravanRequestComp caravanRequest = (settlement2 == null) ? null : ((WorldObject)settlement2).GetComponent<CaravanRequestComp>();
				if (caravanRequest != null)
				{
					if (!caravanRequest.ActiveRequest)
					{
						Log.Error("Attempted to fulfill an unavailable request");
					}
					else if (!CaravanInventoryUtility.HasThings(caravan, caravanRequest.requestThingDef, caravanRequest.requestCount, validator))
					{
						Messages.Message("CommandFulfillTradeOfferFailInsufficient".Translate(GenLabel.ThingLabel(caravanRequest.requestThingDef, null, caravanRequest.requestCount)), MessageTypeDefOf.RejectInput);
					}
					else
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandFulfillTradeOfferConfirm".Translate(GenLabel.ThingLabel(caravanRequest.requestThingDef, null, caravanRequest.requestCount), caravanRequest.rewards[0].Label), (Action)delegate()
						{
							int remaining = caravanRequest.requestCount;
							List<Thing> list = CaravanInventoryUtility.TakeThings(caravan, (Func<Thing, int>)delegate(Thing thing)
							{
								int result;
								if (caravanRequest.requestThingDef != thing.def)
								{
									result = 0;
								}
								else
								{
									int num = Mathf.Min(remaining, thing.stackCount);
									remaining -= num;
									result = num;
								}
								return result;
							});
							for (int i = 0; i < list.Count; i++)
							{
								list[i].Destroy(DestroyMode.Vanish);
							}
							while (caravanRequest.rewards.Count > 0)
							{
								Thing thing2 = caravanRequest.rewards[caravanRequest.rewards.Count - 1];
								caravanRequest.rewards.Remove(thing2);
								CaravanInventoryUtility.GiveThing(caravan, thing2);
							}
							caravanRequest.Disable();
						}, false, (string)null));
					}
				}
			};
			Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(caravan);
			CaravanRequestComp caravanRequestComp = (settlement == null) ? null : ((WorldObject)settlement).GetComponent<CaravanRequestComp>();
			if (!CaravanInventoryUtility.HasThings(caravan, caravanRequestComp.requestThingDef, caravanRequestComp.requestCount, validator))
			{
				command_Action.Disable("CommandFulfillTradeOfferFailInsufficient".Translate(GenLabel.ThingLabel(caravanRequestComp.requestThingDef, null, caravanRequestComp.requestCount)));
			}
			return command_Action;
		}
	}
}
