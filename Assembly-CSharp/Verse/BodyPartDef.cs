using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B01 RID: 2817
	public class BodyPartDef : Def
	{
		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0020DF60 File Offset: 0x0020C360
		public bool IsDelicate
		{
			get
			{
				return this.permanentInjuryBaseChance >= 0.999f;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x0020DF88 File Offset: 0x0020C388
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x0020DFA4 File Offset: 0x0020C3A4
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003E64 RID: 15972 RVA: 0x0020DFC0 File Offset: 0x0020C3C0
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x0020DFF8 File Offset: 0x0020C3F8
		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x0020E018 File Offset: 0x0020C418
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

		// Token: 0x06003E67 RID: 15975 RVA: 0x0020E044 File Offset: 0x0020C444
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

		// Token: 0x06003E68 RID: 15976 RVA: 0x0020E0D0 File Offset: 0x0020C4D0
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x0020E100 File Offset: 0x0020C500
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x0020E12C File Offset: 0x0020C52C
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

		// Token: 0x04002776 RID: 10102
		[MustTranslate]
		public string labelShort;

		// Token: 0x04002777 RID: 10103
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x04002778 RID: 10104
		public int hitPoints = 10;

		// Token: 0x04002779 RID: 10105
		public float permanentInjuryBaseChance = 0.08f;

		// Token: 0x0400277A RID: 10106
		public float bleedRate = 1f;

		// Token: 0x0400277B RID: 10107
		public float frostbiteVulnerability;

		// Token: 0x0400277C RID: 10108
		private bool skinCovered = false;

		// Token: 0x0400277D RID: 10109
		private bool solid = false;

		// Token: 0x0400277E RID: 10110
		public bool alive = true;

		// Token: 0x0400277F RID: 10111
		public bool beautyRelated;

		// Token: 0x04002780 RID: 10112
		public bool conceptual;

		// Token: 0x04002781 RID: 10113
		public bool socketed;

		// Token: 0x04002782 RID: 10114
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04002783 RID: 10115
		public bool pawnGeneratorCanAmputate;

		// Token: 0x04002784 RID: 10116
		public bool canSuggestAmputation = true;

		// Token: 0x04002785 RID: 10117
		public Dictionary<DamageDef, float> hitChanceFactors;
	}
}
