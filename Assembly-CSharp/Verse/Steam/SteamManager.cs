using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000FBD RID: 4029
	public static class SteamManager
	{
		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06006166 RID: 24934 RVA: 0x0031309C File Offset: 0x0031149C
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06006167 RID: 24935 RVA: 0x003130B8 File Offset: 0x003114B8
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006168 RID: 24936 RVA: 0x003130D0 File Offset: 0x003114D0
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

		// Token: 0x06006169 RID: 24937 RVA: 0x003131BC File Offset: 0x003115BC
		public static void Update()
		{
			if (SteamManager.initializedInt)
			{
				SteamAPI.RunCallbacks();
			}
		}

		// Token: 0x0600616A RID: 24938 RVA: 0x003131D3 File Offset: 0x003115D3
		public static void ShutdownSteam()
		{
			if (SteamManager.initializedInt)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x0600616B RID: 24939 RVA: 0x003131EA File Offset: 0x003115EA
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString(), false);
		}

		// Token: 0x04003FB6 RID: 16310
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x04003FB7 RID: 16311
		private static bool initializedInt = false;
	}
}
