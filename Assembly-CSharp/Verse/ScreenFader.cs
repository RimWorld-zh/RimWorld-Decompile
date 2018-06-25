using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FB3 RID: 4019
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x04003F8E RID: 16270
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04003F8F RID: 16271
		private static Texture2D fadeTexture;

		// Token: 0x04003F90 RID: 16272
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F91 RID: 16273
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F92 RID: 16274
		private static float sourceTime = 0f;

		// Token: 0x04003F93 RID: 16275
		private static float targetTime = 0f;

		// Token: 0x04003F94 RID: 16276
		private static bool fadeTextureDirty = true;

		// Token: 0x06006136 RID: 24886 RVA: 0x003120DC File Offset: 0x003104DC
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06006137 RID: 24887 RVA: 0x00312180 File Offset: 0x00310580
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x0031219C File Offset: 0x0031059C
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

		// Token: 0x06006139 RID: 24889 RVA: 0x00312220 File Offset: 0x00310620
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

		// Token: 0x0600613A RID: 24890 RVA: 0x00312286 File Offset: 0x00310686
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x003122AF File Offset: 0x003106AF
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
