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

		public float amputateIfGeneratedInjuredChance = 0f;

		public float bleedingRateMultiplier = 1f;

		private bool skinCovered = false;

		public bool useDestroyedOutLabel = false;

		public ThingDef spawnThingOnRemoved = null;

		private bool isSolid = false;

		public bool dontSuggestAmputation = false;

		public float frostbiteVulnerability = 0f;

		public bool beautyRelated = false;

		public bool isAlive = true;

		public bool isConceptual = false;

		public Dictionary<DamageDef, float> hitChanceFactors = null;

		public bool IsDelicate
		{
			get
			{
				return this.oldInjuryBaseChance >= 0.800000011920929;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
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
			IL_010a:
			/*Error near IL_010b: Unexpected return in MoveNext()*/;
		}

		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			BodyPartRecord bodyPartRecord = part;
			bool result;
			while (true)
			{
				int i;
				if (bodyPartRecord != null)
				{
					for (i = 0; i < hediffs.Count; i++)
					{
						if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
							goto IL_0034;
					}
					bodyPartRecord = bodyPartRecord.parent;
					continue;
				}
				result = this.isSolid;
				break;
				IL_0034:
				result = hediffs[i].def.addedPartProps.isSolid;
				break;
			}
			return result;
		}

		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		public float GetHitChanceFactorFor(DamageDef damage)
		{
			float num = default(float);
			return (float)((!this.isConceptual) ? ((this.hitChanceFactors != null) ? ((!this.hitChanceFactors.TryGetValue(damage, out num)) ? 1.0 : num) : 1.0) : 0.0);
		}
	}
}
