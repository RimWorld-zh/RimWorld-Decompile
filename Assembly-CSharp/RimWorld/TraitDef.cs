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

		private float commonality = 0f;

		private float commonalityFemale = -1f;

		public bool allowOnHostileSpawn = true;

		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		public TraitDegreeData DataAtDegree(int degree)
		{
			int num = 0;
			TraitDegreeData result;
			while (true)
			{
				if (num < this.degreeDatas.Count)
				{
					if (this.degreeDatas[num].degree == degree)
					{
						result = this.degreeDatas[num];
						break;
					}
					num++;
					continue;
				}
				Log.Error(base.defName + " found no data at degree " + degree + ", returning first defined.");
				result = this.degreeDatas[0];
				break;
			}
			return result;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
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
					_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_017f: stateMachine*/;
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
			IL_023f:
			/*Error near IL_0240: Unexpected return in MoveNext()*/;
		}

		public bool ConflictsWith(Trait other)
		{
			if (other.def.conflictingTraits != null)
			{
				for (int i = 0; i < other.def.conflictingTraits.Count; i++)
				{
					if (other.def.conflictingTraits[i] == this)
						goto IL_0031;
				}
			}
			bool result = false;
			goto IL_005b;
			IL_0031:
			result = true;
			goto IL_005b;
			IL_005b:
			return result;
		}

		public float GetGenderSpecificCommonality(Pawn pawn)
		{
			return (pawn.gender != Gender.Female || !(this.commonalityFemale >= 0.0)) ? this.commonality : this.commonalityFemale;
		}
	}
}
