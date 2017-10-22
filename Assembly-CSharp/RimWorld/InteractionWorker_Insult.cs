using Verse;

namespace RimWorld
{
	public class InteractionWorker_Insult : InteractionWorker
	{
		private const float BaseSelectionWeight = 0.007f;

		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return (float)(0.0070000002160668373 * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient));
		}
	}
}
