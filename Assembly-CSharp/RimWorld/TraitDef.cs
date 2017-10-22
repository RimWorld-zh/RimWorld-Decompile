using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class TraitDef : Def
	{
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		public WorkTags requiredWorkTags;

		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		public WorkTags disabledWorkTags;

		private float commonality;

		private float commonalityFemale = -1f;

		public bool allowOnHostileSpawn = true;

		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		public TraitDegreeData DataAtDegree(int degree)
		{
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				if (this.degreeDatas[i].degree == degree)
				{
					return this.degreeDatas[i];
				}
			}
			Log.Error(base.defName + " found no data at degree " + degree + ", returning first defined.");
			return this.degreeDatas[0];
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.commonality < 0.0010000000474974513 && this.commonalityFemale < 0.0010000000474974513)
			{
				yield return "TraitDef " + base.defName + " has 0 commonality.";
			}
			if (!this.degreeDatas.Any())
			{
				yield return base.defName + " has no degree datas.";
			}
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				TraitDegreeData dd3 = this.degreeDatas[i];
				if ((from dd2 in this.degreeDatas
				where dd2.degree == ((_003CConfigErrors_003Ec__Iterator9D)/*Error near IL_0177: stateMachine*/)._003Cdd_003E__3.degree
				select dd2).Count() > 1)
				{
					yield return ">1 datas for degree " + dd3.degree;
				}
			}
		}

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

		public float GetGenderSpecificCommonality(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && this.commonalityFemale >= 0.0)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}
	}
}
