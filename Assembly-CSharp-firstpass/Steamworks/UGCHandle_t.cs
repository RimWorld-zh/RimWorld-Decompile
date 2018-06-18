using System;

namespace Steamworks
{
	// Token: 0x02000166 RID: 358
	public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
	{
		// Token: 0x0600082B RID: 2091 RVA: 0x0000E4E9 File Offset: 0x0000C6E9
		public UGCHandle_t(ulong value)
		{
			this.m_UGCHandle = value;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		public override string ToString()
		{
			return this.m_UGCHandle.ToString();
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0000E51C File Offset: 0x0000C71C
		public override bool Equals(object other)
		{
			return other is UGCHandle_t && this == (UGCHandle_t)other;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0000E550 File Offset: 0x0000C750
		public override int GetHashCode()
		{
			return this.m_UGCHandle.GetHashCode();
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0000E578 File Offset: 0x0000C778
		public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
		{
			return x.m_UGCHandle == y.m_UGCHandle;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public static bool operator !=(UGCHandle_t x, UGCHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
		public static explicit operator UGCHandle_t(ulong value)
		{
			return new UGCHandle_t(value);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		public static explicit operator ulong(UGCHandle_t that)
		{
			return that.m_UGCHandle;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
		public bool Equals(UGCHandle_t other)
		{
			return this.m_UGCHandle == other.m_UGCHandle;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0000E61C File Offset: 0x0000C81C
		public int CompareTo(UGCHandle_t other)
		{
			return this.m_UGCHandle.CompareTo(other.m_UGCHandle);
		}

		// Token: 0x0400067E RID: 1662
		public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);

		// Token: 0x0400067F RID: 1663
		public ulong m_UGCHandle;
	}
}
