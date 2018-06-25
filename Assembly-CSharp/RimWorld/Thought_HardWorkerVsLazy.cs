using System;

namespace RimWorld
{
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		public Thought_HardWorkerVsLazy()
		{
		}

		public override float OpinionOffset()
		{
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
			float result;
			if (num > 0)
			{
				result = 0f;
			}
			else if (num == 0)
			{
				result = -5f;
			}
			else if (num == -1)
			{
				result = -20f;
			}
			else
			{
				result = -30f;
			}
			return result;
		}
	}
}
