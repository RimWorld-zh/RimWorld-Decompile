using System;

namespace Steamworks
{
	// Token: 0x0200016C RID: 364
	public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
	{
		// Token: 0x04000689 RID: 1673
		public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);

		// Token: 0x0400068A RID: 1674
		public ulong m_SteamAPICall;

		// Token: 0x0600086C RID: 2156 RVA: 0x0000ED4D File Offset: 0x0000CF4D
		public SteamAPICall_t(ulong value)
		{
			this.m_SteamAPICall = value;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0000ED58 File Offset: 0x0000CF58
		public override string ToString()
		{
			return this.m_SteamAPICall.ToString();
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0000ED80 File Offset: 0x0000CF80
		public override bool Equals(object other)
		{
			return other is SteamAPICall_t && this == (SteamAPICall_t)other;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		public override int GetHashCode()
		{
			return this.m_SteamAPICall.GetHashCode();
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0000EDDC File Offset: 0x0000CFDC
		public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y)
		{
			return x.m_SteamAPICall == y.m_SteamAPICall;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0000EE04 File Offset: 0x0000D004
		public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0000EE24 File Offset: 0x0000D024
		public static explicit operator SteamAPICall_t(ulong value)
		{
			return new SteamAPICall_t(value);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0000EE40 File Offset: 0x0000D040
		public static explicit operator ulong(SteamAPICall_t that)
		{
			return that.m_SteamAPICall;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0000EE5C File Offset: 0x0000D05C
		public bool Equals(SteamAPICall_t other)
		{
			return this.m_SteamAPICall == other.m_SteamAPICall;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0000EE80 File Offset: 0x0000D080
		public int CompareTo(SteamAPICall_t other)
		{
			return this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
		}
	}
}
