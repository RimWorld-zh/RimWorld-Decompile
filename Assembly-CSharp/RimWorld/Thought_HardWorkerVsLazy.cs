using System;

namespace RimWorld
{
	// Token: 0x020001FF RID: 511
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		// Token: 0x060009C9 RID: 2505 RVA: 0x000580C0 File Offset: 0x000564C0
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
