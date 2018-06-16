using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F29 RID: 3881
	public static class Find
	{
		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06005CC9 RID: 23753 RVA: 0x002EFBB8 File Offset: 0x002EDFB8
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06005CCA RID: 23754 RVA: 0x002EFBD4 File Offset: 0x002EDFD4
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005CCB RID: 23755 RVA: 0x002EFBF4 File Offset: 0x002EDFF4
		public static UIRoot UIRoot
		{
			get
			{
				return Current.Root.uiRoot;
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06005CCC RID: 23756 RVA: 0x002EFC14 File Offset: 0x002EE014
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06005CCD RID: 23757 RVA: 0x002EFC38 File Offset: 0x002EE038
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06005CCE RID: 23758 RVA: 0x002EFC5C File Offset: 0x002EE05C
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06005CCF RID: 23759 RVA: 0x002EFC7C File Offset: 0x002EE07C
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06005CD0 RID: 23760 RVA: 0x002EFC98 File Offset: 0x002EE098
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x002EFCB4 File Offset: 0x002EE0B4
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06005CD2 RID: 23762 RVA: 0x002EFCD0 File Offset: 0x002EE0D0
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.PortraitCamera;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06005CD3 RID: 23763 RVA: 0x002EFCEC File Offset: 0x002EE0EC
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.PortraitRenderer;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005CD4 RID: 23764 RVA: 0x002EFD08 File Offset: 0x002EE108
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005CD5 RID: 23765 RVA: 0x002EFD24 File Offset: 0x002EE124
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06005CD6 RID: 23766 RVA: 0x002EFD40 File Offset: 0x002EE140
		public static WindowStack WindowStack
		{
			get
			{
				return Find.UIRoot.windows;
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06005CD7 RID: 23767 RVA: 0x002EFD60 File Offset: 0x002EE160
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				return Find.UIRoot.screenshotMode;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06005CD8 RID: 23768 RVA: 0x002EFD80 File Offset: 0x002EE180
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005CD9 RID: 23769 RVA: 0x002EFDA4 File Offset: 0x002EE1A4
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06005CDA RID: 23770 RVA: 0x002EFDC4 File Offset: 0x002EE1C4
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06005CDB RID: 23771 RVA: 0x002EFDE8 File Offset: 0x002EE1E8
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06005CDC RID: 23772 RVA: 0x002EFE08 File Offset: 0x002EE208
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06005CDD RID: 23773 RVA: 0x002EFE28 File Offset: 0x002EE228
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005CDE RID: 23774 RVA: 0x002EFE48 File Offset: 0x002EE248
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06005CDF RID: 23775 RVA: 0x002EFE68 File Offset: 0x002EE268
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06005CE0 RID: 23776 RVA: 0x002EFE88 File Offset: 0x002EE288
		public static GameInitData GameInitData
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.InitData;
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06005CE1 RID: 23777 RVA: 0x002EFEB8 File Offset: 0x002EE2B8
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06005CE2 RID: 23778 RVA: 0x002EFED8 File Offset: 0x002EE2D8
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

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06005CE3 RID: 23779 RVA: 0x002EFF58 File Offset: 0x002EE358
		public static World World
		{
			get
			{
				return (Current.Game == null || Current.Game.World == null) ? Current.CreatingWorld : Current.Game.World;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06005CE4 RID: 23780 RVA: 0x002EFF9C File Offset: 0x002EE39C
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

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06005CE5 RID: 23781 RVA: 0x002EFFCC File Offset: 0x002EE3CC
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

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06005CE6 RID: 23782 RVA: 0x002EFFFC File Offset: 0x002EE3FC
		public static Map AnyPlayerHomeMap
		{
			get
			{
				return Current.Game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06005CE7 RID: 23783 RVA: 0x002F001C File Offset: 0x002EE41C
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06005CE8 RID: 23784 RVA: 0x002F003C File Offset: 0x002EE43C
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06005CE9 RID: 23785 RVA: 0x002F005C File Offset: 0x002EE45C
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

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06005CEA RID: 23786 RVA: 0x002F008C File Offset: 0x002EE48C
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06005CEB RID: 23787 RVA: 0x002F00AC File Offset: 0x002EE4AC
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06005CEC RID: 23788 RVA: 0x002F00CC File Offset: 0x002EE4CC
		public static Archive Archive
		{
			get
			{
				return (Find.History == null) ? null : Find.History.archive;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06005CED RID: 23789 RVA: 0x002F00FC File Offset: 0x002EE4FC
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06005CEE RID: 23790 RVA: 0x002F011C File Offset: 0x002EE51C
		public static History History
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.history;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06005CEF RID: 23791 RVA: 0x002F014C File Offset: 0x002EE54C
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06005CF0 RID: 23792 RVA: 0x002F016C File Offset: 0x002EE56C
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06005CF1 RID: 23793 RVA: 0x002F018C File Offset: 0x002EE58C
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06005CF2 RID: 23794 RVA: 0x002F01AC File Offset: 0x002EE5AC
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06005CF3 RID: 23795 RVA: 0x002F01CC File Offset: 0x002EE5CC
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

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06005CF4 RID: 23796 RVA: 0x002F01FC File Offset: 0x002EE5FC
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06005CF5 RID: 23797 RVA: 0x002F0220 File Offset: 0x002EE620
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

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06005CF6 RID: 23798 RVA: 0x002F0258 File Offset: 0x002EE658
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06005CF7 RID: 23799 RVA: 0x002F0278 File Offset: 0x002EE678
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06005CF8 RID: 23800 RVA: 0x002F0298 File Offset: 0x002EE698
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06005CF9 RID: 23801 RVA: 0x002F02B8 File Offset: 0x002EE6B8
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				return (Current.Game == null) ? null : Current.Game.uniqueIDsManager;
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06005CFA RID: 23802 RVA: 0x002F02E8 File Offset: 0x002EE6E8
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06005CFB RID: 23803 RVA: 0x002F0308 File Offset: 0x002EE708
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06005CFC RID: 23804 RVA: 0x002F0328 File Offset: 0x002EE728
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06005CFD RID: 23805 RVA: 0x002F0348 File Offset: 0x002EE748
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06005CFE RID: 23806 RVA: 0x002F0368 File Offset: 0x002EE768
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06005CFF RID: 23807 RVA: 0x002F0388 File Offset: 0x002EE788
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06005D00 RID: 23808 RVA: 0x002F03A8 File Offset: 0x002EE7A8
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06005D01 RID: 23809 RVA: 0x002F03C8 File Offset: 0x002EE7C8
		public static WorldPathFinder WorldPathFinder
		{
			get
			{
				return Find.World.pathFinder;
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x002F03E8 File Offset: 0x002EE7E8
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06005D03 RID: 23811 RVA: 0x002F0408 File Offset: 0x002EE808
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06005D04 RID: 23812 RVA: 0x002F0428 File Offset: 0x002EE828
		public static WorldFloodFiller WorldFloodFiller
		{
			get
			{
				return Find.World.floodFiller;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06005D05 RID: 23813 RVA: 0x002F0448 File Offset: 0x002EE848
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06005D06 RID: 23814 RVA: 0x002F0468 File Offset: 0x002EE868
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06005D07 RID: 23815 RVA: 0x002F0488 File Offset: 0x002EE888
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06005D08 RID: 23816 RVA: 0x002F04A8 File Offset: 0x002EE8A8
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06005D09 RID: 23817 RVA: 0x002F04C8 File Offset: 0x002EE8C8
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}
	}
}
