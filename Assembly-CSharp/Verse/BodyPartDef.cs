using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AFD RID: 2813
	public class BodyPartDef : Def
	{
		// Token: 0x04002772 RID: 10098
		[MustTranslate]
		public string labelShort;

		// Token: 0x04002773 RID: 10099
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x04002774 RID: 10100
		public int hitPoints = 10;

		// Token: 0x04002775 RID: 10101
		public float permanentInjuryBaseChance = 0.08f;

		// Token: 0x04002776 RID: 10102
		public float bleedRate = 1f;

		// Token: 0x04002777 RID: 10103
		public float frostbiteVulnerability;

		// Token: 0x04002778 RID: 10104
		private bool skinCovered = false;

		// Token: 0x04002779 RID: 10105
		private bool solid = false;

		// Token: 0x0400277A RID: 10106
		public bool alive = true;

		// Token: 0x0400277B RID: 10107
		public bool beautyRelated;

		// Token: 0x0400277C RID: 10108
		public bool conceptual;

		// Token: 0x0400277D RID: 10109
		public bool socketed;

		// Token: 0x0400277E RID: 10110
		public ThingDef spawnThingOnRemoved;

		// Token: 0x0400277F RID: 10111
		public bool pawnGeneratorCanAmputate;

		// Token: 0x04002780 RID: 10112
		public bool canSuggestAmputation = true;

		// Token: 0x04002781 RID: 10113
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003E5D RID: 15965 RVA: 0x0020E29C File Offset: 0x0020C69C
		public bool IsDelicate
		{
			get
			{
				return this.permanentInjuryBaseChance >= 0.999f;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003E5E RID: 15966 RVA: 0x0020E2C4 File Offset: 0x0020C6C4
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003E5F RID: 15967 RVA: 0x0020E2E0 File Offset: 0x0020C6E0
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003E60 RID: 15968 RVA: 0x0020E2FC File Offset: 0x0020C6FC
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0020E334 File Offset: 0x0020C734
		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x0020E354 File Offset: 0x0020C754
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

		// Token: 0x06003E63 RID: 15971 RVA: 0x0020E380 File Offset: 0x0020C780
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

		// Token: 0x06003E64 RID: 15972 RVA: 0x0020E40C File Offset: 0x0020C80C
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x0020E43C File Offset: 0x0020C83C
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x0020E468 File Offset: 0x0020C868
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
