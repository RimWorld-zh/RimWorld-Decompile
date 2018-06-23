using System;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EB RID: 2283
	public class WorldInterface
	{
		// Token: 0x04001C71 RID: 7281
		public WorldSelector selector = new WorldSelector();

		// Token: 0x04001C72 RID: 7282
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x04001C73 RID: 7283
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x04001C74 RID: 7284
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x04001C75 RID: 7285
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x04001C76 RID: 7286
		public bool everReset;

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060034A2 RID: 13474 RVA: 0x001C1CE8 File Offset: 0x001C00E8
		// (set) Token: 0x060034A3 RID: 13475 RVA: 0x001C1D08 File Offset: 0x001C0108
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

		// Token: 0x060034A4 RID: 13476 RVA: 0x001C1D18 File Offset: 0x001C0118
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

		// Token: 0x060034A5 RID: 13477 RVA: 0x001C1E38 File Offset: 0x001C0238
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

		// Token: 0x060034A6 RID: 13478 RVA: 0x001C1EC8 File Offset: 0x001C02C8
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

		// Token: 0x060034A7 RID: 13479 RVA: 0x001C1FF0 File Offset: 0x001C03F0
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

		// Token: 0x060034A8 RID: 13480 RVA: 0x001C2040 File Offset: 0x001C0440
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
