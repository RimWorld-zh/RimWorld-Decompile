using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080B RID: 2059
	public static class ResolutionUtility
	{
		// Token: 0x04001870 RID: 6256
		public const int MinResolutionWidth = 1024;

		// Token: 0x04001871 RID: 6257
		public const int MinResolutionHeight = 768;

		// Token: 0x06002E06 RID: 11782 RVA: 0x0018441C File Offset: 0x0018281C
		public static void SafeSetResolution(Resolution res)
		{
			if (Screen.width != res.width || Screen.height != res.height)
			{
				IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
				Screen.SetResolution(res.width, res.height, Screen.fullScreen);
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
			}
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x0018448C File Offset: 0x0018288C
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen != fullScreen)
			{
				bool fullScreen2 = Screen.fullScreen;
				Screen.fullScreen = fullScreen;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
			}
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x001844C8 File Offset: 0x001828C8
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScale != newScale)
			{
				float uiscale = Prefs.UIScale;
				Prefs.UIScale = newScale;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscale));
			}
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x00184504 File Offset: 0x00182904
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}
	}
}
