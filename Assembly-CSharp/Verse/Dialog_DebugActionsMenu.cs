using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E28 RID: 3624
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x04003817 RID: 14359
		private static List<PawnKindDef> pawnKindsForDamageTypeBattleRoyale = null;

		// Token: 0x04003818 RID: 14360
		private static Map mapLeak = null;

		// Token: 0x06005519 RID: 21785 RVA: 0x002BBA91 File Offset: 0x002B9E91
		public Dialog_DebugActionsMenu()
		{
			this.forcePause = true;
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x0600551A RID: 21786 RVA: 0x002BBAA4 File Offset: 0x002B9EA4
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x002BBABC File Offset: 0x002B9EBC
		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (WorldRendererUtility.WorldRenderedNow)
				{
					this.DoListingItems_World();
				}
				else if (Find.CurrentMap != null)
				{
					this.DoListingItems_MapActions();
					this.DoListingItems_MapTools();
				}
				this.DoListingItems_AllModePlayActions();
			}
			else
			{
				this.DoListingItems_Entry();
			}
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x002BBB3C File Offset: 0x002B9F3C
		private void DoListingItems_Entry()
		{
			base.DoLabel("Translation tools");
			base.DebugAction("Write backstory translation file", delegate
			{
				LanguageDataWriter.WriteBackstoryFile();
			});
			base.DebugAction("Save translation report", delegate
			{
				LanguageReportGenerator.SaveTranslationReport();
			});
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x002BBBA8 File Offset: 0x002B9FA8
		private void DoListingItems_AllModePlayActions()
		{
			base.DoGap();
			base.DoLabel("Actions - Map management");
			base.DebugAction("Generate map", delegate
			{
				MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				mapParent.Tile = TileFinder.RandomStartingTile();
				mapParent.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(mapParent);
				GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
			});
			base.DebugAction("Destroy map", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Map map = maps[i];
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Current.Game.DeinitAndRemoveMap(map);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Leak map", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Map map = maps[i];
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Dialog_DebugActionsMenu.mapLeak = map;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Print leaked map", delegate
			{
				Log.Message(string.Format("Leaked map {0}", Dialog_DebugActionsMenu.mapLeak), false);
			});
			base.DebugToolMap("Transfer", delegate
			{
				List<Thing> toTransfer = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
				if (toTransfer.Any<Thing>())
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						Map map = maps[i];
						if (map != Find.CurrentMap)
						{
							list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
							{
								for (int j = 0; j < toTransfer.Count; j++)
								{
									IntVec3 center;
									if (CellFinder.TryFindRandomCellNear(map.Center, map, Mathf.Max(map.Size.x, map.Size.z), (IntVec3 x) => !x.Fogged(map) && x.Standable(map), out center, -1))
									{
										toTransfer[j].DeSpawn(DestroyMode.Vanish);
										GenPlace.TryPlaceThing(toTransfer[j], center, map, ThingPlaceMode.Near, null, null);
									}
									else
									{
										Log.Error("Could not find spawn cell.", false);
									}
								}
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
			});
			base.DebugAction("Change map", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Map map = maps[i];
					if (map != Find.CurrentMap)
					{
						list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
						{
							Current.Game.CurrentMap = map;
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Regenerate current map", delegate
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				int tile = Find.CurrentMap.Tile;
				MapParent parent = Find.CurrentMap.Parent;
				IntVec3 size = Find.CurrentMap.Size;
				Current.Game.DeinitAndRemoveMap(Find.CurrentMap);
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def);
				Current.Game.CurrentMap = orGenerateMap;
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
			});
			base.DebugAction("Generate map with caves", delegate
			{
				int tile = TileFinder.RandomFactionBaseTileFor(Faction.OfPlayer, false, (int x) => Find.World.HasCaves(x));
				if (Find.CurrentMap != null)
				{
					Find.WorldObjects.Remove(Find.CurrentMap.Parent);
				}
				MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				mapParent.Tile = tile;
				mapParent.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(mapParent);
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, null);
				Current.Game.CurrentMap = orGenerateMap;
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			});
			base.DebugAction("Run MapGenerator", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				using (List<MapGeneratorDef>.Enumerator enumerator = DefDatabase<MapGeneratorDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MapGeneratorDef mapgen = enumerator.Current;
						list.Add(new DebugMenuOption(mapgen.defName, DebugMenuOptionMode.Action, delegate()
						{
							MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
							mapParent.Tile = (from tile in Enumerable.Range(0, Find.WorldGrid.TilesCount)
							where Find.WorldGrid[tile].biome.canBuildBase
							select tile).RandomElement<int>();
							mapParent.SetFaction(Faction.OfPlayer);
							Find.WorldObjects.Add(mapParent);
							Map currentMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, mapgen, null, null);
							Current.Game.CurrentMap = currentMap;
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Force reform in current map", delegate
			{
				if (Find.CurrentMap != null)
				{
					TimedForcedExit.ForceReform(Find.CurrentMap.Parent);
				}
			});
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x002BBD60 File Offset: 0x002BA160
		private void DoListingItems_MapActions()
		{
			Text.Font = GameFont.Tiny;
			base.DoLabel("Incidents");
			if (Find.CurrentMap != null)
			{
				this.DoIncidentDebugAction(Find.CurrentMap);
				this.DoIncidentWithPointsAction(Find.CurrentMap);
			}
			this.DoIncidentDebugAction(Find.World);
			base.DebugAction("Execute raid with points...", delegate
			{
				this.Close(true);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (float localP2 in Dialog_DebugActionsMenu.PointsOptions(true))
				{
					float localP = localP2;
					list.Add(new FloatMenuOption(localP.ToString() + " points", delegate()
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = localP;
						IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			});
			base.DebugAction("Execute raid with specifics...", delegate
			{
				StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain);
				IncidentParms parms = storytellerComp.GenerateParms(IncidentCategoryDefOf.ThreatBig, Find.CurrentMap);
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (Faction localFac2 in Find.FactionManager.AllFactions)
				{
					Faction localFac = localFac2;
					list.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate()
					{
						parms.faction = localFac;
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float num in Dialog_DebugActionsMenu.PointsOptions(true))
						{
							float localPoints = num;
							list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
							{
								parms.points = localPoints;
								List<DebugMenuOption> list3 = new List<DebugMenuOption>();
								foreach (RaidStrategyDef localStrat2 in DefDatabase<RaidStrategyDef>.AllDefs)
								{
									RaidStrategyDef localStrat = localStrat2;
									string text = localStrat.defName;
									if (!localStrat.Worker.CanUseWith(parms, PawnGroupKindDefOf.Combat))
									{
										text += " [NO]";
									}
									list3.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
									{
										parms.raidStrategy = localStrat;
										List<DebugMenuOption> list4 = new List<DebugMenuOption>();
										list4.Add(new DebugMenuOption("-Random-", DebugMenuOptionMode.Action, delegate()
										{
											this.DoRaid(parms);
										}));
										foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
										{
											PawnsArrivalModeDef localArrival = localArrival2;
											string text2 = localArrival.defName;
											if (!localArrival.Worker.CanUseWith(parms) || !localStrat.arriveModes.Contains(localArrival))
											{
												text2 += " [NO]";
											}
											list4.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Action, delegate()
											{
												parms.raidArrivalMode = localArrival;
												this.DoRaid(parms);
											}));
										}
										Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DoGap();
			base.DoLabel("Actions - Misc");
			base.DebugAction("Destroy all plants", delegate
			{
				foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
				{
					if (thing is Plant)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			});
			base.DebugAction("Destroy all things", delegate
			{
				foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			});
			base.DebugAction("Finish all research", delegate
			{
				Find.ResearchManager.DebugSetAllProjectsFinished();
				Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
			});
			base.DebugAction("Replace all trade ships", delegate
			{
				Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
				for (int i = 0; i < 5; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
				}
			});
			base.DebugAction("Change weather...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (WeatherDef localWeather2 in DefDatabase<WeatherDef>.AllDefs)
				{
					WeatherDef localWeather = localWeather2;
					list.Add(new DebugMenuOption(localWeather.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						Find.CurrentMap.weatherManager.TransitionTo(localWeather);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Play song...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (SongDef localSong2 in DefDatabase<SongDef>.AllDefs)
				{
					SongDef localSong = localSong2;
					list.Add(new DebugMenuOption(localSong.defName, DebugMenuOptionMode.Action, delegate()
					{
						Find.MusicManagerPlay.ForceStartSong(localSong, false);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Play sound...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (SoundDef localSd2 in from s in DefDatabase<SoundDef>.AllDefs
				where !s.sustain
				select s)
				{
					SoundDef localSd = localSd2;
					list.Add(new DebugMenuOption(localSd.defName, DebugMenuOptionMode.Action, delegate()
					{
						localSd.PlayOneShotOnCamera(null);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			if (Find.CurrentMap.gameConditionManager.ActiveConditions.Count > 0)
			{
				base.DebugAction("End game condition ...", delegate
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (GameCondition localMc2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
					{
						GameCondition localMc = localMc2;
						list.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, delegate()
						{
							localMc.End();
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				});
			}
			base.DebugAction("Add prisoner", delegate
			{
				this.AddGuest(true);
			});
			base.DebugAction("Add guest", delegate
			{
				this.AddGuest(false);
			});
			base.DebugAction("Force enemy assault", delegate
			{
				foreach (Lord lord in Find.CurrentMap.lordManager.lords)
				{
					LordToil_Stage lordToil_Stage = lord.CurLordToil as LordToil_Stage;
					if (lordToil_Stage != null)
					{
						foreach (Transition transition in lord.Graph.transitions)
						{
							if (transition.sources.Contains(lordToil_Stage) && transition.target is LordToil_AssaultColony)
							{
								Messages.Message("Debug forcing to assault toil: " + lord.faction, MessageTypeDefOf.TaskCompletion, false);
								lord.GotoToil(transition.target);
								return;
							}
						}
					}
				}
			});
			base.DebugAction("Force enemy flee", delegate
			{
				foreach (Lord lord in Find.CurrentMap.lordManager.lords)
				{
					if (lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer) && lord.faction.def.autoFlee)
					{
						LordToil lordToil = lord.Graph.lordToils.FirstOrDefault((LordToil st) => st is LordToil_PanicFlee);
						if (lordToil != null)
						{
							lord.GotoToil(lordToil);
						}
					}
				}
			});
			base.DebugAction("Unload unused assets", delegate
			{
				MemoryUtility.UnloadUnusedUnityAssets();
			});
			base.DebugAction("Name colony...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				list.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFaction());
				}));
				if (Find.CurrentMap != null && Find.CurrentMap.IsPlayerHome)
				{
					FactionBase factionBase = (FactionBase)Find.CurrentMap.Parent;
					list.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, delegate()
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionBase(factionBase));
					}));
					list.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, delegate()
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(factionBase));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Next lesson", delegate
			{
				LessonAutoActivator.DebugForceInitiateBestLessonNow();
			});
			base.DebugAction("Regen all map mesh sections", delegate
			{
				Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
			});
			base.DebugAction("Change camera config...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (Type localType2 in typeof(CameraMapConfig).AllSubclasses())
				{
					Type localType = localType2;
					string text = localType.Name;
					if (text.StartsWith("CameraMapConfig_"))
					{
						text = text.Substring("CameraMapConfig_".Length);
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
					{
						Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Force ship countdown", delegate
			{
				ShipCountdown.InitiateCountdown(null);
			});
			base.DebugAction("Flash trade drop spot", delegate
			{
				IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
				Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
				Log.Message("trade drop spot: " + intVec, false);
			});
			base.DebugAction("Kill faction leader", delegate
			{
				Pawn leader = (from x in Find.FactionManager.AllFactions
				where x.leader != null
				select x).RandomElement<Faction>().leader;
				int num = 0;
				while (!leader.Dead)
				{
					if (++num > 1000)
					{
						Log.Warning("Could not kill faction leader.", false);
						break;
					}
					leader.TakeDamage(new DamageInfo(DamageDefOf.Bullet, 30f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			});
			base.DebugAction("Refog map", delegate
			{
				FloodFillerFog.DebugRefogMap(Find.CurrentMap);
			});
			base.DebugAction("Use GenStep", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (Type localGenStep2 in typeof(GenStep).AllSubclassesNonAbstract())
				{
					Type localGenStep = localGenStep2;
					list.Add(new DebugMenuOption(localGenStep.Name, DebugMenuOptionMode.Action, delegate()
					{
						GenStep genStep = (GenStep)Activator.CreateInstance(localGenStep);
						genStep.Generate(Find.CurrentMap);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Increment time 1 hour", delegate
			{
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 2500);
			});
			base.DebugAction("Increment time 6 hours", delegate
			{
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 15000);
			});
			base.DebugAction("Increment time 1 day", delegate
			{
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 60000);
			});
			base.DebugAction("Increment time 1 season", delegate
			{
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 900000);
			});
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x002BC214 File Offset: 0x002BA614
		private void DoListingItems_MapTools()
		{
			base.DoGap();
			base.DoLabel("Tools - General");
			base.DebugToolMap("T: Destroy", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			});
			base.DebugToolMap("T: Kill", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Kill(null, null);
				}
			});
			base.DebugToolMap("T: 10 damage", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			});
			base.DebugToolMap("T: 10 damage until dead", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					for (int i = 0; i < 1000; i++)
					{
						thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
						if (thing.Destroyed)
						{
							string str = "Took " + (i + 1) + " hits";
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
								{
									str = str + " (reached lethal damage threshold of " + pawn.health.LethalDamageThreshold.ToString("0.#") + ")";
								}
								else if (PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
								{
									str += " (core part hp reached 0)";
								}
								else
								{
									PawnCapacityDef pawnCapacityDef = pawn.health.ShouldBeDeadFromRequiredCapacity();
									if (pawnCapacityDef != null)
									{
										str = str + " (incapable of " + pawnCapacityDef.defName + ")";
									}
								}
							}
							Log.Message(str + ".", false);
							break;
						}
					}
				}
			});
			base.DebugToolMap("T: 5000 damage", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 5000f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			});
			base.DebugToolMap("T: 5000 flame damage", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.TakeDamage(new DamageInfo(DamageDefOf.Flame, 5000f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			});
			base.DebugToolMap("T: Clear area 21x21", delegate
			{
				CellRect r = CellRect.CenteredOn(UI.MouseCell(), 10);
				GenDebug.ClearArea(r, Find.CurrentMap);
			});
			base.DebugToolMap("T: Rock 21x21", delegate
			{
				CellRect cellRect = CellRect.CenteredOn(UI.MouseCell(), 10);
				cellRect.ClipInsideMap(Find.CurrentMap);
				foreach (IntVec3 loc in cellRect)
				{
					GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
				}
			});
			base.DebugToolMap("T: Destroy trees 21x21", delegate
			{
				CellRect cellRect = CellRect.CenteredOn(UI.MouseCell(), 10);
				cellRect.ClipInsideMap(Find.CurrentMap);
				foreach (IntVec3 c in cellRect)
				{
					List<Thing> thingList = c.GetThingList(Find.CurrentMap);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						if (thingList[i].def.category == ThingCategory.Plant && thingList[i].def.plant.IsTree)
						{
							thingList[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
			});
			base.DoGap();
			base.DebugToolMap("T: Explosion (bomb)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Bomb, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("T: Explosion (flame)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Flame, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("T: Explosion (stun)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Stun, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("T: Explosion (EMP)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.EMP, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("T: Explosion (extinguisher)", delegate
			{
				IntVec3 center = UI.MouseCell();
				Map currentMap = Find.CurrentMap;
				float radius = 10f;
				DamageDef extinguish = DamageDefOf.Extinguish;
				Thing instigator = null;
				ThingDef filth_FireFoam = ThingDefOf.Filth_FireFoam;
				GenExplosion.DoExplosion(center, currentMap, radius, extinguish, instigator, -1, null, null, null, null, filth_FireFoam, 1f, 3, true, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("T: Explosion (smoke)", delegate
			{
				IntVec3 center = UI.MouseCell();
				Map currentMap = Find.CurrentMap;
				float radius = 10f;
				DamageDef smoke = DamageDefOf.Smoke;
				Thing instigator = null;
				ThingDef gas_Smoke = ThingDefOf.Gas_Smoke;
				GenExplosion.DoExplosion(center, currentMap, radius, smoke, instigator, -1, null, null, null, null, gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("T: Lightning strike", delegate
			{
				Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, UI.MouseCell()));
			});
			base.DoGap();
			base.DebugToolMap("T: Add snow", delegate
			{
				SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
			});
			base.DebugToolMap("T: Remove snow", delegate
			{
				SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
			});
			base.DebugAction("Clear all snow", delegate
			{
				foreach (IntVec3 c in Find.CurrentMap.AllCells)
				{
					Find.CurrentMap.snowGrid.SetDepth(c, 0f);
				}
			});
			base.DebugToolMap("T: Push heat (10)", delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 10f);
			});
			base.DebugToolMap("T: Push heat (10000)", delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 10000f);
			});
			base.DebugToolMap("T: Push heat (-1000)", delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, -1000f);
			});
			base.DoGap();
			base.DebugToolMap("T: Finish plant growth", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					Plant plant = thing as Plant;
					if (plant != null)
					{
						plant.Growth = 1f;
					}
				}
			});
			base.DebugToolMap("T: Grow 1 day", delegate
			{
				IntVec3 intVec = UI.MouseCell();
				Plant plant = intVec.GetPlant(Find.CurrentMap);
				if (plant != null && plant.def.plant != null)
				{
					int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
					if (num >= 60000)
					{
						plant.Age += 60000;
					}
					else if (num > 0)
					{
						plant.Age += num;
					}
					plant.Growth += 1f / plant.def.plant.growDays;
					if ((double)plant.Growth > 1.0)
					{
						plant.Growth = 1f;
					}
					Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
				}
			});
			base.DebugToolMap("T: Grow to maturity", delegate
			{
				IntVec3 intVec = UI.MouseCell();
				Plant plant = intVec.GetPlant(Find.CurrentMap);
				if (plant != null && plant.def.plant != null)
				{
					int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
					plant.Age += num;
					plant.Growth = 1f;
					Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
				}
			});
			base.DoGap();
			base.DebugToolMap("T: Regen section", delegate
			{
				Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
			});
			base.DebugToolMap("T: Randomize color", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompColorable compColorable = thing.TryGetComp<CompColorable>();
					if (compColorable != null)
					{
						thing.SetColor(GenColor.RandomColorOpaque(), true);
					}
				}
			});
			base.DebugToolMap("T: Rot 1 day", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += 60000f;
					}
				}
			});
			base.DebugToolMap("T: Fuel -20%", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel(compRefuelable.Props.fuelCapacity * 0.2f);
					}
				}
			});
			base.DebugToolMap("T: Break down...", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompBreakdownable compBreakdownable = thing.TryGetComp<CompBreakdownable>();
					if (compBreakdownable != null && !compBreakdownable.BrokenDown)
					{
						compBreakdownable.DoBreakdown();
					}
				}
			});
			base.DebugAction("T: Use scatterer", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_MapGen.Options_Scatterers()));
			});
			base.DebugAction("T: BaseGen", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (string text in (from x in DefDatabase<RuleDef>.AllDefs
				select x.symbol).Distinct<string>())
				{
					string localSymbol = text;
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
					{
						DebugTool tool = null;
						IntVec3 firstCorner;
						tool = new DebugTool("first corner...", delegate()
						{
							firstCorner = UI.MouseCell();
							DebugTools.curTool = new DebugTool("second corner...", delegate()
							{
								IntVec3 second = UI.MouseCell();
								CellRect rect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
								BaseGen.globalSettings.map = Find.CurrentMap;
								BaseGen.symbolStack.Push(localSymbol, rect);
								BaseGen.Generate();
								DebugTools.curTool = tool;
							}, firstCorner);
						}, null);
						DebugTools.curTool = tool;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugToolMap("T: Make roof", delegate
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
				while (!iterator.Done())
				{
					Find.CurrentMap.roofGrid.SetRoof(iterator.Current, RoofDefOf.RoofConstructed);
					iterator.MoveNext();
				}
			});
			base.DebugToolMap("T: Delete roof", delegate
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
				while (!iterator.Done())
				{
					Find.CurrentMap.roofGrid.SetRoof(iterator.Current, null);
					iterator.MoveNext();
				}
			});
			base.DebugToolMap("T: Toggle trap status", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					Building_Trap building_Trap = thing as Building_Trap;
					if (building_Trap != null)
					{
						if (building_Trap.Armed)
						{
							building_Trap.Spring(null);
						}
						else
						{
							Building_TrapRearmable building_TrapRearmable = building_Trap as Building_TrapRearmable;
							if (building_TrapRearmable != null)
							{
								building_TrapRearmable.Rearm();
							}
						}
					}
				}
			});
			base.DebugToolMap("T: Add trap memory", delegate
			{
				foreach (Faction faction in Find.World.factionManager.AllFactions)
				{
					faction.TacticalMemory.TrapRevealed(UI.MouseCell(), Find.CurrentMap);
				}
				Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "added", 50);
			});
			base.DebugToolMap("T: Test flood unfog", delegate
			{
				FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.CurrentMap);
			});
			base.DebugToolMap("T: Flash closewalk cell 30", delegate
			{
				IntVec3 c = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.CurrentMap, 30, null);
				Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
			});
			base.DebugToolMap("T: Flash walk path", delegate
			{
				WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
			});
			base.DebugToolMap("T: Flash skygaze cell", delegate
			{
				Pawn pawn = Find.CurrentMap.mapPawns.FreeColonists.First<Pawn>();
				IntVec3 c;
				RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn, out c);
				Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
				MoteMaker.ThrowText(c.ToVector3Shifted(), Find.CurrentMap, "for " + pawn.Label, Color.white, -1f);
			});
			base.DebugToolMap("T: Flash direct flee dest", delegate
			{
				Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
				IntVec3 c;
				if (pawn == null)
				{
					Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "select a pawn", 50);
				}
				else if (RCellFinder.TryFindDirectFleeDestination(UI.MouseCell(), 9f, pawn, out c))
				{
					Find.CurrentMap.debugDrawer.FlashCell(c, 0.5f, null, 50);
				}
				else
				{
					Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0.8f, "not found", 50);
				}
			});
			base.DebugAction("T: Flash spectators cells", delegate
			{
				Action<bool> act = delegate(bool bestSideOnly)
				{
					DebugTool tool = null;
					IntVec3 firstCorner;
					tool = new DebugTool("first watch rect corner...", delegate()
					{
						firstCorner = UI.MouseCell();
						DebugTools.curTool = new DebugTool("second watch rect corner...", delegate()
						{
							IntVec3 second = UI.MouseCell();
							CellRect spectateRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
							SpectateRectSide allowedSides = SpectateRectSide.All;
							if (bestSideOnly)
							{
								allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1);
							}
							SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.CurrentMap, allowedSides, 1);
							DebugTools.curTool = tool;
						}, firstCorner);
					}, null);
					DebugTools.curTool = tool;
				};
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				list.Add(new DebugMenuOption("All sides", DebugMenuOptionMode.Action, delegate()
				{
					act(false);
				}));
				list.Add(new DebugMenuOption("Best side only", DebugMenuOptionMode.Action, delegate()
				{
					act(true);
				}));
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Check reachability", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				TraverseMode[] array = (TraverseMode[])Enum.GetValues(typeof(TraverseMode));
				for (int i = 0; i < array.Length; i++)
				{
					TraverseMode traverseMode3 = array[i];
					TraverseMode traverseMode = traverseMode3;
					list.Add(new DebugMenuOption(traverseMode3.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						DebugTool tool = null;
						IntVec3 from;
						Pawn fromPawn;
						tool = new DebugTool("from...", delegate()
						{
							from = UI.MouseCell();
							fromPawn = from.GetFirstPawn(Find.CurrentMap);
							string text = "to...";
							if (fromPawn != null)
							{
								text = text + " (pawn=" + fromPawn.LabelShort + ")";
							}
							DebugTools.curTool = new DebugTool(text, delegate()
							{
								DebugTools.curTool = tool;
							}, delegate()
							{
								IntVec3 c = UI.MouseCell();
								Pawn fromPawn;
								bool flag;
								IntVec3 intVec;
								if (fromPawn != null)
								{
									fromPawn = fromPawn;
									LocalTargetInfo dest = c;
									PathEndMode peMode = PathEndMode.OnCell;
									Danger maxDanger = Danger.Deadly;
									TraverseMode traverseMode2 = traverseMode;
									flag = fromPawn.CanReach(dest, peMode, maxDanger, false, traverseMode2);
									intVec = fromPawn.Position;
								}
								else
								{
									flag = Find.CurrentMap.reachability.CanReach(from, c, PathEndMode.OnCell, traverseMode, Danger.Deadly);
									intVec = from;
								}
								Color color = (!flag) ? Color.red : Color.green;
								Widgets.DrawLine(intVec.ToUIPosition(), c.ToUIPosition(), color, 2f);
							});
						}, null);
						DebugTools.curTool = tool;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugToolMapForPawns("T: Flash TryFindRandomPawnExitCell", delegate(Pawn p)
			{
				IntVec3 intVec;
				if (CellFinder.TryFindRandomPawnExitCell(p, out intVec))
				{
					p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
					p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				}
				else
				{
					p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no exit cell", 50);
				}
			});
			base.DebugToolMapForPawns("T: RandomSpotJustOutsideColony", delegate(Pawn p)
			{
				IntVec3 intVec;
				if (RCellFinder.TryFindRandomSpotJustOutsideColony(p, out intVec))
				{
					p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
					p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				}
				else
				{
					p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no cell", 50);
				}
			});
			base.DoGap();
			base.DoLabel("Tools - Pawns");
			base.DebugToolMap("T: Resurrect", delegate
			{
				foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null)
					{
						ResurrectionUtility.Resurrect(corpse.InnerPawn);
					}
				}
			});
			base.DebugToolMapForPawns("T: Damage to down", delegate(Pawn p)
			{
				HealthUtility.DamageUntilDowned(p);
			});
			base.DebugToolMapForPawns("T: Damage legs", delegate(Pawn p)
			{
				HealthUtility.DamageLegsUntilIncapableOfMoving(p);
			});
			base.DebugToolMapForPawns("T: Damage to death", delegate(Pawn p)
			{
				HealthUtility.DamageUntilDead(p);
			});
			base.DebugToolMap("T: Damage held pawn to death", delegate
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing is Pawn)
					{
						HealthUtility.DamageUntilDead((Pawn)pawn.carryTracker.CarriedThing);
					}
				}
			});
			base.DebugToolMapForPawns("T: Surgery fail minor", delegate(Pawn p)
			{
				BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where !x.def.conceptual
				select x).RandomElement<BodyPartRecord>();
				Log.Message("part is " + bodyPartRecord, false);
				HealthUtility.GiveInjuriesOperationFailureMinor(p, bodyPartRecord);
			});
			base.DebugToolMapForPawns("T: Surgery fail catastrophic", delegate(Pawn p)
			{
				BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where !x.def.conceptual
				select x).RandomElement<BodyPartRecord>();
				Log.Message("part is " + bodyPartRecord, false);
				HealthUtility.GiveInjuriesOperationFailureCatastrophic(p, bodyPartRecord);
			});
			base.DebugToolMapForPawns("T: Surgery fail ridiculous", delegate(Pawn p)
			{
				HealthUtility.GiveInjuriesOperationFailureRidiculous(p);
			});
			base.DebugToolMapForPawns("T: Restore body part...", delegate(Pawn p)
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
			});
			base.DebugAction("T: Apply damage...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_ApplyDamage()));
			});
			base.DebugAction("T: Add Hediff...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff()));
			});
			base.DebugToolMapForPawns("T: Remove Hediff...", delegate(Pawn p)
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RemoveHediff(p)));
			});
			base.DebugToolMapForPawns("T: Heal random injury (10)", delegate(Pawn p)
			{
				Hediff_Injury hediff_Injury;
				if ((from x in p.health.hediffSet.GetHediffs<Hediff_Injury>()
				where x.CanHealNaturally() || x.CanHealFromTending()
				select x).TryRandomElement(out hediff_Injury))
				{
					hediff_Injury.Heal(10f);
				}
			});
			base.DebugToolMapForPawns("T: Activate HediffGiver", delegate(Pawn p)
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				if (p.RaceProps.hediffGiverSets != null)
				{
					foreach (HediffGiver localHdg2 in p.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef set) => set.hediffGivers))
					{
						HediffGiver localHdg = localHdg2;
						list.Add(new FloatMenuOption(localHdg.hediff.defName, delegate()
						{
							localHdg.TryApply(p, null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				if (list.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list));
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Discover Hediffs", delegate(Pawn p)
			{
				foreach (Hediff hediff in p.health.hediffSet.hediffs)
				{
					if (!hediff.Visible)
					{
						hediff.Severity = Mathf.Max(hediff.Severity, hediff.def.stages.First((HediffStage s) => s.becomeVisible).minSeverity);
					}
				}
			});
			base.DebugToolMapForPawns("T: Grant immunities", delegate(Pawn p)
			{
				foreach (Hediff hediff in p.health.hediffSet.hediffs)
				{
					ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff.def);
					if (immunityRecord != null)
					{
						immunityRecord.immunity = 1f;
					}
				}
			});
			base.DebugToolMapForPawns("T: Give birth", delegate(Pawn p)
			{
				Hediff_Pregnant.DoBirthSpawn(p, null);
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("T: List melee verbs", delegate(Pawn p)
			{
				Log.Message(string.Format("Verb list:\n  {0}", GenText.ToTextList(from verb in p.meleeVerbs.GetUpdatedAvailableVerbsList()
				select verb.ToString(), "\n  ")), false);
			});
			base.DebugToolMapForPawns("T: Add/remove pawn relation", delegate(Pawn p)
			{
				if (p.RaceProps.IsFlesh)
				{
					Action<bool> act = delegate(bool add)
					{
						if (add)
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							foreach (PawnRelationDef pawnRelationDef in DefDatabase<PawnRelationDef>.AllDefs)
							{
								if (!pawnRelationDef.implied)
								{
									PawnRelationDef defLocal = pawnRelationDef;
									list2.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate()
									{
										List<DebugMenuOption> list4 = new List<DebugMenuOption>();
										IOrderedEnumerable<Pawn> orderedEnumerable = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
										where x.RaceProps.IsFlesh
										orderby x.def == p.def descending, x.IsWorldPawn()
										select x;
										foreach (Pawn pawn in orderedEnumerable)
										{
											if (p != pawn)
											{
												if (!defLocal.familyByBloodRelation || pawn.def == p.def)
												{
													if (!p.relations.DirectRelationExists(defLocal, pawn))
													{
														Pawn otherLocal = pawn;
														list4.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
														{
															p.relations.AddDirectRelation(defLocal, otherLocal);
														}));
													}
												}
											}
										}
										Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
									}));
								}
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						}
						else
						{
							List<DebugMenuOption> list3 = new List<DebugMenuOption>();
							List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
							for (int i = 0; i < directRelations.Count; i++)
							{
								DirectPawnRelation rel = directRelations[i];
								list3.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate()
								{
									p.relations.RemoveDirectRelation(rel);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
						}
					};
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					list.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, delegate()
					{
						act(true);
					}));
					list.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate()
					{
						act(false);
					}));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
			});
			base.DebugToolMapForPawns("T: Add opinion thoughts about", delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike)
				{
					Action<bool> act = delegate(bool good)
					{
						foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike
						select x)
						{
							if (p != pawn)
							{
								IEnumerable<ThoughtDef> source = DefDatabase<ThoughtDef>.AllDefs.Where((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0f) || (!good && x.stages[0].baseOpinionOffset < 0f)));
								if (source.Any<ThoughtDef>())
								{
									int num = Rand.Range(2, 5);
									for (int i = 0; i < num; i++)
									{
										ThoughtDef def = source.RandomElement<ThoughtDef>();
										pawn.needs.mood.thoughts.memories.TryGainMemory(def, p);
									}
								}
							}
						}
					};
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					list.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, delegate()
					{
						act(true);
					}));
					list.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, delegate()
					{
						act(false);
					}));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
			});
			base.DebugToolMapForPawns("T: Force vomit...", delegate(Pawn p)
			{
				p.jobs.StartJob(new Job(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false);
			});
			base.DebugToolMap("T: Food -20%", delegate
			{
				this.OffsetNeed(NeedDefOf.Food, -0.2f);
			});
			base.DebugToolMap("T: Rest -20%", delegate
			{
				this.OffsetNeed(NeedDefOf.Rest, -0.2f);
			});
			base.DebugToolMap("T: Joy -20%", delegate
			{
				this.OffsetNeed(NeedDefOf.Joy, -0.2f);
			});
			base.DebugToolMap("T: Chemical -20%", delegate
			{
				List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (typeof(Need_Chemical).IsAssignableFrom(allDefsListForReading[i].needClass))
					{
						this.OffsetNeed(allDefsListForReading[i], -0.2f);
					}
				}
			});
			base.DebugAction("T: Set skill", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (SkillDef localDef in DefDatabase<SkillDef>.AllDefs)
				{
					Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey22 <DoListingItems_MapTools>c__AnonStorey = new Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey22();
					<DoListingItems_MapTools>c__AnonStorey.$this = this;
					<DoListingItems_MapTools>c__AnonStorey.localDef = localDef;
					list.Add(new DebugMenuOption(<DoListingItems_MapTools>c__AnonStorey.localDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						for (int i = 0; i <= 20; i++)
						{
							int level = i;
							list2.Add(new DebugMenuOption(level.ToString(), DebugMenuOptionMode.Tool, delegate()
							{
								Pawn pawn = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
								where t is Pawn
								select t).Cast<Pawn>().FirstOrDefault<Pawn>();
								if (pawn != null)
								{
									SkillRecord skill = pawn.skills.GetSkill(<DoListingItems_MapTools>c__AnonStorey.localDef);
									skill.Level = level;
									skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
									<DoListingItems_MapTools>c__AnonStorey.DustPuffFrom(pawn);
								}
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugToolMapForPawns("T: Max skills", delegate(Pawn p)
			{
				if (p.skills != null)
				{
					foreach (SkillDef sDef in DefDatabase<SkillDef>.AllDefs)
					{
						p.skills.Learn(sDef, 1E+08f, false);
					}
					this.DustPuffFrom(p);
				}
				if (p.training != null)
				{
					foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
					{
						Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
						bool flag;
						if (p.training.CanAssignToTrain(td, out flag).Accepted)
						{
							p.training.Train(td, trainer, false);
						}
					}
				}
			});
			base.DebugAction("T: Mental break...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				list.Add(new DebugMenuOption("(log possibles)", DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						pawn.mindState.mentalBreaker.LogPossibleMentalBreaks();
						this.DustPuffFrom(pawn);
					}
				}));
				list.Add(new DebugMenuOption("(natural mood break)", DebugMenuOptionMode.Tool, delegate()
				{
					foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						pawn.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
						this.DustPuffFrom(pawn);
					}
				}));
				foreach (MentalBreakDef locBrDef2 in from x in DefDatabase<MentalBreakDef>.AllDefs
				orderby x.intensity descending
				select x)
				{
					MentalBreakDef locBrDef = locBrDef2;
					string text = locBrDef.defName;
					if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef.Worker.BreakCanOccur(x)))
					{
						text += " [NO]";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate()
					{
						foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>())
						{
							locBrDef.Worker.TryStart(pawn, null, false);
							this.DustPuffFrom(pawn);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Mental state...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (MentalStateDef locBrDef2 in DefDatabase<MentalStateDef>.AllDefs)
				{
					MentalStateDef locBrDef = locBrDef2;
					string text = locBrDef.defName;
					if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef.Worker.StateCanOccur(x)))
					{
						text += " [NO]";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate()
					{
						foreach (Pawn locP2 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							Pawn locP = locP2;
							if (locBrDef != MentalStateDefOf.SocialFighting)
							{
								locP.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, true, false, null, false);
								this.DustPuffFrom(locP);
							}
							else
							{
								DebugTools.curTool = new DebugTool("...with", delegate()
								{
									Pawn pawn = (Pawn)(from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
									where t is Pawn
									select t).FirstOrDefault<Thing>();
									if (pawn != null)
									{
										if (!InteractionUtility.HasAnySocialFightProvokingThought(locP, pawn))
										{
											locP.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Insulted, pawn);
											Messages.Message("Dev: Auto added negative thought.", locP, MessageTypeDefOf.TaskCompletion, false);
										}
										locP.interactions.StartSocialFight(pawn);
										DebugTools.curTool = null;
									}
								}, null);
							}
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Inspiration...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (InspirationDef localDef2 in DefDatabase<InspirationDef>.AllDefs)
				{
					InspirationDef localDef = localDef2;
					string text = localDef.defName;
					if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => localDef.Worker.InspirationCanOccur(x)))
					{
						text += " [NO]";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate()
					{
						foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
						{
							pawn.mindState.inspirationHandler.TryStartInspiration(localDef);
							this.DustPuffFrom(pawn);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Give trait...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefs)
				{
					TraitDef trDef = traitDef;
					for (int j = 0; j < traitDef.degreeDatas.Count; j++)
					{
						int i = j;
						list.Add(new DebugMenuOption(string.Concat(new object[]
						{
							trDef.degreeDatas[i].label,
							" (",
							trDef.degreeDatas[j].degree,
							")"
						}), DebugMenuOptionMode.Tool, delegate()
						{
							foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).Cast<Pawn>())
							{
								if (pawn.story != null)
								{
									Trait item = new Trait(trDef, trDef.degreeDatas[i].degree, false);
									pawn.story.traits.allTraits.Add(item);
									this.DustPuffFrom(pawn);
								}
							}
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugToolMapForPawns("T: Give good thought", delegate(Pawn p)
			{
				if (p.needs.mood != null)
				{
					p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null);
				}
			});
			base.DebugToolMapForPawns("T: Give bad thought", delegate(Pawn p)
			{
				if (p.needs.mood != null)
				{
					p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null);
				}
			});
			base.DebugToolMapForPawns("T: Make faction hostile", delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					p.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Make faction neutral", delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					p.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Neutral, true, null, null);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Make faction allied", delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					p.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Ally, true, null, null);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMap("T: Clear bound unfinished things", delegate
			{
				foreach (Building_WorkTable building_WorkTable in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Building_WorkTable
				select t).Cast<Building_WorkTable>())
				{
					foreach (Bill bill in building_WorkTable.BillStack)
					{
						Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
						if (bill_ProductionWithUft != null)
						{
							bill_ProductionWithUft.ClearBoundUft();
						}
					}
				}
			});
			base.DebugToolMapForPawns("T: Force birthday", delegate(Pawn p)
			{
				p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
				p.ageTracker.DebugForceBirthdayBiological();
			});
			base.DebugToolMapForPawns("T: Recruit", delegate(Pawn p)
			{
				if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, 1f, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Damage apparel", delegate(Pawn p)
			{
				if (p.apparel != null && p.apparel.WornApparelCount > 0)
				{
					p.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Tame animal", delegate(Pawn p)
			{
				if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, 1f, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Train animal", delegate(Pawn p)
			{
				if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
				{
					this.DustPuffFrom(p);
					bool flag = false;
					foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
					{
						if (p.training.GetWanted(td))
						{
							p.training.Train(td, null, true);
							flag = true;
						}
					}
					if (!flag)
					{
						foreach (TrainableDef td2 in DefDatabase<TrainableDef>.AllDefs)
						{
							if (p.training.CanAssignToTrain(td2).Accepted)
							{
								p.training.Train(td2, null, true);
							}
						}
					}
				}
			});
			base.DebugToolMapForPawns("T: Name animal by nuzzling", delegate(Pawn p)
			{
				if ((p.Name == null || p.Name.Numerical) && p.RaceProps.Animal)
				{
					PawnUtility.GiveNameBecauseOfNuzzle(p.Map.mapPawns.FreeColonists.First<Pawn>(), p);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Try develop bond relation", delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					if (p.RaceProps.Humanlike)
					{
						IEnumerable<Pawn> source = from x in p.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Animal && x.Faction == p.Faction
						select x;
						if (source.Any<Pawn>())
						{
							RelationsUtility.TryDevelopBondRelation(p, source.RandomElement<Pawn>(), 999999f);
						}
					}
					else if (p.RaceProps.Animal)
					{
						IEnumerable<Pawn> source2 = from x in p.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike && x.Faction == p.Faction
						select x;
						if (source2.Any<Pawn>())
						{
							RelationsUtility.TryDevelopBondRelation(source2.RandomElement<Pawn>(), p, 999999f);
						}
					}
				}
			});
			base.DebugToolMapForPawns("T: Queue training decay", delegate(Pawn p)
			{
				if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
				{
					p.training.Debug_MakeDegradeHappenSoon();
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("T: Start marriage ceremony", delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike)
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
					where x.RaceProps.Humanlike
					select x)
					{
						if (p != pawn)
						{
							Pawn otherLocal = pawn;
							list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
							{
								if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal))
								{
									p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal);
									p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal);
									p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal);
									Messages.Message("Dev: Auto added fiance relation.", p, MessageTypeDefOf.TaskCompletion, false);
								}
								if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal))
								{
									Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput, false);
								}
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
			});
			base.DebugToolMapForPawns("T: Force interaction", delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (Pawn pawn in p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction))
					{
						if (pawn != p)
						{
							Pawn otherLocal = pawn;
							list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list2 = new List<DebugMenuOption>();
								foreach (InteractionDef interactionLocal2 in DefDatabase<InteractionDef>.AllDefsListForReading)
								{
									InteractionDef interactionLocal = interactionLocal2;
									list2.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate()
									{
										p.interactions.TryInteractWith(otherLocal, interactionLocal);
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
			});
			base.DebugAction("T: Start party", delegate
			{
				if (!Find.CurrentMap.lordsStarter.TryStartParty())
				{
					Messages.Message("Could not find any valid party spot or organizer.", MessageTypeDefOf.RejectInput, false);
				}
			});
			base.DebugToolMapForPawns("T: Start prison break", delegate(Pawn p)
			{
				if (p.IsPrisoner)
				{
					PrisonBreakUtility.StartPrisonBreak(p);
				}
			});
			base.DebugToolMapForPawns("T: Pass to world", delegate(Pawn p)
			{
				p.DeSpawn(DestroyMode.Vanish);
				Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
			});
			base.DebugToolMapForPawns("T: Make 1 year older", delegate(Pawn p)
			{
				p.ageTracker.DebugMake1YearOlder();
			});
			base.DoGap();
			base.DebugToolMapForPawns("T: Try job giver", delegate(Pawn p)
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (Type localType2 in typeof(ThinkNode_JobGiver).AllSubclasses())
				{
					Type localType = localType2;
					list.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate()
					{
						ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(localType);
						thinkNode_JobGiver.ResolveReferences();
						ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(p, default(JobIssueParams));
						if (thinkResult.Job != null)
						{
							p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, null, false);
						}
						else
						{
							Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugToolMapForPawns("T: Try joy giver", delegate(Pawn p)
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				using (List<JoyGiverDef>.Enumerator enumerator = DefDatabase<JoyGiverDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JoyGiverDef def = enumerator.Current;
						list.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Action, delegate()
						{
							Job job = def.Worker.TryGiveJob(p);
							if (job != null)
							{
								p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false);
							}
							else
							{
								Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
							}
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugToolMapForPawns("T: EndCurrentJob(" + JobCondition.InterruptForced.ToString() + ")", delegate(Pawn p)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("T: CheckForJobOverride", delegate(Pawn p)
			{
				p.jobs.CheckForJobOverride();
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("T: Toggle job logging", delegate(Pawn p)
			{
				p.jobs.debugLog = !p.jobs.debugLog;
				this.DustPuffFrom(p);
				MoteMaker.ThrowText(p.DrawPos, p.Map, p.LabelShort + "\n" + ((!p.jobs.debugLog) ? "OFF" : "ON"), -1f);
			});
			base.DebugToolMapForPawns("T: Toggle stance logging", delegate(Pawn p)
			{
				p.stances.debugLog = !p.stances.debugLog;
				this.DustPuffFrom(p);
			});
			base.DoGap();
			base.DoLabel("Tools - Spawning");
			base.DebugAction("T: Spawn pawn", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
				orderby kd.defName
				select kd)
				{
					PawnKindDef localKindDef = localKindDef2;
					list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Tool, delegate()
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
						Pawn newPawn = PawnGenerator.GeneratePawn(localKindDef, faction);
						GenSpawn.Spawn(newPawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						if (faction != null && faction != Faction.OfPlayer)
						{
							Lord lord = null;
							if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn))
							{
								Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, (Thing p) => p != newPawn && ((Pawn)p).GetLord() != null, null);
								lord = p2.GetLord();
							}
							if (lord == null)
							{
								LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn.Position);
								lord = LordMaker.MakeNewLord(faction, lordJob, Find.CurrentMap, null);
							}
							lord.AddPawn(newPawn);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Spawn weapon...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.equipmentType == EquipmentType.Primary
				select def into d
				orderby d.defName
				select d)
				{
					ThingDef localDef = localDef2;
					list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Spawn apparel...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.IsApparel
				select def into d
				orderby d.defName
				select d)
				{
					ThingDef localDef = localDef2;
					list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Try place near thing...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)));
			});
			base.DebugAction("T: Try place near stacks of 25...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)));
			});
			base.DebugAction("T: Try place near stacks of 75...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)));
			});
			base.DebugAction("T: Try place direct thing...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)));
			});
			base.DebugAction("T: Try place direct stacks of 25...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)));
			});
			base.DebugAction("T: Spawn thing with wipe mode...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				WipeMode[] array = (WipeMode[])Enum.GetValues(typeof(WipeMode));
				for (int i = 0; i < array.Length; i++)
				{
					WipeMode localWipeMode2 = array[i];
					WipeMode localWipeMode = localWipeMode2;
					list.Add(new DebugMenuOption(localWipeMode2.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.SpawnOptions(localWipeMode)));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Set terrain...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (TerrainDef localDef2 in DefDatabase<TerrainDef>.AllDefs)
				{
					TerrainDef localDef = localDef2;
					list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
					{
						if (UI.MouseCell().InBounds(Find.CurrentMap))
						{
							Find.CurrentMap.terrainGrid.SetTerrain(UI.MouseCell(), localDef);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugToolMap("T: Make filth x100", delegate
			{
				for (int i = 0; i < 100; i++)
				{
					IntVec3 c = UI.MouseCell() + GenRadial.RadialPattern[i];
					if (c.InBounds(Find.CurrentMap) && c.Walkable(Find.CurrentMap))
					{
						FilthMaker.MakeFilth(c, Find.CurrentMap, ThingDefOf.Filth_Dirt, 2);
						MoteMaker.ThrowMetaPuff(c.ToVector3Shifted(), Find.CurrentMap);
					}
				}
			});
			base.DebugToolMap("T: Spawn faction leader", delegate
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (Faction localFac2 in Find.FactionManager.AllFactions)
				{
					Faction localFac = localFac2;
					if (localFac.leader != null)
					{
						list.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, delegate()
						{
							GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			});
			base.DebugAction("T: Spawn world pawn...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				Action<Pawn> act = delegate(Pawn p)
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (PawnKindDef kLocal2 in from x in DefDatabase<PawnKindDef>.AllDefs
					where x.race == p.def
					select x)
					{
						PawnKindDef kLocal = kLocal2;
						list2.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate()
						{
							PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
							PawnGenerator.RedressPawn(p, request);
							GenSpawn.Spawn(p, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
							DebugTools.curTool = null;
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				};
				foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
				{
					Pawn pLocal = pawn;
					list.Add(new DebugMenuOption(pawn.LabelShort, DebugMenuOptionMode.Action, delegate()
					{
						act(pLocal);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Spawn thing set...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<ThingSetMakerDef> allDefsListForReading = DefDatabase<ThingSetMakerDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					ThingSetMakerDef localGenerator = allDefsListForReading[i];
					list.Add(new DebugMenuOption(localGenerator.defName, DebugMenuOptionMode.Tool, delegate()
					{
						if (UI.MouseCell().InBounds(Find.CurrentMap))
						{
							StringBuilder stringBuilder = new StringBuilder();
							string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localGenerator.debugParams);
							List<Thing> list2 = localGenerator.root.Generate(localGenerator.debugParams);
							stringBuilder.Append(string.Concat(new object[]
							{
								localGenerator.defName,
								" generated ",
								list2.Count,
								" things"
							}));
							if (!nonNullFieldsDebugInfo.NullOrEmpty())
							{
								stringBuilder.Append(" (used custom debug params: " + nonNullFieldsDebugInfo + ")");
							}
							stringBuilder.AppendLine(":");
							float num = 0f;
							float num2 = 0f;
							for (int j = 0; j < list2.Count; j++)
							{
								stringBuilder.AppendLine("   - " + list2[j].LabelCap);
								num += list2[j].MarketValue * (float)list2[j].stackCount;
								if (!(list2[j] is Pawn))
								{
									num2 += list2[j].GetStatValue(StatDefOf.Mass, true) * (float)list2[j].stackCount;
								}
								if (!GenPlace.TryPlaceThing(list2[j], UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null))
								{
									list2[j].Destroy(DestroyMode.Vanish);
								}
							}
							stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
							stringBuilder.AppendLine("Total mass: " + num2.ToStringMass());
							Log.Message(stringBuilder.ToString(), false);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("T: Trigger effecter...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<EffecterDef> allDefsListForReading = DefDatabase<EffecterDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					EffecterDef localDef = allDefsListForReading[i];
					list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate()
					{
						Effecter effecter = localDef.Spawn();
						effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), new TargetInfo(UI.MouseCell(), Find.CurrentMap, false));
						effecter.Cleanup();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DoGap();
			base.DoLabel("Autotests");
			base.DebugAction("Make colony (full)", delegate
			{
				Autotests_ColonyMaker.MakeColony_Full();
			});
			base.DebugAction("Make colony (animals)", delegate
			{
				Autotests_ColonyMaker.MakeColony_Animals();
			});
			base.DebugAction("Test force downed x100", delegate
			{
				for (int i = 0; i < 100; i++)
				{
					PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
					GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
					HealthUtility.DamageUntilDowned(pawn);
					if (pawn.Dead)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn died while force downing: ",
							pawn,
							" at ",
							pawn.Position
						}), false);
						break;
					}
				}
			});
			base.DebugAction("Test force kill x100", delegate
			{
				for (int i = 0; i < 100; i++)
				{
					PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
					GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
					HealthUtility.DamageUntilDead(pawn);
					if (!pawn.Dead)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn died not die: ",
							pawn,
							" at ",
							pawn.Position
						}), false);
						break;
					}
				}
			});
			base.DebugAction("Test Surgery fail catastrophic x100", delegate
			{
				for (int i = 0; i < 100; i++)
				{
					PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
					GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
					pawn.health.forceIncap = true;
					BodyPartRecord part = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).RandomElement<BodyPartRecord>();
					HealthUtility.GiveInjuriesOperationFailureCatastrophic(pawn, part);
					pawn.health.forceIncap = false;
					if (pawn.Dead)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn died: ",
							pawn,
							" at ",
							pawn.Position
						}), false);
					}
				}
			});
			base.DebugAction("Test Surgery fail ridiculous x100", delegate
			{
				for (int i = 0; i < 100; i++)
				{
					PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
					GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
					pawn.health.forceIncap = true;
					HealthUtility.GiveInjuriesOperationFailureRidiculous(pawn);
					pawn.health.forceIncap = false;
					if (pawn.Dead)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn died: ",
							pawn,
							" at ",
							pawn.Position
						}), false);
					}
				}
			});
			base.DebugAction("Test generate pawn x1000", delegate
			{
				float[] array = new float[]
				{
					10f,
					20f,
					50f,
					100f,
					200f,
					500f,
					1000f,
					2000f,
					5000f,
					1E+20f
				};
				int[] array2 = new int[array.Length];
				for (int i = 0; i < 1000; i++)
				{
					PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
					PerfLogger.Reset();
					Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionType));
					float ms = PerfLogger.Duration() * 1000f;
					array2[array.FirstIndexOf((float time) => ms <= time)]++;
					if (pawn.Dead)
					{
						Log.Error("Pawn is dead", false);
					}
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Pawn creation time histogram:");
				for (int j = 0; j < array2.Length; j++)
				{
					stringBuilder.AppendLine(string.Format("<{0}ms: {1}", array[j], array2[j]));
				}
				Log.Message(stringBuilder.ToString(), false);
			});
			base.DebugAction("Check region listers", delegate
			{
				Autotests_RegionListers.CheckBugs(Find.CurrentMap);
			});
			base.DebugAction("Test time-to-down", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				using (IEnumerator<PawnKindDef> enumerator = (from kd in DefDatabase<PawnKindDef>.AllDefs
				orderby kd.defName
				select kd).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PawnKindDef kindDef = enumerator.Current;
						list.Add(new DebugMenuOption(kindDef.label, DebugMenuOptionMode.Action, delegate()
						{
							if (kindDef == PawnKindDefOf.Colonist)
							{
								Log.Message("Current colonist TTD reference point: 22.3 seconds, stddev 8.35 seconds", false);
							}
							List<float> results = new List<float>();
							List<PawnKindDef> list2 = new List<PawnKindDef>();
							List<PawnKindDef> list3 = new List<PawnKindDef>();
							list2.Add(kindDef);
							list3.Add(kindDef);
							ArenaUtility.BeginArenaFightSet(1000, list2, list3, delegate(ArenaUtility.ArenaResult result)
							{
								if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
								{
									results.Add(result.tickDuration.TicksToSeconds());
								}
							}, delegate
							{
								string format = "Finished {0} tests; time-to-down {1}, stddev {2}\n\nraw: {3}";
								object[] array = new object[4];
								array[0] = results.Count;
								array[1] = results.Average();
								array[2] = GenMath.Stddev(results);
								array[3] = (from res in results
								select res.ToString()).ToLineList("");
								Log.Message(string.Format(format, array), false);
							});
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Battle Royale All PawnKinds", delegate
			{
				ArenaUtility.PerformBattleRoyale(DefDatabase<PawnKindDef>.AllDefs);
			});
			base.DebugAction("Battle Royale Humanlikes", delegate
			{
				ArenaUtility.PerformBattleRoyale(from k in DefDatabase<PawnKindDef>.AllDefs
				where k.RaceProps.Humanlike
				select k);
			});
			base.DebugAction("Battle Royale by Damagetype", delegate
			{
				PawnKindDef[] array = new PawnKindDef[]
				{
					PawnKindDefOf.Colonist,
					PawnKindDefOf.Muffalo
				};
				IEnumerable<ToolCapacityDef> enumerable = from tc in DefDatabase<ToolCapacityDef>.AllDefsListForReading
				where tc != ToolCapacityDefOf.KickMaterialInEyes
				select tc;
				Func<PawnKindDef, ToolCapacityDef, string> func = (PawnKindDef pkd, ToolCapacityDef dd) => string.Format("{0}_{1}", pkd.label, dd.defName);
				if (Dialog_DebugActionsMenu.pawnKindsForDamageTypeBattleRoyale == null)
				{
					Dialog_DebugActionsMenu.pawnKindsForDamageTypeBattleRoyale = new List<PawnKindDef>();
					foreach (PawnKindDef pawnKindDef in array)
					{
						using (IEnumerator<ToolCapacityDef> enumerator = enumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ToolCapacityDef toolType = enumerator.Current;
								string text = func(pawnKindDef, toolType);
								ThingDef thingDef = Gen.MemberwiseClone<ThingDef>(pawnKindDef.race);
								thingDef.defName = text;
								thingDef.label = text;
								thingDef.tools = new List<Tool>(pawnKindDef.race.tools.Select(delegate(Tool tool)
								{
									Tool tool2 = Gen.MemberwiseClone<Tool>(tool);
									tool2.capacities = new List<ToolCapacityDef>();
									tool2.capacities.Add(toolType);
									return tool2;
								}));
								PawnKindDef pawnKindDef2 = Gen.MemberwiseClone<PawnKindDef>(pawnKindDef);
								pawnKindDef2.defName = text;
								pawnKindDef2.label = text;
								pawnKindDef2.race = thingDef;
								Dialog_DebugActionsMenu.pawnKindsForDamageTypeBattleRoyale.Add(pawnKindDef2);
							}
						}
					}
				}
				ArenaUtility.PerformBattleRoyale(Dialog_DebugActionsMenu.pawnKindsForDamageTypeBattleRoyale);
			});
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x002BD544 File Offset: 0x002BB944
		private void DoListingItems_World()
		{
			base.DoLabel("Tools - World");
			Text.Font = GameFont.Tiny;
			base.DoLabel("Incidents");
			IIncidentTarget incidentTarget = Find.WorldSelector.SingleSelectedObject as IIncidentTarget;
			if (incidentTarget == null)
			{
				incidentTarget = Find.CurrentMap;
			}
			if (incidentTarget != null)
			{
				this.DoIncidentDebugAction(incidentTarget);
				this.DoIncidentWithPointsAction(incidentTarget);
			}
			this.DoIncidentDebugAction(Find.World);
			this.DoIncidentWithPointsAction(Find.World);
			base.DoLabel("Tools - Spawning");
			base.DebugToolWorld("Spawn random caravan", delegate
			{
				int num = GenWorld.MouseTile(false);
				Tile tile = Find.WorldGrid[num];
				if (!tile.biome.impassable)
				{
					List<Pawn> list = new List<Pawn>();
					int num2 = Rand.RangeInclusive(1, 10);
					for (int i = 0; i < num2; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
						list.Add(pawn);
						if (!pawn.story.WorkTagIsDisabled(WorkTags.Violent) && Rand.Value < 0.9f)
						{
							ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefs
							where def.IsWeapon && def.PlayerAcquirable
							select def).RandomElementWithFallback(null);
							pawn.equipment.AddEquipment((ThingWithComps)ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
						}
					}
					int num3 = Rand.RangeInclusive(-4, 10);
					for (int j = 0; j < num3; j++)
					{
						PawnKindDef kindDef = (from d in DefDatabase<PawnKindDef>.AllDefs
						where d.RaceProps.Animal && d.RaceProps.wildness < 1f
						select d).RandomElement<PawnKindDef>();
						Pawn item = PawnGenerator.GeneratePawn(kindDef, Faction.OfPlayer);
						list.Add(item);
					}
					Caravan caravan = CaravanMaker.MakeCaravan(list, Faction.OfPlayer, num, true);
					List<Thing> list2 = ThingSetMakerDefOf.DebugCaravanInventory.root.Generate();
					for (int k = 0; k < list2.Count; k++)
					{
						Thing thing = list2[k];
						if (thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount > caravan.MassCapacity - caravan.MassUsage)
						{
							break;
						}
						CaravanInventoryUtility.GiveThing(caravan, thing);
					}
				}
			});
			base.DebugToolWorld("Spawn random faction base", delegate
			{
				Faction faction;
				if ((from x in Find.FactionManager.AllFactions
				where !x.IsPlayer && !x.def.hidden
				select x).TryRandomElement(out faction))
				{
					int num = GenWorld.MouseTile(false);
					Tile tile = Find.WorldGrid[num];
					if (!tile.biome.impassable)
					{
						FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
						factionBase.SetFaction(faction);
						factionBase.Tile = num;
						factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, null);
						Find.WorldObjects.Add(factionBase);
					}
				}
			});
			base.DebugToolWorld("Spawn site", delegate
			{
				int tile = GenWorld.MouseTile(false);
				if (tile < 0 || Find.World.Impassable(tile))
				{
					Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					List<SitePartDef> parts = new List<SitePartDef>();
					foreach (SiteCoreDef localDef2 in DefDatabase<SiteCoreDef>.AllDefs)
					{
						SiteCoreDef localDef = localDef2;
						Action addPart = null;
						addPart = delegate()
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							list2.Add(new DebugMenuOption("-Done (" + parts.Count + " parts)-", DebugMenuOptionMode.Action, delegate()
							{
								Site site = SiteMaker.TryMakeSite(localDef, parts, true, null, true);
								if (site == null)
								{
									Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
								}
								else
								{
									site.Tile = tile;
									Find.WorldObjects.Add(site);
								}
							}));
							foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
							{
								SitePartDef localPart = sitePartDef;
								list2.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
								{
									parts.Add(localPart);
									addPart();
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						};
						list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, addPart));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
			});
			base.DoLabel("Tools - Misc");
			base.DebugAction("Change camera config...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (Type localType2 in typeof(WorldCameraConfig).AllSubclasses())
				{
					Type localType = localType2;
					string text = localType.Name;
					if (text.StartsWith("WorldCameraConfig_"))
					{
						text = text.Substring("WorldCameraConfig_".Length);
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
					{
						Find.WorldCameraDriver.config = (WorldCameraConfig)Activator.CreateInstance(localType);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x002BD670 File Offset: 0x002BBA70
		private void DoRaid(IncidentParms parms)
		{
			IncidentDef incidentDef;
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				incidentDef = IncidentDefOf.RaidEnemy;
			}
			else
			{
				incidentDef = IncidentDefOf.RaidFriendly;
			}
			incidentDef.Worker.TryExecute(parms);
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x002BD6B4 File Offset: 0x002BBAB4
		private void DoIncidentDebugAction(IIncidentTarget target)
		{
			base.DebugAction("Do incident (" + this.GetIncidentTargetLabel(target) + ")...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef localDef2 in from d in DefDatabase<IncidentDef>.AllDefs
				where d.TargetAllowed(target)
				orderby d.defName
				select d)
				{
					IncidentDef localDef = localDef2;
					string text = localDef.defName;
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
					if (!localDef.Worker.CanFireNow(parms))
					{
						text += " [NO]";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
					{
						if (localDef.pointsScaleable)
						{
							StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain);
							parms = storytellerComp.GenerateParms(localDef.category, parms.target);
						}
						localDef.Worker.TryExecute(parms);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x002BD700 File Offset: 0x002BBB00
		private void DoIncidentWithPointsAction(IIncidentTarget target)
		{
			base.DebugAction("Do incident w/ points (" + this.GetIncidentTargetLabel(target) + ")...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef localDef2 in from d in DefDatabase<IncidentDef>.AllDefs
				where d.TargetAllowed(target) && d.pointsScaleable
				orderby d.defName
				select d)
				{
					IncidentDef localDef = localDef2;
					string text = localDef.defName;
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, target);
					if (!localDef.Worker.CanFireNow(parms))
					{
						text += " [NO]";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float num in Dialog_DebugActionsMenu.PointsOptions(true))
						{
							float localPoints = num;
							list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
							{
								parms.points = localPoints;
								localDef.Worker.TryExecute(parms);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}

		// Token: 0x06005524 RID: 21796 RVA: 0x002BD74C File Offset: 0x002BBB4C
		private string GetIncidentTargetLabel(IIncidentTarget target)
		{
			string result;
			if (target == null)
			{
				result = "null target";
			}
			else if (target is Map)
			{
				result = "Map";
			}
			else if (target is World)
			{
				result = "World";
			}
			else if (target is Caravan)
			{
				result = ((Caravan)target).LabelCap;
			}
			else
			{
				result = target.ToString();
			}
			return result;
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x002BD7C0 File Offset: 0x002BBBC0
		private void DebugGiveResource(ThingDef resDef, int count)
		{
			Pawn pawn = Find.CurrentMap.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			int i = count;
			int num = 0;
			while (i > 0)
			{
				int num2 = Math.Min(resDef.stackLimit, i);
				i -= num2;
				Thing thing = ThingMaker.MakeThing(resDef, null);
				thing.stackCount = num2;
				if (!GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, null, null))
				{
					break;
				}
				num += num2;
			}
			Messages.Message(string.Concat(new object[]
			{
				"Made ",
				num,
				" ",
				resDef,
				" near ",
				pawn,
				"."
			}), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x002BD888 File Offset: 0x002BBC88
		private void OffsetNeed(NeedDef nd, float offsetPct)
		{
			foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Pawn
			select t).Cast<Pawn>())
			{
				Need need = pawn.needs.TryGetNeed(nd);
				if (need != null)
				{
					need.CurLevel += offsetPct * need.MaxLevel;
					this.DustPuffFrom(pawn);
				}
			}
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x002BD944 File Offset: 0x002BBD44
		private void DustPuffFrom(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.Drawer.Notify_DebugAffected();
			}
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x002BD96C File Offset: 0x002BBD6C
		private void AddGuest(bool prisoner)
		{
			foreach (Building_Bed building_Bed in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if (building_Bed.ForPrisoners == prisoner && (!building_Bed.owners.Any<Pawn>() || (prisoner && building_Bed.AnyUnownedSleepingSlot)))
				{
					PawnKindDef pawnKindDef;
					if (!prisoner)
					{
						pawnKindDef = PawnKindDefOf.SpaceRefugee;
					}
					else
					{
						pawnKindDef = (from pk in DefDatabase<PawnKindDef>.AllDefs
						where pk.defaultFactionType != null && !pk.defaultFactionType.isPlayer && pk.RaceProps.Humanlike
						select pk).RandomElement<PawnKindDef>();
					}
					Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction);
					GenSpawn.Spawn(pawn, building_Bed.Position, Find.CurrentMap, WipeMode.Vanish);
					foreach (ThingWithComps eq in pawn.equipment.AllEquipmentListForReading.ToList<ThingWithComps>())
					{
						ThingWithComps thingWithComps;
						if (pawn.equipment.TryDropEquipment(eq, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
					pawn.inventory.innerContainer.Clear();
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					pawn.guest.SetGuestStatus(Faction.OfPlayer, prisoner);
					break;
				}
			}
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x002BDB2C File Offset: 0x002BBF2C
		public static IEnumerable<float> PointsOptions(bool extended)
		{
			if (!extended)
			{
				yield return 35f;
				yield return 70f;
				yield return 100f;
				yield return 150f;
				yield return 200f;
				yield return 350f;
				yield return 500f;
				yield return 750f;
				yield return 1000f;
				yield return 1500f;
				yield return 2000f;
				yield return 3000f;
				yield return 4000f;
			}
			else
			{
				for (int i = 20; i < 100; i += 10)
				{
					yield return (float)i;
				}
				for (int j = 100; j < 500; j += 25)
				{
					yield return (float)j;
				}
				for (int k = 500; k < 1500; k += 50)
				{
					yield return (float)k;
				}
				for (int l = 1500; l <= 5000; l += 100)
				{
					yield return (float)l;
				}
			}
			yield break;
		}
	}
}
