using RimWorld;

namespace Verse
{
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		public ChemicalDef chemical;

		public HediffCompProperties_DrugEffectFactor()
		{
			base.compClass = typeof(HediffComp_DrugEffectFactor);
		}
	}
}
