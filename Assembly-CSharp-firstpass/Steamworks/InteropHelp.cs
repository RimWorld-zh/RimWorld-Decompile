using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Steamworks
{
	// Token: 0x0200002C RID: 44
	public class InteropHelp
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00002EE8 File Offset: 0x000010E8
		public static void TestIfPlatformSupported()
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002EEB File Offset: 0x000010EB
		public static void TestIfAvailableClient()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamClient() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002F13 File Offset: 0x00001113
		public static void TestIfAvailableGameServer()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamClientGameServer() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00002F3C File Offset: 0x0000113C
		public static string PtrToStringUTF8(IntPtr nativeUtf8)
		{
			string result;
			if (nativeUtf8 == IntPtr.Zero)
			{
				result = string.Empty;
			}
			else
			{
				int num = 0;
				while (Marshal.ReadByte(nativeUtf8, num) != 0)
				{
					num++;
				}
				if (num == 0)
				{
					result = string.Empty;
				}
				else
				{
					byte[] array = new byte[num];
					Marshal.Copy(nativeUtf8, array, 0, array.Length);
					result = Encoding.UTF8.GetString(array);
				}
			}
			return result;
		}

		// Token: 0x0200002D RID: 45
		public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x060000B0 RID: 176 RVA: 0x00002FB8 File Offset: 0x000011B8
			public UTF8StringHandle(string str) : base(true)
			{
				if (str == null)
				{
					base.SetHandle(IntPtr.Zero);
				}
				else
				{
					byte[] array = new byte[Encoding.UTF8.GetByteCount(str) + 1];
					Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
					IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
					Marshal.Copy(array, 0, intPtr, array.Length);
					base.SetHandle(intPtr);
				}
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00003028 File Offset: 0x00001228
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					Marshal.FreeHGlobal(this.handle);
				}
				return true;
			}
		}

		// Token: 0x0200002E RID: 46
		public class SteamParamStringArray
		{
			// Token: 0x0400003B RID: 59
			private IntPtr[] m_Strings;

			// Token: 0x0400003C RID: 60
			private IntPtr m_ptrStrings;

			// Token: 0x0400003D RID: 61
			private IntPtr m_pSteamParamStringArray;

			// Token: 0x060000B2 RID: 178 RVA: 0x00003058 File Offset: 0x00001258
			public SteamParamStringArray(IList<string> strings)
			{
				if (strings == null)
				{
					this.m_pSteamParamStringArray = IntPtr.Zero;
				}
				else
				{
					this.m_Strings = new IntPtr[strings.Count];
					for (int i = 0; i < strings.Count; i++)
					{
						byte[] array = new byte[Encoding.UTF8.GetByteCount(strings[i]) + 1];
						Encoding.UTF8.GetBytes(strings[i], 0, strings[i].Length, array, 0);
						this.m_Strings[i] = Marshal.AllocHGlobal(array.Length);
						Marshal.Copy(array, 0, this.m_Strings[i], array.Length);
					}
					this.m_ptrStrings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * this.m_Strings.Length);
					SteamParamStringArray_t steamParamStringArray_t = new SteamParamStringArray_t
					{
						m_ppStrings = this.m_ptrStrings,
						m_nNumStrings = this.m_Strings.Length
					};
					Marshal.Copy(this.m_Strings, 0, steamParamStringArray_t.m_ppStrings, this.m_Strings.Length);
					this.m_pSteamParamStringArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SteamParamStringArray_t)));
					Marshal.StructureToPtr(steamParamStringArray_t, this.m_pSteamParamStringArray, false);
				}
			}

			// Token: 0x060000B3 RID: 179 RVA: 0x000031A0 File Offset: 0x000013A0
			protected override void Finalize()
			{
				try
				{
					foreach (IntPtr hglobal in this.m_Strings)
					{
						Marshal.FreeHGlobal(hglobal);
					}
					if (this.m_ptrStrings != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_ptrStrings);
					}
					if (this.m_pSteamParamStringArray != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x00003238 File Offset: 0x00001438
			public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that)
			{
				return that.m_pSteamParamStringArray;
			}
		}
	}
}
