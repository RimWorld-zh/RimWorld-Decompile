using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FB2 RID: 4018
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x04003F86 RID: 16262
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04003F87 RID: 16263
		private static Texture2D fadeTexture;

		// Token: 0x04003F88 RID: 16264
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F89 RID: 16265
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F8A RID: 16266
		private static float sourceTime = 0f;

		// Token: 0x04003F8B RID: 16267
		private static float targetTime = 0f;

		// Token: 0x04003F8C RID: 16268
		private static bool fadeTextureDirty = true;

		// Token: 0x06006136 RID: 24886 RVA: 0x00311E98 File Offset: 0x00310298
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06006137 RID: 24887 RVA: 0x00311F3C File Offset: 0x0031033C
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x00311F58 File Offset: 0x00310358
		public static void OverlayOnGUI(Vector2 windowSize)
		{
			Color color = ScreenFader.CurrentInstantColor();
			if (color.a > 0f)
			{
				if (ScreenFader.fadeTextureDirty)
				{
					ScreenFader.fadeTexture.SetPixel(0, 0, color);
					ScreenFader.fadeTexture.Apply();
				}
				GUI.Label(new Rect(-10f, -10f, windowSize.x + 10f, windowSize.y + 10f), ScreenFader.fadeTexture, ScreenFader.backgroundStyle);
			}
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x00311FDC File Offset: 0x003103DC
		private static Color CurrentInstantColor()
		{
			Color result;
			if (ScreenFader.CurTime > ScreenFader.targetTime || ScreenFader.targetTime == ScreenFader.sourceTime)
			{
				result = ScreenFader.targetColor;
			}
			else
			{
				result = Color.Lerp(ScreenFader.sourceColor, ScreenFader.targetColor, (ScreenFader.CurTime - ScreenFader.sourceTime) / (ScreenFader.targetTime - ScreenFader.sourceTime));
			}
			return result;
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x00312042 File Offset: 0x00310442
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x0031206B File Offset: 0x0031046B
		public static void StartFade(Color finalColor, float duration)
		{
			if (duration <= 0f)
			{
				ScreenFader.SetColor(finalColor);
			}
			else
			{
				ScreenFader.sourceColor = ScreenFader.CurrentInstantColor();
				ScreenFader.targetColor = finalColor;
				ScreenFader.sourceTime = ScreenFader.CurTime;
				ScreenFader.targetTime = ScreenFader.CurTime + duration;
			}
		}
	}
}
