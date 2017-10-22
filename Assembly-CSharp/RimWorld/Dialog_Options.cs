using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Dialog_Options : Window
	{
		private const float SubOptionTabWidth = 40f;

		private static readonly float[] UIScales = new float[9]
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

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		public Dialog_Options()
		{
			base.closeOnEscapeKey = true;
			base.doCloseButton = true;
			base.doCloseX = true;
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			rect.yMax -= 45f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = (float)((rect.width - 34.0) / 3.0);
			listing_Standard.Begin(rect);
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Audiovisuals".Translate(), -1f);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			listing_Standard.Label("GameVolume".Translate(), -1f);
			Prefs.VolumeGame = listing_Standard.Slider(Prefs.VolumeGame, 0f, 1f);
			listing_Standard.Label("MusicVolume".Translate(), -1f);
			Prefs.VolumeMusic = listing_Standard.Slider(Prefs.VolumeMusic, 0f, 1f);
			if (listing_Standard.ButtonTextLabeled("Resolution".Translate(), Dialog_Options.ResToString(Screen.width, Screen.height)))
			{
				Find.WindowStack.Add(new Dialog_ResolutionPicker());
			}
			bool customCursorEnabled = Prefs.CustomCursorEnabled;
			listing_Standard.CheckboxLabeled("CustomCursor".Translate(), ref customCursorEnabled, (string)null);
			Prefs.CustomCursorEnabled = customCursorEnabled;
			bool fullScreen;
			bool flag = fullScreen = Screen.fullScreen;
			listing_Standard.CheckboxLabeled("Fullscreen".Translate(), ref flag, (string)null);
			if (flag != fullScreen)
			{
				ResolutionUtility.SafeSetFullscreen(flag);
			}
			listing_Standard.Label("UIScale".Translate(), -1f);
			float[] uIScales = Dialog_Options.UIScales;
			for (int i = 0; i < uIScales.Length; i++)
			{
				float num = uIScales[i];
				if (listing_Standard.RadioButton(num.ToString() + "x", Prefs.UIScale == num, 8f))
				{
					if (!ResolutionUtility.UIScaleSafeWithResolution(num, Screen.width, Screen.height))
					{
						Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageSound.RejectInput);
					}
					else
					{
						ResolutionUtility.SafeSetUIScale(num);
					}
				}
			}
			listing_Standard.Gap(12f);
			bool hatsOnlyOnMap = Prefs.HatsOnlyOnMap;
			listing_Standard.CheckboxLabeled("HatsShownOnlyOnMap".Translate(), ref hatsOnlyOnMap, (string)null);
			if (hatsOnlyOnMap != Prefs.HatsOnlyOnMap)
			{
				PortraitsCache.Clear();
			}
			Prefs.HatsOnlyOnMap = hatsOnlyOnMap;
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Gameplay".Translate(), -1f);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			if (listing_Standard.ButtonText("KeyboardConfig".Translate(), (string)null))
			{
				Find.WindowStack.Add(new Dialog_KeyBindings());
			}
			if (listing_Standard.ButtonText("ChooseLanguage".Translate(), (string)null))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Messages.Message("ChangeLanguageFromMainMenu".Translate(), MessageSound.RejectInput);
				}
				else
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (LoadedLanguage allLoadedLanguage in LanguageDatabase.AllLoadedLanguages)
					{
						LoadedLanguage localLang = allLoadedLanguage;
						list.Add(new FloatMenuOption(localLang.FriendlyNameNative, (Action)delegate
						{
							LanguageDatabase.SelectLanguage(localLang);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) && listing_Standard.ButtonText("OpenSaveGameDataFolder".Translate(), (string)null))
			{
				Application.OpenURL(GenFilePaths.SaveDataFolderPath);
			}
			bool adaptiveTrainingEnabled = Prefs.AdaptiveTrainingEnabled;
			listing_Standard.CheckboxLabeled("LearningHelper".Translate(), ref adaptiveTrainingEnabled, (string)null);
			Prefs.AdaptiveTrainingEnabled = adaptiveTrainingEnabled;
			if (listing_Standard.ButtonText("ResetAdaptiveTutor".Translate(), (string)null))
			{
				Messages.Message("AdaptiveTutorIsReset".Translate(), MessageSound.Benefit);
				PlayerKnowledgeDatabase.ResetPersistent();
			}
			bool runInBackground = Prefs.RunInBackground;
			listing_Standard.CheckboxLabeled("RunInBackground".Translate(), ref runInBackground, (string)null);
			Prefs.RunInBackground = runInBackground;
			bool edgeScreenScroll = Prefs.EdgeScreenScroll;
			listing_Standard.CheckboxLabeled("EdgeScreenScroll".Translate(), ref edgeScreenScroll, (string)null);
			Prefs.EdgeScreenScroll = edgeScreenScroll;
			bool pauseOnLoad = Prefs.PauseOnLoad;
			listing_Standard.CheckboxLabeled("PauseOnLoad".Translate(), ref pauseOnLoad, (string)null);
			Prefs.PauseOnLoad = pauseOnLoad;
			bool pauseOnUrgentLetter = Prefs.PauseOnUrgentLetter;
			listing_Standard.CheckboxLabeled("PauseOnUrgentLetter".Translate(), ref pauseOnUrgentLetter, (string)null);
			Prefs.PauseOnUrgentLetter = pauseOnUrgentLetter;
			bool showRealtimeClock = Prefs.ShowRealtimeClock;
			listing_Standard.CheckboxLabeled("ShowRealtimeClock".Translate(), ref showRealtimeClock, (string)null);
			Prefs.ShowRealtimeClock = showRealtimeClock;
			bool plantWindSway = Prefs.PlantWindSway;
			listing_Standard.CheckboxLabeled("PlantWindSway".Translate(), ref plantWindSway, (string)null);
			Prefs.PlantWindSway = plantWindSway;
			int maxNumberOfPlayerHomes = Prefs.MaxNumberOfPlayerHomes;
			listing_Standard.Label("MaxNumberOfPlayerHomes".Translate(maxNumberOfPlayerHomes), -1f);
			int num2 = Mathf.RoundToInt(listing_Standard.Slider((float)maxNumberOfPlayerHomes, 1f, 5f));
			Prefs.MaxNumberOfPlayerHomes = num2;
			if (maxNumberOfPlayerHomes != num2 && num2 > 1)
			{
				TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.MaxNumberOfPlayerHomes);
			}
			if (listing_Standard.ButtonTextLabeled("TemperatureMode".Translate(), Prefs.TemperatureMode.ToStringHuman()))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (byte value in Enum.GetValues(typeof(TemperatureDisplayMode)))
				{
					TemperatureDisplayMode localTmode = (TemperatureDisplayMode)value;
					list2.Add(new FloatMenuOption(((Enum)(object)localTmode).ToString(), (Action)delegate
					{
						Prefs.TemperatureMode = localTmode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			float autosaveIntervalDays = Prefs.AutosaveIntervalDays;
			string text = "Days".Translate();
			string text2 = "Day".Translate().ToLower();
			if (listing_Standard.ButtonTextLabeled("AutosaveInterval".Translate(), autosaveIntervalDays + " " + ((autosaveIntervalDays != 1.0) ? text : text2)))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				if (Prefs.DevMode)
				{
					list3.Add(new FloatMenuOption("0.125 " + text + "(debug)", (Action)delegate
					{
						Prefs.AutosaveIntervalDays = 0.125f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					list3.Add(new FloatMenuOption("0.25 " + text + "(debug)", (Action)delegate
					{
						Prefs.AutosaveIntervalDays = 0.25f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				list3.Add(new FloatMenuOption("0.5 " + text + string.Empty, (Action)delegate
				{
					Prefs.AutosaveIntervalDays = 0.5f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list3.Add(new FloatMenuOption(1.ToString() + " " + text2, (Action)delegate
				{
					Prefs.AutosaveIntervalDays = 1f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list3.Add(new FloatMenuOption(3.ToString() + " " + text, (Action)delegate
				{
					Prefs.AutosaveIntervalDays = 3f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list3.Add(new FloatMenuOption(7.ToString() + " " + text, (Action)delegate
				{
					Prefs.AutosaveIntervalDays = 7f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list3.Add(new FloatMenuOption(14.ToString() + " " + text, (Action)delegate
				{
					Prefs.AutosaveIntervalDays = 14f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			if (Current.ProgramState == ProgramState.Playing && Current.Game.Info.permadeathMode && Prefs.AutosaveIntervalDays > 1.0)
			{
				GUI.color = Color.red;
				listing_Standard.Label("MaxPermadeathAutosaveIntervalInfo".Translate(1f), -1f);
				GUI.color = Color.white;
			}
			if (Current.ProgramState == ProgramState.Playing && listing_Standard.ButtonText("ChangeStoryteller".Translate(), "OptionsButton-ChooseStoryteller") && TutorSystem.AllowAction("ChooseStoryteller"))
			{
				Find.WindowStack.Add(new Page_SelectStorytellerInGame());
			}
			if (listing_Standard.ButtonTextLabeled("ShowAnimalNames".Translate(), Prefs.AnimalNameMode.ToStringHuman()))
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				foreach (byte value2 in Enum.GetValues(typeof(AnimalNameDisplayMode)))
				{
					AnimalNameDisplayMode localMode = (AnimalNameDisplayMode)value2;
					list4.Add(new FloatMenuOption(localMode.ToStringHuman(), (Action)delegate
					{
						Prefs.AnimalNameMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list4));
			}
			bool devMode = Prefs.DevMode;
			listing_Standard.CheckboxLabeled("DevelopmentMode".Translate(), ref devMode, (string)null);
			Prefs.DevMode = devMode;
			if (Prefs.DevMode)
			{
				bool resetModsConfigOnCrash = Prefs.ResetModsConfigOnCrash;
				listing_Standard.CheckboxLabeled("ResetModsConfigOnCrash".Translate(), ref resetModsConfigOnCrash, (string)null);
				Prefs.ResetModsConfigOnCrash = resetModsConfigOnCrash;
				bool logVerbose = Prefs.LogVerbose;
				listing_Standard.CheckboxLabeled("LogVerbose".Translate(), ref logVerbose, (string)null);
				Prefs.LogVerbose = logVerbose;
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label(string.Empty, -1f);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			if (listing_Standard.ButtonText("ModSettings".Translate(), (string)null))
			{
				Find.WindowStack.Add(new Dialog_ModSettings());
			}
			listing_Standard.Label(string.Empty, -1f);
			listing_Standard.Label("NamesYouWantToSee".Translate(), -1f);
			Prefs.PreferredNames.RemoveAll((Predicate<string>)((string n) => n.NullOrEmpty()));
			for (int j = 0; j < Prefs.PreferredNames.Count; j++)
			{
				string name = Prefs.PreferredNames[j];
				PawnBio pawnBio = (from b in SolidBioDatabase.allBios
				where b.name.ToString() == name
				select b).FirstOrDefault();
				if (pawnBio == null)
				{
					name += " [N]";
				}
				else
				{
					switch (pawnBio.BioType)
					{
					case PawnBioType.BackstoryInGame:
					{
						name += " [B]";
						break;
					}
					case PawnBioType.PirateKing:
					{
						name += " [PK]";
						break;
					}
					}
				}
				Rect rect2 = listing_Standard.GetRect(24f);
				Widgets.Label(rect2, name);
				Rect butRect = new Rect((float)(rect2.xMax - 24.0), rect2.y, 24f, 24f);
				if (Widgets.ButtonImage(butRect, TexButton.DeleteX))
				{
					Prefs.PreferredNames.RemoveAt(j);
					SoundDefOf.TickLow.PlayOneShotOnCamera(null);
				}
			}
			if (Prefs.PreferredNames.Count < 6 && listing_Standard.ButtonText("AddName".Translate() + "...", (string)null))
			{
				Find.WindowStack.Add(new Dialog_AddPreferredName());
			}
			listing_Standard.End();
		}

		public override void PreClose()
		{
			base.PreClose();
			Prefs.Save();
		}

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
	}
}
