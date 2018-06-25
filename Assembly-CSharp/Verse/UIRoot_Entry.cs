using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000E5D RID: 3677
	public class UIRoot_Entry : UIRoot
	{
		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x060056A9 RID: 22185 RVA: 0x002CAD18 File Offset: 0x002C9118
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

		// Token: 0x060056AA RID: 22186 RVA: 0x002CAD90 File Offset: 0x002C9190
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

		// Token: 0x060056AB RID: 22187 RVA: 0x002CAE20 File Offset: 0x002C9220
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

		// Token: 0x060056AC RID: 22188 RVA: 0x002CAED6 File Offset: 0x002C92D6
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

		// Token: 0x060056AD RID: 22189 RVA: 0x002CAF14 File Offset: 0x002C9314
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
