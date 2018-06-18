using System;

namespace Steamworks
{
	// Token: 0x02000165 RID: 357
	public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t>
	{
		// Token: 0x06000820 RID: 2080 RVA: 0x0000E381 File Offset: 0x0000C581
		public UGCFileWriteStreamHandle_t(ulong value)
		{
			this.m_UGCFileWriteStreamHandle = value;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0000E38C File Offset: 0x0000C58C
		public override string ToString()
		{
			return this.m_UGCFileWriteStreamHandle.ToString();
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		public override bool Equals(object other)
		{
			return other is UGCFileWriteStreamHandle_t && this == (UGCFileWriteStreamHandle_t)other;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		public override int GetHashCode()
		{
			return this.m_UGCFileWriteStreamHandle.GetHashCode();
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0000E410 File Offset: 0x0000C610
		public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return x.m_UGCFileWriteStreamHandle == y.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0000E438 File Offset: 0x0000C638
		public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0000E458 File Offset: 0x0000C658
		public static explicit operator UGCFileWriteStreamHandle_t(ulong value)
		{
			return new UGCFileWriteStreamHandle_t(value);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0000E474 File Offset: 0x0000C674
		public static explicit operator ulong(UGCFileWriteStreamHandle_t that)
		{
			return that.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0000E490 File Offset: 0x0000C690
		public bool Equals(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle == other.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		public int CompareTo(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
		}

		// Token: 0x0400067C RID: 1660
		public static readonly UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);

		// Token: 0x0400067D RID: 1661
		public ulong m_UGCFileWriteStreamHandle;
	}
}
