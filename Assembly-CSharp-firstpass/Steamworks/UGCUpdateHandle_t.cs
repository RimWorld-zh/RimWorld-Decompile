using System;

namespace Steamworks
{
	// Token: 0x0200016E RID: 366
	public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
	{
		// Token: 0x0400068D RID: 1677
		public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);

		// Token: 0x0400068E RID: 1678
		public ulong m_UGCUpdateHandle;

		// Token: 0x06000882 RID: 2178 RVA: 0x0000F01D File Offset: 0x0000D21D
		public UGCUpdateHandle_t(ulong value)
		{
			this.m_UGCUpdateHandle = value;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0000F028 File Offset: 0x0000D228
		public override string ToString()
		{
			return this.m_UGCUpdateHandle.ToString();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0000F050 File Offset: 0x0000D250
		public override bool Equals(object other)
		{
			return other is UGCUpdateHandle_t && this == (UGCUpdateHandle_t)other;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0000F084 File Offset: 0x0000D284
		public override int GetHashCode()
		{
			return this.m_UGCUpdateHandle.GetHashCode();
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return x.m_UGCUpdateHandle == y.m_UGCUpdateHandle;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000F0F4 File Offset: 0x0000D2F4
		public static explicit operator UGCUpdateHandle_t(ulong value)
		{
			return new UGCUpdateHandle_t(value);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0000F110 File Offset: 0x0000D310
		public static explicit operator ulong(UGCUpdateHandle_t that)
		{
			return that.m_UGCUpdateHandle;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0000F12C File Offset: 0x0000D32C
		public bool Equals(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle == other.m_UGCUpdateHandle;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0000F150 File Offset: 0x0000D350
		public int CompareTo(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
		}
	}
}
