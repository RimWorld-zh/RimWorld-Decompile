using System;

namespace Steamworks
{
	// Token: 0x02000168 RID: 360
	public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
	{
		// Token: 0x04000682 RID: 1666
		public uint m_AccountID;

		// Token: 0x06000841 RID: 2113 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		public AccountID_t(uint value)
		{
			this.m_AccountID = value;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		public override string ToString()
		{
			return this.m_AccountID.ToString();
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0000E7EC File Offset: 0x0000C9EC
		public override bool Equals(object other)
		{
			return other is AccountID_t && this == (AccountID_t)other;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0000E820 File Offset: 0x0000CA20
		public override int GetHashCode()
		{
			return this.m_AccountID.GetHashCode();
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0000E848 File Offset: 0x0000CA48
		public static bool operator ==(AccountID_t x, AccountID_t y)
		{
			return x.m_AccountID == y.m_AccountID;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0000E870 File Offset: 0x0000CA70
		public static bool operator !=(AccountID_t x, AccountID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0000E890 File Offset: 0x0000CA90
		public static explicit operator AccountID_t(uint value)
		{
			return new AccountID_t(value);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0000E8AC File Offset: 0x0000CAAC
		public static explicit operator uint(AccountID_t that)
		{
			return that.m_AccountID;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
		public bool Equals(AccountID_t other)
		{
			return this.m_AccountID == other.m_AccountID;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0000E8EC File Offset: 0x0000CAEC
		public int CompareTo(AccountID_t other)
		{
			return this.m_AccountID.CompareTo(other.m_AccountID);
		}
	}
}
