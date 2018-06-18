using System;

namespace Steamworks
{
	// Token: 0x02000150 RID: 336
	public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
	{
		// Token: 0x06000732 RID: 1842 RVA: 0x0000C40B File Offset: 0x0000A60B
		public HSteamUser(int value)
		{
			this.m_HSteamUser = value;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0000C418 File Offset: 0x0000A618
		public override string ToString()
		{
			return this.m_HSteamUser.ToString();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0000C440 File Offset: 0x0000A640
		public override bool Equals(object other)
		{
			return other is HSteamUser && this == (HSteamUser)other;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0000C474 File Offset: 0x0000A674
		public override int GetHashCode()
		{
			return this.m_HSteamUser.GetHashCode();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0000C49C File Offset: 0x0000A69C
		public static bool operator ==(HSteamUser x, HSteamUser y)
		{
			return x.m_HSteamUser == y.m_HSteamUser;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0000C4C4 File Offset: 0x0000A6C4
		public static bool operator !=(HSteamUser x, HSteamUser y)
		{
			return !(x == y);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0000C4E4 File Offset: 0x0000A6E4
		public static explicit operator HSteamUser(int value)
		{
			return new HSteamUser(value);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0000C500 File Offset: 0x0000A700
		public static explicit operator int(HSteamUser that)
		{
			return that.m_HSteamUser;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0000C51C File Offset: 0x0000A71C
		public bool Equals(HSteamUser other)
		{
			return this.m_HSteamUser == other.m_HSteamUser;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0000C540 File Offset: 0x0000A740
		public int CompareTo(HSteamUser other)
		{
			return this.m_HSteamUser.CompareTo(other.m_HSteamUser);
		}

		// Token: 0x04000656 RID: 1622
		public int m_HSteamUser;
	}
}
