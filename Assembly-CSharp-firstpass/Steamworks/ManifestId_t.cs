using System;

namespace Steamworks
{
	// Token: 0x0200016B RID: 363
	public struct ManifestId_t : IEquatable<ManifestId_t>, IComparable<ManifestId_t>
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x0000EBE4 File Offset: 0x0000CDE4
		public ManifestId_t(ulong value)
		{
			this.m_ManifestId = value;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
		public override string ToString()
		{
			return this.m_ManifestId.ToString();
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0000EC18 File Offset: 0x0000CE18
		public override bool Equals(object other)
		{
			return other is ManifestId_t && this == (ManifestId_t)other;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0000EC4C File Offset: 0x0000CE4C
		public override int GetHashCode()
		{
			return this.m_ManifestId.GetHashCode();
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0000EC74 File Offset: 0x0000CE74
		public static bool operator ==(ManifestId_t x, ManifestId_t y)
		{
			return x.m_ManifestId == y.m_ManifestId;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		public static bool operator !=(ManifestId_t x, ManifestId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0000ECBC File Offset: 0x0000CEBC
		public static explicit operator ManifestId_t(ulong value)
		{
			return new ManifestId_t(value);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0000ECD8 File Offset: 0x0000CED8
		public static explicit operator ulong(ManifestId_t that)
		{
			return that.m_ManifestId;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0000ECF4 File Offset: 0x0000CEF4
		public bool Equals(ManifestId_t other)
		{
			return this.m_ManifestId == other.m_ManifestId;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000ED18 File Offset: 0x0000CF18
		public int CompareTo(ManifestId_t other)
		{
			return this.m_ManifestId.CompareTo(other.m_ManifestId);
		}

		// Token: 0x04000687 RID: 1671
		public static readonly ManifestId_t Invalid = new ManifestId_t(0UL);

		// Token: 0x04000688 RID: 1672
		public ulong m_ManifestId;
	}
}
