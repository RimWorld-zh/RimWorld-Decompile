using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;
using Verse.AI.Group;
using Verse.Profile;

namespace Verse
{
	public sealed class Map : IIncidentTarget, IThingHolder, IExposable, ILoadReferenceable
	{
		public MapFileCompressor compressor;

		private List<Thing> loadedFullThings;

		public int uniqueID = -1;

		public MapInfo info = new MapInfo();

		public List<MapComponent> components = new List<MapComponent>();

		public ThingOwner spawnedThings;

		public CellIndices cellIndices;

		public ListerThings listerThings;

		public ListerBuildings listerBuildings;

		public MapPawns mapPawns;

		public DynamicDrawManager dynamicDrawManager;

		public MapDrawer mapDrawer;

		public PawnDestinationReservationManager pawnDestinationReservationManager;

		public TooltipGiverList tooltipGiverList;

		public ReservationManager reservationManager;

		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		public DesignationManager designationManager;

		public LordManager lordManager;

		public PassingShipManager passingShipManager;

		public HaulDestinationManager haulDestinationManager;

		public DebugCellDrawer debugDrawer;

		public GameConditionManager gameConditionManager;

		public WeatherManager weatherManager;

		public ZoneManager zoneManager;

		public ResourceCounter resourceCounter;

		public MapTemperature mapTemperature;

		public TemperatureCache temperatureCache;

		public AreaManager areaManager;

		public AttackTargetsCache attackTargetsCache;

		public AttackTargetReservationManager attackTargetReservationManager;

		public VoluntarilyJoinableLordsStarter lordsStarter;

		public ThingGrid thingGrid;

		public CoverGrid coverGrid;

		public EdificeGrid edificeGrid;

		public BlueprintGrid blueprintGrid;

		public FogGrid fogGrid;

		public RegionGrid regionGrid;

		public GlowGrid glowGrid;

		public TerrainGrid terrainGrid;

		public PathGrid pathGrid;

		public RoofGrid roofGrid;

		public FertilityGrid fertilityGrid;

		public SnowGrid snowGrid;

		public DeepResourceGrid deepResourceGrid;

		public ExitMapGrid exitMapGrid;

		public LinkGrid linkGrid;

		public GlowFlooder glowFlooder;

		public PowerNetManager powerNetManager;

		public PowerNetGrid powerNetGrid;

		public RegionMaker regionMaker;

		public PathFinder pathFinder;

		public PawnPathPool pawnPathPool;

		public RegionAndRoomUpdater regionAndRoomUpdater;

		public RegionLinkDatabase regionLinkDatabase;

		public MoteCounter moteCounter;

		public GatherSpotLister gatherSpotLister;

		public WindManager windManager;

		public ListerBuildingsRepairable listerBuildingsRepairable;

		public ListerHaulables listerHaulables;

		public ListerMergeables listerMergeables;

		public ListerFilthInHomeArea listerFilthInHomeArea;

		public Reachability reachability;

		public ItemAvailability itemAvailability;

		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		public RoofCollapseBuffer roofCollapseBuffer;

		public WildAnimalSpawner wildAnimalSpawner;

		public WildPlantSpawner wildPlantSpawner;

		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		public SkyManager skyManager;

		public OverlayDrawer overlayDrawer;

		public FloodFiller floodFiller;

		public WeatherDecider weatherDecider;

		public FireWatcher fireWatcher;

		public DangerWatcher dangerWatcher;

		public DamageWatcher damageWatcher;

		public StrengthWatcher strengthWatcher;

		public WealthWatcher wealthWatcher;

		public RegionDirtyer regionDirtyer;

		public MapCellsInRandomOrder cellsInRandomOrder;

		public RememberedCameraPos rememberedCameraPos;

		public MineStrikeManager mineStrikeManager;

		public StoryState storyState;

		public RoadInfo roadInfo;

		public WaterInfo waterInfo;

		public RetainedCaravanData retainedCaravanData;

		public const string ThingSaveKey = "thing";

		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows;

		[CompilerGenerated]
		private static Predicate<MapComponent> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Building, float> <>f__am$cache1;

		public Map()
		{
			MapLeakTracker.AddReference(this);
		}

		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		public Faction ParentFaction
		{
			get
			{
				return this.info.parent.Faction;
			}
		}

		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

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

		public bool IsPlayerHome
		{
			get
			{
				return this.info != null && this.info.parent.def.canBePlayerHome && this.info.parent.Faction == Faction.OfPlayer;
			}
		}

		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		public IEnumerator<IntVec3> GetEnumerator()
		{
			foreach (IntVec3 c in this.AllCells)
			{
				yield return c;
			}
			yield break;
		}

