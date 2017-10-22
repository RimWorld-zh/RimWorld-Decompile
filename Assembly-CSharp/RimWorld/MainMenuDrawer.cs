using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class MainMenuDrawer
	{
		private static bool anyMapFiles;

		private const float PlayRectWidth = 170f;

		private const float WebRectWidth = 145f;

		private const float RightEdgeMargin = 50f;

		private static readonly Vector2 PaneSize = new Vector2(450f, 450f);

		private static readonly Vector2 TitleSize = new Vector2(1032f, 146f);

		private static readonly Texture2D TexTitle = ContentFinder<Texture2D>.Get("UI/HeroArt/GameTitle", true);

		private const float TitleShift = 50f;

		private static readonly Vector2 LudeonLogoSize = new Vector2(200f, 58f);

		private static readonly Texture2D TexLudeonLogo = ContentFinder<Texture2D>.Get("UI/HeroArt/LudeonLogoSmall", true);

		public static void Init()
		{
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			MainMenuDrawer.anyMapFiles = GenFilePaths.AllSavedGameFiles.Any();
		}

		public static void MainMenuOnGUI()
		{
			VersionControl.DrawInfoInCorner();
			float num = (float)(UI.screenWidth / 2);
			Vector2 paneSize = MainMenuDrawer.PaneSize;
			double x = num - paneSize.x / 2.0;
			float num2 = (float)(UI.screenHeight / 2);
			Vector2 paneSize2 = MainMenuDrawer.PaneSize;
			double y = num2 - paneSize2.y / 2.0 + 50.0;
			Vector2 paneSize3 = MainMenuDrawer.PaneSize;
			float x2 = paneSize3.x;
			Vector2 paneSize4 = MainMenuDrawer.PaneSize;
			Rect rect = new Rect((float)x, (float)y, x2, paneSize4.y);
			rect.x = (float)((float)UI.screenWidth - rect.width - 30.0);
			Rect rect2 = new Rect(0f, (float)(rect.y - 30.0), (float)((float)UI.screenWidth - 85.0), 30f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperRight;
			string text = "MainPageCredit".Translate();
			if (UI.screenWidth < 990)
			{
				Rect position = rect2;
				float xMax = position.xMax;
				Vector2 vector = Text.CalcSize(text);
				position.xMin = xMax - vector.x;
				position.xMin -= 4f;
				position.xMax += 4f;
				GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
				GUI.DrawTexture(position, BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			Widgets.Label(rect2, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			Vector2 a = MainMenuDrawer.TitleSize;
			if (a.x > (float)UI.screenWidth)
			{
				a *= (float)UI.screenWidth / a.x;
			}
			a *= 0.7f;
			Rect position2 = new Rect((float)((float)UI.screenWidth - a.x - 50.0), rect2.y - a.y, a.x, a.y);
			GUI.DrawTexture(position2, MainMenuDrawer.TexTitle, ScaleMode.StretchToFill, true);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			float num3 = (float)(UI.screenWidth - 8);
			Vector2 ludeonLogoSize = MainMenuDrawer.LudeonLogoSize;
			float x3 = num3 - ludeonLogoSize.x;
			Vector2 ludeonLogoSize2 = MainMenuDrawer.LudeonLogoSize;
			float x4 = ludeonLogoSize2.x;
			Vector2 ludeonLogoSize3 = MainMenuDrawer.LudeonLogoSize;
			Rect position3 = new Rect(x3, 8f, x4, ludeonLogoSize3.y);
			GUI.DrawTexture(position3, MainMenuDrawer.TexLudeonLogo, ScaleMode.StretchToFill, true);
			GUI.color = Color.white;
			rect.yMin += 17f;
			MainMenuDrawer.DoMainMenuControls(rect, MainMenuDrawer.anyMapFiles);
		}

		public static void DoMainMenuControls(Rect rect, bool anyMapFiles)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 170f, rect.height);
			Rect rect3 = new Rect((float)(rect2.xMax + 17.0), 0f, 145f, rect.height);
			Text.Font = GameFont.Small;
			List<ListableOption> list = new List<ListableOption>();
			if (Current.ProgramState == ProgramState.Entry)
			{
				string label = "Tutorial".CanTranslate() ? "Tutorial".Translate() : "LearnToPlay".Translate();
				list.Add(new ListableOption(label, (Action)delegate
				{
					MainMenuDrawer.InitLearnToPlay();
				}, (string)null));
				list.Add(new ListableOption("NewColony".Translate(), (Action)delegate
				{
					Find.WindowStack.Add(new Page_SelectScenario());
				}, (string)null));
			}
			if (Current.ProgramState == ProgramState.Playing && !Current.Game.Info.permadeathMode)
			{
				list.Add(new ListableOption("Save".Translate(), (Action)delegate
				{
					MainMenuDrawer.CloseMainTab();
					Find.WindowStack.Add(new Dialog_SaveFileList_Save());
				}, (string)null));
			}
			ListableOption item;
			if (anyMapFiles && (Current.ProgramState != ProgramState.Playing || !Current.Game.Info.permadeathMode))
			{
				item = new ListableOption("LoadGame".Translate(), (Action)delegate
				{
					MainMenuDrawer.CloseMainTab();
					Find.WindowStack.Add(new Dialog_SaveFileList_Load());
				}, (string)null);
				list.Add(item);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				list.Add(new ListableOption("ReviewScenario".Translate(), (Action)delegate
				{
					WindowStack windowStack = Find.WindowStack;
					string fullInformationText = Find.Scenario.GetFullInformationText();
					string name = Find.Scenario.name;
					windowStack.Add(new Dialog_MessageBox(fullInformationText, (string)null, null, (string)null, null, name, false));
				}, (string)null));
			}
			item = new ListableOption("Options".Translate(), (Action)delegate
			{
				MainMenuDrawer.CloseMainTab();
				Find.WindowStack.Add(new Dialog_Options());
			}, "MenuButton-Options");
			list.Add(item);
			if (Current.ProgramState == ProgramState.Entry)
			{
				item = new ListableOption("Mods".Translate(), (Action)delegate
				{
					Find.WindowStack.Add(new Page_ModsConfig());
				}, (string)null);
				list.Add(item);
				item = new ListableOption("Credits".Translate(), (Action)delegate
				{
					Find.WindowStack.Add(new Screen_Credits());
				}, (string)null);
				list.Add(item);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Current.Game.Info.permadeathMode)
				{
					item = new ListableOption("SaveAndQuitToMainMenu".Translate(), (Action)delegate
					{
						LongEventHandler.QueueLongEvent((Action)delegate
						{
							GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
							MemoryUtility.ClearAllMapsAndWorld();
						}, "Entry", "SavingLongEvent", false, null);
					}, (string)null);
					list.Add(item);
					item = new ListableOption("SaveAndQuitToOS".Translate(), (Action)delegate
					{
						LongEventHandler.QueueLongEvent((Action)delegate
						{
							GameDataSaveLoader.SaveGame(Current.Game.Info.permadeathModeUniqueName);
							LongEventHandler.ExecuteWhenFinished((Action)delegate
							{
								Root.Shutdown();
							});
						}, "SavingLongEvent", false, null);
					}, (string)null);
					list.Add(item);
				}
				else
				{
					Action action = (Action)delegate
					{
						if (GameDataSaveLoader.CurrentGameStateIsValuable)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), (Action)delegate
							{
								GenScene.GoToMainMenu();
							}, true, (string)null));
						}
						else
						{
							GenScene.GoToMainMenu();
						}
					};
					item = new ListableOption("QuitToMainMenu".Translate(), action, (string)null);
					list.Add(item);
					Action action2 = (Action)delegate
					{
						if (GameDataSaveLoader.CurrentGameStateIsValuable)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmQuit".Translate(), (Action)delegate
							{
								Root.Shutdown();
							}, true, (string)null));
						}
						else
						{
							Root.Shutdown();
						}
					};
					item = new ListableOption("QuitToOS".Translate(), action2, (string)null);
					list.Add(item);
				}
			}
			else
			{
				item = new ListableOption("QuitToOS".Translate(), (Action)delegate
				{
					Root.Shutdown();
				}, (string)null);
				list.Add(item);
			}
			OptionListingUtility.DrawOptionListing(rect2, list);
			Text.Font = GameFont.Small;
			List<ListableOption> list2 = new List<ListableOption>();
			ListableOption item2 = new ListableOption_WebLink("FictionPrimer".Translate(), "http://rimworldgame.com/backstory", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("LudeonBlog".Translate(), "http://ludeon.com/blog", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("Forums".Translate(), "http://ludeon.com/forums", TexButton.IconForums);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("OfficialWiki".Translate(), "http://rimworldwiki.com", TexButton.IconBlog);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("TynansTwitter".Translate(), "https://twitter.com/TynanSylvester", TexButton.IconTwitter);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("TynansDesignBook".Translate(), "http://tynansylvester.com/book", TexButton.IconBook);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("HelpTranslate".Translate(), "http://ludeon.com/forums/index.php?topic=2933.0", TexButton.IconForums);
			list2.Add(item2);
			item2 = new ListableOption_WebLink("BuySoundtrack".Translate(), "http://www.lasgameaudio.co.uk/#!store/t04fw", TexButton.IconSoundtrack);
			list2.Add(item2);
			float num = OptionListingUtility.DrawOptionListing(rect3, list2);
			GUI.BeginGroup(rect3);
			if (Current.ProgramState == ProgramState.Entry && Widgets.ButtonImage(new Rect(0f, (float)(num + 10.0), 64f, 32f), LanguageDatabase.activeLanguage.icon))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				foreach (LoadedLanguage allLoadedLanguage in LanguageDatabase.AllLoadedLanguages)
				{
					LoadedLanguage localLang = allLoadedLanguage;
					list3.Add(new FloatMenuOption(localLang.FriendlyNameNative, (Action)delegate
					{
						LanguageDatabase.SelectLanguage(localLang);
						Prefs.Save();
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			GUI.EndGroup();
			GUI.EndGroup();
		}

		private static void InitLearnToPlay()
		{
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Tutor, DifficultyDefOf.VeryEasy);
			Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();
			Page next = firstConfigPage.next;
			next.prev = null;
			Find.WindowStack.Add(next);
		}

		private static void CloseMainTab()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
			}
		}
	}
}
