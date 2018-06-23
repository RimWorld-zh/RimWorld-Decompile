using System;

namespace Steamworks
{
	// Token: 0x02000163 RID: 355
	public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t>
	{
		// Token: 0x04000678 RID: 1656
		public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);

		// Token: 0x04000679 RID: 1657
		public ulong m_PublishedFileId;

		// Token: 0x0600080A RID: 2058 RVA: 0x0000E0AF File Offset: 0x0000C2AF
		public PublishedFileId_t(ulong value)
		{
			this.m_PublishedFileId = value;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0000E0BC File Offset: 0x0000C2BC
		public override string ToString()
		{
			return this.m_PublishedFileId.ToString();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		public override bool Equals(object other)
		{
			return other is PublishedFileId_t && this == (PublishedFileId_t)other;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0000E118 File Offset: 0x0000C318
		public override int GetHashCode()
		{
			return this.m_PublishedFileId.GetHashCode();
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0000E140 File Offset: 0x0000C340
		public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
		{
			return x.m_PublishedFileId == y.m_PublishedFileId;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0000E168 File Offset: 0x0000C368
		public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0000E188 File Offset: 0x0000C388
		public static explicit operator PublishedFileId_t(ulong value)
		{
			return new PublishedFileId_t(value);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
		public static explicit operator ulong(PublishedFileId_t that)
		{
			return that.m_PublishedFileId;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0000E1C0 File Offset: 0x0000C3C0
		public bool Equals(PublishedFileId_t other)
		{
			return this.m_PublishedFileId == other.m_PublishedFileId;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0000E1E4 File Offset: 0x0000C3E4
		public int CompareTo(PublishedFileId_t other)
		{
			return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
		}
	}
}
