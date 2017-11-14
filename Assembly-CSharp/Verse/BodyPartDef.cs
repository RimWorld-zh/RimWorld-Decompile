using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class BodyPartDef : Def
	{
		[NoTranslate]
		public List<string> tags = new List<string>();

		public int hitPoints = 100;

		public float oldInjuryBaseChance = 0.2f;

		public float amputateIfGeneratedInjuredChance;

		public float bleedingRateMultiplier = 1f;

		private bool skinCovered;

		public bool useDestroyedOutLabel;

		public ThingDef spawnThingOnRemoved;

		private bool isSolid;

		public bool dontSuggestAmputation;

		public float frostbiteVulnerability;

		public bool beautyRelated;

		public bool isAlive = true;

		public bool isConceptual;

		public Dictionary<DamageDef, float> hitChanceFactors;

		public bool IsDelicate
		{
			get
			{
				return this.oldInjuryBaseChance >= 0.800000011920929;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!(this.frostbiteVulnerability > 10.0))
				yield break;
			yield return "frostbitePriority > max 10: " + this.frostbiteVulnerability;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0106:
			/*Error near IL_0107: Unexpected return in MoveNext()*/;
		}

		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.isSolid;
					}
				}
			}
			return this.isSolid;
		}

		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			if (body.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return false;
			}
			return this.skinCovered;
		}

		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		public float GetHitChanceFactorFor(DamageDef damage)
		{
			if (this.isConceptual)
			{
				return 0f;
			}
			if (this.hitChanceFactors == null)
			{
				return 1f;
			}
			float result = default(float);
			if (this.hitChanceFactors.TryGetValue(damage, out result))
			{
				return result;
			}
			return 1f;
		}
	}
}
