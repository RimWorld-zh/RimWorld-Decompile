using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class FactionDialogMaker
	{
		[CompilerGenerated]
		private static Predicate<ResearchProjectDef> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Thing, int> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<TraderKindDef, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<IAttackTarget, bool> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<IAttackTarget, Faction> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<Faction, string> <>f__am$cache5;

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
				}).AdjustedFor(p, "PAWN"));
			}
			else if (faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				diaNode = new DiaNode("FactionGreetingWary".Translate(new object[]
				{
					text,
					negotiator.LabelShort
				}).AdjustedFor(p, "PAWN"));
			}
			else
			{
				diaNode = new DiaNode("FactionGreetingWarm".Translate(new object[]
				{
					text,
					negotiator.LabelShort
				}).AdjustedFor(p, "PAWN"));
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

		private static int AmountSendableSilver(Map map)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount);
		}

		private static DiaOption RequestAICoreQuest(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestAICoreInformation".Translate(new object[]
			{
				ThingDefOf.AIPersonaCore.label,
				1500.ToString()
			});
			if (faction.PlayerGoodwill < 40)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("NeedGoodwill".Translate(new object[]
				{
					40.ToString("F0")
				}));
				return diaOption;
			}
			IncidentDef def = IncidentDefOf.Quest_ItemStashAICore;
			bool flag = PlayerItemAccessibilityUtility.ItemStashHas(ThingDefOf.AIPersonaCore);
			IncidentParms coreIncidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, Find.World);
			coreIncidentParms.faction = faction;
			bool flag2 = def.Worker.CanFireNow(coreIncidentParms, false);
			if (flag || !flag2)
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("NoKnownAICore".Translate(new object[]
				{
					1500
				}));
				return diaOption2;
			}
			if (FactionDialogMaker.AmountSendableSilver(map) < 1500)
			{
				DiaOption diaOption3 = new DiaOption(text);
				diaOption3.Disable("NeedSilverLaunchable".Translate(new object[]
				{
					1500
				}));
				return diaOption3;
			}
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
			return diaOption4;
		}

		private static DiaOption RequestTraderOption(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestTrader".Translate(new object[]
			{
				15
			});
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("MustBeAlly".Translate());
				return diaOption;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("BadTemperature".Translate());
				return diaOption2;
			}
			int num = faction.lastTraderRequestTick + 240000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption3 = new DiaOption(text);
				diaOption3.Disable("WaitTime".Translate(new object[]
				{
					num.ToStringTicksToPeriod()
				}));
				return diaOption3;
			}
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
					Find.Storyteller.incidentQueue.Add(IncidentDefOf.TraderCaravanArrival, Find.TickManager.TicksGame + 120000, incidentParms, 240000);
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
			return diaOption4;
		}

		private static DiaOption RequestMilitaryAidOption(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestMilitaryAid".Translate(new object[]
			{
				25
			});
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("MustBeAlly".Translate());
				return diaOption;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("BadTemperature".Translate());
				return diaOption2;
			}
			int num = faction.lastMilitaryAidRequestTick + 60000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption3 = new DiaOption(text);
				diaOption3.Disable("WaitTime".Translate(new object[]
				{
					num.ToStringTicksToPeriod()
				}));
				return diaOption3;
			}
			if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, faction))
			{
				DiaOption diaOption4 = new DiaOption(text);
				diaOption4.Disable("HostileVisitorsPresent".Translate());
				return diaOption4;
			}
			DiaOption diaOption5 = new DiaOption(text);
			if (faction.def.techLevel < TechLevel.Industrial)
			{
				diaOption5.link = FactionDialogMaker.CantMakeItInTime(faction, negotiator);
			}
			else
			{
				IEnumerable<IAttackTarget> targetsHostileToColony = map.attackTargetsCache.TargetsHostileToColony;
				if (FactionDialogMaker.<>f__mg$cache0 == null)
				{
					FactionDialogMaker.<>f__mg$cache0 = new Func<IAttackTarget, bool>(GenHostility.IsActiveThreatToPlayer);
				}
				IEnumerable<Faction> source = (from x in targetsHostileToColony.Where(FactionDialogMaker.<>f__mg$cache0)
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
					DiaOption diaOption6 = new DiaOption("CallConfirm".Translate());
					diaOption6.action = delegate()
					{
						FactionDialogMaker.CallForAid(map, faction);
					};
					diaOption6.link = FactionDialogMaker.FightersSent(faction, negotiator);
					DiaOption diaOption7 = new DiaOption("CallCancel".Translate());
					diaOption7.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
					diaNode.options.Add(diaOption6);
					diaNode.options.Add(diaOption7);
					diaOption5.link = diaNode;
				}
				else
				{
					diaOption5.action = delegate()
					{
						FactionDialogMaker.CallForAid(map, faction);
					};
					diaOption5.link = FactionDialogMaker.FightersSent(faction, negotiator);
				}
			}
			return diaOption5;
		}

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

		private static void CallForAid(Map map, Faction faction)
		{
			Faction ofPlayer = Faction.OfPlayer;
			int goodwillChange = -25;
			bool canSendMessage = false;
			string reason = "GoodwillChangedReason_RequestedMilitaryAid".Translate();
			faction.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, true, reason, null);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = faction;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = DiplomacyTuning.RequestedMilitaryAidPointsRange.RandomInRange;
			faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
			IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
		}

		private static DiaOption OKToRoot(Faction faction, Pawn negotiator)
		{
			return new DiaOption("OK".Translate())
			{
				linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator)
			};
		}

		private static Func<DiaNode> ResetToRoot(Faction faction, Pawn negotiator)
		{
			return () => FactionDialogMaker.FactionDialogFor(negotiator, faction);
		}

		[CompilerGenerated]
		private static bool <FactionDialogFor>m__0(ResearchProjectDef rp)
		{
			return rp.HasTag(ResearchProjectTagDefOf.ShipRelated) && rp.IsFinished;
		}

		[CompilerGenerated]
		private static bool <AmountSendableSilver>m__1(Thing t)
		{
			return t.def == ThingDefOf.Silver;
		}

		[CompilerGenerated]
		private static int <AmountSendableSilver>m__2(Thing t)
		{
			return t.stackCount;
		}

		[CompilerGenerated]
		private static bool <RequestTraderOption>m__3(TraderKindDef x)
		{
			return x.requestable;
		}

		[CompilerGenerated]
		private static Faction <RequestMilitaryAidOption>m__4(IAttackTarget x)
		{
			return ((Thing)x).Faction;
		}

		[CompilerGenerated]
		private static string <RequestMilitaryAidOption>m__5(Faction fa)
		{
			return fa.Name;
		}

		[CompilerGenerated]
		private sealed class <DebugOptions>c__Iterator0 : IEnumerable, IEnumerable<DiaOption>, IEnumerator, IDisposable, IEnumerator<DiaOption>
		{
			internal DiaOption <opt>__1;

			internal Faction faction;

			internal Pawn negotiator;

			internal DiaOption <opt>__2;

			internal DiaOption $current;

			internal bool $disposing;

			internal int $PC;

			private FactionDialogMaker.<DebugOptions>c__Iterator0.<DebugOptions>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <DebugOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
				{
					DiaOption opt = new DiaOption("(Debug) Goodwill +10");
					opt.action = delegate()
					{
						faction.TryAffectGoodwillWith(Faction.OfPlayer, 10, false, true, null, null);
					};
					opt.linkLateBind = (() => FactionDialogMaker.FactionDialogFor(negotiator, faction));
					this.$current = opt;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
				{
					DiaOption opt2 = new DiaOption("(Debug) Goodwill -10");
					opt2.action = delegate()
					{
						<DebugOptions>c__AnonStorey.faction.TryAffectGoodwillWith(Faction.OfPlayer, -10, false, true, null, null);
					};
					opt2.linkLateBind = (() => FactionDialogMaker.FactionDialogFor(<DebugOptions>c__AnonStorey.negotiator, <DebugOptions>c__AnonStorey.faction));
					this.$current = opt2;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			DiaOption IEnumerator<DiaOption>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.DiaOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				FactionDialogMaker.<DebugOptions>c__Iterator0 <DebugOptions>c__Iterator = new FactionDialogMaker.<DebugOptions>c__Iterator0();
				<DebugOptions>c__Iterator.faction = faction;
				<DebugOptions>c__Iterator.negotiator = negotiator;
				return <DebugOptions>c__Iterator;
			}

			private sealed class <DebugOptions>c__AnonStorey1
			{
				internal Faction faction;

				internal Pawn negotiator;

				public <DebugOptions>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.faction.TryAffectGoodwillWith(Faction.OfPlayer, 10, false, true, null, null);
				}

				internal DiaNode <>m__1()
				{
					return FactionDialogMaker.FactionDialogFor(this.negotiator, this.faction);
				}

				internal void <>m__2()
				{
					this.faction.TryAffectGoodwillWith(Faction.OfPlayer, -10, false, true, null, null);
				}

				internal DiaNode <>m__3()
				{
					return FactionDialogMaker.FactionDialogFor(this.negotiator, this.faction);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <RequestAICoreQuest>c__AnonStorey2
		{
			internal IncidentDef def;

			internal IncidentParms coreIncidentParms;

			internal Map map;

			public <RequestAICoreQuest>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				if (this.def.Worker.TryExecute(this.coreIncidentParms))
				{
					TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, 1500, this.map, null);
				}
				Current.Game.GetComponent<GameComponent_OnetimeNotification>().sendAICoreRequestReminder = false;
			}
		}

		[CompilerGenerated]
		private sealed class <RequestTraderOption>c__AnonStorey3
		{
			internal Map map;

			internal Faction faction;

			public <RequestTraderOption>c__AnonStorey3()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <RequestTraderOption>c__AnonStorey4
		{
			internal TraderKindDef localTk;

			internal FactionDialogMaker.<RequestTraderOption>c__AnonStorey3 <>f__ref$3;

			public <RequestTraderOption>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = this.<>f__ref$3.map;
				incidentParms.faction = this.<>f__ref$3.faction;
				incidentParms.traderKind = this.localTk;
				incidentParms.forced = true;
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.TraderCaravanArrival, Find.TickManager.TicksGame + 120000, incidentParms, 240000);
				this.<>f__ref$3.faction.lastTraderRequestTick = Find.TickManager.TicksGame;
				Faction faction = this.<>f__ref$3.faction;
				Faction ofPlayer = Faction.OfPlayer;
				int goodwillChange = -15;
				bool canSendMessage = false;
				string reason = "GoodwillChangedReason_RequestedTrader".Translate();
				faction.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, true, reason, null);
			}
		}

		[CompilerGenerated]
		private sealed class <RequestMilitaryAidOption>c__AnonStorey5
		{
			internal Faction faction;

			internal Map map;

			public <RequestMilitaryAidOption>c__AnonStorey5()
			{
			}

			internal bool <>m__0(Faction x)
			{
				return x != null && !x.HostileTo(this.faction);
			}

			internal void <>m__1()
			{
				FactionDialogMaker.CallForAid(this.map, this.faction);
			}

			internal void <>m__2()
			{
				FactionDialogMaker.CallForAid(this.map, this.faction);
			}
		}

		[CompilerGenerated]
		private sealed class <ResetToRoot>c__AnonStorey6
		{
			internal Pawn negotiator;

			internal Faction faction;

			public <ResetToRoot>c__AnonStorey6()
			{
			}

			internal DiaNode <>m__0()
			{
				return FactionDialogMaker.FactionDialogFor(this.negotiator, this.faction);
			}
		}
	}
}
