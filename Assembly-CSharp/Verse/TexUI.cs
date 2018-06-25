using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAB RID: 3755
	[StaticConstructorOnStartup]
	public static class TexUI
	{
		// Token: 0x04003AEA RID: 15082
		public static readonly Texture2D TitleBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x04003AEB RID: 15083
		public static readonly Texture2D HighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x04003AEC RID: 15084
		public static readonly Texture2D HighlightSelectedTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.94f, 0.5f, 0.18f));

		// Token: 0x04003AED RID: 15085
		public static readonly Texture2D ArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowRight", true);

		// Token: 0x04003AEE RID: 15086
		public static readonly Texture2D ArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowLeft", true);

		// Token: 0x04003AEF RID: 15087
		public static readonly Texture2D WinExpandWidget = ContentFinder<Texture2D>.Get("UI/Widgets/WinExpandWidget", true);

		// Token: 0x04003AF0 RID: 15088
		public static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Misc/AlertFlashArrow", true);

		// Token: 0x04003AF1 RID: 15089
		public static readonly Texture2D RotLeftTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotLeft", true);

		// Token: 0x04003AF2 RID: 15090
		public static readonly Texture2D RotRightTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotRight", true);

		// Token: 0x04003AF3 RID: 15091
		public static readonly Texture2D GrayBg;

		// Token: 0x04003AF4 RID: 15092
		public static readonly Color AvailResearchColor;

		// Token: 0x04003AF5 RID: 15093
		public static readonly Color ActiveResearchColor;

		// Token: 0x04003AF6 RID: 15094
		public static readonly Color FinishedResearchColor;

		// Token: 0x04003AF7 RID: 15095
		public static readonly Color LockedResearchColor;

		// Token: 0x04003AF8 RID: 15096
		public static readonly Color RelatedResearchColor;

		// Token: 0x04003AF9 RID: 15097
		public static readonly Color HighlightBgResearchColor;

		// Token: 0x04003AFA RID: 15098
		public static readonly Color HighlightBorderResearchColor;

		// Token: 0x04003AFB RID: 15099
		public static readonly Color DefaultBorderResearchColor;

		// Token: 0x04003AFC RID: 15100
		public static readonly Color DefaultLineResearchColor;

		// Token: 0x04003AFD RID: 15101
		public static readonly Color HighlightLineResearchColor;

		// Token: 0x04003AFE RID: 15102
		public static readonly Texture2D FastFillTex;

		// Token: 0x04003AFF RID: 15103
		public static readonly GUIStyle FastFillStyle;

		// Token: 0x04003B00 RID: 15104
		public static readonly Texture2D TextBGBlack;

		// Token: 0x04003B01 RID: 15105
		public static readonly Texture2D GrayTextBG;

		// Token: 0x04003B02 RID: 15106
		public static readonly Texture2D FloatMenuOptionBG;

		// Token: 0x04003B03 RID: 15107
		public static readonly Material GrayscaleGUI;

		// Token: 0x06005887 RID: 22663 RVA: 0x002D6960 File Offset: 0x002D4D60
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
			TexUI.GrayscaleGUI = MatLoader.LoadMat("Misc/GrayscaleGUI", -1);
		}
	}
}
