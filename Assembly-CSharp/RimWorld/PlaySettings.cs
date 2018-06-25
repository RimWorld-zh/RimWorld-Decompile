using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BD0 RID: 3024
	public sealed class PlaySettings : IExposable
	{
		// Token: 0x04002CFB RID: 11515
		public bool showLearningHelper = true;

		// Token: 0x04002CFC RID: 11516
		public bool showZones = true;

		// Token: 0x04002CFD RID: 11517
		public bool showBeauty = false;

		// Token: 0x04002CFE RID: 11518
		public bool showRoomStats = false;

		// Token: 0x04002CFF RID: 11519
		public bool showColonistBar = true;

		// Token: 0x04002D00 RID: 11520
		public bool showRoofOverlay = false;

		// Token: 0x04002D01 RID: 11521
		public bool autoHomeArea = true;

		// Token: 0x04002D02 RID: 11522
		public bool autoRebuild = false;

		// Token: 0x04002D03 RID: 11523
		public bool lockNorthUp = true;

		// Token: 0x04002D04 RID: 11524
		public bool usePlanetDayNightSystem = true;

		// Token: 0x04002D05 RID: 11525
		public bool showExpandingIcons = true;

		// Token: 0x04002D06 RID: 11526
		public bool showWorldFeatures = true;

		// Token: 0x04002D07 RID: 11527
		public bool useWorkPriorities = false;

		// Token: 0x04002D08 RID: 11528
		public MedicalCareCategory defaultCareForColonyHumanlike = MedicalCareCategory.Best;

		// Token: 0x04002D09 RID: 11529
		public MedicalCareCategory defaultCareForColonyAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04002D0A RID: 11530
		public MedicalCareCategory defaultCareForColonyPrisoner = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04002D0B RID: 11531
		public MedicalCareCategory defaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04002D0C RID: 11532
		public MedicalCareCategory defaultCareForNeutralAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x04002D0D RID: 11533
		public MedicalCareCategory defaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x060041EB RID: 16875 RVA: 0x0022BD10 File Offset: 0x0022A110
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.showLearningHelper, "showLearningHelper", false, false);
			Scribe_Values.Look<bool>(ref this.showZones, "showZones", false, false);
			Scribe_Values.Look<bool>(ref this.showBeauty, "showBeauty", false, false);
			Scribe_Values.Look<bool>(ref this.showRoomStats, "showRoomStats", false, false);
			Scribe_Values.Look<bool>(ref this.showColonistBar, "showColonistBar", false, false);
			Scribe_Values.Look<bool>(ref this.showRoofOverlay, "showRoofOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.autoHomeArea, "autoHomeArea", false, false);
			Scribe_Values.Look<bool>(ref this.autoRebuild, "autoRebuild", false, false);
			Scribe_Values.Look<bool>(ref this.lockNorthUp, "lockNorthUp", false, false);
			Scribe_Values.Look<bool>(ref this.usePlanetDayNightSystem, "usePlanetDayNightSystem", false, false);
			Scribe_Values.Look<bool>(ref this.showExpandingIcons, "showExpandingIcons", false, false);
			Scribe_Values.Look<bool>(ref this.showWorldFeatures, "showWorldFeatures", false, false);
			Scribe_Values.Look<bool>(ref this.useWorkPriorities, "useWorkPriorities", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyHumanlike, "defaultCareForHumanlikeColonists", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyAnimal, "defaultCareForAnimalColonists", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonyPrisoner, "defaultCareForHumanlikeColonistPrisoners", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralFaction, "defaultCareForHumanlikeNeutrals", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralAnimal, "defaultCareForAnimalNeutrals", MedicalCareCategory.NoCare, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForHostileFaction, "defaultCareForHumanlikeEnemies", MedicalCareCategory.NoCare, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.PlaySettingsLoadingVars(this);
			}
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x0022BE88 File Offset: 0x0022A288
		public void DoPlaySettingsGlobalControls(WidgetRow row, bool worldView)
		{
			bool flag = this.showColonistBar;
			if (worldView)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				}
				bool flag2 = this.lockNorthUp;
				row.ToggleableIcon(ref this.lockNorthUp, TexButton.LockNorthUp, "LockNorthUpToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				if (flag2 != this.lockNorthUp && this.lockNorthUp)
				{
					Find.WorldCameraDriver.RotateSoNorthIsUp(true);
				}
				row.ToggleableIcon(ref this.usePlanetDayNightSystem, TexButton.UsePlanetDayNightSystem, "UsePlanetDayNightSystemToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showExpandingIcons, TexButton.ShowExpandingIcons, "ShowExpandingIconsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showWorldFeatures, TexButton.ShowWorldFeatures, "ShowWorldFeaturesToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			else
			{
				row.ToggleableIcon(ref this.showLearningHelper, TexButton.ShowLearningHelper, "ShowLearningHelperWhenEmptyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showZones, TexButton.ShowZones, "ZoneVisibilityToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showBeauty, TexButton.ShowBeauty, "ShowBeautyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				this.CheckKeyBindingToggle(KeyBindingDefOf.ToggleBeautyDisplay, ref this.showBeauty);
				row.ToggleableIcon(ref this.showRoomStats, TexButton.ShowRoomStats, "ShowRoomStatsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, "InspectRoomStats");
				this.CheckKeyBindingToggle(KeyBindingDefOf.ToggleRoomStatsDisplay, ref this.showRoomStats);
				row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.showRoofOverlay, TexButton.ShowRoofOverlay, "ShowRoofOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.autoHomeArea, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				row.ToggleableIcon(ref this.autoRebuild, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				bool resourceReadoutCategorized = Prefs.ResourceReadoutCategorized;
				bool flag3 = resourceReadoutCategorized;
				row.ToggleableIcon(ref resourceReadoutCategorized, TexButton.CategorizedResourceReadout, "CategorizedResourceReadoutToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
				if (resourceReadoutCategorized != flag3)
				{
					Prefs.ResourceReadoutCategorized = resourceReadoutCategorized;
				}
			}
			if (flag != this.showColonistBar)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x0022C0FB File Offset: 0x0022A4FB
		private void CheckKeyBindingToggle(KeyBindingDef keyBinding, ref bool value)
		{
			if (keyBinding.KeyDownEvent)
			{
				value = !value;
				if (value)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}
	}
}
