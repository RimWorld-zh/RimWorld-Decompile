using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E26 RID: 3622
	public class DebugWindowsOpener
	{
		// Token: 0x060054DC RID: 21724 RVA: 0x002B835C File Offset: 0x002B675C
		public void DevToolStarterOnGUI()
		{
			if (Prefs.DevMode)
			{
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
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (KeyBindingDefOf.Dev_ToggleGodMode.KeyDownEvent)
					{
						this.ToggleGodMode();
						Event.current.Use();
					}
				}
			}
		}

		// Token: 0x060054DD RID: 21725 RVA: 0x002B84E8 File Offset: 0x002B68E8
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

		// Token: 0x060054DE RID: 21726 RVA: 0x002B867E File Offset: 0x002B6A7E
		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x002B86AC File Offset: 0x002B6AAC
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

		// Token: 0x060054E0 RID: 21728 RVA: 0x002B8742 File Offset: 0x002B6B42
		private void ToggleDebugSettingsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugSettingsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugSettingsMenu());
			}
		}

		// Token: 0x060054E1 RID: 21729 RVA: 0x002B876E File Offset: 0x002B6B6E
		private void ToggleDebugActionsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugActionsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugActionsMenu());
			}
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x002B879A File Offset: 0x002B6B9A
		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x002B87C6 File Offset: 0x002B6BC6
		private void ToggleDebugLogMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugOutputMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugOutputMenu());
			}
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x002B87F2 File Offset: 0x002B6BF2
		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x002B881E File Offset: 0x002B6C1E
		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
		}
	}
}
