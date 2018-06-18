using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Sequential)]
	internal class CCallbackBaseVTable
	{
		// Token: 0x04000014 RID: 20
		private const CallingConvention cc = CallingConvention.Cdecl;

		// Token: 0x04000015 RID: 21
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.RunCRDel m_RunCallResult;

		// Token: 0x04000016 RID: 22
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.RunCBDel m_RunCallback;

		// Token: 0x04000017 RID: 23
		[NonSerialized]
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

		// Token: 0x02000009 RID: 9
		// (Invoke) Token: 0x06000028 RID: 40
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void RunCBDel(IntPtr thisptr, IntPtr pvParam);

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x0600002C RID: 44
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void RunCRDel(IntPtr thisptr, IntPtr pvParam, [MarshalAs(UnmanagedType.I1)] bool bIOFailure, ulong hSteamAPICall);

		// Token: 0x0200000B RID: 11
		// (Invoke) Token: 0x06000030 RID: 48
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int GetCallbackSizeBytesDel(IntPtr thisptr);
	}
}
