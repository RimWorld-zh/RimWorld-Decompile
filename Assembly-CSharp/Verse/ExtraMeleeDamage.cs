using System;

namespace Verse
{
	public class ExtraMeleeDamage
	{
		public DamageDef def;

		public int amount;

		public float armorPenetration = -1f;

		public ExtraMeleeDamage()
		{
		}

		public float AdjustedDamageAmount(Verb verb, Pawn caster)
		{
			return (float)this.amount * verb.GetDamageFactorFor(caster);
		}

		public float AdjustedArmorPenetration(Verb verb, Pawn caster)
		{
			float result;
			if (this.armorPenetration < 0f)
			{
				result = this.AdjustedDamageAmount(verb, caster) * 0.015f;
			}
			else
			{
				result = this.armorPenetration;
			}
			return result;
		}
	}
}
