using System;

namespace Steamworks
{
	// Token: 0x0200014F RID: 335
	public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
	{
		// Token: 0x04000655 RID: 1621
		public int m_HSteamPipe;

		// Token: 0x06000728 RID: 1832 RVA: 0x0000C2B1 File Offset: 0x0000A4B1
		public HSteamPipe(int value)
		{
			this.m_HSteamPipe = value;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		public override string ToString()
		{
			return this.m_HSteamPipe.ToString();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		public override bool Equals(object other)
		{
			return other is HSteamPipe && this == (HSteamPipe)other;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0000C318 File Offset: 0x0000A518
		public override int GetHashCode()
		{
			return this.m_HSteamPipe.GetHashCode();
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0000C340 File Offset: 0x0000A540
		public static bool operator ==(HSteamPipe x, HSteamPipe y)
		{
			return x.m_HSteamPipe == y.m_HSteamPipe;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000C368 File Offset: 0x0000A568
		public static bool operator !=(HSteamPipe x, HSteamPipe y)
		{
			return !(x == y);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000C388 File Offset: 0x0000A588
		public static explicit operator HSteamPipe(int value)
		{
			return new HSteamPipe(value);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000C3A4 File Offset: 0x0000A5A4
		public static explicit operator int(HSteamPipe that)
		{
			return that.m_HSteamPipe;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
		public bool Equals(HSteamPipe other)
		{
			return this.m_HSteamPipe == other.m_HSteamPipe;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
		public int CompareTo(HSteamPipe other)
		{
			return this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
		}
	}
}
