namespace Verse
{
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		public EffecterDef stateEffecter = null;

		public IntRange severityIndices = new IntRange(-1, -1);

		public HediffCompProperties_Effecter()
		{
			base.compClass = typeof(HediffComp_Effecter);
		}
	}
}
