#define ENABLE_PROFILER
using System;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	public class UIRoot_Play : UIRoot
	{
		public MapInterface mapUI = new MapInterface();

		public MainButtonsRoot mainButtonsRoot = new MainButtonsRoot();

		public AlertsReadout alerts = new AlertsReadout();

		public override void Init()
		{
			base.Init();
			Messages.Clear();
		}

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
			if (!base.screenshotMode.FiltersCurrentEvent)
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
			if (!base.screenshotMode.FiltersCurrentEvent)
			{
				Profiler.BeginSample("TutorOnGUI()");
				Find.Tutor.TutorOnGUI();
				Profiler.EndSample();
			}
			Profiler.BeginSample("WindowStackOnGUI()");
			base.windows.WindowStackOnGUI();
			Profiler.EndSample();
			Profiler.BeginSample("ReorderableWidgetOnGUI()");
			ReorderableWidget.ReorderableWidgetOnGUI();
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
				Log.Error("Exception in UIRootUpdate: " + ex.ToString());
			}
		}

		private void OpenMainMenuShortcut()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
			{
				Event.current.Use();
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}
	}
}
