using System;

namespace Steamworks
{
	// Token: 0x02000160 RID: 352
	public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
	{
		// Token: 0x060007EB RID: 2027 RVA: 0x0000DC90 File Offset: 0x0000BE90
		public HServerQuery(int value)
		{
			this.m_HServerQuery = value;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		public override string ToString()
		{
			return this.m_HServerQuery.ToString();
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0000DCC4 File Offset: 0x0000BEC4
		public override bool Equals(object other)
		{
			return other is HServerQuery && this == (HServerQuery)other;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0000DCF8 File Offset: 0x0000BEF8
		public override int GetHashCode()
		{
			return this.m_HServerQuery.GetHashCode();
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0000DD20 File Offset: 0x0000BF20
		public static bool operator ==(HServerQuery x, HServerQuery y)
		{
			return x.m_HServerQuery == y.m_HServerQuery;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0000DD48 File Offset: 0x0000BF48
		public static bool operator !=(HServerQuery x, HServerQuery y)
		{
			return !(x == y);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0000DD68 File Offset: 0x0000BF68
		public static explicit operator HServerQuery(int value)
		{
			return new HServerQuery(value);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0000DD84 File Offset: 0x0000BF84
		public static explicit operator int(HServerQuery that)
		{
			return that.m_HServerQuery;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		public bool Equals(HServerQuery other)
		{
			return this.m_HServerQuery == other.m_HServerQuery;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0000DDC4 File Offset: 0x0000BFC4
		public int CompareTo(HServerQuery other)
		{
			return this.m_HServerQuery.CompareTo(other.m_HServerQuery);
		}

		// Token: 0x04000674 RID: 1652
		public static readonly HServerQuery Invalid = new HServerQuery(-1);

		// Token: 0x04000675 RID: 1653
		public int m_HServerQuery;
	}
}
