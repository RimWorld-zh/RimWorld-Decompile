using System;
using RimWorld;
using Steamworks;

namespace Verse
{
	// Token: 0x02000FAF RID: 4015
	public static class ScreenshotTaker
	{
		// Token: 0x06006132 RID: 24882 RVA: 0x00311A2B File Offset: 0x0030FE2B
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

		// Token: 0x06006133 RID: 24883 RVA: 0x00311A63 File Offset: 0x0030FE63
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = true;
		}

		// Token: 0x06006134 RID: 24884 RVA: 0x00311A6C File Offset: 0x0030FE6C
		private static void TakeShot()
		{
			SteamScreenshots.TriggerScreenshot();
		}

		// Token: 0x04003F8A RID: 16266
		private static bool takeScreenshot;
	}
}
