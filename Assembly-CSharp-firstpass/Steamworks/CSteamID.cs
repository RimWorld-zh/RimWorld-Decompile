using System;

namespace Steamworks
{
	// Token: 0x02000156 RID: 342
	public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID>
	{
		// Token: 0x0400065D RID: 1629
		public static readonly CSteamID Nil = default(CSteamID);

		// Token: 0x0400065E RID: 1630
		public static readonly CSteamID OutofDateGS = new CSteamID(new AccountID_t(0u), 0u, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x0400065F RID: 1631
		public static readonly CSteamID LanModeGS = new CSteamID(new AccountID_t(0u), 0u, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000660 RID: 1632
		public static readonly CSteamID NotInitYetGS = new CSteamID(new AccountID_t(1u), 0u, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000661 RID: 1633
		public static readonly CSteamID NonSteamGS = new CSteamID(new AccountID_t(2u), 0u, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000662 RID: 1634
		public ulong m_SteamID;

		// Token: 0x06000761 RID: 1889 RVA: 0x0000C92B File Offset: 0x0000AB2B
		public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.Set(unAccountID, eUniverse, eAccountType);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0000C93F File Offset: 0x0000AB3F
		public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000C955 File Offset: 0x0000AB55
		public CSteamID(ulong ulSteamID)
		{
			this.m_SteamID = ulSteamID;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0000C95F File Offset: 0x0000AB5F
		public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer)
			{
				this.SetAccountInstance(0u);
			}
			else
			{
				this.SetAccountInstance(1u);
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0000C99C File Offset: 0x0000AB9C
		public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			this.SetAccountInstance(unInstance);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000C9BC File Offset: 0x0000ABBC
		public void Clear()
		{
			this.m_SteamID = 0UL;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0000C9C7 File Offset: 0x0000ABC7
		public void CreateBlankAnonLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0u));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
			this.SetAccountInstance(0u);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0000C9EB File Offset: 0x0000ABEB
		public void CreateBlankAnonUserLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0u));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
			this.SetAccountInstance(0u);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0000CA10 File Offset: 0x0000AC10
		public bool BBlankAnonAccount()
		{
			return this.GetAccountID() == new AccountID_t(0u) && this.BAnonAccount() && this.GetUnAccountInstance() == 0u;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000CA54 File Offset: 0x0000AC54
		public bool BGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0000CA84 File Offset: 0x0000AC84
		public bool BPersistentGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0000CAA4 File Offset: 0x0000ACA4
		public bool BAnonGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0000CAC4 File Offset: 0x0000ACC4
		public bool BContentServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeContentServer;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		public bool BClanAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeClan;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0000CB04 File Offset: 0x0000AD04
		public bool BChatAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000CB24 File Offset: 0x0000AD24
		public bool IsLobby()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat && (this.GetUnAccountInstance() & 262144u) != 0u;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000CB5C File Offset: 0x0000AD5C
		public bool BIndividualAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual || this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0000CB8C File Offset: 0x0000AD8C
		public bool BAnonAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000CBBC File Offset: 0x0000ADBC
		public bool BAnonUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000CBDC File Offset: 0x0000ADDC
		public bool BConsoleUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0000CBFB File Offset: 0x0000ADFB
		public void SetAccountID(AccountID_t other)
		{
			this.m_SteamID = ((this.m_SteamID & 18446744069414584320UL) | ((ulong)((uint)other) & (ulong)-1) << 0);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0000CC21 File Offset: 0x0000AE21
		public void SetAccountInstance(uint other)
		{
			this.m_SteamID = ((this.m_SteamID & 18442240478377148415UL) | ((ulong)other & 1048575UL) << 32);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0000CC47 File Offset: 0x0000AE47
		public void SetEAccountType(EAccountType other)
		{
			this.m_SteamID = ((this.m_SteamID & 18379190079298994175UL) | (ulong)((ulong)((long)other & 15L) << 52));
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0000CC6A File Offset: 0x0000AE6A
		public void SetEUniverse(EUniverse other)
		{
			this.m_SteamID = ((this.m_SteamID & 72057594037927935UL) | (ulong)((ulong)((long)other & 255L) << 56));
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0000CC90 File Offset: 0x0000AE90
		public void ClearIndividualInstance()
		{
			if (this.BIndividualAccount())
			{
				this.SetAccountInstance(0u);
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0000CCA8 File Offset: 0x0000AEA8
		public bool HasNoIndividualInstance()
		{
			return this.BIndividualAccount() && this.GetUnAccountInstance() == 0u;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0000CCD4 File Offset: 0x0000AED4
		public AccountID_t GetAccountID()
		{
			return new AccountID_t((uint)(this.m_SteamID & (ulong)-1));
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0000CCF8 File Offset: 0x0000AEF8
		public uint GetUnAccountInstance()
		{
			return (uint)(this.m_SteamID >> 32 & 1048575UL);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0000CD20 File Offset: 0x0000AF20
		public EAccountType GetEAccountType()
		{
			return (EAccountType)(this.m_SteamID >> 52 & 15UL);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000CD44 File Offset: 0x0000AF44
		public EUniverse GetEUniverse()
		{
			return (EUniverse)(this.m_SteamID >> 56 & 255UL);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000CD6C File Offset: 0x0000AF6C
		public bool IsValid()
		{
			bool result;
			if (this.GetEAccountType() <= EAccountType.k_EAccountTypeInvalid || this.GetEAccountType() >= EAccountType.k_EAccountTypeMax)
			{
				result = false;
			}
			else if (this.GetEUniverse() <= EUniverse.k_EUniverseInvalid || this.GetEUniverse() >= EUniverse.k_EUniverseMax)
			{
				result = false;
			}
			else
			{
				if (this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual)
				{
					if (this.GetAccountID() == new AccountID_t(0u) || this.GetUnAccountInstance() > 4u)
					{
						return false;
					}
				}
				if (this.GetEAccountType() == EAccountType.k_EAccountTypeClan)
				{
					if (this.GetAccountID() == new AccountID_t(0u) || this.GetUnAccountInstance() != 0u)
					{
						return false;
					}
				}
				if (this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer)
				{
					if (this.GetAccountID() == new AccountID_t(0u))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public override string ToString()
		{
			return this.m_SteamID.ToString();
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0000CE84 File Offset: 0x0000B084
		public override bool Equals(object other)
		{
			return other is CSteamID && this == (CSteamID)other;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0000CEB8 File Offset: 0x0000B0B8
		public override int GetHashCode()
		{
			return this.m_SteamID.GetHashCode();
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0000CEE0 File Offset: 0x0000B0E0
		public static bool operator ==(CSteamID x, CSteamID y)
		{
			return x.m_SteamID == y.m_SteamID;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0000CF08 File Offset: 0x0000B108
		public static bool operator !=(CSteamID x, CSteamID y)
		{
			return !(x == y);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0000CF28 File Offset: 0x0000B128
		public static explicit operator CSteamID(ulong value)
		{
			return new CSteamID(value);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0000CF44 File Offset: 0x0000B144
		public static explicit operator ulong(CSteamID that)
		{
			return that.m_SteamID;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0000CF60 File Offset: 0x0000B160
		public bool Equals(CSteamID other)
		{
			return this.m_SteamID == other.m_SteamID;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0000CF84 File Offset: 0x0000B184
		public int CompareTo(CSteamID other)
		{
			return this.m_SteamID.CompareTo(other.m_SteamID);
		}
	}
}
