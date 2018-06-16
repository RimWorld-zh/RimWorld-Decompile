using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FAF RID: 4015
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x06006105 RID: 24837 RVA: 0x0030F698 File Offset: 0x0030DA98
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06006106 RID: 24838 RVA: 0x0030F73C File Offset: 0x0030DB3C
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x0030F758 File Offset: 0x0030DB58
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

		// Token: 0x06006108 RID: 24840 RVA: 0x0030F7DC File Offset: 0x0030DBDC
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

		// Token: 0x06006109 RID: 24841 RVA: 0x0030F842 File Offset: 0x0030DC42
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x0600610A RID: 24842 RVA: 0x0030F86B File Offset: 0x0030DC6B
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

		// Token: 0x04003F72 RID: 16242
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04003F73 RID: 16243
		private static Texture2D fadeTexture;

		// Token: 0x04003F74 RID: 16244
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F75 RID: 16245
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F76 RID: 16246
		private static float sourceTime = 0f;

		// Token: 0x04003F77 RID: 16247
		private static float targetTime = 0f;

		// Token: 0x04003F78 RID: 16248
		private static bool fadeTextureDirty = true;
	}
}
