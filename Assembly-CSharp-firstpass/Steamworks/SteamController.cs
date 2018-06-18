using System;

namespace Steamworks
{
	// Token: 0x02000134 RID: 308
	public static class SteamController
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00004480 File Offset: 0x00002680
		public static bool Init(string pchAbsolutePathToControllerConfigVDF)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchAbsolutePathToControllerConfigVDF))
			{
				result = NativeMethods.ISteamController_Init(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000044C8 File Offset: 0x000026C8
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_Shutdown();
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000044E7 File Offset: 0x000026E7
		public static void RunFrame()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_RunFrame();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000044F4 File Offset: 0x000026F4
		public static bool GetControllerState(uint unControllerIndex, out SteamControllerState_t pState)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamController_GetControllerState(unControllerIndex, out pState);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00004515 File Offset: 0x00002715
		public static void TriggerHapticPulse(uint unControllerIndex, ESteamControllerPad eTargetPad, ushort usDurationMicroSec)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamController_TriggerHapticPulse(unControllerIndex, eTargetPad, usDurationMicroSec);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00004528 File Offset: 0x00002728
		public static void SetOverrideMode(string pchMode)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMode))
			{
				NativeMethods.ISteamController_SetOverrideMode(utf8StringHandle);
			}
		}
	}
}
