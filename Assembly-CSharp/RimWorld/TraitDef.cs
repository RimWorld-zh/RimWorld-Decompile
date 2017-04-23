using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			Log.Error(string.Concat(new object[]
			{
				this.defName,
				" found no data at degree ",
				degree,
				", returning first defined."
			}));
			return this.degreeDatas[0];
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			TraitDef.<ConfigErrors>c__Iterator9C <ConfigErrors>c__Iterator9C = new TraitDef.<ConfigErrors>c__Iterator9C();
			<ConfigErrors>c__Iterator9C.<>f__this = this;
			TraitDef.<ConfigErrors>c__Iterator9C expr_0E = <ConfigErrors>c__Iterator9C;
			expr_0E.$PC = -2;
			return expr_0E;
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
			if (pawn.gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}
	}
}
