using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B00 RID: 2816
	public class BodyPartDef : Def
	{
		// Token: 0x0400277A RID: 10106
		[MustTranslate]
		public string labelShort;

		// Token: 0x0400277B RID: 10107
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x0400277C RID: 10108
		public int hitPoints = 10;

		// Token: 0x0400277D RID: 10109
		public float permanentInjuryBaseChance = 0.08f;

		// Token: 0x0400277E RID: 10110
		public float bleedRate = 1f;

		// Token: 0x0400277F RID: 10111
		public float frostbiteVulnerability;

		// Token: 0x04002780 RID: 10112
		private bool skinCovered = false;

		// Token: 0x04002781 RID: 10113
		private bool solid = false;

		// Token: 0x04002782 RID: 10114
		public bool alive = true;

		// Token: 0x04002783 RID: 10115
		public bool beautyRelated;

		// Token: 0x04002784 RID: 10116
		public bool conceptual;

		// Token: 0x04002785 RID: 10117
		public bool socketed;

		// Token: 0x04002786 RID: 10118
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04002787 RID: 10119
		public bool pawnGeneratorCanAmputate;

		// Token: 0x04002788 RID: 10120
		public bool canSuggestAmputation = true;

		// Token: 0x04002789 RID: 10121
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0020E6A8 File Offset: 0x0020CAA8
		public bool IsDelicate
		{
			get
			{
				return this.permanentInjuryBaseChance >= 0.999f;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x0020E6D0 File Offset: 0x0020CAD0
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x0020E6EC File Offset: 0x0020CAEC
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003E64 RID: 15972 RVA: 0x0020E708 File Offset: 0x0020CB08
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x0020E740 File Offset: 0x0020CB40
		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x0020E760 File Offset: 0x0020CB60
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.frostbiteVulnerability > 10f)
			{
				yield return "frostbitePriority > max 10: " + this.frostbiteVulnerability;
			}
			if (this.solid && this.permanentInjuryBaseChance > 0f)
			{
				yield return "solid but permanentInjuryBaseChance is not zero; it is " + this.permanentInjuryBaseChance + ". Solid parts must have zero permanent injury chance.";
			}
			if (this.solid && this.bleedRate > 0f)
			{
				yield return "solid but bleedRate is not zero";
			}
			if (this.solid && this.permanentInjuryBaseChance > 0f)
			{
				yield return "solid but permanentInjuryBaseChance is not zero";
			}
			yield break;
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x0020E78C File Offset: 0x0020CB8C
		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.solid;
					}
				}
			}
			return this.solid;
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x0020E818 File Offset: 0x0020CC18
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x0020E848 File Offset: 0x0020CC48
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x0020E874 File Offset: 0x0020CC74
		public float GetHitChanceFactorFor(DamageDef damage)
		{
			float result;
			float num;
			if (this.conceptual)
			{
				result = 0f;
			}
			else if (this.hitChanceFactors == null)
			{
				result = 1f;
			}
			else if (this.hitChanceFactors.TryGetValue(damage, out num))
			{
				result = num;
			}
			else
			{
				result = 1f;
			}
			return result;
		}
	}
}
