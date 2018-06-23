using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000130 RID: 304
	public struct MatchMakingKeyValuePair_t
	{
		// Token: 0x0400063E RID: 1598
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szKey;

		// Token: 0x0400063F RID: 1599
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szValue;

		// Token: 0x06000410 RID: 1040 RVA: 0x00003843 File Offset: 0x00001A43
		private MatchMakingKeyValuePair_t(string strKey, string strValue)
		{
			this.m_szKey = strKey;
			this.m_szValue = strValue;
		}
	}
}
