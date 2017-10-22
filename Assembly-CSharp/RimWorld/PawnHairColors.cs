using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class PawnHairColors
	{
		public static Color RandomHairColor(Color skinColor, int ageYears)
		{
			Color result;
			if (Rand.Value < 0.019999999552965164)
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
						result = new Color(num2, num2, num2);
						goto IL_01af;
					}
				}
				if (PawnSkinColors.IsDarkSkin(skinColor) || Rand.Value < 0.5)
				{
					float value = Rand.Value;
					result = ((!(value < 0.25)) ? ((!(value < 0.5)) ? ((!(value < 0.75)) ? new Color(0.3f, 0.2f, 0.1f) : new Color(0.25f, 0.2f, 0.15f)) : new Color(0.31f, 0.28f, 0.26f)) : new Color(0.2f, 0.2f, 0.2f));
				}
				else
				{
					float value2 = Rand.Value;
					result = ((!(value2 < 0.25)) ? ((!(value2 < 0.5)) ? ((!(value2 < 0.75)) ? new Color(0.929411769f, 0.7921569f, 0.6117647f) : new Color(0.75686276f, 0.572549045f, 0.333333343f)) : new Color(0.5176471f, 0.3254902f, 0.184313729f)) : new Color(0.3529412f, 0.227450982f, 0.1254902f));
				}
			}
			goto IL_01af;
			IL_01af:
			return result;
		}
	}
}
