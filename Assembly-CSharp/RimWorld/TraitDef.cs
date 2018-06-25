using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EB RID: 747
	public class TraitDef : Def
	{
		// Token: 0x040007ED RID: 2029
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		// Token: 0x040007EE RID: 2030
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		// Token: 0x040007EF RID: 2031
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040007F0 RID: 2032
		public WorkTags requiredWorkTags;

		// Token: 0x040007F1 RID: 2033
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040007F2 RID: 2034
		public WorkTags disabledWorkTags;

		// Token: 0x040007F3 RID: 2035
		private float commonality = 1f;

		// Token: 0x040007F4 RID: 2036
		private float commonalityFemale = -1f;

		// Token: 0x040007F5 RID: 2037
		public bool allowOnHostileSpawn = true;

		// Token: 0x06000C51 RID: 3153 RVA: 0x0006D4F4 File Offset: 0x0006B8F4
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0006D510 File Offset: 0x0006B910
		public TraitDegreeData DataAtDegree(int degree)
		{
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				if (this.degreeDatas[i].degree == degree)
				{
					return this.degreeDatas[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				this.defName,
				" found no data at degree ",
				degree,
				", returning first defined."
			}), false);
			return this.degreeDatas[0];
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0006D5AC File Offset: 0x0006B9AC
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.commonality < 0.001f && this.commonalityFemale < 0.001f)
			{
				yield return "TraitDef " + this.defName + " has 0 commonality.";
			}
			if (!this.degreeDatas.Any<TraitDegreeData>())
			{
				yield return this.defName + " has no degree datas.";
			}
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				TraitDegreeData dd = this.degreeDatas[i];
				if ((from dd2 in this.degreeDatas
				where dd2.degree == dd.degree
				select dd2).Count<TraitDegreeData>() > 1)
				{
					yield return ">1 datas for degree " + dd.degree;
				}
			}
			yield break;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0006D5D8 File Offset: 0x0006B9D8
		public bool ConflictsWith(Trait other)
		{
			if (other.def.conflictingTraits != null)
			{
				for (int i = 0; i < other.def.conflictingTraits.Count; i++)
				{
					if (other.def.conflictingTraits[i] == this)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0006D644 File Offset: 0x0006BA44
		public float GetGenderSpecificCommonality(Gender gender)
		{
			float result;
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				result = this.commonalityFemale;
			}
			else
			{
				result = this.commonality;
			}
			return result;
		}
	}
}
