namespace Verse
{
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		public float infectionChance = 0.75f;

		public HediffCompProperties_Infecter()
		{
			base.compClass = typeof(HediffComp_Infecter);
		}
	}
}
