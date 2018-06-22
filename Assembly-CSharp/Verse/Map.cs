using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.AI.Group;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000C33 RID: 3123
	public sealed class Map : IIncidentTarget, IThingHolder, IExposable, ILoadReferenceable
	{
		// Token: 0x060044C5 RID: 17605 RVA: 0x00242C91 File Offset: 0x00241091
		public Map()
		{
			MapLeakTracker.AddReference(this);
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x00242CC0 File Offset: 0x002410C0
		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060044C7 RID: 17607 RVA: 0x00242CE0 File Offset: 0x002410E0
		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x060044C8 RID: 17608 RVA: 0x00242D00 File Offset: 0x00241100
		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x060044C9 RID: 17609 RVA: 0x00242D3C File Offset: 0x0024113C
		public Faction ParentFaction
		{
			get
			{
				return this.info.parent.Faction;
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x060044CA RID: 17610 RVA: 0x00242D64 File Offset: 0x00241164
		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x060044CB RID: 17611 RVA: 0x00242D98 File Offset: 0x00241198
		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x060044CC RID: 17612 RVA: 0x00242DB8 File Offset: 0x002411B8
		public IEnumerable<IntVec3> AllCells
		{
			get
			{
				for (int z = 0; z < this.Size.z; z++)
				{
					for (int y = 0; y < this.Size.y; y++)
					{
						for (int x = 0; x < this.Size.x; x++)
						{
							yield return new IntVec3(x, y, z);
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x060044CD RID: 17613 RVA: 0x00242DE4 File Offset: 0x002411E4
		public bool IsPlayerHome
		{
			get
			{
				return this.info != null && this.info.parent is FactionBase && this.info.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x060044CE RID: 17614 RVA: 0x00242E34 File Offset: 0x00241234
		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x00242E60 File Offset: 0x00241260
		public IEnumerator<IntVec3> GetEnumerator()
		{
			foreach (IntVec3 c in this.AllCells)
			{
				yield return c;
			}
			yield break;
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x060044D0 RID: 17616 RVA: 0x00242E84 File Offset: 0x00241284
		public int Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060044D1 RID: 17617 RVA: 0x00242EA4 File Offset: 0x002412A4
		public Tile TileInfo
		{
			get
			{
				return Find.WorldGrid[this.Tile];
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060044D2 RID: 17618 RVA: 0x00242ECC File Offset: 0x002412CC
		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.biome;
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060044D3 RID: 17619 RVA: 0x00242EEC File Offset: 0x002412EC
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060044D4 RID: 17620 RVA: 0x00242F08 File Offset: 0x00241308
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060044D5 RID: 17621 RVA: 0x00242F24 File Offset: 0x00241324
		public float PlayerWealthForStoryteller
		{
			get
			{
				float result;
				if (this.IsPlayerHome)
				{
					result = this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthTameAnimals;
				}
				else
				{
					float num = 0f;
					foreach (Pawn pawn in this.mapPawns.PawnsInFaction(Faction.OfPlayer))
					{
						if (pawn.IsFreeColonist)
						{
							num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn);
						}
						if (pawn.RaceProps.Animal)
						{
							num += pawn.MarketValue;
						}
					}
					result = num;
				}
				return result;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060044D6 RID: 17622 RVA: 0x00242FFC File Offset: 0x002413FC
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060044D7 RID: 17623 RVA: 0x00243024 File Offset: 0x00241424
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x00243040 File Offset: 0x00241440
		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00243060 File Offset: 0x00241460
		public IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			return this.info.parent.AcceptedTypes();
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00243088 File Offset: 0x00241488
		public void ConstructComponents()
		{
			this.spawnedThings = new ThingOwner<Thing>(this);
			this.cellIndices = new CellIndices(this);
			this.listerThings = new ListerThings(ListerThingsUse.Global);
			this.listerBuildings = new ListerBuildings();
			this.mapPawns = new MapPawns(this);
			this.dynamicDrawManager = new DynamicDrawManager(this);
			this.mapDrawer = new MapDrawer(this);
			this.tooltipGiverList = new TooltipGiverList();
			this.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			this.reservationManager = new ReservationManager(this);
			this.physicalInteractionReservationManager = new PhysicalInteractionReservationManager();
			this.designationManager = new DesignationManager(this);
			this.lordManager = new LordManager(this);
			this.debugDrawer = new DebugCellDrawer();
			this.passingShipManager = new PassingShipManager(this);
			this.haulDestinationManager = new HaulDestinationManager(this);
			this.gameConditionManager = new GameConditionManager(this);
			this.weatherManager = new WeatherManager(this);
			this.zoneManager = new ZoneManager(this);
			this.resourceCounter = new ResourceCounter(this);
			this.mapTemperature = new MapTemperature(this);
			this.temperatureCache = new TemperatureCache(this);
			this.areaManager = new AreaManager(this);
			this.attackTargetsCache = new AttackTargetsCache(this);
			this.attackTargetReservationManager = new AttackTargetReservationManager(this);
			this.lordsStarter = new VoluntarilyJoinableLordsStarter(this);
			this.thingGrid = new ThingGrid(this);
			this.coverGrid = new CoverGrid(this);
			this.edificeGrid = new EdificeGrid(this);
			this.blueprintGrid = new BlueprintGrid(this);
			this.fogGrid = new FogGrid(this);
			this.glowGrid = new GlowGrid(this);
			this.regionGrid = new RegionGrid(this);
			this.terrainGrid = new TerrainGrid(this);
			this.pathGrid = new PathGrid(this);
			this.roofGrid = new RoofGrid(this);
			this.fertilityGrid = new FertilityGrid(this);
			this.snowGrid = new SnowGrid(this);
			this.deepResourceGrid = new DeepResourceGrid(this);
			this.exitMapGrid = new ExitMapGrid(this);
			this.linkGrid = new LinkGrid(this);
			this.glowFlooder = new GlowFlooder(this);
			this.powerNetManager = new PowerNetManager(this);
			this.powerNetGrid = new PowerNetGrid(this);
			this.regionMaker = new RegionMaker(this);
			this.pathFinder = new PathFinder(this);
			this.pawnPathPool = new PawnPathPool(this);
			this.regionAndRoomUpdater = new RegionAndRoomUpdater(this);
			this.regionLinkDatabase = new RegionLinkDatabase();
			this.moteCounter = new MoteCounter();
			this.gatherSpotLister = new GatherSpotLister();
			this.windManager = new WindManager(this);
			this.listerBuildingsRepairable = new ListerBuildingsRepairable();
			this.listerHaulables = new ListerHaulables(this);
			this.listerMergeables = new ListerMergeables(this);
			this.listerFilthInHomeArea = new ListerFilthInHomeArea(this);
			this.reachability = new Reachability(this);
			this.itemAvailability = new ItemAvailability(this);
			this.autoBuildRoofAreaSetter = new AutoBuildRoofAreaSetter(this);
			this.roofCollapseBufferResolver = new RoofCollapseBufferResolver(this);
			this.roofCollapseBuffer = new RoofCollapseBuffer();
			this.wildAnimalSpawner = new WildAnimalSpawner(this);
			this.wildPlantSpawner = new WildPlantSpawner(this);
			this.steadyEnvironmentEffects = new SteadyEnvironmentEffects(this);
			this.skyManager = new SkyManager(this);
			this.overlayDrawer = new OverlayDrawer();
			this.floodFiller = new FloodFiller(this);
			this.weatherDecider = new WeatherDecider(this);
			this.fireWatcher = new FireWatcher(this);
			this.dangerWatcher = new DangerWatcher(this);
			this.damageWatcher = new DamageWatcher();
			this.strengthWatcher = new StrengthWatcher(this);
			this.wealthWatcher = new WealthWatcher(this);
			this.regionDirtyer = new RegionDirtyer(this);
			this.cellsInRandomOrder = new MapCellsInRandomOrder(this);
			this.rememberedCameraPos = new RememberedCameraPos(this);
			this.mineStrikeManager = new MineStrikeManager();
			this.storyState = new StoryState(this);
			this.retainedCaravanData = new RetainedCaravanData(this);
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x00243450 File Offset: 0x00241850
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Deep.Look<MapInfo>(ref this.info, "mapInfo", new object[0]);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.compressor = new MapFileCompressor(this);
				this.compressor.BuildCompressedString();
				this.ExposeComponents();
				this.compressor.ExposeData();
				HashSet<string> hashSet = new HashSet<string>();
				if (Scribe.EnterNode("things"))
				{
					try
					{
						foreach (Thing thing in this.listerThings.AllThings)
						{
							try
							{
								if (thing.def.isSaveable && !thing.IsSaveCompressible())
								{
									if (hashSet.Contains(thing.ThingID))
									{
										Log.Error("Saving Thing with already-used ID " + thing.ThingID, false);
									}
									else
									{
										hashSet.Add(thing.ThingID);
									}
									Thing thing2 = thing;
									Scribe_Deep.Look<Thing>(ref thing2, "thing", new object[0]);
								}
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Exception saving ",
									thing,
									": ",
									ex
								}), false);
							}
						}
					}
					finally
					{
						Scribe.ExitNode();
					}
				}
				else
				{
					Log.Error("Could not enter the things node while saving.", false);
				}
				this.compressor = null;
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					this.ConstructComponents();
					this.regionAndRoomUpdater.Enabled = false;
					this.compressor = new MapFileCompressor(this);
				}
				this.ExposeComponents();
				DeepProfiler.Start("Load compressed things");
				this.compressor.ExposeData();
				DeepProfiler.End();
				DeepProfiler.Start("Load non-compressed things");
				Scribe_Collections.Look<Thing>(ref this.loadedFullThings, "things", LookMode.Deep, new object[0]);
				DeepProfiler.End();
			}
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x0024367C File Offset: 0x00241A7C
		private void FillComponents()
		{
			this.components.RemoveAll((MapComponent component) => component == null);
			foreach (Type type in typeof(MapComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					MapComponent item = (MapComponent)Activator.CreateInstance(type, new object[]
					{
						this
					});
					this.components.Add(item);
				}
			}
			this.roadInfo = this.GetComponent<RoadInfo>();
			this.waterInfo = this.GetComponent<WaterInfo>();
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x0024374C File Offset: 0x00241B4C
		public void FinalizeLoading()
		{
			List<Thing> list = this.compressor.ThingsToSpawnAfterLoad().ToList<Thing>();
			this.compressor = null;
			DeepProfiler.Start("Merge compressed and non-compressed thing lists");
			List<Thing> list2 = new List<Thing>(this.loadedFullThings.Count + list.Count);
			foreach (Thing item in this.loadedFullThings.Concat(list))
			{
				list2.Add(item);
			}
			this.loadedFullThings.Clear();
			DeepProfiler.End();
			DeepProfiler.Start("Spawn everything into the map");
			foreach (Thing thing in list2)
			{
				if (!(thing is Building))
				{
					try
					{
						GenSpawn.Spawn(thing, thing.Position, this, thing.Rotation, WipeMode.FullRefund, true);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception spawning loaded thing ",
							thing.ToStringSafe<Thing>(),
							": ",
							ex
						}), false);
					}
				}
			}
			foreach (Building building in from t in list2.OfType<Building>()
			orderby t.def.size.Magnitude
			select t)
			{
				try
				{
					GenSpawn.SpawnBuildingAsPossible(building, this, true);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception spawning loaded thing ",
						building.ToStringSafe<Building>(),
						": ",
						ex2
					}), false);
				}
			}
			DeepProfiler.End();
			this.FinalizeInit();
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x0024398C File Offset: 0x00241D8C
		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			this.regionAndRoomUpdater.Enabled = true;
			this.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.temperatureCache.temperatureSaveLoad.ApplyLoadedDataToRegions();
			foreach (Thing thing in this.listerThings.AllThings.ToList<Thing>())
			{
				try
				{
					thing.PostMapInit();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception PostMapInit in ",
						thing,
						": ",
						ex
					}), false);
				}
			}
			this.listerFilthInHomeArea.RebuildAll();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mapDrawer.RegenerateEverythingNow();
			});
			this.resourceCounter.UpdateResourceCounts();
			this.wealthWatcher.ForceRecount(true);
			MapComponentUtility.FinalizeInit(this);
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x00243AB0 File Offset: 0x00241EB0
		private void ExposeComponents()
		{
			Scribe_Deep.Look<WeatherManager>(ref this.weatherManager, "weatherManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<ReservationManager>(ref this.reservationManager, "reservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PhysicalInteractionReservationManager>(ref this.physicalInteractionReservationManager, "physicalInteractionReservationManager", new object[0]);
			Scribe_Deep.Look<DesignationManager>(ref this.designationManager, "designationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PawnDestinationReservationManager>(ref this.pawnDestinationReservationManager, "pawnDestinationReservationManager", new object[0]);
			Scribe_Deep.Look<LordManager>(ref this.lordManager, "lordManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PassingShipManager>(ref this.passingShipManager, "visitorManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<FogGrid>(ref this.fogGrid, "fogGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<RoofGrid>(ref this.roofGrid, "roofGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<TerrainGrid>(ref this.terrainGrid, "terrainGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<ZoneManager>(ref this.zoneManager, "zoneManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemperatureCache>(ref this.temperatureCache, "temperatureCache", new object[]
			{
				this
			});
			Scribe_Deep.Look<SnowGrid>(ref this.snowGrid, "snowGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<AreaManager>(ref this.areaManager, "areaManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<VoluntarilyJoinableLordsStarter>(ref this.lordsStarter, "lordsStarter", new object[]
			{
				this
			});
			Scribe_Deep.Look<AttackTargetReservationManager>(ref this.attackTargetReservationManager, "attackTargetReservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeepResourceGrid>(ref this.deepResourceGrid, "deepResourceGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WeatherDecider>(ref this.weatherDecider, "weatherDecider", new object[]
			{
				this
			});
			Scribe_Deep.Look<DamageWatcher>(ref this.damageWatcher, "damageWatcher", new object[0]);
			Scribe_Deep.Look<RememberedCameraPos>(ref this.rememberedCameraPos, "rememberedCameraPos", new object[]
			{
				this
			});
			Scribe_Deep.Look<MineStrikeManager>(ref this.mineStrikeManager, "mineStrikeManager", new object[0]);
			Scribe_Deep.Look<RetainedCaravanData>(ref this.retainedCaravanData, "retainedCaravanData", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WildPlantSpawner>(ref this.wildPlantSpawner, "wildPlantSpawner", new object[]
			{
				this
			});
			Scribe_Collections.Look<MapComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.MapPostLoadInit(this);
			}
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x00243D6C File Offset: 0x0024216C
		public void MapPreTick()
		{
			Profiler.BeginSample("ItemAvailabilityUtility.Tick()");
			this.itemAvailability.Tick();
			Profiler.EndSample();
			Profiler.BeginSample("ListerHaulables.ListerHaulablesTick");
			this.listerHaulables.ListerHaulablesTick();
			Profiler.EndSample();
			Profiler.BeginSample("AutoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick()");
			try
			{
				this.autoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick_First();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("RoofCollapseChecker.RoofCollapseCheckerTick_First()");
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			Profiler.EndSample();
			Profiler.BeginSample("WindManager.WindManagerTick()");
			this.windManager.WindManagerTick();
			Profiler.EndSample();
			Profiler.BeginSample("MapTemperature.MapTemperatureTick()");
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			Profiler.EndSample();
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x00243E68 File Offset: 0x00242268
		public void MapPostTick()
		{
			Profiler.BeginSample("WildAnimalSpawnerTick()");
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("WildPlantSpawnerTick()");
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("PowerNetManager.PowerNetsTick()");
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("SteadyEnvironmentEffects.SteadyEnvironmentEffectsTick()");
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("LordManagerTick()");
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("PassingShipManagerTick()");
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("DebugDrawer.DebugDrawerTick()");
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("VoluntarilyJoinableLordsStarterTick()");
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("GameConditionManager.GameConditionManagerTick()");
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("WeatherManager.WeatherManagerTick()");
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("ResourceCounter.ResourceCounterTick()");
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("WeatherDecided.WeatherDeciderTick()");
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("FireWatcher.FireWatcherTick()");
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("DamageWatcher.DamageWatcherTick()");
			try
			{
				this.damageWatcher.DamageWatcherTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("MapComponentTick()");
			MapComponentUtility.MapComponentTick(this);
			Profiler.EndSample();
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x00244234 File Offset: 0x00242634
		public void MapUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			Profiler.BeginSample("SkyManagerUpdate()");
			this.skyManager.SkyManagerUpdate();
			Profiler.EndSample();
			Profiler.BeginSample("PowerNetManager.UpdatePowerNetsAndConnections_First()");
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			Profiler.EndSample();
			Profiler.BeginSample("regionGrid.UpdateClean()");
			this.regionGrid.UpdateClean();
			Profiler.EndSample();
			Profiler.BeginSample("RegionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms()");
			this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			Profiler.EndSample();
			Profiler.BeginSample("glowGrid.GlowGridUpdate_First()");
			this.glowGrid.GlowGridUpdate_First();
			Profiler.EndSample();
			Profiler.BeginSample("LordManagerUpdate()");
			this.lordManager.LordManagerUpdate();
			Profiler.EndSample();
			if (!worldRenderedNow && Find.CurrentMap == this)
			{
				if (Map.AlwaysRedrawShadows)
				{
					this.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
				}
				Profiler.BeginSample("FallIntensityUpdate");
				GenPlant.SetFallShaderGlobals(this);
				Profiler.EndSample();
				Profiler.BeginSample("waterInfo.SetTextures()");
				this.waterInfo.SetTextures();
				Profiler.EndSample();
				Profiler.BeginSample("FactionsDebugDrawOnMap()");
				Find.FactionManager.FactionsDebugDrawOnMap();
				Profiler.EndSample();
				Profiler.BeginSample("mapDrawer.MapMeshDrawerUpdate_First");
				this.mapDrawer.MapMeshDrawerUpdate_First();
				Profiler.EndSample();
				Profiler.BeginSample("PowerNetGrid.DrawDebugPowerNetGrid()");
				this.powerNetGrid.DrawDebugPowerNetGrid();
				Profiler.EndSample();
				Profiler.BeginSample("DoorsDebugDrawer.DrawDebug()");
				DoorsDebugDrawer.DrawDebug();
				Profiler.EndSample();
				Profiler.BeginSample("mapDrawer.DrawMapMesh");
				this.mapDrawer.DrawMapMesh();
				Profiler.EndSample();
				Profiler.BeginSample("drawManager.DrawDynamicThings");
				this.dynamicDrawManager.DrawDynamicThings();
				Profiler.EndSample();
				Profiler.BeginSample("GameConditionManagerDraw");
				this.gameConditionManager.GameConditionManagerDraw(this);
				Profiler.EndSample();
				Profiler.BeginSample("DrawClippers");
				MapEdgeClipDrawer.DrawClippers(this);
				Profiler.EndSample();
				Profiler.BeginSample("designationManager.DrawDesignations()");
				this.designationManager.DrawDesignations();
				Profiler.EndSample();
				Profiler.BeginSample("OverlayDrawer.DrawAllOverlays()");
				this.overlayDrawer.DrawAllOverlays();
				Profiler.EndSample();
			}
			Profiler.BeginSample("AreaManagerUpdate()");
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("WeatherManagerUpdate()");
			this.weatherManager.WeatherManagerUpdate();
			Profiler.EndSample();
			Profiler.BeginSample("MapComponentUpdate()");
			MapComponentUtility.MapComponentUpdate(this);
			Profiler.EndSample();
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x002444AC File Offset: 0x002428AC
		public T GetComponent<T>() where T : MapComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				T t = this.components[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x00244510 File Offset: 0x00242910
		public MapComponent GetComponent(Type type)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (type.IsAssignableFrom(this.components[i].GetType()))
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00244574 File Offset: 0x00242974
		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID;
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x002445A0 File Offset: 0x002429A0
		public override string ToString()
		{
			string str = "(Map-" + this.uniqueID;
			if (this.IsPlayerHome)
			{
				str += "-PlayerHome";
			}
			return str + ")";
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x002445F0 File Offset: 0x002429F0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x0024460C File Offset: 0x00242A0C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder));
			List<PassingShip> passingShips = this.passingShipManager.passingShips;
			for (int i = 0; i < passingShips.Count; i++)
			{
				IThingHolder thingHolder = passingShips[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
			}
			for (int j = 0; j < this.components.Count; j++)
			{
				IThingHolder thingHolder2 = this.components[j] as IThingHolder;
				if (thingHolder2 != null)
				{
					outChildren.Add(thingHolder2);
				}
			}
		}

		// Token: 0x04002EBF RID: 11967
		public MapFileCompressor compressor;

		// Token: 0x04002EC0 RID: 11968
		private List<Thing> loadedFullThings;

		// Token: 0x04002EC1 RID: 11969
		public int uniqueID = -1;

		// Token: 0x04002EC2 RID: 11970
		public MapInfo info = new MapInfo();

		// Token: 0x04002EC3 RID: 11971
		public List<MapComponent> components = new List<MapComponent>();

		// Token: 0x04002EC4 RID: 11972
		public ThingOwner spawnedThings;

		// Token: 0x04002EC5 RID: 11973
		public CellIndices cellIndices;

		// Token: 0x04002EC6 RID: 11974
		public ListerThings listerThings;

		// Token: 0x04002EC7 RID: 11975
		public ListerBuildings listerBuildings;

		// Token: 0x04002EC8 RID: 11976
		public MapPawns mapPawns;

		// Token: 0x04002EC9 RID: 11977
		public DynamicDrawManager dynamicDrawManager;

		// Token: 0x04002ECA RID: 11978
		public MapDrawer mapDrawer;

		// Token: 0x04002ECB RID: 11979
		public PawnDestinationReservationManager pawnDestinationReservationManager;

		// Token: 0x04002ECC RID: 11980
		public TooltipGiverList tooltipGiverList;

		// Token: 0x04002ECD RID: 11981
		public ReservationManager reservationManager;

		// Token: 0x04002ECE RID: 11982
		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		// Token: 0x04002ECF RID: 11983
		public DesignationManager designationManager;

		// Token: 0x04002ED0 RID: 11984
		public LordManager lordManager;

		// Token: 0x04002ED1 RID: 11985
		public PassingShipManager passingShipManager;

		// Token: 0x04002ED2 RID: 11986
		public HaulDestinationManager haulDestinationManager;

		// Token: 0x04002ED3 RID: 11987
		public DebugCellDrawer debugDrawer;

		// Token: 0x04002ED4 RID: 11988
		public GameConditionManager gameConditionManager;

		// Token: 0x04002ED5 RID: 11989
		public WeatherManager weatherManager;

		// Token: 0x04002ED6 RID: 11990
		public ZoneManager zoneManager;

		// Token: 0x04002ED7 RID: 11991
		public ResourceCounter resourceCounter;

		// Token: 0x04002ED8 RID: 11992
		public MapTemperature mapTemperature;

		// Token: 0x04002ED9 RID: 11993
		public TemperatureCache temperatureCache;

		// Token: 0x04002EDA RID: 11994
		public AreaManager areaManager;

		// Token: 0x04002EDB RID: 11995
		public AttackTargetsCache attackTargetsCache;

		// Token: 0x04002EDC RID: 11996
		public AttackTargetReservationManager attackTargetReservationManager;

		// Token: 0x04002EDD RID: 11997
		public VoluntarilyJoinableLordsStarter lordsStarter;

		// Token: 0x04002EDE RID: 11998
		public ThingGrid thingGrid;

		// Token: 0x04002EDF RID: 11999
		public CoverGrid coverGrid;

		// Token: 0x04002EE0 RID: 12000
		public EdificeGrid edificeGrid;

		// Token: 0x04002EE1 RID: 12001
		public BlueprintGrid blueprintGrid;

		// Token: 0x04002EE2 RID: 12002
		public FogGrid fogGrid;

		// Token: 0x04002EE3 RID: 12003
		public RegionGrid regionGrid;

		// Token: 0x04002EE4 RID: 12004
		public GlowGrid glowGrid;

		// Token: 0x04002EE5 RID: 12005
		public TerrainGrid terrainGrid;

		// Token: 0x04002EE6 RID: 12006
		public PathGrid pathGrid;

		// Token: 0x04002EE7 RID: 12007
		public RoofGrid roofGrid;

		// Token: 0x04002EE8 RID: 12008
		public FertilityGrid fertilityGrid;

		// Token: 0x04002EE9 RID: 12009
		public SnowGrid snowGrid;

		// Token: 0x04002EEA RID: 12010
		public DeepResourceGrid deepResourceGrid;

		// Token: 0x04002EEB RID: 12011
		public ExitMapGrid exitMapGrid;

		// Token: 0x04002EEC RID: 12012
		public LinkGrid linkGrid;

		// Token: 0x04002EED RID: 12013
		public GlowFlooder glowFlooder;

		// Token: 0x04002EEE RID: 12014
		public PowerNetManager powerNetManager;

		// Token: 0x04002EEF RID: 12015
		public PowerNetGrid powerNetGrid;

		// Token: 0x04002EF0 RID: 12016
		public RegionMaker regionMaker;

		// Token: 0x04002EF1 RID: 12017
		public PathFinder pathFinder;

		// Token: 0x04002EF2 RID: 12018
		public PawnPathPool pawnPathPool;

		// Token: 0x04002EF3 RID: 12019
		public RegionAndRoomUpdater regionAndRoomUpdater;

		// Token: 0x04002EF4 RID: 12020
		public RegionLinkDatabase regionLinkDatabase;

		// Token: 0x04002EF5 RID: 12021
		public MoteCounter moteCounter;

		// Token: 0x04002EF6 RID: 12022
		public GatherSpotLister gatherSpotLister;

		// Token: 0x04002EF7 RID: 12023
		public WindManager windManager;

		// Token: 0x04002EF8 RID: 12024
		public ListerBuildingsRepairable listerBuildingsRepairable;

		// Token: 0x04002EF9 RID: 12025
		public ListerHaulables listerHaulables;

		// Token: 0x04002EFA RID: 12026
		public ListerMergeables listerMergeables;

		// Token: 0x04002EFB RID: 12027
		public ListerFilthInHomeArea listerFilthInHomeArea;

		// Token: 0x04002EFC RID: 12028
		public Reachability reachability;

		// Token: 0x04002EFD RID: 12029
		public ItemAvailability itemAvailability;

		// Token: 0x04002EFE RID: 12030
		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		// Token: 0x04002EFF RID: 12031
		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		// Token: 0x04002F00 RID: 12032
		public RoofCollapseBuffer roofCollapseBuffer;

		// Token: 0x04002F01 RID: 12033
		public WildAnimalSpawner wildAnimalSpawner;

		// Token: 0x04002F02 RID: 12034
		public WildPlantSpawner wildPlantSpawner;

		// Token: 0x04002F03 RID: 12035
		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		// Token: 0x04002F04 RID: 12036
		public SkyManager skyManager;

		// Token: 0x04002F05 RID: 12037
		public OverlayDrawer overlayDrawer;

		// Token: 0x04002F06 RID: 12038
		public FloodFiller floodFiller;

		// Token: 0x04002F07 RID: 12039
		public WeatherDecider weatherDecider;

		// Token: 0x04002F08 RID: 12040
		public FireWatcher fireWatcher;

		// Token: 0x04002F09 RID: 12041
		public DangerWatcher dangerWatcher;

		// Token: 0x04002F0A RID: 12042
		public DamageWatcher damageWatcher;

		// Token: 0x04002F0B RID: 12043
		public StrengthWatcher strengthWatcher;

		// Token: 0x04002F0C RID: 12044
		public WealthWatcher wealthWatcher;

		// Token: 0x04002F0D RID: 12045
		public RegionDirtyer regionDirtyer;

		// Token: 0x04002F0E RID: 12046
		public MapCellsInRandomOrder cellsInRandomOrder;

		// Token: 0x04002F0F RID: 12047
		public RememberedCameraPos rememberedCameraPos;

		// Token: 0x04002F10 RID: 12048
		public MineStrikeManager mineStrikeManager;

		// Token: 0x04002F11 RID: 12049
		public StoryState storyState;

		// Token: 0x04002F12 RID: 12050
		public RoadInfo roadInfo;

		// Token: 0x04002F13 RID: 12051
		public WaterInfo waterInfo;

		// Token: 0x04002F14 RID: 12052
		public RetainedCaravanData retainedCaravanData;

		// Token: 0x04002F15 RID: 12053
		public const string ThingSaveKey = "thing";

		// Token: 0x04002F16 RID: 12054
		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows = false;
	}
}
