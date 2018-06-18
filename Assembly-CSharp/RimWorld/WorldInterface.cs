using System;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x020008EF RID: 2287
	public class WorldInterface
	{
		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060034A9 RID: 13481 RVA: 0x001C1B00 File Offset: 0x001BFF00
		// (set) Token: 0x060034AA RID: 13482 RVA: 0x001C1B20 File Offset: 0x001BFF20
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

		// Token: 0x060034AB RID: 13483 RVA: 0x001C1B30 File Offset: 0x001BFF30
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

		// Token: 0x060034AC RID: 13484 RVA: 0x001C1C50 File Offset: 0x001C0050
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

		// Token: 0x060034AD RID: 13485 RVA: 0x001C1CE0 File Offset: 0x001C00E0
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

		// Token: 0x060034AE RID: 13486 RVA: 0x001C1E08 File Offset: 0x001C0208
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

		// Token: 0x060034AF RID: 13487 RVA: 0x001C1E58 File Offset: 0x001C0258
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

		// Token: 0x04001C73 RID: 7283
		public WorldSelector selector = new WorldSelector();

		// Token: 0x04001C74 RID: 7284
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x04001C75 RID: 7285
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x04001C76 RID: 7286
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x04001C77 RID: 7287
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x04001C78 RID: 7288
		public bool everReset;
	}
}
