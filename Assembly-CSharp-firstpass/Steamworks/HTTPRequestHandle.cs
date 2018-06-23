using System;

namespace Steamworks
{
	// Token: 0x0200015B RID: 347
	public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
	{
		// Token: 0x0400066B RID: 1643
		public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0u);

		// Token: 0x0400066C RID: 1644
		public uint m_HTTPRequestHandle;

		// Token: 0x060007B6 RID: 1974 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
		public HTTPRequestHandle(uint value)
		{
			this.m_HTTPRequestHandle = value;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0000D5C0 File Offset: 0x0000B7C0
		public override string ToString()
		{
			return this.m_HTTPRequestHandle.ToString();
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0000D5E8 File Offset: 0x0000B7E8
		public override bool Equals(object other)
		{
			return other is HTTPRequestHandle && this == (HTTPRequestHandle)other;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0000D61C File Offset: 0x0000B81C
		public override int GetHashCode()
		{
			return this.m_HTTPRequestHandle.GetHashCode();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000D644 File Offset: 0x0000B844
		public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0000D66C File Offset: 0x0000B86C
		public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return !(x == y);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0000D68C File Offset: 0x0000B88C
		public static explicit operator HTTPRequestHandle(uint value)
		{
			return new HTTPRequestHandle(value);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		public static explicit operator uint(HTTPRequestHandle that)
		{
			return that.m_HTTPRequestHandle;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0000D6C4 File Offset: 0x0000B8C4
		public bool Equals(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle == other.m_HTTPRequestHandle;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0000D6E8 File Offset: 0x0000B8E8
		public int CompareTo(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
		}
	}
}
