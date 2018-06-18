using System;

namespace Steamworks
{
	// Token: 0x0200015F RID: 351
	public struct HServerListRequest : IEquatable<HServerListRequest>
	{
		// Token: 0x060007E1 RID: 2017 RVA: 0x0000DB49 File Offset: 0x0000BD49
		public HServerListRequest(IntPtr value)
		{
			this.m_HServerListRequest = value;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0000DB54 File Offset: 0x0000BD54
		public override string ToString()
		{
			return this.m_HServerListRequest.ToString();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0000DB7C File Offset: 0x0000BD7C
		public override bool Equals(object other)
		{
			return other is HServerListRequest && this == (HServerListRequest)other;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
		public override int GetHashCode()
		{
			return this.m_HServerListRequest.GetHashCode();
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
		public static bool operator ==(HServerListRequest x, HServerListRequest y)
		{
			return x.m_HServerListRequest == y.m_HServerListRequest;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0000DC00 File Offset: 0x0000BE00
		public static bool operator !=(HServerListRequest x, HServerListRequest y)
		{
			return !(x == y);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0000DC20 File Offset: 0x0000BE20
		public static explicit operator HServerListRequest(IntPtr value)
		{
			return new HServerListRequest(value);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		public static explicit operator IntPtr(HServerListRequest that)
		{
			return that.m_HServerListRequest;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0000DC58 File Offset: 0x0000BE58
		public bool Equals(HServerListRequest other)
		{
			return this.m_HServerListRequest == other.m_HServerListRequest;
		}

		// Token: 0x04000672 RID: 1650
		public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);

		// Token: 0x04000673 RID: 1651
		public IntPtr m_HServerListRequest;
	}
}
