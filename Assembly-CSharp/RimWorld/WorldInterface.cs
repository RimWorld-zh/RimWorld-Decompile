using System;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	public class WorldInterface
	{
		public WorldSelector selector = new WorldSelector();

		public WorldTargeter targeter = new WorldTargeter();

		public WorldInspectPane inspectPane = new WorldInspectPane();

		public WorldGlobalControls globalControls = new WorldGlobalControls();

		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		public bool everReset;

		public WorldInterface()
		{
		}

		public int SelectedTile
		{
			get
			{
				return this.selector.selectedTile;
			}
			set
			{
				this.selector.selectedTile = value;
			}
		}

		public void Reset()
		{
			this.everReset = true;
			this.inspectPane.Reset();
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Find.CurrentMap != null)
				{
					this.SelectedTile = Find.CurrentMap.Tile;
				}
				else
				{
					this.SelectedTile = -1;
				}
			}
			else if (Find.GameInitData != null)
			{
				if (Find.GameInitData.startingTile >= 0 && Find.World != null && !Find.WorldGrid.InBounds(Find.GameInitData.startingTile))
				{
					Log.Error("Map world tile was out of bounds.", false);
					Find.GameInitData.startingTile = -1;
				}
				this.SelectedTile = Find.GameInitData.startingTile;
				this.inspectPane.OpenTabType = typeof(WITab_Terrain);
			}
			else
			{
				this.SelectedTile = -1;
			}
			if (this.SelectedTile >= 0)
			{
				Find.WorldCameraDriver.JumpTo(this.SelectedTile);
			}
			else
			{
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.viewCenter);
			}
			Find.WorldCameraDriver.ResetAltitude();
		}

		public void WorldInterfaceUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			if (worldRenderedNow)
			{
				Profiler.BeginSample("TargeterUpdate()");
				this.targeter.TargeterUpdate();
				Profiler.EndSample();
				Profiler.BeginSample("WorldSelectionDrawer.DrawSelectionOverlays()");
				WorldSelectionDrawer.DrawSelectionOverlays();
				Profiler.EndSample();
				Profiler.BeginSample("WorldDebugDrawerUpdate()");
				Find.WorldDebugDrawer.WorldDebugDrawerUpdate();
				Profiler.EndSample();
			}
			else
			{
				this.targeter.StopTargeting();
			}
			Profiler.BeginSample("WorldRoutePlannerUpdate()");
			this.routePlanner.WorldRoutePlannerUpdate();
			Profiler.EndSample();
		}

		public void WorldInterfaceOnGUI()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			Profiler.BeginSample("CheckOpenOrCloseInspectPane()");
			this.CheckOpenOrCloseInspectPane();
			Profiler.EndSample();
			if (worldRenderedNow)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				Profiler.BeginSample("ExpandableWorldObjectsOnGUI()");
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsOnGUI();
				Profiler.EndSample();
				Profiler.BeginSample("WorldSelectionDrawer.SelectionOverlaysOnGUI()");
				WorldSelectionDrawer.SelectionOverlaysOnGUI();
				Profiler.EndSample();
				Profiler.BeginSample("WorldRoutePlannerOnGUI()");
				this.routePlanner.WorldRoutePlannerOnGUI();
				Profiler.EndSample();
				if (!screenshotMode.FiltersCurrentEvent && Current.ProgramState == ProgramState.Playing)
				{
					Profiler.BeginSample("ColonistBarOnGUI()");
					Find.ColonistBar.ColonistBarOnGUI();
					Profiler.EndSample();
				}
				Profiler.BeginSample("selector.dragBox.DragBoxOnGUI()");
				this.selector.dragBox.DragBoxOnGUI();
				Profiler.EndSample();
				Profiler.BeginSample("TargeterOnGUI()");
				this.targeter.TargeterOnGUI();
				Profiler.EndSample();
				if (!screenshotMode.FiltersCurrentEvent)
				{
					Profiler.BeginSample("globalControls.WorldGlobalControlsOnGUI()");
					this.globalControls.WorldGlobalControlsOnGUI();
					Profiler.EndSample();
				}
				Profiler.BeginSample("WorldDebugDrawerOnGUI()");
				Find.WorldDebugDrawer.WorldDebugDrawerOnGUI();
				Profiler.EndSample();
			}
		}

		public void HandleLowPriorityInput()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			if (worldRenderedNow)
			{
				Profiler.BeginSample("targeter.ProcessInputEvents()");
				this.targeter.ProcessInputEvents();
				Profiler.EndSample();
				Profiler.BeginSample("selector.WorldSelectorOnGUI()");
				this.selector.WorldSelectorOnGUI();
				Profiler.EndSample();
			}
		}

		private void CheckOpenOrCloseInspectPane()
		{
			if (this.selector.AnyObjectOrTileSelected && WorldRendererUtility.WorldRenderedNow && (Current.ProgramState != ProgramState.Playing || Find.MainTabsRoot.OpenTab == null))
			{
				if (!Find.WindowStack.IsOpen<WorldInspectPane>())
				{
					Find.WindowStack.Add(this.inspectPane);
				}
			}
			else if (Find.WindowStack.IsOpen<WorldInspectPane>())
			{
				Find.WindowStack.TryRemove(this.inspectPane, false);
			}
		}
	}
}
