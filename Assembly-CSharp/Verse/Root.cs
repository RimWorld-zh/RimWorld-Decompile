using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.Profile;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000BDD RID: 3037
	public abstract class Root : MonoBehaviour
	{
		// Token: 0x04002D56 RID: 11606
		private static bool globalInitDone;

		// Token: 0x04002D57 RID: 11607
		private static bool prefsApplied;

		// Token: 0x04002D58 RID: 11608
		protected static bool checkedAutostartSaveFile;

		// Token: 0x04002D59 RID: 11609
		protected bool destroyed;

		// Token: 0x04002D5A RID: 11610
		public SoundRoot soundRoot;

		// Token: 0x04002D5B RID: 11611
		public UIRoot uiRoot;

		// Token: 0x04002D5D RID: 11613
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		// Token: 0x0600424A RID: 16970 RVA: 0x0022E478 File Offset: 0x0022C878
		public virtual void Start()
		{
			try
			{
				CultureInfoUtility.EnsureEnglish();
				Current.Notify_LoadedSceneChanged();
				Root.CheckGlobalInit();
				Action action = delegate()
				{
					this.soundRoot = new SoundRoot();
					if (GenScene.InPlayScene)
					{
						this.uiRoot = new UIRoot_Play();
					}
					else if (GenScene.InEntryScene)
					{
						this.uiRoot = new UIRoot_Entry();
					}
					this.uiRoot.Init();
					Messages.Notify_LoadedLevelChanged();
					if (Current.SubcameraDriver != null)
					{
						Current.SubcameraDriver.Init();
					}
				};
				if (!PlayDataLoader.Loaded)
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						PlayDataLoader.LoadAllPlayData(false);
					}, null, true, null);
					LongEventHandler.QueueLongEvent(action, "InitializingInterface", false, null);
				}
				else
				{
					action();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0022E51C File Offset: 0x0022C91C
		private static void CheckGlobalInit()
		{
			if (!Root.globalInitDone)
			{
				UnityDataInitializer.CopyUnityData();
				SteamManager.InitIfNeeded();
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				if (commandLineArgs != null && commandLineArgs.Length > 1)
				{
					Log.Message("Command line arguments: " + GenText.ToSpaceList(commandLineArgs.Skip(1)), false);
				}
				VersionControl.LogVersionNumber();
				Application.targetFrameRate = 60;
				Prefs.Init();
				if (Prefs.DevMode)
				{
					StaticConstructorOnStartupUtility.ReportProbablyMissingAttributes();
				}
				if (Root.<>f__mg$cache0 == null)
				{
					Root.<>f__mg$cache0 = new Action(StaticConstructorOnStartupUtility.CallAll);
				}
				LongEventHandler.QueueLongEvent(Root.<>f__mg$cache0, null, false, null);
				Root.globalInitDone = true;
			}
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0022E5C0 File Offset: 0x0022C9C0
		public virtual void Update()
		{
			try
			{
				RealTime.Update();
				bool flag;
				LongEventHandler.LongEventsUpdate(out flag);
				if (flag)
				{
					this.destroyed = true;
				}
				else if (!LongEventHandler.ShouldWaitForEvent)
				{
					Rand.EnsureStateStackEmpty();
					Widgets.EnsureMousePositionStackEmpty();
					SteamManager.Update();
					PortraitsCache.PortraitsCacheUpdate();
					AttackTargetsCache.AttackTargetsCacheStaticUpdate();
					Pawn_MeleeVerbs.PawnMeleeVerbsStaticUpdate();
					Storyteller.StorytellerStaticUpdate();
					CaravanInventoryUtility.CaravanInventoryUtilityStaticUpdate();
					this.uiRoot.UIRootUpdate();
					if (Time.frameCount > 3 && !Root.prefsApplied)
					{
						Root.prefsApplied = true;
						Prefs.Apply();
					}
					this.soundRoot.Update();
					try
					{
						MemoryTracker.Update();
					}
					catch (Exception arg)
					{
						Log.Error("Error in MemoryTracker: " + arg, false);
					}
					try
					{
						MapLeakTracker.Update();
					}
					catch (Exception arg2)
					{
						Log.Error("Error in MapLeakTracker: " + arg2, false);
					}
				}
			}
			catch (Exception arg3)
			{
				Log.Error("Root level exception in Update(): " + arg3, false);
			}
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x0022E6F0 File Offset: 0x0022CAF0
		public void OnGUI()
		{
			try
			{
				if (!this.destroyed)
				{
					GUI.depth = 50;
					UI.ApplyUIScale();
					LongEventHandler.LongEventsOnGUI();
					if (LongEventHandler.ShouldWaitForEvent)
					{
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
					}
					else
					{
						this.uiRoot.UIRootOnGUI();
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in OnGUI(): " + arg, false);
			}
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0022E798 File Offset: 0x0022CB98
		public static void Shutdown()
		{
			SteamManager.ShutdownSteam();
			DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.TempFolderPath);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				fileInfo.Delete();
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				directoryInfo2.Delete(true);
			}
			Application.Quit();
		}
	}
}
