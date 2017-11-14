using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public static class ArmorUtility
	{
		public static int GetPostArmorDamage(Pawn pawn, int amountInt, BodyPartRecord part, DamageDef damageDef)
		{
			float num = (float)amountInt;
			if (damageDef.armorCategory == null)
			{
				return amountInt;
			}
			StatDef deflectionStat = damageDef.armorCategory.deflectionStat;
			if (pawn.apparel != null)
			{
				List<Apparel> wornApparel = pawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					Apparel apparel = wornApparel[i];
					if (apparel.def.apparel.CoversBodyPart(part))
					{
						ArmorUtility.ApplyArmor(ref num, apparel.GetStatValue(deflectionStat, true), apparel, damageDef);
						if (num < 0.0010000000474974513)
						{
							return 0;
						}
					}
				}
			}
			ArmorUtility.ApplyArmor(ref num, pawn.GetStatValue(deflectionStat, true), null, damageDef);
			return GenMath.RoundRandom(num);
		}

		private static void ApplyArmor(ref float damAmount, float armorRating, Thing armorThing, DamageDef damageDef)
		{
			float num;
			float num2;
			if ((double)armorRating <= 0.5)
			{
				num = armorRating;
				num2 = 0f;
			}
			else if (armorRating < 1.0)
			{
				num = 0.5f;
				num2 = (float)(armorRating - 0.5);
			}
			else
			{
				num = (float)(0.5 + (armorRating - 1.0) * 0.25);
				num2 = (float)(0.5 + (armorRating - 1.0) * 0.25);
			}
			if (num > 0.89999997615814209)
			{
				num = 0.9f;
			}
			if (num2 > 0.89999997615814209)
			{
				num2 = 0.9f;
			}
			float num3 = (!(Rand.Value < num2)) ? (damAmount * num) : damAmount;
			if (armorThing != null)
			{
				float f = (float)(damAmount * 0.25);
				armorThing.TakeDamage(new DamageInfo(damageDef, GenMath.RoundRandom(f), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
			damAmount -= num3;
		}
	}
}
