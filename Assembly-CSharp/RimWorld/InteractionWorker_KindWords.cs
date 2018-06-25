using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B3 RID: 1203
	public class InteractionWorker_KindWords : InteractionWorker
	{
		// Token: 0x0600157A RID: 5498 RVA: 0x000BED28 File Offset: 0x000BD128
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
