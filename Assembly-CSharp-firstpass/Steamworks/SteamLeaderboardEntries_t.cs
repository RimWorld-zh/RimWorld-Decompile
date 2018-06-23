using System;

namespace Steamworks
{
	// Token: 0x02000170 RID: 368
	public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
	{
		// Token: 0x04000691 RID: 1681
		public ulong m_SteamLeaderboardEntries;

		// Token: 0x06000898 RID: 2200 RVA: 0x0000F2ED File Offset: 0x0000D4ED
		public SteamLeaderboardEntries_t(ulong value)
		{
			this.m_SteamLeaderboardEntries = value;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		public override string ToString()
		{
			return this.m_SteamLeaderboardEntries.ToString();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0000F320 File Offset: 0x0000D520
		public override bool Equals(object other)
		{
			return other is SteamLeaderboardEntries_t && this == (SteamLeaderboardEntries_t)other;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0000F354 File Offset: 0x0000D554
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboardEntries.GetHashCode();
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0000F37C File Offset: 0x0000D57C
		public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return x.m_SteamLeaderboardEntries == y.m_SteamLeaderboardEntries;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0000F3A4 File Offset: 0x0000D5A4
		public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return !(x == y);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0000F3C4 File Offset: 0x0000D5C4
		public static explicit operator SteamLeaderboardEntries_t(ulong value)
		{
			return new SteamLeaderboardEntries_t(value);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
		public static explicit operator ulong(SteamLeaderboardEntries_t that)
		{
			return that.m_SteamLeaderboardEntries;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0000F3FC File Offset: 0x0000D5FC
		public bool Equals(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries == other.m_SteamLeaderboardEntries;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0000F420 File Offset: 0x0000D620
		public int CompareTo(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
		}
	}
}
