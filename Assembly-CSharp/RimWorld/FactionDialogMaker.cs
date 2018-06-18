using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200055A RID: 1370
	public static class FactionDialogMaker
	{
		// Token: 0x060019AF RID: 6575 RVA: 0x000DECEC File Offset: 0x000DD0EC
		public static DiaNode FactionDialogFor(Pawn negotiator, Faction faction)
		{
			Map map = negotiator.Map;
			Pawn p;
			string text;
			if (faction.leader != null)
			{
				p = faction.leader;
				text = faction.leader.Name.ToStringFull;
			}
			else
			{
				Log.Error("Faction " + faction + " has no leader.", false);
				p = negotiator;
				text = faction.Name;
			}
			DiaNode diaNode;
			if (faction.PlayerRelationKind == FactionRelationKind.Hostile)
			{
				string key;
				if (!faction.def.permanentEnemy && "FactionGreetingHostileAppreciative".CanTranslate())
				{
					key = "FactionGreetingHostileAppreciative";
				}
				else
				{
					key = "FactionGreetingHostile";
				}
				diaNode = new DiaNode(key.Translate(new object[]
				{
					text
				}).AdjustedFor(p));
			}
			else if (faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				diaNode = new DiaNode("FactionGreetingWary".Translate(new object[]
				{
					text,
					negotiator.LabelShort
				}).AdjustedFor(p));
			}
			else
			{
				diaNode = new DiaNode("FactionGreetingWarm".Translate(new object[]
				{
					text,
					negotiator.LabelShort
				}).AdjustedFor(p));
			}
			if (map != null && map.IsPlayerHome)
			{
				diaNode.options.Add(FactionDialogMaker.RequestTraderOption(map, faction, negotiator));
				diaNode.options.Add(FactionDialogMaker.RequestMilitaryAidOption(map, faction, negotiator));
				if (DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any((ResearchProjectDef rp) => rp.HasTag(ResearchProjectTagDefOf.ShipRelated) && rp.IsFinished))
				{
					diaNode.options.Add(FactionDialogMaker.RequestAICoreQuest(map, faction, negotiator));
				}
			}
			if (Prefs.DevMode)
			{
				foreach (DiaOption item in FactionDialogMaker.DebugOptions(faction, negotiator))
				{
					diaNode.options.Add(item);
				}
			}
			DiaOption diaOption = new DiaOption("(" + "Disconnect".Translate() + ")");
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			return diaNode;
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x000DEF34 File Offset: 0x000DD334
		private static IEnumerable<DiaOption> DebugOptions(Faction faction, Pawn negotiator)
		{
			yield return new DiaOption("(Debug) Goodwill +10")
			{
				action = delegate()
				{
					faction.TryAffectGoodwillWith(Faction.OfPlayer, 10, false, true, null, null);
				},
				linkLateBind = (() => FactionDialogMaker.FactionDialogFor(negotiator, faction))
			};
			yield return new DiaOption("(Debug) Goodwill -10")
			{
				action = delegate()
				{
					faction.TryAffectGoodwillWith(Faction.OfPlayer, -10, false, true, null, null);
				},
				linkLateBind = (() => FactionDialogMaker.FactionDialogFor(negotiator, faction))
			};
			yield break;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x000DEF68 File Offset: 0x000DD368
		private static int AmountSendableSilver(Map map)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount);
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x000DEFC8 File Offset: 0x000DD3C8
		private static DiaOption RequestAICoreQuest(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestAICoreInformation".Translate(new object[]
			{
				ThingDefOf.AIPersonaCore.label,
				1500.ToString()
			});
			DiaOption result;
			if (faction.PlayerGoodwill < 40)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("NeedGoodwill".Translate(new object[]
				{
					40.ToString("F0")
				}));
				result = diaOption;
			}
			else
			{
				IncidentDef def = IncidentDefOf.Quest_ItemStashAICore;
				bool flag = PlayerItemAccessibilityUtility.ItemStashHas(ThingDefOf.AIPersonaCore);
				IncidentParms coreIncidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, Find.World);
				coreIncidentParms.faction = faction;
				bool flag2 = def.Worker.CanFireNow(coreIncidentParms);
				if (flag || !flag2)
				{
					DiaOption diaOption2 = new DiaOption(text);
					diaOption2.Disable("NoKnownAICore".Translate(new object[]
					{
						1500
					}));
					result = diaOption2;
				}
				else if (FactionDialogMaker.AmountSendableSilver(map) < 1500)
				{
					DiaOption diaOption3 = new DiaOption(text);
					diaOption3.Disable("NeedSilverLaunchable".Translate(new object[]
					{
						1500
					}));
					result = diaOption3;
				}
				else
				{
					DiaOption diaOption4 = new DiaOption(text);
					diaOption4.action = delegate()
					{
						if (def.Worker.TryExecute(coreIncidentParms))
						{
							TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, 1500, map, null);
						}
						Current.Game.GetComponent<GameComponent_OnetimeNotification>().sendAICoreRequestReminder = false;
					};
					string text2 = "RequestAICoreInformationResult".Translate(new object[]
					{
						faction.leader.LabelIndefinite()
					}).CapitalizeFirst();
					diaOption4.link = new DiaNode(text2)
					{
						options = 
						{
							FactionDialogMaker.OKToRoot(faction, negotiator)
						}
					};
					result = diaOption4;
				}
			}
			return result;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x000DF1AC File Offset: 0x000DD5AC
		private static DiaOption RequestTraderOption(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestTrader".Translate(new object[]
			{
				15
			});
			DiaOption result;
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("MustBeAlly".Translate());
				result = diaOption;
			}
			else if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("BadTemperature".Translate());
				result = diaOption2;
			}
			else
			{
				int num = faction.lastTraderRequestTick + 240000 - Find.TickManager.TicksGame;
				if (num > 0)
				{
					DiaOption diaOption3 = new DiaOption(text);
					diaOption3.Disable("WaitTime".Translate(new object[]
					{
						num.ToStringTicksToPeriod()
					}));
					result = diaOption3;
				}
				else
				{
					DiaOption diaOption4 = new DiaOption(text);
					DiaNode diaNode = new DiaNode("TraderSent".Translate(new object[]
					{
						faction.leader.LabelIndefinite()
					}).CapitalizeFirst());
					diaNode.options.Add(FactionDialogMaker.OKToRoot(faction, negotiator));
					DiaNode diaNode2 = new DiaNode("ChooseTraderKind".Translate(new object[]
					{
						faction.leader.LabelIndefinite()
					}));
					foreach (TraderKindDef localTk2 in from x in faction.def.caravanTraderKinds
					where x.requestable
					select x)
					{
						TraderKindDef localTk = localTk2;
						DiaOption diaOption5 = new DiaOption(localTk.LabelCap);
						diaOption5.action = delegate()
						{
							IncidentParms incidentParms = new IncidentParms();
							incidentParms.target = map;
							incidentParms.faction = faction;
							incidentParms.traderKind = localTk;
							incidentParms.forced = true;
							Find.Storyteller.incidentQueue.Add(IncidentDefOf.TraderCaravanArrival, Find.TickManager.TicksGame + 120000, incidentParms);
							faction.lastTraderRequestTick = Find.TickManager.TicksGame;
							Faction faction2 = faction;
							Faction ofPlayer = Faction.OfPlayer;
							int goodwillChange = -15;
							bool canSendMessage = false;
							string reason = "GoodwillChangedReason_RequestedTrader".Translate();
							faction2.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, true, reason, null);
						};
						diaOption5.link = diaNode;
						diaNode2.options.Add(diaOption5);
					}
					DiaOption diaOption6 = new DiaOption("GoBack".Translate());
					diaOption6.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
					diaNode2.options.Add(diaOption6);
					diaOption4.link = diaNode2;
					result = diaOption4;
				}
			}
			return result;
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000DF458 File Offset: 0x000DD858
		private static DiaOption RequestMilitaryAidOption(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestMilitaryAid".Translate(new object[]
			{
				20
			});
			DiaOption result;
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("MustBeAlly".Translate());
				result = diaOption;
			}
			else if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("BadTemperature".Translate());
				result = diaOption2;
			}
			else
			{
				DiaOption diaOption3 = new DiaOption(text);
				if (faction.def.techLevel < TechLevel.Industrial)
				{
					diaOption3.link = FactionDialogMaker.CantMakeItInTime(faction, negotiator);
				}
				else
				{
					IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
					where GenHostility.IsActiveThreatToPlayer(x)
					select ((Thing)x).Faction into x
					where x != null && !x.HostileTo(faction)
					select x).Distinct<Faction>();
					if (source.Any<Faction>())
					{
						string key = "MilitaryAidConfirmMutualEnemy";
						object[] array = new object[2];
						array[0] = faction.Name;
						array[1] = (from fa in source
						select fa.Name).ToCommaList(true);
						DiaNode diaNode = new DiaNode(key.Translate(array));
						DiaOption diaOption4 = new DiaOption("CallConfirm".Translate());
						diaOption4.action = delegate()
						{
							FactionDialogMaker.CallForAid(map, faction);
						};
						diaOption4.link = FactionDialogMaker.FightersSent(faction, negotiator);
						DiaOption diaOption5 = new DiaOption("CallCancel".Translate());
						diaOption5.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
						diaNode.options.Add(diaOption4);
						diaNode.options.Add(diaOption5);
						diaOption3.link = diaNode;
					}
					else
					{
						diaOption3.action = delegate()
						{
							FactionDialogMaker.CallForAid(map, faction);
						};
						diaOption3.link = FactionDialogMaker.FightersSent(faction, negotiator);
					}
				}
				result = diaOption3;
			}
			return result;
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x000DF6DC File Offset: 0x000DDADC
		private static DiaNode CantMakeItInTime(Faction faction, Pawn negotiator)
		{
			return new DiaNode("CantSendMilitaryAidInTime".Translate(new object[]
			{
				faction.leader.LabelIndefinite()
			}).CapitalizeFirst())
			{
				options = 
				{
					FactionDialogMaker.OKToRoot(faction, negotiator)
				}
			};
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000DF730 File Offset: 0x000DDB30
		private static DiaNode FightersSent(Faction faction, Pawn negotiator)
		{
			return new DiaNode("MilitaryAidSent".Translate(new object[]
			{
				faction.leader.LabelIndefinite()
			}).CapitalizeFirst())
			{
				options = 
				{
					FactionDialogMaker.OKToRoot(faction, negotiator)
				}
			};
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x000DF784 File Offset: 0x000DDB84
		private static void CallForAid(Map map, Faction faction)
		{
			Faction ofPlayer = Faction.OfPlayer;
			int goodwillChange = -20;
			bool canSendMessage = false;
			string reason = "GoodwillChangedReason_RequestedMilitaryAid".Translate();
			faction.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, true, reason, null);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = faction;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = (float)Rand.Range(150, 400);
			IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x000DF808 File Offset: 0x000DDC08
		private static DiaOption OKToRoot(Faction faction, Pawn negotiator)
		{
			return new DiaOption("OK".Translate())
			{
				linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator)
			};
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x000DF83C File Offset: 0x000DDC3C
		private static Func<DiaNode> ResetToRoot(Faction faction, Pawn negotiator)
		{
			return () => FactionDialogMaker.FactionDialogFor(negotiator, faction);
		}
	}
}
