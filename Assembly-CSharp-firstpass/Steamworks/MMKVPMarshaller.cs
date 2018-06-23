using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200002F RID: 47
	public class MMKVPMarshaller
	{
		// Token: 0x0400003E RID: 62
		private IntPtr m_pNativeArray;

		// Token: 0x0400003F RID: 63
		private IntPtr m_pArrayEntries;

		// Token: 0x060000B5 RID: 181 RVA: 0x00003254 File Offset: 0x00001454
		public MMKVPMarshaller(MatchMakingKeyValuePair_t[] filters)
		{
			if (filters != null)
			{
				int num = Marshal.SizeOf(typeof(MatchMakingKeyValuePair_t));
				this.m_pNativeArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * filters.Length);
				this.m_pArrayEntries = Marshal.AllocHGlobal(num * filters.Length);
				for (int i = 0; i < filters.Length; i++)
				{
					Marshal.StructureToPtr(filters[i], new IntPtr(this.m_pArrayEntries.ToInt64() + (long)(i * num)), false);
				}
				Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003308 File Offset: 0x00001508
		~MMKVPMarshaller()
		{
			if (this.m_pArrayEntries != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pArrayEntries);
			}
			if (this.m_pNativeArray != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pNativeArray);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003378 File Offset: 0x00001578
		public static implicit operator IntPtr(MMKVPMarshaller that)
		{
			return that.m_pNativeArray;
		}
	}
}
