using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> _003C_003Ef__mg_0024cache0;

		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		public Dialog_DebugActionsMenu()
		{
			base.forcePause = true;
		}

		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.ToggleDebugActionsMenu.KeyDownEvent)
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
				else if (Find.VisibleMap != null)
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

		private void DoListingItems_Entry()
		{
			base.DoLabel("Translation tools");
			base.DebugAction("Write backstory translation file", delegate
			{
				LanguageDataWriter.WriteBackstoryFile();
			});
			base.DebugAction("Output translation report", delegate
			{
				LanguageReportGenerator.OutputTranslationReport();
			});
		}

		private void DoListingItems_AllModePlayActions()
		{
			base.DoGap();
			base.DoLabel("Actions - Map management");
			base.DebugAction("Generate map", delegate
			{
				MapParent mapParent3 = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				mapParent3.Tile = TileFinder.RandomStartingTile();
				mapParent3.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(mapParent3);
				GetOrGenerateMapUtility.GetOrGenerateMap(mapParent3.Tile, new IntVec3(50, 1, 50), null);
			});
			base.DebugAction("Destroy map", delegate
			{
				List<DebugMenuOption> list4 = new List<DebugMenuOption>();
				List<Map> maps3 = Find.Maps;
				for (int l = 0; l < maps3.Count; l++)
				{
					Map map4 = maps3[l];
					list4.Add(new DebugMenuOption(map4.ToString(), DebugMenuOptionMode.Action, delegate
					{
						Current.Game.DeinitAndRemoveMap(map4);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
			});
			base.DebugToolMap("Transfer", delegate
			{
				List<Thing> toTransfer = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList();
				if (toTransfer.Any())
				{
					List<DebugMenuOption> list3 = new List<DebugMenuOption>();
					List<Map> maps2 = Find.Maps;
					for (int j = 0; j < maps2.Count; j++)
					{
						Map map2 = maps2[j];
						if (map2 != Find.VisibleMap)
						{
							list3.Add(new DebugMenuOption(map2.ToString(), DebugMenuOptionMode.Action, delegate
							{
								for (int k = 0; k < toTransfer.Count; k++)
								{
									IntVec3 center = map2.Center;
									Map map3 = map2;
									IntVec3 size2 = map2.Size;
									int x2 = size2.x;
									IntVec3 size3 = map2.Size;
									IntVec3 center2 = default(IntVec3);
									if (CellFinder.TryFindRandomCellNear(center, map3, Mathf.Max(x2, size3.z), (Predicate<IntVec3>)((IntVec3 x) => !x.Fogged(map2) && x.Standable(map2)), out center2))
									{
										toTransfer[k].DeSpawn();
										GenPlace.TryPlaceThing(toTransfer[k], center2, map2, ThingPlaceMode.Near, null);
									}
									else
									{
										Log.Error("Could not find spawn cell.");
									}
								}
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
				}
			});
			base.DebugAction("Change map", delegate
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Map map = maps[i];
					if (map != Find.VisibleMap)
					{
						list2.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate
						{
							Current.Game.VisibleMap = map;
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			});
			base.DebugAction("Regenerate visible map", delegate
			{
				RememberedCameraPos rememberedCameraPos = Find.VisibleMap.rememberedCameraPos;
				int tile3 = Find.VisibleMap.Tile;
				MapParent parent = Find.VisibleMap.info.parent;
				IntVec3 size = Find.VisibleMap.Size;
				Current.Game.DeinitAndRemoveMap(Find.VisibleMap);
				Map orGenerateMap2 = GetOrGenerateMapUtility.GetOrGenerateMap(tile3, size, parent.def);
				Current.Game.VisibleMap = orGenerateMap2;
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
			});
			base.DebugAction("Generate map with caves", delegate
			{
				int tile2 = TileFinder.RandomFactionBaseTileFor(Faction.OfPlayer, false, (int x) => Find.World.HasCaves(x));
				if (Find.VisibleMap != null)
				{
					Find.WorldObjects.Remove(Find.VisibleMap.info.parent);
				}
				MapParent mapParent2 = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				mapParent2.Tile = tile2;
				mapParent2.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(mapParent2);
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile2, Find.World.info.initialMapSize, null);
				Current.Game.VisibleMap = orGenerateMap;
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			});
			base.DebugAction("Run MapGenerator", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (MapGeneratorDef item in DefDatabase<MapGeneratorDef>.AllDefsListForReading)
				{
					list.Add(new DebugMenuOption(item.defName, DebugMenuOptionMode.Action, delegate
					{
						MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
						mapParent.Tile = (from tile in Enumerable.Range(0, Find.WorldGrid.TilesCount)
						where Find.WorldGrid[tile].biome.canBuildBase
						select tile).RandomElement();
						mapParent.SetFaction(Faction.OfPlayer);
						Find.WorldObjects.Add(mapParent);
						Map visibleMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, item, null, null);
						Current.Game.VisibleMap = visibleMap;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}

		private void DoListingItems_MapActions()
		{
			Text.Font = GameFont.Tiny;
			base.DoLabel("Incidents");
			this.DoExecuteIncidentDebugAction(Find.VisibleMap, null);
			this.DoExecuteIncidentWithDebugAction(Find.VisibleMap, null);
			Faction localFac;
			float localPoints;
			RaidStrategyDef localStrat;
			base.DebugAction("Execute raid with...", delegate
			{
				StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain);
				IncidentParms parms = storytellerComp.GenerateParms(IncidentCategory.ThreatBig, Find.VisibleMap);
				List<DebugMenuOption> list8 = new List<DebugMenuOption>();
				foreach (Faction allFaction in Find.FactionManager.AllFactions)
				{
					localFac = allFaction;
					list8.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, delegate
					{
						parms.faction = localFac;
						List<DebugMenuOption> list9 = new List<DebugMenuOption>();
						foreach (float item in Dialog_DebugActionsMenu.PointsOptions())
						{
							localPoints = item;
							list9.Add(new DebugMenuOption(item + " points", DebugMenuOptionMode.Action, delegate
							{
								parms.points = localPoints;
								List<DebugMenuOption> list10 = new List<DebugMenuOption>();
								foreach (RaidStrategyDef allDef in DefDatabase<RaidStrategyDef>.AllDefs)
								{
									localStrat = allDef;
									string text = localStrat.defName;
									if (!localStrat.Worker.CanUseWith(parms))
									{
										text += " [NO]";
									}
									list10.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
									{
										parms.raidStrategy = localStrat;
										this.DoRaid(parms);
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list10));
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list9));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list8));
			});
			Action<int> DoRandomEnemyRaid = delegate(int pts)
			{
				this.Close(true);
				IncidentParms incidentParms2 = new IncidentParms();
				incidentParms2.target = Find.VisibleMap;
				incidentParms2.points = (float)pts;
				IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms2);
			};
			base.DebugAction("Raid (35 pts)", delegate
			{
				DoRandomEnemyRaid(35);
			});
			base.DebugAction("Raid (75 pts)", delegate
			{
				DoRandomEnemyRaid(75);
			});
			base.DebugAction("Raid (300 pts)", delegate
			{
				DoRandomEnemyRaid(300);
			});
			base.DebugAction("Raid (400 pts)", delegate
			{
				DoRandomEnemyRaid(400);
			});
			base.DebugAction("Raid  (1000 pts)", delegate
			{
				DoRandomEnemyRaid(1000);
			});
			base.DebugAction("Raid  (3000 pts)", delegate
			{
				DoRandomEnemyRaid(3000);
			});
			base.DoGap();
			base.DoLabel("Actions - Misc");
			WeatherDef localWeather;
			base.DebugAction("Change weather...", delegate
			{
				List<DebugMenuOption> list7 = new List<DebugMenuOption>();
				foreach (WeatherDef allDef2 in DefDatabase<WeatherDef>.AllDefs)
				{
					localWeather = allDef2;
					list7.Add(new DebugMenuOption(localWeather.LabelCap, DebugMenuOptionMode.Action, delegate
					{
						Find.VisibleMap.weatherManager.TransitionTo(localWeather);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list7));
			});
			SongDef localSong;
			base.DebugAction("Play song...", delegate
			{
				List<DebugMenuOption> list6 = new List<DebugMenuOption>();
				foreach (SongDef allDef3 in DefDatabase<SongDef>.AllDefs)
				{
					localSong = allDef3;
					list6.Add(new DebugMenuOption(localSong.defName, DebugMenuOptionMode.Action, delegate
					{
						Find.MusicManagerPlay.ForceStartSong(localSong, false);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list6));
			});
			SoundDef localSd;
			base.DebugAction("Play sound...", delegate
			{
				List<DebugMenuOption> list5 = new List<DebugMenuOption>();
				foreach (SoundDef item2 in from s in DefDatabase<SoundDef>.AllDefs
				where !s.sustain
				select s)
				{
					localSd = item2;
					list5.Add(new DebugMenuOption(localSd.defName, DebugMenuOptionMode.Action, delegate
					{
						localSd.PlayOneShotOnCamera(null);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list5));
			});
			if (Find.VisibleMap.gameConditionManager.ActiveConditions.Count > 0)
			{
				GameCondition localMc;
				base.DebugAction("End game condition ...", delegate
				{
					List<DebugMenuOption> list4 = new List<DebugMenuOption>();
					foreach (GameCondition activeCondition in Find.VisibleMap.gameConditionManager.ActiveConditions)
					{
						localMc = activeCondition;
						list4.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, delegate
						{
							localMc.End();
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
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
				foreach (Lord lord in Find.VisibleMap.lordManager.lords)
				{
					LordToil_Stage lordToil_Stage = lord.CurLordToil as LordToil_Stage;
					if (lordToil_Stage != null)
					{
						foreach (Transition transition in lord.Graph.transitions)
						{
							if (transition.sources.Contains(lordToil_Stage) && transition.target is LordToil_AssaultColony)
							{
								Messages.Message("Debug forcing to assault toil: " + lord.faction, MessageTypeDefOf.TaskCompletion);
								lord.GotoToil(transition.target);
								return;
							}
						}
					}
				}
			});
			base.DebugAction("Force enemy flee", delegate
			{
				foreach (Lord lord2 in Find.VisibleMap.lordManager.lords)
				{
					if (lord2.faction != null && lord2.faction.HostileTo(Faction.OfPlayer) && lord2.faction.def.autoFlee)
					{
						LordToil lordToil = lord2.Graph.lordToils.FirstOrDefault((LordToil st) => st is LordToil_PanicFlee);
						if (lordToil != null)
						{
							lord2.GotoToil(lordToil);
						}
					}
				}
			});
			base.DebugAction("Destroy all things", delegate
			{
				foreach (Thing item3 in Find.VisibleMap.listerThings.AllThings.ToList())
				{
					item3.Destroy(DestroyMode.Vanish);
				}
			});
			base.DebugAction("Destroy all plants", delegate
			{
				foreach (Thing item4 in Find.VisibleMap.listerThings.AllThings.ToList())
				{
					if (item4 is Plant)
					{
						item4.Destroy(DestroyMode.Vanish);
					}
				}
			});
			base.DebugAction("Unload unused assets", delegate
			{
				MemoryUtility.UnloadUnusedUnityAssets();
			});
			base.DebugAction("Name colony...", delegate
			{
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				list3.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, delegate
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFaction());
				}));
				if (Find.VisibleMap != null && Find.VisibleMap.IsPlayerHome)
				{
					FactionBase factionBase = (FactionBase)Find.VisibleMap.info.parent;
					list3.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, delegate
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionBase(factionBase));
					}));
					list3.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, delegate
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionAndBase(factionBase));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
			});
			base.DebugAction("Next lesson", delegate
			{
				LessonAutoActivator.DebugForceInitiateBestLessonNow();
			});
			base.DebugAction("Regen all map mesh sections", delegate
			{
				Find.VisibleMap.mapDrawer.RegenerateEverythingNow();
			});
			base.DebugAction("Finish all research", delegate
			{
				Find.ResearchManager.DebugSetAllProjectsFinished();
				Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion);
			});
			base.DebugAction("Replace all trade ships", delegate
			{
				Find.VisibleMap.passingShipManager.DebugSendAllShipsAway();
				for (int i = 0; i < 5; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.VisibleMap;
					IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
				}
			});
			Type localType;
			base.DebugAction("Change camera config...", delegate
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				foreach (Type item5 in typeof(CameraMapConfig).AllSubclasses())
				{
					localType = item5;
					list2.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate
					{
						Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			});
			base.DebugAction("Force ship countdown", delegate
			{
				ShipCountdown.InitiateCountdown(null);
			});
			base.DebugAction("Flash trade drop spot", delegate
			{
				IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.VisibleMap);
				Find.VisibleMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
				Log.Message("trade drop spot: " + intVec);
			});
			base.DebugAction("Kill faction leader", delegate
			{
				Pawn leader = (from x in Find.FactionManager.AllFactions
				where x.leader != null
				select x).RandomElement().leader;
				int num = 0;
				while (true)
				{
					if (!leader.Dead)
					{
						if (++num <= 1000)
						{
							leader.TakeDamage(new DamageInfo(DamageDefOf.Bullet, 30, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
							continue;
						}
						break;
					}
					return;
				}
				Log.Warning("Could not kill faction leader.");
			});
			base.DebugAction("Refog map", delegate
			{
				FloodFillerFog.DebugRefogMap(Find.VisibleMap);
			});
			Type localGenStep;
			base.DebugAction("Use GenStep", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (Type item6 in typeof(GenStep).AllSubclassesNonAbstract())
				{
					localGenStep = item6;
					list.Add(new DebugMenuOption(localGenStep.Name, DebugMenuOptionMode.Action, delegate
					{
						GenStep genStep = (GenStep)Activator.CreateInstance(localGenStep);
						genStep.Generate(Find.VisibleMap);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
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

		private void DoListingItems_MapTools()
		{
			base.DoGap();
			base.DoLabel("Tools - General");
			base.DebugToolMap("Tool: Destroy", delegate
			{
				foreach (Thing item2 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
				{
					item2.Destroy(DestroyMode.Vanish);
				}
			});
			base.DebugToolMap("Tool: Kill", delegate
			{
				foreach (Thing item3 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
				{
					item3.Kill(null, null);
				}
			});
			base.DebugToolMap("Tool: 10 damage", delegate
			{
				foreach (Thing item4 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
				{
					item4.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			});
			base.DebugToolMap("Tool: 5000 damage", delegate
			{
				foreach (Thing item5 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
				{
					item5.TakeDamage(new DamageInfo(DamageDefOf.Crush, 5000, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			});
			base.DebugToolMap("Tool: 5000 flame damage", delegate
			{
				foreach (Thing item6 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
				{
					item6.TakeDamage(new DamageInfo(DamageDefOf.Flame, 5000, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			});
			base.DebugToolMap("Tool: Clear area 21x21", delegate
			{
				CellRect r = CellRect.CenteredOn(UI.MouseCell(), 10);
				GenDebug.ClearArea(r, Find.VisibleMap);
			});
			base.DebugToolMap("Tool: Rock 21x21", delegate
			{
				CellRect cellRect = CellRect.CenteredOn(UI.MouseCell(), 10);
				cellRect.ClipInsideMap(Find.VisibleMap);
				ThingDef granite = ThingDefOf.Granite;
				foreach (IntVec3 item7 in cellRect)
				{
					GenSpawn.Spawn(granite, item7, Find.VisibleMap);
				}
			});
			base.DoGap();
			base.DebugToolMap("Tool: Explosion (bomb)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.Bomb, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("Tool: Explosion (flame)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.Flame, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("Tool: Explosion (stun)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.Stun, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("Tool: Explosion (EMP)", delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.EMP, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("Tool: Explosion (extinguisher)", delegate
			{
				IntVec3 center2 = UI.MouseCell();
				Map visibleMap2 = Find.VisibleMap;
				float radius2 = 10f;
				DamageDef extinguish = DamageDefOf.Extinguish;
				Thing instigator2 = null;
				ThingDef filthFireFoam = ThingDefOf.FilthFireFoam;
				GenExplosion.DoExplosion(center2, visibleMap2, radius2, extinguish, instigator2, -1, null, null, null, filthFireFoam, 1f, 3, true, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("Tool: Explosion (smoke)", delegate
			{
				IntVec3 center = UI.MouseCell();
				Map visibleMap = Find.VisibleMap;
				float radius = 10f;
				DamageDef smoke = DamageDefOf.Smoke;
				Thing instigator = null;
				ThingDef gas_Smoke = ThingDefOf.Gas_Smoke;
				GenExplosion.DoExplosion(center, visibleMap, radius, smoke, instigator, -1, null, null, null, gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false);
			});
			base.DebugToolMap("Tool: Lightning strike", delegate
			{
				Find.VisibleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.VisibleMap, UI.MouseCell()));
			});
			base.DoGap();
			base.DebugToolMap("Tool: Add snow", delegate
			{
				SnowUtility.AddSnowRadial(UI.MouseCell(), Find.VisibleMap, 5f, 1f);
			});
			base.DebugToolMap("Tool: Remove snow", delegate
			{
				SnowUtility.AddSnowRadial(UI.MouseCell(), Find.VisibleMap, 5f, -1f);
			});
			base.DebugAction("Clear all snow", delegate
			{
				foreach (IntVec3 allCell in Find.VisibleMap.AllCells)
				{
					Find.VisibleMap.snowGrid.SetDepth(allCell, 0f);
				}
			});
			base.DebugToolMap("Tool: Push heat (10)", delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.VisibleMap, 10f);
			});
			base.DebugToolMap("Tool: Push heat (10000)", delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.VisibleMap, 10000f);
			});
			base.DebugToolMap("Tool: Push heat (-1000)", delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.VisibleMap, -1000f);
			});
			base.DoGap();
			base.DebugToolMap("Tool: Finish plant growth", delegate
			{
				foreach (Thing item8 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					Plant plant6 = item8 as Plant;
					if (plant6 != null)
					{
						plant6.Growth = 1f;
					}
				}
			});
			base.DebugToolMap("Tool: Grow 1 day", delegate
			{
				IntVec3 intVec5 = UI.MouseCell();
				Plant plant5 = intVec5.GetPlant(Find.VisibleMap);
				if (plant5 != null && plant5.def.plant != null)
				{
					int num20 = (int)((1.0 - plant5.Growth) * plant5.def.plant.growDays);
					if (num20 >= 60000)
					{
						plant5.Age += 60000;
					}
					else if (num20 > 0)
					{
						plant5.Age += num20;
					}
					plant5.Growth += (float)(1.0 / plant5.def.plant.growDays);
					if ((double)plant5.Growth > 1.0)
					{
						plant5.Growth = 1f;
					}
					Find.VisibleMap.mapDrawer.SectionAt(intVec5).RegenerateAllLayers();
				}
			});
			base.DebugToolMap("Tool: Grow to maturity", delegate
			{
				IntVec3 intVec4 = UI.MouseCell();
				Plant plant4 = intVec4.GetPlant(Find.VisibleMap);
				if (plant4 != null && plant4.def.plant != null)
				{
					int num19 = (int)((1.0 - plant4.Growth) * plant4.def.plant.growDays);
					plant4.Age += num19;
					plant4.Growth = 1f;
					Find.VisibleMap.mapDrawer.SectionAt(intVec4).RegenerateAllLayers();
				}
			});
			base.DebugToolMap("Tool: Reproduce present plant", delegate
			{
				IntVec3 c7 = UI.MouseCell();
				Plant plant2 = c7.GetPlant(Find.VisibleMap);
				if (plant2 != null && plant2.def.plant != null)
				{
					Plant plant3 = GenPlantReproduction.TryReproduceFrom(plant2.Position, plant2.def, SeedTargFindMode.Reproduce, plant2.Map);
					if (plant3 != null)
					{
						Find.VisibleMap.debugDrawer.FlashCell(plant3.Position, 0f, null, 50);
						Find.VisibleMap.debugDrawer.FlashLine(plant2.Position, plant3.Position, 50);
					}
					else
					{
						Find.VisibleMap.debugDrawer.FlashCell(plant2.Position, 0f, null, 50);
					}
				}
			});
			ThingDef localDef6;
			base.DebugToolMap("Tool: Reproduce plant...", delegate
			{
				List<FloatMenuOption> list33 = new List<FloatMenuOption>();
				foreach (ThingDef item9 in from d in DefDatabase<ThingDef>.AllDefs
				where d.category == ThingCategory.Plant && d.plant.reproduces
				select d)
				{
					localDef6 = item9;
					list33.Add(new FloatMenuOption(localDef6.LabelCap, delegate
					{
						Plant plant = GenPlantReproduction.TryReproduceFrom(UI.MouseCell(), localDef6, SeedTargFindMode.Reproduce, Find.VisibleMap);
						if (plant != null)
						{
							Find.VisibleMap.debugDrawer.FlashCell(plant.Position, 0f, null, 50);
							Find.VisibleMap.debugDrawer.FlashLine(UI.MouseCell(), plant.Position, 50);
						}
						else
						{
							Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, null, 50);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list33));
			});
			base.DoGap();
			base.DebugToolMap("Tool: Regen section", delegate
			{
				Find.VisibleMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
			});
			base.DebugToolMap("Tool: Randomize color", delegate
			{
				foreach (Thing item10 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompColorable compColorable = item10.TryGetComp<CompColorable>();
					if (compColorable != null)
					{
						item10.SetColor(GenColor.RandomColorOpaque(), true);
					}
				}
			});
			base.DebugToolMap("Tool: Rot 1 day", delegate
			{
				foreach (Thing item11 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompRottable compRottable = item11.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += 60000f;
					}
				}
			});
			base.DebugToolMap("Tool: Fuel -20%", delegate
			{
				foreach (Thing item12 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompRefuelable compRefuelable = item12.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel((float)(compRefuelable.Props.fuelCapacity * 0.20000000298023224));
					}
				}
			});
			base.DebugToolMap("Tool: Break down...", delegate
			{
				foreach (Thing item13 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompBreakdownable compBreakdownable = item13.TryGetComp<CompBreakdownable>();
					if (compBreakdownable != null && !compBreakdownable.BrokenDown)
					{
						compBreakdownable.DoBreakdown();
					}
				}
			});
			base.DebugAction("Tool: Use scatterer", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_MapGen.Options_Scatterers()));
			});
			string localSymbol;
			base.DebugAction("Tool: BaseGen", delegate
			{
				List<DebugMenuOption> list32 = new List<DebugMenuOption>();
				foreach (string item14 in (from x in DefDatabase<RuleDef>.AllDefs
				select x.symbol).Distinct())
				{
					localSymbol = item14;
					list32.Add(new DebugMenuOption(item14, DebugMenuOptionMode.Action, delegate
					{
						DebugTool tool3 = null;
						IntVec3 firstCorner2;
						tool3 = new DebugTool("first corner...", delegate
						{
							firstCorner2 = UI.MouseCell();
							DebugTools.curTool = new DebugTool("second corner...", delegate
							{
								IntVec3 second2 = UI.MouseCell();
								CellRect rect = CellRect.FromLimits(firstCorner2, second2).ClipInsideMap(Find.VisibleMap);
								BaseGen.globalSettings.map = Find.VisibleMap;
								BaseGen.symbolStack.Push(localSymbol, rect);
								BaseGen.Generate();
								DebugTools.curTool = tool3;
							}, firstCorner2);
						}, null);
						DebugTools.curTool = tool3;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list32));
			});
			base.DebugToolMap("Tool: Make roof", delegate
			{
				CellRect.CellRectIterator iterator2 = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
				while (!iterator2.Done())
				{
					Find.VisibleMap.roofGrid.SetRoof(iterator2.Current, RoofDefOf.RoofConstructed);
					iterator2.MoveNext();
				}
			});
			base.DebugToolMap("Tool: Delete roof", delegate
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
				while (!iterator.Done())
				{
					Find.VisibleMap.roofGrid.SetRoof(iterator.Current, null);
					iterator.MoveNext();
				}
			});
			base.DebugToolMap("Tool: Toggle trap status", delegate
			{
				foreach (Thing item15 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
				{
					Building_Trap building_Trap = item15 as Building_Trap;
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
			base.DebugToolMap("Tool: Add trap memory", delegate
			{
				foreach (Faction allFaction in Find.World.factionManager.AllFactions)
				{
					allFaction.TacticalMemory.TrapRevealed(UI.MouseCell(), Find.VisibleMap);
				}
				Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "added", 50);
			});
			base.DebugToolMap("Tool: Test flood unfog", delegate
			{
				FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.VisibleMap);
			});
			base.DebugToolMap("Tool: Flash closewalk cell 30", delegate
			{
				IntVec3 c6 = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.VisibleMap, 30, null);
				Find.VisibleMap.debugDrawer.FlashCell(c6, 0f, null, 50);
			});
			base.DebugToolMap("Tool: Flash walk path", delegate
			{
				WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
			});
			base.DebugToolMap("Tool: Flash skygaze cell", delegate
			{
				Pawn pawn10 = Find.VisibleMap.mapPawns.FreeColonists.First();
				IntVec3 c5 = default(IntVec3);
				RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn10, out c5);
				Find.VisibleMap.debugDrawer.FlashCell(c5, 0f, null, 50);
				MoteMaker.ThrowText(c5.ToVector3Shifted(), Find.VisibleMap, "for " + pawn10.Label, Color.white, -1f);
			});
			base.DebugToolMap("Tool: Flash direct flee dest", delegate
			{
				Pawn pawn9 = Find.Selector.SingleSelectedThing as Pawn;
				IntVec3 c4 = default(IntVec3);
				if (pawn9 == null)
				{
					Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "select a pawn", 50);
				}
				else if (RCellFinder.TryFindDirectFleeDestination(UI.MouseCell(), 9f, pawn9, out c4))
				{
					Find.VisibleMap.debugDrawer.FlashCell(c4, 0.5f, null, 50);
				}
				else
				{
					Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0.8f, "not found", 50);
				}
			});
			base.DebugAction("Tool: Flash spectators cells", delegate
			{
				Action<bool> act4 = delegate(bool bestSideOnly)
				{
					DebugTool tool2 = null;
					IntVec3 firstCorner;
					tool2 = new DebugTool("first watch rect corner...", delegate
					{
						firstCorner = UI.MouseCell();
						DebugTools.curTool = new DebugTool("second watch rect corner...", delegate
						{
							IntVec3 second = UI.MouseCell();
							CellRect spectateRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.VisibleMap);
							SpectateRectSide allowedSides = SpectateRectSide.All;
							if (bestSideOnly)
							{
								allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.VisibleMap, SpectateRectSide.All, 1);
							}
							SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.VisibleMap, allowedSides, 1);
							DebugTools.curTool = tool2;
						}, firstCorner);
					}, null);
					DebugTools.curTool = tool2;
				};
				List<DebugMenuOption> list31 = new List<DebugMenuOption>();
				list31.Add(new DebugMenuOption("All sides", DebugMenuOptionMode.Action, delegate
				{
					act4(false);
				}));
				list31.Add(new DebugMenuOption("Best side only", DebugMenuOptionMode.Action, delegate
				{
					act4(true);
				}));
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list31));
			});
			base.DebugAction("Tool: Check reachability", delegate
			{
				List<DebugMenuOption> list30 = new List<DebugMenuOption>();
				TraverseMode[] array3 = (TraverseMode[])Enum.GetValues(typeof(TraverseMode));
				for (int num18 = 0; num18 < array3.Length; num18++)
				{
					TraverseMode traverseMode2 = array3[num18];
					TraverseMode traverseMode = traverseMode2;
					list30.Add(new DebugMenuOption(traverseMode2.ToString(), DebugMenuOptionMode.Action, delegate
					{
						DebugTool tool = null;
						IntVec3 from;
						Pawn fromPawn;
						tool = new DebugTool("from...", delegate
						{
							from = UI.MouseCell();
							fromPawn = from.GetFirstPawn(Find.VisibleMap);
							string text4 = "to...";
							if (fromPawn != null)
							{
								text4 = text4 + " (pawn=" + fromPawn.LabelShort + ")";
							}
							DebugTools.curTool = new DebugTool(text4, delegate
							{
								DebugTools.curTool = tool;
							}, delegate
							{
								IntVec3 c3 = UI.MouseCell();
								bool flag2;
								IntVec3 intVec3;
								if (fromPawn != null)
								{
									Pawn pawn8 = fromPawn;
									LocalTargetInfo dest = c3;
									PathEndMode peMode = PathEndMode.OnCell;
									Danger maxDanger = Danger.Deadly;
									TraverseMode mode = traverseMode;
									flag2 = pawn8.CanReach(dest, peMode, maxDanger, false, mode);
									intVec3 = fromPawn.Position;
								}
								else
								{
									flag2 = Find.VisibleMap.reachability.CanReach(from, c3, PathEndMode.OnCell, traverseMode, Danger.Deadly);
									intVec3 = from;
								}
								Color color = (!flag2) ? Color.red : Color.green;
								Widgets.DrawLine(intVec3.ToUIPosition(), c3.ToUIPosition(), color, 2f);
							});
						}, null);
						DebugTools.curTool = tool;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list30));
			});
			base.DebugToolMapForPawns("Tool: Flash TryFindRandomPawnExitCell", delegate(Pawn p)
			{
				IntVec3 intVec2 = default(IntVec3);
				if (CellFinder.TryFindRandomPawnExitCell(p, out intVec2))
				{
					p.Map.debugDrawer.FlashCell(intVec2, 0.5f, null, 50);
					p.Map.debugDrawer.FlashLine(p.Position, intVec2, 50);
				}
				else
				{
					p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no exit cell", 50);
				}
			});
			base.DebugToolMapForPawns("Tool: RandomSpotJustOutsideColony", delegate(Pawn p)
			{
				IntVec3 intVec = default(IntVec3);
				if (RCellFinder.TryFindRandomSpotJustOutsideColony(p, out intVec))
				{
					p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
					p.Map.debugDrawer.FlashLine(p.Position, intVec, 50);
				}
				else
				{
					p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no cell", 50);
				}
			});
			base.DoGap();
			base.DoLabel("Tools - Pawns");
			base.DebugToolMap("Tool: Resurrect", delegate
			{
				foreach (Thing item16 in UI.MouseCell().GetThingList(Find.VisibleMap).ToList())
				{
					Corpse corpse = item16 as Corpse;
					if (corpse != null)
					{
						ResurrectionUtility.Resurrect(corpse.InnerPawn);
					}
				}
			});
			base.DebugToolMapForPawns("Tool: Damage to down", delegate(Pawn p)
			{
				HealthUtility.DamageUntilDowned(p);
			});
			base.DebugToolMapForPawns("Tool: Damage legs", delegate(Pawn p)
			{
				HealthUtility.DamageLegsUntilIncapableOfMoving(p);
			});
			base.DebugToolMapForPawns("Tool: Damage to death", delegate(Pawn p)
			{
				HealthUtility.DamageUntilDead(p);
			});
			base.DebugToolMap("Tool: Damage held pawn to death", delegate
			{
				foreach (Thing item17 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList())
				{
					Pawn pawn7 = item17 as Pawn;
					if (pawn7 != null && pawn7.carryTracker.CarriedThing != null && pawn7.carryTracker.CarriedThing is Pawn)
					{
						HealthUtility.DamageUntilDead((Pawn)pawn7.carryTracker.CarriedThing);
					}
				}
			});
			base.DebugToolMapForPawns("Tool: Surgery fail minor", delegate(Pawn p)
			{
				BodyPartRecord bodyPartRecord2 = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
				where !x.def.isConceptual
				select x).RandomElement();
				Log.Message("part is " + bodyPartRecord2);
				HealthUtility.GiveInjuriesOperationFailureMinor(p, bodyPartRecord2);
			});
			base.DebugToolMapForPawns("Tool: Surgery fail catastrophic", delegate(Pawn p)
			{
				BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
				where !x.def.isConceptual
				select x).RandomElement();
				Log.Message("part is " + bodyPartRecord);
				HealthUtility.GiveInjuriesOperationFailureCatastrophic(p, bodyPartRecord);
			});
			base.DebugToolMapForPawns("Tool: Surgery fail ridiculous", delegate(Pawn p)
			{
				HealthUtility.GiveInjuriesOperationFailureRidiculous(p);
			});
			base.DebugToolMapForPawns("Tool: Restore body part...", delegate(Pawn p)
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
			});
			base.DebugAction("Tool: Apply damage...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_ApplyDamage()));
			});
			base.DebugAction("Tool: Add Hediff...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff()));
			});
			base.DebugToolMapForPawns("Tool: Remove Hediff...", delegate(Pawn p)
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RemoveHediff(p)));
			});
			base.DebugToolMapForPawns("Tool: Heal random injury (10)", delegate(Pawn p)
			{
				Hediff_Injury hediff_Injury = default(Hediff_Injury);
				if ((from x in p.health.hediffSet.GetHediffs<Hediff_Injury>()
				where x.CanHealNaturally() || x.CanHealFromTending()
				select x).TryRandomElement<Hediff_Injury>(out hediff_Injury))
				{
					hediff_Injury.Heal(10f);
				}
			});
			HediffGiver localHdg;
			base.DebugToolMapForPawns("Tool: Activate HediffGiver", delegate(Pawn p)
			{
				Dialog_DebugActionsMenu dialog_DebugActionsMenu5 = this;
				List<FloatMenuOption> list29 = new List<FloatMenuOption>();
				if (p.RaceProps.hediffGiverSets != null)
				{
					foreach (HediffGiver item18 in p.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef set) => set.hediffGivers))
					{
						localHdg = item18;
						list29.Add(new FloatMenuOption(localHdg.hediff.defName, delegate
						{
							localHdg.TryApply(p, null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				if (list29.Any())
				{
					Find.WindowStack.Add(new FloatMenu(list29));
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Grant immunities", delegate(Pawn p)
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
			base.DebugToolMapForPawns("Tool: Give birth", delegate(Pawn p)
			{
				Hediff_Pregnant.DoBirthSpawn(p, null);
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("Tool: List melee verbs", delegate(Pawn p)
			{
				Log.Message(string.Format("Verb list:\n  {0}", GenText.ToTextList(from verb in p.meleeVerbs.GetUpdatedAvailableVerbsList()
				select verb.ToString(), "\n  ")));
			});
			PawnRelationDef defLocal;
			Pawn otherLocal3;
			base.DebugToolMapForPawns("Tool: Add/remove pawn relation", delegate(Pawn p)
			{
				if (p.RaceProps.IsFlesh)
				{
					Action<bool> act3 = delegate(bool add)
					{
						if (add)
						{
							List<DebugMenuOption> list26 = new List<DebugMenuOption>();
							foreach (PawnRelationDef allDef in DefDatabase<PawnRelationDef>.AllDefs)
							{
								if (!allDef.implied)
								{
									defLocal = allDef;
									list26.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate
									{
										List<DebugMenuOption> list28 = new List<DebugMenuOption>();
										IOrderedEnumerable<Pawn> orderedEnumerable = (from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
										where x.RaceProps.IsFlesh
										orderby x.def == p.def descending
										select x).ThenBy(WorldPawnsUtility.IsWorldPawn);
										foreach (Pawn item19 in orderedEnumerable)
										{
											if (p != item19 && (!defLocal.familyByBloodRelation || item19.def == p.def) && !p.relations.DirectRelationExists(defLocal, item19))
											{
												otherLocal3 = item19;
												list28.Add(new DebugMenuOption(otherLocal3.LabelShort + " (" + otherLocal3.KindLabel + ")", DebugMenuOptionMode.Action, delegate
												{
													p.relations.AddDirectRelation(defLocal, otherLocal3);
												}));
											}
										}
										Find.WindowStack.Add(new Dialog_DebugOptionListLister(list28));
									}));
								}
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list26));
						}
						else
						{
							List<DebugMenuOption> list27 = new List<DebugMenuOption>();
							List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
							for (int num17 = 0; num17 < directRelations.Count; num17++)
							{
								DirectPawnRelation rel = directRelations[num17];
								list27.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate
								{
									p.relations.RemoveDirectRelation(rel);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list27));
						}
					};
					List<DebugMenuOption> list25 = new List<DebugMenuOption>();
					list25.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, delegate
					{
						act3(true);
					}));
					list25.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate
					{
						act3(false);
					}));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list25));
				}
			});
			base.DebugToolMapForPawns("Tool: Add opinion thoughts about", delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike)
				{
					Action<bool> act2 = delegate(bool good)
					{
						foreach (Pawn item20 in from x in p.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike
						select x)
						{
							if (p != item20)
							{
								IEnumerable<ThoughtDef> source3 = DefDatabase<ThoughtDef>.AllDefs.Where((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0.0) || (!good && x.stages[0].baseOpinionOffset < 0.0)));
								if (source3.Any())
								{
									int num15 = Rand.Range(2, 5);
									for (int num16 = 0; num16 < num15; num16++)
									{
										ThoughtDef def3 = source3.RandomElement();
										item20.needs.mood.thoughts.memories.TryGainMemory(def3, p);
									}
								}
							}
						}
					};
					List<DebugMenuOption> list24 = new List<DebugMenuOption>();
					list24.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, delegate
					{
						act2(true);
					}));
					list24.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, delegate
					{
						act2(false);
					}));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list24));
				}
			});
			base.DebugToolMapForPawns("Tool: Force vomit...", delegate(Pawn p)
			{
				p.jobs.StartJob(new Job(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false);
			});
			base.DebugToolMap("Tool: Food -20%", delegate
			{
				this.OffsetNeed(NeedDefOf.Food, -0.2f);
			});
			base.DebugToolMap("Tool: Rest -20%", delegate
			{
				this.OffsetNeed(NeedDefOf.Rest, -0.2f);
			});
			base.DebugToolMap("Tool: Joy -20%", delegate
			{
				this.OffsetNeed(NeedDefOf.Joy, -0.2f);
			});
			base.DebugToolMap("Tool: Chemical -20%", delegate
			{
				List<NeedDef> allDefsListForReading3 = DefDatabase<NeedDef>.AllDefsListForReading;
				for (int num14 = 0; num14 < allDefsListForReading3.Count; num14++)
				{
					if (typeof(Need_Chemical).IsAssignableFrom(allDefsListForReading3[num14].needClass))
					{
						this.OffsetNeed(allDefsListForReading3[num14], -0.2f);
					}
				}
			});
			SkillDef localDef5;
			base.DebugToolMapForPawns("Tool: Set skill", delegate(Pawn p)
			{
				if (p.skills != null)
				{
					List<DebugMenuOption> list22 = new List<DebugMenuOption>();
					foreach (SkillDef allDef2 in DefDatabase<SkillDef>.AllDefs)
					{
						localDef5 = allDef2;
						list22.Add(new DebugMenuOption(localDef5.defName, DebugMenuOptionMode.Action, delegate
						{
							List<DebugMenuOption> list23 = new List<DebugMenuOption>();
							for (int num13 = 0; num13 <= 20; num13++)
							{
								int level = num13;
								list23.Add(new DebugMenuOption(level.ToString(), DebugMenuOptionMode.Action, delegate
								{
									SkillRecord skill = p.skills.GetSkill(localDef5);
									skill.Level = level;
									skill.xpSinceLastLevel = (float)(skill.XpRequiredForLevelUp / 2.0);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list23));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list22));
				}
			});
			base.DebugToolMapForPawns("Tool: Max skills", delegate(Pawn p)
			{
				if (p.skills != null)
				{
					foreach (SkillDef allDef3 in DefDatabase<SkillDef>.AllDefs)
					{
						p.skills.Learn(allDef3, 1E+08f, false);
					}
					this.DustPuffFrom(p);
				}
				if (p.training != null)
				{
					foreach (TrainableDef allDef4 in DefDatabase<TrainableDef>.AllDefs)
					{
						Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement();
						bool flag = default(bool);
						if (p.training.CanAssignToTrain(allDef4, out flag).Accepted)
						{
							p.training.Train(allDef4, trainer);
						}
					}
				}
			});
			Dialog_DebugActionsMenu dialog_DebugActionsMenu4;
			MentalBreakDef locBrDef2;
			base.DebugAction("Tool: Mental break...", delegate
			{
				List<DebugMenuOption> list21 = new List<DebugMenuOption>();
				list21.Add(new DebugMenuOption("(log possibles)", DebugMenuOptionMode.Tool, delegate
				{
					foreach (Pawn item21 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						item21.mindState.mentalBreaker.LogPossibleMentalBreaks();
						this.DustPuffFrom(item21);
					}
				}));
				list21.Add(new DebugMenuOption("(natural mood break)", DebugMenuOptionMode.Tool, delegate
				{
					foreach (Pawn item22 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						item22.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
						this.DustPuffFrom(item22);
					}
				}));
				foreach (MentalBreakDef item23 in from x in DefDatabase<MentalBreakDef>.AllDefs
				orderby x.intensity descending
				select x)
				{
					dialog_DebugActionsMenu4 = this;
					locBrDef2 = item23;
					string text3 = locBrDef2.defName;
					if (!Find.VisibleMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef2.Worker.BreakCanOccur(x)))
					{
						text3 += " [NO]";
					}
					list21.Add(new DebugMenuOption(text3, DebugMenuOptionMode.Tool, delegate
					{
						foreach (Pawn item24 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>())
						{
							locBrDef2.Worker.TryStart(item24, null, false);
							dialog_DebugActionsMenu4.DustPuffFrom(item24);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list21));
			});
			Dialog_DebugActionsMenu dialog_DebugActionsMenu3;
			MentalStateDef locBrDef;
			Pawn locP;
			base.DebugAction("Tool: Mental state...", delegate
			{
				List<DebugMenuOption> list20 = new List<DebugMenuOption>();
				foreach (MentalStateDef allDef5 in DefDatabase<MentalStateDef>.AllDefs)
				{
					dialog_DebugActionsMenu3 = this;
					locBrDef = allDef5;
					string text2 = locBrDef.defName;
					if (!Find.VisibleMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef.Worker.StateCanOccur(x)))
					{
						text2 += " [NO]";
					}
					list20.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Tool, delegate
					{
						foreach (Pawn item25 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							locP = item25;
							if (locBrDef != MentalStateDefOf.SocialFighting)
							{
								locP.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, true, false, null);
								dialog_DebugActionsMenu3.DustPuffFrom(locP);
							}
							else
							{
								DebugTools.curTool = new DebugTool("...with", delegate
								{
									Pawn pawn6 = (Pawn)(from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
									where t is Pawn
									select t).FirstOrDefault();
									if (pawn6 != null)
									{
										if (!InteractionUtility.HasAnySocialFightProvokingThought(locP, pawn6))
										{
											locP.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Insulted, pawn6);
											Messages.Message("Dev: auto added negative thought.", locP, MessageTypeDefOf.TaskCompletion);
										}
										locP.interactions.StartSocialFight(pawn6);
										DebugTools.curTool = null;
									}
								}, null);
							}
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list20));
			});
			Dialog_DebugActionsMenu dialog_DebugActionsMenu2;
			InspirationDef localDef4;
			base.DebugAction("Tool: Inspiration...", delegate
			{
				List<DebugMenuOption> list19 = new List<DebugMenuOption>();
				foreach (InspirationDef allDef6 in DefDatabase<InspirationDef>.AllDefs)
				{
					dialog_DebugActionsMenu2 = this;
					localDef4 = allDef6;
					string text = localDef4.defName;
					if (!Find.VisibleMap.mapPawns.FreeColonists.Any((Pawn x) => localDef4.Worker.InspirationCanOccur(x)))
					{
						text += " [NO]";
					}
					list19.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate
					{
						foreach (Pawn item26 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
						{
							item26.mindState.inspirationHandler.TryStartInspiration(localDef4);
							dialog_DebugActionsMenu2.DustPuffFrom(item26);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list19));
			});
			Dialog_DebugActionsMenu dialog_DebugActionsMenu;
			TraitDef trDef;
			base.DebugAction("Tool: Give trait...", delegate
			{
				List<DebugMenuOption> list18 = new List<DebugMenuOption>();
				foreach (TraitDef allDef7 in DefDatabase<TraitDef>.AllDefs)
				{
					dialog_DebugActionsMenu = this;
					trDef = allDef7;
					for (int num12 = 0; num12 < allDef7.degreeDatas.Count; num12++)
					{
						int i2 = num12;
						list18.Add(new DebugMenuOption(trDef.degreeDatas[i2].label + " (" + trDef.degreeDatas[num12].degree + ")", DebugMenuOptionMode.Tool, delegate
						{
							foreach (Pawn item27 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).Cast<Pawn>())
							{
								if (item27.story != null)
								{
									Trait item = new Trait(trDef, trDef.degreeDatas[i2].degree, false);
									item27.story.traits.allTraits.Add(item);
									dialog_DebugActionsMenu.DustPuffFrom(item27);
								}
							}
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list18));
			});
			base.DebugToolMapForPawns("Tool: Give good thought", delegate(Pawn p)
			{
				if (p.needs.mood != null)
				{
					p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null);
				}
			});
			base.DebugToolMapForPawns("Tool: Give bad thought", delegate(Pawn p)
			{
				if (p.needs.mood != null)
				{
					p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null);
				}
			});
			base.DebugToolMapForPawns("Tool: Make faction hostile", delegate(Pawn p)
			{
				if (p.Faction != null && !p.Faction.HostileTo(Faction.OfPlayer))
				{
					p.Faction.SetHostileTo(Faction.OfPlayer, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Make faction neutral", delegate(Pawn p)
			{
				if (p.Faction != null && p.Faction.HostileTo(Faction.OfPlayer))
				{
					p.Faction.SetHostileTo(Faction.OfPlayer, false);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMap("Tool: Clear bound unfinished things", delegate
			{
				foreach (Building_WorkTable item28 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Building_WorkTable
				select t).Cast<Building_WorkTable>())
				{
					foreach (Bill item29 in item28.BillStack)
					{
						Bill_ProductionWithUft bill_ProductionWithUft = item29 as Bill_ProductionWithUft;
						if (bill_ProductionWithUft != null)
						{
							bill_ProductionWithUft.ClearBoundUft();
						}
					}
				}
			});
			base.DebugToolMapForPawns("Tool: Force birthday", delegate(Pawn p)
			{
				p.ageTracker.AgeBiologicalTicks = (p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1;
				p.ageTracker.DebugForceBirthdayBiological();
			});
			base.DebugToolMapForPawns("Tool: Recruit", delegate(Pawn p)
			{
				if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement(), p, 1f, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Damage apparel", delegate(Pawn p)
			{
				if (p.apparel != null && p.apparel.WornApparelCount > 0)
				{
					p.apparel.WornApparel.RandomElement().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Tame animal", delegate(Pawn p)
			{
				if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault(), p, 1f, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Train animal", delegate(Pawn p)
			{
				if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
				{
					foreach (TrainableDef allDef8 in DefDatabase<TrainableDef>.AllDefs)
					{
						while (!p.training.IsCompleted(allDef8))
						{
							p.training.Train(allDef8, null);
						}
					}
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Name animal by nuzzling", delegate(Pawn p)
			{
				if (p.Name != null && !p.Name.Numerical)
					return;
				if (p.RaceProps.Animal)
				{
					PawnUtility.GiveNameBecauseOfNuzzle(p.Map.mapPawns.FreeColonists.First(), p);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Try develop bond relation", delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					if (p.RaceProps.Humanlike)
					{
						IEnumerable<Pawn> source = from x in p.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Animal && x.Faction == p.Faction
						select x;
						if (source.Any())
						{
							RelationsUtility.TryDevelopBondRelation(p, source.RandomElement(), 999999f);
						}
					}
					else if (p.RaceProps.Animal)
					{
						IEnumerable<Pawn> source2 = from x in p.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike && x.Faction == p.Faction
						select x;
						if (source2.Any())
						{
							RelationsUtility.TryDevelopBondRelation(source2.RandomElement(), p, 999999f);
						}
					}
				}
			});
			Pawn otherLocal2;
			base.DebugToolMapForPawns("Tool: Start marriage ceremony", delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike)
				{
					List<DebugMenuOption> list17 = new List<DebugMenuOption>();
					foreach (Pawn item30 in from x in p.Map.mapPawns.AllPawnsSpawned
					where x.RaceProps.Humanlike
					select x)
					{
						if (p != item30)
						{
							otherLocal2 = item30;
							list17.Add(new DebugMenuOption(otherLocal2.LabelShort + " (" + otherLocal2.KindLabel + ")", DebugMenuOptionMode.Action, delegate
							{
								if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal2))
								{
									p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal2);
									p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal2);
									p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal2);
									Messages.Message("Dev: auto added fiance relation.", p, MessageTypeDefOf.TaskCompletion);
								}
								if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal2))
								{
									Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput);
								}
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list17));
				}
			});
			Pawn otherLocal;
			InteractionDef interactionLocal;
			base.DebugToolMapForPawns("Tool: Force interaction", delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					List<DebugMenuOption> list15 = new List<DebugMenuOption>();
					foreach (Pawn item31 in p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction))
					{
						if (item31 != p)
						{
							otherLocal = item31;
							list15.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate
							{
								List<DebugMenuOption> list16 = new List<DebugMenuOption>();
								foreach (InteractionDef item32 in DefDatabase<InteractionDef>.AllDefsListForReading)
								{
									interactionLocal = item32;
									list16.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate
									{
										p.interactions.TryInteractWith(otherLocal, interactionLocal);
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list16));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list15));
				}
			});
			base.DebugAction("Tool: Start party", delegate
			{
				if (!Find.VisibleMap.lordsStarter.TryStartParty())
				{
					Messages.Message("Could not find any valid party spot or organizer.", MessageTypeDefOf.RejectInput);
				}
			});
			base.DebugToolMapForPawns("Tool: Start prison break", delegate(Pawn p)
			{
				if (p.IsPrisoner)
				{
					PrisonBreakUtility.StartPrisonBreak(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Pass to world", delegate(Pawn p)
			{
				p.DeSpawn();
				Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
			});
			base.DebugToolMapForPawns("Tool: Make 1 year older", delegate(Pawn p)
			{
				p.ageTracker.DebugMake1YearOlder();
			});
			base.DoGap();
			Type localType;
			base.DebugToolMapForPawns("Tool: Try job giver", delegate(Pawn p)
			{
				List<DebugMenuOption> list14 = new List<DebugMenuOption>();
				foreach (Type item33 in typeof(ThinkNode_JobGiver).AllSubclasses())
				{
					localType = item33;
					list14.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate
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
							Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list14));
			});
			base.DebugToolMapForPawns("Tool: Try joy giver", delegate(Pawn p)
			{
				List<DebugMenuOption> list13 = new List<DebugMenuOption>();
				foreach (JoyGiverDef item34 in DefDatabase<JoyGiverDef>.AllDefsListForReading)
				{
					list13.Add(new DebugMenuOption(item34.defName, DebugMenuOptionMode.Action, delegate
					{
						Job job = item34.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false);
						}
						else
						{
							Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list13));
			});
			base.DebugToolMapForPawns("Tool: EndCurrentJob(" + 5.ToString() + ")", delegate(Pawn p)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("Tool: CheckForJobOverride", delegate(Pawn p)
			{
				p.jobs.CheckForJobOverride();
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("Tool: Toggle job logging", delegate(Pawn p)
			{
				p.jobs.debugLog = !p.jobs.debugLog;
				this.DustPuffFrom(p);
				MoteMaker.ThrowText(p.DrawPos, p.Map, p.LabelShort + "\n" + ((!p.jobs.debugLog) ? "OFF" : "ON"), -1f);
			});
			base.DebugToolMapForPawns("Tool: Toggle stance logging", delegate(Pawn p)
			{
				p.stances.debugLog = !p.stances.debugLog;
				this.DustPuffFrom(p);
			});
			base.DoGap();
			base.DoLabel("Tools - Spawning");
			PawnKindDef localKindDef;
			base.DebugAction("Tool: Spawn pawn", delegate
			{
				List<DebugMenuOption> list12 = new List<DebugMenuOption>();
				foreach (PawnKindDef item35 in from kd in DefDatabase<PawnKindDef>.AllDefs
				orderby kd.defName
				select kd)
				{
					localKindDef = item35;
					list12.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Tool, delegate
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
						Pawn newPawn = PawnGenerator.GeneratePawn(localKindDef, faction);
						GenSpawn.Spawn(newPawn, UI.MouseCell(), Find.VisibleMap);
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
								lord = LordMaker.MakeNewLord(faction, lordJob, Find.VisibleMap, null);
							}
							lord.AddPawn(newPawn);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list12));
			});
			ThingDef localDef3;
			base.DebugAction("Tool: Spawn weapon...", delegate
			{
				List<DebugMenuOption> list11 = new List<DebugMenuOption>();
				foreach (ThingDef item36 in from def in DefDatabase<ThingDef>.AllDefs
				where def.equipmentType == EquipmentType.Primary
				select def)
				{
					localDef3 = item36;
					list11.Add(new DebugMenuOption(localDef3.LabelCap, DebugMenuOptionMode.Tool, delegate
					{
						DebugThingPlaceHelper.DebugSpawn(localDef3, UI.MouseCell(), -1, false);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list11));
			});
			base.DebugAction("Tool: Try place near thing...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)));
			});
			base.DebugAction("Tool: Try place near stacks of 25...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)));
			});
			base.DebugAction("Tool: Try place near stacks of 75...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)));
			});
			base.DebugAction("Tool: Try place direct thing...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)));
			});
			base.DebugAction("Tool: Try place direct stacks of 25...", delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)));
			});
			TerrainDef localDef2;
			base.DebugAction("Tool: Set terrain...", delegate
			{
				List<DebugMenuOption> list10 = new List<DebugMenuOption>();
				foreach (TerrainDef allDef9 in DefDatabase<TerrainDef>.AllDefs)
				{
					localDef2 = allDef9;
					list10.Add(new DebugMenuOption(localDef2.LabelCap, DebugMenuOptionMode.Tool, delegate
					{
						if (UI.MouseCell().InBounds(Find.VisibleMap))
						{
							Find.VisibleMap.terrainGrid.SetTerrain(UI.MouseCell(), localDef2);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list10));
			});
			base.DebugToolMap("Tool: Make filth x100", delegate
			{
				for (int num11 = 0; num11 < 100; num11++)
				{
					IntVec3 c2 = UI.MouseCell() + GenRadial.RadialPattern[num11];
					if (c2.InBounds(Find.VisibleMap) && c2.Walkable(Find.VisibleMap))
					{
						FilthMaker.MakeFilth(c2, Find.VisibleMap, ThingDefOf.FilthDirt, 2);
						MoteMaker.ThrowMetaPuff(c2.ToVector3Shifted(), Find.VisibleMap);
					}
				}
			});
			Faction localFac;
			base.DebugToolMap("Tool: Spawn faction leader", delegate
			{
				List<FloatMenuOption> list9 = new List<FloatMenuOption>();
				foreach (Faction allFaction2 in Find.FactionManager.AllFactions)
				{
					localFac = allFaction2;
					if (localFac.leader != null)
					{
						list9.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, delegate
						{
							GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.VisibleMap);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list9));
			});
			PawnKindDef kLocal;
			Pawn pLocal;
			base.DebugAction("Spawn world pawn...", delegate
			{
				List<DebugMenuOption> list7 = new List<DebugMenuOption>();
				Action<Pawn> act = delegate(Pawn p)
				{
					List<DebugMenuOption> list8 = new List<DebugMenuOption>();
					foreach (PawnKindDef item37 in from x in DefDatabase<PawnKindDef>.AllDefs
					where x.race == p.def
					select x)
					{
						kLocal = item37;
						list8.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate
						{
							PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null);
							PawnGenerator.RedressPawn(p, request);
							GenSpawn.Spawn(p, UI.MouseCell(), Find.VisibleMap);
							DebugTools.curTool = null;
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list8));
				};
				foreach (Pawn item38 in Find.WorldPawns.AllPawnsAlive)
				{
					pLocal = item38;
					list7.Add(new DebugMenuOption(item38.LabelShort, DebugMenuOptionMode.Action, delegate
					{
						act(pLocal);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list7));
			});
			base.DebugAction("Spawn item collection...", delegate
			{
				List<DebugMenuOption> list5 = new List<DebugMenuOption>();
				List<ItemCollectionGeneratorDef> allDefsListForReading2 = DefDatabase<ItemCollectionGeneratorDef>.AllDefsListForReading;
				for (int n = 0; n < allDefsListForReading2.Count; n++)
				{
					ItemCollectionGeneratorDef localGenerator = allDefsListForReading2[n];
					list5.Add(new DebugMenuOption(localGenerator.defName, DebugMenuOptionMode.Tool, delegate
					{
						if (UI.MouseCell().InBounds(Find.VisibleMap))
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							List<Thing> list6 = localGenerator.Worker.Generate(default(ItemCollectionGeneratorParams));
							stringBuilder2.AppendLine(localGenerator.Worker.GetType().Name + " generated " + list6.Count + " things:");
							float num9 = 0f;
							for (int num10 = 0; num10 < list6.Count; num10++)
							{
								stringBuilder2.AppendLine("   - " + list6[num10].LabelCap);
								num9 += list6[num10].MarketValue * (float)list6[num10].stackCount;
								if (!GenPlace.TryPlaceThing(list6[num10], UI.MouseCell(), Find.VisibleMap, ThingPlaceMode.Near, null))
								{
									list6[num10].Destroy(DestroyMode.Vanish);
								}
							}
							stringBuilder2.AppendLine("Total market value: " + num9.ToString("0.##"));
							Log.Message(stringBuilder2.ToString());
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list5));
			});
			base.DebugAction("Tool: Trigger effecter...", delegate
			{
				List<DebugMenuOption> list4 = new List<DebugMenuOption>();
				List<EffecterDef> allDefsListForReading = DefDatabase<EffecterDef>.AllDefsListForReading;
				for (int m = 0; m < allDefsListForReading.Count; m++)
				{
					EffecterDef localDef = allDefsListForReading[m];
					list4.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate
					{
						Effecter effecter = localDef.Spawn();
						effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.VisibleMap, false), new TargetInfo(UI.MouseCell(), Find.VisibleMap, false));
						effecter.Cleanup();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
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
				int num8 = 0;
				Pawn pawn5;
				while (true)
				{
					if (num8 < 100)
					{
						PawnKindDef random5 = DefDatabase<PawnKindDef>.GetRandom();
						pawn5 = PawnGenerator.GeneratePawn(random5, FactionUtility.DefaultFactionFrom(random5.defaultFactionType));
						GenSpawn.Spawn(pawn5, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.VisibleMap), Find.VisibleMap, 1000), Find.VisibleMap);
						HealthUtility.DamageUntilDowned(pawn5);
						if (!pawn5.Dead)
						{
							num8++;
							continue;
						}
						break;
					}
					return;
				}
				Log.Error("Pawn died while force downing: " + pawn5 + " at " + pawn5.Position);
			});
			base.DebugAction("Test force kill x100", delegate
			{
				int num7 = 0;
				Pawn pawn4;
				while (true)
				{
					if (num7 < 100)
					{
						PawnKindDef random4 = DefDatabase<PawnKindDef>.GetRandom();
						pawn4 = PawnGenerator.GeneratePawn(random4, FactionUtility.DefaultFactionFrom(random4.defaultFactionType));
						GenSpawn.Spawn(pawn4, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.VisibleMap), Find.VisibleMap, 1000), Find.VisibleMap);
						HealthUtility.DamageUntilDead(pawn4);
						if (pawn4.Dead)
						{
							num7++;
							continue;
						}
						break;
					}
					return;
				}
				Log.Error("Pawn died not die: " + pawn4 + " at " + pawn4.Position);
			});
			base.DebugAction("Test Surgery fail catastrophic x100", delegate
			{
				for (int l = 0; l < 100; l++)
				{
					PawnKindDef random3 = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn3 = PawnGenerator.GeneratePawn(random3, FactionUtility.DefaultFactionFrom(random3.defaultFactionType));
					GenSpawn.Spawn(pawn3, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.VisibleMap), Find.VisibleMap, 1000), Find.VisibleMap);
					pawn3.health.forceIncap = true;
					BodyPartRecord part = pawn3.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).RandomElement();
					HealthUtility.GiveInjuriesOperationFailureCatastrophic(pawn3, part);
					pawn3.health.forceIncap = false;
					if (pawn3.Dead)
					{
						Log.Error("Pawn died: " + pawn3 + " at " + pawn3.Position);
					}
				}
			});
			base.DebugAction("Test Surgery fail ridiculous x100", delegate
			{
				for (int k = 0; k < 100; k++)
				{
					PawnKindDef random2 = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn2 = PawnGenerator.GeneratePawn(random2, FactionUtility.DefaultFactionFrom(random2.defaultFactionType));
					GenSpawn.Spawn(pawn2, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.VisibleMap), Find.VisibleMap, 1000), Find.VisibleMap);
					pawn2.health.forceIncap = true;
					HealthUtility.GiveInjuriesOperationFailureRidiculous(pawn2);
					pawn2.health.forceIncap = false;
					if (pawn2.Dead)
					{
						Log.Error("Pawn died: " + pawn2 + " at " + pawn2.Position);
					}
				}
			});
			base.DebugAction("Test generate pawn x1000", delegate
			{
				float[] array = new float[10]
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
					float ms = (float)(PerfLogger.Duration() * 1000.0);
					array2[array.FirstIndexOf((float time) => ms <= time)]++;
					if (pawn.Dead)
					{
						Log.Error("Pawn is dead");
					}
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Pawn creation time histogram:");
				for (int j = 0; j < array2.Length; j++)
				{
					stringBuilder.AppendLine(string.Format("<{0}ms: {1}", array[j], array2[j]));
				}
				Log.Message(stringBuilder.ToString());
			});
			base.DebugAction("Check region listers", delegate
			{
				Autotests_RegionListers.CheckBugs(Find.VisibleMap);
			});
			base.DebugAction("Test time-to-down", delegate
			{
				if (Find.World.info.planetCoverage < 0.25)
				{
					Log.Error("Planet coverage must be 0.3+");
				}
				else
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (PawnKindDef item39 in from kd in DefDatabase<PawnKindDef>.AllDefs
					orderby kd.defName
					select kd)
					{
						list.Add(new DebugMenuOption(item39.label, DebugMenuOptionMode.Action, delegate
						{
							if (item39 == PawnKindDefOf.Colonist)
							{
								Log.Message("Current colonist TTD reference point: 22.3 seconds, stddev 8.35 seconds");
							}
							List<float> results = new List<float>();
							List<PawnKindDef> list2 = new List<PawnKindDef>();
							List<PawnKindDef> list3 = new List<PawnKindDef>();
							list2.Add(item39);
							list3.Add(item39);
							ArenaUtility.BeginArenaFightSet(1000, list2, list3, delegate(ArenaUtility.ArenaResult result)
							{
								if (result.winner != 0)
								{
									results.Add(result.tickDuration.TicksToSeconds());
								}
							}, delegate
							{
								Log.Message(string.Format("Finished {0} tests; time-to-down {1}, stddev {2}\n\nraw: {3}", results.Count, results.Average(), GenMath.Stddev(results), GenText.ToLineList(results.Select((float res) => res.ToString()), string.Empty)));
							});
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
			});
			base.DebugAction("Battle Royale", delegate
			{
				if (Find.World.info.planetCoverage < 0.25)
				{
					Log.Error("Planet coverage must be 0.3+");
				}
				else
				{
					Dictionary<PawnKindDef, float> ratings = new Dictionary<PawnKindDef, float>();
					foreach (PawnKindDef allDef10 in DefDatabase<PawnKindDef>.AllDefs)
					{
						ratings[allDef10] = EloUtility.CalculateRating(allDef10.combatPower, 1500f, 60f);
					}
					int currentFights = 0;
					Current.Game.GetComponent<GameComponent_DebugTools>().AddPerFrameCallback(delegate
					{
						if (currentFights >= 15)
						{
							return false;
						}
						PawnKindDef lhsDef = DefDatabase<PawnKindDef>.AllDefs.RandomElement();
						PawnKindDef rhsDef = DefDatabase<PawnKindDef>.AllDefs.RandomElement();
						float num = EloUtility.CalculateExpectation(ratings[lhsDef], ratings[rhsDef]);
						float num2 = (float)(1.0 - num);
						float num3 = num;
						float num4 = Mathf.Min(num2, num3);
						num2 /= num4;
						num3 /= num4;
						float num5 = Mathf.Max(num2, num3);
						if (num5 > 40.0)
						{
							return false;
						}
						float num6 = Rand.Range(1f, (float)(40.0 / num5));
						num2 *= num6;
						num3 *= num6;
						List<PawnKindDef> lhs = Enumerable.Repeat(lhsDef, GenMath.RoundRandom(num2)).ToList();
						List<PawnKindDef> rhs = Enumerable.Repeat(rhsDef, GenMath.RoundRandom(num3)).ToList();
						currentFights++;
						ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
						{
							currentFights--;
							if (result.winner != 0)
							{
								float value = ratings[lhsDef];
								float value2 = ratings[rhsDef];
								EloUtility.Update(ref value, ref value2, 0.5f, (float)((result.winner == ArenaUtility.ArenaResult.Winner.Lhs) ? 1 : 0), 32f);
								ratings[lhsDef] = value;
								ratings[rhsDef] = value2;
								Log.Message(string.Format("Current scores:\n\n{0}", GenText.ToLineList(from v in ratings
								orderby v.Value
								select string.Format("{0}: {1}->{2} (rating {2})", v.Key.label, v.Key.combatPower, EloUtility.CalculateLinearScore(v.Value, 1500f, 60f), v.Value), string.Empty)));
							}
						});
						return false;
					});
				}
			});
		}

		private void DoListingItems_World()
		{
			base.DoLabel("Tools - World");
			Text.Font = GameFont.Tiny;
			base.DoLabel("Incidents");
			IIncidentTarget altTarget = Find.WorldSelector.SingleSelectedObject as IIncidentTarget;
			this.DoExecuteIncidentDebugAction(Find.World, altTarget);
			this.DoExecuteIncidentWithDebugAction(Find.World, altTarget);
			base.DoLabel("Tools - Spawning");
			base.DebugToolWorld("Spawn random caravan", delegate
			{
				int num2 = GenWorld.MouseTile(false);
				Tile tile3 = Find.WorldGrid[num2];
				if (!tile3.biome.impassable)
				{
					Caravan caravan = (Caravan)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Caravan);
					caravan.Tile = num2;
					caravan.SetFaction(Faction.OfPlayer);
					Find.WorldObjects.Add(caravan);
					int num3 = Rand.RangeInclusive(1, 10);
					for (int i = 0; i < num3; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
						caravan.AddPawn(pawn, true);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						if (Rand.Value < 0.5)
						{
							ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefs
							where def.IsWeapon && def.PlayerAcquirable
							select def).RandomElementWithFallback(null);
							pawn.equipment.AddEquipment((ThingWithComps)ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
						}
					}
					List<Thing> list3 = ItemCollectionGeneratorDefOf.DebugCaravanInventory.Worker.Generate(default(ItemCollectionGeneratorParams));
					for (int j = 0; j < list3.Count; j++)
					{
						CaravanInventoryUtility.GiveThing(caravan, list3[j]);
					}
				}
			});
			base.DebugToolWorld("Spawn random faction base", delegate
			{
				Faction faction = default(Faction);
				if ((from x in Find.FactionManager.AllFactions
				where !x.IsPlayer && !x.def.hidden
				select x).TryRandomElement<Faction>(out faction))
				{
					int num = GenWorld.MouseTile(false);
					Tile tile2 = Find.WorldGrid[num];
					if (!tile2.biome.impassable)
					{
						FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
						factionBase.SetFaction(faction);
						factionBase.Tile = num;
						factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase);
						Find.WorldObjects.Add(factionBase);
					}
				}
			});
			SiteCoreDef localDef;
			Action addPart;
			SitePartDef localPart;
			base.DebugToolWorld("Spawn site", delegate
			{
				int tile = GenWorld.MouseTile(false);
				if (tile < 0 || Find.World.Impassable(tile))
				{
					Messages.Message("Impassable", MessageTypeDefOf.RejectInput);
				}
				else
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					List<SitePartDef> parts = new List<SitePartDef>();
					foreach (SiteCoreDef allDef in DefDatabase<SiteCoreDef>.AllDefs)
					{
						localDef = allDef;
						addPart = null;
						addPart = delegate
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							list2.Add(new DebugMenuOption("-Done (" + parts.Count + " parts)-", DebugMenuOptionMode.Action, delegate
							{
								Site site = SiteMaker.TryMakeSite(localDef, parts, true, null);
								if (site == null)
								{
									Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput);
								}
								else
								{
									site.Tile = tile;
									Find.WorldObjects.Add(site);
								}
							}));
							foreach (SitePartDef allDef2 in DefDatabase<SitePartDef>.AllDefs)
							{
								localPart = allDef2;
								list2.Add(new DebugMenuOption(allDef2.defName, DebugMenuOptionMode.Action, delegate
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
		}

		private void DoRaid(IncidentParms parms)
		{
			IncidentDef incidentDef = (!parms.faction.HostileTo(Faction.OfPlayer)) ? IncidentDefOf.RaidFriendly : IncidentDefOf.RaidEnemy;
			incidentDef.Worker.TryExecute(parms);
		}

		private void DoExecuteIncidentDebugAction(IIncidentTarget target, IIncidentTarget altTarget)
		{
			IIncidentTarget thisIncidentTarget;
			IncidentDef localDef;
			base.DebugAction("Execute incident...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef item in from d in DefDatabase<IncidentDef>.AllDefs
				where d.TargetAllowed(target) || (altTarget != null && d.TargetAllowed(altTarget))
				orderby !d.TargetAllowed(target), d.defName
				select d)
				{
					thisIncidentTarget = ((!item.TargetAllowed(target)) ? altTarget : target);
					localDef = item;
					string text = localDef.defName;
					if (!localDef.Worker.CanFireNow(thisIncidentTarget))
					{
						text += " [NO]";
					}
					if (thisIncidentTarget == altTarget)
					{
						text = text + " (" + altTarget.GetType().Name.Truncate(52f, null) + ")";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
					{
						IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, localDef.category, thisIncidentTarget);
						if (localDef.pointsScaleable)
						{
							StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain);
							incidentParms = storytellerComp.GenerateParms(localDef.category, incidentParms.target);
						}
						localDef.Worker.TryExecute(incidentParms);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}

		private void DoExecuteIncidentWithDebugAction(IIncidentTarget target, IIncidentTarget altTarget)
		{
			IIncidentTarget thisIncidentTarget;
			IncidentDef localDef;
			float localPoints;
			base.DebugAction("Execute incident with...", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef item in from d in DefDatabase<IncidentDef>.AllDefs
				where (d.TargetAllowed(target) || (altTarget != null && d.TargetAllowed(altTarget))) && d.pointsScaleable
				orderby !d.TargetAllowed(target), d.defName
				select d)
				{
					thisIncidentTarget = ((!item.TargetAllowed(target)) ? altTarget : target);
					localDef = item;
					string text = localDef.defName;
					if (!localDef.Worker.CanFireNow(thisIncidentTarget))
					{
						text += " [NO]";
					}
					if (thisIncidentTarget == altTarget)
					{
						text = text + " (" + altTarget.GetType().Name.Truncate(52f, null) + ")";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
					{
						IncidentParms parms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, localDef.category, thisIncidentTarget);
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float item2 in Dialog_DebugActionsMenu.PointsOptions())
						{
							localPoints = item2;
							list2.Add(new DebugMenuOption(item2 + " points", DebugMenuOptionMode.Action, delegate
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

		private void DebugGiveResource(ThingDef resDef, int count)
		{
			Pawn pawn = Find.VisibleMap.mapPawns.FreeColonistsSpawned.RandomElement();
			int num = count;
			int num2 = 0;
			while (num > 0)
			{
				int num3 = Math.Min(resDef.stackLimit, num);
				num -= num3;
				Thing thing = ThingMaker.MakeThing(resDef, null);
				thing.stackCount = num3;
				if (GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, null))
				{
					num2 += num3;
					continue;
				}
				break;
			}
			Messages.Message("Made " + num2 + " " + resDef + " near " + pawn + ".", MessageTypeDefOf.TaskCompletion);
		}

		private void OffsetNeed(NeedDef nd, float offsetPct)
		{
			foreach (Pawn item in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Pawn
			select t).Cast<Pawn>())
			{
				Need need = item.needs.TryGetNeed(nd);
				if (need != null)
				{
					need.CurLevel += offsetPct * need.MaxLevel;
					this.DustPuffFrom(item);
				}
			}
		}

		private void DustPuffFrom(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.Drawer.Notify_DebugAffected();
			}
		}

		private void AddGuest(bool prisoner)
		{
			foreach (Building_Bed item in Find.VisibleMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if (item.ForPrisoners == prisoner && (!item.owners.Any() || (prisoner && item.AnyUnownedSleepingSlot)))
				{
					PawnKindDef pawnKindDef = prisoner ? (from pk in DefDatabase<PawnKindDef>.AllDefs
					where pk.defaultFactionType != null && !pk.defaultFactionType.isPlayer && pk.RaceProps.Humanlike
					select pk).RandomElement() : PawnKindDefOf.SpaceRefugee;
					Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction);
					GenSpawn.Spawn(pawn, item.Position, Find.VisibleMap);
					foreach (ThingWithComps item2 in pawn.equipment.AllEquipmentListForReading.ToList())
					{
						ThingWithComps thingWithComps = default(ThingWithComps);
						if (pawn.equipment.TryDropEquipment(item2, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
					pawn.inventory.innerContainer.Clear();
					pawn.ownership.ClaimBedIfNonMedical(item);
					pawn.guest.SetGuestStatus(Faction.OfPlayer, prisoner);
					break;
				}
			}
		}

		public static IEnumerable<float> PointsOptions()
		{
			yield return 35f;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
