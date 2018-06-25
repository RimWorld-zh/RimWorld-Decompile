using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	public class RecipeMakerProperties
	{
		public int productCount = 1;

		public int targetCountAdjustment = 1;

		public int workAmount = -1;

		public StatDef workSpeedStat = null;

		public StatDef efficiencyStat = null;

		public ThingDef unfinishedThingDef = null;

		public ThingFilter defaultIngredientFilter = null;

		public List<SkillRequirement> skillRequirements = null;

		public SkillDef workSkill = null;

		public float workSkillLearnPerTick = 1f;

		public EffecterDef effectWorking = null;

		public SoundDef soundWorking = null;

		public List<ThingDef> recipeUsers = null;

		public ResearchProjectDef researchPrerequisite = null;

		[NoTranslate]
		public List<string> factionPrerequisiteTags = null;

		public RecipeMakerProperties()
		{
		}
	}
}
