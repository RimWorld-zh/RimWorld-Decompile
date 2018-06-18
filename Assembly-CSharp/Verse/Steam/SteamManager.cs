using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000FBC RID: 4028
	public static class SteamManager
	{
		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x0600613D RID: 24893 RVA: 0x00310FC8 File Offset: 0x0030F3C8
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x0600613E RID: 24894 RVA: 0x00310FE4 File Offset: 0x0030F3E4
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x00310FFC File Offset: 0x0030F3FC
		public static void InitIfNeeded()
		{
			if (!SteamManager.initializedInt)
			{
				if (!Packsize.Test())
				{
					Log.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", false);
				}
				if (!DllCheck.Test())
				{
					Log.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", false);
				}
				try
				{
					if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
					{
						Application.Quit();
						return;
					}
				}
				catch (DllNotFoundException arg)
				{
					Log.Error("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, false);
					Application.Quit();
					return;
				}
				SteamManager.initializedInt = SteamAPI.Init();
				if (!SteamManager.initializedInt)
				{
					Log.Warning("[Steamworks.NET] SteamAPI.Init() failed. Possible causes: Steam client not running, launched from outside Steam without steam_appid.txt in place, running with different privileges than Steam client (e.g. \"as administrator\")", false);
				}
				else
				{
					if (SteamManager.steamAPIWarningMessageHook == null)
					{
						SteamManager.steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
						SteamClient.SetWarningMessageHook(SteamManager.steamAPIWarningMessageHook);
					}
					Workshop.Init();
				}
			}
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x003110E8 File Offset: 0x0030F4E8
		public static void Update()
		{
			if (SteamManager.initializedInt)
			{
				SteamAPI.RunCallbacks();
			}
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x003110FF File Offset: 0x0030F4FF
		public static void ShutdownSteam()
		{
			if (SteamManager.initializedInt)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x06006142 RID: 24898 RVA: 0x00311116 File Offset: 0x0030F516
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString(), false);
		}

		// Token: 0x04003F99 RID: 16281
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x04003F9A RID: 16282
		private static bool initializedInt = false;
	}
}
