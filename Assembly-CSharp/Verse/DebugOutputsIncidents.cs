using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E18 RID: 3608
	[HasDebugOutput]
	internal static class DebugOutputsIncidents
	{
		// Token: 0x060052E9 RID: 21225 RVA: 0x002A7A00 File Offset: 0x002A5E00
		[DebugOutput]
		[Category("Incidents")]
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

		// Token: 0x060052EA RID: 21226 RVA: 0x002A7A6C File Offset: 0x002A5E6C
		[DebugOutput]
		[Category("Incidents")]
		public static void TradeRequestsSampled()
		{
			Map currentMap = Find.CurrentMap;
			IncidentWorker_QuestTradeRequest incidentWorker_QuestTradeRequest = (IncidentWorker_QuestTradeRequest)IncidentDefOf.Quest_TradeRequest.Worker;
			Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
			for (int i = 0; i < 100; i++)
			{
				Settlement settlement = IncidentWorker_QuestTradeRequest.RandomNearbyTradeableSettlement(currentMap.Tile);
				if (settlement == null)
				{
					break;
				}
				TradeRequestComp component = settlement.GetComponent<TradeRequestComp>();
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
				settlement.GetComponent<TradeRequestComp>().Disable();
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

		// Token: 0x060052EB RID: 21227 RVA: 0x002A7BC7 File Offset: 0x002A5FC7
		[DebugOutput]
		[Category("Incidents")]
		[ModeRestrictionPlay]
		public static void FutureIncidents()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(false);
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x002A7BD0 File Offset: 0x002A5FD0
		[DebugOutput]
		[Category("Incidents")]
		[ModeRestrictionPlay]
		public static void FutureIncidentsCurrentMap()
		{
			StorytellerUtility.ShowFutureIncidentsDebugLogFloatMenu(true);
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x002A7BD9 File Offset: 0x002A5FD9
		[DebugOutput]
		[Category("Incidents")]
		[ModeRestrictionPlay]
		public static void IncidentTargetsList()
		{
			StorytellerUtility.DebugLogTestIncidentTargets();
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x002A7BE4 File Offset: 0x002A5FE4
		[DebugOutput]
		[Category("Incidents")]
		public static void TradeRequests()
		{
			Map currentMap = Find.CurrentMap;
			IncidentWorker_QuestTradeRequest incidentWorker_QuestTradeRequest = (IncidentWorker_QuestTradeRequest)IncidentDefOf.Quest_TradeRequest.Worker;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Randomly-generated trade requests for map " + currentMap.ToString() + ":");
			stringBuilder.AppendLine();
			for (int i = 0; i < 50; i++)
			{
				Settlement settlement = IncidentWorker_QuestTradeRequest.RandomNearbyTradeableSettlement(currentMap.Tile);
				if (settlement == null)
				{
					break;
				}
				stringBuilder.AppendLine("Settlement: " + settlement.LabelCap);
				TradeRequestComp component = settlement.GetComponent<TradeRequestComp>();
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
				settlement.GetComponent<TradeRequestComp>().Disable();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x002A7DB8 File Offset: 0x002A61B8
		[DebugOutput]
		[Category("Incidents")]
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

		// Token: 0x060052F0 RID: 21232 RVA: 0x002A7EB4 File Offset: 0x002A62B4
		[DebugOutput]
		[Category("Incidents")]
		public static void TraderStockMarketValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TraderKindDef traderKindDef in DefDatabase<TraderKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(traderKindDef.defName + " : " + ((ThingSetMaker_TraderStock)ThingSetMakerDefOf.TraderStock.root).AverageTotalStockValue(traderKindDef).ToString("F0"));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x002A7F54 File Offset: 0x002A6354
		[DebugOutput]
		[Category("Incidents")]
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

		// Token: 0x060052F2 RID: 21234 RVA: 0x002A7FFC File Offset: 0x002A63FC
		[DebugOutput]
		[Category("Incidents")]
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

		// Token: 0x060052F3 RID: 21235 RVA: 0x002A8080 File Offset: 0x002A6480
		[DebugOutput]
		[Category("Incidents")]
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

		// Token: 0x060052F4 RID: 21236 RVA: 0x002A8188 File Offset: 0x002A6588
		[DebugOutput]
		[Category("Incidents")]
		public static void RaidFactionSampled()
		{
			((IncidentWorker_Raid)IncidentDefOf.RaidEnemy.Worker).DoTable_RaidFactionSampled();
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x002A81A0 File Offset: 0x002A65A0
		[DebugOutput]
		[Category("Incidents")]
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

		// Token: 0x060052F6 RID: 21238 RVA: 0x002A829C File Offset: 0x002A669C
		[DebugOutput]
		[Category("Incidents")]
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
	}
}
