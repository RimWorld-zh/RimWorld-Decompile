using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080D RID: 2061
	public static class ResolutionUtility
	{
		// Token: 0x04001874 RID: 6260
		public const int MinResolutionWidth = 1024;

		// Token: 0x04001875 RID: 6261
		public const int MinResolutionHeight = 768;

		// Token: 0x06002E09 RID: 11785 RVA: 0x001847D0 File Offset: 0x00182BD0
		public static void SafeSetResolution(Resolution res)
		{
			if (Screen.width != res.width || Screen.height != res.height)
			{
				IntVec2 oldRes = new IntVec2(Screen.width, Screen.height);
				Screen.SetResolution(res.width, res.height, Screen.fullScreen);
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(oldRes));
			}
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x00184840 File Offset: 0x00182C40
		public static void SafeSetFullscreen(bool fullScreen)
		{
			if (Screen.fullScreen != fullScreen)
			{
				bool fullScreen2 = Screen.fullScreen;
				Screen.fullScreen = fullScreen;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(fullScreen2));
			}
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x0018487C File Offset: 0x00182C7C
		public static void SafeSetUIScale(float newScale)
		{
			if (Prefs.UIScale != newScale)
			{
				float uiscale = Prefs.UIScale;
				Prefs.UIScale = newScale;
				Find.WindowStack.Add(new Dialog_ResolutionConfirm(uiscale));
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x001848B8 File Offset: 0x00182CB8
		public static bool UIScaleSafeWithResolution(float scale, int w, int h)
		{
			return (float)w / scale >= 1024f && (float)h / scale >= 768f;
		}
	}
}
