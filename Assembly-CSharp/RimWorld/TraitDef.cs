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
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err = enumerator.Current;
					yield return err;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.commonality < 0.0010000000474974513 && this.commonalityFemale < 0.0010000000474974513)
			{
				yield return "TraitDef " + base.defName + " has 0 commonality.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.degreeDatas.Any())
			{
				yield return base.defName + " has no degree datas.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			int i = 0;
			TraitDegreeData dd3;
			while (true)
			{
				if (i < this.degreeDatas.Count)
				{
					_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_017b: stateMachine*/;
					dd3 = this.degreeDatas[i];
					if ((from dd2 in this.degreeDatas
					where dd2.degree == dd3.degree
					select dd2).Count() <= 1)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return ">1 datas for degree " + dd3.degree;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0239:
			/*Error near IL_023a: Unexpected return in MoveNext()*/;
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
