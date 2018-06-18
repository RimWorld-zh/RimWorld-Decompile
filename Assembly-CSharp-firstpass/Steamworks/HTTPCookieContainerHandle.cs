using System;

namespace Steamworks
{
	// Token: 0x0200015A RID: 346
	public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle>
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x0000D44C File Offset: 0x0000B64C
		public HTTPCookieContainerHandle(uint value)
		{
			this.m_HTTPCookieContainerHandle = value;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0000D458 File Offset: 0x0000B658
		public override string ToString()
		{
			return this.m_HTTPCookieContainerHandle.ToString();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0000D480 File Offset: 0x0000B680
		public override bool Equals(object other)
		{
			return other is HTTPCookieContainerHandle && this == (HTTPCookieContainerHandle)other;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		public override int GetHashCode()
		{
			return this.m_HTTPCookieContainerHandle.GetHashCode();
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0000D4DC File Offset: 0x0000B6DC
		public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0000D504 File Offset: 0x0000B704
		public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return !(x == y);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0000D524 File Offset: 0x0000B724
		public static explicit operator HTTPCookieContainerHandle(uint value)
		{
			return new HTTPCookieContainerHandle(value);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0000D540 File Offset: 0x0000B740
		public static explicit operator uint(HTTPCookieContainerHandle that)
		{
			return that.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0000D55C File Offset: 0x0000B75C
		public bool Equals(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0000D580 File Offset: 0x0000B780
		public int CompareTo(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
		}

		// Token: 0x04000669 RID: 1641
		public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0u);

		// Token: 0x0400066A RID: 1642
		public uint m_HTTPCookieContainerHandle;
	}
}
