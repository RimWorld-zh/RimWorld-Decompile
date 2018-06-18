using System;

namespace Steamworks
{
	// Token: 0x02000171 RID: 369
	public struct SteamLeaderboard_t : IEquatable<SteamLeaderboard_t>, IComparable<SteamLeaderboard_t>
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x0000F447 File Offset: 0x0000D647
		public SteamLeaderboard_t(ulong value)
		{
			this.m_SteamLeaderboard = value;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0000F454 File Offset: 0x0000D654
		public override string ToString()
		{
			return this.m_SteamLeaderboard.ToString();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0000F47C File Offset: 0x0000D67C
		public override bool Equals(object other)
		{
			return other is SteamLeaderboard_t && this == (SteamLeaderboard_t)other;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboard.GetHashCode();
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0000F4D8 File Offset: 0x0000D6D8
		public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return x.m_SteamLeaderboard == y.m_SteamLeaderboard;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000F500 File Offset: 0x0000D700
		public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return !(x == y);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000F520 File Offset: 0x0000D720
		public static explicit operator SteamLeaderboard_t(ulong value)
		{
			return new SteamLeaderboard_t(value);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000F53C File Offset: 0x0000D73C
		public static explicit operator ulong(SteamLeaderboard_t that)
		{
			return that.m_SteamLeaderboard;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0000F558 File Offset: 0x0000D758
		public bool Equals(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard == other.m_SteamLeaderboard;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0000F57C File Offset: 0x0000D77C
		public int CompareTo(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
		}

		// Token: 0x04000692 RID: 1682
		public ulong m_SteamLeaderboard;
	}
}
