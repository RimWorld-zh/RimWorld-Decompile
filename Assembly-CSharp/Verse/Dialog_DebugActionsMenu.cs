using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Profile;

namespace Verse
{
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		private const float DebugOptionsGap = 7f;

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
			this.DoLabel("Translation tools");
			base.DebugAction("Write backstory translation file", (Action)delegate
			{
				LanguageDataWriter.WriteBackstoryFile();
			});
			base.DebugAction("Output translation report", (Action)delegate
			{
				LanguageReportGenerator.OutputTranslationReport();
			});
		}

		private void DoListingItems_AllModePlayActions()
		{
			this.DoGap();
			this.DoLabel("Actions - Map management");
			base.DebugAction("Generate map", (Action)delegate
			{
				MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				mapParent.Tile = TileFinder.RandomStartingTile();
				mapParent.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(mapParent);
				GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
			});
			base.DebugAction("Destroy map", (Action)delegate
			{
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				List<Map> maps3 = Find.Maps;
				for (int l = 0; l < maps3.Count; l++)
				{
					Map map4 = maps3[l];
					list3.Add(new DebugMenuOption(map4.ToString(), DebugMenuOptionMode.Action, (Action)delegate
					{
						Current.Game.DeinitAndRemoveMap(map4);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
			});
			base.DebugToolMap("Transfer", (Action)delegate()
			{
				List<Thing> toTransfer = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList();
				if (toTransfer.Any())
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<Map> maps2 = Find.Maps;
					for (int j = 0; j < maps2.Count; j++)
					{
						Map map2 = maps2[j];
						if (map2 != Find.VisibleMap)
						{
							list2.Add(new DebugMenuOption(map2.ToString(), DebugMenuOptionMode.Action, (Action)delegate()
							{
								for (int k = 0; k < toTransfer.Count; k++)
								{
									IntVec3 center = map2.Center;
									Map map3 = map2;
									IntVec3 size = map2.Size;
									int x2 = size.x;
									IntVec3 size2 = map2.Size;
									IntVec3 center2 = default(IntVec3);
									if (CellFinder.TryFindRandomCellNear(center, map3, Mathf.Max(x2, size2.z), (Predicate<IntVec3>)((IntVec3 x) => !x.Fogged(map2) && x.Standable(map2)), out center2))
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
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}
			});
			base.DebugAction("Change map", (Action)delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Map map = maps[i];
					if (map != Find.VisibleMap)
					{
						list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, (Action)delegate
						{
							Current.Game.VisibleMap = map;
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}

		private void DoListingItems_MapActions()
		{
			Text.Font = GameFont.Tiny;
			this.DoLabel("Incidents");
			this.DoExecuteIncidentDebugAction(Find.VisibleMap, null);
			this.DoExecuteIncidentWithDebugAction(Find.VisibleMap, null);
			base.DebugAction("Execute raid with...", (Action)delegate()
			{
				StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((Func<StorytellerComp, bool>)((StorytellerComp x) => x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain));
				IncidentParms parms = storytellerComp.GenerateParms(IncidentCategory.ThreatBig, Find.VisibleMap);
				List<DebugMenuOption> list8 = new List<DebugMenuOption>();
				foreach (Faction allFaction in Find.FactionManager.AllFactions)
				{
					Faction localFac = allFaction;
					list8.Add(new DebugMenuOption(localFac.Name + " (" + localFac.def.defName + ")", DebugMenuOptionMode.Action, (Action)delegate
					{
						parms.faction = localFac;
						List<DebugMenuOption> list9 = new List<DebugMenuOption>();
						foreach (float item in Dialog_DebugActionsMenu.PointsOptions())
						{
							float num2 = item;
							float localPoints = num2;
							list9.Add(new DebugMenuOption(num2 + " points", DebugMenuOptionMode.Action, (Action)delegate
							{
								parms.points = localPoints;
								List<DebugMenuOption> list10 = new List<DebugMenuOption>();
								foreach (RaidStrategyDef allDef in DefDatabase<RaidStrategyDef>.AllDefs)
								{
									RaidStrategyDef localStrat = allDef;
									string text = localStrat.defName;
									if (!localStrat.Worker.CanUseWith(parms))
									{
										text += " [NO]";
									}
									list10.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, (Action)delegate
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
			Action<int> DoRandomEnemyRaid = (Action<int>)delegate(int pts)
			{
				this.Close(true);
				IncidentParms incidentParms2 = new IncidentParms();
				incidentParms2.target = Find.VisibleMap;
				incidentParms2.points = (float)pts;
				IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms2);
			};
			base.DebugAction("Raid (35 pts)", (Action)delegate
			{
				DoRandomEnemyRaid(35);
			});
			base.DebugAction("Raid (75 pts)", (Action)delegate
			{
				DoRandomEnemyRaid(75);
			});
			base.DebugAction("Raid (300 pts)", (Action)delegate
			{
				DoRandomEnemyRaid(300);
			});
			base.DebugAction("Raid (400 pts)", (Action)delegate
			{
				DoRandomEnemyRaid(400);
			});
			base.DebugAction("Raid  (1000 pts)", (Action)delegate
			{
				DoRandomEnemyRaid(1000);
			});
			base.DebugAction("Raid  (3000 pts)", (Action)delegate
			{
				DoRandomEnemyRaid(3000);
			});
			this.DoGap();
			this.DoLabel("Actions - Misc");
			base.DebugAction("Change weather...", (Action)delegate
			{
				List<DebugMenuOption> list7 = new List<DebugMenuOption>();
				foreach (WeatherDef allDef in DefDatabase<WeatherDef>.AllDefs)
				{
					WeatherDef localWeather = allDef;
					list7.Add(new DebugMenuOption(localWeather.LabelCap, DebugMenuOptionMode.Action, (Action)delegate
					{
						Find.VisibleMap.weatherManager.TransitionTo(localWeather);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list7));
			});
			base.DebugAction("Start song...", (Action)delegate
			{
				List<DebugMenuOption> list6 = new List<DebugMenuOption>();
				foreach (SongDef allDef in DefDatabase<SongDef>.AllDefs)
				{
					SongDef localSong = allDef;
					list6.Add(new DebugMenuOption(localSong.defName, DebugMenuOptionMode.Action, (Action)delegate
					{
						Find.MusicManagerPlay.ForceStartSong(localSong, false);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list6));
			});
			if (Find.VisibleMap.gameConditionManager.ActiveConditions.Count > 0)
			{
				base.DebugAction("End game condition ...", (Action)delegate
				{
					List<DebugMenuOption> list5 = new List<DebugMenuOption>();
					List<GameCondition>.Enumerator enumerator8 = Find.VisibleMap.gameConditionManager.ActiveConditions.GetEnumerator();
					try
					{
						while (enumerator8.MoveNext())
						{
							GameCondition current8 = enumerator8.Current;
							GameCondition localMc = current8;
							list5.Add(new DebugMenuOption(localMc.LabelCap, DebugMenuOptionMode.Action, (Action)delegate
							{
								localMc.End();
							}));
						}
					}
					finally
					{
						((IDisposable)(object)enumerator8).Dispose();
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list5));
				});
			}
			base.DebugAction("Add prisoner", (Action)delegate
			{
				this.AddGuest(true);
			});
			base.DebugAction("Add guest", (Action)delegate
			{
				this.AddGuest(false);
			});
			base.DebugAction("Force enemy assault", (Action)delegate
			{
				List<Lord>.Enumerator enumerator6 = Find.VisibleMap.lordManager.lords.GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						Lord current6 = enumerator6.Current;
						LordToil_Stage lordToil_Stage = current6.CurLordToil as LordToil_Stage;
						if (lordToil_Stage != null)
						{
							List<Transition>.Enumerator enumerator7 = current6.Graph.transitions.GetEnumerator();
							try
							{
								while (enumerator7.MoveNext())
								{
									Transition current7 = enumerator7.Current;
									if (current7.sources.Contains(lordToil_Stage) && current7.target is LordToil_AssaultColony)
									{
										Messages.Message("Debug forcing to assault toil: " + current6.faction, MessageSound.SeriousAlert);
										current6.GotoToil(current7.target);
										return;
									}
								}
							}
							finally
							{
								((IDisposable)(object)enumerator7).Dispose();
							}
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator6).Dispose();
				}
			});
			base.DebugAction("Force enemy flee", (Action)delegate()
			{
				List<Lord>.Enumerator enumerator5 = Find.VisibleMap.lordManager.lords.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						Lord current5 = enumerator5.Current;
						if (current5.faction.HostileTo(Faction.OfPlayer) && current5.faction.def.autoFlee)
						{
							LordToil lordToil = current5.Graph.lordToils.FirstOrDefault((Func<LordToil, bool>)((LordToil st) => st is LordToil_PanicFlee));
							if (lordToil != null)
							{
								current5.GotoToil(lordToil);
							}
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator5).Dispose();
				}
			});
			base.DebugAction("Destroy all things", (Action)delegate
			{
				List<Thing>.Enumerator enumerator4 = Find.VisibleMap.listerThings.AllThings.ToList().GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						Thing current4 = enumerator4.Current;
						current4.Destroy(DestroyMode.Vanish);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator4).Dispose();
				}
			});
			base.DebugAction("Destroy all plants", (Action)delegate
			{
				List<Thing>.Enumerator enumerator3 = Find.VisibleMap.listerThings.AllThings.ToList().GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						Thing current3 = enumerator3.Current;
						if (current3 is Plant)
						{
							current3.Destroy(DestroyMode.Vanish);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator3).Dispose();
				}
			});
			base.DebugAction("Unload unused assets", (Action)delegate
			{
				MemoryUtility.UnloadUnusedUnityAssets();
			});
			base.DebugAction("Name colony...", (Action)delegate
			{
				List<DebugMenuOption> list4 = new List<DebugMenuOption>();
				list4.Add(new DebugMenuOption("Faction", DebugMenuOptionMode.Action, (Action)delegate
				{
					Find.WindowStack.Add(new Dialog_NamePlayerFaction());
				}));
				if (Find.VisibleMap != null && Find.VisibleMap.IsPlayerHome)
				{
					FactionBase factionBase = (FactionBase)Find.VisibleMap.info.parent;
					list4.Add(new DebugMenuOption("Faction base", DebugMenuOptionMode.Action, (Action)delegate
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionBase(factionBase));
					}));
					list4.Add(new DebugMenuOption("Faction and faction base", DebugMenuOptionMode.Action, (Action)delegate
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionAndBase(factionBase));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
			});
			base.DebugAction("Next lesson", (Action)delegate
			{
				LessonAutoActivator.DebugForceInitiateBestLessonNow();
			});
			base.DebugAction("Regen all map mesh sections", (Action)delegate
			{
				Find.VisibleMap.mapDrawer.RegenerateEverythingNow();
			});
			base.DebugAction("Finish all research", (Action)delegate
			{
				Find.ResearchManager.DebugSetAllProjectsFinished();
				Messages.Message("All research finished.", MessageSound.Benefit);
			});
			base.DebugAction("Replace all trade ships", (Action)delegate
			{
				Find.VisibleMap.passingShipManager.DebugSendAllShipsAway();
				for (int i = 0; i < 5; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.VisibleMap;
					IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
				}
			});
			base.DebugAction("Change camera config...", (Action)delegate
			{
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				foreach (Type item in typeof(CameraMapConfig).AllSubclasses())
				{
					Type localType = item;
					list3.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, (Action)delegate
					{
						Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(localType);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
			});
			base.DebugAction("Force ship countdown...", (Action)delegate
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				list2.Add(new DebugMenuOption("Player ship", DebugMenuOptionMode.Action, (Action)delegate
				{
					ShipCountdown.InitiateCountdown(null, -1);
				}));
				list2.Add(new DebugMenuOption("Journey destination", DebugMenuOptionMode.Action, (Action)delegate
				{
					ShipCountdown.InitiateCountdown(null, 0);
				}));
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			});
			base.DebugAction("Flash trade drop spot", (Action)delegate
			{
				IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.VisibleMap);
				Find.VisibleMap.debugDrawer.FlashCell(intVec, 0f, (string)null);
				Log.Message("trade drop spot: " + intVec);
			});
			base.DebugAction("Kill faction leader", (Action)delegate()
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
			base.DebugAction("Refog map", (Action)delegate
			{
				FloodFillerFog.DebugRefogMap(Find.VisibleMap);
			});
			base.DebugAction("Use GenStep", (Action)delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (Type item in typeof(GenStep).AllSubclassesNonAbstract())
				{
					Type localGenStep = item;
					list.Add(new DebugMenuOption(localGenStep.Name, DebugMenuOptionMode.Action, (Action)delegate
					{
						GenStep genStep = (GenStep)Activator.CreateInstance(localGenStep);
						genStep.Generate(Find.VisibleMap);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			base.DebugAction("Increment time 1 day", (Action)delegate
			{
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 60000);
			});
			base.DebugAction("Increment time 1 season", (Action)delegate
			{
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 900000);
			});
		}

		private void DoListingItems_MapTools()
		{
			this.DoGap();
			this.DoLabel("Tools - General");
			base.DebugToolMap("Tool: Destroy", (Action)delegate
			{
				List<Thing>.Enumerator enumerator43 = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList().GetEnumerator();
				try
				{
					while (enumerator43.MoveNext())
					{
						Thing current43 = enumerator43.Current;
						current43.Destroy(DestroyMode.Vanish);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator43).Dispose();
				}
			});
			base.DebugToolMap("Tool: Kill", (Action)delegate
			{
				List<Thing>.Enumerator enumerator42 = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList().GetEnumerator();
				try
				{
					while (enumerator42.MoveNext())
					{
						Thing current42 = enumerator42.Current;
						current42.Kill(default(DamageInfo?));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator42).Dispose();
				}
			});
			base.DebugToolMap("Tool: 10 damage", (Action)delegate
			{
				List<Thing>.Enumerator enumerator41 = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList().GetEnumerator();
				try
				{
					while (enumerator41.MoveNext())
					{
						Thing current41 = enumerator41.Current;
						current41.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator41).Dispose();
				}
			});
			base.DebugToolMap("Tool: 5000 damage", (Action)delegate
			{
				List<Thing>.Enumerator enumerator40 = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList().GetEnumerator();
				try
				{
					while (enumerator40.MoveNext())
					{
						Thing current40 = enumerator40.Current;
						current40.TakeDamage(new DamageInfo(DamageDefOf.Crush, 5000, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator40).Dispose();
				}
			});
			base.DebugToolMap("Tool: 5000 flame damage", (Action)delegate
			{
				List<Thing>.Enumerator enumerator39 = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList().GetEnumerator();
				try
				{
					while (enumerator39.MoveNext())
					{
						Thing current39 = enumerator39.Current;
						current39.TakeDamage(new DamageInfo(DamageDefOf.Flame, 5000, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator39).Dispose();
				}
			});
			base.DebugToolMap("Tool: Clear area 21x21", (Action)delegate
			{
				CellRect r = CellRect.CenteredOn(UI.MouseCell(), 10);
				GenDebug.ClearArea(r, Find.VisibleMap);
			});
			base.DebugToolMap("Tool: Rock 21x21", (Action)delegate
			{
				CellRect cellRect = CellRect.CenteredOn(UI.MouseCell(), 10);
				cellRect.ClipInsideMap(Find.VisibleMap);
				ThingDef granite = ThingDefOf.Granite;
				foreach (IntVec3 item in cellRect)
				{
					GenSpawn.Spawn(granite, item, Find.VisibleMap);
				}
			});
			this.DoGap();
			base.DebugToolMap("Tool: Explosion (bomb)", (Action)delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.Bomb, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			});
			base.DebugToolMap("Tool: Explosion (flame)", (Action)delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.Flame, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			});
			base.DebugToolMap("Tool: Explosion (stun)", (Action)delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.Stun, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			});
			base.DebugToolMap("Tool: Explosion (EMP)", (Action)delegate
			{
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 3.9f, DamageDefOf.EMP, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			});
			base.DebugToolMap("Tool: Explosion (extinguisher)", (Action)delegate
			{
				ThingDef filthFireFoam = ThingDefOf.FilthFireFoam;
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 10f, DamageDefOf.Extinguish, null, null, null, null, filthFireFoam, 1f, 3, true, null, 0f, 1);
			});
			base.DebugToolMap("Tool: Explosion (smoke)", (Action)delegate
			{
				ThingDef gas_Smoke = ThingDefOf.Gas_Smoke;
				GenExplosion.DoExplosion(UI.MouseCell(), Find.VisibleMap, 10f, DamageDefOf.Smoke, null, null, null, null, gas_Smoke, 1f, 1, false, null, 0f, 1);
			});
			base.DebugToolMap("Tool: Lightning strike", (Action)delegate
			{
				Find.VisibleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.VisibleMap, UI.MouseCell()));
			});
			this.DoGap();
			base.DebugToolMap("Tool: Add snow", (Action)delegate
			{
				SnowUtility.AddSnowRadial(UI.MouseCell(), Find.VisibleMap, 5f, 1f);
			});
			base.DebugToolMap("Tool: Remove snow", (Action)delegate
			{
				SnowUtility.AddSnowRadial(UI.MouseCell(), Find.VisibleMap, 5f, -1f);
			});
			base.DebugAction("Clear all snow", (Action)delegate
			{
				foreach (IntVec3 allCell in Find.VisibleMap.AllCells)
				{
					Find.VisibleMap.snowGrid.SetDepth(allCell, 0f);
				}
			});
			base.DebugToolMap("Tool: Push heat (10)", (Action)delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.VisibleMap, 10f);
			});
			base.DebugToolMap("Tool: Push heat (10000)", (Action)delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.VisibleMap, 10000f);
			});
			base.DebugToolMap("Tool: Push heat (-1000)", (Action)delegate
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.VisibleMap, -1000f);
			});
			this.DoGap();
			base.DebugToolMap("Tool: Finish plant growth", (Action)delegate
			{
				foreach (Thing item in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					Plant plant6 = item as Plant;
					if (plant6 != null)
					{
						plant6.Growth = 1f;
					}
				}
			});
			base.DebugToolMap("Tool: Grow 1 day", (Action)delegate
			{
				IntVec3 intVec5 = UI.MouseCell();
				Plant plant5 = intVec5.GetPlant(Find.VisibleMap);
				if (plant5 != null && plant5.def.plant != null)
				{
					int num13 = (int)((1.0 - plant5.Growth) * plant5.def.plant.growDays);
					if (num13 >= 60000)
					{
						plant5.Age += 60000;
					}
					else if (num13 > 0)
					{
						plant5.Age += num13;
					}
					plant5.Growth += (float)(1.0 / plant5.def.plant.growDays);
					if ((double)plant5.Growth > 1.0)
					{
						plant5.Growth = 1f;
					}
					Find.VisibleMap.mapDrawer.SectionAt(intVec5).RegenerateAllLayers();
				}
			});
			base.DebugToolMap("Tool: Grow to maturity", (Action)delegate
			{
				IntVec3 intVec4 = UI.MouseCell();
				Plant plant4 = intVec4.GetPlant(Find.VisibleMap);
				if (plant4 != null && plant4.def.plant != null)
				{
					int num12 = (int)((1.0 - plant4.Growth) * plant4.def.plant.growDays);
					plant4.Age += num12;
					plant4.Growth = 1f;
					Find.VisibleMap.mapDrawer.SectionAt(intVec4).RegenerateAllLayers();
				}
			});
			base.DebugToolMap("Tool: Reproduce present plant", (Action)delegate
			{
				IntVec3 c7 = UI.MouseCell();
				Plant plant2 = c7.GetPlant(Find.VisibleMap);
				if (plant2 != null && plant2.def.plant != null)
				{
					Plant plant3 = GenPlantReproduction.TryReproduceFrom(plant2.Position, plant2.def, SeedTargFindMode.Reproduce, plant2.Map);
					if (plant3 != null)
					{
						Find.VisibleMap.debugDrawer.FlashCell(plant3.Position, 0f, (string)null);
						Find.VisibleMap.debugDrawer.FlashLine(plant2.Position, plant3.Position);
					}
					else
					{
						Find.VisibleMap.debugDrawer.FlashCell(plant2.Position, 0f, (string)null);
					}
				}
			});
			base.DebugToolMap("Tool: Reproduce plant...", (Action)delegate()
			{
				List<FloatMenuOption> list26 = new List<FloatMenuOption>();
				foreach (ThingDef item in from d in DefDatabase<ThingDef>.AllDefs
				where d.category == ThingCategory.Plant && d.plant.reproduces
				select d)
				{
					ThingDef localDef4 = item;
					list26.Add(new FloatMenuOption(localDef4.LabelCap, (Action)delegate
					{
						Plant plant = GenPlantReproduction.TryReproduceFrom(UI.MouseCell(), localDef4, SeedTargFindMode.Reproduce, Find.VisibleMap);
						if (plant != null)
						{
							Find.VisibleMap.debugDrawer.FlashCell(plant.Position, 0f, (string)null);
							Find.VisibleMap.debugDrawer.FlashLine(UI.MouseCell(), plant.Position);
						}
						else
						{
							Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, (string)null);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list26));
			});
			this.DoGap();
			base.DebugToolMap("Tool: Regen section", (Action)delegate
			{
				Find.VisibleMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
			});
			base.DebugToolMap("Tool: Randomize color", (Action)delegate
			{
				foreach (Thing item in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompColorable compColorable = item.TryGetComp<CompColorable>();
					if (compColorable != null)
					{
						item.SetColor(GenColor.RandomColorOpaque(), true);
					}
				}
			});
			base.DebugToolMap("Tool: Rot 1 day", (Action)delegate
			{
				foreach (Thing item in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompRottable compRottable = item.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += 60000f;
					}
				}
			});
			base.DebugToolMap("Tool: Fuel -20%", (Action)delegate
			{
				foreach (Thing item in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompRefuelable compRefuelable = item.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel((float)(compRefuelable.Props.fuelCapacity * 0.20000000298023224));
					}
				}
			});
			base.DebugToolMap("Tool: Break down...", (Action)delegate
			{
				foreach (Thing item in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()))
				{
					CompBreakdownable compBreakdownable = item.TryGetComp<CompBreakdownable>();
					if (compBreakdownable != null && !compBreakdownable.BrokenDown)
					{
						compBreakdownable.DoBreakdown();
					}
				}
			});
			base.DebugAction("Tool: Use scatterer", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_MapGen.Options_Scatterers()));
			});
			base.DebugAction("Tool: BaseGen", (Action)delegate()
			{
				List<DebugMenuOption> list25 = new List<DebugMenuOption>();
				foreach (string item in (from x in DefDatabase<RuleDef>.AllDefs
				select x.symbol).Distinct())
				{
					string localSymbol = item;
					list25.Add(new DebugMenuOption(item, DebugMenuOptionMode.Action, (Action)delegate
					{
						DebugTool tool3 = null;
						tool3 = new DebugTool("first corner...", (Action)delegate
						{
							IntVec3 firstCorner2 = UI.MouseCell();
							DebugTools.curTool = new DebugTool("second corner...", (Action)delegate
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
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list25));
			});
			base.DebugToolMap("Tool: Make roof", (Action)delegate
			{
				CellRect.CellRectIterator iterator2 = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
				while (!iterator2.Done())
				{
					Find.VisibleMap.roofGrid.SetRoof(iterator2.Current, RoofDefOf.RoofConstructed);
					iterator2.MoveNext();
				}
			});
			base.DebugToolMap("Tool: Delete roof", (Action)delegate
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
				while (!iterator.Done())
				{
					Find.VisibleMap.roofGrid.SetRoof(iterator.Current, null);
					iterator.MoveNext();
				}
			});
			base.DebugToolMap("Tool: Toggle trap status", (Action)delegate
			{
				List<Thing>.Enumerator enumerator29 = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList().GetEnumerator();
				try
				{
					while (enumerator29.MoveNext())
					{
						Thing current29 = enumerator29.Current;
						Building_Trap building_Trap = current29 as Building_Trap;
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
				}
				finally
				{
					((IDisposable)(object)enumerator29).Dispose();
				}
			});
			base.DebugToolMap("Tool: Add trap memory", (Action)delegate
			{
				foreach (Faction allFaction in Find.World.factionManager.AllFactions)
				{
					allFaction.TacticalMemory.TrapRevealed(UI.MouseCell(), Find.VisibleMap);
				}
				Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "added");
			});
			base.DebugToolMap("Tool: Test flood unfog", (Action)delegate
			{
				FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.VisibleMap);
			});
			base.DebugToolMap("Tool: Flash closewalk cell 30", (Action)delegate
			{
				IntVec3 c6 = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.VisibleMap, 30, null);
				Find.VisibleMap.debugDrawer.FlashCell(c6, 0f, (string)null);
			});
			base.DebugToolMap("Tool: Flash walk path", (Action)delegate
			{
				WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
			});
			base.DebugToolMap("Tool: Flash skygaze cell", (Action)delegate
			{
				Pawn pawn9 = Find.VisibleMap.mapPawns.FreeColonists.First();
				IntVec3 c5 = default(IntVec3);
				RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn9, out c5);
				Find.VisibleMap.debugDrawer.FlashCell(c5, 0f, (string)null);
				MoteMaker.ThrowText(c5.ToVector3Shifted(), Find.VisibleMap, "for " + pawn9.Label, Color.white, -1f);
			});
			base.DebugToolMap("Tool: Flash direct flee dest", (Action)delegate
			{
				Pawn pawn8 = Find.Selector.SingleSelectedThing as Pawn;
				IntVec3 c4 = default(IntVec3);
				if (pawn8 == null)
				{
					Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "select a pawn");
				}
				else if (RCellFinder.TryFindDirectFleeDestination(UI.MouseCell(), 9f, pawn8, out c4))
				{
					Find.VisibleMap.debugDrawer.FlashCell(c4, 0.5f, (string)null);
				}
				else
				{
					Find.VisibleMap.debugDrawer.FlashCell(UI.MouseCell(), 0.8f, "not found");
				}
			});
			base.DebugAction("Tool: Flash spectators cells", (Action)delegate()
			{
				Action<bool> act4 = (Action<bool>)delegate(bool bestSideOnly)
				{
					DebugTool tool2 = null;
					tool2 = new DebugTool("first watch rect corner...", (Action)delegate()
					{
						IntVec3 firstCorner = UI.MouseCell();
						DebugTools.curTool = new DebugTool("second watch rect corner...", (Action)delegate()
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
				List<DebugMenuOption> list24 = new List<DebugMenuOption>();
				list24.Add(new DebugMenuOption("All sides", DebugMenuOptionMode.Action, (Action)delegate
				{
					act4(false);
				}));
				list24.Add(new DebugMenuOption("Best side only", DebugMenuOptionMode.Action, (Action)delegate
				{
					act4(true);
				}));
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list24));
			});
			base.DebugAction("Tool: Check reachability", (Action)delegate
			{
				List<DebugMenuOption> list23 = new List<DebugMenuOption>();
				TraverseMode[] array3 = (TraverseMode[])Enum.GetValues(typeof(TraverseMode));
				DebugTool tool;
				for (int num11 = 0; num11 < array3.Length; num11++)
				{
					TraverseMode traverseMode2 = array3[num11];
					TraverseMode traverseMode = traverseMode2;
					list23.Add(new DebugMenuOption(((Enum)(object)traverseMode2).ToString(), DebugMenuOptionMode.Action, (Action)delegate
					{
						tool = null;
						tool = new DebugTool("from...", (Action)delegate
						{
							IntVec3 from = UI.MouseCell();
							Pawn fromPawn = from.GetFirstPawn(Find.VisibleMap);
							string text2 = "to...";
							if (fromPawn != null)
							{
								text2 = text2 + " (pawn=" + fromPawn.LabelShort + ")";
							}
							DebugTools.curTool = new DebugTool(text2, (Action)delegate
							{
								DebugTools.curTool = tool;
							}, (Action)delegate
							{
								IntVec3 c3 = UI.MouseCell();
								bool flag2;
								IntVec3 intVec3;
								if (fromPawn != null)
								{
									TraverseMode mode = traverseMode;
									flag2 = fromPawn.CanReach(c3, PathEndMode.OnCell, Danger.Deadly, false, mode);
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
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list23));
			});
			base.DebugToolMapForPawns("Tool: Flash TryFindRandomPawnExitCell", (Action<Pawn>)delegate(Pawn p)
			{
				IntVec3 intVec2 = default(IntVec3);
				if (CellFinder.TryFindRandomPawnExitCell(p, out intVec2))
				{
					p.Map.debugDrawer.FlashCell(intVec2, 0.5f, (string)null);
					p.Map.debugDrawer.FlashLine(p.Position, intVec2);
				}
				else
				{
					p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no exit cell");
				}
			});
			base.DebugToolMapForPawns("Tool: RandomSpotJustOutsideColony", (Action<Pawn>)delegate(Pawn p)
			{
				IntVec3 intVec = default(IntVec3);
				if (RCellFinder.TryFindRandomSpotJustOutsideColony(p, out intVec))
				{
					p.Map.debugDrawer.FlashCell(intVec, 0.5f, (string)null);
					p.Map.debugDrawer.FlashLine(p.Position, intVec);
				}
				else
				{
					p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no cell");
				}
			});
			this.DoGap();
			this.DoLabel("Tools - Pawns");
			base.DebugToolMapForPawns("Tool: Damage to down", (Action<Pawn>)delegate(Pawn p)
			{
				HealthUtility.DamageUntilDowned(p);
			});
			base.DebugToolMapForPawns("Tool: Damage to death", (Action<Pawn>)delegate(Pawn p)
			{
				HealthUtility.DamageUntilDead(p);
			});
			base.DebugToolMap("Tool: Damage held pawn to death", (Action)delegate
			{
				List<Thing>.Enumerator enumerator27 = Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).ToList().GetEnumerator();
				try
				{
					while (enumerator27.MoveNext())
					{
						Thing current27 = enumerator27.Current;
						Pawn pawn7 = current27 as Pawn;
						if (pawn7 != null && pawn7.carryTracker.CarriedThing != null && pawn7.carryTracker.CarriedThing is Pawn)
						{
							HealthUtility.DamageUntilDead((Pawn)pawn7.carryTracker.CarriedThing);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator27).Dispose();
				}
			});
			base.DebugToolMapForPawns("Tool: Surgery fail minor", (Action<Pawn>)delegate(Pawn p)
			{
				BodyPartRecord bodyPartRecord2 = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).RandomElement();
				Log.Message("part is " + bodyPartRecord2);
				HealthUtility.GiveInjuriesOperationFailureMinor(p, bodyPartRecord2);
			});
			base.DebugToolMapForPawns("Tool: Surgery fail catastrophic", (Action<Pawn>)delegate(Pawn p)
			{
				BodyPartRecord bodyPartRecord = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).RandomElement();
				Log.Message("part is " + bodyPartRecord);
				HealthUtility.GiveInjuriesOperationFailureCatastrophic(p, bodyPartRecord);
			});
			base.DebugToolMapForPawns("Tool: Surgery fail ridiculous", (Action<Pawn>)delegate(Pawn p)
			{
				HealthUtility.GiveInjuriesOperationFailureRidiculous(p);
			});
			base.DebugToolMapForPawns("Tool: Restore body part...", (Action<Pawn>)delegate(Pawn p)
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
			});
			base.DebugAction("Tool: Apply damage...", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_ApplyDamage()));
			});
			base.DebugAction("Tool: Add Hediff...", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff()));
			});
			base.DebugToolMapForPawns("Tool: Heal random injury (10)", (Action<Pawn>)delegate(Pawn p)
			{
				Hediff_Injury hediff_Injury = default(Hediff_Injury);
				if ((from x in p.health.hediffSet.GetHediffs<Hediff_Injury>()
				where x.CanHealNaturally() || x.CanHealFromTending()
				select x).TryRandomElement<Hediff_Injury>(out hediff_Injury))
				{
					hediff_Injury.Heal(10f);
				}
			});
			base.DebugToolMapForPawns("Tool: Activate HediffGiver", (Action<Pawn>)delegate(Pawn p)
			{
				List<FloatMenuOption> list22 = new List<FloatMenuOption>();
				if (p.RaceProps.hediffGiverSets != null)
				{
					foreach (HediffGiver item in p.RaceProps.hediffGiverSets.SelectMany((Func<HediffGiverSetDef, IEnumerable<HediffGiver>>)((HediffGiverSetDef set) => set.hediffGivers)))
					{
						HediffGiver localHdg = item;
						list22.Add(new FloatMenuOption(localHdg.hediff.defName, (Action)delegate()
						{
							localHdg.TryApply(p, null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				if (list22.Any())
				{
					Find.WindowStack.Add(new FloatMenu(list22));
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Give birth", (Action<Pawn>)delegate(Pawn p)
			{
				Hediff_Pregnant.DoBirthSpawn(p, null);
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("Tool: Add/remove pawn relation", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.RaceProps.IsFlesh)
				{
					Action<bool> act3 = (Action<bool>)delegate(bool add)
					{
						if (add)
						{
							List<DebugMenuOption> list19 = new List<DebugMenuOption>();
							foreach (PawnRelationDef allDef in DefDatabase<PawnRelationDef>.AllDefs)
							{
								if (!allDef.implied)
								{
									PawnRelationDef defLocal = allDef;
									list19.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, (Action)delegate()
									{
										List<DebugMenuOption> list21 = new List<DebugMenuOption>();
										IOrderedEnumerable<Pawn> orderedEnumerable = from x in PawnsFinder.AllMapsAndWorld_Alive
										where x.RaceProps.IsFlesh
										orderby x.def == p.def descending, x.IsWorldPawn()
										select x;
										foreach (Pawn item in orderedEnumerable)
										{
											if (p != item && (!defLocal.familyByBloodRelation || item.def == p.def) && !p.relations.DirectRelationExists(defLocal, item))
											{
												Pawn otherLocal3 = item;
												list21.Add(new DebugMenuOption(otherLocal3.LabelShort + " (" + otherLocal3.KindLabel + ")", DebugMenuOptionMode.Action, (Action)delegate()
												{
													_003CDoListingItems_MapTools_003Ec__AnonStorey5A6._003CDoListingItems_MapTools_003Ec__AnonStorey5A7 _003CDoListingItems_MapTools_003Ec__AnonStorey5A;
													p.relations.AddDirectRelation(_003CDoListingItems_MapTools_003Ec__AnonStorey5A.defLocal, otherLocal3);
												}));
											}
										}
										Find.WindowStack.Add(new Dialog_DebugOptionListLister(list21));
									}));
								}
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list19));
						}
						else
						{
							List<DebugMenuOption> list20 = new List<DebugMenuOption>();
							List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
							for (int num10 = 0; num10 < directRelations.Count; num10++)
							{
								DirectPawnRelation rel = directRelations[num10];
								list20.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, (Action)delegate()
								{
									p.relations.RemoveDirectRelation(rel);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list20));
						}
					};
					List<DebugMenuOption> list18 = new List<DebugMenuOption>();
					list18.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, (Action)delegate
					{
						act3(true);
					}));
					list18.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, (Action)delegate
					{
						act3(false);
					}));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list18));
				}
			});
			base.DebugToolMapForPawns("Tool: Add opinion thoughts about", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike)
				{
					Action<bool> act2 = (Action<bool>)delegate(bool good)
					{
						foreach (Pawn item in from x in p.Map.mapPawns.AllPawnsSpawned
						where x.RaceProps.Humanlike
						select x)
						{
							if (p != item)
							{
								IEnumerable<ThoughtDef> source3 = DefDatabase<ThoughtDef>.AllDefs.Where((Func<ThoughtDef, bool>)((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0.0) || (!good && x.stages[0].baseOpinionOffset < 0.0))));
								if (source3.Any())
								{
									int num8 = Rand.Range(2, 5);
									for (int num9 = 0; num9 < num8; num9++)
									{
										ThoughtDef def2 = source3.RandomElement();
										item.needs.mood.thoughts.memories.TryGainMemory(def2, p);
									}
								}
							}
						}
					};
					List<DebugMenuOption> list17 = new List<DebugMenuOption>();
					list17.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, (Action)delegate
					{
						act2(true);
					}));
					list17.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, (Action)delegate
					{
						act2(false);
					}));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list17));
				}
			});
			base.DebugToolMapForPawns("Tool: Force vomit...", (Action<Pawn>)delegate(Pawn p)
			{
				p.jobs.StartJob(new Job(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, default(JobTag?));
			});
			base.DebugToolMap("Tool: Food -20%", (Action)delegate
			{
				this.OffsetNeed(NeedDefOf.Food, -0.2f);
			});
			base.DebugToolMap("Tool: Rest -20%", (Action)delegate
			{
				this.OffsetNeed(NeedDefOf.Rest, -0.2f);
			});
			base.DebugToolMap("Tool: Joy -20%", (Action)delegate
			{
				this.OffsetNeed(NeedDefOf.Joy, -0.2f);
			});
			base.DebugToolMap("Tool: Chemical -20%", (Action)delegate
			{
				List<NeedDef> allDefsListForReading2 = DefDatabase<NeedDef>.AllDefsListForReading;
				for (int num7 = 0; num7 < allDefsListForReading2.Count; num7++)
				{
					if (typeof(Need_Chemical).IsAssignableFrom(allDefsListForReading2[num7].needClass))
					{
						this.OffsetNeed(allDefsListForReading2[num7], -0.2f);
					}
				}
			});
			base.DebugToolMapForPawns("Tool: Set skill", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.skills != null)
				{
					List<DebugMenuOption> list15 = new List<DebugMenuOption>();
					foreach (SkillDef allDef in DefDatabase<SkillDef>.AllDefs)
					{
						SkillDef localDef3 = allDef;
						list15.Add(new DebugMenuOption(localDef3.defName, DebugMenuOptionMode.Action, (Action)delegate()
						{
							List<DebugMenuOption> list16 = new List<DebugMenuOption>();
							for (int num6 = 0; num6 <= 20; num6++)
							{
								int level = num6;
								list16.Add(new DebugMenuOption(level.ToString(), DebugMenuOptionMode.Action, (Action)delegate()
								{
									SkillRecord skill = p.skills.GetSkill(localDef3);
									skill.Level = level;
									skill.xpSinceLastLevel = (float)(skill.XpRequiredForLevelUp / 2.0);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list16));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list15));
				}
			});
			base.DebugToolMapForPawns("Tool: Max skills", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.skills != null)
				{
					foreach (SkillDef allDef in DefDatabase<SkillDef>.AllDefs)
					{
						p.skills.Learn(allDef, 1E+08f, false);
					}
					this.DustPuffFrom(p);
				}
				if (p.training != null)
				{
					foreach (TrainableDef allDef2 in DefDatabase<TrainableDef>.AllDefs)
					{
						Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement();
						bool flag = default(bool);
						if (p.training.CanAssignToTrain(allDef2, out flag).Accepted)
						{
							p.training.Train(allDef2, trainer);
						}
					}
				}
			});
			base.DebugAction("Tool: Mental break...", (Action)delegate()
			{
				List<DebugMenuOption> list14 = new List<DebugMenuOption>();
				list14.Add(new DebugMenuOption("(log possibles)", DebugMenuOptionMode.Tool, (Action)delegate()
				{
					foreach (Pawn item in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						item.mindState.mentalBreaker.LogPossibleMentalBreaks();
						this.DustPuffFrom(item);
					}
				}));
				list14.Add(new DebugMenuOption("(natural mood break)", DebugMenuOptionMode.Tool, (Action)delegate()
				{
					foreach (Pawn item in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						item.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
						this.DustPuffFrom(item);
					}
				}));
				foreach (MentalStateDef allDef in DefDatabase<MentalStateDef>.AllDefs)
				{
					MentalStateDef locBrDef = allDef;
					string text = locBrDef.defName;
					if (!Find.VisibleMap.mapPawns.FreeColonists.Any((Func<Pawn, bool>)((Pawn x) => locBrDef.Worker.StateCanOccur(x))))
					{
						text += " [NO]";
					}
					list14.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, (Action)delegate()
					{
						foreach (Pawn item in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							Pawn locP = item;
							if (locBrDef != MentalStateDefOf.SocialFighting)
							{
								locP.mindState.mentalStateHandler.TryStartMentalState(locBrDef, (string)null, true, false, null);
								this.DustPuffFrom(locP);
							}
							else
							{
								DebugTools.curTool = new DebugTool("...with", (Action)delegate()
								{
									Pawn pawn6 = (Pawn)(from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
									where t is Pawn
									select t).FirstOrDefault();
									if (pawn6 != null)
									{
										if (!InteractionUtility.HasAnySocialFightProvokingThought(locP, pawn6))
										{
											locP.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Insulted, pawn6);
											Messages.Message("Dev: auto added negative thought.", (Thing)locP, MessageSound.Standard);
										}
										locP.interactions.StartSocialFight(pawn6);
										DebugTools.curTool = null;
									}
								}, null);
							}
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list14));
			});
			base.DebugAction("Tool: Give trait...", (Action)delegate()
			{
				List<DebugMenuOption> list13 = new List<DebugMenuOption>();
				foreach (TraitDef allDef in DefDatabase<TraitDef>.AllDefs)
				{
					TraitDef trDef = allDef;
					for (int num5 = 0; num5 < allDef.degreeDatas.Count; num5++)
					{
						int i2 = num5;
						list13.Add(new DebugMenuOption(trDef.degreeDatas[i2].label + " (" + trDef.degreeDatas[num5].degree + ")", DebugMenuOptionMode.Tool, (Action)delegate()
						{
							foreach (Pawn item2 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).Cast<Pawn>())
							{
								if (item2.story != null)
								{
									Trait item = new Trait(trDef, trDef.degreeDatas[i2].degree, false);
									item2.story.traits.allTraits.Add(item);
									this.DustPuffFrom(item2);
								}
							}
						}));
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list13));
			});
			base.DebugToolMapForPawns("Tool: Give good thought", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.needs.mood != null)
				{
					p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null);
				}
			});
			base.DebugToolMapForPawns("Tool: Give bad thought", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.needs.mood != null)
				{
					p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null);
				}
			});
			base.DebugToolMapForPawns("Tool: Make faction hostile", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.Faction != null && !p.Faction.HostileTo(Faction.OfPlayer))
				{
					p.Faction.SetHostileTo(Faction.OfPlayer, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Make faction neutral", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.Faction != null && p.Faction.HostileTo(Faction.OfPlayer))
				{
					p.Faction.SetHostileTo(Faction.OfPlayer, false);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMap("Tool: Clear bound unfinished things", (Action)delegate()
			{
				foreach (Building_WorkTable item in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Building_WorkTable
				select t).Cast<Building_WorkTable>())
				{
					foreach (Bill item2 in item.BillStack)
					{
						Bill_ProductionWithUft bill_ProductionWithUft = item2 as Bill_ProductionWithUft;
						if (bill_ProductionWithUft != null)
						{
							bill_ProductionWithUft.ClearBoundUft();
						}
					}
				}
			});
			base.DebugToolMapForPawns("Tool: Force birthday", (Action<Pawn>)delegate(Pawn p)
			{
				p.ageTracker.AgeBiologicalTicks = (p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1;
				p.ageTracker.DebugForceBirthdayBiological();
			});
			base.DebugToolMapForPawns("Tool: Recruit", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement(), p, 1f, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Damage apparel", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.apparel != null && p.apparel.WornApparelCount > 0)
				{
					p.apparel.WornApparel.RandomElement().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Tame animal", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.RaceProps.Animal && p.Faction != Faction.OfPlayer)
				{
					InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault(), p, 1f, true);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Train animal", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
				{
					foreach (TrainableDef allDef in DefDatabase<TrainableDef>.AllDefs)
					{
						while (!p.training.IsCompleted(allDef))
						{
							p.training.Train(allDef, null);
						}
					}
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Name animal by nuzzling", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.Name != null && !p.Name.Numerical)
					return;
				if (p.RaceProps.Animal)
				{
					PawnUtility.GiveNameBecauseOfNuzzle(p.Map.mapPawns.FreeColonists.First(), p);
					this.DustPuffFrom(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Try develop bond relation", (Action<Pawn>)delegate(Pawn p)
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
			base.DebugToolMapForPawns("Tool: Start marriage ceremony", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.RaceProps.Humanlike)
				{
					List<DebugMenuOption> list12 = new List<DebugMenuOption>();
					foreach (Pawn item in from x in p.Map.mapPawns.AllPawnsSpawned
					where x.RaceProps.Humanlike
					select x)
					{
						if (p != item)
						{
							Pawn otherLocal2 = item;
							list12.Add(new DebugMenuOption(otherLocal2.LabelShort + " (" + otherLocal2.KindLabel + ")", DebugMenuOptionMode.Action, (Action)delegate()
							{
								if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal2))
								{
									p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal2);
									p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal2);
									p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal2);
									Messages.Message("Dev: auto added fiance relation.", (Thing)p, MessageSound.Standard);
								}
								if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal2))
								{
									Messages.Message("Could not find any valid marriage site.", MessageSound.Negative);
								}
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list12));
				}
			});
			base.DebugToolMapForPawns("Tool: Force interaction", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.Faction != null)
				{
					List<DebugMenuOption> list10 = new List<DebugMenuOption>();
					List<Pawn>.Enumerator enumerator8 = p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction).GetEnumerator();
					try
					{
						while (enumerator8.MoveNext())
						{
							Pawn current8 = enumerator8.Current;
							if (current8 != p)
							{
								Pawn otherLocal = current8;
								list10.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, (Action)delegate()
								{
									List<DebugMenuOption> list11 = new List<DebugMenuOption>();
									List<InteractionDef>.Enumerator enumerator9 = DefDatabase<InteractionDef>.AllDefsListForReading.GetEnumerator();
									try
									{
										while (enumerator9.MoveNext())
										{
											InteractionDef current9 = enumerator9.Current;
											InteractionDef interactionLocal = current9;
											list11.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, (Action)delegate()
											{
												p.interactions.TryInteractWith(otherLocal, interactionLocal);
											}));
										}
									}
									finally
									{
										((IDisposable)(object)enumerator9).Dispose();
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list11));
								}));
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator8).Dispose();
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list10));
				}
			});
			base.DebugAction("Tool: Start party", (Action)delegate
			{
				if (!Find.VisibleMap.lordsStarter.TryStartParty())
				{
					Messages.Message("Could not find any valid party spot or organizer.", MessageSound.Negative);
				}
			});
			base.DebugToolMapForPawns("Tool: Start prison break", (Action<Pawn>)delegate(Pawn p)
			{
				if (p.IsPrisoner)
				{
					PrisonBreakUtility.StartPrisonBreak(p);
				}
			});
			base.DebugToolMapForPawns("Tool: Pass to world", (Action<Pawn>)delegate(Pawn p)
			{
				p.DeSpawn();
				Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
			});
			base.DebugToolMapForPawns("Tool: Make 1 year older", (Action<Pawn>)delegate(Pawn p)
			{
				p.ageTracker.DebugMake1YearOlder();
			});
			this.DoGap();
			base.DebugToolMapForPawns("Tool: Try job giver", (Action<Pawn>)delegate(Pawn p)
			{
				List<DebugMenuOption> list9 = new List<DebugMenuOption>();
				foreach (Type item in typeof(ThinkNode_JobGiver).AllSubclasses())
				{
					Type localType = item;
					list9.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, (Action)delegate()
					{
						ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(localType);
						ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(p, default(JobIssueParams));
						if (thinkResult.Job != null)
						{
							p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, default(JobTag?));
						}
						else
						{
							Messages.Message("Failed to give job", MessageSound.Silent);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list9));
			});
			base.DebugToolMapForPawns("Tool: EndCurrentJob(" + ((Enum)(object)JobCondition.InterruptForced).ToString() + ")", (Action<Pawn>)delegate(Pawn p)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("Tool: CheckForJobOverride", (Action<Pawn>)delegate(Pawn p)
			{
				p.jobs.CheckForJobOverride();
				this.DustPuffFrom(p);
			});
			base.DebugToolMapForPawns("Tool: Toggle job logging", (Action<Pawn>)delegate(Pawn p)
			{
				p.jobs.debugLog = !p.jobs.debugLog;
				this.DustPuffFrom(p);
				MoteMaker.ThrowText(p.DrawPos, p.Map, p.LabelShort + "\n" + ((!p.jobs.debugLog) ? "OFF" : "ON"), -1f);
			});
			base.DebugToolMapForPawns("Tool: Toggle stance logging", (Action<Pawn>)delegate(Pawn p)
			{
				p.stances.debugLog = !p.stances.debugLog;
				this.DustPuffFrom(p);
			});
			this.DoGap();
			this.DoLabel("Tools - Spawning");
			base.DebugAction("Tool: Spawn pawn", (Action)delegate()
			{
				List<DebugMenuOption> list8 = new List<DebugMenuOption>();
				foreach (PawnKindDef item in from kd in DefDatabase<PawnKindDef>.AllDefs
				orderby kd.defName
				select kd)
				{
					PawnKindDef localKindDef = item;
					list8.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Tool, (Action)delegate()
					{
						Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
						Pawn newPawn = PawnGenerator.GeneratePawn(localKindDef, faction);
						GenSpawn.Spawn(newPawn, UI.MouseCell(), Find.VisibleMap);
						if (faction != null && faction != Faction.OfPlayer)
						{
							Lord lord = null;
							if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Predicate<Pawn>)((Pawn p) => p != newPawn)))
							{
								Predicate<Thing> validator = (Predicate<Thing>)((Thing p) => p != newPawn && ((Pawn)p).GetLord() != null);
								Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, validator);
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
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list8));
			});
			base.DebugAction("Tool: Spawn weapon...", (Action)delegate()
			{
				List<DebugMenuOption> list7 = new List<DebugMenuOption>();
				foreach (ThingDef item in from def in DefDatabase<ThingDef>.AllDefs
				where def.equipmentType == EquipmentType.Primary
				select def)
				{
					ThingDef localDef2 = item;
					list7.Add(new DebugMenuOption(localDef2.LabelCap, DebugMenuOptionMode.Tool, (Action)delegate
					{
						DebugThingPlaceHelper.DebugSpawn(localDef2, UI.MouseCell(), -1, false);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list7));
			});
			base.DebugAction("Tool: Try place near thing...", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)));
			});
			base.DebugAction("Tool: Try place near stacks of 25...", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)));
			});
			base.DebugAction("Tool: Try place near stacks of 75...", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)));
			});
			base.DebugAction("Tool: Try place direct thing...", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)));
			});
			base.DebugAction("Tool: Try place direct stacks of 25...", (Action)delegate
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)));
			});
			base.DebugAction("Tool: Set terrain...", (Action)delegate
			{
				List<DebugMenuOption> list6 = new List<DebugMenuOption>();
				foreach (TerrainDef allDef in DefDatabase<TerrainDef>.AllDefs)
				{
					TerrainDef localDef = allDef;
					list6.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, (Action)delegate
					{
						if (UI.MouseCell().InBounds(Find.VisibleMap))
						{
							Find.VisibleMap.terrainGrid.SetTerrain(UI.MouseCell(), localDef);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list6));
			});
			base.DebugToolMap("Tool: Make filth x100", (Action)delegate
			{
				for (int num4 = 0; num4 < 100; num4++)
				{
					IntVec3 c2 = UI.MouseCell() + GenRadial.RadialPattern[num4];
					if (c2.InBounds(Find.VisibleMap) && c2.Walkable(Find.VisibleMap))
					{
						FilthMaker.MakeFilth(c2, Find.VisibleMap, ThingDefOf.FilthDirt, 2);
						MoteMaker.ThrowMetaPuff(c2.ToVector3Shifted(), Find.VisibleMap);
					}
				}
			});
			base.DebugToolMap("Tool: Spawn faction leader", (Action)delegate
			{
				List<FloatMenuOption> list5 = new List<FloatMenuOption>();
				foreach (Faction allFaction in Find.FactionManager.AllFactions)
				{
					Faction localFac = allFaction;
					if (localFac.leader != null)
					{
						list5.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, (Action)delegate
						{
							GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.VisibleMap);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list5));
			});
			base.DebugAction("Spawn world pawn...", (Action)delegate()
			{
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				Action<Pawn> act = (Action<Pawn>)delegate(Pawn p)
				{
					List<DebugMenuOption> list4 = new List<DebugMenuOption>();
					foreach (PawnKindDef item in from x in DefDatabase<PawnKindDef>.AllDefs
					where x.race == p.def
					select x)
					{
						PawnKindDef kLocal = item;
						list4.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, (Action)delegate()
						{
							PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
							PawnGenerator.RedressPawn(p, request);
							GenSpawn.Spawn(p, UI.MouseCell(), Find.VisibleMap);
							DebugTools.curTool = null;
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
				};
				foreach (Pawn item2 in Find.WorldPawns.AllPawnsAlive)
				{
					Pawn pLocal = item2;
					list3.Add(new DebugMenuOption(item2.LabelShort, DebugMenuOptionMode.Action, (Action)delegate
					{
						act(pLocal);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
			});
			base.DebugAction("Spawn item collection...", (Action)delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<ItemCollectionGeneratorDef> allDefsListForReading = DefDatabase<ItemCollectionGeneratorDef>.AllDefsListForReading;
				for (int m = 0; m < allDefsListForReading.Count; m++)
				{
					ItemCollectionGeneratorDef localGenerator = allDefsListForReading[m];
					list.Add(new DebugMenuOption(localGenerator.defName, DebugMenuOptionMode.Tool, (Action)delegate
					{
						if (UI.MouseCell().InBounds(Find.VisibleMap))
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							List<Thing> list2 = localGenerator.Worker.GenerateRandomTestItems();
							stringBuilder2.AppendLine(localGenerator.Worker.GetType().Name + " generated " + list2.Count + " things:");
							float num3 = 0f;
							for (int n = 0; n < list2.Count; n++)
							{
								stringBuilder2.AppendLine("   - " + list2[n].LabelCap);
								num3 += list2[n].MarketValue * (float)list2[n].stackCount;
								if (!GenPlace.TryPlaceThing(list2[n], UI.MouseCell(), Find.VisibleMap, ThingPlaceMode.Near, null))
								{
									list2[n].Destroy(DestroyMode.Vanish);
								}
							}
							stringBuilder2.AppendLine("Total market value: " + num3.ToString("0.##"));
							Log.Message(stringBuilder2.ToString());
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
			this.DoGap();
			this.DoLabel("Autotests");
			base.DebugAction("Make colony (full)", (Action)delegate
			{
				Autotests_ColonyMaker.MakeColony_Full();
			});
			base.DebugAction("Make colony (animals)", (Action)delegate
			{
				Autotests_ColonyMaker.MakeColony_Animals();
			});
			base.DebugAction("Test force downed x100", (Action)delegate()
			{
				int num2 = 0;
				Pawn pawn5;
				while (true)
				{
					if (num2 < 100)
					{
						PawnKindDef random5 = DefDatabase<PawnKindDef>.GetRandom();
						pawn5 = PawnGenerator.GeneratePawn(random5, FactionUtility.DefaultFactionFrom(random5.defaultFactionType));
						GenSpawn.Spawn(pawn5, CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(Find.VisibleMap)), Find.VisibleMap, 1000), Find.VisibleMap);
						HealthUtility.DamageUntilDowned(pawn5);
						if (!pawn5.Dead)
						{
							num2++;
							continue;
						}
						break;
					}
					return;
				}
				Log.Error("Pawn died while force downing: " + pawn5 + " at " + pawn5.Position);
			});
			base.DebugAction("Test force kill x100", (Action)delegate()
			{
				int num = 0;
				Pawn pawn4;
				while (true)
				{
					if (num < 100)
					{
						PawnKindDef random4 = DefDatabase<PawnKindDef>.GetRandom();
						pawn4 = PawnGenerator.GeneratePawn(random4, FactionUtility.DefaultFactionFrom(random4.defaultFactionType));
						GenSpawn.Spawn(pawn4, CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(Find.VisibleMap)), Find.VisibleMap, 1000), Find.VisibleMap);
						HealthUtility.DamageUntilDead(pawn4);
						if (pawn4.Dead)
						{
							num++;
							continue;
						}
						break;
					}
					return;
				}
				Log.Error("Pawn died not die: " + pawn4 + " at " + pawn4.Position);
			});
			base.DebugAction("Test Surgery fail catastrophic x100", (Action)delegate()
			{
				for (int l = 0; l < 100; l++)
				{
					PawnKindDef random3 = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn3 = PawnGenerator.GeneratePawn(random3, FactionUtility.DefaultFactionFrom(random3.defaultFactionType));
					GenSpawn.Spawn(pawn3, CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(Find.VisibleMap)), Find.VisibleMap, 1000), Find.VisibleMap);
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
			base.DebugAction("Test Surgery fail ridiculous x100", (Action)delegate()
			{
				for (int k = 0; k < 100; k++)
				{
					PawnKindDef random2 = DefDatabase<PawnKindDef>.GetRandom();
					Pawn pawn2 = PawnGenerator.GeneratePawn(random2, FactionUtility.DefaultFactionFrom(random2.defaultFactionType));
					GenSpawn.Spawn(pawn2, CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(Find.VisibleMap)), Find.VisibleMap, 1000), Find.VisibleMap);
					pawn2.health.forceIncap = true;
					HealthUtility.GiveInjuriesOperationFailureRidiculous(pawn2);
					pawn2.health.forceIncap = false;
					if (pawn2.Dead)
					{
						Log.Error("Pawn died: " + pawn2 + " at " + pawn2.Position);
					}
				}
			});
			base.DebugAction("Test generate pawn x1000", (Action)delegate()
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
					array2[array.FirstIndexOf((Func<float, bool>)((float time) => ms <= time))]++;
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
			base.DebugAction("Check region listers", (Action)delegate
			{
				Autotests_RegionListers.CheckBugs(Find.VisibleMap);
			});
		}

		private void DoListingItems_World()
		{
			this.DoLabel("Tools - World");
			Text.Font = GameFont.Tiny;
			this.DoLabel("Incidents");
			IIncidentTarget altTarget = Find.WorldSelector.SingleSelectedObject as IIncidentTarget;
			this.DoExecuteIncidentDebugAction(Find.World, altTarget);
			this.DoExecuteIncidentWithDebugAction(Find.World, altTarget);
			this.DoLabel("Tools - Spawning");
			base.DebugToolWorld("Spawn random caravan", (Action)delegate()
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
					for (int num4 = 0; num4 < num3; num4++)
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
					for (int i = 0; i < list3.Count; i++)
					{
						CaravanInventoryUtility.GiveThing(caravan, list3[i]);
					}
				}
			});
			base.DebugToolWorld("Spawn random faction base", (Action)delegate()
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
			base.DebugToolWorld("Spawn site", (Action)delegate
			{
				int tile = GenWorld.MouseTile(false);
				if (tile < 0 || Find.World.Impassable(tile))
				{
					Messages.Message("Impassable", MessageSound.RejectInput);
				}
				else
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					List<SitePartDef> parts = new List<SitePartDef>();
					foreach (SiteCoreDef allDef in DefDatabase<SiteCoreDef>.AllDefs)
					{
						SiteCoreDef localDef = allDef;
						Action addPart = null;
						addPart = (Action)delegate
						{
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							list2.Add(new DebugMenuOption("-Done (" + parts.Count + " parts)-", DebugMenuOptionMode.Action, (Action)delegate
							{
								Site site = SiteMaker.TryMakeSite(localDef, parts, true, null);
								if (site == null)
								{
									Messages.Message("Could not find any valid faction for this site.", MessageSound.RejectInput);
								}
								else
								{
									site.Tile = tile;
									Find.WorldObjects.Add(site);
								}
							}));
							foreach (SitePartDef allDef in DefDatabase<SitePartDef>.AllDefs)
							{
								SitePartDef localPart = allDef;
								list2.Add(new DebugMenuOption(allDef.defName, DebugMenuOptionMode.Action, (Action)delegate
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

		private void DoLabel(string label)
		{
			base.listing.Label(label, -1f);
			base.totalOptionsHeight += (float)(Text.CalcHeight(label, 300f) + 2.0);
		}

		private void DoGap()
		{
			base.listing.Gap(7f);
			base.totalOptionsHeight += 7f;
		}

		private void DoRaid(IncidentParms parms)
		{
			IncidentDef incidentDef = (!parms.faction.HostileTo(Faction.OfPlayer)) ? IncidentDefOf.RaidFriendly : IncidentDefOf.RaidEnemy;
			incidentDef.Worker.TryExecute(parms);
		}

		private void DoExecuteIncidentDebugAction(IIncidentTarget target, IIncidentTarget altTarget)
		{
			base.DebugAction("Execute incident...", (Action)delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef item in from d in DefDatabase<IncidentDef>.AllDefs
				where d.TargetAllowed(target) || (altTarget != null && d.TargetAllowed(altTarget))
				orderby !d.TargetAllowed(target), d.defName
				select d)
				{
					object obj;
					if (item.TargetAllowed(target))
					{
						IIncidentTarget incidentTarget = target;
						obj = incidentTarget;
					}
					else
					{
						obj = altTarget;
					}
					IIncidentTarget thisIncidentTarget = (IIncidentTarget)obj;
					IncidentDef localDef = item;
					string text = localDef.defName;
					if (!localDef.Worker.CanFireNow(thisIncidentTarget))
					{
						text += " [NO]";
					}
					if (thisIncidentTarget == altTarget)
					{
						text = text + " (" + altTarget.GetType().Name.Truncate(52f, null) + ")";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, (Action)delegate()
					{
						IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, localDef.category, thisIncidentTarget);
						if (localDef.pointsScaleable)
						{
							StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((Func<StorytellerComp, bool>)((StorytellerComp x) => x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain));
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
			base.DebugAction("Execute incident with...", (Action)delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef item in from d in DefDatabase<IncidentDef>.AllDefs
				where (d.TargetAllowed(target) || (altTarget != null && d.TargetAllowed(altTarget))) && d.pointsScaleable
				orderby !d.TargetAllowed(target), d.defName
				select d)
				{
					object obj;
					if (item.TargetAllowed(target))
					{
						IIncidentTarget incidentTarget = target;
						obj = incidentTarget;
					}
					else
					{
						obj = altTarget;
					}
					IIncidentTarget thisIncidentTarget = (IIncidentTarget)obj;
					IncidentDef localDef = item;
					string text = localDef.defName;
					if (!localDef.Worker.CanFireNow(thisIncidentTarget))
					{
						text += " [NO]";
					}
					if (thisIncidentTarget == altTarget)
					{
						text = text + " (" + altTarget.GetType().Name.Truncate(52f, null) + ")";
					}
					list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, (Action)delegate
					{
						IncidentParms parms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, localDef.category, thisIncidentTarget);
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float item in Dialog_DebugActionsMenu.PointsOptions())
						{
							float num = item;
							float localPoints = num;
							list2.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, (Action)delegate
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
			Messages.Message("Made " + num2 + " " + resDef + " near " + pawn + ".", MessageSound.Benefit);
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
			using (IEnumerator<Building_Bed> enumerator = Find.VisibleMap.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>().GetEnumerator())
			{
				Building_Bed current;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						current = enumerator.Current;
						if (current.ForPrisoners == prisoner)
						{
							if (!current.owners.Any())
								break;
							if (prisoner && current.AnyUnownedSleepingSlot)
								break;
						}
						continue;
					}
					return;
				}
				PawnKindDef pawnKindDef = prisoner ? (from pk in DefDatabase<PawnKindDef>.AllDefs
				where pk.defaultFactionType != null && !pk.defaultFactionType.isPlayer && pk.RaceProps.Humanlike
				select pk).RandomElement() : PawnKindDefOf.SpaceRefugee;
				Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
				Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction);
				GenSpawn.Spawn(pawn, current.Position, Find.VisibleMap);
				List<ThingWithComps>.Enumerator enumerator2 = pawn.equipment.AllEquipmentListForReading.ToList().GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						ThingWithComps current2 = enumerator2.Current;
						ThingWithComps thingWithComps = default(ThingWithComps);
						if (pawn.equipment.TryDropEquipment(current2, out thingWithComps, pawn.Position, true))
						{
							thingWithComps.Destroy(DestroyMode.Vanish);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
				pawn.inventory.innerContainer.Clear();
				pawn.ownership.ClaimBedIfNonMedical(current);
				pawn.guest.SetGuestStatus(Faction.OfPlayer, prisoner);
			}
		}

		public static IEnumerable<float> PointsOptions()
		{
			yield return 35f;
			yield return 70f;
			yield return 135f;
			yield return 200f;
			yield return 300f;
			yield return 500f;
			yield return 800f;
			yield return 1200f;
			yield return 2000f;
			yield return 3000f;
			yield return 4000f;
		}
	}
}
