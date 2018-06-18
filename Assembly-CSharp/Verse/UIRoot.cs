using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E95 RID: 3733
	public abstract class UIRoot
	{
		// Token: 0x060057FE RID: 22526 RVA: 0x001BAAB3 File Offset: 0x001B8EB3
		public virtual void Init()
		{
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x001BAAB8 File Offset: 0x001B8EB8
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

		// Token: 0x06005800 RID: 22528 RVA: 0x001BAB62 File Offset: 0x001B8F62
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		// Token: 0x06005801 RID: 22529 RVA: 0x001BAB89 File Offset: 0x001B8F89
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x04003A38 RID: 14904
		public WindowStack windows = new WindowStack();

		// Token: 0x04003A39 RID: 14905
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x04003A3A RID: 14906
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x04003A3B RID: 14907
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x04003A3C RID: 14908
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
