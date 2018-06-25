using System;
using RimWorld;
using Steamworks;

namespace Verse
{
	// Token: 0x02000FB4 RID: 4020
	public static class ScreenshotTaker
	{
		// Token: 0x04003F95 RID: 16277
		private static bool takeScreenshot;

		// Token: 0x0600613C RID: 24892 RVA: 0x003122EF File Offset: 0x003106EF
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

		// Token: 0x0600613D RID: 24893 RVA: 0x00312327 File Offset: 0x00310727
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = true;
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x00312330 File Offset: 0x00310730
		private static void TakeShot()
		{
			SteamScreenshots.TriggerScreenshot();
		}
	}
}
