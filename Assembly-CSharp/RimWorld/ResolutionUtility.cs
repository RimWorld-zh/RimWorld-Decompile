using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080F RID: 2063
	public static class ResolutionUtility
	{
		// Token: 0x06002E0D RID: 11789 RVA: 0x00184244 File Offset: 0x00182644
		public static void SafeSetResolution(Resolution res)
		{
			if (Screen.width != res.width || Screen.height != res.height)
			{
				IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
				Screen.SetResolution(res.width, res.height, Screen.fullScreen);
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
			}
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x001842B4 File Offset: 0x001826B4
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen != fullScreen)
			{
				bool fullScreen2 = Screen.fullScreen;
				Screen.fullScreen = fullScreen;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x001842F0 File Offset: 0x001826F0
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScale != newScale)
			{
				float uiscale = Prefs.UIScale;
				Prefs.UIScale = newScale;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscale));
			}
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x0018432C File Offset: 0x0018272C
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}

		// Token: 0x04001872 RID: 6258
		public const int MinResolutionWidth = 1024;

		// Token: 0x04001873 RID: 6259
		public const int MinResolutionHeight = 768;
	}
}
