using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Profiling;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x020005B1 RID: 1457
	public sealed class World : IThingHolder, IExposable, IIncidentTarget, ILoadReferenceable
	{
		// Token: 0x0400109C RID: 4252
		public WorldInfo info = new WorldInfo();

		// Token: 0x0400109D RID: 4253
		public List<WorldComponent> components = new List<WorldComponent>();

		// Token: 0x0400109E RID: 4254
		public FactionManager factionManager;

		// Token: 0x0400109F RID: 4255
		public WorldPawns worldPawns;

		// Token: 0x040010A0 RID: 4256
		public WorldObjectsHolder worldObjects;

		// Token: 0x040010A1 RID: 4257
		public GameConditionManager gameConditionManager;

		// Token: 0x040010A2 RID: 4258
		public StoryState storyState;

		// Token: 0x040010A3 RID: 4259
		public WorldFeatures features;

		// Token: 0x040010A4 RID: 4260
		public WorldGrid grid;

		// Token: 0x040010A5 RID: 4261
		public WorldPathGrid pathGrid;

		// Token: 0x040010A6 RID: 4262
		public WorldRenderer renderer;

		// Token: 0x040010A7 RID: 4263
		public WorldInterface UI;

		// Token: 0x040010A8 RID: 4264
		public WorldDebugDrawer debugDrawer;

		// Token: 0x040010A9 RID: 4265
		public WorldDynamicDrawManager dynamicDrawManager;

		// Token: 0x040010AA RID: 4266
		public WorldPathFinder pathFinder;

		// Token: 0x040010AB RID: 4267
		public WorldPathPool pathPool;

		// Token: 0x040010AC RID: 4268
		public WorldReachability reachability;

		// Token: 0x040010AD RID: 4269
		public WorldFloodFiller floodFiller;

		// Token: 0x040010AE RID: 4270
		public ConfiguredTicksAbsAtGameStartCache ticksAbsCache;

		// Token: 0x040010AF RID: 4271
		public TileTemperaturesComp tileTemperatures;

		// Token: 0x040010B0 RID: 4272
		public WorldGenData genData;

		// Token: 0x040010B1 RID: 4273
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x040010B2 RID: 4274
		private static List<Rot4> tmpOceanDirs = new List<Rot4>();

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x000F0724 File Offset: 0x000EEB24
		public float PlanetCoverage
		{
			get
			{
				return this.info.planetCoverage;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x000F0744 File Offset: 0x000EEB44
		public IThingHolder ParentHolder
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x000F075C File Offset: 0x000EEB5C
		public int Tile
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x000F0774 File Offset: 0x000EEB74
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x000F0790 File Offset: 0x000EEB90
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x000F07AC File Offset: 0x000EEBAC
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

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001BEA RID: 7146 RVA: 0x000F0830 File Offset: 0x000EEC30
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x000F084C File Offset: 0x000EEC4C
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000F0868 File Offset: 0x000EEC68
		public IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			yield return IncidentTargetTypeDefOf.World;
			yield break;
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000F088C File Offset: 0x000EEC8C
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

		// Token: 0x06001BEE RID: 7150 RVA: 0x000F0920 File Offset: 0x000EED20
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

		// Token: 0x06001BEF RID: 7151 RVA: 0x000F09EC File Offset: 0x000EEDEC
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

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000F0AA8 File Offset: 0x000EEEA8
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

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000F0B78 File Offset: 0x000EEF78
		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			AmbientSoundManager.EnsureWorldAmbientSoundCreated();
			WorldComponentUtility.FinalizeInit(this);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000F0B94 File Offset: 0x000EEF94
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

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000F0C3C File Offset: 0x000EF03C
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

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000F0C90 File Offset: 0x000EF090
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

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000F0D44 File Offset: 0x000EF144
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

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000F0DA8 File Offset: 0x000EF1A8
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

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000F0E0C File Offset: 0x000EF20C
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

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000F0F2C File Offset: 0x000EF32C
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

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000F0FA0 File Offset: 0x000EF3A0
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

		// Token: 0x06001BFA RID: 7162 RVA: 0x000F1044 File Offset: 0x000EF444
		public bool Impassable(int tileID)
		{
			return !this.pathGrid.Passable(tileID);
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000F1068 File Offset: 0x000EF468
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000F1080 File Offset: 0x000EF480
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

		// Token: 0x06001BFD RID: 7165 RVA: 0x000F1168 File Offset: 0x000EF568
		public string GetUniqueLoadID()
		{
			return "World";
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000F1184 File Offset: 0x000EF584
		public override string ToString()
		{
			return "(World-" + this.info.name + ")";
		}
	}
}
