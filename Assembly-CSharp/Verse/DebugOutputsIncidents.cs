using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	[HasDebugOutput]
	internal static class DebugOutputsIncidents
	{
		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thing, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<PawnGroupMaker> <>f__am$cache2;

		[CompilerGenerated]
		private static Action <>f__am$cache3;

		[CompilerGenerated]
		private static Action <>f__am$cache4;

		[Category("Incidents")]
		[DebugOutput]
		public static void MiscIncidentChances()
		{
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp_CategoryMTB storytellerComp_CategoryMTB = storytellerComps[i] as StorytellerComp_CategoryMTB;
				if (storytellerComp_CategoryMTB != null && ((StorytellerCompProperties_CategoryMTB)storytellerComp_CategoryMTB.props).category == IncidentCategoryDefOf.Misc)
				{
					storytellerComp_CategoryMTB.DebugTablesIncidentChances(IncidentCategoryDefOf.Misc);
				}
			}
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void TradeRequestsSampled()
		{
			Map currentMap = Find.CurrentMap;
			IncidentWorker_QuestTradeRequest incidentWorker_QuestTradeRequest = (IncidentWorker_QuestTradeRequest)IncidentDefOf.Quest_TradeRequest.Worker;
			Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
			for (int i = 0; i < 100; i++)
			{
				SettlementBase settlementBase = IncidentWorker_QuestTradeRequest.RandomNearbyTradeableSettlement(currentMap.Tile);
				if (settlementBase == null)
				{
					break;
				}
				TradeRequestComp component = settlementBase.GetComponent<TradeRequestComp>();
				if (incidentWorker_QuestTradeRequest.TryGenerateTradeRequest(component, currentMap))
				{
					if (!counts.ContainsKey(component.requestThingDef))
					{
						counts.Add(component.requestThingDef, 0);
					}
					Dictionary<ThingDef, int> counts2;
					ThingDef requestThingDef;
					(counts2 = counts)[requestThingDef = component.requestThingDef] = counts2[requestThingDef] + 1;
				}
				settlementBase.GetComponent<TradeRequestComp>().Disable();
			}
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where counts.ContainsKey(d)
			orderby counts[d] descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[2];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("appearance rate in " + 100 + " trade requests", (ThingDef d) => ((float)counts[d] / 100f).ToStringPercent());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[Category("Incidents")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void FutureIncidents()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(false);
		}

		[Category("Incidents")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void FutureIncidentsCurrentMap()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(true);
		}

		[Category("Incidents")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void IncidentTargetsList()
		{
			StorytellerUtility.DebugLogTestIncidentTargets();
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void TradeRequests()
		{
			Map currentMap = Find.CurrentMap;
			IncidentWorker_QuestTradeRequest incidentWorker_QuestTradeRequest = (IncidentWorker_QuestTradeRequest)IncidentDefOf.Quest_TradeRequest.Worker;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Randomly-generated trade requests for map " + currentMap.ToString() + ":");
			stringBuilder.AppendLine();
			for (int i = 0; i < 50; i++)
			{
				SettlementBase settlementBase = IncidentWorker_QuestTradeRequest.RandomNearbyTradeableSettlement(currentMap.Tile);
				if (settlementBase == null)
				{
					break;
				}
				stringBuilder.AppendLine("Settlement: " + settlementBase.LabelCap);
				TradeRequestComp component = settlementBase.GetComponent<TradeRequestComp>();
				if (incidentWorker_QuestTradeRequest.TryGenerateTradeRequest(component, currentMap))
				{
					stringBuilder.AppendLine("Duration: " + (component.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F1"));
					string str = GenLabel.ThingLabel(component.requestThingDef, null, component.requestCount) + " ($" + (component.requestThingDef.BaseMarketValue * (float)component.requestCount).ToString("F0") + ")";
					stringBuilder.AppendLine("Request: " + str);
					string str2 = GenThing.ThingsToCommaList(component.rewards, false, true, -1) + " ($" + (from t in component.rewards
					select t.MarketValue * (float)t.stackCount).Sum().ToString("F0") + ")";
					stringBuilder.AppendLine("Reward: " + str2);
				}
				else
				{
					stringBuilder.AppendLine("TryGenerateTradeRequest failed.");
				}
				stringBuilder.AppendLine();
				settlementBase.GetComponent<TradeRequestComp>().Disable();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void PawnArrivalCandidates()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(IncidentDefOf.RaidEnemy.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidEnemy.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.RaidFriendly.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidFriendly.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.VisitorGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.VisitorGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TravelerGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TravelerGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TraderCaravanArrival.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TraderCaravanArrival.Worker).DebugListingOfGroupSources());
			Log.Message(stringBuilder.ToString(), false);
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void TraderStockMarketValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TraderKindDef traderKindDef in DefDatabase<TraderKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(traderKindDef.defName + " : " + ((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).AverageTotalStockValue(traderKindDef).ToString("F0"));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void TraderStockGeneration()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TraderKindDef localDef2 in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localDef = localDef2;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate()
				{
					Log.Message(((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).GenerationDataFor(localDef), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void TraderStockGeneratorsDefs()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Requires visible map.", false);
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				Action<StockGenerator> action = delegate(StockGenerator gen)
				{
					sb.AppendLine(gen.GetType().ToString());
					sb.AppendLine("ALLOWED DEFS:");
					foreach (ThingDef thingDef in from d in DefDatabase<ThingDef>.AllDefs
					where gen.HandlesThingDef(d)
					select d)
					{
						sb.AppendLine(string.Concat(new object[]
						{
							thingDef.defName,
							" [",
							thingDef.BaseMarketValue,
							"]"
						}));
					}
					sb.AppendLine();
					sb.AppendLine("GENERATION TEST:");
					gen.countRange = IntRange.one;
					for (int i = 0; i < 30; i++)
					{
						foreach (Thing thing in gen.GenerateThings(Find.CurrentMap.Tile))
						{
							sb.AppendLine(string.Concat(new object[]
							{
								thing.Label,
								" [",
								thing.MarketValue,
								"]"
							}));
						}
					}
					sb.AppendLine("---------------------------------------------------------");
				};
				action(new StockGenerator_Armor());
				action(new StockGenerator_WeaponsRanged());
				action(new StockGenerator_Clothes());
				action(new StockGenerator_Art());
				Log.Message(sb.ToString(), false);
			}
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void PawnGroupGenSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.def.pawnGroupMakers != null)
				{
					if (faction.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat))
					{
						Faction localFac = faction;
						list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							foreach (float num in Dialog_DebugActionsMenu.PointsOptions(true))
							{
								float localP2 = num;
								float localP = localP2;
								float maxPawnCost = PawnGroupMakerUtility.MaxPawnCost(localFac, localP, null, PawnGroupKindDefOf.Combat);
								string defName = (from op in localFac.def.pawnGroupMakers.SelectMany((PawnGroupMaker gm) => gm.options)
								where op.Cost <= maxPawnCost
								select op).MaxBy((PawnGenOption op) => op.Cost).kind.defName;
								string label = string.Concat(new string[]
								{
									localP.ToString(),
									", max ",
									maxPawnCost.ToString("F0"),
									" ",
									defName
								});
								list2.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate()
								{
									Dictionary<ThingDef, int>[] weaponsCount = new Dictionary<ThingDef, int>[20];
									string[] pawnKinds = new string[20];
									for (int i = 0; i < 20; i++)
									{
										weaponsCount[i] = new Dictionary<ThingDef, int>();
										List<Pawn> list3 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
										{
											groupKind = PawnGroupKindDefOf.Combat,
											tile = Find.CurrentMap.Tile,
											points = localP,
											faction = localFac
										}, false).ToList<Pawn>();
										pawnKinds[i] = PawnUtility.PawnKindsToCommaList(list3, true);
										foreach (Pawn pawn in list3)
										{
											if (pawn.equipment.Primary != null)
											{
												if (!weaponsCount[i].ContainsKey(pawn.equipment.Primary.def))
												{
													weaponsCount[i].Add(pawn.equipment.Primary.def, 0);
												}
												Dictionary<ThingDef, int> dictionary;
												ThingDef def;
												(dictionary = weaponsCount[i])[def = pawn.equipment.Primary.def] = dictionary[def] + 1;
											}
											pawn.Destroy(DestroyMode.Vanish);
										}
									}
									int totalPawns = weaponsCount.Sum((Dictionary<ThingDef, int> x) => x.Sum((KeyValuePair<ThingDef, int> y) => y.Value));
									List<TableDataGetter<int>> list4 = new List<TableDataGetter<int>>();
									list4.Add(new TableDataGetter<int>("", (int x) => (x != 20) ? (x + 1).ToString() : "avg"));
									list4.Add(new TableDataGetter<int>("pawns", delegate(int x)
									{
										string str = " ";
										string str2;
										if (x == 20)
										{
											str2 = ((float)totalPawns / 20f).ToString("0.#");
										}
										else
										{
											str2 = weaponsCount[x].Sum((KeyValuePair<ThingDef, int> y) => y.Value).ToString();
										}
										return str + str2;
									}));
									list4.Add(new TableDataGetter<int>("kinds", (int x) => (x != 20) ? pawnKinds[x] : ""));
									list4.AddRange(from x in DefDatabase<ThingDef>.AllDefs
									where x.IsWeapon && !x.weaponTags.NullOrEmpty<string>() && weaponsCount.Any((Dictionary<ThingDef, int> wc) => wc.ContainsKey(x))
									orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
									select new TableDataGetter<int>(x.label.Shorten(), delegate(int y)
									{
										string result;
										if (y == 20)
										{
											result = " " + ((float)weaponsCount.Sum((Dictionary<ThingDef, int> z) => (!z.ContainsKey(x)) ? 0 : z[x]) / 20f).ToString("0.#");
										}
										else
										{
											string text;
											if (weaponsCount[y].ContainsKey(x))
											{
												object[] array = new object[5];
												array[0] = " ";
												array[1] = weaponsCount[y][x];
												array[2] = " (";
												array[3] = ((float)weaponsCount[y][x] / (float)weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0");
												array[4] = ")";
												text = string.Concat(array);
											}
											else
											{
												text = "";
											}
											result = text;
										}
										return result;
									}));
									DebugTables.MakeTablesDialog<int>(Enumerable.Range(0, 21), list4.ToArray());
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void RaidFactionSampled()
		{
			((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidFactionSampled();
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void RaidStrategySampled()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Choose factions randomly like a real raid", delegate()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction f = enumerator.Current;
					Faction f2 = f;
					list.Add(new FloatMenuOption(f2.Name + " (" + f2.def.defName + ")", delegate()
					{
						((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(f);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[Category("Incidents")]
		[DebugOutput]
		public static void RaidArrivemodeSampled()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Choose factions randomly like a real raid", delegate()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction f = enumerator.Current;
					Faction f2 = f;
					list.Add(new FloatMenuOption(f2.Name + " (" + f2.def.defName + ")", delegate()
					{
						((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(f);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[CompilerGenerated]
		private static string <TradeRequestsSampled>m__0(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static float <TradeRequests>m__1(Thing t)
		{
			return t.MarketValue * (float)t.stackCount;
		}

		[CompilerGenerated]
		private static bool <PawnGroupGenSampled>m__2(PawnGroupMaker x)
		{
			return x.kindDef == PawnGroupKindDefOf.Combat;
		}

		[CompilerGenerated]
		private static void <RaidStrategySampled>m__3()
		{
			((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(null);
		}

		[CompilerGenerated]
		private static void <RaidArrivemodeSampled>m__4()
		{
			((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(null);
		}

		[CompilerGenerated]
		private sealed class <TradeRequestsSampled>c__AnonStorey0
		{
			internal Dictionary<ThingDef, int> counts;

			public <TradeRequestsSampled>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingDef d)
			{
				return this.counts.ContainsKey(d);
			}

			internal int <>m__1(ThingDef d)
			{
				return this.counts[d];
			}

			internal string <>m__2(ThingDef d)
			{
				return ((float)this.counts[d] / 100f).ToStringPercent();
			}
		}

		[CompilerGenerated]
		private sealed class <TraderStockGeneration>c__AnonStorey1
		{
			internal TraderKindDef localDef;

			public <TraderStockGeneration>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Log.Message(((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).GenerationDataFor(this.localDef), false);
			}
		}

		[CompilerGenerated]
		private sealed class <TraderStockGeneratorsDefs>c__AnonStorey2
		{
			internal StringBuilder sb;

			public <TraderStockGeneratorsDefs>c__AnonStorey2()
			{
			}

			internal void <>m__0(StockGenerator gen)
			{
				this.sb.AppendLine(gen.GetType().ToString());
				this.sb.AppendLine("ALLOWED DEFS:");
				foreach (ThingDef thingDef in from d in DefDatabase<ThingDef>.AllDefs
				where gen.HandlesThingDef(d)
				select d)
				{
					this.sb.AppendLine(string.Concat(new object[]
					{
						thingDef.defName,
						" [",
						thingDef.BaseMarketValue,
						"]"
					}));
				}
				this.sb.AppendLine();
				this.sb.AppendLine("GENERATION TEST:");
				gen.countRange = IntRange.one;
				for (int i = 0; i < 30; i++)
				{
					foreach (Thing thing in gen.GenerateThings(Find.CurrentMap.Tile))
					{
						this.sb.AppendLine(string.Concat(new object[]
						{
							thing.Label,
							" [",
							thing.MarketValue,
							"]"
						}));
					}
				}
				this.sb.AppendLine("---------------------------------------------------------");
			}

			private sealed class <TraderStockGeneratorsDefs>c__AnonStorey3
			{
				internal StockGenerator gen;

				internal DebugOutputsIncidents.<TraderStockGeneratorsDefs>c__AnonStorey2 <>f__ref$2;

				public <TraderStockGeneratorsDefs>c__AnonStorey3()
				{
				}

				internal bool <>m__0(ThingDef d)
				{
					return this.gen.HandlesThingDef(d);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PawnGroupGenSampled>c__AnonStorey4
		{
			internal Faction localFac;

			private static Func<PawnGroupMaker, IEnumerable<PawnGenOption>> <>f__am$cache0;

			private static Func<PawnGenOption, float> <>f__am$cache1;

			public <PawnGroupGenSampled>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (float num in Dialog_DebugActionsMenu.PointsOptions(true))
				{
					float localP2 = num;
					float localP = localP2;
					float maxPawnCost = PawnGroupMakerUtility.MaxPawnCost(this.localFac, localP, null, PawnGroupKindDefOf.Combat);
					string defName = (from op in this.localFac.def.pawnGroupMakers.SelectMany((PawnGroupMaker gm) => gm.options)
					where op.Cost <= maxPawnCost
					select op).MaxBy((PawnGenOption op) => op.Cost).kind.defName;
					string label = string.Concat(new string[]
					{
						localP.ToString(),
						", max ",
						maxPawnCost.ToString("F0"),
						" ",
						defName
					});
					list.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate()
					{
						Dictionary<ThingDef, int>[] weaponsCount = new Dictionary<ThingDef, int>[20];
						string[] pawnKinds = new string[20];
						for (int i = 0; i < 20; i++)
						{
							weaponsCount[i] = new Dictionary<ThingDef, int>();
							List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
							{
								groupKind = PawnGroupKindDefOf.Combat,
								tile = Find.CurrentMap.Tile,
								points = localP,
								faction = this.localFac
							}, false).ToList<Pawn>();
							pawnKinds[i] = PawnUtility.PawnKindsToCommaList(list2, true);
							foreach (Pawn pawn in list2)
							{
								if (pawn.equipment.Primary != null)
								{
									if (!weaponsCount[i].ContainsKey(pawn.equipment.Primary.def))
									{
										weaponsCount[i].Add(pawn.equipment.Primary.def, 0);
									}
									Dictionary<ThingDef, int> dictionary;
									ThingDef def;
									(dictionary = weaponsCount[i])[def = pawn.equipment.Primary.def] = dictionary[def] + 1;
								}
								pawn.Destroy(DestroyMode.Vanish);
							}
						}
						int totalPawns = weaponsCount.Sum((Dictionary<ThingDef, int> x) => x.Sum((KeyValuePair<ThingDef, int> y) => y.Value));
						List<TableDataGetter<int>> list3 = new List<TableDataGetter<int>>();
						list3.Add(new TableDataGetter<int>("", (int x) => (x != 20) ? (x + 1).ToString() : "avg"));
						list3.Add(new TableDataGetter<int>("pawns", delegate(int x)
						{
							string str = " ";
							string str2;
							if (x == 20)
							{
								str2 = ((float)totalPawns / 20f).ToString("0.#");
							}
							else
							{
								str2 = weaponsCount[x].Sum((KeyValuePair<ThingDef, int> y) => y.Value).ToString();
							}
							return str + str2;
						}));
						list3.Add(new TableDataGetter<int>("kinds", (int x) => (x != 20) ? pawnKinds[x] : ""));
						list3.AddRange(from x in DefDatabase<ThingDef>.AllDefs
						where x.IsWeapon && !x.weaponTags.NullOrEmpty<string>() && weaponsCount.Any((Dictionary<ThingDef, int> wc) => wc.ContainsKey(x))
						orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
						select new TableDataGetter<int>(x.label.Shorten(), delegate(int y)
						{
							string result;
							if (y == 20)
							{
								result = " " + ((float)weaponsCount.Sum((Dictionary<ThingDef, int> z) => (!z.ContainsKey(x)) ? 0 : z[x]) / 20f).ToString("0.#");
							}
							else
							{
								string text;
								if (weaponsCount[y].ContainsKey(x))
								{
									object[] array = new object[5];
									array[0] = " ";
									array[1] = weaponsCount[y][x];
									array[2] = " (";
									array[3] = ((float)weaponsCount[y][x] / (float)weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0");
									array[4] = ")";
									text = string.Concat(array);
								}
								else
								{
									text = "";
								}
								result = text;
							}
							return result;
						}));
						DebugTables.MakeTablesDialog<int>(Enumerable.Range(0, 21), list3.ToArray());
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			}

			private static IEnumerable<PawnGenOption> <>m__1(PawnGroupMaker gm)
			{
				return gm.options;
			}

			private static float <>m__2(PawnGenOption op)
			{
				return op.Cost;
			}

			private sealed class <PawnGroupGenSampled>c__AnonStorey5
			{
				internal float maxPawnCost;

				internal float localP;

				internal DebugOutputsIncidents.<PawnGroupGenSampled>c__AnonStorey4 <>f__ref$4;

				private static Func<Dictionary<ThingDef, int>, int> <>f__am$cache0;

				private static Func<int, string> <>f__am$cache1;

				private static Func<ThingDef, bool> <>f__am$cache2;

				private static Func<ThingDef, TechLevel> <>f__am$cache3;

				private static Func<ThingDef, float> <>f__am$cache4;

				private static Func<KeyValuePair<ThingDef, int>, int> <>f__am$cache5;

				public <PawnGroupGenSampled>c__AnonStorey5()
				{
				}

				internal bool <>m__0(PawnGenOption op)
				{
					return op.Cost <= this.maxPawnCost;
				}

				internal void <>m__1()
				{
					DebugOutputsIncidents.<PawnGroupGenSampled>c__AnonStorey4 <>f__ref$4 = this.<>f__ref$4;
					DebugOutputsIncidents.<PawnGroupGenSampled>c__AnonStorey4.<PawnGroupGenSampled>c__AnonStorey5 <>f__ref$5 = this;
					Dictionary<ThingDef, int>[] weaponsCount = new Dictionary<ThingDef, int>[20];
					string[] pawnKinds = new string[20];
					for (int i = 0; i < 20; i++)
					{
						weaponsCount[i] = new Dictionary<ThingDef, int>();
						List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
						{
							groupKind = PawnGroupKindDefOf.Combat,
							tile = Find.CurrentMap.Tile,
							points = this.localP,
							faction = this.<>f__ref$4.localFac
						}, false).ToList<Pawn>();
						pawnKinds[i] = PawnUtility.PawnKindsToCommaList(list, true);
						foreach (Pawn pawn in list)
						{
							if (pawn.equipment.Primary != null)
							{
								if (!weaponsCount[i].ContainsKey(pawn.equipment.Primary.def))
								{
									weaponsCount[i].Add(pawn.equipment.Primary.def, 0);
								}
								Dictionary<ThingDef, int> dictionary;
								ThingDef def;
								(dictionary = weaponsCount[i])[def = pawn.equipment.Primary.def] = dictionary[def] + 1;
							}
							pawn.Destroy(DestroyMode.Vanish);
						}
					}
					int totalPawns = weaponsCount.Sum((Dictionary<ThingDef, int> x) => x.Sum((KeyValuePair<ThingDef, int> y) => y.Value));
					List<TableDataGetter<int>> list2 = new List<TableDataGetter<int>>();
					list2.Add(new TableDataGetter<int>("", (int x) => (x != 20) ? (x + 1).ToString() : "avg"));
					list2.Add(new TableDataGetter<int>("pawns", delegate(int x)
					{
						string str = " ";
						string str2;
						if (x == 20)
						{
							str2 = ((float)totalPawns / 20f).ToString("0.#");
						}
						else
						{
							str2 = weaponsCount[x].Sum((KeyValuePair<ThingDef, int> y) => y.Value).ToString();
						}
						return str + str2;
					}));
					list2.Add(new TableDataGetter<int>("kinds", (int x) => (x != 20) ? pawnKinds[x] : ""));
					list2.AddRange(from x in DefDatabase<ThingDef>.AllDefs
					where x.IsWeapon && !x.weaponTags.NullOrEmpty<string>() && weaponsCount.Any((Dictionary<ThingDef, int> wc) => wc.ContainsKey(x))
					orderby x.IsMeleeWeapon descending, x.techLevel, x.BaseMarketValue
					select new TableDataGetter<int>(x.label.Shorten(), delegate(int y)
					{
						string result;
						if (y == 20)
						{
							result = " " + ((float)weaponsCount.Sum((Dictionary<ThingDef, int> z) => (!z.ContainsKey(x)) ? 0 : z[x]) / 20f).ToString("0.#");
						}
						else
						{
							string text;
							if (weaponsCount[y].ContainsKey(x))
							{
								object[] array = new object[5];
								array[0] = " ";
								array[1] = weaponsCount[y][x];
								array[2] = " (";
								array[3] = ((float)weaponsCount[y][x] / (float)weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0");
								array[4] = ")";
								text = string.Concat(array);
							}
							else
							{
								text = "";
							}
							result = text;
						}
						return result;
					}));
					DebugTables.MakeTablesDialog<int>(Enumerable.Range(0, 21), list2.ToArray());
				}

				private static int <>m__2(Dictionary<ThingDef, int> x)
				{
					return x.Sum((KeyValuePair<ThingDef, int> y) => y.Value);
				}

				private static string <>m__3(int x)
				{
					return (x != 20) ? (x + 1).ToString() : "avg";
				}

				private static bool <>m__4(ThingDef x)
				{
					return x.IsMeleeWeapon;
				}

				private static TechLevel <>m__5(ThingDef x)
				{
					return x.techLevel;
				}

				private static float <>m__6(ThingDef x)
				{
					return x.BaseMarketValue;
				}

				private static int <>m__7(KeyValuePair<ThingDef, int> y)
				{
					return y.Value;
				}

				private sealed class <PawnGroupGenSampled>c__AnonStorey6
				{
					internal int totalPawns;

					internal Dictionary<ThingDef, int>[] weaponsCount;

					internal string[] pawnKinds;

					internal DebugOutputsIncidents.<PawnGroupGenSampled>c__AnonStorey4 <>f__ref$4;

					internal DebugOutputsIncidents.<PawnGroupGenSampled>c__AnonStorey4.<PawnGroupGenSampled>c__AnonStorey5 <>f__ref$5;

					private static Func<KeyValuePair<ThingDef, int>, int> <>f__am$cache0;

					public <PawnGroupGenSampled>c__AnonStorey6()
					{
					}

					internal string <>m__0(int x)
					{
						string str = " ";
						string str2;
						if (x == 20)
						{
							str2 = ((float)this.totalPawns / 20f).ToString("0.#");
						}
						else
						{
							str2 = this.weaponsCount[x].Sum((KeyValuePair<ThingDef, int> y) => y.Value).ToString();
						}
						return str + str2;
					}

					internal string <>m__1(int x)
					{
						return (x != 20) ? this.pawnKinds[x] : "";
					}

					internal bool <>m__2(ThingDef x)
					{
						return x.IsWeapon && !x.weaponTags.NullOrEmpty<string>() && this.weaponsCount.Any((Dictionary<ThingDef, int> wc) => wc.ContainsKey(x));
					}

					internal TableDataGetter<int> <>m__3(ThingDef x)
					{
						return new TableDataGetter<int>(x.label.Shorten(), delegate(int y)
						{
							string result;
							if (y == 20)
							{
								result = " " + ((float)this.weaponsCount.Sum((Dictionary<ThingDef, int> z) => (!z.ContainsKey(x)) ? 0 : z[x]) / 20f).ToString("0.#");
							}
							else
							{
								string text;
								if (this.weaponsCount[y].ContainsKey(x))
								{
									object[] array = new object[5];
									array[0] = " ";
									array[1] = this.weaponsCount[y][x];
									array[2] = " (";
									array[3] = ((float)this.weaponsCount[y][x] / (float)this.weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0");
									array[4] = ")";
									text = string.Concat(array);
								}
								else
								{
									text = "";
								}
								result = text;
							}
							return result;
						});
					}

					private static int <>m__4(KeyValuePair<ThingDef, int> y)
					{
						return y.Value;
					}

					private sealed class <PawnGroupGenSampled>c__AnonStorey7
					{
						internal ThingDef x;

						internal DebugOutputsIncidents.<PawnGroupGenSampled>c__AnonStorey4.<PawnGroupGenSampled>c__AnonStorey5.<PawnGroupGenSampled>c__AnonStorey6 <>f__ref$6;

						public <PawnGroupGenSampled>c__AnonStorey7()
						{
						}

						internal bool <>m__0(Dictionary<ThingDef, int> wc)
						{
							return wc.ContainsKey(this.x);
						}
					}

					private sealed class <PawnGroupGenSampled>c__AnonStorey8
					{
						internal ThingDef x;

						internal DebugOutputsIncidents.<PawnGroupGenSampled>c__AnonStorey4.<PawnGroupGenSampled>c__AnonStorey5.<PawnGroupGenSampled>c__AnonStorey6 <>f__ref$6;

						private static Func<KeyValuePair<ThingDef, int>, int> <>f__am$cache0;

						public <PawnGroupGenSampled>c__AnonStorey8()
						{
						}

						internal string <>m__0(int y)
						{
							string result;
							if (y == 20)
							{
								result = " " + ((float)this.<>f__ref$6.weaponsCount.Sum((Dictionary<ThingDef, int> z) => (!z.ContainsKey(this.x)) ? 0 : z[this.x]) / 20f).ToString("0.#");
							}
							else
							{
								string text;
								if (this.<>f__ref$6.weaponsCount[y].ContainsKey(this.x))
								{
									object[] array = new object[5];
									array[0] = " ";
									array[1] = this.<>f__ref$6.weaponsCount[y][this.x];
									array[2] = " (";
									array[3] = ((float)this.<>f__ref$6.weaponsCount[y][this.x] / (float)this.<>f__ref$6.weaponsCount[y].Sum((KeyValuePair<ThingDef, int> z) => z.Value)).ToStringPercent("F0");
									array[4] = ")";
									text = string.Concat(array);
								}
								else
								{
									text = "";
								}
								result = text;
							}
							return result;
						}

						internal int <>m__1(Dictionary<ThingDef, int> z)
						{
							return (!z.ContainsKey(this.x)) ? 0 : z[this.x];
						}

						private static int <>m__2(KeyValuePair<ThingDef, int> z)
						{
							return z.Value;
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <RaidStrategySampled>c__AnonStorey9
		{
			internal Faction f;

			public <RaidStrategySampled>c__AnonStorey9()
			{
			}

			internal void <>m__0()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidStrategySampled(this.f);
			}
		}

		[CompilerGenerated]
		private sealed class <RaidArrivemodeSampled>c__AnonStoreyA
		{
			internal Faction f;

			public <RaidArrivemodeSampled>c__AnonStoreyA()
			{
			}

			internal void <>m__0()
			{
				((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidArrivalModeSampled(this.f);
			}
		}
	}
}
