using Verse;

namespace RimWorld
{
	public class Alert_TatteredApparel : Alert_Thought
	{
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}

		public Alert_TatteredApparel()
		{
			base.defaultLabel = "AlertTatteredApparel".Translate();
			base.explanationKey = "AlertTatteredApparelDesc";
		}
	}
}
