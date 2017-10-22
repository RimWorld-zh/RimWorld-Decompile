using Verse;

namespace RimWorld
{
	public class InteractionWorker_Slight : InteractionWorker
	{
		private const float BaseSelectionWeight = 0.02f;

		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return (float)(0.019999999552965164 * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient));
		}
	}
}
