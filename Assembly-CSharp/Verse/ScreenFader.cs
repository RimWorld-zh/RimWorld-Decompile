using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FAE RID: 4014
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x06006103 RID: 24835 RVA: 0x0030F774 File Offset: 0x0030DB74
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06006104 RID: 24836 RVA: 0x0030F818 File Offset: 0x0030DC18
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06006105 RID: 24837 RVA: 0x0030F834 File Offset: 0x0030DC34
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

		// Token: 0x06006106 RID: 24838 RVA: 0x0030F8B8 File Offset: 0x0030DCB8
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

		// Token: 0x06006107 RID: 24839 RVA: 0x0030F91E File Offset: 0x0030DD1E
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x0030F947 File Offset: 0x0030DD47
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

		// Token: 0x04003F71 RID: 16241
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04003F72 RID: 16242
		private static Texture2D fadeTexture;

		// Token: 0x04003F73 RID: 16243
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F74 RID: 16244
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F75 RID: 16245
		private static float sourceTime = 0f;

		// Token: 0x04003F76 RID: 16246
		private static float targetTime = 0f;

		// Token: 0x04003F77 RID: 16247
		private static bool fadeTextureDirty = true;
	}
}
