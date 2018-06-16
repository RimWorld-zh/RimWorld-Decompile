using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E96 RID: 3734
	public abstract class UIRoot
	{
		// Token: 0x06005800 RID: 22528 RVA: 0x001BA9EB File Offset: 0x001B8DEB
		public virtual void Init()
		{
		}

		// Token: 0x06005801 RID: 22529 RVA: 0x001BA9F0 File Offset: 0x001B8DF0
		public virtual void UIRootOnGUI()
		{
			UnityGUIBugsFixer.OnGUI();
			Text.StartOfOnGUI();
			this.CheckOpenLogWindow();
			DelayedErrorWindowRequest.DelayedErrorWindowRequestOnGUI();
			DebugInputLogger.InputLogOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.debugWindowOpener.DevToolStarterOnGUI();
			}
			this.windows.HandleEventsHighPriority();
			this.screenshotMode.ScreenshotModesOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				TooltipHandler.DoTooltipGUI();
				this.feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				Messages.MessagesDoGUI();
			}
			this.shortcutKeys.ShortcutKeysOnGUI();
			NoiseDebugUI.NoiseDebugOnGUI();
			Debug.developerConsoleVisible = false;
			if (Current.Game != null)
			{
				GameComponentUtility.GameComponentOnGUI();
			}
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x001BAA9A File Offset: 0x001B8E9A
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x001BAAC1 File Offset: 0x001B8EC1
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x04003A3A RID: 14906
		public WindowStack windows = new WindowStack();

		// Token: 0x04003A3B RID: 14907
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x04003A3C RID: 14908
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x04003A3D RID: 14909
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x04003A3E RID: 14910
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
