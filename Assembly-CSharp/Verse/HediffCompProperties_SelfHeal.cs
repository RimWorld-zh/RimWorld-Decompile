namespace Verse
{
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		public int healIntervalTicksStanding = 50;

		public HediffCompProperties_SelfHeal()
		{
			base.compClass = typeof(HediffComp_SelfHeal);
		}
	}
}
