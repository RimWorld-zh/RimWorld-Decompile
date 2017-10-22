using RimWorld;
using Steamworks;

namespace Verse
{
	public static class ScreenshotTaker
	{
		public static void Update()
		{
			if (!LongEventHandler.ShouldWaitForEvent && KeyBindingDefOf.TakeScreenshot.JustPressed)
			{
				ScreenshotTaker.TakeShot();
			}
		}

		private static void TakeShot()
		{
			SteamScreenshots.TriggerScreenshot();
		}
	}
}
