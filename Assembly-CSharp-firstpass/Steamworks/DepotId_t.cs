using System;

namespace Steamworks
{
	// Token: 0x0200016A RID: 362
	public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
	{
		// Token: 0x04000685 RID: 1669
		public static readonly DepotId_t Invalid = new DepotId_t(0u);

		// Token: 0x04000686 RID: 1670
		public uint m_DepotId;

		// Token: 0x06000856 RID: 2134 RVA: 0x0000EA7C File Offset: 0x0000CC7C
		public DepotId_t(uint value)
		{
			this.m_DepotId = value;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0000EA88 File Offset: 0x0000CC88
		public override string ToString()
		{
			return this.m_DepotId.ToString();
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
		public override bool Equals(object other)
		{
			return other is DepotId_t && this == (DepotId_t)other;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0000EAE4 File Offset: 0x0000CCE4
		public override int GetHashCode()
		{
			return this.m_DepotId.GetHashCode();
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0000EB0C File Offset: 0x0000CD0C
		public static bool operator ==(DepotId_t x, DepotId_t y)
		{
			return x.m_DepotId == y.m_DepotId;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0000EB34 File Offset: 0x0000CD34
		public static bool operator !=(DepotId_t x, DepotId_t y)
		{
			return !(x == y);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0000EB54 File Offset: 0x0000CD54
		public static explicit operator DepotId_t(uint value)
		{
			return new DepotId_t(value);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0000EB70 File Offset: 0x0000CD70
		public static explicit operator uint(DepotId_t that)
		{
			return that.m_DepotId;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0000EB8C File Offset: 0x0000CD8C
		public bool Equals(DepotId_t other)
		{
			return this.m_DepotId == other.m_DepotId;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0000EBB0 File Offset: 0x0000CDB0
		public int CompareTo(DepotId_t other)
		{
			return this.m_DepotId.CompareTo(other.m_DepotId);
		}
	}
}
