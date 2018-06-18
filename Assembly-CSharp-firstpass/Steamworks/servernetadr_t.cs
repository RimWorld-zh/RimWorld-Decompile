using System;

namespace Steamworks
{
	// Token: 0x0200014E RID: 334
	public struct servernetadr_t
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x0000BF21 File Offset: 0x0000A121
		public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
		{
			this.m_unIP = ip;
			this.m_usQueryPort = usQueryPort;
			this.m_usConnectionPort = usConnectionPort;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0000BF3C File Offset: 0x0000A13C
		public ushort GetQueryPort()
		{
			return this.m_usQueryPort;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0000BF57 File Offset: 0x0000A157
		public void SetQueryPort(ushort usPort)
		{
			this.m_usQueryPort = usPort;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0000BF64 File Offset: 0x0000A164
		public ushort GetConnectionPort()
		{
			return this.m_usConnectionPort;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0000BF7F File Offset: 0x0000A17F
		public void SetConnectionPort(ushort usPort)
		{
			this.m_usConnectionPort = usPort;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0000BF8C File Offset: 0x0000A18C
		public uint GetIP()
		{
			return this.m_unIP;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0000BFA7 File Offset: 0x0000A1A7
		public void SetIP(uint unIP)
		{
			this.m_unIP = unIP;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
		public string GetConnectionAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0000BFDC File Offset: 0x0000A1DC
		public string GetQueryAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0000C004 File Offset: 0x0000A204
		public static string ToString(uint unIP, ushort usPort)
		{
			return string.Format("{0}.{1}.{2}.{3}:{4}", new object[]
			{
				(ulong)(unIP >> 24) & 255UL,
				(ulong)(unIP >> 16) & 255UL,
				(ulong)(unIP >> 8) & 255UL,
				(ulong)unIP & 255UL,
				usPort
			});
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0000C080 File Offset: 0x0000A280
		public static bool operator <(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP < y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort < y.m_usQueryPort);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public static bool operator >(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP > y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort > y.m_usQueryPort);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000C128 File Offset: 0x0000A328
		public override bool Equals(object other)
		{
			return other is servernetadr_t && this == (servernetadr_t)other;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0000C15C File Offset: 0x0000A35C
		public override int GetHashCode()
		{
			return this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0000C1A8 File Offset: 0x0000A3A8
		public static bool operator ==(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP == y.m_unIP && x.m_usQueryPort == y.m_usQueryPort && x.m_usConnectionPort == y.m_usConnectionPort;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		public static bool operator !=(servernetadr_t x, servernetadr_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0000C218 File Offset: 0x0000A418
		public bool Equals(servernetadr_t other)
		{
			return this.m_unIP == other.m_unIP && this.m_usQueryPort == other.m_usQueryPort && this.m_usConnectionPort == other.m_usConnectionPort;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000C264 File Offset: 0x0000A464
		public int CompareTo(servernetadr_t other)
		{
			return this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
		}

		// Token: 0x04000652 RID: 1618
		private ushort m_usConnectionPort;

		// Token: 0x04000653 RID: 1619
		private ushort m_usQueryPort;

		// Token: 0x04000654 RID: 1620
		private uint m_unIP;
	}
}
