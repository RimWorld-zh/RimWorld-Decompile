using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Profiling;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	public sealed class World : IThingHolder, IExposable, IIncidentTarget, ILoadReferenceable
	{
		public WorldInfo info = new WorldInfo();

		public List<WorldComponent> components = new List<WorldComponent>();

		public FactionManager factionManager;

		public WorldPawns worldPawns;

		public WorldObjectsHolder worldObjects;

		public GameConditionManager gameConditionManager;

		public StoryState storyState;

		public WorldFeatures features;

		public WorldGrid grid;

		public WorldPathGrid pathGrid;

		public WorldRenderer renderer;

		public WorldInterface UI;

		public WorldDebugDrawer debugDrawer;

		public WorldDynamicDrawManager dynamicDrawManager;

		public WorldPathFinder pathFinder;

		public WorldPathPool pathPool;

		public WorldReachability reachability;

		public WorldFloodFiller floodFiller;

		public ConfiguredTicksAbsAtGameStartCache ticksAbsCache;

		public TileTemperaturesComp tileTemperatures;

		public WorldGenData genData;

		private static List<int> tmpNeighbors = new List<int>();

		private static List<Rot4> tmpOceanDirs = new List<Rot4>();

		[CompilerGenerated]
		private static Predicate<WorldComponent> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1;

		public World()
		{
		}

		public float PlanetCoverage
		{
			get
			{
				return this.info.planetCoverage;
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return null;
			}
		}

		public int Tile
		{
			get
			{
				return -1;
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
				float num = 0f;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					num += maps[i].PlayerWealthForStoryteller;
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					num += caravans[j].PlayerWealthForStoryteller;
				}
				return num;
			}
		}

		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
			}
		}

		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		public IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			yield return IncidentTargetTypeDefOf.World;
			yield break;
		}

		public void ExposeData()
		{
			Scribe_Deep.Look<WorldInfo>(ref this.info, "info", new object[0]);
			Scribe_Deep.Look<WorldGrid>(ref this.grid, "grid", new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.grid == null || !this.grid.HasWorldData)
				{
					WorldGenerator.GenerateWithoutWorldData(this.info.seedString);
				}
				else
				{
					WorldGenerator.GenerateFromScribe(this.info.seedString);
				}
			}
			else
			{
				this.ExposeComponents();
			}
		}

		public void ExposeComponents()
		{
			Scribe_Deep.Look<FactionManager>(ref this.factionManager, "factionManager", new object[0]);
			Scribe_Deep.Look<WorldPawns>(ref this.worldPawns, "worldPawns", new object[0]);
			Scribe_Deep.Look<WorldObjectsHolder>(ref this.worldObjects, "worldObjects", new object[0]);
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WorldFeatures>(ref this.features, "features", new object[0]);
			Scribe_Collections.Look<WorldComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.WorldLoadingVars();
			}
		}

		public void ConstructComponents()
		{
			this.worldObjects = new WorldObjectsHolder();
			this.factionManager = new FactionManager();
			this.worldPawns = new WorldPawns();
			this.gameConditionManager = new GameConditionManager(this);
			this.storyState = new StoryState(this);
			this.renderer = new WorldRenderer();
			this.UI = new WorldInterface();
			this.debugDrawer = new WorldDebugDrawer();
			this.dynamicDrawManager = new WorldDynamicDrawManager();
			this.pathFinder = new WorldPathFinder();
			this.pathPool = new WorldPathPool();
			this.reachability = new WorldReachability();
			this.floodFiller = new WorldFloodFiller();
			this.ticksAbsCache = new ConfiguredTicksAbsAtGameStartCache();
			this.components.Clear();
			this.FillComponents();
		}

		private void FillComponents()
		{
			this.components.RemoveAll((WorldComponent component) => component == null);
			foreach (Type type in typeof(WorldComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					WorldComponent item = (WorldComponent)Activator.CreateInstance(type, new object[]
					{
						this
					});
					this.components.Add(item);
				}
			}
			this.tileTemperatures = this.GetComponent<TileTemperaturesComp>();
			this.genData = this.GetComponent<WorldGenData>();
		}

		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			AmbientSoundManager.EnsureWorldAmbientSoundCreated();
			WorldComponentUtility.FinalizeInit(this);
		}

		public void WorldTick()
		{
			Profiler.BeginSample("WorldPawnsTick()");
			this.worldPawns.WorldPawnsTick();
			Profiler.EndSample();
			Profiler.BeginSample("FactionManagerTick()");
			this.factionManager.FactionManagerTick();
			Profiler.EndSample();
			Profiler.BeginSample("WorldObjectsHolderTick()");
			this.worldObjects.WorldObjectsHolderTick();
			Profiler.EndSample();
			Profiler.BeginSample("WorldDebugDrawerTick()");
			this.debugDrawer.WorldDebugDrawerTick();
			Profiler.EndSample();
			Profiler.BeginSample("WorldPathGridTick()");
			this.pathGrid.WorldPathGridTick();
			Profiler.EndSample();
			Profiler.BeginSample("WorldComponentTick()");
			WorldComponentUtility.WorldComponentTick(this);
			Profiler.EndSample();
		}

		public void WorldPostTick()
		{
			Profiler.BeginSample("GameConditionManager.GameConditionManagerTick()");
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			Profiler.EndSample();
		}

		public void WorldUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.renderer.CheckActivateWorldCamera();
			if (worldRenderedNow)
			{
				Profiler.BeginSample("ExpandableWorldObjectsUpdate()");
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsUpdate();
				Profiler.EndSample();
				Profiler.BeginSample("World.renderer.DrawWorldLayers()");
				this.renderer.DrawWorldLayers();
				Profiler.EndSample();
				Profiler.BeginSample("World.dynamicDrawManager.DrawDynamicWorldObjects()");
				this.dynamicDrawManager.DrawDynamicWorldObjects();
				Profiler.EndSample();
				Profiler.BeginSample("World.features.UpdateFeatures()");
				this.features.UpdateFeatures();
				Profiler.EndSample();
				Profiler.BeginSample("NoiseDebugUI.RenderPlanetNoise()");
				NoiseDebugUI.RenderPlanetNoise();
				Profiler.EndSample();
			}
			Profiler.BeginSample("WorldComponentUpdate()");
			WorldComponentUtility.WorldComponentUpdate(this);
			Profiler.EndSample();
		}

		public T GetComponent<T>() where T : WorldComponent
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

		public WorldComponent GetComponent(Type type)
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

		public Rot4 CoastDirectionAt(int tileID)
		{
			Tile tile = this.grid[tileID];
			Rot4 result;
			if (!tile.biome.canBuildBase)
			{
				result = Rot4.Invalid;
			}
			else
			{
				World.tmpOceanDirs.Clear();
				this.grid.GetTileNeighbors(tileID, World.tmpNeighbors);
				int i = 0;
				int count = World.tmpNeighbors.Count;
				while (i < count)
				{
					Tile tile2 = this.grid[World.tmpNeighbors[i]];
					if (tile2.biome == BiomeDefOf.Ocean)
					{
						Rot4 rotFromTo = this.grid.GetRotFromTo(tileID, World.tmpNeighbors[i]);
						if (!World.tmpOceanDirs.Contains(rotFromTo))
						{
							World.tmpOceanDirs.Add(rotFromTo);
						}
					}
					i++;
				}
				if (World.tmpOceanDirs.Count == 0)
				{
					result = Rot4.Invalid;
				}
				else
				{
					Rand.PushState();
					Rand.Seed = tileID;
					int index = Rand.Range(0, World.tmpOceanDirs.Count);
					Rand.PopState();
					result = World.tmpOceanDirs[index];
				}
			}
			return result;
		}

		public bool HasCaves(int tile)
		{
			Tile tile2 = this.grid[tile];
			float chance;
			if (tile2.hilliness >= Hilliness.Mountainous)
			{
				chance = 0.5f;
			}
			else
			{
				if (tile2.hilliness < Hilliness.LargeHills)
				{
					return false;
				}
				chance = 0.25f;
			}
			return Rand.ChanceSeeded(chance, Gen.HashCombineInt(Find.World.info.Seed, tile));
		}

		public IEnumerable<ThingDef> NaturalRockTypesIn(int tile)
		{
			Rand.PushState();
			Rand.Seed = tile;
			List<ThingDef> list = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.building.isNaturalRock && !d.building.isResourceRock && !d.IsSmoothed
			select d).ToList<ThingDef>();
			int num = Rand.RangeInclusive(2, 3);
			if (num > list.Count)
			{
				num = list.Count;
			}
			List<ThingDef> list2 = new List<ThingDef>();
			for (int i = 0; i < num; i++)
			{
				ThingDef item = list.RandomElement<ThingDef>();
				list.Remove(item);
				list2.Add(item);
			}
			Rand.PopState();
			return list2;
		}

		public bool Impassable(int tileID)
		{
			return !this.pathGrid.Passable(tileID);
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			List<WorldObject> allWorldObjects = this.worldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				IThingHolder thingHolder = allWorldObjects[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
				List<WorldObjectComp> allComps = allWorldObjects[i].AllComps;
				for (int j = 0; j < allComps.Count; j++)
				{
					IThingHolder thingHolder2 = allComps[j] as IThingHolder;
					if (thingHolder2 != null)
					{
						outChildren.Add(thingHolder2);
					}
				}
			}
			for (int k = 0; k < this.components.Count; k++)
			{
				IThingHolder thingHolder3 = this.components[k] as IThingHolder;
				if (thingHolder3 != null)
				{
					outChildren.Add(thingHolder3);
				}
			}
		}

		public string GetUniqueLoadID()
		{
			return "World";
		}

		public override string ToString()
		{
			return "(World-" + this.info.name + ")";
		}

		// Note: this type is marked as 'beforefieldinit'.
		static World()
		{
		}

		[CompilerGenerated]
		private static bool <FillComponents>m__0(WorldComponent component)
		{
			return component == null;
		}

		[CompilerGenerated]
		private static bool <NaturalRockTypesIn>m__1(ThingDef d)
		{
			return d.category == ThingCategory.Building && d.building.isNaturalRock && !d.building.isResourceRock && !d.IsSmoothed;
		}

		[CompilerGenerated]
		private sealed class <AcceptedTypes>c__Iterator0 : IEnumerable, IEnumerable<IncidentTargetTypeDef>, IEnumerator, IDisposable, IEnumerator<IncidentTargetTypeDef>
		{
			internal IncidentTargetTypeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AcceptedTypes>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = IncidentTargetTypeDefOf.World;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			IncidentTargetTypeDef IEnumerator<IncidentTargetTypeDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.IncidentTargetTypeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IncidentTargetTypeDef> IEnumerable<IncidentTargetTypeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new World.<AcceptedTypes>c__Iterator0();
			}
		}
	}
}
