using RimWorld;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse.Profile;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	public abstract class Root : MonoBehaviour
	{
		private static bool globalInitDone;

		private static bool prefsApplied;

		protected bool destroyed;

		public SoundRoot soundRoot;

		public UIRoot uiRoot;

		[CompilerGenerated]
		private static Action _003C_003Ef__mg_0024cache0;

		public virtual void Start()
		{
			Current.Notify_LoadedSceneChanged();
			Root.CheckGlobalInit();
			Action action = delegate
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
				if ((UnityEngine.Object)Current.SubcameraDriver != (UnityEngine.Object)null)
				{
					Current.SubcameraDriver.Init();
				}
			};
			if (!PlayDataLoader.Loaded)
			{
				LongEventHandler.QueueLongEvent(delegate
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

		private static void CheckGlobalInit()
		{
			if (!Root.globalInitDone)
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				if (currentCulture.Name != "en-US")
				{
					Log.Warning("Unexpected culture: " + currentCulture + ". Resetting to en-US.");
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				}
				SteamManager.InitIfNeeded();
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				if (commandLineArgs != null && commandLineArgs.Length > 1)
				{
					Log.Message("Command line arguments: " + GenText.ToSpaceList(commandLineArgs.Skip(1)));
				}
				UnityData.CopyUnityData();
				VersionControl.LogVersionNumber();
				Application.targetFrameRate = 60;
				Prefs.Init();
				if (Prefs.DevMode)
				{
					StaticConstructorOnStartupUtility.ReportProbablyMissingAttributes();
				}
				LongEventHandler.QueueLongEvent(StaticConstructorOnStartupUtility.CallAll, null, false, null);
				Root.globalInitDone = true;
			}
		}

		public virtual void Update()
		{
			try
			{
				RealTime.Update();
				bool flag = default(bool);
				LongEventHandler.LongEventsUpdate(out flag);
				if (flag)
				{
					this.destroyed = true;
				}
				else if (!LongEventHandler.ShouldWaitForEvent)
				{
					Rand.EnsureStateStackEmpty();
					SteamManager.Update();
					PortraitsCache.PortraitsCacheUpdate();
					this.uiRoot.UIRootUpdate();
					if (Time.frameCount > 3 && !Root.prefsApplied)
					{
						Root.prefsApplied = true;
						Prefs.Apply();
					}
					this.soundRoot.Update();
					MemoryTracker.Update();
				}
			}
			catch (Exception e)
			{
				Log.Notify_Exception(e);
				throw;
			}
		}

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
			catch (Exception e)
			{
				Log.Notify_Exception(e);
				throw;
			}
		}

		public static void Shutdown()
		{
			SteamManager.ShutdownSteam();
			DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.TempFolderPath);
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				fileInfo.Delete();
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			foreach (DirectoryInfo directoryInfo2 in directories)
			{
				directoryInfo2.Delete(true);
			}
			Application.Quit();
		}
	}
}
