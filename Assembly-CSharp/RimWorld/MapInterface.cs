using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000878 RID: 2168
	public class MapInterface
	{
		// Token: 0x0600317D RID: 12669 RVA: 0x001AD9EC File Offset: 0x001ABDEC
		public void MapInterfaceOnGUI_BeforeMainTabs()
		{
			if (Find.CurrentMap != null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
					Profiler.BeginSample("ThingOverlaysOnGUI()");
					this.thingOverlays.ThingOverlaysOnGUI();
					Profiler.EndSample();
					Profiler.BeginSample("MapComponentOnGUI()");
					MapComponentUtility.MapComponentOnGUI(Find.CurrentMap);
					Profiler.EndSample();
					Profiler.BeginSample("BeautyDrawerOnGUI()");
					BeautyDrawer.BeautyDrawerOnGUI();
					Profiler.EndSample();
					if (!screenshotMode.FiltersCurrentEvent)
					{
						Profiler.BeginSample("ColonistBarOnGUI()");
						this.colonistBar.ColonistBarOnGUI();
						Profiler.EndSample();
					}
					Profiler.BeginSample("DragBoxOnGUI()");
					this.selector.dragBox.DragBoxOnGUI();
					Profiler.EndSample();
					Profiler.BeginSample("DesignationManagerOnGUI()");
					this.designatorManager.DesignationManagerOnGUI();
					Profiler.EndSample();
					Profiler.BeginSample("TargeterOnGUI()");
					this.targeter.TargeterOnGUI();
					Profiler.EndSample();
					Profiler.BeginSample("DispenseAllThingTooltips()");
					Find.CurrentMap.tooltipGiverList.DispenseAllThingTooltips();
					Profiler.EndSample();
					if (DebugViewSettings.drawFoodSearchFromMouse)
					{
						Profiler.BeginSample("FoodUtility.DebugFoodSearchFromMouse_OnGUI()");
						FoodUtility.DebugFoodSearchFromMouse_OnGUI();
						Profiler.EndSample();
					}
					if (DebugViewSettings.drawAttackTargetScores)
					{
						Profiler.BeginSample("AttackTargetFinder.DebugDrawAttackTargetScores_OnGUI()");
						AttackTargetFinder.DebugDrawAttackTargetScores_OnGUI();
						Profiler.EndSample();
					}
					if (!screenshotMode.FiltersCurrentEvent)
					{
						Profiler.BeginSample("GlobalControlsOnGUI()");
						this.globalControls.GlobalControlsOnGUI();
						Profiler.EndSample();
						Profiler.BeginSample("ResourceReadoutOnGUI()");
						this.resourceReadout.ResourceReadoutOnGUI();
						Profiler.EndSample();
					}
				}
				else
				{
					this.targeter.StopTargeting();
				}
			}
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x001ADB94 File Offset: 0x001ABF94
		public void MapInterfaceOnGUI_AfterMainTabs()
		{
			if (Find.CurrentMap != null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
					if (!screenshotMode.FiltersCurrentEvent)
					{
						Profiler.BeginSample("MouseoverReadoutOnGUI()");
						this.mouseoverReadout.MouseoverReadoutOnGUI();
						Profiler.EndSample();
						Profiler.BeginSample("EnvironmentStatsOnGUI()");
						EnvironmentStatsDrawer.EnvironmentStatsOnGUI();
						Profiler.EndSample();
						Profiler.BeginSample("DebugDrawerOnGUI()");
						Find.CurrentMap.debugDrawer.DebugDrawerOnGUI();
						Profiler.EndSample();
					}
				}
			}
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x001ADC24 File Offset: 0x001AC024
		public void HandleMapClicks()
		{
			if (Find.CurrentMap != null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					Profiler.BeginSample("DesignatorManager.ProcessInputEvents()");
					this.designatorManager.ProcessInputEvents();
					Profiler.EndSample();
					Profiler.BeginSample("targeter.ProcessInputEvents()");
					this.targeter.ProcessInputEvents();
					Profiler.EndSample();
				}
			}
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x001ADC84 File Offset: 0x001AC084
		public void HandleLowPriorityInput()
		{
			if (Find.CurrentMap != null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					Profiler.BeginSample("SelectorOnGUI()");
					this.selector.SelectorOnGUI();
					Profiler.EndSample();
					Profiler.BeginSample("LordManagerOnGUI()");
					Find.CurrentMap.lordManager.LordManagerOnGUI();
					Profiler.EndSample();
				}
			}
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x001ADCE8 File Offset: 0x001AC0E8
		public void MapInterfaceUpdate()
		{
			if (Find.CurrentMap != null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					Profiler.BeginSample("TargeterUpdate()");
					this.targeter.TargeterUpdate();
					Profiler.EndSample();
					Profiler.BeginSample("DrawSelectionOverlays()");
					SelectionDrawer.DrawSelectionOverlays();
					Profiler.EndSample();
					Profiler.BeginSample("EnvironmentStatsDrawer.DrawRoomOverlays()");
					EnvironmentStatsDrawer.DrawRoomOverlays();
					Profiler.EndSample();
					Profiler.BeginSample("DesignatorManagerUpdate()");
					this.designatorManager.DesignatorManagerUpdate();
					Profiler.EndSample();
					Profiler.BeginSample("RoofGridUpdate()");
					Find.CurrentMap.roofGrid.RoofGridUpdate();
					Profiler.EndSample();
					Profiler.BeginSample("ExitMapGridUpdate()");
					Find.CurrentMap.exitMapGrid.ExitMapGridUpdate();
					Profiler.EndSample();
					Profiler.BeginSample("DeepResourceGridUpdate()");
					Find.CurrentMap.deepResourceGrid.DeepResourceGridUpdate();
					Profiler.EndSample();
					Profiler.BeginSample("Debug drawing");
					if (DebugViewSettings.drawPawnDebug)
					{
						Find.CurrentMap.pawnDestinationReservationManager.DebugDrawDestinations();
						Find.CurrentMap.reservationManager.DebugDrawReservations();
					}
					if (DebugViewSettings.drawFoodSearchFromMouse)
					{
						FoodUtility.DebugFoodSearchFromMouse_Update();
					}
					if (DebugViewSettings.drawPreyInfo)
					{
						FoodUtility.DebugDrawPredatorFoodSource();
					}
					if (DebugViewSettings.drawAttackTargetScores)
					{
						AttackTargetFinder.DebugDrawAttackTargetScores_Update();
					}
					MiscDebugDrawer.DebugDrawInteractionCells();
					Find.CurrentMap.debugDrawer.DebugDrawerUpdate();
					Find.CurrentMap.regionGrid.DebugDraw();
					InfestationCellFinder.DebugDraw();
					StealAIDebugDrawer.DebugDraw();
					if (DebugViewSettings.drawRiverDebug)
					{
						Find.CurrentMap.waterInfo.DebugDrawRiver();
					}
					Profiler.EndSample();
				}
			}
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001ADE78 File Offset: 0x001AC278
		public void Notify_SwitchedMap()
		{
			this.designatorManager.Deselect();
			this.reverseDesignatorDatabase.Reinit();
			this.selector.ClearSelection();
			this.selector.dragBox.active = false;
			this.targeter.StopTargeting();
			MainButtonDef openTab = Find.MainTabsRoot.OpenTab;
			List<MainButtonDef> allDefsListForReading = DefDatabase<MainButtonDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				allDefsListForReading[i].Notify_SwitchedMap();
			}
			if (openTab != null && openTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.SetCurrentTab(openTab, false);
			}
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
			}
		}

		// Token: 0x04001AC2 RID: 6850
		public ThingOverlays thingOverlays = new ThingOverlays();

		// Token: 0x04001AC3 RID: 6851
		public Selector selector = new Selector();

		// Token: 0x04001AC4 RID: 6852
		public Targeter targeter = new Targeter();

		// Token: 0x04001AC5 RID: 6853
		public DesignatorManager designatorManager = new DesignatorManager();

		// Token: 0x04001AC6 RID: 6854
		public ReverseDesignatorDatabase reverseDesignatorDatabase = new ReverseDesignatorDatabase();

		// Token: 0x04001AC7 RID: 6855
		private MouseoverReadout mouseoverReadout = new MouseoverReadout();

		// Token: 0x04001AC8 RID: 6856
		public GlobalControls globalControls = new GlobalControls();

		// Token: 0x04001AC9 RID: 6857
		protected ResourceReadout resourceReadout = new ResourceReadout();

		// Token: 0x04001ACA RID: 6858
		public ColonistBar colonistBar = new ColonistBar();
	}
}
