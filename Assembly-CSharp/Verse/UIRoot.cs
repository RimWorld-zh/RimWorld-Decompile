using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E97 RID: 3735
	public abstract class UIRoot
	{
		// Token: 0x04003A50 RID: 14928
		public WindowStack windows = new WindowStack();

		// Token: 0x04003A51 RID: 14929
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x04003A52 RID: 14930
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x04003A53 RID: 14931
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x04003A54 RID: 14932
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();

		// Token: 0x06005822 RID: 22562 RVA: 0x001BB0AF File Offset: 0x001B94AF
		public virtual void Init()
		{
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x001BB0B4 File Offset: 0x001B94B4
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

		// Token: 0x06005824 RID: 22564 RVA: 0x001BB15E File Offset: 0x001B955E
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x001BB185 File Offset: 0x001B9585
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}
	}
}
