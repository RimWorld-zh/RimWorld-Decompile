namespace Verse
{
	public class HediffGiver_Event : HediffGiver
	{
		private float chance = 1f;

		public bool EventOccurred(Pawn pawn)
		{
			if (Rand.Value < this.chance)
			{
				return base.TryApply(pawn, null);
			}
			return false;
		}
	}
}
