using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CFB RID: 3323
	public static class ArmorUtility
	{
		// Token: 0x06004931 RID: 18737 RVA: 0x00267C18 File Offset: 0x00266018
		public static float GetPostArmorDamage(Pawn pawn, float amount, BodyPartRecord part, DamageDef damageDef, out bool deflectedByMetalArmor)
		{
			deflectedByMetalArmor = false;
			float result;
			if (damageDef.armorCategory == null)
			{
				result = amount;
			}
			else
			{
				StatDef deflectionStat = damageDef.armorCategory.deflectionStat;
				if (pawn.apparel != null)
				{
					List<Apparel> wornApparel = pawn.apparel.WornApparel;
					for (int i = wornApparel.Count - 1; i >= 0; i--)
					{
						Apparel apparel = wornApparel[i];
						if (apparel.def.apparel.CoversBodyPart(part))
						{
							ArmorUtility.ApplyArmor(ref amount, apparel.GetStatValue(deflectionStat, true), apparel, damageDef, pawn, ref deflectedByMetalArmor);
							if (amount < 0.001f)
							{
								return 0f;
							}
						}
					}
				}
				ArmorUtility.ApplyArmor(ref amount, pawn.GetStatValue(deflectionStat, true), null, damageDef, pawn, ref deflectedByMetalArmor);
				if (amount < 0.001f)
				{
					result = 0f;
				}
				else
				{
					result = amount;
				}
			}
			return result;
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x00267CFC File Offset: 0x002660FC
		private static void ApplyArmor(ref float damAmount, float armorRating, Thing armorThing, DamageDef damageDef, Pawn pawn, ref bool deflectedByMetalArmor)
		{
			if (armorThing != null)
			{
				float f = damAmount * 0.25f;
				armorThing.TakeDamage(new DamageInfo(damageDef, (float)GenMath.RoundRandom(f), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			if (Rand.Value < armorRating)
			{
				damAmount = 0f;
				if (armorThing != null)
				{
					deflectedByMetalArmor = (armorThing.def.apparel.useDeflectMetalEffect || (armorThing.Stuff != null && armorThing.Stuff.IsMetal));
				}
				else
				{
					deflectedByMetalArmor = pawn.RaceProps.IsMechanoid;
				}
			}
		}
	}
}
