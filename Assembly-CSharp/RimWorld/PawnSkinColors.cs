using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000494 RID: 1172
	public static class PawnSkinColors
	{
		// Token: 0x04000C83 RID: 3203
		private static readonly PawnSkinColors.SkinColorData[] SkinColors = new PawnSkinColors.SkinColorData[]
		{
			new PawnSkinColors.SkinColorData(0f, 0f, new Color(0.9490196f, 0.929411769f, 0.8784314f)),
			new PawnSkinColors.SkinColorData(0.25f, 0.2f, new Color(1f, 0.9372549f, 0.8352941f)),
			new PawnSkinColors.SkinColorData(0.5f, 0.7f, new Color(1f, 0.9372549f, 0.7411765f)),
			new PawnSkinColors.SkinColorData(0.75f, 0.8f, new Color(0.894117653f, 0.619607866f, 0.3529412f)),
			new PawnSkinColors.SkinColorData(0.9f, 0.9f, new Color(0.509803951f, 0.356862754f, 0.1882353f)),
			new PawnSkinColors.SkinColorData(1f, 1f, new Color(0.3882353f, 0.274509817f, 0.141176477f))
		};

		// Token: 0x060014C4 RID: 5316 RVA: 0x000B755C File Offset: 0x000B595C
		public static bool IsDarkSkin(Color color)
		{
			Color skinColor = PawnSkinColors.GetSkinColor(0.5f);
			return color.r + color.g + color.b <= skinColor.r + skinColor.g + skinColor.b + 0.01f;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x000B75B8 File Offset: 0x000B59B8
		public static Color GetSkinColor(float melanin)
		{
			int skinDataIndexOfMelanin = PawnSkinColors.GetSkinDataIndexOfMelanin(melanin);
			Color result;
			if (skinDataIndexOfMelanin == PawnSkinColors.SkinColors.Length - 1)
			{
				result = PawnSkinColors.SkinColors[skinDataIndexOfMelanin].color;
			}
			else
			{
				float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].melanin, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].melanin, melanin);
				result = Color.Lerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].color, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].color, t);
			}
			return result;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x000B764C File Offset: 0x000B5A4C
		public static float RandomMelanin(Faction fac)
		{
			float num;
			if (fac == null)
			{
				num = Rand.Value;
			}
			else
			{
				num = Rand.Range(Mathf.Clamp01(fac.centralMelanin - fac.def.geneticVariance), Mathf.Clamp01(fac.centralMelanin + fac.def.geneticVariance));
			}
			int num2 = 0;
			for (int i = 0; i < PawnSkinColors.SkinColors.Length; i++)
			{
				if (num < PawnSkinColors.SkinColors[i].selector)
				{
					break;
				}
				num2 = i;
			}
			float result;
			if (num2 == PawnSkinColors.SkinColors.Length - 1)
			{
				result = PawnSkinColors.SkinColors[num2].melanin;
			}
			else
			{
				float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[num2].selector, PawnSkinColors.SkinColors[num2 + 1].selector, num);
				result = Mathf.Lerp(PawnSkinColors.SkinColors[num2].melanin, PawnSkinColors.SkinColors[num2 + 1].melanin, t);
			}
			return result;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x000B7760 File Offset: 0x000B5B60
		public static float GetMelaninCommonalityFactor(float melanin)
		{
			int skinDataIndexOfMelanin = PawnSkinColors.GetSkinDataIndexOfMelanin(melanin);
			float result;
			if (skinDataIndexOfMelanin == PawnSkinColors.SkinColors.Length - 1)
			{
				result = PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin);
			}
			else
			{
				float t = Mathf.InverseLerp(PawnSkinColors.SkinColors[skinDataIndexOfMelanin].melanin, PawnSkinColors.SkinColors[skinDataIndexOfMelanin + 1].melanin, melanin);
				result = Mathf.Lerp(PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin), PawnSkinColors.GetSkinDataCommonalityFactor(skinDataIndexOfMelanin + 1), t);
			}
			return result;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000B77D4 File Offset: 0x000B5BD4
		public static float GetRandomMelaninSimilarTo(float value, float clampMin = 0f, float clampMax = 1f)
		{
			return Mathf.Clamp01(Mathf.Clamp(Rand.Gaussian(value, 0.05f), clampMin, clampMax));
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x000B7800 File Offset: 0x000B5C00
		private static float GetSkinDataCommonalityFactor(int skinDataIndex)
		{
			float num = 0f;
			for (int i = 0; i < PawnSkinColors.SkinColors.Length; i++)
			{
				num = Mathf.Max(num, PawnSkinColors.GetTotalAreaWhereClosestToSelector(i));
			}
			return PawnSkinColors.GetTotalAreaWhereClosestToSelector(skinDataIndex) / num;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x000B784C File Offset: 0x000B5C4C
		private static float GetTotalAreaWhereClosestToSelector(int skinDataIndex)
		{
			float num = 0f;
			if (skinDataIndex == 0)
			{
				num += PawnSkinColors.SkinColors[skinDataIndex].selector;
			}
			else if (PawnSkinColors.SkinColors.Length > 1)
			{
				num += (PawnSkinColors.SkinColors[skinDataIndex].selector - PawnSkinColors.SkinColors[skinDataIndex - 1].selector) / 2f;
			}
			if (skinDataIndex == PawnSkinColors.SkinColors.Length - 1)
			{
				num += 1f - PawnSkinColors.SkinColors[skinDataIndex].selector;
			}
			else if (PawnSkinColors.SkinColors.Length > 1)
			{
				num += (PawnSkinColors.SkinColors[skinDataIndex + 1].selector - PawnSkinColors.SkinColors[skinDataIndex].selector) / 2f;
			}
			return num;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x000B7928 File Offset: 0x000B5D28
		private static int GetSkinDataIndexOfMelanin(float melanin)
		{
			int result = 0;
			for (int i = 0; i < PawnSkinColors.SkinColors.Length; i++)
			{
				if (melanin < PawnSkinColors.SkinColors[i].melanin)
				{
					break;
				}
				result = i;
			}
			return result;
		}

		// Token: 0x02000495 RID: 1173
		private struct SkinColorData
		{
			// Token: 0x04000C84 RID: 3204
			public float melanin;

			// Token: 0x04000C85 RID: 3205
			public float selector;

			// Token: 0x04000C86 RID: 3206
			public Color color;

			// Token: 0x060014CD RID: 5325 RVA: 0x000B7AAE File Offset: 0x000B5EAE
			public SkinColorData(float melanin, float selector, Color color)
			{
				this.melanin = melanin;
				this.selector = selector;
				this.color = color;
			}
		}
	}
}
