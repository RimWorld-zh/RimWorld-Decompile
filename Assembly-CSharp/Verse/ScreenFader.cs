using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FAE RID: 4014
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x04003F83 RID: 16259
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04003F84 RID: 16260
		private static Texture2D fadeTexture;

		// Token: 0x04003F85 RID: 16261
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F86 RID: 16262
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F87 RID: 16263
		private static float sourceTime = 0f;

		// Token: 0x04003F88 RID: 16264
		private static float targetTime = 0f;

		// Token: 0x04003F89 RID: 16265
		private static bool fadeTextureDirty = true;

		// Token: 0x0600612C RID: 24876 RVA: 0x00311818 File Offset: 0x0030FC18
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x0600612D RID: 24877 RVA: 0x003118BC File Offset: 0x0030FCBC
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x0600612E RID: 24878 RVA: 0x003118D8 File Offset: 0x0030FCD8
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

		// Token: 0x0600612F RID: 24879 RVA: 0x0031195C File Offset: 0x0030FD5C
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

		// Token: 0x06006130 RID: 24880 RVA: 0x003119C2 File Offset: 0x0030FDC2
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x003119EB File Offset: 0x0030FDEB
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
