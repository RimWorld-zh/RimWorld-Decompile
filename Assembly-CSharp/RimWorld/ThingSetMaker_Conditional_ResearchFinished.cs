using System;
using Verse;

namespace RimWorld
{
	public class ThingSetMaker_Conditional_ResearchFinished : ThingSetMaker_Conditional
	{
		public ResearchProjectDef researchProject;

		public ThingSetMaker_Conditional_ResearchFinished()
		{
		}

		protected override bool Condition(ThingSetMakerParams parms)
		{
			return this.researchProject.IsFinished;
		}
	}
}
