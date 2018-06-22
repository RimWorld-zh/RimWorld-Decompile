using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B1 RID: 1201
	public class InteractionWorker_KindWords : InteractionWorker
	{
		// Token: 0x06001576 RID: 5494 RVA: 0x000BEBD8 File Offset: 0x000BCFD8
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			float result;
			if (initiator.story.traits.HasTrait(TraitDefOf.Kind))
			{
				result = 0.01f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
