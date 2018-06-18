using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000152 RID: 338
	// (Invoke) Token: 0x06000741 RID: 1857
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}
