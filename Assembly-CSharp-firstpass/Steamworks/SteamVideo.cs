using System;

namespace Steamworks
{
	// Token: 0x0200014C RID: 332
	public static class SteamVideo
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x0000BD2E File Offset: 0x00009F2E
		public static void GetVideoURL(AppId_t unVideoAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamVideo_GetVideoURL(unVideoAppID);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0000BD3C File Offset: 0x00009F3C
		public static bool IsBroadcasting(out int pnNumViewers)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamVideo_IsBroadcasting(out pnNumViewers);
		}
	}
}
