using System;

namespace Steamworks
{
	// Token: 0x02000157 RID: 343
	public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
	{
		// Token: 0x0600078A RID: 1930 RVA: 0x0000D013 File Offset: 0x0000B213
		public HAuthTicket(uint value)
		{
			this.m_HAuthTicket = value;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0000D020 File Offset: 0x0000B220
		public override string ToString()
		{
			return this.m_HAuthTicket.ToString();
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0000D048 File Offset: 0x0000B248
		public override bool Equals(object other)
		{
			return other is HAuthTicket && this == (HAuthTicket)other;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0000D07C File Offset: 0x0000B27C
		public override int GetHashCode()
		{
			return this.m_HAuthTicket.GetHashCode();
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0000D0A4 File Offset: 0x0000B2A4
		public static bool operator ==(HAuthTicket x, HAuthTicket y)
		{
			return x.m_HAuthTicket == y.m_HAuthTicket;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0000D0CC File Offset: 0x0000B2CC
		public static bool operator !=(HAuthTicket x, HAuthTicket y)
		{
			return !(x == y);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0000D0EC File Offset: 0x0000B2EC
		public static explicit operator HAuthTicket(uint value)
		{
			return new HAuthTicket(value);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0000D108 File Offset: 0x0000B308
		public static explicit operator uint(HAuthTicket that)
		{
			return that.m_HAuthTicket;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000D124 File Offset: 0x0000B324
		public bool Equals(HAuthTicket other)
		{
			return this.m_HAuthTicket == other.m_HAuthTicket;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0000D148 File Offset: 0x0000B348
		public int CompareTo(HAuthTicket other)
		{
			return this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
		}

		// Token: 0x04000663 RID: 1635
		public static readonly HAuthTicket Invalid = new HAuthTicket(0u);

		// Token: 0x04000664 RID: 1636
		public uint m_HAuthTicket;
	}
}
