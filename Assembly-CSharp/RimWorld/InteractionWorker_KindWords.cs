using System;
using Verse;

namespace RimWorld
{
	public class InteractionWorker_KindWords : InteractionWorker
	{
		public InteractionWorker_KindWords()
		{
		}

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