		public int Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		public Tile TileInfo
		{
			get
			{
				return Find.WorldGrid[this.Tile];
			}
		}

		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.biome;
			}
		}

		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		public float PlayerWealthForStoryteller
		{
			get
			{
				if (this.IsPlayerHome)
				{
					return this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthPawns;
				}
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
				return num;
			}
		}

		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return this.info.parent.IncidentTargetTags();
		}

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
			BackCompatibility.PreCheckSpawnBackCompatibleThingAfterLoading(this);
			foreach (Thing thing in list2)
			{
				if (!(thing is Building))
				{
					try
					{
						if (!BackCompatibility.CheckSpawnBackCompatibleThingAfterLoading(thing, this))
						{
							GenSpawn.Spawn(thing, thing.Position, this, thing.Rotation, WipeMode.FullRefund, true);
						}
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
			BackCompatibility.PostCheckSpawnBackCompatibleThingAfterLoading(this);
			DeepProfiler.End();
			this.FinalizeInit();
		}

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
						"Error in PostMapInit() for ",
						thing.ToStringSafe<Thing>(),
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

		public void MapPreTick()
		{
			this.itemAvailability.Tick();
			this.listerHaulables.ListerHaulablesTick();
			try
			{
				this.autoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick_First();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			this.windManager.WindManagerTick();
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
		}

		public void MapPostTick()
		{
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString(), false);
			}
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString(), false);
			}
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString(), false);
			}
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString(), false);
			}
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString(), false);
			}
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString(), false);
			}
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString(), false);
			}
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString(), false);
			}
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString(), false);
			}
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString(), false);
			}
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString(), false);
			}
			MapComponentUtility.MapComponentTick(this);
		}

		public void MapUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.skyManager.SkyManagerUpdate();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.regionGrid.UpdateClean();
			this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			this.glowGrid.GlowGridUpdate_First();
			this.lordManager.LordManagerUpdate();
			if (!worldRenderedNow && Find.CurrentMap == this)
			{
				if (Map.AlwaysRedrawShadows)
				{
					this.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
				}
				PlantFallColors.SetFallShaderGlobals(this);
				this.waterInfo.SetTextures();
				Find.FactionManager.FactionsDebugDrawOnMap();
				this.mapDrawer.MapMeshDrawerUpdate_First();
				this.powerNetGrid.DrawDebugPowerNetGrid();
				DoorsDebugDrawer.DrawDebug();
				this.mapDrawer.DrawMapMesh();
				this.dynamicDrawManager.DrawDynamicThings();
				this.gameConditionManager.GameConditionManagerDraw(this);
				MapEdgeClipDrawer.DrawClippers(this);
				this.designationManager.DrawDesignations();
				this.overlayDrawer.DrawAllOverlays();
			}
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			this.weatherManager.WeatherManagerUpdate();
			MapComponentUtility.MapComponentUpdate(this);
		}

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

		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueID ^ 16622162;
			}
		}

		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID;
		}

		public override string ToString()
		{
			string str = "(Map-" + this.uniqueID;
			if (this.IsPlayerHome)
			{
				str += "-PlayerHome";
			}
			return str + ")";
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static Map()
		{
		}

		[CompilerGenerated]
		private static bool <FillComponents>m__0(MapComponent component)
		{
			return component == null;
		}

		[CompilerGenerated]
		private static float <FinalizeLoading>m__1(Building t)
		{
			return t.def.size.Magnitude;
		}

		[CompilerGenerated]
		private void <FinalizeInit>m__2()
		{
			this.mapDrawer.RegenerateEverythingNow();
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <z>__1;

			internal int <y>__2;

			internal int <x>__3;

			internal Map $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					z = 0;
					goto IL_DC;
				case 1u:
					x++;
					break;
				default:
					return false;
				}
				IL_84:
				if (x < base.Size.x)
				{
					this.$current = new IntVec3(x, y, z);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				y++;
				IL_B0:
				if (y < base.Size.y)
				{
					x = 0;
					goto IL_84;
				}
				z++;
				IL_DC:
				if (z < base.Size.z)
				{
					y = 0;
					goto IL_B0;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Map.<>c__Iterator0 <>c__Iterator = new Map.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetEnumerator>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__1;

			internal Map $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetEnumerator>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.AllCells.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}
		}
	}
}
