using System;
using RimWorld;
using Steamworks;

namespace Verse
{
	// Token: 0x02000FB0 RID: 4016
	public static class ScreenshotTaker
	{
		// Token: 0x0600610B RID: 24843 RVA: 0x0030F8AB File Offset: 0x0030DCAB
		public static void Update()
		{
			if (!LongEventHandler.ShouldWaitForEvent)
			{
				if (KeyBindingDefOf.TakeScreenshot.JustPressed || ScreenshotTaker.takeScreenshot)
				{
					ScreenshotTaker.TakeShot();
					ScreenshotTaker.takeScreenshot = false;
				}
			}
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x0030F8E3 File Offset: 0x0030DCE3
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = true;
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x0030F8EC File Offset: 0x0030DCEC
		private static void TakeShot()
		{
			SteamScreenshots.TriggerScreenshot();
		}

		// Token: 0x04003F79 RID: 16249
		private static bool takeScreenshot;
	}
}
