using System;

namespace Steamworks
{
	// Token: 0x02000161 RID: 353
	public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
	{
		// Token: 0x04000676 RID: 1654
		public uint m_SNetListenSocket;

		// Token: 0x060007F6 RID: 2038 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
		public SNetListenSocket_t(uint value)
		{
			this.m_SNetListenSocket = value;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0000DE04 File Offset: 0x0000C004
		public override string ToString()
		{
			return this.m_SNetListenSocket.ToString();
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0000DE2C File Offset: 0x0000C02C
		public override bool Equals(object other)
		{
			return other is SNetListenSocket_t && this == (SNetListenSocket_t)other;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0000DE60 File Offset: 0x0000C060
		public override int GetHashCode()
		{
			return this.m_SNetListenSocket.GetHashCode();
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0000DE88 File Offset: 0x0000C088
		public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return x.m_SNetListenSocket == y.m_SNetListenSocket;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0000DEB0 File Offset: 0x0000C0B0
		public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0000DED0 File Offset: 0x0000C0D0
		public static explicit operator SNetListenSocket_t(uint value)
		{
			return new SNetListenSocket_t(value);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0000DEEC File Offset: 0x0000C0EC
		public static explicit operator uint(SNetListenSocket_t that)
		{
			return that.m_SNetListenSocket;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0000DF08 File Offset: 0x0000C108
		public bool Equals(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket == other.m_SNetListenSocket;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0000DF2C File Offset: 0x0000C12C
		public int CompareTo(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
		}
	}
}
