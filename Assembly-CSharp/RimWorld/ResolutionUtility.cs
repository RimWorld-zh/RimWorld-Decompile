using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080D RID: 2061
	public static class ResolutionUtility
	{
		// Token: 0x04001870 RID: 6256
		public const int MinResolutionWidth = 1024;

		// Token: 0x04001871 RID: 6257
		public const int MinResolutionHeight = 768;

		// Token: 0x06002E0A RID: 11786 RVA: 0x0018456C File Offset: 0x0018296C
		public static void SafeSetResolution(Resolution res)
		{
			if (Screen.width != res.width || Screen.height != res.height)
			{
				IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
				Screen.SetResolution(res.width, res.height, Screen.fullScreen);
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
			}
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x001845DC File Offset: 0x001829DC
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen != fullScreen)
			{
				bool fullScreen2 = Screen.fullScreen;
				Screen.fullScreen = fullScreen;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x00184618 File Offset: 0x00182A18
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScale != newScale)
			{
				float uiscale = Prefs.UIScale;
				Prefs.UIScale = newScale;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscale));
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x00184654 File Offset: 0x00182A54
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}
	}
}
