using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class PawnSkinColors
	{
		private struct SkinColorData
		{
			public float melanin;

			public float selector;

			public Color color;

			public SkinColorData(float melanin, float selector, Color color)
			{
				this.melanin = melanin;
				this.selector = selector;
				this.color = color;
			}
		}

		private static readonly SkinColorData[] SkinColors = new SkinColorData[6]
		{
			new SkinColorData(0f, 0f, new Color(0.9490196f, 0.929411769f, 0.8784314f)),
			new SkinColorData(0.25f, 0.215f, new Color(1f, 0.9372549f, 0.8352941f)),
			new SkinColorData(0.5f, 0.715f, new Color(1f, 0.9372549f, 0.7411765f)),
			new SkinColorData(0.75f, 0.8f, new Color(0.894117653f, 0.619607866f, 0.3529412f)),
			new SkinColorData(0.9f, 0.95f, new Color(0.509803951f, 0.356862754f, 0.1882353f)),
			new SkinColorData(1f, 1f, new Color(0.3882353f, 0.274509817f, 0.141176477f))
		};

		public static bool IsDarkSkin(Color color)
		{
			Color skinColor = PawnSkinColors.GetSkinColor(0.5f);
			return color.r + color.g + color.b <= skinColor.r + skinColor.g + skinColor.b + 0.0099999997764825821;
		}

		public static Color GetSkinColor(float melanin)
		{
			int skinDataIndexOfMelanin = PawnSkinColors.GetSkinDataIndexOfMelanin(melanin);
			if (skinDataIndexOfMelanin == PawnSkinColors.SkinColors.Length - 1)
			{
				return PawnSkinColors.SkinColors[skinDataIndexOfMelanin].color;
			}
			float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].melanin, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].melanin, melanin);
			return Color.Lerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].color, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].color, t);
		}

		public static float RandomMelanin()
		{
			float value = Rand.Value;
			int num = 0;
			int num2 = 0;
			while (num2 < PawnSkinColors.SkinColors.Length && value >= PawnSkinColors.SkinColors[num2].selector)
			{
				num = num2;
				num2++;
			}
			if (num == PawnSkinColors.SkinColors.Length - 1)
			{
				return PawnSkinColors.SkinColors[num].melanin;
			}
			float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[num].selector, PawnSkinColors.SkinColors[num + 1].selector, value);
			return Mathf.Lerp(PawnSkinColors.SkinColors[num].melanin, PawnSkinColors.SkinColors[num + 1].melanin, t);
		}

		public static float GetMelaninCommonalityFactor(float melanin)
		{
			int skinDataIndexOfMelanin = PawnSkinColors.GetSkinDataIndexOfMelanin(melanin);
			if (skinDataIndexOfMelanin == PawnSkinColors.SkinColors.Length - 1)
			{
				return PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin);
			}
			float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].melanin, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].melanin, melanin);
			return Mathf.Lerp(PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin), PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin + 1), t);
		}

		public static float GetRandomMelaninSimilarTo(float value, float clampMin = 0f, float clampMax = 1f)
		{
			return Mathf.Clamp01(Mathf.Clamp(Rand.Gaussian(value, 0.05f), clampMin, clampMax));
		}

		private static float GetSkinDataCommonalityFactor(int skinDataIndex)
		{
			float num = 0f;
			for (int i = 0; i < PawnSkinColors.SkinColors.Length; i++)
			{
				num = Mathf.Max(num, PawnSkinColors.GetTotalAreaWhereClosestToSelector(i));
			}
			return PawnSkinColors.GetTotalAreaWhereClosestToSelector(skinDataIndex) / num;
		}

		private static float GetTotalAreaWhereClosestToSelector(int skinDataIndex)
		{
			float num = 0f;
			if (skinDataIndex == 0)
			{
				num += PawnSkinColors.SkinColors[skinDataIndex].selector;
			}
			else if (PawnSkinColors.SkinColors.Length > 1)
			{
				num = (float)(num + (PawnSkinColors.SkinColors[skinDataIndex].selector - PawnSkinColors.SkinColors[skinDataIndex - 1].selector) / 2.0);
			}
			if (skinDataIndex == PawnSkinColors.SkinColors.Length - 1)
			{
				num = (float)(num + (1.0 - PawnSkinColors.SkinColors[skinDataIndex].selector));
			}
			else if (PawnSkinColors.SkinColors.Length > 1)
			{
				num = (float)(num + (PawnSkinColors.SkinColors[skinDataIndex + 1].selector - PawnSkinColors.SkinColors[skinDataIndex].selector) / 2.0);
			}
			return num;
		}

		private static int GetSkinDataIndexOfMelanin(float melanin)
		{
			int result = 0;
			int num = 0;
			while (num < PawnSkinColors.SkinColors.Length && melanin >= PawnSkinColors.SkinColors[num].melanin)
			{
				result = num;
				num++;
			}
			return result;
		}
	}
}
