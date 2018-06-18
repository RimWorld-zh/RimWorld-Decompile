using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000153 RID: 339
	// (Invoke) Token: 0x06000745 RID: 1861
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void SteamAPI_PostAPIResultInProcess_t(SteamAPICall_t callHandle, IntPtr pUnknown, uint unCallbackSize, int iCallbackNum);
}
