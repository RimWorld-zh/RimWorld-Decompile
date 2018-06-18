using System;

namespace Steamworks
{
	// Token: 0x02000142 RID: 322
	public static class SteamMusic
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x00008A24 File Offset: 0x00006C24
		public static bool BIsEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsEnabled();
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00008A44 File Offset: 0x00006C44
		public static bool BIsPlaying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsPlaying();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00008A64 File Offset: 0x00006C64
		public static AudioPlayback_Status GetPlaybackStatus()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetPlaybackStatus();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00008A83 File Offset: 0x00006C83
		public static void Play()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Play();
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00008A90 File Offset: 0x00006C90
		public static void Pause()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Pause();
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00008A9D File Offset: 0x00006C9D
		public static void PlayPrevious()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayPrevious();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00008AAA File Offset: 0x00006CAA
		public static void PlayNext()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayNext();
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00008AB7 File Offset: 0x00006CB7
		public static void SetVolume(float flVolume)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_SetVolume(flVolume);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00008AC8 File Offset: 0x00006CC8
		public static float GetVolume()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetVolume();
		}
	}
}
