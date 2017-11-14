namespace RimWorld
{
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		public override float OpinionOffset()
		{
			int num = base.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
			if (num > 0)
			{
				return 0f;
			}
			switch (num)
			{
			case 0:
				return -5f;
			case -1:
				return -20f;
			default:
				return -30f;
			}
		}
	}
}
