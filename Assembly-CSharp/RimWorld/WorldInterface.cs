using System;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x020008ED RID: 2285
	public class WorldInterface
	{
		// Token: 0x04001C77 RID: 7287
		public WorldSelector selector = new WorldSelector();

		// Token: 0x04001C78 RID: 7288
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x04001C79 RID: 7289
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x04001C7A RID: 7290
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x04001C7B RID: 7291
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x04001C7C RID: 7292
		public bool everReset;

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060034A6 RID: 13478 RVA: 0x001C20FC File Offset: 0x001C04FC
		// (set) Token: 0x060034A7 RID: 13479 RVA: 0x001C211C File Offset: 0x001C051C
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

		// Token: 0x060034A8 RID: 13480 RVA: 0x001C212C File Offset: 0x001C052C
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

		// Token: 0x060034A9 RID: 13481 RVA: 0x001C224C File Offset: 0x001C064C
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

		// Token: 0x060034AA RID: 13482 RVA: 0x001C22DC File Offset: 0x001C06DC
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

		// Token: 0x060034AB RID: 13483 RVA: 0x001C2404 File Offset: 0x001C0804
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

		// Token: 0x060034AC RID: 13484 RVA: 0x001C2454 File Offset: 0x001C0854
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
