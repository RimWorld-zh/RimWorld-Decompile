using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EA9 RID: 3753
	[StaticConstructorOnStartup]
	internal class TexButton
	{
		// Token: 0x04003A9F RID: 15007
		public static readonly Texture2D CloseXBig = ContentFinder<Texture2D>.Get("UI/Widgets/CloseX", true);

		// Token: 0x04003AA0 RID: 15008
		public static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall", true);

		// Token: 0x04003AA1 RID: 15009
		public static readonly Texture2D NextBig = ContentFinder<Texture2D>.Get("UI/Widgets/NextArrow", true);

		// Token: 0x04003AA2 RID: 15010
		public static readonly Texture2D DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);

		// Token: 0x04003AA3 RID: 15011
		public static readonly Texture2D ReorderUp = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderUp", true);

		// Token: 0x04003AA4 RID: 15012
		public static readonly Texture2D ReorderDown = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderDown", true);

		// Token: 0x04003AA5 RID: 15013
		public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x04003AA6 RID: 15014
		public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);

		// Token: 0x04003AA7 RID: 15015
		public static readonly Texture2D Suspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);

		// Token: 0x04003AA8 RID: 15016
		public static readonly Texture2D SelectOverlappingNext = ContentFinder<Texture2D>.Get("UI/Buttons/SelectNextOverlapping", true);

		// Token: 0x04003AA9 RID: 15017
		public static readonly Texture2D Info = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);

		// Token: 0x04003AAA RID: 15018
		public static readonly Texture2D Rename = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);

		// Token: 0x04003AAB RID: 15019
		public static readonly Texture2D Banish = ContentFinder<Texture2D>.Get("UI/Buttons/Banish", true);

		// Token: 0x04003AAC RID: 15020
		public static readonly Texture2D OpenStatsReport = ContentFinder<Texture2D>.Get("UI/Buttons/OpenStatsReport", true);

		// Token: 0x04003AAD RID: 15021
		public static readonly Texture2D Copy = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);

		// Token: 0x04003AAE RID: 15022
		public static readonly Texture2D Paste = ContentFinder<Texture2D>.Get("UI/Buttons/Paste", true);

		// Token: 0x04003AAF RID: 15023
		public static readonly Texture2D Drop = ContentFinder<Texture2D>.Get("UI/Buttons/Drop", true);

		// Token: 0x04003AB0 RID: 15024
		public static readonly Texture2D Ingest = ContentFinder<Texture2D>.Get("UI/Buttons/Ingest", true);

		// Token: 0x04003AB1 RID: 15025
		public static readonly Texture2D DragHash = ContentFinder<Texture2D>.Get("UI/Buttons/DragHash", true);

		// Token: 0x04003AB2 RID: 15026
		public static readonly Texture2D ToggleLog = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleLog", true);

		// Token: 0x04003AB3 RID: 15027
		public static readonly Texture2D OpenDebugActionsMenu = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenDebugActionsMenu", true);

		// Token: 0x04003AB4 RID: 15028
		public static readonly Texture2D OpenInspector = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspector", true);

		// Token: 0x04003AB5 RID: 15029
		public static readonly Texture2D OpenInspectSettings = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspectSettings", true);

		// Token: 0x04003AB6 RID: 15030
		public static readonly Texture2D ToggleGodMode = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleGodMode", true);

		// Token: 0x04003AB7 RID: 15031
		public static readonly Texture2D OpenPackageEditor = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenPackageEditor", true);

		// Token: 0x04003AB8 RID: 15032
		public static readonly Texture2D TogglePauseOnError = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/TogglePauseOnError", true);

		// Token: 0x04003AB9 RID: 15033
		public static readonly Texture2D ToggleTweak = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleTweak", true);

		// Token: 0x04003ABA RID: 15034
		public static readonly Texture2D Add = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Add", true);

		// Token: 0x04003ABB RID: 15035
		public static readonly Texture2D NewItem = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewItem", true);

		// Token: 0x04003ABC RID: 15036
		public static readonly Texture2D Reveal = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reveal", true);

		// Token: 0x04003ABD RID: 15037
		public static readonly Texture2D Collapse = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Collapse", true);

		// Token: 0x04003ABE RID: 15038
		public static readonly Texture2D Empty = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Empty", true);

		// Token: 0x04003ABF RID: 15039
		public static readonly Texture2D Save = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Save", true);

		// Token: 0x04003AC0 RID: 15040
		public static readonly Texture2D NewFile = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewFile", true);

		// Token: 0x04003AC1 RID: 15041
		public static readonly Texture2D RenameDev = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Rename", true);

		// Token: 0x04003AC2 RID: 15042
		public static readonly Texture2D Reload = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reload", true);

		// Token: 0x04003AC3 RID: 15043
		public static readonly Texture2D Play = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Play", true);

		// Token: 0x04003AC4 RID: 15044
		public static readonly Texture2D Stop = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Stop", true);

		// Token: 0x04003AC5 RID: 15045
		public static readonly Texture2D RangeMatch = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/RangeMatch", true);

		// Token: 0x04003AC6 RID: 15046
		public static readonly Texture2D InspectModeToggle = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/InspectModeToggle", true);

		// Token: 0x04003AC7 RID: 15047
		public static readonly Texture2D CenterOnPointsTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CenterOnPoints", true);

		// Token: 0x04003AC8 RID: 15048
		public static readonly Texture2D CurveResetTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CurveReset", true);

		// Token: 0x04003AC9 RID: 15049
		public static readonly Texture2D QuickZoomHor1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor1", true);

		// Token: 0x04003ACA RID: 15050
		public static readonly Texture2D QuickZoomHor100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor100", true);

		// Token: 0x04003ACB RID: 15051
		public static readonly Texture2D QuickZoomHor20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor20k", true);

		// Token: 0x04003ACC RID: 15052
		public static readonly Texture2D QuickZoomVer1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer1", true);

		// Token: 0x04003ACD RID: 15053
		public static readonly Texture2D QuickZoomVer100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer100", true);

		// Token: 0x04003ACE RID: 15054
		public static readonly Texture2D QuickZoomVer20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer20k", true);

		// Token: 0x04003ACF RID: 15055
		public static readonly Texture2D IconBlog = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Blog", true);

		// Token: 0x04003AD0 RID: 15056
		public static readonly Texture2D IconForums = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Forums", true);

		// Token: 0x04003AD1 RID: 15057
		public static readonly Texture2D IconTwitter = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Twitter", true);

		// Token: 0x04003AD2 RID: 15058
		public static readonly Texture2D IconBook = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Book", true);

		// Token: 0x04003AD3 RID: 15059
		public static readonly Texture2D IconSoundtrack = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Soundtrack", true);

		// Token: 0x04003AD4 RID: 15060
		public static readonly Texture2D ShowLearningHelper = ContentFinder<Texture2D>.Get("UI/Buttons/ShowLearningHelper", true);

		// Token: 0x04003AD5 RID: 15061
		public static readonly Texture2D ShowZones = ContentFinder<Texture2D>.Get("UI/Buttons/ShowZones", true);

		// Token: 0x04003AD6 RID: 15062
		public static readonly Texture2D ShowBeauty = ContentFinder<Texture2D>.Get("UI/Buttons/ShowBeauty", true);

		// Token: 0x04003AD7 RID: 15063
		public static readonly Texture2D ShowRoomStats = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoomStats", true);

		// Token: 0x04003AD8 RID: 15064
		public static readonly Texture2D ShowColonistBar = ContentFinder<Texture2D>.Get("UI/Buttons/ShowColonistBar", true);

		// Token: 0x04003AD9 RID: 15065
		public static readonly Texture2D ShowRoofOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoofOverlay", true);

		// Token: 0x04003ADA RID: 15066
		public static readonly Texture2D AutoHomeArea = ContentFinder<Texture2D>.Get("UI/Buttons/AutoHomeArea", true);

		// Token: 0x04003ADB RID: 15067
		public static readonly Texture2D AutoRebuild = ContentFinder<Texture2D>.Get("UI/Buttons/AutoRebuild", true);

		// Token: 0x04003ADC RID: 15068
		public static readonly Texture2D CategorizedResourceReadout = ContentFinder<Texture2D>.Get("UI/Buttons/ResourceReadoutCategorized", true);

		// Token: 0x04003ADD RID: 15069
		public static readonly Texture2D LockNorthUp = ContentFinder<Texture2D>.Get("UI/Buttons/LockNorthUp", true);

		// Token: 0x04003ADE RID: 15070
		public static readonly Texture2D UsePlanetDayNightSystem = ContentFinder<Texture2D>.Get("UI/Buttons/UsePlanetDayNightSystem", true);

		// Token: 0x04003ADF RID: 15071
		public static readonly Texture2D ShowExpandingIcons = ContentFinder<Texture2D>.Get("UI/Buttons/ShowExpandingIcons", true);

		// Token: 0x04003AE0 RID: 15072
		public static readonly Texture2D ShowWorldFeatures = ContentFinder<Texture2D>.Get("UI/Buttons/ShowWorldFeatures", true);

		// Token: 0x04003AE1 RID: 15073
		public static readonly Texture2D[] SpeedButtonTextures = new Texture2D[]
		{
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Pause", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Normal", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Fast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true)
		};
	}
}
