namespace RimWorld
{
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		public override float OpinionOffset()
		{
			int num = base.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
			float result;
			if (num > 0)
			{
				result = 0f;
			}
			else
			{
				switch (num)
				{
				case 0:
				{
					result = -5f;
					break;
				}
				case -1:
				{
					result = -20f;
					break;
				}
				default:
				{
					result = -30f;
					break;
				}
				}
			}
			return result;
		}
	}
}
