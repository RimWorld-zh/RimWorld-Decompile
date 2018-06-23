using System;

namespace Steamworks
{
	// Token: 0x02000167 RID: 359
	public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
	{
		// Token: 0x04000680 RID: 1664
		public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0u);

		// Token: 0x04000681 RID: 1665
		public uint m_ScreenshotHandle;

		// Token: 0x06000836 RID: 2102 RVA: 0x0000E651 File Offset: 0x0000C851
		public ScreenshotHandle(uint value)
		{
			this.m_ScreenshotHandle = value;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0000E65C File Offset: 0x0000C85C
		public override string ToString()
		{
			return this.m_ScreenshotHandle.ToString();
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0000E684 File Offset: 0x0000C884
		public override bool Equals(object other)
		{
			return other is ScreenshotHandle && this == (ScreenshotHandle)other;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0000E6B8 File Offset: 0x0000C8B8
		public override int GetHashCode()
		{
			return this.m_ScreenshotHandle.GetHashCode();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
		public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
		{
			return x.m_ScreenshotHandle == y.m_ScreenshotHandle;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0000E708 File Offset: 0x0000C908
		public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y)
		{
			return !(x == y);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0000E728 File Offset: 0x0000C928
		public static explicit operator ScreenshotHandle(uint value)
		{
			return new ScreenshotHandle(value);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0000E744 File Offset: 0x0000C944
		public static explicit operator uint(ScreenshotHandle that)
		{
			return that.m_ScreenshotHandle;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0000E760 File Offset: 0x0000C960
		public bool Equals(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle == other.m_ScreenshotHandle;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0000E784 File Offset: 0x0000C984
		public int CompareTo(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
		}
	}
}
