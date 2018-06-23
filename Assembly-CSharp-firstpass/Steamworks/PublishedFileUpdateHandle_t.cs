using System;

namespace Steamworks
{
	// Token: 0x02000164 RID: 356
	public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
	{
		// Token: 0x0400067A RID: 1658
		public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);

		// Token: 0x0400067B RID: 1659
		public ulong m_PublishedFileUpdateHandle;

		// Token: 0x06000815 RID: 2069 RVA: 0x0000E219 File Offset: 0x0000C419
		public PublishedFileUpdateHandle_t(ulong value)
		{
			this.m_PublishedFileUpdateHandle = value;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0000E224 File Offset: 0x0000C424
		public override string ToString()
		{
			return this.m_PublishedFileUpdateHandle.ToString();
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0000E24C File Offset: 0x0000C44C
		public override bool Equals(object other)
		{
			return other is PublishedFileUpdateHandle_t && this == (PublishedFileUpdateHandle_t)other;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0000E280 File Offset: 0x0000C480
		public override int GetHashCode()
		{
			return this.m_PublishedFileUpdateHandle.GetHashCode();
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0000E2A8 File Offset: 0x0000C4A8
		public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0000E2D0 File Offset: 0x0000C4D0
		public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0000E2F0 File Offset: 0x0000C4F0
		public static explicit operator PublishedFileUpdateHandle_t(ulong value)
		{
			return new PublishedFileUpdateHandle_t(value);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0000E30C File Offset: 0x0000C50C
		public static explicit operator ulong(PublishedFileUpdateHandle_t that)
		{
			return that.m_PublishedFileUpdateHandle;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0000E328 File Offset: 0x0000C528
		public bool Equals(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0000E34C File Offset: 0x0000C54C
		public int CompareTo(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
		}
	}
}
