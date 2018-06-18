using System;

namespace Steamworks
{
	// Token: 0x0200015C RID: 348
	public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t>
	{
		// Token: 0x060007C1 RID: 1985 RVA: 0x0000D71C File Offset: 0x0000B91C
		public SteamInventoryResult_t(int value)
		{
			this.m_SteamInventoryResult = value;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0000D728 File Offset: 0x0000B928
		public override string ToString()
		{
			return this.m_SteamInventoryResult.ToString();
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0000D750 File Offset: 0x0000B950
		public override bool Equals(object other)
		{
			return other is SteamInventoryResult_t && this == (SteamInventoryResult_t)other;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0000D784 File Offset: 0x0000B984
		public override int GetHashCode()
		{
			return this.m_SteamInventoryResult.GetHashCode();
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0000D7D4 File Offset: 0x0000B9D4
		public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return !(x == y);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		public static explicit operator SteamInventoryResult_t(int value)
		{
			return new SteamInventoryResult_t(value);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0000D810 File Offset: 0x0000BA10
		public static explicit operator int(SteamInventoryResult_t that)
		{
			return that.m_SteamInventoryResult;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0000D82C File Offset: 0x0000BA2C
		public bool Equals(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0000D850 File Offset: 0x0000BA50
		public int CompareTo(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
		}

		// Token: 0x0400066D RID: 1645
		public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);

		// Token: 0x0400066E RID: 1646
		public int m_SteamInventoryResult;
	}
}
