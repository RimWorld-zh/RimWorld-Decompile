using System;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Steam;

namespace Verse
{
	public class UIRoot_Entry : UIRoot
	{
		[CompilerGenerated]
		private static Action <>f__am$cache0;

		public UIRoot_Entry()
		{
		}

		private bool ShouldDoMainMenu
		{
			get
			{
				bool result;
				if (LongEventHandler.AnyEventNowOrWaiting)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < Find.WindowStack.Count; i++)
					{
						if (this.windows[i].layer == WindowLayer.Dialog && !Find.WindowStack[i].IsDebug)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}

		public override void Init()
		{
			base.Init();
			UIMenuBackgroundManager.background = new UI_BackgroundMain();
			MainMenuDrawer.Init();
			QuickStarter.CheckQuickStart();
			VersionUpdateDialogMaker.CreateVersionUpdateDialogIfNecessary();
			if (!SteamManager.Initialized)
			{
				string text = "SteamClientMissing".Translate();
				Dialog_MessageBox window = new Dialog_MessageBox(text, "Quit".Translate(), delegate()
				{
					Application.Quit();
				}, "Ignore".Translate(), null, null, false, null, null);
				Find.WindowStack.Add(window);
			}
		}

		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceOnGUI();
			}
			this.DoMainMenu();
			if (Current.Game != null)
			{
				Find.Tutor.TutorOnGUI();
			}
			Profiler.BeginSample("ReorderableWidgetOnGUI_BeforeWindowStack()");
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			Profiler.EndSample();
			Profiler.BeginSample("WindowStackOnGUI()");
			this.windows.WindowStackOnGUI();
			Profiler.EndSample();
			Profiler.BeginSample("ReorderableWidgetOnGUI_AfterWindowStack()");
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Profiler.EndSample();
			Profiler.BeginSample("WidgetsOnGUI()");
			Widgets.WidgetsOnGUI();
			Profiler.EndSample();
			if (Find.World != null)
			{
				Find.World.UI.HandleLowPriorityInput();
			}
		}

		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceUpdate();
			}
			if (Current.Game != null)
			{
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
		}

		private void DoMainMenu()
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				if (this.ShouldDoMainMenu)
				{
					Current.Game = null;
					MainMenuDrawer.MainMenuOnGUI();
				}
			}
		}

		[CompilerGenerated]
		private static void <Init>m__0()
		{
			Application.Quit();
		}
	}
}
