using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class FactionDialogMaker
	{
		private static DiaNode root;

		private static Pawn negotiator;

		private static Faction faction;

		private const float MinRelationsToCommunicate = -70f;

		private const float MinRelationsFriendly = 40f;

		private const int GiftSilverAmount = 300;

		private const float GiftSilverGoodwillChange = 12f;

		private const float MilitaryAidRelsChange = -25f;

		private const int TradeRequestCost_Wary = 1100;

		private const int TradeRequestCost_Warm = 700;

		[CompilerGenerated]
		private static Func<IAttackTarget, bool> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<IAttackTarget, bool> _003C_003Ef__mg_0024cache1;

		public static DiaNode FactionDialogFor(Pawn negotiator, Faction faction)
		{
			Map map = negotiator.Map;
			FactionDialogMaker.negotiator = negotiator;
			FactionDialogMaker.faction = faction;
			string text = (faction.leader != null) ? faction.leader.Name.ToStringFull : faction.Name;
			if (faction.PlayerGoodwill < -70.0)
			{
				FactionDialogMaker.root = new DiaNode("FactionGreetingHostile".Translate(text));
			}
			else if (faction.PlayerGoodwill < 40.0)
			{
				string text2 = "FactionGreetingWary".Translate(text, negotiator.LabelShort);
				text2 = text2.AdjustedFor(negotiator);
				FactionDialogMaker.root = new DiaNode(text2);
				if (!SettlementUtility.IsPlayerAttackingAnySettlementOf(faction))
				{
					FactionDialogMaker.root.options.Add(FactionDialogMaker.OfferGiftOption(negotiator.Map));
				}
				if (!faction.HostileTo(Faction.OfPlayer) && negotiator.Spawned && negotiator.Map.IsPlayerHome)
				{
					FactionDialogMaker.root.options.Add(FactionDialogMaker.RequestTraderOption(map, 1100));
				}
			}
			else
			{
				FactionDialogMaker.root = new DiaNode("FactionGreetingWarm".Translate(text, negotiator.LabelShort));
				if (!SettlementUtility.IsPlayerAttackingAnySettlementOf(faction))
				{
					FactionDialogMaker.root.options.Add(FactionDialogMaker.OfferGiftOption(negotiator.Map));
				}
				if (!faction.HostileTo(Faction.OfPlayer) && negotiator.Spawned && negotiator.Map.IsPlayerHome)
				{
					FactionDialogMaker.root.options.Add(FactionDialogMaker.RequestTraderOption(map, 700));
					FactionDialogMaker.root.options.Add(FactionDialogMaker.RequestMilitaryAidOption(map));
				}
			}
			if (Prefs.DevMode)
			{
				foreach (DiaOption item in FactionDialogMaker.DebugOptions())
				{
					FactionDialogMaker.root.options.Add(item);
				}
			}
			DiaOption diaOption = new DiaOption("(" + "Disconnect".Translate() + ")");
			diaOption.resolveTree = true;
			FactionDialogMaker.root.options.Add(diaOption);
			return FactionDialogMaker.root;
		}

		private static IEnumerable<DiaOption> DebugOptions()
		{
			yield return new DiaOption("(Debug) Goodwill +10")
			{
				action = delegate
				{
					FactionDialogMaker.faction.AffectGoodwillWith(Faction.OfPlayer, 10f);
				},
				linkLateBind = (() => FactionDialogMaker.FactionDialogFor(FactionDialogMaker.negotiator, FactionDialogMaker.faction))
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private static int AmountSendableSilver(Map map)
		{
			return (from t in TradeUtility.AllLaunchableThings(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount);
		}

		private static DiaOption OfferGiftOption(Map map)
		{
			if (FactionDialogMaker.AmountSendableSilver(map) < 300)
			{
				DiaOption diaOption = new DiaOption("OfferGift".Translate());
				diaOption.Disable("NeedSilverLaunchable".Translate(300));
				return diaOption;
			}
			float goodwillDelta = (float)(12.0 * FactionDialogMaker.negotiator.GetStatValue(StatDefOf.DiplomacyPower, true));
			DiaOption diaOption2 = new DiaOption("OfferGift".Translate() + " (" + "SilverForGoodwill".Translate(300, goodwillDelta.ToString("#####0")) + ")");
			diaOption2.action = delegate
			{
				TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, 300, map, null);
				FactionDialogMaker.faction.AffectGoodwillWith(Faction.OfPlayer, goodwillDelta);
			};
			string text = "SilverGiftSent".Translate(FactionDialogMaker.faction.leader.LabelIndefinite(), Mathf.RoundToInt(goodwillDelta)).CapitalizeFirst();
			DiaNode diaNode = new DiaNode(text);
			diaNode.options.Add(FactionDialogMaker.OKToRoot());
			diaOption2.link = diaNode;
			return diaOption2;
		}

		private static DiaOption RequestTraderOption(Map map, int silverCost)
		{
			string text = "RequestTrader".Translate(silverCost.ToString());
			if (FactionDialogMaker.AmountSendableSilver(map) < silverCost)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("NeedSilverLaunchable".Translate(silverCost));
				return diaOption;
			}
			if (!FactionDialogMaker.faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("BadTemperature".Translate());
				return diaOption2;
			}
			int num = FactionDialogMaker.faction.lastTraderRequestTick + 240000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption3 = new DiaOption(text);
				diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod(true, false, true)));
				return diaOption3;
			}
			DiaOption diaOption4 = new DiaOption(text);
			DiaNode diaNode = new DiaNode("TraderSent".Translate(FactionDialogMaker.faction.leader.LabelIndefinite()).CapitalizeFirst());
			diaNode.options.Add(FactionDialogMaker.OKToRoot());
			DiaNode diaNode2 = new DiaNode("ChooseTraderKind".Translate(FactionDialogMaker.faction.leader.LabelIndefinite()));
			foreach (TraderKindDef caravanTraderKind in FactionDialogMaker.faction.def.caravanTraderKinds)
			{
				TraderKindDef localTk = caravanTraderKind;
				DiaOption diaOption5 = new DiaOption(localTk.LabelCap);
				diaOption5.action = delegate
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = map;
					incidentParms.faction = FactionDialogMaker.faction;
					incidentParms.traderKind = localTk;
					incidentParms.forced = true;
					Find.Storyteller.incidentQueue.Add(IncidentDefOf.TraderCaravanArrival, Find.TickManager.TicksGame + 120000, incidentParms);
					FactionDialogMaker.faction.lastTraderRequestTick = Find.TickManager.TicksGame;
					TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, silverCost, map, null);
				};
				diaOption5.link = diaNode;
				diaNode2.options.Add(diaOption5);
			}
			DiaOption diaOption6 = new DiaOption("GoBack".Translate());
			diaOption6.linkLateBind = FactionDialogMaker.ResetToRoot();
			diaNode2.options.Add(diaOption6);
			diaOption4.link = diaNode2;
			return diaOption4;
		}

		private static DiaOption RequestMilitaryAidOption(Map map)
		{
			string text = "RequestMilitaryAid".Translate(-25f);
			if (!FactionDialogMaker.faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("BadTemperature".Translate());
				return diaOption;
			}
			DiaOption diaOption2 = new DiaOption(text);
			if (map.attackTargetsCache.TargetsHostileToColony.Any(GenHostility.IsActiveThreatToPlayer) && !map.attackTargetsCache.TargetsHostileToColony.Any((IAttackTarget p) => ((Thing)p).Faction != null && ((Thing)p).Faction.HostileTo(FactionDialogMaker.faction)))
			{
				IEnumerable<Faction> source = (from pa in map.attackTargetsCache.TargetsHostileToColony.Where(GenHostility.IsActiveThreatToPlayer)
				select ((Thing)pa).Faction into fa
				where fa != null && !fa.HostileTo(FactionDialogMaker.faction)
				select fa).Distinct();
				DiaNode diaNode = new DiaNode("MilitaryAidConfirmMutualEnemy".Translate(FactionDialogMaker.faction.Name, GenText.ToCommaList(from fa in source
				select fa.Name, true)));
				DiaOption diaOption3 = new DiaOption("CallConfirm".Translate());
				diaOption3.action = delegate
				{
					FactionDialogMaker.CallForAid(map);
				};
				diaOption3.link = FactionDialogMaker.FightersSent();
				DiaOption diaOption4 = new DiaOption("CallCancel".Translate());
				diaOption4.linkLateBind = FactionDialogMaker.ResetToRoot();
				diaNode.options.Add(diaOption3);
				diaNode.options.Add(diaOption4);
				diaOption2.link = diaNode;
			}
			else
			{
				diaOption2.action = delegate
				{
					FactionDialogMaker.CallForAid(map);
				};
				diaOption2.link = FactionDialogMaker.FightersSent();
			}
			return diaOption2;
		}

		private static DiaNode FightersSent()
		{
			DiaNode diaNode = new DiaNode("MilitaryAidSent".Translate(FactionDialogMaker.faction.leader.LabelIndefinite()).CapitalizeFirst());
			diaNode.options.Add(FactionDialogMaker.OKToRoot());
			return diaNode;
		}

		private static void CallForAid(Map map)
		{
			FactionDialogMaker.faction.AffectGoodwillWith(Faction.OfPlayer, -25f);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = FactionDialogMaker.faction;
			incidentParms.points = (float)Rand.Range(150, 400);
			IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
		}

		private static DiaOption OKToRoot()
		{
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.linkLateBind = FactionDialogMaker.ResetToRoot();
			return diaOption;
		}

		private static Func<DiaNode> ResetToRoot()
		{
			return () => FactionDialogMaker.FactionDialogFor(FactionDialogMaker.negotiator, FactionDialogMaker.faction);
		}
	}
}
