#define ENABLE_PROFILER
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Profiling;
using Verse.Profile;

namespace Verse
{
	public class Game : IExposable
	{
		private GameInitData initData;

		public sbyte visibleMapIndex = (sbyte)(-1);

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

		public Map VisibleMap
		{
			get
			{
				return (this.visibleMapIndex >= 0) ? this.maps[this.visibleMapIndex] : null;
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
						Log.Error("Could not set visible map because it does not exist.");
						return;
					}
				}
				if (this.visibleMapIndex != num)
				{
					this.visibleMapIndex = (sbyte)num;
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
				Map map;
				if (Faction.OfPlayerSilentFail == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < this.maps.Count; i++)
					{
						map = this.maps[i];
						if (map.IsPlayerHome)
							goto IL_0032;
					}
					result = null;
				}
				goto IL_0056;
				IL_0032:
				result = map;
				goto IL_0056;
				IL_0056:
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

		public Game()
		{
			this.FillComponents();
		}

		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.");
			}
			else if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.");
			}
			else if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + (sbyte)127 + ").");
			}
			else
			{
				this.maps.Add(map);
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		public Map FindMap(MapParent mapParent)
		{
			int num = 0;
			Map result;
			while (true)
			{
				if (num < this.maps.Count)
				{
					if (this.maps[num].info.parent == mapParent)
					{
						result = this.maps[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public Map FindMap(int tile)
		{
			int num = 0;
			Map result;
			while (true)
			{
				if (num < this.maps.Count)
				{
					if (this.maps[num].Tile == tile)
					{
						result = this.maps[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.");
			}
			else
			{
				Scribe_Values.Look<sbyte>(ref this.visibleMapIndex, "visibleMapIndex", (sbyte)(-1), false);
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
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[1]
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
			this.components.RemoveAll((Predicate<GameComponent>)((GameComponent component) => component == null));
			foreach (Type item2 in typeof(GameComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(item2) == null)
				{
					GameComponent item = (GameComponent)Activator.CreateInstance(item2, this);
					this.components.Add(item);
				}
			}
		}

		public void InitNewGame()
		{
			string str = GenText.ToCommaList(from mod in LoadedModManager.RunningMods
			select mod.ToString(), true);
			Log.Message("Initializing new game with mods " + str);
			if (this.maps.Any())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...");
			}
			else if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.");
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
					int num = 0;
					while (num < factionBases.Count)
					{
						if (factionBases[num].Faction != Faction.OfPlayer)
						{
							num++;
							continue;
						}
						factionBase = factionBases[num];
						break;
					}
					if (factionBase == null)
					{
						Log.Error("Could not generate starting map because there is no any player faction base.");
					}
					this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
					Map visibleMap = MapGenerator.GenerateMap(intVec, factionBase, factionBase.MapGeneratorDef, factionBase.ExtraGenStepDefs, null);
					this.worldInt.info.initialMapSize = intVec;
					if (this.initData.permadeath)
					{
						this.info.permadeathMode = true;
						this.info.permadeathModeUniqueName = PermadeathModeUtility.GeneratePermadeathSaveName();
					}
					PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.NewColonyOptimism);
					this.FinalizeInit();
					Current.Game.VisibleMap = visibleMap;
					Find.CameraDriver.JumpToVisibleMapLoc(MapGenerator.PlayerStartSpot);
					Find.CameraDriver.ResetSize();
					if (Prefs.PauseOnLoad && this.initData.startedFromEntry)
					{
						LongEventHandler.ExecuteWhenFinished((Action)delegate
						{
							this.tickManager.DoSingleTick();
							this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
						});
					}
					Find.Scenario.PostGameStart();
					if (Faction.OfPlayer.def.startingResearchTags != null)
					{
						foreach (string startingResearchTag in Faction.OfPlayer.def.startingResearchTags)
						{
							foreach (ResearchProjectDef allDef in DefDatabase<ResearchProjectDef>.AllDefs)
							{
								if (allDef.HasTag(startingResearchTag))
								{
									this.researchManager.InstantFinish(allDef, false);
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
			if (this.maps.Any())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...");
			}
			else
			{
				MemoryUtility.UnloadUnusedUnityAssets();
				Current.ProgramState = ProgramState.MapInitializing;
				this.ExposeSmallComponents();
				BackCompatibility.AfterLoadingSmallGameClassComponents(this);
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
					Scribe_Values.Look(ref num, "visibleMapIndex", -1, false);
					if (num < 0 && this.maps.Any())
					{
						Log.Error("Visible map is null after loading but there are maps available. Setting visible map to [0].");
						num = 0;
					}
					if (num >= this.maps.Count)
					{
						Log.Error("Visible map index out of bounds after loading.");
						num = ((!this.maps.Any()) ? (-1) : 0);
					}
					this.visibleMapIndex = (sbyte)(-128);
					this.VisibleMap = ((num < 0) ? null : this.maps[num]);
					LongEventHandler.SetCurrentEventText("InitializingGame".Translate());
					Find.CameraDriver.Expose();
					DeepProfiler.Start("FinalizeLoading");
					Scribe.loader.FinalizeLoading();
					DeepProfiler.End();
					LongEventHandler.SetCurrentEventText("SpawningAllThings".Translate());
					for (int i = 0; i < this.maps.Count; i++)
					{
						this.maps[i].FinalizeLoading();
					}
					this.FinalizeInit();
					if (Prefs.PauseOnLoad)
					{
						LongEventHandler.ExecuteWhenFinished((Action)delegate
						{
							Find.TickManager.DoSingleTick();
							Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
						});
					}
					GameComponentUtility.LoadedGame();
				}
				else
				{
					Log.Error("Could not find world XML node.");
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
			int num = 0;
			T result;
			while (true)
			{
				if (num < this.components.Count)
				{
					T val = (T)(this.components[num] as T);
					if (val != null)
					{
						result = val;
						break;
					}
					num++;
					continue;
				}
				result = (T)null;
				break;
			}
			return result;
		}

		public GameComponent GetComponent(Type type)
		{
			int num = 0;
			GameComponent result;
			while (true)
			{
				if (num < this.components.Count)
				{
					if (type.IsAssignableFrom(this.components[num].GetType()))
					{
						result = this.components[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
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
				Log.Error("Tried to remove null map.");
			}
			else if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + map + " but it's not here.");
			}
			else
			{
				Map visibleMap = this.VisibleMap;
				MapDeiniter.Deinit(map);
				this.maps.Remove(map);
				if (visibleMap != null)
				{
					sbyte b = (sbyte)this.maps.IndexOf(visibleMap);
					if (b < 0)
					{
						if (this.maps.Any())
						{
							this.VisibleMap = this.maps[0];
						}
						else
						{
							this.VisibleMap = null;
						}
						Find.World.renderer.wantedMode = WorldRenderMode.Planet;
					}
					else
					{
						this.visibleMapIndex = b;
					}
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				MapComponentUtility.MapRemoved(map);
				if (map.info.parent != null)
				{
					map.info.parent.Notify_MyMapRemoved(map);
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
	}
}
