using System;

namespace Steamworks
{
	// Token: 0x02000169 RID: 361
	public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
	{
		// Token: 0x0600084B RID: 2123 RVA: 0x0000E913 File Offset: 0x0000CB13
		public AppId_t(uint value)
		{
			this.m_AppId = value;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0000E920 File Offset: 0x0000CB20
		public override string ToString()
		{
			return this.m_AppId.ToString();
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0000E948 File Offset: 0x0000CB48
		public override bool Equals(object other)
		{
			return other is AppId_t && this == (AppId_t)other;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0000E97C File Offset: 0x0000CB7C
		public override int GetHashCode()
		{
			return this.m_AppId.GetHashCode();
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
		public static bool operator ==(AppId_t x, AppId_t y)
		{
			return x.m_AppId == y.m_AppId;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0000E9CC File Offset: 0x0000CBCC
		public static bool operator !=(AppId_t x, AppId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0000E9EC File Offset: 0x0000CBEC
		public static explicit operator AppId_t(uint value)
		{
			return new AppId_t(value);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0000EA08 File Offset: 0x0000CC08
		public static explicit operator uint(AppId_t that)
		{
			return that.m_AppId;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0000EA24 File Offset: 0x0000CC24
		public bool Equals(AppId_t other)
		{
			return this.m_AppId == other.m_AppId;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0000EA48 File Offset: 0x0000CC48
		public int CompareTo(AppId_t other)
		{
			return this.m_AppId.CompareTo(other.m_AppId);
		}

		// Token: 0x04000683 RID: 1667
		public static readonly AppId_t Invalid = new AppId_t(0u);

		// Token: 0x04000684 RID: 1668
		public uint m_AppId;
	}
}
