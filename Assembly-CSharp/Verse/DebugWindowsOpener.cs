﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class DebugWindowsOpener
	{
		[CompilerGenerated]
		private static Action <>f__am$cache0;

		[CompilerGenerated]
		private static Action <>f__am$cache1;

		public DebugWindowsOpener()
		{
		}

		public void DevToolStarterOnGUI()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f, 3f);
			int num = 6;
			if (Current.ProgramState == ProgramState.Playing)
			{
				num += 2;
			}
			float num2 = 25f;
			if (Current.ProgramState == ProgramState.Playing && DebugSettings.godMode)
			{
				num2 += 15f;
			}
			Find.WindowStack.ImmediateWindow(1593759361, new Rect(vector.x, vector.y, (float)num * 28f - 4f + 1f, num2).Rounded(), WindowLayer.GameUI, delegate
			{
				this.DrawButtons();
			}, false, false, 0f);
			if (KeyBindingDefOf.Dev_ToggleDebugLog.KeyDownEvent)
			{
				this.ToggleLogWindow();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				this.ToggleDebugActionsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugLogMenu.KeyDownEvent)
			{
				this.ToggleDebugLogMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				this.ToggleDebugSettingsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				this.ToggleDebugInspector();
				Event.current.Use();
			}
			if (Current.ProgramState == ProgramState.Playing && KeyBindingDefOf.Dev_ToggleGodMode.KeyDownEvent)
			{
				this.ToggleGodMode();
				Event.current.Use();
			}
		}

		private void DrawButtons()
		{
			WidgetRow widgetRow = new WidgetRow();
			if (widgetRow.ButtonIcon(TexButton.ToggleLog, "Open the debug log.", null))
			{
				this.ToggleLogWindow();
			}
			if (widgetRow.ButtonIcon(TexButton.ToggleTweak, "Open tweakvalues menu.\n\nThis lets you change internal values.", null))
			{
				this.ToggleTweakValuesMenu();
			}
			if (widgetRow.ButtonIcon(TexButton.OpenPackageEditor, "Open the package editor.\n\nThis lets you edit game data while the game is running.", null))
			{
				this.OpenPackageEditor();
			}
			if (widgetRow.ButtonIcon(TexButton.OpenInspectSettings, "Open the view settings.\n\nThis lets you see special debug visuals.", null))
			{
				this.ToggleDebugSettingsMenu();
			}
			if (widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug actions menu.\n\nThis lets you spawn items and force various events.", null))
			{
				this.ToggleDebugActionsMenu();
			}
			if (widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug logging menu.", null))
			{
				this.ToggleDebugLogMenu();
			}
			if (widgetRow.ButtonIcon(TexButton.OpenInspector, "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables.", null))
			{
				this.ToggleDebugInspector();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (widgetRow.ButtonIcon(TexButton.ToggleGodMode, "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours.", null))
				{
					this.ToggleGodMode();
				}
				if (DebugSettings.godMode)
				{
					Text.Font = GameFont.Tiny;
					Widgets.Label(new Rect(0f, 25f, 200f, 100f), "God mode");
				}
				bool pauseOnError = Prefs.PauseOnError;
				widgetRow.ToggleableIcon(ref pauseOnError, TexButton.TogglePauseOnError, "Pause the game when an error is logged.", null, null);
				Prefs.PauseOnError = pauseOnError;
			}
		}

		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		private void OpenPackageEditor()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			FloatMenuOption item = new FloatMenuOption("SoundDefs", delegate()
			{
				Find.WindowStack.Add(new EditWindow_PackageEditor<SoundDef>("SoundDefs"));
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			list.Add(item);
			item = new FloatMenuOption("HairDefs", delegate()
			{
				Find.WindowStack.Add(new EditWindow_PackageEditor<HairDef>("HairDefs"));
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			list.Add(item);
			Find.WindowStack.Add(new FloatMenu(list));
		}

		private void ToggleDebugSettingsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugSettingsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugSettingsMenu());
			}
		}

		private void ToggleDebugActionsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugActionsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugActionsMenu());
			}
		}

		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		private void ToggleDebugLogMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugOutputMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugOutputMenu());
			}
		}

		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
		}

		[CompilerGenerated]
		private void <DevToolStarterOnGUI>m__0()
		{
			this.DrawButtons();
		}

		[CompilerGenerated]
		private static void <OpenPackageEditor>m__1()
		{
			Find.WindowStack.Add(new EditWindow_PackageEditor<SoundDef>("SoundDefs"));
		}

		[CompilerGenerated]
		private static void <OpenPackageEditor>m__2()
		{
			Find.WindowStack.Add(new EditWindow_PackageEditor<HairDef>("HairDefs"));
		}
	}
}
