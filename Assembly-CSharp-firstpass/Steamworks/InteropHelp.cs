using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Steamworks
{
	public class InteropHelp
	{
		public InteropHelp()
		{
		}

		public static void TestIfPlatformSupported()
		{
		}

		public static void TestIfAvailableClient()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamClient() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		public static void TestIfAvailableGameServer()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamClientGameServer() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

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

		public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
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

			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					Marshal.FreeHGlobal(this.handle);
				}
				return true;
			}
		}

		public class SteamParamStringArray
		{
			private IntPtr[] m_Strings;

			private IntPtr m_ptrStrings;

			private IntPtr m_pSteamParamStringArray;

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

			public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that)
			{
				return that.m_pSteamParamStringArray;
			}
		}
	}
}
