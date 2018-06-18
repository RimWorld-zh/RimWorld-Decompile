using System;

namespace Steamworks
{
	// Token: 0x0200015D RID: 349
	public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
	{
		// Token: 0x060007CC RID: 1996 RVA: 0x0000D884 File Offset: 0x0000BA84
		public SteamItemDef_t(int value)
		{
			this.m_SteamItemDef = value;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0000D890 File Offset: 0x0000BA90
		public override string ToString()
		{
			return this.m_SteamItemDef.ToString();
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		public override bool Equals(object other)
		{
			return other is SteamItemDef_t && this == (SteamItemDef_t)other;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		public override int GetHashCode()
		{
			return this.m_SteamItemDef.GetHashCode();
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0000D914 File Offset: 0x0000BB14
		public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y)
		{
			return x.m_SteamItemDef == y.m_SteamItemDef;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0000D93C File Offset: 0x0000BB3C
		public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y)
		{
			return !(x == y);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000D95C File Offset: 0x0000BB5C
		public static explicit operator SteamItemDef_t(int value)
		{
			return new SteamItemDef_t(value);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000D978 File Offset: 0x0000BB78
		public static explicit operator int(SteamItemDef_t that)
		{
			return that.m_SteamItemDef;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0000D994 File Offset: 0x0000BB94
		public bool Equals(SteamItemDef_t other)
		{
			return this.m_SteamItemDef == other.m_SteamItemDef;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
		public int CompareTo(SteamItemDef_t other)
		{
			return this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
		}

		// Token: 0x0400066F RID: 1647
		public int m_SteamItemDef;
	}
}
