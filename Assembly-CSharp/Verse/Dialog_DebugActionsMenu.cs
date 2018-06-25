using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
	public class Dialog_DebugActionsMenu : Dialog_DebugOptionLister
	{
		private static List<PawnKindDef> pawnKindsForDamageTypeBattleRoyale = null;

		private static Map mapLeak = null;

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		[CompilerGenerated]
		private static Action <>f__am$cache1;

		[CompilerGenerated]
		private static Action <>f__am$cache2;

		[CompilerGenerated]
		private static Action <>f__am$cache3;

		[CompilerGenerated]
		private static Action <>f__am$cache4;

		[CompilerGenerated]
		private static Action <>f__am$cache5;

		[CompilerGenerated]
		private static Action <>f__am$cache6;

		[CompilerGenerated]
		private static Action <>f__am$cache7;

		[CompilerGenerated]
		private static Action <>f__am$cache8;

		[CompilerGenerated]
		private static Action <>f__am$cache9;

		[CompilerGenerated]
		private static Action <>f__am$cacheA;

		[CompilerGenerated]
		private static Action <>f__am$cacheB;

		[CompilerGenerated]
		private static Action <>f__am$cacheC;

		[CompilerGenerated]
		private static Action <>f__am$cacheD;

		[CompilerGenerated]
		private static Action <>f__am$cacheE;

		[CompilerGenerated]
		private static Action <>f__am$cacheF;

		[CompilerGenerated]
		private static Action <>f__am$cache10;

		[CompilerGenerated]
		private static Action <>f__am$cache11;

		[CompilerGenerated]
		private static Action <>f__am$cache12;

		[CompilerGenerated]
		private static Action <>f__am$cache13;

		[CompilerGenerated]
		private static Action <>f__am$cache14;

		[CompilerGenerated]
		private static Action <>f__am$cache15;

		[CompilerGenerated]
		private static Action <>f__am$cache16;

		[CompilerGenerated]
		private static Action <>f__am$cache17;

		[CompilerGenerated]
		private static Action <>f__am$cache18;

		[CompilerGenerated]
		private static Action <>f__am$cache19;

		[CompilerGenerated]
		private static Action <>f__am$cache1A;

		[CompilerGenerated]
		private static Action <>f__am$cache1B;

		[CompilerGenerated]
		private static Action <>f__am$cache1C;

		[CompilerGenerated]
		private static Action <>f__am$cache1D;

		[CompilerGenerated]
		private static Action <>f__am$cache1E;

		[CompilerGenerated]
		private static Action <>f__am$cache1F;

		[CompilerGenerated]
		private static Action <>f__am$cache20;

		[CompilerGenerated]
		private static Action <>f__am$cache21;

		[CompilerGenerated]
		private static Action <>f__am$cache22;

		[CompilerGenerated]
		private static Action <>f__am$cache23;

		[CompilerGenerated]
		private static Action <>f__am$cache24;

		[CompilerGenerated]
		private static Action <>f__am$cache25;

		[CompilerGenerated]
		private static Action <>f__am$cache26;

		[CompilerGenerated]
		private static Action <>f__am$cache27;

		[CompilerGenerated]
		private static Action <>f__am$cache28;

		[CompilerGenerated]
		private static Action <>f__am$cache29;

		[CompilerGenerated]
		private static Action <>f__am$cache2A;

		[CompilerGenerated]
		private static Action <>f__am$cache2B;

		[CompilerGenerated]
		private static Action <>f__am$cache2C;

		[CompilerGenerated]
		private static Action <>f__am$cache2D;

		[CompilerGenerated]
		private static Action <>f__am$cache2E;

		[CompilerGenerated]
		private static Action <>f__am$cache2F;

		[CompilerGenerated]
		private static Action <>f__am$cache30;

		[CompilerGenerated]
		private static Action <>f__am$cache31;

		[CompilerGenerated]
		private static Action <>f__am$cache32;

		[CompilerGenerated]
		private static Action <>f__am$cache33;

		[CompilerGenerated]
		private static Action <>f__am$cache34;

		[CompilerGenerated]
		private static Action <>f__am$cache35;

		[CompilerGenerated]
		private static Action <>f__am$cache36;

		[CompilerGenerated]
		private static Action <>f__am$cache37;

		[CompilerGenerated]
		private static Action <>f__am$cache38;

		[CompilerGenerated]
		private static Action <>f__am$cache39;

		[CompilerGenerated]
		private static Action <>f__am$cache3A;

		[CompilerGenerated]
		private static Action <>f__am$cache3B;

		[CompilerGenerated]
		private static Action <>f__am$cache3C;

		[CompilerGenerated]
		private static Action <>f__am$cache3D;

		[CompilerGenerated]
		private static Action <>f__am$cache3E;

		[CompilerGenerated]
		private static Action <>f__am$cache3F;

		[CompilerGenerated]
		private static Action <>f__am$cache40;

		[CompilerGenerated]
		private static Action <>f__am$cache41;

		[CompilerGenerated]
		private static Action <>f__am$cache42;

		[CompilerGenerated]
		private static Action <>f__am$cache43;

		[CompilerGenerated]
		private static Action <>f__am$cache44;

		[CompilerGenerated]
		private static Action <>f__am$cache45;

		[CompilerGenerated]
		private static Action <>f__am$cache46;

		[CompilerGenerated]
		private static Action <>f__am$cache47;

		[CompilerGenerated]
		private static Action <>f__am$cache48;

		[CompilerGenerated]
		private static Action <>f__am$cache49;

		[CompilerGenerated]
		private static Action <>f__am$cache4A;

		[CompilerGenerated]
		private static Action <>f__am$cache4B;

		[CompilerGenerated]
		private static Action <>f__am$cache4C;

		[CompilerGenerated]
		private static Action <>f__am$cache4D;

		[CompilerGenerated]
		private static Action <>f__am$cache4E;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache4F;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache50;

		[CompilerGenerated]
		private static Action <>f__am$cache51;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache52;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache53;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache54;

		[CompilerGenerated]
		private static Action <>f__am$cache55;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache56;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache57;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache58;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache59;

		[CompilerGenerated]
		private static Action <>f__am$cache5A;

		[CompilerGenerated]
		private static Action <>f__am$cache5B;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache5C;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache5D;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache5E;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache5F;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache60;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache61;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache62;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache63;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache64;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache65;

		[CompilerGenerated]
		private static Action <>f__am$cache66;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache67;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache68;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache69;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache6A;

		[CompilerGenerated]
		private static Action <>f__am$cache6B;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache6C;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache6D;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache6E;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache6F;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache70;

		[CompilerGenerated]
		private static Action <>f__am$cache71;

		[CompilerGenerated]
		private static Action <>f__am$cache72;

		[CompilerGenerated]
		private static Action <>f__am$cache73;

		[CompilerGenerated]
		private static Action <>f__am$cache74;

		[CompilerGenerated]
		private static Action <>f__am$cache75;

		[CompilerGenerated]
		private static Action <>f__am$cache76;

		[CompilerGenerated]
		private static Action <>f__am$cache77;

		[CompilerGenerated]
		private static Action <>f__am$cache78;

		[CompilerGenerated]
		private static Action <>f__am$cache79;

		[CompilerGenerated]
		private static Action <>f__am$cache7A;

		[CompilerGenerated]
		private static Action <>f__am$cache7B;

		[CompilerGenerated]
		private static Action <>f__am$cache7C;

		[CompilerGenerated]
		private static Action <>f__am$cache7D;

		[CompilerGenerated]
		private static Action <>f__am$cache7E;

		[CompilerGenerated]
		private static Action <>f__am$cache7F;

		[CompilerGenerated]
		private static Action <>f__am$cache80;

		[CompilerGenerated]
		private static Action <>f__am$cache81;

		[CompilerGenerated]
		private static Action <>f__am$cache82;

		[CompilerGenerated]
		private static Action <>f__am$cache83;

		[CompilerGenerated]
		private static Action <>f__am$cache84;

		[CompilerGenerated]
		private static Action <>f__am$cache85;

		[CompilerGenerated]
		private static Action <>f__am$cache86;

		[CompilerGenerated]
		private static Action <>f__am$cache87;

		[CompilerGenerated]
		private static Action <>f__am$cache88;

		[CompilerGenerated]
		private static Action <>f__am$cache89;

		[CompilerGenerated]
		private static Action <>f__am$cache8A;

		[CompilerGenerated]
		private static Action <>f__am$cache8B;

		[CompilerGenerated]
		private static Action <>f__am$cache8C;

		[CompilerGenerated]
		private static Action <>f__am$cache8D;

		[CompilerGenerated]
		private static Action <>f__am$cache8E;

		[CompilerGenerated]
		private static Action <>f__am$cache8F;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache90;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache91;

		[CompilerGenerated]
		private static Predicate<int> <>f__am$cache92;

		[CompilerGenerated]
		private static Func<StorytellerComp, bool> <>f__am$cache93;

		[CompilerGenerated]
		private static Func<SoundDef, bool> <>f__am$cache94;

		[CompilerGenerated]
		private static Func<LordToil, bool> <>f__am$cache95;

		[CompilerGenerated]
		private static Action <>f__am$cache96;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache97;

		[CompilerGenerated]
		private static Func<RuleDef, string> <>f__am$cache98;

		[CompilerGenerated]
		private static Action<bool> <>f__am$cache99;

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache9A;

		[CompilerGenerated]
		private static Func<BodyPartRecord, bool> <>f__am$cache9B;

		[CompilerGenerated]
		private static Func<Hediff_Injury, bool> <>f__am$cache9C;

		[CompilerGenerated]
		private static Func<HediffGiverSetDef, IEnumerable<HediffGiver>> <>f__am$cache9D;

		[CompilerGenerated]
		private static Func<HediffStage, bool> <>f__am$cache9E;

		[CompilerGenerated]
		private static Func<VerbEntry, string> <>f__am$cache9F;

		[CompilerGenerated]
		private static Func<MentalBreakDef, MentalBreakIntensity> <>f__am$cacheA0;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cacheA1;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cacheA2;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheA3;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cacheA4;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheA5;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cacheA6;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheA7;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cacheA8;

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cacheA9;

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cacheAA;

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cacheAB;

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cacheAC;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cacheAD;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cacheAE;

		[CompilerGenerated]
		private static Func<ToolCapacityDef, bool> <>f__am$cacheAF;

		[CompilerGenerated]
		private static Func<PawnKindDef, ToolCapacityDef, string> <>f__am$cacheB0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cacheB1;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cacheB2;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cacheB3;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cacheB4;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cacheB5;

		public Dialog_DebugActionsMenu()
		{
			this.forcePause = true;
		}

		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static Dialog_DebugActionsMenu()
		{
		}

		[CompilerGenerated]
		private static void <DoListingItems_Entry>m__0()
		{
			LanguageDataWriter.WriteBackstoryFile();
		}

		[CompilerGenerated]
		private static void <DoListingItems_Entry>m__1()
		{
			LanguageReportGenerator.SaveTranslationReport();
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__2()
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
			mapParent.Tile = TileFinder.RandomStartingTile();
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__3()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__4()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__5()
		{
			Log.Message(string.Format("Leaked map {0}", Dialog_DebugActionsMenu.mapLeak), false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__6()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__7()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__8()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__9()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__A()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_AllModePlayActions>m__B()
		{
			if (Find.CurrentMap != null)
			{
				TimedForcedExit.ForceReform(Find.CurrentMap.Parent);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapActions>m__C()
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapActions>m__D()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__E()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				if (thing is Plant)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__F()
		{
			foreach (Thing thing in Find.CurrentMap.listerThings.AllThings.ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__10()
		{
			Find.ResearchManager.DebugSetAllProjectsFinished();
			Messages.Message("All research finished.", MessageTypeDefOf.TaskCompletion, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__11()
		{
			Find.CurrentMap.passingShipManager.DebugSendAllShipsAway();
			for (int i = 0; i < 5; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.CurrentMap;
				IncidentDefOf.OrbitalTraderArrival.Worker.TryExecute(incidentParms);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__12()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__13()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__14()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__15()
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapActions>m__16()
		{
			this.AddGuest(true);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapActions>m__17()
		{
			this.AddGuest(false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__18()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__19()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__1A()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__1B()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__1C()
		{
			LessonAutoActivator.DebugForceInitiateBestLessonNow();
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__1D()
		{
			Find.CurrentMap.mapDrawer.RegenerateEverythingNow();
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__1E()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__1F()
		{
			ShipCountdown.InitiateCountdown(null);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__20()
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(Find.CurrentMap);
			Find.CurrentMap.debugDrawer.FlashCell(intVec, 0f, null, 50);
			Log.Message("trade drop spot: " + intVec, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__21()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__22()
		{
			FloodFillerFog.DebugRefogMap(Find.CurrentMap);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__23()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__24()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 2500);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__25()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 15000);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__26()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 60000);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__27()
		{
			Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 900000);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__28()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__29()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Kill(null, null);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__2A()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__2B()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__2C()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 5000f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__2D()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Flame, 5000f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__2E()
		{
			CellRect r = CellRect.CenteredOn(UI.MouseCell(), 10);
			GenDebug.ClearArea(r, Find.CurrentMap);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__2F()
		{
			CellRect cellRect = CellRect.CenteredOn(UI.MouseCell(), 10);
			cellRect.ClipInsideMap(Find.CurrentMap);
			foreach (IntVec3 loc in cellRect)
			{
				GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__30()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__31()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Bomb, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__32()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Flame, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__33()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Stun, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__34()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.EMP, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__35()
		{
			IntVec3 center = UI.MouseCell();
			Map currentMap = Find.CurrentMap;
			float radius = 10f;
			DamageDef extinguish = DamageDefOf.Extinguish;
			Thing instigator = null;
			ThingDef filth_FireFoam = ThingDefOf.Filth_FireFoam;
			GenExplosion.DoExplosion(center, currentMap, radius, extinguish, instigator, -1, null, null, null, null, filth_FireFoam, 1f, 3, true, null, 0f, 1, 0f, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__36()
		{
			IntVec3 center = UI.MouseCell();
			Map currentMap = Find.CurrentMap;
			float radius = 10f;
			DamageDef smoke = DamageDefOf.Smoke;
			Thing instigator = null;
			ThingDef gas_Smoke = ThingDefOf.Gas_Smoke;
			GenExplosion.DoExplosion(center, currentMap, radius, smoke, instigator, -1, null, null, null, null, gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__37()
		{
			Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, UI.MouseCell()));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__38()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__39()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__3A()
		{
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.snowGrid.SetDepth(c, 0f);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__3B()
		{
			GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 10f);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__3C()
		{
			GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 10000f);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__3D()
		{
			GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, -1000f);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__3E()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				Plant plant = thing as Plant;
				if (plant != null)
				{
					plant.Growth = 1f;
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__3F()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__40()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__41()
		{
			Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__42()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompColorable compColorable = thing.TryGetComp<CompColorable>();
				if (compColorable != null)
				{
					thing.SetColor(GenColor.RandomColorOpaque(), true);
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__43()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRottable compRottable = thing.TryGetComp<CompRottable>();
				if (compRottable != null)
				{
					compRottable.RotProgress += 60000f;
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__44()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
				if (compRefuelable != null)
				{
					compRefuelable.ConsumeFuel(compRefuelable.Props.fuelCapacity * 0.2f);
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__45()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompBreakdownable compBreakdownable = thing.TryGetComp<CompBreakdownable>();
				if (compBreakdownable != null && !compBreakdownable.BrokenDown)
				{
					compBreakdownable.DoBreakdown();
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__46()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_MapGen.Options_Scatterers()));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__47()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__48()
		{
			CellRect.CellRectIterator iterator = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
			while (!iterator.Done())
			{
				Find.CurrentMap.roofGrid.SetRoof(iterator.Current, RoofDefOf.RoofConstructed);
				iterator.MoveNext();
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__49()
		{
			CellRect.CellRectIterator iterator = CellRect.CenteredOn(UI.MouseCell(), 1).GetIterator();
			while (!iterator.Done())
			{
				Find.CurrentMap.roofGrid.SetRoof(iterator.Current, null);
				iterator.MoveNext();
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__4A()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__4B()
		{
			foreach (Faction faction in Find.World.factionManager.AllFactions)
			{
				faction.TacticalMemory.TrapRevealed(UI.MouseCell(), Find.CurrentMap);
			}
			Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "added", 50);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__4C()
		{
			FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.CurrentMap);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__4D()
		{
			IntVec3 c = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.CurrentMap, 30, null);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__4E()
		{
			WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__4F()
		{
			Pawn pawn = Find.CurrentMap.mapPawns.FreeColonists.First<Pawn>();
			IntVec3 c;
			RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn, out c);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
			MoteMaker.ThrowText(c.ToVector3Shifted(), Find.CurrentMap, "for " + pawn.Label, Color.white, -1f);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__50()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__51()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__52()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__53(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__54(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__55()
		{
			foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					ResurrectionUtility.Resurrect(corpse.InnerPawn);
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__56(Pawn p)
		{
			HealthUtility.DamageUntilDowned(p);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__57(Pawn p)
		{
			HealthUtility.DamageLegsUntilIncapableOfMoving(p);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__58(Pawn p)
		{
			HealthUtility.DamageUntilDead(p);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__59()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					HealthUtility.DamageUntilDead((Pawn)pawn.carryTracker.CarriedThing);
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__5A(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord, false);
			HealthUtility.GiveInjuriesOperationFailureMinor(p, bodyPartRecord);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__5B(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord, false);
			HealthUtility.GiveInjuriesOperationFailureCatastrophic(p, bodyPartRecord);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__5C(Pawn p)
		{
			HealthUtility.GiveInjuriesOperationFailureRidiculous(p);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__5D(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__5E()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_ApplyDamage()));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__5F()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff()));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__60(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RemoveHediff(p)));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__61(Pawn p)
		{
			Hediff_Injury hediff_Injury;
			if ((from x in p.health.hediffSet.GetHediffs<Hediff_Injury>()
			where x.CanHealNaturally() || x.CanHealFromTending()
			select x).TryRandomElement(out hediff_Injury))
			{
				hediff_Injury.Heal(10f);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__62(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__63(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (!hediff.Visible)
				{
					hediff.Severity = Mathf.Max(hediff.Severity, hediff.def.stages.First((HediffStage s) => s.becomeVisible).minSeverity);
				}
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__64(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = 1f;
				}
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__65(Pawn p)
		{
			Hediff_Pregnant.DoBirthSpawn(p, null);
			this.DustPuffFrom(p);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__66(Pawn p)
		{
			Log.Message(string.Format("Verb list:\n  {0}", GenText.ToTextList(from verb in p.meleeVerbs.GetUpdatedAvailableVerbsList()
			select verb.ToString(), "\n  ")), false);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__67(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__68(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__69(Pawn p)
		{
			p.jobs.StartJob(new Job(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__6A()
		{
			this.OffsetNeed(NeedDefOf.Food, -0.2f);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__6B()
		{
			this.OffsetNeed(NeedDefOf.Rest, -0.2f);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__6C()
		{
			this.OffsetNeed(NeedDefOf.Joy, -0.2f);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__6D()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (typeof(Need_Chemical).IsAssignableFrom(allDefsListForReading[i].needClass))
				{
					this.OffsetNeed(allDefsListForReading[i], -0.2f);
				}
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__6E()
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__6F(Pawn p)
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__70()
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__71()
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__72()
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__73()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__74(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__75(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__76(Pawn p)
		{
			if (p.Faction != null)
			{
				p.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__77(Pawn p)
		{
			if (p.Faction != null)
			{
				p.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Neutral, true, null, null);
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__78(Pawn p)
		{
			if (p.Faction != null)
			{
				p.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Ally, true, null, null);
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__79()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__7A(Pawn p)
		{
			p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
			p.ageTracker.DebugForceBirthdayBiological();
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__7B(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, 1f, true);
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__7C(Pawn p)
		{
			if (p.apparel != null && p.apparel.WornApparelCount > 0)
			{
				p.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__7D(Pawn p)
		{
			if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, 1f, true);
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__7E(Pawn p)
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__7F(Pawn p)
		{
			if ((p.Name == null || p.Name.Numerical) && p.RaceProps.Animal)
			{
				PawnUtility.GiveNameBecauseOfNuzzle(p.Map.mapPawns.FreeColonists.First<Pawn>(), p);
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__80(Pawn p)
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__81(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				p.training.Debug_MakeDegradeHappenSoon();
				this.DustPuffFrom(p);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__82(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__83(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__84()
		{
			if (!Find.CurrentMap.lordsStarter.TryStartParty())
			{
				Messages.Message("Could not find any valid party spot or organizer.", MessageTypeDefOf.RejectInput, false);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__85(Pawn p)
		{
			if (p.IsPrisoner)
			{
				PrisonBreakUtility.StartPrisonBreak(p);
			}
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__86(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__87(Pawn p)
		{
			p.ageTracker.DebugMake1YearOlder();
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__88(Pawn p)
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__89(Pawn p)
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
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__8A(Pawn p)
		{
			p.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
			this.DustPuffFrom(p);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__8B(Pawn p)
		{
			p.jobs.CheckForJobOverride();
			this.DustPuffFrom(p);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__8C(Pawn p)
		{
			p.jobs.debugLog = !p.jobs.debugLog;
			this.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPos, p.Map, p.LabelShort + "\n" + ((!p.jobs.debugLog) ? "OFF" : "ON"), -1f);
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__8D(Pawn p)
		{
			p.stances.debugLog = !p.stances.debugLog;
			this.DustPuffFrom(p);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__8E()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__8F()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__90()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__91()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__92()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__93()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__94()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__95()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)));
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__96()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__97()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__98()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__99()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__9A()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__9B()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__9C()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__9D()
		{
			Autotests_ColonyMaker.MakeColony_Full();
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__9E()
		{
			Autotests_ColonyMaker.MakeColony_Animals();
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__9F()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A0()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A1()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A2()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A3()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A4()
		{
			Autotests_RegionListers.CheckBugs(Find.CurrentMap);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A5()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A6()
		{
			ArenaUtility.PerformBattleRoyale(DefDatabase<PawnKindDef>.AllDefs);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A7()
		{
			ArenaUtility.PerformBattleRoyale(from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			select k);
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__A8()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_World>m__A9()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_World>m__AA()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_World>m__AB()
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
		}

		[CompilerGenerated]
		private static void <DoListingItems_World>m__AC()
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
		}

		[CompilerGenerated]
		private static bool <OffsetNeed>m__AD(Thing t)
		{
			return t is Pawn;
		}

		[CompilerGenerated]
		private static bool <AddGuest>m__AE(PawnKindDef pk)
		{
			return pk.defaultFactionType != null && !pk.defaultFactionType.isPlayer && pk.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_AllModePlayActions>m__AF(int x)
		{
			return Find.World.HasCaves(x);
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapActions>m__B0(StorytellerComp x)
		{
			return x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapActions>m__B1(SoundDef s)
		{
			return !s.sustain;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapActions>m__B2(LordToil st)
		{
			return st is LordToil_PanicFlee;
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapActions>m__B3()
		{
			Find.WindowStack.Add(new Dialog_NamePlayerFaction());
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapActions>m__B4(Faction x)
		{
			return x.leader != null;
		}

		[CompilerGenerated]
		private static string <DoListingItems_MapTools>m__B5(RuleDef x)
		{
			return x.symbol;
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__B6(bool bestSideOnly)
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
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__B7(BodyPartRecord x)
		{
			return !x.def.conceptual;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__B8(BodyPartRecord x)
		{
			return !x.def.conceptual;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__B9(Hediff_Injury x)
		{
			return x.CanHealNaturally() || x.CanHealFromTending();
		}

		[CompilerGenerated]
		private static IEnumerable<HediffGiver> <DoListingItems_MapTools>m__BA(HediffGiverSetDef set)
		{
			return set.hediffGivers;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__BB(HediffStage s)
		{
			return s.becomeVisible;
		}

		[CompilerGenerated]
		private static string <DoListingItems_MapTools>m__BC(VerbEntry verb)
		{
			return verb.ToString();
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__BD()
		{
			foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Pawn
			select t).Cast<Pawn>())
			{
				pawn.mindState.mentalBreaker.LogPossibleMentalBreaks();
				this.DustPuffFrom(pawn);
			}
		}

		[CompilerGenerated]
		private void <DoListingItems_MapTools>m__BE()
		{
			foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Pawn
			select t).Cast<Pawn>())
			{
				pawn.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
				this.DustPuffFrom(pawn);
			}
		}

		[CompilerGenerated]
		private static MentalBreakIntensity <DoListingItems_MapTools>m__BF(MentalBreakDef x)
		{
			return x.intensity;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__C0(Thing t)
		{
			return t is Building_WorkTable;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__C1(Pawn x)
		{
			return x.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static string <DoListingItems_MapTools>m__C2(PawnKindDef kd)
		{
			return kd.defName;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__C3(ThingDef def)
		{
			return def.equipmentType == EquipmentType.Primary;
		}

		[CompilerGenerated]
		private static string <DoListingItems_MapTools>m__C4(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__C5(ThingDef def)
		{
			return def.IsApparel;
		}

		[CompilerGenerated]
		private static string <DoListingItems_MapTools>m__C6(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static void <DoListingItems_MapTools>m__C7(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (PawnKindDef kLocal2 in from x in DefDatabase<PawnKindDef>.AllDefs
			where x.race == p.def
			select x)
			{
				PawnKindDef kLocal = kLocal2;
				list.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate()
				{
					PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
					PawnGenerator.RedressPawn(p, request);
					GenSpawn.Spawn(p, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
					DebugTools.curTool = null;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__C8(IntVec3 c)
		{
			return c.Standable(Find.CurrentMap);
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__C9(IntVec3 c)
		{
			return c.Standable(Find.CurrentMap);
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__CA(IntVec3 c)
		{
			return c.Standable(Find.CurrentMap);
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__CB(IntVec3 c)
		{
			return c.Standable(Find.CurrentMap);
		}

		[CompilerGenerated]
		private static string <DoListingItems_MapTools>m__CC(PawnKindDef kd)
		{
			return kd.defName;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__CD(PawnKindDef k)
		{
			return k.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__CE(ToolCapacityDef tc)
		{
			return tc != ToolCapacityDefOf.KickMaterialInEyes;
		}

		[CompilerGenerated]
		private static string <DoListingItems_MapTools>m__CF(PawnKindDef pkd, ToolCapacityDef dd)
		{
			return string.Format("{0}_{1}", pkd.label, dd.defName);
		}

		[CompilerGenerated]
		private static bool <DoListingItems_World>m__D0(ThingDef def)
		{
			return def.IsWeapon && def.PlayerAcquirable;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_World>m__D1(PawnKindDef d)
		{
			return d.RaceProps.Animal && d.RaceProps.wildness < 1f;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_World>m__D2(Faction x)
		{
			return !x.IsPlayer && !x.def.hidden;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__D3(Thing t)
		{
			return t is Pawn;
		}

		[CompilerGenerated]
		private static bool <DoListingItems_MapTools>m__D4(Thing t)
		{
			return t is Pawn;
		}

		[CompilerGenerated]
		private sealed class <DoIncidentDebugAction>c__AnonStorey4A
		{
			internal IIncidentTarget target;

			private static Func<IncidentDef, string> <>f__am$cache0;

			public <DoIncidentDebugAction>c__AnonStorey4A()
			{
			}

			internal void <>m__0()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef localDef2 in from d in DefDatabase<IncidentDef>.AllDefs
				where d.TargetAllowed(this.target)
				orderby d.defName
				select d)
				{
					IncidentDef localDef = localDef2;
					string text = localDef.defName;
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, this.target);
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
			}

			internal bool <>m__1(IncidentDef d)
			{
				return d.TargetAllowed(this.target);
			}

			private static string <>m__2(IncidentDef d)
			{
				return d.defName;
			}

			private sealed class <DoIncidentDebugAction>c__AnonStorey4B
			{
				internal IncidentDef localDef;

				internal IncidentParms parms;

				internal Dialog_DebugActionsMenu.<DoIncidentDebugAction>c__AnonStorey4A <>f__ref$74;

				private static Func<StorytellerComp, bool> <>f__am$cache0;

				public <DoIncidentDebugAction>c__AnonStorey4B()
				{
				}

				internal void <>m__0()
				{
					if (this.localDef.pointsScaleable)
					{
						StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain);
						this.parms = storytellerComp.GenerateParms(this.localDef.category, this.parms.target);
					}
					this.localDef.Worker.TryExecute(this.parms);
				}

				private static bool <>m__1(StorytellerComp x)
				{
					return x is StorytellerComp_ThreatCycle || x is StorytellerComp_RandomMain;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoIncidentWithPointsAction>c__AnonStorey4C
		{
			internal IIncidentTarget target;

			private static Func<IncidentDef, string> <>f__am$cache0;

			public <DoIncidentWithPointsAction>c__AnonStorey4C()
			{
			}

			internal void <>m__0()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (IncidentDef localDef2 in from d in DefDatabase<IncidentDef>.AllDefs
				where d.TargetAllowed(this.target) && d.pointsScaleable
				orderby d.defName
				select d)
				{
					IncidentDef localDef = localDef2;
					string text = localDef.defName;
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, this.target);
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
			}

			internal bool <>m__1(IncidentDef d)
			{
				return d.TargetAllowed(this.target) && d.pointsScaleable;
			}

			private static string <>m__2(IncidentDef d)
			{
				return d.defName;
			}

			private sealed class <DoIncidentWithPointsAction>c__AnonStorey4E
			{
				internal IncidentParms parms;

				internal IncidentDef localDef;

				internal Dialog_DebugActionsMenu.<DoIncidentWithPointsAction>c__AnonStorey4C <>f__ref$76;

				public <DoIncidentWithPointsAction>c__AnonStorey4E()
				{
				}

				internal void <>m__0()
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (float num in Dialog_DebugActionsMenu.PointsOptions(true))
					{
						float localPoints = num;
						list.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
						{
							this.parms.points = localPoints;
							this.localDef.Worker.TryExecute(this.parms);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}

				private sealed class <DoIncidentWithPointsAction>c__AnonStorey4D
				{
					internal float localPoints;

					internal Dialog_DebugActionsMenu.<DoIncidentWithPointsAction>c__AnonStorey4C.<DoIncidentWithPointsAction>c__AnonStorey4E <>f__ref$78;

					public <DoIncidentWithPointsAction>c__AnonStorey4D()
					{
					}

					internal void <>m__0()
					{
						this.<>f__ref$78.parms.points = this.localPoints;
						this.<>f__ref$78.localDef.Worker.TryExecute(this.<>f__ref$78.parms);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PointsOptions>c__Iterator0 : IEnumerable, IEnumerable<float>, IEnumerator, IDisposable, IEnumerator<float>
		{
			internal bool extended;

			internal int <i>__1;

			internal int <i>__2;

			internal int <i>__3;

			internal int <i>__4;

			internal float $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PointsOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (!extended)
					{
						this.$current = 35f;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					i = 20;
					break;
				case 1u:
					this.$current = 70f;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = 100f;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = 150f;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = 200f;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = 350f;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = 500f;
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = 750f;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = 1000f;
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = 1500f;
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
					this.$current = 2000f;
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = 3000f;
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
					this.$current = 4000f;
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				case 13u:
					goto IL_351;
				case 14u:
					i += 10;
					break;
				case 15u:
					j += 25;
					goto IL_29A;
				case 16u:
					k += 50;
					goto IL_2ED;
				case 17u:
					l += 100;
					goto IL_340;
				default:
					return false;
				}
				if (i < 100)
				{
					this.$current = (float)i;
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				}
				j = 100;
				IL_29A:
				if (j < 500)
				{
					this.$current = (float)j;
					if (!this.$disposing)
					{
						this.$PC = 15;
					}
					return true;
				}
				k = 500;
				IL_2ED:
				if (k < 1500)
				{
					this.$current = (float)k;
					if (!this.$disposing)
					{
						this.$PC = 16;
					}
					return true;
				}
				l = 1500;
				IL_340:
				if (l <= 5000)
				{
					this.$current = (float)l;
					if (!this.$disposing)
					{
						this.$PC = 17;
					}
					return true;
				}
				IL_351:
				this.$PC = -1;
				return false;
			}

			float IEnumerator<float>.Current
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
				return this.System.Collections.Generic.IEnumerable<float>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<float> IEnumerable<float>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Dialog_DebugActionsMenu.<PointsOptions>c__Iterator0 <PointsOptions>c__Iterator = new Dialog_DebugActionsMenu.<PointsOptions>c__Iterator0();
				<PointsOptions>c__Iterator.extended = extended;
				return <PointsOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_AllModePlayActions>c__AnonStorey1
		{
			internal Map map;

			public <DoListingItems_AllModePlayActions>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Current.Game.DeinitAndRemoveMap(this.map);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_AllModePlayActions>c__AnonStorey2
		{
			internal Map map;

			public <DoListingItems_AllModePlayActions>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Dialog_DebugActionsMenu.mapLeak = this.map;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_AllModePlayActions>c__AnonStorey3
		{
			internal List<Thing> toTransfer;

			public <DoListingItems_AllModePlayActions>c__AnonStorey3()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_AllModePlayActions>c__AnonStorey4
		{
			internal Map map;

			internal Dialog_DebugActionsMenu.<DoListingItems_AllModePlayActions>c__AnonStorey3 <>f__ref$3;

			public <DoListingItems_AllModePlayActions>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				for (int i = 0; i < this.<>f__ref$3.toTransfer.Count; i++)
				{
					IntVec3 center;
					if (CellFinder.TryFindRandomCellNear(this.map.Center, this.map, Mathf.Max(this.map.Size.x, this.map.Size.z), (IntVec3 x) => !x.Fogged(this.map) && x.Standable(this.map), out center, -1))
					{
						this.<>f__ref$3.toTransfer[i].DeSpawn(DestroyMode.Vanish);
						GenPlace.TryPlaceThing(this.<>f__ref$3.toTransfer[i], center, this.map, ThingPlaceMode.Near, null, null);
					}
					else
					{
						Log.Error("Could not find spawn cell.", false);
					}
				}
			}

			internal bool <>m__1(IntVec3 x)
			{
				return !x.Fogged(this.map) && x.Standable(this.map);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_AllModePlayActions>c__AnonStorey5
		{
			internal Map map;

			public <DoListingItems_AllModePlayActions>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				Current.Game.CurrentMap = this.map;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_AllModePlayActions>c__AnonStorey6
		{
			internal MapGeneratorDef mapgen;

			private static Func<int, bool> <>f__am$cache0;

			public <DoListingItems_AllModePlayActions>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				mapParent.Tile = (from tile in Enumerable.Range(0, Find.WorldGrid.TilesCount)
				where Find.WorldGrid[tile].biome.canBuildBase
				select tile).RandomElement<int>();
				mapParent.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(mapParent);
				Map currentMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, this.mapgen, null, null);
				Current.Game.CurrentMap = currentMap;
			}

			private static bool <>m__1(int tile)
			{
				return Find.WorldGrid[tile].biome.canBuildBase;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStorey7
		{
			internal float localP;

			public <DoListingItems_MapActions>c__AnonStorey7()
			{
			}

			internal void <>m__0()
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.CurrentMap;
				incidentParms.points = this.localP;
				IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStorey9
		{
			internal IncidentParms parms;

			internal Dialog_DebugActionsMenu $this;

			public <DoListingItems_MapActions>c__AnonStorey9()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStorey8
		{
			internal Faction localFac;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9;

			public <DoListingItems_MapActions>c__AnonStorey8()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$9.parms.faction = this.localFac;
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (float num in Dialog_DebugActionsMenu.PointsOptions(true))
				{
					Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9 = this.<>f__ref$9;
					Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8 <>f__ref$8 = this;
					float localPoints = num;
					list.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
					{
						<>f__ref$9.parms.points = localPoints;
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (RaidStrategyDef localStrat in DefDatabase<RaidStrategyDef>.AllDefs)
						{
							Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB <DoListingItems_MapActions>c__AnonStoreyB = new Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB();
							<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9 = <>f__ref$9;
							<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$8 = <>f__ref$8;
							<DoListingItems_MapActions>c__AnonStoreyB.localStrat = localStrat;
							string text = <DoListingItems_MapActions>c__AnonStoreyB.localStrat.defName;
							if (!<DoListingItems_MapActions>c__AnonStoreyB.localStrat.Worker.CanUseWith(<>f__ref$9.parms, PawnGroupKindDefOf.Combat))
							{
								text += " [NO]";
							}
							list2.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
							{
								<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.parms.raidStrategy = <DoListingItems_MapActions>c__AnonStoreyB.localStrat;
								List<DebugMenuOption> list3 = new List<DebugMenuOption>();
								list3.Add(new DebugMenuOption("-Random-", DebugMenuOptionMode.Action, delegate()
								{
									<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.$this.DoRaid(<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.parms);
								}));
								foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
								{
									Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9 = <DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9;
									Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8 <>f__ref$8 = <DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$8;
									Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB <>f__ref$11 = <DoListingItems_MapActions>c__AnonStoreyB;
									PawnsArrivalModeDef localArrival = localArrival2;
									string text2 = localArrival.defName;
									if (!localArrival.Worker.CanUseWith(<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.parms) || !<DoListingItems_MapActions>c__AnonStoreyB.localStrat.arriveModes.Contains(localArrival))
									{
										text2 += " [NO]";
									}
									list3.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Action, delegate()
									{
										<>f__ref$9.parms.raidArrivalMode = localArrival;
										<>f__ref$9.$this.DoRaid(<>f__ref$9.parms);
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			}

			private sealed class <DoListingItems_MapActions>c__AnonStoreyA
			{
				internal float localPoints;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8 <>f__ref$8;

				public <DoListingItems_MapActions>c__AnonStoreyA()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$9.parms.points = this.localPoints;
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (RaidStrategyDef localStrat in DefDatabase<RaidStrategyDef>.AllDefs)
					{
						Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB <DoListingItems_MapActions>c__AnonStoreyB = new Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB();
						<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9 = this.<>f__ref$9;
						<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$8 = this.<>f__ref$8;
						<DoListingItems_MapActions>c__AnonStoreyB.localStrat = localStrat;
						string text = <DoListingItems_MapActions>c__AnonStoreyB.localStrat.defName;
						if (!<DoListingItems_MapActions>c__AnonStoreyB.localStrat.Worker.CanUseWith(this.<>f__ref$9.parms, PawnGroupKindDefOf.Combat))
						{
							text += " [NO]";
						}
						list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
						{
							<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.parms.raidStrategy = <DoListingItems_MapActions>c__AnonStoreyB.localStrat;
							List<DebugMenuOption> list2 = new List<DebugMenuOption>();
							list2.Add(new DebugMenuOption("-Random-", DebugMenuOptionMode.Action, delegate()
							{
								<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.$this.DoRaid(<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.parms);
							}));
							foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
							{
								Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9 = <DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9;
								Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8 <>f__ref$8 = <DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$8;
								Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB <>f__ref$11 = <DoListingItems_MapActions>c__AnonStoreyB;
								PawnsArrivalModeDef localArrival = localArrival2;
								string text2 = localArrival.defName;
								if (!localArrival.Worker.CanUseWith(<DoListingItems_MapActions>c__AnonStoreyB.<>f__ref$9.parms) || !<DoListingItems_MapActions>c__AnonStoreyB.localStrat.arriveModes.Contains(localArrival))
								{
									text2 += " [NO]";
								}
								list2.Add(new DebugMenuOption(text2, DebugMenuOptionMode.Action, delegate()
								{
									<>f__ref$9.parms.raidArrivalMode = localArrival;
									<>f__ref$9.$this.DoRaid(<>f__ref$9.parms);
								}));
							}
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}

				private sealed class <DoListingItems_MapActions>c__AnonStoreyB
				{
					internal RaidStrategyDef localStrat;

					internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9;

					internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8 <>f__ref$8;

					public <DoListingItems_MapActions>c__AnonStoreyB()
					{
					}

					internal void <>m__0()
					{
						this.<>f__ref$9.parms.raidStrategy = this.localStrat;
						List<DebugMenuOption> list = new List<DebugMenuOption>();
						list.Add(new DebugMenuOption("-Random-", DebugMenuOptionMode.Action, delegate()
						{
							this.<>f__ref$9.$this.DoRaid(this.<>f__ref$9.parms);
						}));
						foreach (PawnsArrivalModeDef localArrival2 in DefDatabase<PawnsArrivalModeDef>.AllDefs)
						{
							Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9 = this.<>f__ref$9;
							Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8 <>f__ref$8 = this.<>f__ref$8;
							Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB <>f__ref$11 = this;
							PawnsArrivalModeDef localArrival = localArrival2;
							string text = localArrival.defName;
							if (!localArrival.Worker.CanUseWith(this.<>f__ref$9.parms) || !this.localStrat.arriveModes.Contains(localArrival))
							{
								text += " [NO]";
							}
							list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
							{
								<>f__ref$9.parms.raidArrivalMode = localArrival;
								<>f__ref$9.$this.DoRaid(<>f__ref$9.parms);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
					}

					internal void <>m__1()
					{
						this.<>f__ref$9.$this.DoRaid(this.<>f__ref$9.parms);
					}

					private sealed class <DoListingItems_MapActions>c__AnonStoreyC
					{
						internal PawnsArrivalModeDef localArrival;

						internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey9 <>f__ref$9;

						internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8 <>f__ref$8;

						internal Dialog_DebugActionsMenu.<DoListingItems_MapActions>c__AnonStorey8.<DoListingItems_MapActions>c__AnonStoreyA.<DoListingItems_MapActions>c__AnonStoreyB <>f__ref$11;

						public <DoListingItems_MapActions>c__AnonStoreyC()
						{
						}

						internal void <>m__0()
						{
							this.<>f__ref$9.parms.raidArrivalMode = this.localArrival;
							this.<>f__ref$9.$this.DoRaid(this.<>f__ref$9.parms);
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStoreyD
		{
			internal WeatherDef localWeather;

			public <DoListingItems_MapActions>c__AnonStoreyD()
			{
			}

			internal void <>m__0()
			{
				Find.CurrentMap.weatherManager.TransitionTo(this.localWeather);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStoreyE
		{
			internal SongDef localSong;

			public <DoListingItems_MapActions>c__AnonStoreyE()
			{
			}

			internal void <>m__0()
			{
				Find.MusicManagerPlay.ForceStartSong(this.localSong, false);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStoreyF
		{
			internal SoundDef localSd;

			public <DoListingItems_MapActions>c__AnonStoreyF()
			{
			}

			internal void <>m__0()
			{
				this.localSd.PlayOneShotOnCamera(null);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStorey10
		{
			internal GameCondition localMc;

			public <DoListingItems_MapActions>c__AnonStorey10()
			{
			}

			internal void <>m__0()
			{
				this.localMc.End();
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStorey11
		{
			internal FactionBase factionBase;

			public <DoListingItems_MapActions>c__AnonStorey11()
			{
			}

			internal void <>m__0()
			{
				Find.WindowStack.Add(new Dialog_NamePlayerFactionBase(this.factionBase));
			}

			internal void <>m__1()
			{
				Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(this.factionBase));
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStorey12
		{
			internal Type localType;

			public <DoListingItems_MapActions>c__AnonStorey12()
			{
			}

			internal void <>m__0()
			{
				Find.CameraDriver.config = (CameraMapConfig)Activator.CreateInstance(this.localType);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapActions>c__AnonStorey13
		{
			internal Type localGenStep;

			public <DoListingItems_MapActions>c__AnonStorey13()
			{
			}

			internal void <>m__0()
			{
				GenStep genStep = (GenStep)Activator.CreateInstance(this.localGenStep);
				genStep.Generate(Find.CurrentMap);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey15
		{
			internal string localSymbol;

			public <DoListingItems_MapTools>c__AnonStorey15()
			{
			}

			internal void <>m__0()
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
						BaseGen.symbolStack.Push(this.localSymbol, rect);
						BaseGen.Generate();
						DebugTools.curTool = tool;
					}, firstCorner);
				}, null);
				DebugTools.curTool = tool;
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey14
			{
				internal IntVec3 firstCorner;

				internal DebugTool tool;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey15 <>f__ref$21;

				public <DoListingItems_MapTools>c__AnonStorey14()
				{
				}

				internal void <>m__0()
				{
					this.firstCorner = UI.MouseCell();
					DebugTools.curTool = new DebugTool("second corner...", delegate()
					{
						IntVec3 second = UI.MouseCell();
						CellRect rect = CellRect.FromLimits(this.firstCorner, second).ClipInsideMap(Find.CurrentMap);
						BaseGen.globalSettings.map = Find.CurrentMap;
						BaseGen.symbolStack.Push(this.<>f__ref$21.localSymbol, rect);
						BaseGen.Generate();
						DebugTools.curTool = this.tool;
					}, this.firstCorner);
				}

				internal void <>m__1()
				{
					IntVec3 second = UI.MouseCell();
					CellRect rect = CellRect.FromLimits(this.firstCorner, second).ClipInsideMap(Find.CurrentMap);
					BaseGen.globalSettings.map = Find.CurrentMap;
					BaseGen.symbolStack.Push(this.<>f__ref$21.localSymbol, rect);
					BaseGen.Generate();
					DebugTools.curTool = this.tool;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey17
		{
			internal Action<bool> act;

			public <DoListingItems_MapTools>c__AnonStorey17()
			{
			}

			internal void <>m__0()
			{
				this.act(false);
			}

			internal void <>m__1()
			{
				this.act(true);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey19
		{
			internal TraverseMode traverseMode;

			public <DoListingItems_MapTools>c__AnonStorey19()
			{
			}

			internal void <>m__0()
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
							TraverseMode mode = this.traverseMode;
							flag = fromPawn.CanReach(dest, peMode, maxDanger, false, mode);
							intVec = fromPawn.Position;
						}
						else
						{
							flag = Find.CurrentMap.reachability.CanReach(from, c, PathEndMode.OnCell, this.traverseMode, Danger.Deadly);
							intVec = from;
						}
						Color color = (!flag) ? Color.red : Color.green;
						Widgets.DrawLine(intVec.ToUIPosition(), c.ToUIPosition(), color, 2f);
					});
				}, null);
				DebugTools.curTool = tool;
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey18
			{
				internal IntVec3 from;

				internal Pawn fromPawn;

				internal DebugTool tool;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey19 <>f__ref$25;

				public <DoListingItems_MapTools>c__AnonStorey18()
				{
				}

				internal void <>m__0()
				{
					this.from = UI.MouseCell();
					this.fromPawn = this.from.GetFirstPawn(Find.CurrentMap);
					string text = "to...";
					if (this.fromPawn != null)
					{
						text = text + " (pawn=" + this.fromPawn.LabelShort + ")";
					}
					DebugTools.curTool = new DebugTool(text, delegate()
					{
						DebugTools.curTool = this.tool;
					}, delegate()
					{
						IntVec3 c = UI.MouseCell();
						bool flag;
						IntVec3 position;
						if (this.fromPawn != null)
						{
							Pawn pawn = this.fromPawn;
							LocalTargetInfo dest = c;
							PathEndMode peMode = PathEndMode.OnCell;
							Danger maxDanger = Danger.Deadly;
							TraverseMode traverseMode = this.<>f__ref$25.traverseMode;
							flag = pawn.CanReach(dest, peMode, maxDanger, false, traverseMode);
							position = this.fromPawn.Position;
						}
						else
						{
							flag = Find.CurrentMap.reachability.CanReach(this.from, c, PathEndMode.OnCell, this.<>f__ref$25.traverseMode, Danger.Deadly);
							position = this.from;
						}
						Color color = (!flag) ? Color.red : Color.green;
						Widgets.DrawLine(position.ToUIPosition(), c.ToUIPosition(), color, 2f);
					});
				}

				internal void <>m__1()
				{
					DebugTools.curTool = this.tool;
				}

				internal void <>m__2()
				{
					IntVec3 c = UI.MouseCell();
					bool flag;
					IntVec3 position;
					if (this.fromPawn != null)
					{
						Pawn pawn = this.fromPawn;
						LocalTargetInfo dest = c;
						PathEndMode peMode = PathEndMode.OnCell;
						Danger maxDanger = Danger.Deadly;
						TraverseMode traverseMode = this.<>f__ref$25.traverseMode;
						flag = pawn.CanReach(dest, peMode, maxDanger, false, traverseMode);
						position = this.fromPawn.Position;
					}
					else
					{
						flag = Find.CurrentMap.reachability.CanReach(this.from, c, PathEndMode.OnCell, this.<>f__ref$25.traverseMode, Danger.Deadly);
						position = this.from;
					}
					Color color = (!flag) ? Color.red : Color.green;
					Widgets.DrawLine(position.ToUIPosition(), c.ToUIPosition(), color, 2f);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey1B
		{
			internal Pawn p;

			internal Dialog_DebugActionsMenu $this;

			public <DoListingItems_MapTools>c__AnonStorey1B()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey1A
		{
			internal HediffGiver localHdg;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey1B <>f__ref$27;

			public <DoListingItems_MapTools>c__AnonStorey1A()
			{
			}

			internal void <>m__0()
			{
				this.localHdg.TryApply(this.<>f__ref$27.p, null);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey1C
		{
			internal Pawn p;

			internal Action<bool> act;

			public <DoListingItems_MapTools>c__AnonStorey1C()
			{
			}

			internal void <>m__0(bool add)
			{
				if (add)
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (PawnRelationDef pawnRelationDef in DefDatabase<PawnRelationDef>.AllDefs)
					{
						if (!pawnRelationDef.implied)
						{
							PawnRelationDef defLocal = pawnRelationDef;
							list.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list3 = new List<DebugMenuOption>();
								IOrderedEnumerable<Pawn> orderedEnumerable = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
								where x.RaceProps.IsFlesh
								orderby x.def == this.p.def descending, x.IsWorldPawn()
								select x;
								foreach (Pawn pawn in orderedEnumerable)
								{
									if (this.p != pawn)
									{
										if (!defLocal.familyByBloodRelation || pawn.def == this.p.def)
										{
											if (!this.p.relations.DirectRelationExists(defLocal, pawn))
											{
												Pawn otherLocal = pawn;
												list3.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
												{
													this.p.relations.AddDirectRelation(defLocal, otherLocal);
												}));
											}
										}
									}
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
				else
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DirectPawnRelation> directRelations = this.p.relations.DirectRelations;
					for (int i = 0; i < directRelations.Count; i++)
					{
						DirectPawnRelation rel = directRelations[i];
						list2.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate()
						{
							this.p.relations.RemoveDirectRelation(rel);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}
			}

			internal void <>m__1()
			{
				this.act(true);
			}

			internal void <>m__2()
			{
				this.act(false);
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey1D
			{
				internal PawnRelationDef defLocal;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey1C <>f__ref$28;

				private static Func<Pawn, bool> <>f__am$cache0;

				private static Func<Pawn, bool> <>f__am$cache1;

				public <DoListingItems_MapTools>c__AnonStorey1D()
				{
				}

				internal void <>m__0()
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					IOrderedEnumerable<Pawn> orderedEnumerable = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
					where x.RaceProps.IsFlesh
					orderby x.def == this.<>f__ref$28.p.def descending, x.IsWorldPawn()
					select x;
					foreach (Pawn pawn in orderedEnumerable)
					{
						Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey1C <>f__ref$28 = this.<>f__ref$28;
						Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey1C.<DoListingItems_MapTools>c__AnonStorey1D <>f__ref$29 = this;
						if (this.<>f__ref$28.p != pawn)
						{
							if (!this.defLocal.familyByBloodRelation || pawn.def == this.<>f__ref$28.p.def)
							{
								if (!this.<>f__ref$28.p.relations.DirectRelationExists(this.defLocal, pawn))
								{
									Pawn otherLocal = pawn;
									list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate()
									{
										<>f__ref$28.p.relations.AddDirectRelation(<>f__ref$29.defLocal, otherLocal);
									}));
								}
							}
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}

				private static bool <>m__1(Pawn x)
				{
					return x.RaceProps.IsFlesh;
				}

				internal bool <>m__2(Pawn x)
				{
					return x.def == this.<>f__ref$28.p.def;
				}

				private static bool <>m__3(Pawn x)
				{
					return x.IsWorldPawn();
				}

				private sealed class <DoListingItems_MapTools>c__AnonStorey1E
				{
					internal Pawn otherLocal;

					internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey1C <>f__ref$28;

					internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey1C.<DoListingItems_MapTools>c__AnonStorey1D <>f__ref$29;

					public <DoListingItems_MapTools>c__AnonStorey1E()
					{
					}

					internal void <>m__0()
					{
						this.<>f__ref$28.p.relations.AddDirectRelation(this.<>f__ref$29.defLocal, this.otherLocal);
					}
				}
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey1F
			{
				internal DirectPawnRelation rel;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey1C <>f__ref$28;

				public <DoListingItems_MapTools>c__AnonStorey1F()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$28.p.relations.RemoveDirectRelation(this.rel);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey20
		{
			internal Pawn p;

			internal Action<bool> act;

			private static Func<Pawn, bool> <>f__am$cache0;

			public <DoListingItems_MapTools>c__AnonStorey20()
			{
			}

			internal void <>m__0(bool good)
			{
				foreach (Pawn pawn in from x in this.p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike
				select x)
				{
					if (this.p != pawn)
					{
						IEnumerable<ThoughtDef> source = DefDatabase<ThoughtDef>.AllDefs.Where((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0f) || (!good && x.stages[0].baseOpinionOffset < 0f)));
						if (source.Any<ThoughtDef>())
						{
							int num = Rand.Range(2, 5);
							for (int i = 0; i < num; i++)
							{
								ThoughtDef def = source.RandomElement<ThoughtDef>();
								pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.p);
							}
						}
					}
				}
			}

			internal void <>m__1()
			{
				this.act(true);
			}

			internal void <>m__2()
			{
				this.act(false);
			}

			private static bool <>m__3(Pawn x)
			{
				return x.RaceProps.Humanlike;
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey21
			{
				internal bool good;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey20 <>f__ref$32;

				public <DoListingItems_MapTools>c__AnonStorey21()
				{
				}

				internal bool <>m__0(ThoughtDef x)
				{
					return typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((this.good && x.stages[0].baseOpinionOffset > 0f) || (!this.good && x.stages[0].baseOpinionOffset < 0f));
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey22
		{
			internal SkillDef localDef;

			internal Dialog_DebugActionsMenu $this;

			public <DoListingItems_MapTools>c__AnonStorey22()
			{
			}

			internal void <>m__0()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				for (int i = 0; i <= 20; i++)
				{
					int level = i;
					list.Add(new DebugMenuOption(level.ToString(), DebugMenuOptionMode.Tool, delegate()
					{
						Pawn pawn = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>().FirstOrDefault<Pawn>();
						if (pawn != null)
						{
							SkillRecord skill = pawn.skills.GetSkill(this.localDef);
							skill.Level = level;
							skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
							this.DustPuffFrom(pawn);
						}
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey23
			{
				internal int level;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey22 <>f__ref$34;

				private static Func<Thing, bool> <>f__am$cache0;

				public <DoListingItems_MapTools>c__AnonStorey23()
				{
				}

				internal void <>m__0()
				{
					Pawn pawn = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().FirstOrDefault<Pawn>();
					if (pawn != null)
					{
						SkillRecord skill = pawn.skills.GetSkill(this.<>f__ref$34.localDef);
						skill.Level = this.level;
						skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
						this.<>f__ref$34.$this.DustPuffFrom(pawn);
					}
				}

				private static bool <>m__1(Thing t)
				{
					return t is Pawn;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey24
		{
			internal MentalBreakDef locBrDef;

			internal Dialog_DebugActionsMenu $this;

			private static Func<Thing, bool> <>f__am$cache0;

			public <DoListingItems_MapTools>c__AnonStorey24()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return this.locBrDef.Worker.BreakCanOccur(x);
			}

			internal void <>m__1()
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					this.locBrDef.Worker.TryStart(pawn, null, false);
					this.$this.DustPuffFrom(pawn);
				}
			}

			private static bool <>m__2(Thing t)
			{
				return t is Pawn;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey25
		{
			internal MentalStateDef locBrDef;

			internal Dialog_DebugActionsMenu $this;

			private static Func<Thing, bool> <>f__am$cache0;

			public <DoListingItems_MapTools>c__AnonStorey25()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return this.locBrDef.Worker.StateCanOccur(x);
			}

			internal void <>m__1()
			{
				foreach (Pawn locP2 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					Pawn locP = locP2;
					if (this.locBrDef != MentalStateDefOf.SocialFighting)
					{
						locP.mindState.mentalStateHandler.TryStartMentalState(this.locBrDef, null, true, false, null, false);
						this.$this.DustPuffFrom(locP);
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
			}

			private static bool <>m__2(Thing t)
			{
				return t is Pawn;
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey26
			{
				internal Pawn locP;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey25 <>f__ref$37;

				private static Func<Thing, bool> <>f__am$cache0;

				public <DoListingItems_MapTools>c__AnonStorey26()
				{
				}

				internal void <>m__0()
				{
					Pawn pawn = (Pawn)(from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).FirstOrDefault<Thing>();
					if (pawn != null)
					{
						if (!InteractionUtility.HasAnySocialFightProvokingThought(this.locP, pawn))
						{
							this.locP.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Insulted, pawn);
							Messages.Message("Dev: Auto added negative thought.", this.locP, MessageTypeDefOf.TaskCompletion, false);
						}
						this.locP.interactions.StartSocialFight(pawn);
						DebugTools.curTool = null;
					}
				}

				private static bool <>m__1(Thing t)
				{
					return t is Pawn;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey27
		{
			internal InspirationDef localDef;

			internal Dialog_DebugActionsMenu $this;

			public <DoListingItems_MapTools>c__AnonStorey27()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return this.localDef.Worker.InspirationCanOccur(x);
			}

			internal void <>m__1()
			{
				foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
				{
					pawn.mindState.inspirationHandler.TryStartInspiration(this.localDef);
					this.$this.DustPuffFrom(pawn);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey28
		{
			internal TraitDef trDef;

			internal Dialog_DebugActionsMenu $this;

			public <DoListingItems_MapTools>c__AnonStorey28()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey29
		{
			internal int i;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey28 <>f__ref$40;

			private static Func<Thing, bool> <>f__am$cache0;

			public <DoListingItems_MapTools>c__AnonStorey29()
			{
			}

			internal void <>m__0()
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					if (pawn.story != null)
					{
						Trait item = new Trait(this.<>f__ref$40.trDef, this.<>f__ref$40.trDef.degreeDatas[this.i].degree, false);
						pawn.story.traits.allTraits.Add(item);
						this.<>f__ref$40.$this.DustPuffFrom(pawn);
					}
				}
			}

			private static bool <>m__1(Thing t)
			{
				return t is Pawn;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey2A
		{
			internal Pawn p;

			public <DoListingItems_MapTools>c__AnonStorey2A()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return x.RaceProps.Animal && x.Faction == this.p.Faction;
			}

			internal bool <>m__1(Pawn x)
			{
				return x.RaceProps.Humanlike && x.Faction == this.p.Faction;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey2B
		{
			internal Pawn p;

			public <DoListingItems_MapTools>c__AnonStorey2B()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey2C
		{
			internal Pawn otherLocal;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey2B <>f__ref$43;

			public <DoListingItems_MapTools>c__AnonStorey2C()
			{
			}

			internal void <>m__0()
			{
				if (!this.<>f__ref$43.p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.otherLocal))
				{
					this.<>f__ref$43.p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, this.otherLocal);
					this.<>f__ref$43.p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, this.otherLocal);
					this.<>f__ref$43.p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, this.otherLocal);
					Messages.Message("Dev: Auto added fiance relation.", this.<>f__ref$43.p, MessageTypeDefOf.TaskCompletion, false);
				}
				if (!this.<>f__ref$43.p.Map.lordsStarter.TryStartMarriageCeremony(this.<>f__ref$43.p, this.otherLocal))
				{
					Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput, false);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey2D
		{
			internal Pawn p;

			public <DoListingItems_MapTools>c__AnonStorey2D()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey2E
		{
			internal Pawn otherLocal;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey2D <>f__ref$45;

			public <DoListingItems_MapTools>c__AnonStorey2E()
			{
			}

			internal void <>m__0()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				foreach (InteractionDef interactionLocal2 in DefDatabase<InteractionDef>.AllDefsListForReading)
				{
					Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey2D <>f__ref$45 = this.<>f__ref$45;
					Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey2E <>f__ref$46 = this;
					InteractionDef interactionLocal = interactionLocal2;
					list.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate()
					{
						<>f__ref$45.p.interactions.TryInteractWith(<>f__ref$46.otherLocal, interactionLocal);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey2F
			{
				internal InteractionDef interactionLocal;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey2D <>f__ref$45;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey2E <>f__ref$46;

				public <DoListingItems_MapTools>c__AnonStorey2F()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$45.p.interactions.TryInteractWith(this.<>f__ref$46.otherLocal, this.interactionLocal);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey31
		{
			internal Pawn p;

			public <DoListingItems_MapTools>c__AnonStorey31()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey30
		{
			internal Type localType;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey31 <>f__ref$49;

			public <DoListingItems_MapTools>c__AnonStorey30()
			{
			}

			internal void <>m__0()
			{
				ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(this.localType);
				thinkNode_JobGiver.ResolveReferences();
				ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(this.<>f__ref$49.p, default(JobIssueParams));
				if (thinkResult.Job != null)
				{
					this.<>f__ref$49.p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, null, false);
				}
				else
				{
					Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey33
		{
			internal Pawn p;

			public <DoListingItems_MapTools>c__AnonStorey33()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey32
		{
			internal JoyGiverDef def;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey33 <>f__ref$51;

			public <DoListingItems_MapTools>c__AnonStorey32()
			{
			}

			internal void <>m__0()
			{
				Job job = this.def.Worker.TryGiveJob(this.<>f__ref$51.p);
				if (job != null)
				{
					this.<>f__ref$51.p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false);
				}
				else
				{
					Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey34
		{
			internal PawnKindDef localKindDef;

			public <DoListingItems_MapTools>c__AnonStorey34()
			{
			}

			internal void <>m__0()
			{
				Faction faction = FactionUtility.DefaultFactionFrom(this.localKindDef.defaultFactionType);
				Pawn newPawn = PawnGenerator.GeneratePawn(this.localKindDef, faction);
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
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey35
			{
				internal Pawn newPawn;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey34 <>f__ref$52;

				public <DoListingItems_MapTools>c__AnonStorey35()
				{
				}

				internal bool <>m__0(Pawn p)
				{
					return p != this.newPawn;
				}

				internal bool <>m__1(Thing p)
				{
					return p != this.newPawn && ((Pawn)p).GetLord() != null;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey36
		{
			internal ThingDef localDef;

			public <DoListingItems_MapTools>c__AnonStorey36()
			{
			}

			internal void <>m__0()
			{
				DebugThingPlaceHelper.DebugSpawn(this.localDef, UI.MouseCell(), -1, false);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey37
		{
			internal ThingDef localDef;

			public <DoListingItems_MapTools>c__AnonStorey37()
			{
			}

			internal void <>m__0()
			{
				DebugThingPlaceHelper.DebugSpawn(this.localDef, UI.MouseCell(), -1, false);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey38
		{
			internal WipeMode localWipeMode;

			public <DoListingItems_MapTools>c__AnonStorey38()
			{
			}

			internal void <>m__0()
			{
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.SpawnOptions(this.localWipeMode)));
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey39
		{
			internal TerrainDef localDef;

			public <DoListingItems_MapTools>c__AnonStorey39()
			{
			}

			internal void <>m__0()
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					Find.CurrentMap.terrainGrid.SetTerrain(UI.MouseCell(), this.localDef);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey3A
		{
			internal Faction localFac;

			public <DoListingItems_MapTools>c__AnonStorey3A()
			{
			}

			internal void <>m__0()
			{
				GenSpawn.Spawn(this.localFac.leader, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey3D
		{
			internal Action<Pawn> act;

			public <DoListingItems_MapTools>c__AnonStorey3D()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey3E
		{
			internal Pawn pLocal;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey3D <>f__ref$61;

			public <DoListingItems_MapTools>c__AnonStorey3E()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$61.act(this.pLocal);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey3F
		{
			internal ThingSetMakerDef localGenerator;

			public <DoListingItems_MapTools>c__AnonStorey3F()
			{
			}

			internal void <>m__0()
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					StringBuilder stringBuilder = new StringBuilder();
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(this.localGenerator.debugParams);
					List<Thing> list = this.localGenerator.root.Generate(this.localGenerator.debugParams);
					stringBuilder.Append(string.Concat(new object[]
					{
						this.localGenerator.defName,
						" generated ",
						list.Count,
						" things"
					}));
					if (!nonNullFieldsDebugInfo.NullOrEmpty())
					{
						stringBuilder.Append(" (used custom debug params: " + nonNullFieldsDebugInfo + ")");
					}
					stringBuilder.AppendLine(":");
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine("   - " + list[i].LabelCap);
						num += list[i].MarketValue * (float)list[i].stackCount;
						if (!(list[i] is Pawn))
						{
							num2 += list[i].GetStatValue(StatDefOf.Mass, true) * (float)list[i].stackCount;
						}
						if (!GenPlace.TryPlaceThing(list[i], UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null))
						{
							list[i].Destroy(DestroyMode.Vanish);
						}
					}
					stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
					stringBuilder.AppendLine("Total mass: " + num2.ToStringMass());
					Log.Message(stringBuilder.ToString(), false);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey40
		{
			internal EffecterDef localDef;

			public <DoListingItems_MapTools>c__AnonStorey40()
			{
			}

			internal void <>m__0()
			{
				Effecter effecter = this.localDef.Spawn();
				effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), new TargetInfo(UI.MouseCell(), Find.CurrentMap, false));
				effecter.Cleanup();
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey41
		{
			internal float ms;

			public <DoListingItems_MapTools>c__AnonStorey41()
			{
			}

			internal bool <>m__0(float time)
			{
				return this.ms <= time;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey42
		{
			internal PawnKindDef kindDef;

			public <DoListingItems_MapTools>c__AnonStorey42()
			{
			}

			internal void <>m__0()
			{
				if (this.kindDef == PawnKindDefOf.Colonist)
				{
					Log.Message("Current colonist TTD reference point: 22.3 seconds, stddev 8.35 seconds", false);
				}
				List<float> results = new List<float>();
				List<PawnKindDef> list = new List<PawnKindDef>();
				List<PawnKindDef> list2 = new List<PawnKindDef>();
				list.Add(this.kindDef);
				list2.Add(this.kindDef);
				ArenaUtility.BeginArenaFightSet(1000, list, list2, delegate(ArenaUtility.ArenaResult result)
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
			}

			private sealed class <DoListingItems_MapTools>c__AnonStorey43
			{
				internal List<float> results;

				internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey42 <>f__ref$66;

				private static Func<float, string> <>f__am$cache0;

				public <DoListingItems_MapTools>c__AnonStorey43()
				{
				}

				internal void <>m__0(ArenaUtility.ArenaResult result)
				{
					if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
					{
						this.results.Add(result.tickDuration.TicksToSeconds());
					}
				}

				internal void <>m__1()
				{
					string format = "Finished {0} tests; time-to-down {1}, stddev {2}\n\nraw: {3}";
					object[] array = new object[4];
					array[0] = this.results.Count;
					array[1] = this.results.Average();
					array[2] = GenMath.Stddev(this.results);
					array[3] = (from res in this.results
					select res.ToString()).ToLineList("");
					Log.Message(string.Format(format, array), false);
				}

				private static string <>m__2(float res)
				{
					return res.ToString();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey44
		{
			internal ToolCapacityDef toolType;

			public <DoListingItems_MapTools>c__AnonStorey44()
			{
			}

			internal Tool <>m__0(Tool tool)
			{
				Tool tool2 = Gen.MemberwiseClone<Tool>(tool);
				tool2.capacities = new List<ToolCapacityDef>();
				tool2.capacities.Add(this.toolType);
				return tool2;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_World>c__AnonStorey47
		{
			internal int tile;

			public <DoListingItems_World>c__AnonStorey47()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_World>c__AnonStorey45
		{
			internal List<SitePartDef> parts;

			public <DoListingItems_World>c__AnonStorey45()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_World>c__AnonStorey46
		{
			internal SiteCoreDef localDef;

			internal Action addPart;

			internal Dialog_DebugActionsMenu.<DoListingItems_World>c__AnonStorey47 <>f__ref$71;

			internal Dialog_DebugActionsMenu.<DoListingItems_World>c__AnonStorey45 <>f__ref$69;

			public <DoListingItems_World>c__AnonStorey46()
			{
			}

			internal void <>m__0()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				list.Add(new DebugMenuOption("-Done (" + this.<>f__ref$69.parts.Count + " parts)-", DebugMenuOptionMode.Action, delegate()
				{
					Site site = SiteMaker.TryMakeSite(this.localDef, this.<>f__ref$69.parts, true, null, true);
					if (site == null)
					{
						Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						site.Tile = this.<>f__ref$71.tile;
						Find.WorldObjects.Add(site);
					}
				}));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					Dialog_DebugActionsMenu.<DoListingItems_World>c__AnonStorey45 <>f__ref$69 = this.<>f__ref$69;
					Dialog_DebugActionsMenu.<DoListingItems_World>c__AnonStorey46 <>f__ref$70 = this;
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						<>f__ref$69.parts.Add(localPart);
						<>f__ref$70.addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			}

			internal void <>m__1()
			{
				Site site = SiteMaker.TryMakeSite(this.localDef, this.<>f__ref$69.parts, true, null, true);
				if (site == null)
				{
					Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					site.Tile = this.<>f__ref$71.tile;
					Find.WorldObjects.Add(site);
				}
			}

			private sealed class <DoListingItems_World>c__AnonStorey48
			{
				internal SitePartDef localPart;

				internal Dialog_DebugActionsMenu.<DoListingItems_World>c__AnonStorey45 <>f__ref$69;

				internal Dialog_DebugActionsMenu.<DoListingItems_World>c__AnonStorey46 <>f__ref$70;

				public <DoListingItems_World>c__AnonStorey48()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$69.parts.Add(this.localPart);
					this.<>f__ref$70.addPart();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_World>c__AnonStorey49
		{
			internal Type localType;

			public <DoListingItems_World>c__AnonStorey49()
			{
			}

			internal void <>m__0()
			{
				Find.WorldCameraDriver.config = (WorldCameraConfig)Activator.CreateInstance(this.localType);
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey16
		{
			internal IntVec3 firstCorner;

			internal bool bestSideOnly;

			internal DebugTool tool;

			public <DoListingItems_MapTools>c__AnonStorey16()
			{
			}

			internal void <>m__0()
			{
				this.firstCorner = UI.MouseCell();
				DebugTools.curTool = new DebugTool("second watch rect corner...", delegate()
				{
					IntVec3 second = UI.MouseCell();
					CellRect spectateRect = CellRect.FromLimits(this.firstCorner, second).ClipInsideMap(Find.CurrentMap);
					SpectateRectSide allowedSides = SpectateRectSide.All;
					if (this.bestSideOnly)
					{
						allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1);
					}
					SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.CurrentMap, allowedSides, 1);
					DebugTools.curTool = this.tool;
				}, this.firstCorner);
			}

			internal void <>m__1()
			{
				IntVec3 second = UI.MouseCell();
				CellRect spectateRect = CellRect.FromLimits(this.firstCorner, second).ClipInsideMap(Find.CurrentMap);
				SpectateRectSide allowedSides = SpectateRectSide.All;
				if (this.bestSideOnly)
				{
					allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1);
				}
				SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.CurrentMap, allowedSides, 1);
				DebugTools.curTool = this.tool;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey3B
		{
			internal Pawn p;

			public <DoListingItems_MapTools>c__AnonStorey3B()
			{
			}

			internal bool <>m__0(PawnKindDef x)
			{
				return x.race == this.p.def;
			}
		}

		[CompilerGenerated]
		private sealed class <DoListingItems_MapTools>c__AnonStorey3C
		{
			internal PawnKindDef kLocal;

			internal Dialog_DebugActionsMenu.<DoListingItems_MapTools>c__AnonStorey3B <>f__ref$59;

			public <DoListingItems_MapTools>c__AnonStorey3C()
			{
			}

			internal void <>m__0()
			{
				PawnGenerationRequest request = new PawnGenerationRequest(this.kLocal, this.<>f__ref$59.p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				PawnGenerator.RedressPawn(this.<>f__ref$59.p, request);
				GenSpawn.Spawn(this.<>f__ref$59.p, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
				DebugTools.curTool = null;
			}
		}
	}
}
