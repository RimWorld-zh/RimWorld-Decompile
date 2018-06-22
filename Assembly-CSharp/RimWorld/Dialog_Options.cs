using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200080A RID: 2058
	public class Dialog_Options : Window
	{
		// Token: 0x06002DF2 RID: 11762 RVA: 0x001830D7 File Offset: 0x001814D7
		public Dialog_Options()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002DF3 RID: 11763 RVA: 0x001830FC File Offset: 0x001814FC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x00183120 File Offset: 0x00181520
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			rect.yMax -= 35f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = (rect.width - 34f) / 3f;
			listing_Standard.Begin(rect);
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Audiovisuals".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			listing_Standard.Label("GameVolume".Translate(), -1f, null);
			Prefs.VolumeGame = listing_Standard.Slider(Prefs.VolumeGame, 0f, 1f);
			listing_Standard.Label("MusicVolume".Translate(), -1f, null);
			Prefs.VolumeMusic = listing_Standard.Slider(Prefs.VolumeMusic, 0f, 1f);
			listing_Standard.Label("AmbientVolume".Translate(), -1f, null);
			Prefs.VolumeAmbient = listing_Standard.Slider(Prefs.VolumeAmbient, 0f, 1f);
			if (listing_Standard.ButtonTextLabeled("Resolution".Translate(), Dialog_Options.ResToString(Screen.width, Screen.height)))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator<Resolution> enumerator = (from x in UnityGUIBugsFixer.ScreenResolutionsWithoutDuplicates
				where x.width >= 1024 && x.height >= 768
				orderby x.width, x.height
				select x).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Resolution res = enumerator.Current;
						list.Add(new FloatMenuOption(Dialog_Options.ResToString(res.width, res.height), delegate()
						{
							if (!ResolutionUtility.UIScaleSafeWithResolution(Prefs.UIScale, res.width, res.height))
							{
								Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								ResolutionUtility.SafeSetResolution(res);
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (listing_Standard.ButtonTextLabeled("UIScale".Translate(), Prefs.UIScale.ToString() + "x"))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				for (int i = 0; i < Dialog_Options.UIScales.Length; i++)
				{
					float scale = Dialog_Options.UIScales[i];
					list2.Add(new FloatMenuOption(Dialog_Options.UIScales[i].ToString() + "x", delegate()
					{
						if (scale != 1f && !ResolutionUtility.UIScaleSafeWithResolution(scale, Screen.width, Screen.height))
						{
							Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
						}
						else
						{
							ResolutionUtility.SafeSetUIScale(scale);
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			bool customCursorEnabled = Prefs.CustomCursorEnabled;
			listing_Standard.CheckboxLabeled("CustomCursor".Translate(), ref customCursorEnabled, null);
			Prefs.CustomCursorEnabled = customCursorEnabled;
			bool fullScreen = Screen.fullScreen;
			bool flag = fullScreen;
			listing_Standard.CheckboxLabeled("Fullscreen".Translate(), ref fullScreen, null);
			if (fullScreen != flag)
			{
				ResolutionUtility.SafeSetFullscreen(fullScreen);
			}
			listing_Standard.Gap(12f);
			bool hatsOnlyOnMap = Prefs.HatsOnlyOnMap;
			listing_Standard.CheckboxLabeled("HatsShownOnlyOnMap".Translate(), ref hatsOnlyOnMap, null);
			if (hatsOnlyOnMap != Prefs.HatsOnlyOnMap)
			{
				PortraitsCache.Clear();
			}
			Prefs.HatsOnlyOnMap = hatsOnlyOnMap;
			bool plantWindSway = Prefs.PlantWindSway;
			listing_Standard.CheckboxLabeled("PlantWindSway".Translate(), ref plantWindSway, null);
			Prefs.PlantWindSway = plantWindSway;
			bool showRealtimeClock = Prefs.ShowRealtimeClock;
			listing_Standard.CheckboxLabeled("ShowRealtimeClock".Translate(), ref showRealtimeClock, null);
			Prefs.ShowRealtimeClock = showRealtimeClock;
			if (listing_Standard.ButtonTextLabeled("ShowAnimalNames".Translate(), Prefs.AnimalNameMode.ToStringHuman()))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				IEnumerator enumerator2 = Enum.GetValues(typeof(AnimalNameDisplayMode)).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj = enumerator2.Current;
						AnimalNameDisplayMode localMode2 = (AnimalNameDisplayMode)obj;
						AnimalNameDisplayMode localMode = localMode2;
						list3.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
						{
							Prefs.AnimalNameMode = localMode;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Gameplay".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			if (listing_Standard.ButtonText("KeyboardConfig".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_KeyBindings());
			}
			if (listing_Standard.ButtonText("ChooseLanguage".Translate(), null))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Messages.Message("ChangeLanguageFromMainMenu".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					List<FloatMenuOption> list4 = new List<FloatMenuOption>();
					foreach (LoadedLanguage localLang2 in LanguageDatabase.AllLoadedLanguages)
					{
						LoadedLanguage localLang = localLang2;
						list4.Add(new FloatMenuOption(localLang.FriendlyNameNative, delegate()
						{
							LanguageDatabase.SelectLanguage(localLang);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list4));
				}
			}
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				if (listing_Standard.ButtonText("OpenSaveGameDataFolder".Translate(), null))
				{
					Application.OpenURL(GenFilePaths.SaveDataFolderPath);
				}
			}
			else if (listing_Standard.ButtonText("ShowSaveGameDataLocation".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_MessageBox(Path.GetFullPath(GenFilePaths.SaveDataFolderPath), null, null, null, null, null, false, null, null));
			}
			if (listing_Standard.ButtonText("ResetAdaptiveTutor".Translate(), null))
			{
				Messages.Message("AdaptiveTutorIsReset".Translate(), MessageTypeDefOf.TaskCompletion, false);
				PlayerKnowledgeDatabase.ResetPersistent();
			}
			bool adaptiveTrainingEnabled = Prefs.AdaptiveTrainingEnabled;
			listing_Standard.CheckboxLabeled("LearningHelper".Translate(), ref adaptiveTrainingEnabled, null);
			Prefs.AdaptiveTrainingEnabled = adaptiveTrainingEnabled;
			bool runInBackground = Prefs.RunInBackground;
			listing_Standard.CheckboxLabeled("RunInBackground".Translate(), ref runInBackground, null);
			Prefs.RunInBackground = runInBackground;
			bool edgeScreenScroll = Prefs.EdgeScreenScroll;
			listing_Standard.CheckboxLabeled("EdgeScreenScroll".Translate(), ref edgeScreenScroll, null);
			Prefs.EdgeScreenScroll = edgeScreenScroll;
			bool pauseOnLoad = Prefs.PauseOnLoad;
			listing_Standard.CheckboxLabeled("PauseOnLoad".Translate(), ref pauseOnLoad, null);
			Prefs.PauseOnLoad = pauseOnLoad;
			bool pauseOnUrgentLetter = Prefs.PauseOnUrgentLetter;
			listing_Standard.CheckboxLabeled("PauseOnUrgentLetter".Translate(), ref pauseOnUrgentLetter, null);
			Prefs.PauseOnUrgentLetter = pauseOnUrgentLetter;
			int maxNumberOfPlayerHomes = Prefs.MaxNumberOfPlayerHomes;
			listing_Standard.Label("MaxNumberOfPlayerHomes".Translate(new object[]
			{
				maxNumberOfPlayerHomes
			}), -1f, null);
			int num = Mathf.RoundToInt(listing_Standard.Slider((float)maxNumberOfPlayerHomes, 1f, 5f));
			Prefs.MaxNumberOfPlayerHomes = num;
			if (maxNumberOfPlayerHomes != num && num > 1)
			{
				TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.MaxNumberOfPlayerHomes);
			}
			if (listing_Standard.ButtonTextLabeled("TemperatureMode".Translate(), Prefs.TemperatureMode.ToStringHuman()))
			{
				List<FloatMenuOption> list5 = new List<FloatMenuOption>();
				IEnumerator enumerator4 = Enum.GetValues(typeof(TemperatureDisplayMode)).GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						object obj2 = enumerator4.Current;
						TemperatureDisplayMode localTmode2 = (TemperatureDisplayMode)obj2;
						TemperatureDisplayMode localTmode = localTmode2;
						list5.Add(new FloatMenuOption(localTmode.ToString(), delegate()
						{
							Prefs.TemperatureMode = localTmode;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator4 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				Find.WindowStack.Add(new FloatMenu(list5));
			}
			float autosaveIntervalDays = Prefs.AutosaveIntervalDays;
			string text = "Days".Translate();
			string text2 = "Day".Translate().ToLower();
			if (listing_Standard.ButtonTextLabeled("AutosaveInterval".Translate(), autosaveIntervalDays + " " + ((autosaveIntervalDays != 1f) ? text : text2)))
			{
				List<FloatMenuOption> list6 = new List<FloatMenuOption>();
				if (Prefs.DevMode)
				{
					list6.Add(new FloatMenuOption("0.125 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.125f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					list6.Add(new FloatMenuOption("0.25 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.25f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				list6.Add(new FloatMenuOption("0.5 " + text + "", delegate()
				{
					Prefs.AutosaveIntervalDays = 0.5f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list6.Add(new FloatMenuOption(1.ToString() + " " + text2, delegate()
				{
					Prefs.AutosaveIntervalDays = 1f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list6.Add(new FloatMenuOption(3.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 3f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list6.Add(new FloatMenuOption(7.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 7f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list6.Add(new FloatMenuOption(14.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 14f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list6));
			}
			if (Current.ProgramState == ProgramState.Playing && Current.Game.Info.permadeathMode && Prefs.AutosaveIntervalDays > 1f)
			{
				GUI.color = Color.red;
				listing_Standard.Label("MaxPermadeathAutosaveIntervalInfo".Translate(new object[]
				{
					1f
				}), -1f, null);
				GUI.color = Color.white;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (listing_Standard.ButtonText("ChangeStoryteller".Translate(), "OptionsButton-ChooseStoryteller"))
				{
					if (TutorSystem.AllowAction("ChooseStoryteller"))
					{
						Find.WindowStack.Add(new Page_SelectStorytellerInGame());
					}
				}
			}
			if (!DevModePermanentlyDisabledUtility.Disabled)
			{
				if (listing_Standard.ButtonText("PermanentlyDisableDevMode".Translate(), null))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmPermanentlyDisableDevMode".Translate(), delegate
					{
						DevModePermanentlyDisabledUtility.Disable();
					}, true, null));
				}
			}
			if (!DevModePermanentlyDisabledUtility.Disabled || Prefs.DevMode)
			{
				bool devMode = Prefs.DevMode;
				listing_Standard.CheckboxLabeled("DevelopmentMode".Translate(), ref devMode, null);
				Prefs.DevMode = devMode;
			}
			bool testMapSizes = Prefs.TestMapSizes;
			listing_Standard.CheckboxLabeled("EnableTestMapSizes".Translate(), ref testMapSizes, null);
			Prefs.TestMapSizes = testMapSizes;
			if (Prefs.DevMode)
			{
				bool resetModsConfigOnCrash = Prefs.ResetModsConfigOnCrash;
				listing_Standard.CheckboxLabeled("ResetModsConfigOnCrash".Translate(), ref resetModsConfigOnCrash, null);
				Prefs.ResetModsConfigOnCrash = resetModsConfigOnCrash;
				bool logVerbose = Prefs.LogVerbose;
				listing_Standard.CheckboxLabeled("LogVerbose".Translate(), ref logVerbose, null);
				Prefs.LogVerbose = logVerbose;
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label("", -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			if (listing_Standard.ButtonText("ModSettings".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_ModSettings());
			}
			listing_Standard.Label("", -1f, null);
			listing_Standard.Label("NamesYouWantToSee".Translate(), -1f, null);
			Prefs.PreferredNames.RemoveAll((string n) => n.NullOrEmpty());
			for (int j = 0; j < Prefs.PreferredNames.Count; j++)
			{
				string name = Prefs.PreferredNames[j];
				PawnBio pawnBio = (from b in SolidBioDatabase.allBios
				where b.name.ToString() == name
				select b).FirstOrDefault<PawnBio>();
				if (pawnBio == null)
				{
					name += " [N]";
				}
				else
				{
					PawnBioType bioType = pawnBio.BioType;
					if (bioType != PawnBioType.BackstoryInGame)
					{
						if (bioType == PawnBioType.PirateKing)
						{
							name += " [PK]";
						}
					}
					else
					{
						name += " [B]";
					}
				}
				Rect rect2 = listing_Standard.GetRect(24f);
				Widgets.Label(rect2, name);
				Rect butRect = new Rect(rect2.xMax - 24f, rect2.y, 24f, 24f);
				if (Widgets.ButtonImage(butRect, TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor))
				{
					Prefs.PreferredNames.RemoveAt(j);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (Prefs.PreferredNames.Count < 6)
			{
				if (listing_Standard.ButtonText("AddName".Translate() + "...", null))
				{
					Find.WindowStack.Add(new Dialog_AddPreferredName());
				}
			}
			listing_Standard.Label("", -1f, null);
			if (listing_Standard.ButtonText("RestoreToDefaultSettings".Translate(), null))
			{
				this.RestoreToDefaultSettings();
			}
			listing_Standard.End();
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x001840AC File Offset: 0x001824AC
		public override void PreClose()
		{
			base.PreClose();
			Prefs.Save();
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x001840BC File Offset: 0x001824BC
		public static string ResToString(int width, int height)
		{
			string text = width + "x" + height;
			if (width == 1280 && height == 720)
			{
				text += " (720p)";
			}
			if (width == 1920 && height == 1080)
			{
				text += " (1080p)";
			}
			return text;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x00184130 File Offset: 0x00182530
		public void RestoreToDefaultSettings()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ConfigFolderPath);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml"))
			{
				try
				{
					fileInfo.Delete();
				}
				catch (SystemException)
				{
				}
			}
			Find.WindowStack.Add(new Dialog_MessageBox("ResetAndRestart".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		// Token: 0x04001861 RID: 6241
		private const float SubOptionTabWidth = 40f;

		// Token: 0x04001862 RID: 6242
		private static readonly float[] UIScales = new float[]
		{
			1f,
			1.25f,
			1.5f,
			1.75f,
			2f,
			2.5f,
			3f,
			3.5f,
			4f
		};
	}
}
