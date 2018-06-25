using System;
using RimWorld;
using Steamworks;

namespace Verse
{
	// Token: 0x02000FB3 RID: 4019
	public static class ScreenshotTaker
	{
		// Token: 0x04003F8D RID: 16269
		private static bool takeScreenshot;

		// Token: 0x0600613C RID: 24892 RVA: 0x003120AB File Offset: 0x003104AB
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

		// Token: 0x0600613D RID: 24893 RVA: 0x003120E3 File Offset: 0x003104E3
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = true;
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x003120EC File Offset: 0x003104EC
		private static void TakeShot()
		{
			SteamScreenshots.TriggerScreenshot();
		}
	}
}
