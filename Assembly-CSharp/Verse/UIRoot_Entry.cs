using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000E5B RID: 3675
	public class UIRoot_Entry : UIRoot
	{
		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x060056A5 RID: 22181 RVA: 0x002CABEC File Offset: 0x002C8FEC
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

		// Token: 0x060056A6 RID: 22182 RVA: 0x002CAC64 File Offset: 0x002C9064
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

		// Token: 0x060056A7 RID: 22183 RVA: 0x002CACF4 File Offset: 0x002C90F4
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

		// Token: 0x060056A8 RID: 22184 RVA: 0x002CADAA File Offset: 0x002C91AA
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

		// Token: 0x060056A9 RID: 22185 RVA: 0x002CADE8 File Offset: 0x002C91E8
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
	}
}
