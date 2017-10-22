using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class TraitDegreeData
	{
		[MustTranslate]
		public string label;

		[MustTranslate]
		public string description;

		public int degree = 0;

		public List<StatModifier> statOffsets;

		public List<StatModifier> statFactors;

		public ThinkTreeDef thinkTree = null;

		private float commonality = -1f;

		public MentalStateDef randomMentalState = null;

		public SimpleCurve randomMentalStateMtbDaysMoodCurve = null;

		public List<MentalStateDef> disallowedMentalStates = null;

		public List<MentalBreakDef> theOnlyAllowedMentalBreaks = null;

		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		public float socialFightChanceFactor = 1f;

		public float marketValueFactorOffset = 0f;

		public float Commonality
		{
			get
			{
				float result;
				if (this.commonality >= 0.0)
				{
					result = this.commonality;
				}
				else
				{
					switch (Mathf.Abs(this.degree))
					{
					case 0:
					{
						result = 1f;
						break;
					}
					case 1:
					{
						result = 1f;
						break;
					}
					case 2:
					{
						result = 0.4f;
						break;
					}
					case 3:
					{
						result = 0.2f;
						break;
					}
					case 4:
					{
						result = 0.1f;
						break;
					}
					default:
					{
						result = 0.05f;
						break;
					}
					}
				}
				return result;
			}
		}
	}
}
