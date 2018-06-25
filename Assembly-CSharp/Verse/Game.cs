using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000BC3 RID: 3011
	public class Game : IExposable
	{
		// Token: 0x04002CB7 RID: 11447
		private GameInitData initData;

		// Token: 0x04002CB8 RID: 11448
		public sbyte currentMapIndex = -1;

		// Token: 0x04002CB9 RID: 11449
		private GameInfo info = new GameInfo();

		// Token: 0x04002CBA RID: 11450
		public List<GameComponent> components = new List<GameComponent>();

		// Token: 0x04002CBB RID: 11451
		private GameRules rules = new GameRules();

		// Token: 0x04002CBC RID: 11452
		private Scenario scenarioInt;

		// Token: 0x04002CBD RID: 11453
		private World worldInt;

		// Token: 0x04002CBE RID: 11454
		private List<Map> maps = new List<Map>();

		// Token: 0x04002CBF RID: 11455
		public PlaySettings playSettings = new PlaySettings();

		// Token: 0x04002CC0 RID: 11456
		public StoryWatcher storyWatcher = new StoryWatcher();

		// Token: 0x04002CC1 RID: 11457
		public LetterStack letterStack = new LetterStack();

		// Token: 0x04002CC2 RID: 11458
		public ResearchManager researchManager = new ResearchManager();

		// Token: 0x04002CC3 RID: 11459
		public GameEnder gameEnder = new GameEnder();

		// Token: 0x04002CC4 RID: 11460
		public Storyteller storyteller = new Storyteller();

		// Token: 0x04002CC5 RID: 11461
		public History history = new History();

		// Token: 0x04002CC6 RID: 11462
		public TaleManager taleManager = new TaleManager();

		// Token: 0x04002CC7 RID: 11463
		public PlayLog playLog = new PlayLog();

		// Token: 0x04002CC8 RID: 11464
		public BattleLog battleLog = new BattleLog();

		// Token: 0x04002CC9 RID: 11465
		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		// Token: 0x04002CCA RID: 11466
		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		// Token: 0x04002CCB RID: 11467
		public TickManager tickManager = new TickManager();

		// Token: 0x04002CCC RID: 11468
		public Tutor tutor = new Tutor();

		// Token: 0x04002CCD RID: 11469
		public Autosaver autosaver = new Autosaver();

		// Token: 0x04002CCE RID: 11470
		public DateNotifier dateNotifier = new DateNotifier();

		// Token: 0x04002CCF RID: 11471
		public SignalManager signalManager = new SignalManager();

		// Token: 0x04002CD0 RID: 11472
		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		// Token: 0x06004179 RID: 16761 RVA: 0x00228FC8 File Offset: 0x002273C8
		public Game()
		{
			this.FillComponents();
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x0600417A RID: 16762 RVA: 0x002290DC File Offset: 0x002274DC
		// (set) Token: 0x0600417B RID: 16763 RVA: 0x002290F7 File Offset: 0x002274F7
		public Scenario Scenario
		{
			get
			{
				return this.scenarioInt;
			}
			set
			{
				this.scenarioInt = value;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x0600417C RID: 16764 RVA: 0x00229104 File Offset: 0x00227504
		// (set) Token: 0x0600417D RID: 16765 RVA: 0x0022911F File Offset: 0x0022751F
		public World World
		{
			get
			{
				return this.worldInt;
			}
			set
			{
				if (this.worldInt != value)
				{
					this.worldInt = value;
				}
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x0600417E RID: 16766 RVA: 0x0022913C File Offset: 0x0022753C
		// (set) Token: 0x0600417F RID: 16767 RVA: 0x00229178 File Offset: 0x00227578
		public Map CurrentMap
		{
			get
			{
				Map result;
				if ((int)this.currentMapIndex < 0)
				{
					result = null;
				}
				else
				{
					result = this.maps[(int)this.currentMapIndex];
				}
				return result;
			}
			set
			{
				int num;
				if (value == null)
				{
					num = -1;
				}
				else
				{
					num = this.maps.IndexOf(value);
					if (num < 0)
					{
						Log.Error("Could not set current map because it does not exist.", false);
						return;
					}
				}
				if ((int)this.currentMapIndex != num)
				{
					this.currentMapIndex = (sbyte)num;
					Find.MapUI.Notify_SwitchedMap();
					AmbientSoundManager.Notify_SwitchedMap();
				}
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06004180 RID: 16768 RVA: 0x002291E0 File Offset: 0x002275E0
		public Map AnyPlayerHomeMap
		{
			get
			{
				Map result;
				if (Faction.OfPlayerSilentFail == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < this.maps.Count; i++)
					{
						Map map = this.maps[i];
						if (map.IsPlayerHome)
						{
							return map;
						}
					}
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06004181 RID: 16769 RVA: 0x00229244 File Offset: 0x00227644
		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06004182 RID: 16770 RVA: 0x00229260 File Offset: 0x00227660
		// (set) Token: 0x06004183 RID: 16771 RVA: 0x0022927B File Offset: 0x0022767B
		public GameInitData InitData
		{
			get
			{
				return this.initData;
			}
			set
			{
				this.initData = value;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06004184 RID: 16772 RVA: 0x00229288 File Offset: 0x00227688
		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06004185 RID: 16773 RVA: 0x002292A4 File Offset: 0x002276A4
		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x002292C0 File Offset: 0x002276C0
		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.", false);
			}
			else if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.", false);
			}
			else if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + sbyte.MaxValue + ").", false);
			}
			else
			{
				this.maps.Add(map);
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x00229354 File Offset: 0x00227754
		public Map FindMap(MapParent mapParent)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].info.parent == mapParent)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x002293B8 File Offset: 0x002277B8
		public Map FindMap(int tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].Tile == tile)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x00229418 File Offset: 0x00227818
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.", false);
			}
			else
			{
				Scribe_Values.Look<sbyte>(ref this.currentMapIndex, "currentMapIndex", -1, false);
				this.ExposeSmallComponents();
				Scribe_Deep.Look<World>(ref this.worldInt, "world", new object[0]);
				Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, new object[0]);
				Find.CameraDriver.Expose();
			}
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x00229494 File Offset: 0x00227894
		private void ExposeSmallComponents()
		{
			Scribe_Deep.Look<GameInfo>(ref this.info, "info", new object[0]);
			Scribe_Deep.Look<GameRules>(ref this.rules, "rules", new object[0]);
			Scribe_Deep.Look<Scenario>(ref this.scenarioInt, "scenario", new object[0]);
			Scribe_Deep.Look<TickManager>(ref this.tickManager, "tickManager", new object[0]);
			Scribe_Deep.Look<PlaySettings>(ref this.playSettings, "playSettings", new object[0]);
			Scribe_Deep.Look<StoryWatcher>(ref this.storyWatcher, "storyWatcher", new object[0]);
			Scribe_Deep.Look<GameEnder>(ref this.gameEnder, "gameEnder", new object[0]);
			Scribe_Deep.Look<LetterStack>(ref this.letterStack, "letterStack", new object[0]);
			Scribe_Deep.Look<ResearchManager>(ref this.researchManager, "researchManager", new object[0]);
			Scribe_Deep.Look<Storyteller>(ref this.storyteller, "storyteller", new object[0]);
			Scribe_Deep.Look<History>(ref this.history, "history", new object[0]);
			Scribe_Deep.Look<TaleManager>(ref this.taleManager, "taleManager", new object[0]);
			Scribe_Deep.Look<PlayLog>(ref this.playLog, "playLog", new object[0]);
			Scribe_Deep.Look<BattleLog>(ref this.battleLog, "battleLog", new object[0]);
			Scribe_Deep.Look<OutfitDatabase>(ref this.outfitDatabase, "outfitDatabase", new object[0]);
			Scribe_Deep.Look<DrugPolicyDatabase>(ref this.drugPolicyDatabase, "drugPolicyDatabase", new object[0]);
			Scribe_Deep.Look<Tutor>(ref this.tutor, "tutor", new object[0]);
			Scribe_Deep.Look<DateNotifier>(ref this.dateNotifier, "dateNotifier", new object[0]);
			Scribe_Deep.Look<UniqueIDsManager>(ref this.uniqueIDsManager, "uniqueIDsManager", new object[0]);
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.FillComponents();
				BackCompatibility.GameLoadingVars(this);
			}
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x00229678 File Offset: 0x00227A78
		private void FillComponents()
		{
			this.components.RemoveAll((GameComponent component) => component == null);
			foreach (Type type in typeof(GameComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					GameComponent item = (GameComponent)Activator.CreateInstance(type, new object[]
					{
						this
					});
					this.components.Add(item);
				}
			}
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x00229730 File Offset: 0x00227B30
		public void InitNewGame()
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.ToString()).ToCommaList(false);
			Log.Message("Initializing new game with mods " + str, false);
			if (this.maps.Any<Map>())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...", false);
			}
			else if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.", false);
			}
			else
			{
				MemoryUtility.UnloadUnusedUnityAssets();
				DeepProfiler.Start("InitNewGame");
				try
				{
					Current.ProgramState = ProgramState.MapInitializing;
					IntVec3 intVec = new IntVec3(this.initData.mapSize, 1, this.initData.mapSize);
					FactionBase factionBase = null;
					List<FactionBase> factionBases = Find.WorldObjects.FactionBases;
					for (int i = 0; i < factionBases.Count; i++)
					{
						if (factionBases[i].Faction == Faction.OfPlayer)
						{
							factionBase = factionBases[i];
							break;
						}
					}
					if (factionBase == null)
					{
						Log.Error("Could not generate starting map because there is no any player faction base.", false);
					}
					this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
					Map currentMap = MapGenerator.GenerateMap(intVec, factionBase, factionBase.MapGeneratorDef, factionBase.ExtraGenStepDefs, null);
					this.worldInt.info.initialMapSize = intVec;
					if (this.initData.permadeath)
					{
						this.info.permadeathMode = true;
						this.info.permadeathModeUniqueName = PermadeathModeUtility.GeneratePermadeathSaveName();
					}
					PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.NewColonyOptimism);
					this.FinalizeInit();
					Current.Game.CurrentMap = currentMap;
					Find.CameraDriver.JumpToCurrentMapLoc(MapGenerator.PlayerStartSpot);
					Find.CameraDriver.ResetSize();
					if (Prefs.PauseOnLoad && this.initData.startedFromEntry)
					{
						LongEventHandler.ExecuteWhenFinished(delegate
						{
							this.tickManager.DoSingleTick();
							this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
						});
					}
					Find.Scenario.PostGameStart();
					if (Faction.OfPlayer.def.startingResearchTags != null)
					{
						foreach (ResearchProjectTagDef tag in Faction.OfPlayer.def.startingResearchTags)
						{
							foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
							{
								if (researchProjectDef.HasTag(tag))
								{
									this.researchManager.InstantFinish(researchProjectDef, false);
								}
							}
						}
					}
					GameComponentUtility.StartedNewGame();
					this.initData = null;
				}
				finally
				{
					DeepProfiler.End();
				}
			}
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x00229A3C File Offset: 0x00227E3C
		public void LoadGame()
		{
			if (this.maps.Any<Map>())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...", false);
			}
			else
			{
				MemoryUtility.UnloadUnusedUnityAssets();
				Current.ProgramState = ProgramState.MapInitializing;
				this.ExposeSmallComponents();
				LongEventHandler.SetCurrentEventText("LoadingWorld".Translate());
				if (Scribe.EnterNode("world"))
				{
					try
					{
						this.World = new World();
						this.World.ExposeData();
					}
					finally
					{
						Scribe.ExitNode();
					}
					this.World.FinalizeInit();
					LongEventHandler.SetCurrentEventText("LoadingMap".Translate());
					Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, new object[0]);
					int num = -1;
					Scribe_Values.Look<int>(ref num, "currentMapIndex", -1, false);
					if (num < 0 && this.maps.Any<Map>())
					{
						Log.Error("Current map is null after loading but there are maps available. Setting current map to [0].", false);
						num = 0;
					}
					if (num >= this.maps.Count)
					{
						Log.Error("Current map index out of bounds after loading.", false);
						if (this.maps.Any<Map>())
						{
							num = 0;
						}
						else
						{
							num = -1;
						}
					}
					this.currentMapIndex = sbyte.MinValue;
					this.CurrentMap = ((num < 0) ? null : this.maps[num]);
					LongEventHandler.SetCurrentEventText("InitializingGame".Translate());
					Find.CameraDriver.Expose();
					DeepProfiler.Start("FinalizeLoading");
					Scribe.loader.FinalizeLoading();
					DeepProfiler.End();
					LongEventHandler.SetCurrentEventText("SpawningAllThings".Translate());
					for (int i = 0; i < this.maps.Count; i++)
					{
						this.maps[i].FinalizeLoading();
						this.maps[i].Parent.FinalizeLoading();
					}
					this.FinalizeInit();
					if (Prefs.PauseOnLoad)
					{
						LongEventHandler.ExecuteWhenFinished(delegate
						{
							Find.TickManager.DoSingleTick();
							Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
						});
					}
					GameComponentUtility.LoadedGame();
				}
				else
				{
					Log.Error("Could not find world XML node.", false);
				}
			}
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x00229C70 File Offset: 0x00228070
		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x00229C78 File Offset: 0x00228078
		public void UpdatePlay()
		{
			Profiler.BeginSample("tickManager.TickManagerUpdate()");
			this.tickManager.TickManagerUpdate();
			Profiler.EndSample();
			Profiler.BeginSample("letterStack.LetterStackUpdate()");
			this.letterStack.LetterStackUpdate();
			Profiler.EndSample();
			Profiler.BeginSample("World.WorldUpdate()");
			this.World.WorldUpdate();
			Profiler.EndSample();
			Profiler.BeginSample("Map.MapUpdate()");
			for (int i = 0; i < this.maps.Count; i++)
			{
				Profiler.BeginSample("Map " + i);
				this.maps[i].MapUpdate();
				Profiler.EndSample();
			}
			Profiler.EndSample();
			Profiler.BeginSample("GameInfoUpdate()");
			this.Info.GameInfoUpdate();
			Profiler.EndSample();
			Profiler.BeginSample("GameComponentUpdate()");
			GameComponentUtility.GameComponentUpdate();
			Profiler.EndSample();
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x00229D5C File Offset: 0x0022815C
		public T GetComponent<T>() where T : GameComponent
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

		// Token: 0x06004191 RID: 16785 RVA: 0x00229DC0 File Offset: 0x002281C0
		public GameComponent GetComponent(Type type)
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

		// Token: 0x06004192 RID: 16786 RVA: 0x00229E22 File Offset: 0x00228222
		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x00229E48 File Offset: 0x00228248
		public void DeinitAndRemoveMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to remove null map.", false);
			}
			else if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + map + " but it's not here.", false);
			}
			else
			{
				Map currentMap = this.CurrentMap;
				MapDeiniter.Deinit(map);
				this.maps.Remove(map);
				if (currentMap != null)
				{
					sbyte b = (sbyte)this.maps.IndexOf(currentMap);
					if ((int)b < 0)
					{
						if (this.maps.Any<Map>())
						{
							this.CurrentMap = this.maps[0];
						}
						else
						{
							this.CurrentMap = null;
						}
						Find.World.renderer.wantedMode = WorldRenderMode.Planet;
					}
					else
					{
						this.currentMapIndex = b;
					}
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				MapComponentUtility.MapRemoved(map);
				if (map.Parent != null)
				{
					map.Parent.Notify_MyMapRemoved(map);
				}
			}
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x00229F50 File Offset: 0x00228350
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Game debug data:");
			stringBuilder.AppendLine("initData:");
			if (this.initData == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine(this.initData.ToString());
			}
			stringBuilder.AppendLine("Scenario:");
			if (this.scenarioInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   " + this.scenarioInt.ToString());
			}
			stringBuilder.AppendLine("World:");
			if (this.worldInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   name: " + this.worldInt.info.name);
			}
			stringBuilder.AppendLine("Maps count: " + this.maps.Count);
			for (int i = 0; i < this.maps.Count; i++)
			{
				stringBuilder.AppendLine("   Map " + this.maps[i].Index + ":");
				stringBuilder.AppendLine("      tile: " + this.maps[i].TileInfo);
			}
			return stringBuilder.ToString();
		}
	}
}
