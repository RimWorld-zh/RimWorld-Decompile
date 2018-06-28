using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse.Profile;

namespace Verse
{
	public class Game : IExposable
	{
		private GameInitData initData;

		public sbyte currentMapIndex = -1;

		private GameInfo info = new GameInfo();

		public List<GameComponent> components = new List<GameComponent>();

		private GameRules rules = new GameRules();

		private Scenario scenarioInt;

		private World worldInt;

		private List<Map> maps = new List<Map>();

		public PlaySettings playSettings = new PlaySettings();

		public StoryWatcher storyWatcher = new StoryWatcher();

		public LetterStack letterStack = new LetterStack();

		public ResearchManager researchManager = new ResearchManager();

		public GameEnder gameEnder = new GameEnder();

		public Storyteller storyteller = new Storyteller();

		public History history = new History();

		public TaleManager taleManager = new TaleManager();

		public PlayLog playLog = new PlayLog();

		public BattleLog battleLog = new BattleLog();

		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		public TickManager tickManager = new TickManager();

		public Tutor tutor = new Tutor();

		public Autosaver autosaver = new Autosaver();

		public DateNotifier dateNotifier = new DateNotifier();

		public SignalManager signalManager = new SignalManager();

		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		[CompilerGenerated]
		private static Predicate<GameComponent> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ModContentPack, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Action <>f__am$cache2;

		public Game()
		{
			this.FillComponents();
		}

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

		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

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

		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

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
					Settlement settlement = null;
					List<Settlement> settlements = Find.WorldObjects.Settlements;
					for (int i = 0; i < settlements.Count; i++)
					{
						if (settlements[i].Faction == Faction.OfPlayer)
						{
							settlement = settlements[i];
							break;
						}
					}
					if (settlement == null)
					{
						Log.Error("Could not generate starting map because there is no any player faction base.", false);
					}
					this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
					Map currentMap = MapGenerator.GenerateMap(intVec, settlement, settlement.MapGeneratorDef, settlement.ExtraGenStepDefs, null);
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

		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

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

		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
		}

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

		[CompilerGenerated]
		private static bool <FillComponents>m__0(GameComponent component)
		{
			return component == null;
		}

		[CompilerGenerated]
		private static string <InitNewGame>m__1(ModContentPack mod)
		{
			return mod.ToString();
		}

		[CompilerGenerated]
		private void <InitNewGame>m__2()
		{
			this.tickManager.DoSingleTick();
			this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
		}

		[CompilerGenerated]
		private static void <LoadGame>m__3()
		{
			Find.TickManager.DoSingleTick();
			Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
		}
	}
}
