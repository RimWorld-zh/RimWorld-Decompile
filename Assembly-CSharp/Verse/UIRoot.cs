using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E94 RID: 3732
	public abstract class UIRoot
	{
		// Token: 0x04003A48 RID: 14920
		public WindowStack windows = new WindowStack();

		// Token: 0x04003A49 RID: 14921
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x04003A4A RID: 14922
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x04003A4B RID: 14923
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x04003A4C RID: 14924
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();

		// Token: 0x0600581E RID: 22558 RVA: 0x001BAC9B File Offset: 0x001B909B
		public virtual void Init()
		{
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x001BACA0 File Offset: 0x001B90A0
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

		// Token: 0x06005820 RID: 22560 RVA: 0x001BAD4A File Offset: 0x001B914A
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x001BAD71 File Offset: 0x001B9171
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
