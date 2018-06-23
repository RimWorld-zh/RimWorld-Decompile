using System;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D5 RID: 2261
	public class UIRoot_Play : UIRoot
	{
		// Token: 0x04001BCB RID: 7115
		public MapInterface mapUI = new MapInterface();

		// Token: 0x04001BCC RID: 7116
		public MainButtonsRoot mainButtonsRoot = new MainButtonsRoot();

		// Token: 0x04001BCD RID: 7117
		public AlertsReadout alerts = new AlertsReadout();

		// Token: 0x060033CD RID: 13261 RVA: 0x001BADD7 File Offset: 0x001B91D7
		public override void Init()
		{
			base.Init();
			Messages.Clear();
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x001BADE8 File Offset: 0x001B91E8
		public override void UIRootOnGUI()
		{
			Profiler.BeginSample("Event: " + Event.current.type);
			Profiler.BeginSample("base.UIRootOnGUI()");
			base.UIRootOnGUI();
			Profiler.EndSample();
			Profiler.BeginSample("GameInfoOnGUI()");
			Find.GameInfo.GameInfoOnGUI();
			Profiler.EndSample();
			Profiler.BeginSample("WorldInterfaceOnGUI()");
			Find.World.UI.WorldInterfaceOnGUI();
			Profiler.EndSample();
			Profiler.BeginSample("MapInterfaceOnGUI_BeforeMainTabs()");
			this.mapUI.MapInterfaceOnGUI_BeforeMainTabs();
			Profiler.EndSample();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				Profiler.BeginSample("MainButtonsOnGUI()");
				this.mainButtonsRoot.MainButtonsOnGUI();
				Profiler.EndSample();
				Profiler.BeginSample("AlertsReadoutOnGUI()");
				this.alerts.AlertsReadoutOnGUI();
				Profiler.EndSample();
			}
			Profiler.BeginSample("MapInterfaceOnGUI_AfterMainTabs()");
			this.mapUI.MapInterfaceOnGUI_AfterMainTabs();
			Profiler.EndSample();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				Profiler.BeginSample("TutorOnGUI()");
				Find.Tutor.TutorOnGUI();
				Profiler.EndSample();
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
			Profiler.BeginSample("HandleMapClicks()");
			this.mapUI.HandleMapClicks();
			Profiler.EndSample();
			Profiler.BeginSample("SelectedDesignator.SelectedProcessInput()");
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.SelectedProcessInput(Event.current);
			}
			Profiler.EndSample();
			Profiler.BeginSample("DebugToolsOnGUI()");
			DebugTools.DebugToolsOnGUI();
			Profiler.EndSample();
			Profiler.BeginSample("HandleLowPriorityShortcuts()");
			this.mainButtonsRoot.HandleLowPriorityShortcuts();
			Profiler.EndSample();
			Profiler.BeginSample("WorldInterface.HandleLowPriorityInput()");
			Find.World.UI.HandleLowPriorityInput();
			Profiler.EndSample();
			Profiler.BeginSample("MapInterface.HandleLowPriorityInput()");
			this.mapUI.HandleLowPriorityInput();
			Profiler.EndSample();
			this.OpenMainMenuShortcut();
			Profiler.EndSample();
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x001BB018 File Offset: 0x001B9418
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			try
			{
				Profiler.BeginSample("WorldInterfaceUpdate()");
				Find.World.UI.WorldInterfaceUpdate();
				Profiler.EndSample();
				Profiler.BeginSample("MapInterfaceUpdate()");
				this.mapUI.MapInterfaceUpdate();
				Profiler.EndSample();
				Profiler.BeginSample("AlertsReadoutUpdate()");
				this.alerts.AlertsReadoutUpdate();
				Profiler.EndSample();
				Profiler.BeginSample("LessonAutoActivatorUpdate()");
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Profiler.EndSample();
				Profiler.BeginSample("TutorialUpdate()");
				Find.Tutor.TutorUpdate();
				Profiler.EndSample();
			}
			catch (Exception ex)
			{
				Log.Error("Exception in UIRootUpdate: " + ex.ToString(), false);
			}
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x001BB0E0 File Offset: 0x001B94E0
		private void OpenMainMenuShortcut()
		{
			if ((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}
	}
}
