using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EB RID: 747
	public class TraitDef : Def
	{
		// Token: 0x040007F0 RID: 2032
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		// Token: 0x040007F1 RID: 2033
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		// Token: 0x040007F2 RID: 2034
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040007F3 RID: 2035
		public WorkTags requiredWorkTags;

		// Token: 0x040007F4 RID: 2036
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040007F5 RID: 2037
		public WorkTags disabledWorkTags;

		// Token: 0x040007F6 RID: 2038
		private float commonality = 1f;

		// Token: 0x040007F7 RID: 2039
		private float commonalityFemale = -1f;

		// Token: 0x040007F8 RID: 2040
		public bool allowOnHostileSpawn = true;

		// Token: 0x06000C50 RID: 3152 RVA: 0x0006D4FC File Offset: 0x0006B8FC
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0006D518 File Offset: 0x0006B918
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

		// Token: 0x06000C52 RID: 3154 RVA: 0x0006D5B4 File Offset: 0x0006B9B4
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

		// Token: 0x06000C53 RID: 3155 RVA: 0x0006D5E0 File Offset: 0x0006B9E0
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

		// Token: 0x06000C54 RID: 3156 RVA: 0x0006D64C File Offset: 0x0006BA4C
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
