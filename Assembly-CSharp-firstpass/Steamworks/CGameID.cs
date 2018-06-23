using System;

namespace Steamworks
{
	// Token: 0x02000154 RID: 340
	public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
	{
		// Token: 0x04000657 RID: 1623
		public ulong m_GameID;

		// Token: 0x06000748 RID: 1864 RVA: 0x0000C567 File Offset: 0x0000A767
		public CGameID(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0000C571 File Offset: 0x0000A771
		public CGameID(AppId_t nAppID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0000C583 File Offset: 0x0000A783
		public CGameID(AppId_t nAppID, uint nModID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
			this.SetType(CGameID.EGameIDType.k_EGameIDTypeGameMod);
			this.SetModID(nModID);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
		public bool IsSteamApp()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeApp;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
		public bool IsMod()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeGameMod;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		public bool IsShortcut()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeShortcut;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000C604 File Offset: 0x0000A804
		public bool IsP2PFile()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeP2P;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0000C624 File Offset: 0x0000A824
		public AppId_t AppID()
		{
			return new AppId_t((uint)(this.m_GameID & 16777215UL));
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0000C64C File Offset: 0x0000A84C
		public CGameID.EGameIDType Type()
		{
			return (CGameID.EGameIDType)(this.m_GameID >> 24 & 255UL);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0000C674 File Offset: 0x0000A874
		public uint ModID()
		{
			return (uint)(this.m_GameID >> 32 & (ulong)-1);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0000C698 File Offset: 0x0000A898
		public bool IsValid()
		{
			bool result;
			switch (this.Type())
			{
			case CGameID.EGameIDType.k_EGameIDTypeApp:
				result = (this.AppID() != AppId_t.Invalid);
				break;
			case CGameID.EGameIDType.k_EGameIDTypeGameMod:
				result = (this.AppID() != AppId_t.Invalid && (this.ModID() & 2147483648u) != 0u);
				break;
			case CGameID.EGameIDType.k_EGameIDTypeShortcut:
				result = ((this.ModID() & 2147483648u) != 0u);
				break;
			case CGameID.EGameIDType.k_EGameIDTypeP2P:
				result = (this.AppID() == AppId_t.Invalid && (this.ModID() & 2147483648u) != 0u);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0000C75E File Offset: 0x0000A95E
		public void Reset()
		{
			this.m_GameID = 0UL;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000C769 File Offset: 0x0000A969
		public void Set(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0000C773 File Offset: 0x0000A973
		private void SetAppID(AppId_t other)
		{
			this.m_GameID = ((this.m_GameID & 18446744073692774400UL) | ((ulong)((uint)other) & 16777215UL) << 0);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0000C79A File Offset: 0x0000A99A
		private void SetType(CGameID.EGameIDType other)
		{
			this.m_GameID = ((this.m_GameID & 18446744069431361535UL) | (ulong)((ulong)((long)other & 255L) << 24));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0000C7C0 File Offset: 0x0000A9C0
		private void SetModID(uint other)
		{
			this.m_GameID = ((this.m_GameID & (ulong)-1) | ((ulong)other & (ulong)-1) << 32);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		public override string ToString()
		{
			return this.m_GameID.ToString();
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000C804 File Offset: 0x0000AA04
		public override bool Equals(object other)
		{
			return other is CGameID && this == (CGameID)other;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0000C838 File Offset: 0x0000AA38
		public override int GetHashCode()
		{
			return this.m_GameID.GetHashCode();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0000C860 File Offset: 0x0000AA60
		public static bool operator ==(CGameID x, CGameID y)
		{
			return x.m_GameID == y.m_GameID;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000C888 File Offset: 0x0000AA88
		public static bool operator !=(CGameID x, CGameID y)
		{
			return !(x == y);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
		public static explicit operator CGameID(ulong value)
		{
			return new CGameID(value);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		public static explicit operator ulong(CGameID that)
		{
			return that.m_GameID;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		public bool Equals(CGameID other)
		{
			return this.m_GameID == other.m_GameID;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0000C904 File Offset: 0x0000AB04
		public int CompareTo(CGameID other)
		{
			return this.m_GameID.CompareTo(other.m_GameID);
		}

		// Token: 0x02000155 RID: 341
		public enum EGameIDType
		{
			// Token: 0x04000659 RID: 1625
			k_EGameIDTypeApp,
			// Token: 0x0400065A RID: 1626
			k_EGameIDTypeGameMod,
			// Token: 0x0400065B RID: 1627
			k_EGameIDTypeShortcut,
			// Token: 0x0400065C RID: 1628
			k_EGameIDTypeP2P
		}
	}
}
