using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000031 RID: 49
	public static class Packsize
	{
		// Token: 0x04000040 RID: 64
		public const int value = 8;

		// Token: 0x060000BD RID: 189 RVA: 0x0000348C File Offset: 0x0000168C
		public static bool Test()
		{
			int num = Marshal.SizeOf(typeof(Packsize.ValvePackingSentinel_t));
			int num2 = Marshal.SizeOf(typeof(RemoteStorageEnumerateUserSubscribedFilesResult_t));
			return num == 32 && num2 == 616;
		}

		// Token: 0x02000032 RID: 50
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		private struct ValvePackingSentinel_t
		{
			// Token: 0x04000041 RID: 65
			private uint m_u32;

			// Token: 0x04000042 RID: 66
			private ulong m_u64;

			// Token: 0x04000043 RID: 67
			private ushort m_u16;

			// Token: 0x04000044 RID: 68
			private double m_d;
		}
	}
}
