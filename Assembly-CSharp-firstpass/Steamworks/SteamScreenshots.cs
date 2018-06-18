using System;

namespace Steamworks
{
	// Token: 0x02000146 RID: 326
	public static class SteamScreenshots
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x00009E8C File Offset: 0x0000808C
		public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return (ScreenshotHandle)NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00009EB4 File Offset: 0x000080B4
		public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight)
		{
			InteropHelp.TestIfAvailableClient();
			ScreenshotHandle result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFilename))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename))
				{
					result = (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(utf8StringHandle, utf8StringHandle2, nWidth, nHeight);
				}
			}
			return result;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00009F24 File Offset: 0x00008124
		public static void TriggerScreenshot()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_TriggerScreenshot();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00009F31 File Offset: 0x00008131
		public static void HookScreenshots(bool bHook)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00009F40 File Offset: 0x00008140
		public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLocation))
			{
				result = NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00009F88 File Offset: 0x00008188
		public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00009FAC File Offset: 0x000081AC
		public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
		}
	}
}
