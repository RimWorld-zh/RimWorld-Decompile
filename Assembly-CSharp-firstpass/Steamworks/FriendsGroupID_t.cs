using System;

namespace Steamworks
{
	// Token: 0x02000158 RID: 344
	public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
	{
		// Token: 0x06000795 RID: 1941 RVA: 0x0000D17C File Offset: 0x0000B37C
		public FriendsGroupID_t(short value)
		{
			this.m_FriendsGroupID = value;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0000D188 File Offset: 0x0000B388
		public override string ToString()
		{
			return this.m_FriendsGroupID.ToString();
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		public override bool Equals(object other)
		{
			return other is FriendsGroupID_t && this == (FriendsGroupID_t)other;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
		public override int GetHashCode()
		{
			return this.m_FriendsGroupID.GetHashCode();
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000D20C File Offset: 0x0000B40C
		public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return x.m_FriendsGroupID == y.m_FriendsGroupID;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000D234 File Offset: 0x0000B434
		public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return !(x == y);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0000D254 File Offset: 0x0000B454
		public static explicit operator FriendsGroupID_t(short value)
		{
			return new FriendsGroupID_t(value);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0000D270 File Offset: 0x0000B470
		public static explicit operator short(FriendsGroupID_t that)
		{
			return that.m_FriendsGroupID;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0000D28C File Offset: 0x0000B48C
		public bool Equals(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID == other.m_FriendsGroupID;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		public int CompareTo(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
		}

		// Token: 0x04000665 RID: 1637
		public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t(-1);

		// Token: 0x04000666 RID: 1638
		public short m_FriendsGroupID;
	}
}
