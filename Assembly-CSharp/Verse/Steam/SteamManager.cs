using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000FC1 RID: 4033
	public static class SteamManager
	{
		// Token: 0x04003FBE RID: 16318
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x04003FBF RID: 16319
		private static bool initializedInt = false;

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06006176 RID: 24950 RVA: 0x00313B7C File Offset: 0x00311F7C
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06006177 RID: 24951 RVA: 0x00313B98 File Offset: 0x00311F98
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x00313BB0 File Offset: 0x00311FB0
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

		// Token: 0x06006179 RID: 24953 RVA: 0x00313C9C File Offset: 0x0031209C
		public static void Update()
		{
			if (SteamManager.initializedInt)
			{
				SteamAPI.RunCallbacks();
			}
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x00313CB3 File Offset: 0x003120B3
		public static void ShutdownSteam()
		{
			if (SteamManager.initializedInt)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x00313CCA File Offset: 0x003120CA
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString(), false);
		}
	}
}
