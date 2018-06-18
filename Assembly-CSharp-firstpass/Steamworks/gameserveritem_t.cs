using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x0200014D RID: 333
	[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 372)]
	public class gameserveritem_t
	{
		// Token: 0x0600070C RID: 1804 RVA: 0x0000BD64 File Offset: 0x00009F64
		public string GetGameDir()
		{
			return Encoding.UTF8.GetString(this.m_szGameDir, 0, Array.IndexOf<byte>(this.m_szGameDir, 0));
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0000BD96 File Offset: 0x00009F96
		public void SetGameDir(string dir)
		{
			this.m_szGameDir = Encoding.UTF8.GetBytes(dir + '\0');
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		public string GetMap()
		{
			return Encoding.UTF8.GetString(this.m_szMap, 0, Array.IndexOf<byte>(this.m_szMap, 0));
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0000BDEA File Offset: 0x00009FEA
		public void SetMap(string map)
		{
			this.m_szMap = Encoding.UTF8.GetBytes(map + '\0');
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0000BE0C File Offset: 0x0000A00C
		public string GetGameDescription()
		{
			return Encoding.UTF8.GetString(this.m_szGameDescription, 0, Array.IndexOf<byte>(this.m_szGameDescription, 0));
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0000BE3E File Offset: 0x0000A03E
		public void SetGameDescription(string desc)
		{
			this.m_szGameDescription = Encoding.UTF8.GetBytes(desc + '\0');
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0000BE60 File Offset: 0x0000A060
		public string GetServerName()
		{
			string result;
			if (this.m_szServerName[0] == 0)
			{
				result = this.m_NetAdr.GetConnectionAddressString();
			}
			else
			{
				result = Encoding.UTF8.GetString(this.m_szServerName, 0, Array.IndexOf<byte>(this.m_szServerName, 0));
			}
			return result;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0000BEB0 File Offset: 0x0000A0B0
		public void SetServerName(string name)
		{
			this.m_szServerName = Encoding.UTF8.GetBytes(name + '\0');
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0000BED0 File Offset: 0x0000A0D0
		public string GetGameTags()
		{
			return Encoding.UTF8.GetString(this.m_szGameTags, 0, Array.IndexOf<byte>(this.m_szGameTags, 0));
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0000BF02 File Offset: 0x0000A102
		public void SetGameTags(string tags)
		{
			this.m_szGameTags = Encoding.UTF8.GetBytes(tags + '\0');
		}

		// Token: 0x04000640 RID: 1600
		public servernetadr_t m_NetAdr;

		// Token: 0x04000641 RID: 1601
		public int m_nPing;

		// Token: 0x04000642 RID: 1602
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHadSuccessfulResponse;

		// Token: 0x04000643 RID: 1603
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDoNotRefresh;

		// Token: 0x04000644 RID: 1604
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szGameDir;

		// Token: 0x04000645 RID: 1605
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szMap;

		// Token: 0x04000646 RID: 1606
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szGameDescription;

		// Token: 0x04000647 RID: 1607
		public uint m_nAppID;

		// Token: 0x04000648 RID: 1608
		public int m_nPlayers;

		// Token: 0x04000649 RID: 1609
		public int m_nMaxPlayers;

		// Token: 0x0400064A RID: 1610
		public int m_nBotPlayers;

		// Token: 0x0400064B RID: 1611
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPassword;

		// Token: 0x0400064C RID: 1612
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSecure;

		// Token: 0x0400064D RID: 1613
		public uint m_ulTimeLastPlayed;

		// Token: 0x0400064E RID: 1614
		public int m_nServerVersion;

		// Token: 0x0400064F RID: 1615
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szServerName;

		// Token: 0x04000650 RID: 1616
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szGameTags;

		// Token: 0x04000651 RID: 1617
		public CSteamID m_steamID;
	}
}
