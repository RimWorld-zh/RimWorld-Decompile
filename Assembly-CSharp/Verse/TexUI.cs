using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAA RID: 3754
	[StaticConstructorOnStartup]
	public static class TexUI
	{
		// Token: 0x06005865 RID: 22629 RVA: 0x002D4A38 File Offset: 0x002D2E38
		// Note: this type is marked as 'beforefieldinit'.
		static TexUI()
		{
			ColorInt colorInt = new ColorInt(51, 63, 51, 200);
			TexUI.GrayBg = SolidColorMaterials.NewSolidColorTexture(colorInt.ToColor);
			ColorInt colorInt2 = new ColorInt(32, 32, 32, 255);
			TexUI.AvailResearchColor = colorInt2.ToColor;
			ColorInt colorInt3 = new ColorInt(0, 64, 64, 255);
			TexUI.ActiveResearchColor = colorInt3.ToColor;
			ColorInt colorInt4 = new ColorInt(0, 64, 16, 255);
			TexUI.FinishedResearchColor = colorInt4.ToColor;
			ColorInt colorInt5 = new ColorInt(42, 42, 42, 255);
			TexUI.LockedResearchColor = colorInt5.ToColor;
			ColorInt colorInt6 = new ColorInt(0, 0, 0, 255);
			TexUI.RelatedResearchColor = colorInt6.ToColor;
			ColorInt colorInt7 = new ColorInt(30, 30, 30, 255);
			TexUI.HighlightBgResearchColor = colorInt7.ToColor;
			ColorInt colorInt8 = new ColorInt(160, 160, 160, 255);
			TexUI.HighlightBorderResearchColor = colorInt8.ToColor;
			ColorInt colorInt9 = new ColorInt(80, 80, 80, 255);
			TexUI.DefaultBorderResearchColor = colorInt9.ToColor;
			ColorInt colorInt10 = new ColorInt(60, 60, 60, 255);
			TexUI.DefaultLineResearchColor = colorInt10.ToColor;
			ColorInt colorInt11 = new ColorInt(51, 205, 217, 255);
			TexUI.HighlightLineResearchColor = colorInt11.ToColor;
			TexUI.FastFillTex = Texture2D.whiteTexture;
			TexUI.FastFillStyle = new GUIStyle
			{
				normal = new GUIStyleState
				{
					background = TexUI.FastFillTex
				}
			};
			TexUI.TextBGBlack = ContentFinder<Texture2D>.Get("UI/Widgets/TextBGBlack", true);
			TexUI.GrayTextBG = ContentFinder<Texture2D>.Get("UI/Overlays/GrayTextBG", true);
			TexUI.FloatMenuOptionBG = ContentFinder<Texture2D>.Get("UI/Widgets/FloatMenuOptionBG", true);
		}

		// Token: 0x04003AD4 RID: 15060
		public static readonly Texture2D TitleBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x04003AD5 RID: 15061
		public static readonly Texture2D HighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x04003AD6 RID: 15062
		public static readonly Texture2D HighlightSelectedTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.94f, 0.5f, 0.18f));

		// Token: 0x04003AD7 RID: 15063
		public static readonly Texture2D ArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowRight", true);

		// Token: 0x04003AD8 RID: 15064
		public static readonly Texture2D ArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowLeft", true);

		// Token: 0x04003AD9 RID: 15065
		public static readonly Texture2D WinExpandWidget = ContentFinder<Texture2D>.Get("UI/Widgets/WinExpandWidget", true);

		// Token: 0x04003ADA RID: 15066
		public static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Misc/AlertFlashArrow", true);

		// Token: 0x04003ADB RID: 15067
		public static readonly Texture2D RotLeftTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotLeft", true);

		// Token: 0x04003ADC RID: 15068
		public static readonly Texture2D RotRightTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotRight", true);

		// Token: 0x04003ADD RID: 15069
		public static readonly Texture2D GrayBg;

		// Token: 0x04003ADE RID: 15070
		public static readonly Color AvailResearchColor;

		// Token: 0x04003ADF RID: 15071
		public static readonly Color ActiveResearchColor;

		// Token: 0x04003AE0 RID: 15072
		public static readonly Color FinishedResearchColor;

		// Token: 0x04003AE1 RID: 15073
		public static readonly Color LockedResearchColor;

		// Token: 0x04003AE2 RID: 15074
		public static readonly Color RelatedResearchColor;

		// Token: 0x04003AE3 RID: 15075
		public static readonly Color HighlightBgResearchColor;

		// Token: 0x04003AE4 RID: 15076
		public static readonly Color HighlightBorderResearchColor;

		// Token: 0x04003AE5 RID: 15077
		public static readonly Color DefaultBorderResearchColor;

		// Token: 0x04003AE6 RID: 15078
		public static readonly Color DefaultLineResearchColor;

		// Token: 0x04003AE7 RID: 15079
		public static readonly Color HighlightLineResearchColor;

		// Token: 0x04003AE8 RID: 15080
		public static readonly Texture2D FastFillTex;

		// Token: 0x04003AE9 RID: 15081
		public static readonly GUIStyle FastFillStyle;

		// Token: 0x04003AEA RID: 15082
		public static readonly Texture2D TextBGBlack;

		// Token: 0x04003AEB RID: 15083
		public static readonly Texture2D GrayTextBG;

		// Token: 0x04003AEC RID: 15084
		public static readonly Texture2D FloatMenuOptionBG;
	}
}
