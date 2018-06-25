using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class TraitDegreeData
	{
		[MustTranslate]
		public string label;

		[TranslationHandle]
		[Unsaved]
		public string untranslatedLabel = null;

		[MustTranslate]
		public string description;

		public int degree = 0;

		public float commonality = 1f;

		public List<StatModifier> statOffsets;

		public List<StatModifier> statFactors;

		public ThinkTreeDef thinkTree = null;

		public MentalStateDef randomMentalState = null;

		public SimpleCurve randomMentalStateMtbDaysMoodCurve = null;

		public List<MentalStateDef> disallowedMentalStates = null;

		public List<MentalBreakDef> theOnlyAllowedMentalBreaks = null;

		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		public float socialFightChanceFactor = 1f;

		public float marketValueFactorOffset = 0f;

		public float randomDiseaseMtbDays = 0f;

		public TraitDegreeData()
		{
		}

		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
