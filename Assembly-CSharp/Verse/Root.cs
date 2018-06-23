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
	// Token: 0x02000BDA RID: 3034
	public abstract class Root : MonoBehaviour
	{
		// Token: 0x04002D4F RID: 11599
		private static bool globalInitDone;

		// Token: 0x04002D50 RID: 11600
		private static bool prefsApplied;

		// Token: 0x04002D51 RID: 11601
		protected static bool checkedAutostartSaveFile;

		// Token: 0x04002D52 RID: 11602
		protected bool destroyed;

		// Token: 0x04002D53 RID: 11603
		public SoundRoot soundRoot;

		// Token: 0x04002D54 RID: 11604
		public UIRoot uiRoot;

		// Token: 0x04002D56 RID: 11606
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		// Token: 0x06004247 RID: 16967 RVA: 0x0022E0BC File Offset: 0x0022C4BC
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

		// Token: 0x06004248 RID: 16968 RVA: 0x0022E160 File Offset: 0x0022C560
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

		// Token: 0x06004249 RID: 16969 RVA: 0x0022E204 File Offset: 0x0022C604
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

		// Token: 0x0600424A RID: 16970 RVA: 0x0022E334 File Offset: 0x0022C734
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

		// Token: 0x0600424B RID: 16971 RVA: 0x0022E3DC File Offset: 0x0022C7DC
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
