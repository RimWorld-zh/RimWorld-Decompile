using System;

namespace Steamworks
{
	// Token: 0x02000148 RID: 328
	public static class SteamUnifiedMessages
	{
		// Token: 0x060006A6 RID: 1702 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public static ClientUnifiedMessageHandle SendMethod(string pchServiceMethod, byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext)
		{
			InteropHelp.TestIfAvailableClient();
			ClientUnifiedMessageHandle result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchServiceMethod))
			{
				result = (ClientUnifiedMessageHandle)NativeMethods.ISteamUnifiedMessages_SendMethod(utf8StringHandle, pRequestBuffer, unRequestBufferSize, unContext);
			}
			return result;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0000AB7C File Offset: 0x00008D7C
		public static bool GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		public static bool GetMethodResponseData(ClientUnifiedMessageHandle hHandle, byte[] pResponseBuffer, uint unResponseBufferSize, bool bAutoRelease)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		public static bool ReleaseMethod(ClientUnifiedMessageHandle hHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		public static bool SendNotification(string pchServiceNotification, byte[] pNotificationBuffer, uint unNotificationBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchServiceNotification))
			{
				result = NativeMethods.ISteamUnifiedMessages_SendNotification(utf8StringHandle, pNotificationBuffer, unNotificationBufferSize);
			}
			return result;
		}
	}
}
