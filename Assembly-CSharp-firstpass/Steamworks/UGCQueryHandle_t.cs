using System;

namespace Steamworks
{
	// Token: 0x0200016D RID: 365
	public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
	{
		// Token: 0x06000877 RID: 2167 RVA: 0x0000EEB5 File Offset: 0x0000D0B5
		public UGCQueryHandle_t(ulong value)
		{
			this.m_UGCQueryHandle = value;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0000EEC0 File Offset: 0x0000D0C0
		public override string ToString()
		{
			return this.m_UGCQueryHandle.ToString();
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
		public override bool Equals(object other)
		{
			return other is UGCQueryHandle_t && this == (UGCQueryHandle_t)other;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0000EF1C File Offset: 0x0000D11C
		public override int GetHashCode()
		{
			return this.m_UGCQueryHandle.GetHashCode();
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0000EF44 File Offset: 0x0000D144
		public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return x.m_UGCQueryHandle == y.m_UGCQueryHandle;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0000EF6C File Offset: 0x0000D16C
		public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public static explicit operator UGCQueryHandle_t(ulong value)
		{
			return new UGCQueryHandle_t(value);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
		public static explicit operator ulong(UGCQueryHandle_t that)
		{
			return that.m_UGCQueryHandle;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
		public bool Equals(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle == other.m_UGCQueryHandle;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
		public int CompareTo(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
		}

		// Token: 0x0400068B RID: 1675
		public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);

		// Token: 0x0400068C RID: 1676
		public ulong m_UGCQueryHandle;
	}
}
