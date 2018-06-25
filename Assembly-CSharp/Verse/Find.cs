using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F2D RID: 3885
	public static class Find
	{
		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005CF9 RID: 23801 RVA: 0x002F2560 File Offset: 0x002F0960
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06005CFA RID: 23802 RVA: 0x002F257C File Offset: 0x002F097C
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06005CFB RID: 23803 RVA: 0x002F259C File Offset: 0x002F099C
		public static UIRoot UIRoot
		{
			get
			{
				return Current.Root.uiRoot;
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06005CFC RID: 23804 RVA: 0x002F25BC File Offset: 0x002F09BC
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06005CFD RID: 23805 RVA: 0x002F25E0 File Offset: 0x002F09E0
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06005CFE RID: 23806 RVA: 0x002F2604 File Offset: 0x002F0A04
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06005CFF RID: 23807 RVA: 0x002F2624 File Offset: 0x002F0A24
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06005D00 RID: 23808 RVA: 0x002F2640 File Offset: 0x002F0A40
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06005D01 RID: 23809 RVA: 0x002F265C File Offset: 0x002F0A5C
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x002F2678 File Offset: 0x002F0A78
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.PortraitCamera;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005D03 RID: 23811 RVA: 0x002F2694 File Offset: 0x002F0A94
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.PortraitRenderer;
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06005D04 RID: 23812 RVA: 0x002F26B0 File Offset: 0x002F0AB0
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06005D05 RID: 23813 RVA: 0x002F26CC File Offset: 0x002F0ACC
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06005D06 RID: 23814 RVA: 0x002F26E8 File Offset: 0x002F0AE8
		public static WindowStack WindowStack
		{
			get
			{
				return Find.UIRoot.windows;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005D07 RID: 23815 RVA: 0x002F2708 File Offset: 0x002F0B08
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				return Find.UIRoot.screenshotMode;
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06005D08 RID: 23816 RVA: 0x002F2728 File Offset: 0x002F0B28
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06005D09 RID: 23817 RVA: 0x002F274C File Offset: 0x002F0B4C
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06005D0A RID: 23818 RVA: 0x002F276C File Offset: 0x002F0B6C
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06005D0B RID: 23819 RVA: 0x002F2790 File Offset: 0x002F0B90
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005D0C RID: 23820 RVA: 0x002F27B0 File Offset: 0x002F0BB0
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06005D0D RID: 23821 RVA: 0x002F27D0 File Offset: 0x002F0BD0
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06005D0E RID: 23822 RVA: 0x002F27F0 File Offset: 0x002F0BF0
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06005D0F RID: 23823 RVA: 0x002F2810 File Offset: 0x002F0C10
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06005D10 RID: 23824 RVA: 0x002F2830 File Offset: 0x002F0C30
		public static GameInitData GameInitData
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.InitData;
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06005D11 RID: 23825 RVA: 0x002F2860 File Offset: 0x002F0C60
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06005D12 RID: 23826 RVA: 0x002F2880 File Offset: 0x002F0C80
		public static Scenario Scenario
		{
			get
			{
				Scenario result;
				if (Current.Game != null && Current.Game.Scenario != null)
				{
					result = Current.Game.Scenario;
				}
				else if (ScenarioMaker.GeneratingScenario != null)
				{
					result = ScenarioMaker.GeneratingScenario;
				}
				else
				{
					if (Find.UIRoot != null)
					{
						Page_ScenarioEditor page_ScenarioEditor = Find.WindowStack.WindowOfType<Page_ScenarioEditor>();
						if (page_ScenarioEditor != null)
						{
							return page_ScenarioEditor.EditingScenario;
						}
					}
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06005D13 RID: 23827 RVA: 0x002F2900 File Offset: 0x002F0D00
		public static World World
		{
			get
			{
				return (Current.Game == null || Current.Game.World == null) ? Current.CreatingWorld : Current.Game.World;
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06005D14 RID: 23828 RVA: 0x002F2944 File Offset: 0x002F0D44
		public static List<Map> Maps
		{
			get
			{
				List<Map> result;
				if (Current.Game == null)
				{
					result = null;
				}
				else
				{
					result = Current.Game.Maps;
				}
				return result;
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06005D15 RID: 23829 RVA: 0x002F2974 File Offset: 0x002F0D74
		public static Map CurrentMap
		{
			get
			{
				Map result;
				if (Current.Game == null)
				{
					result = null;
				}
				else
				{
					result = Current.Game.CurrentMap;
				}
				return result;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06005D16 RID: 23830 RVA: 0x002F29A4 File Offset: 0x002F0DA4
		public static Map AnyPlayerHomeMap
		{
			get
			{
				return Current.Game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06005D17 RID: 23831 RVA: 0x002F29C4 File Offset: 0x002F0DC4
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06005D18 RID: 23832 RVA: 0x002F29E4 File Offset: 0x002F0DE4
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06005D19 RID: 23833 RVA: 0x002F2A04 File Offset: 0x002F0E04
		public static Storyteller Storyteller
		{
			get
			{
				Storyteller result;
				if (Current.Game == null)
				{
					result = null;
				}
				else
				{
					result = Current.Game.storyteller;
				}
				return result;
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06005D1A RID: 23834 RVA: 0x002F2A34 File Offset: 0x002F0E34
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06005D1B RID: 23835 RVA: 0x002F2A54 File Offset: 0x002F0E54
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06005D1C RID: 23836 RVA: 0x002F2A74 File Offset: 0x002F0E74
		public static Archive Archive
		{
			get
			{
				return (Find.History == null) ? null : Find.History.archive;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06005D1D RID: 23837 RVA: 0x002F2AA4 File Offset: 0x002F0EA4
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06005D1E RID: 23838 RVA: 0x002F2AC4 File Offset: 0x002F0EC4
		public static History History
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.history;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06005D1F RID: 23839 RVA: 0x002F2AF4 File Offset: 0x002F0EF4
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06005D20 RID: 23840 RVA: 0x002F2B14 File Offset: 0x002F0F14
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06005D21 RID: 23841 RVA: 0x002F2B34 File Offset: 0x002F0F34
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06005D22 RID: 23842 RVA: 0x002F2B54 File Offset: 0x002F0F54
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06005D23 RID: 23843 RVA: 0x002F2B74 File Offset: 0x002F0F74
		public static Tutor Tutor
		{
			get
			{
				Tutor result;
				if (Current.Game == null)
				{
					result = null;
				}
				else
				{
					result = Current.Game.tutor;
				}
				return result;
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06005D24 RID: 23844 RVA: 0x002F2BA4 File Offset: 0x002F0FA4
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06005D25 RID: 23845 RVA: 0x002F2BC8 File Offset: 0x002F0FC8
		public static ActiveLessonHandler ActiveLesson
		{
			get
			{
				ActiveLessonHandler result;
				if (Current.Game == null)
				{
					result = null;
				}
				else
				{
					result = Current.Game.tutor.activeLesson;
				}
				return result;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06005D26 RID: 23846 RVA: 0x002F2C00 File Offset: 0x002F1000
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06005D27 RID: 23847 RVA: 0x002F2C20 File Offset: 0x002F1020
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06005D28 RID: 23848 RVA: 0x002F2C40 File Offset: 0x002F1040
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06005D29 RID: 23849 RVA: 0x002F2C60 File Offset: 0x002F1060
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.uniqueIDsManager;
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06005D2A RID: 23850 RVA: 0x002F2C90 File Offset: 0x002F1090
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06005D2B RID: 23851 RVA: 0x002F2CB0 File Offset: 0x002F10B0
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06005D2C RID: 23852 RVA: 0x002F2CD0 File Offset: 0x002F10D0
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06005D2D RID: 23853 RVA: 0x002F2CF0 File Offset: 0x002F10F0
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06005D2E RID: 23854 RVA: 0x002F2D10 File Offset: 0x002F1110
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06005D2F RID: 23855 RVA: 0x002F2D30 File Offset: 0x002F1130
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06005D30 RID: 23856 RVA: 0x002F2D50 File Offset: 0x002F1150
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06005D31 RID: 23857 RVA: 0x002F2D70 File Offset: 0x002F1170
		public static WorldPathFinder WorldPathFinder
		{
			get
			{
				return Find.World.pathFinder;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06005D32 RID: 23858 RVA: 0x002F2D90 File Offset: 0x002F1190
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06005D33 RID: 23859 RVA: 0x002F2DB0 File Offset: 0x002F11B0
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06005D34 RID: 23860 RVA: 0x002F2DD0 File Offset: 0x002F11D0
		public static WorldFloodFiller WorldFloodFiller
		{
			get
			{
				return Find.World.floodFiller;
			}
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06005D35 RID: 23861 RVA: 0x002F2DF0 File Offset: 0x002F11F0
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06005D36 RID: 23862 RVA: 0x002F2E10 File Offset: 0x002F1210
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06005D37 RID: 23863 RVA: 0x002F2E30 File Offset: 0x002F1230
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06005D38 RID: 23864 RVA: 0x002F2E50 File Offset: 0x002F1250
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06005D39 RID: 23865 RVA: 0x002F2E70 File Offset: 0x002F1270
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}
	}
}
