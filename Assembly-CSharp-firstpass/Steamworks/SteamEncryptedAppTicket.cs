using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000036 RID: 54
	public static class SteamEncryptedAppTicket
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00003720 File Offset: 0x00001920
		public static bool BDecryptTicket(byte[] rgubTicketEncrypted, uint cubTicketEncrypted, byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, byte[] rgubKey, int cubKey)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.BDecryptTicket(rgubTicketEncrypted, cubTicketEncrypted, rgubTicketDecrypted, ref pcubTicketDecrypted, rgubKey, cubKey);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003748 File Offset: 0x00001948
		public static bool BIsTicketForApp(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.BIsTicketForApp(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000376C File Offset: 0x0000196C
		public static uint GetTicketIssueTime(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.GetTicketIssueTime(rgubTicketDecrypted, cubTicketDecrypted);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000378D File Offset: 0x0000198D
		public static void GetTicketSteamID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID)
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.GetTicketSteamID(rgubTicketDecrypted, cubTicketDecrypted, out psteamID);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000037A0 File Offset: 0x000019A0
		public static uint GetTicketAppID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.GetTicketAppID(rgubTicketDecrypted, cubTicketDecrypted);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000037C4 File Offset: 0x000019C4
		public static bool BUserOwnsAppInTicket(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.BUserOwnsAppInTicket(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000037E8 File Offset: 0x000019E8
		public static bool BUserIsVacBanned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.BUserIsVacBanned(rgubTicketDecrypted, cubTicketDecrypted);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000380C File Offset: 0x00001A0C
		public static byte[] GetUserVariableData(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData)
		{
			InteropHelp.TestIfPlatformSupported();
			IntPtr userVariableData = NativeMethods.GetUserVariableData(rgubTicketDecrypted, cubTicketDecrypted, out pcubUserData);
			byte[] array = new byte[pcubUserData];
			Marshal.Copy(userVariableData, array, 0, (int)pcubUserData);
			return array;
		}
	}
}
