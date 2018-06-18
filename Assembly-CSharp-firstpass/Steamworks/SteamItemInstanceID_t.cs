using System;

namespace Steamworks
{
	// Token: 0x0200015E RID: 350
	public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
	{
		// Token: 0x060007D6 RID: 2006 RVA: 0x0000D9DF File Offset: 0x0000BBDF
		public SteamItemInstanceID_t(ulong value)
		{
			this.m_SteamItemInstanceID = value;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0000D9EC File Offset: 0x0000BBEC
		public override string ToString()
		{
			return this.m_SteamItemInstanceID.ToString();
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0000DA14 File Offset: 0x0000BC14
		public override bool Equals(object other)
		{
			return other is SteamItemInstanceID_t && this == (SteamItemInstanceID_t)other;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0000DA48 File Offset: 0x0000BC48
		public override int GetHashCode()
		{
			return this.m_SteamItemInstanceID.GetHashCode();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0000DA70 File Offset: 0x0000BC70
		public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0000DA98 File Offset: 0x0000BC98
		public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return !(x == y);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
		public static explicit operator SteamItemInstanceID_t(ulong value)
		{
			return new SteamItemInstanceID_t(value);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0000DAD4 File Offset: 0x0000BCD4
		public static explicit operator ulong(SteamItemInstanceID_t that)
		{
			return that.m_SteamItemInstanceID;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0000DAF0 File Offset: 0x0000BCF0
		public bool Equals(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID == other.m_SteamItemInstanceID;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0000DB14 File Offset: 0x0000BD14
		public int CompareTo(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
		}

		// Token: 0x04000670 RID: 1648
		public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);

		// Token: 0x04000671 RID: 1649
		public ulong m_SteamItemInstanceID;
	}
}
