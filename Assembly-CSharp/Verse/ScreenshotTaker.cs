using System;
using RimWorld;
using Steamworks;

namespace Verse
{
	// Token: 0x02000FAF RID: 4015
	public static class ScreenshotTaker
	{
		// Token: 0x06006109 RID: 24841 RVA: 0x0030F987 File Offset: 0x0030DD87
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

		// Token: 0x0600610A RID: 24842 RVA: 0x0030F9BF File Offset: 0x0030DDBF
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = true;
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x0030F9C8 File Offset: 0x0030DDC8
		private static void TakeShot()
		{
			SteamScreenshots.TriggerScreenshot();
		}

		// Token: 0x04003F78 RID: 16248
		private static bool takeScreenshot;
	}
}
