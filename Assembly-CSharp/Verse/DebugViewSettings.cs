using System;

namespace Verse
{
	// Token: 0x02000F10 RID: 3856
	public static class DebugViewSettings
	{
		// Token: 0x06005C69 RID: 23657 RVA: 0x002EDE5B File Offset: 0x002EC25B
		public static void drawTerrainWaterToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged(MapMeshFlag.Terrain);
			}
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x002EDE79 File Offset: 0x002EC279
		public static void drawShadowsToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged((MapMeshFlag)(-1));
			}
		}

		// Token: 0x04003D08 RID: 15624
		public static bool drawFog = true;

		// Token: 0x04003D09 RID: 15625
		public static bool drawSnow = true;

		// Token: 0x04003D0A RID: 15626
		public static bool drawTerrain = true;

		// Token: 0x04003D0B RID: 15627
		public static bool drawTerrainWater = true;

		// Token: 0x04003D0C RID: 15628
		public static bool drawThingsDynamic = true;

		// Token: 0x04003D0D RID: 15629
		public static bool drawThingsPrinted = true;

		// Token: 0x04003D0E RID: 15630
		public static bool drawShadows = true;

		// Token: 0x04003D0F RID: 15631
		public static bool drawLightingOverlay = true;

		// Token: 0x04003D10 RID: 15632
		public static bool drawWorldOverlays = true;

		// Token: 0x04003D11 RID: 15633
		public static bool drawPaths = false;

		// Token: 0x04003D12 RID: 15634
		public static bool drawCastPositionSearch = false;

		// Token: 0x04003D13 RID: 15635
		public static bool drawDestSearch = false;

		// Token: 0x04003D14 RID: 15636
		public static bool drawSectionEdges = false;

		// Token: 0x04003D15 RID: 15637
		public static bool drawRiverDebug = false;

		// Token: 0x04003D16 RID: 15638
		public static bool drawPawnDebug = false;

		// Token: 0x04003D17 RID: 15639
		public static bool drawPawnRotatorTarget = false;

		// Token: 0x04003D18 RID: 15640
		public static bool drawRegions = false;

		// Token: 0x04003D19 RID: 15641
		public static bool drawRegionLinks = false;

		// Token: 0x04003D1A RID: 15642
		public static bool drawRegionDirties = false;

		// Token: 0x04003D1B RID: 15643
		public static bool drawRegionTraversal = false;

		// Token: 0x04003D1C RID: 15644
		public static bool drawRegionThings = false;

		// Token: 0x04003D1D RID: 15645
		public static bool drawRooms = false;

		// Token: 0x04003D1E RID: 15646
		public static bool drawRoomGroups = false;

		// Token: 0x04003D1F RID: 15647
		public static bool drawPower = false;

		// Token: 0x04003D20 RID: 15648
		public static bool drawPowerNetGrid = false;

		// Token: 0x04003D21 RID: 15649
		public static bool drawOpportunisticJobs = false;

		// Token: 0x04003D22 RID: 15650
		public static bool drawTooltipEdges = false;

		// Token: 0x04003D23 RID: 15651
		public static bool drawRecordedNoise = false;

		// Token: 0x04003D24 RID: 15652
		public static bool drawFoodSearchFromMouse = false;

		// Token: 0x04003D25 RID: 15653
		public static bool drawPreyInfo = false;

		// Token: 0x04003D26 RID: 15654
		public static bool drawGlow = false;

		// Token: 0x04003D27 RID: 15655
		public static bool drawFactions = false;

		// Token: 0x04003D28 RID: 15656
		public static bool drawLords = false;

		// Token: 0x04003D29 RID: 15657
		public static bool drawDuties = false;

		// Token: 0x04003D2A RID: 15658
		public static bool drawShooting = false;

		// Token: 0x04003D2B RID: 15659
		public static bool drawInfestationChance = false;

		// Token: 0x04003D2C RID: 15660
		public static bool drawStealDebug = false;

		// Token: 0x04003D2D RID: 15661
		public static bool drawInterceptChecks = false;

		// Token: 0x04003D2E RID: 15662
		public static bool drawDeepResources = false;

		// Token: 0x04003D2F RID: 15663
		public static bool drawAttackTargetScores = false;

		// Token: 0x04003D30 RID: 15664
		public static bool drawInteractionCells = false;

		// Token: 0x04003D31 RID: 15665
		public static bool drawDoorsDebug = false;

		// Token: 0x04003D32 RID: 15666
		public static bool writeGame = false;

		// Token: 0x04003D33 RID: 15667
		public static bool writeSteamItems = false;

		// Token: 0x04003D34 RID: 15668
		public static bool writeConcepts = false;

		// Token: 0x04003D35 RID: 15669
		public static bool writePathCosts = false;

		// Token: 0x04003D36 RID: 15670
		public static bool writeFertility = false;

		// Token: 0x04003D37 RID: 15671
		public static bool writeLinkFlags = false;

		// Token: 0x04003D38 RID: 15672
		public static bool writeCover = false;

		// Token: 0x04003D39 RID: 15673
		public static bool writeCellContents = false;

		// Token: 0x04003D3A RID: 15674
		public static bool writeMusicManagerPlay = false;

		// Token: 0x04003D3B RID: 15675
		public static bool writeStoryteller = false;

		// Token: 0x04003D3C RID: 15676
		public static bool writePlayingSounds = false;

		// Token: 0x04003D3D RID: 15677
		public static bool writeSoundEventsRecord = false;

		// Token: 0x04003D3E RID: 15678
		public static bool writeMoteSaturation = false;

		// Token: 0x04003D3F RID: 15679
		public static bool writeSnowDepth = false;

		// Token: 0x04003D40 RID: 15680
		public static bool writeEcosystem = false;

		// Token: 0x04003D41 RID: 15681
		public static bool writeRecentStrikes = false;

		// Token: 0x04003D42 RID: 15682
		public static bool writeBeauty = false;

		// Token: 0x04003D43 RID: 15683
		public static bool writeListRepairableBldgs = false;

		// Token: 0x04003D44 RID: 15684
		public static bool writeListFilthInHomeArea = false;

		// Token: 0x04003D45 RID: 15685
		public static bool writeListHaulables = false;

		// Token: 0x04003D46 RID: 15686
		public static bool writeListMergeables = false;

		// Token: 0x04003D47 RID: 15687
		public static bool writeTotalSnowDepth = false;

		// Token: 0x04003D48 RID: 15688
		public static bool writeCanReachColony = false;

		// Token: 0x04003D49 RID: 15689
		public static bool writeMentalStateCalcs = false;

		// Token: 0x04003D4A RID: 15690
		public static bool writeWind = false;

		// Token: 0x04003D4B RID: 15691
		public static bool writeTerrain = false;

		// Token: 0x04003D4C RID: 15692
		public static bool writeApparelScore = false;

		// Token: 0x04003D4D RID: 15693
		public static bool writeWorkSettings = false;

		// Token: 0x04003D4E RID: 15694
		public static bool writeSkyManager = false;

		// Token: 0x04003D4F RID: 15695
		public static bool writeMemoryUsage = false;

		// Token: 0x04003D50 RID: 15696
		public static bool writeMapGameConditions = false;

		// Token: 0x04003D51 RID: 15697
		public static bool writeAttackTargets = false;

		// Token: 0x04003D52 RID: 15698
		public static bool logIncapChance = false;

		// Token: 0x04003D53 RID: 15699
		public static bool logInput = false;

		// Token: 0x04003D54 RID: 15700
		public static bool logApparelGeneration = false;

		// Token: 0x04003D55 RID: 15701
		public static bool logLordToilTransitions = false;

		// Token: 0x04003D56 RID: 15702
		public static bool logGrammarResolution = false;

		// Token: 0x04003D57 RID: 15703
		public static bool logCombatLogMouseover = false;

		// Token: 0x04003D58 RID: 15704
		public static bool logMapLoad = false;

		// Token: 0x04003D59 RID: 15705
		public static bool logTutor = false;

		// Token: 0x04003D5A RID: 15706
		public static bool logSignals = false;

		// Token: 0x04003D5B RID: 15707
		public static bool logWorldPawnGC = false;

		// Token: 0x04003D5C RID: 15708
		public static bool logTaleRecording = false;

		// Token: 0x04003D5D RID: 15709
		public static bool logHourlyScreenshot = false;

		// Token: 0x04003D5E RID: 15710
		public static bool logFilthSummary = false;

		// Token: 0x04003D5F RID: 15711
		public static bool debugApparelOptimize = false;

		// Token: 0x04003D60 RID: 15712
		public static bool showAllRoomStats = false;

		// Token: 0x04003D61 RID: 15713
		public static bool showFloatMenuWorkGivers = false;
	}
}
