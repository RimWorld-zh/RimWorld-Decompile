using RimWorld;
using RimWorld.Planet;
using System;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	public class UIRoot_Entry : UIRoot
	{
		private bool ShouldDoMainMenu
		{
			get
			{
				bool result;
				if (LongEventHandler.AnyEventNowOrWaiting)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < Find.WindowStack.Count; i++)
					{
						if (base.windows[i].layer == WindowLayer.Dialog && !Find.WindowStack[i].IsDebug)
							goto IL_0046;
					}
					result = true;
				}
				goto IL_0069;
				IL_0069:
				return result;
				IL_0046:
				result = false;
				goto IL_0069;
			}
		}

		public override void Init()
		{
			base.Init();
			UIMenuBackgroundManager.background = new UI_BackgroundMain();
			MainMenuDrawer.Init();
			QuickStarter.CheckQuickStart();
			VersionUpdateDialogMaker.CreateVersionUpdateDialogIfNecessary();
			if (!SteamManager.Initialized)
			{
				string text = "SteamClientMissing".Translate();
				Dialog_MessageBox window = new Dialog_MessageBox(text, "Quit".Translate(), (Action)delegate
				{
					Application.Quit();
				}, "Ignore".Translate(), null, (string)null, false);
				Find.WindowStack.Add(window);
			}
		}

		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceOnGUI();
			}
			this.DoMainMenu();
			if (Current.Game != null)
			{
				Find.Tutor.TutorOnGUI();
			}
			base.windows.WindowStackOnGUI();
			ReorderableWidget.ReorderableWidgetOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.HandleLowPriorityInput();
			}
		}

		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceUpdate();
			}
			if (Current.Game != null)
			{
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
		}

		private void DoMainMenu()
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				if (this.ShouldDoMainMenu)
				{
					Current.Game = null;
					MainMenuDrawer.MainMenuOnGUI();
				}
			}
		}
	}
}
