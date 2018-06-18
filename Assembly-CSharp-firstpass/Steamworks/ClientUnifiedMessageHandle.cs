using System;

namespace Steamworks
{
	// Token: 0x0200016F RID: 367
	public struct ClientUnifiedMessageHandle : IEquatable<ClientUnifiedMessageHandle>, IComparable<ClientUnifiedMessageHandle>
	{
		// Token: 0x0600088D RID: 2189 RVA: 0x0000F185 File Offset: 0x0000D385
		public ClientUnifiedMessageHandle(ulong value)
		{
			this.m_ClientUnifiedMessageHandle = value;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0000F190 File Offset: 0x0000D390
		public override string ToString()
		{
			return this.m_ClientUnifiedMessageHandle.ToString();
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0000F1B8 File Offset: 0x0000D3B8
		public override bool Equals(object other)
		{
			return other is ClientUnifiedMessageHandle && this == (ClientUnifiedMessageHandle)other;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0000F1EC File Offset: 0x0000D3EC
		public override int GetHashCode()
		{
			return this.m_ClientUnifiedMessageHandle.GetHashCode();
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0000F214 File Offset: 0x0000D414
		public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
		{
			return x.m_ClientUnifiedMessageHandle == y.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0000F23C File Offset: 0x0000D43C
		public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0000F25C File Offset: 0x0000D45C
		public static explicit operator ClientUnifiedMessageHandle(ulong value)
		{
			return new ClientUnifiedMessageHandle(value);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0000F278 File Offset: 0x0000D478
		public static explicit operator ulong(ClientUnifiedMessageHandle that)
		{
			return that.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0000F294 File Offset: 0x0000D494
		public bool Equals(ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle == other.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
		public int CompareTo(ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);
		}

		// Token: 0x0400068F RID: 1679
		public static readonly ClientUnifiedMessageHandle Invalid = new ClientUnifiedMessageHandle(0UL);

		// Token: 0x04000690 RID: 1680
		public ulong m_ClientUnifiedMessageHandle;
	}
}
