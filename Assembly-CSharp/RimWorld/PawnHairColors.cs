using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000495 RID: 1173
	public static class PawnHairColors
	{
		// Token: 0x060014C9 RID: 5321 RVA: 0x000B7034 File Offset: 0x000B5434
		public static Color RandomHairColor(Color skinColor, int ageYears)
		{
			Color result;
			if (Rand.Value < 0.02f)
			{
				result = new Color(Rand.Value, Rand.Value, Rand.Value);
			}
			else
			{
				if (ageYears > 40)
				{
					float num = GenMath.SmootherStep(40f, 75f, (float)ageYears);
					if (Rand.Value < num)
					{
						float num2 = Rand.Range(0.65f, 0.85f);
						return new Color(num2, num2, num2);
					}
				}
				if (PawnSkinColors.IsDarkSkin(skinColor) || Rand.Value < 0.5f)
				{
					float value = Rand.Value;
					if (value < 0.25f)
					{
						result = new Color(0.2f, 0.2f, 0.2f);
					}
					else if (value < 0.5f)
					{
						result = new Color(0.31f, 0.28f, 0.26f);
					}
					else if (value < 0.75f)
					{
						result = new Color(0.25f, 0.2f, 0.15f);
					}
					else
					{
						result = new Color(0.3f, 0.2f, 0.1f);
					}
				}
				else
				{
					float value2 = Rand.Value;
					if (value2 < 0.25f)
					{
						result = new Color(0.3529412f, 0.227450982f, 0.1254902f);
					}
					else if (value2 < 0.5f)
					{
						result = new Color(0.5176471f, 0.3254902f, 0.184313729f);
					}
					else if (value2 < 0.75f)
					{
						result = new Color(0.75686276f, 0.572549045f, 0.333333343f);
					}
					else
					{
						result = new Color(0.929411769f, 0.7921569f, 0.6117647f);
					}
				}
			}
			return result;
		}
	}
}
