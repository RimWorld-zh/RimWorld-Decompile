using System;

namespace Steamworks
{
	// Token: 0x02000162 RID: 354
	public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
	{
		// Token: 0x04000677 RID: 1655
		public uint m_SNetSocket;

		// Token: 0x06000800 RID: 2048 RVA: 0x0000DF53 File Offset: 0x0000C153
		public SNetSocket_t(uint value)
		{
			this.m_SNetSocket = value;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0000DF60 File Offset: 0x0000C160
		public override string ToString()
		{
			return this.m_SNetSocket.ToString();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0000DF88 File Offset: 0x0000C188
		public override bool Equals(object other)
		{
			return other is SNetSocket_t && this == (SNetSocket_t)other;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0000DFBC File Offset: 0x0000C1BC
		public override int GetHashCode()
		{
			return this.m_SNetSocket.GetHashCode();
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
		public static bool operator ==(SNetSocket_t x, SNetSocket_t y)
		{
			return x.m_SNetSocket == y.m_SNetSocket;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0000E00C File Offset: 0x0000C20C
		public static bool operator !=(SNetSocket_t x, SNetSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0000E02C File Offset: 0x0000C22C
		public static explicit operator SNetSocket_t(uint value)
		{
			return new SNetSocket_t(value);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0000E048 File Offset: 0x0000C248
		public static explicit operator uint(SNetSocket_t that)
		{
			return that.m_SNetSocket;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0000E064 File Offset: 0x0000C264
		public bool Equals(SNetSocket_t other)
		{
			return this.m_SNetSocket == other.m_SNetSocket;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0000E088 File Offset: 0x0000C288
		public int CompareTo(SNetSocket_t other)
		{
			return this.m_SNetSocket.CompareTo(other.m_SNetSocket);
		}
	}
}
