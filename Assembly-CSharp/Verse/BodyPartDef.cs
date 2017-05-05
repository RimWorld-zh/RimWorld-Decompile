using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse
{
	public class BodyPartDef : Def
	{
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
				return this.oldInjuryBaseChance >= 0.8f;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			BodyPartDef.<ConfigErrors>c__Iterator1C2 <ConfigErrors>c__Iterator1C = new BodyPartDef.<ConfigErrors>c__Iterator1C2();
			<ConfigErrors>c__Iterator1C.<>f__this = this;
			BodyPartDef.<ConfigErrors>c__Iterator1C2 expr_0E = <ConfigErrors>c__Iterator1C;
			expr_0E.$PC = -2;
			return expr_0E;
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
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
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
			float result;
			if (this.hitChanceFactors.TryGetValue(damage, out result))
			{
				return result;
			}
			return 1f;
		}
	}
}
