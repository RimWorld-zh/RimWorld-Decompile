using System;

namespace Steamworks
{
	// Token: 0x02000159 RID: 345
	public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
	{
		// Token: 0x04000667 RID: 1639
		public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0u);

		// Token: 0x04000668 RID: 1640
		public uint m_HHTMLBrowser;

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000D2E4 File Offset: 0x0000B4E4
		public HHTMLBrowser(uint value)
		{
			this.m_HHTMLBrowser = value;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0000D2F0 File Offset: 0x0000B4F0
		public override string ToString()
		{
			return this.m_HHTMLBrowser.ToString();
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0000D318 File Offset: 0x0000B518
		public override bool Equals(object other)
		{
			return other is HHTMLBrowser && this == (HHTMLBrowser)other;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0000D34C File Offset: 0x0000B54C
		public override int GetHashCode()
		{
			return this.m_HHTMLBrowser.GetHashCode();
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0000D374 File Offset: 0x0000B574
		public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
		{
			return x.m_HHTMLBrowser == y.m_HHTMLBrowser;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0000D39C File Offset: 0x0000B59C
		public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y)
		{
			return !(x == y);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		public static explicit operator HHTMLBrowser(uint value)
		{
			return new HHTMLBrowser(value);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		public static explicit operator uint(HHTMLBrowser that)
		{
			return that.m_HHTMLBrowser;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0000D3F4 File Offset: 0x0000B5F4
		public bool Equals(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser == other.m_HHTMLBrowser;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0000D418 File Offset: 0x0000B618
		public int CompareTo(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
		}
	}
}
