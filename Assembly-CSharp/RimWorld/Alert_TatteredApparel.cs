using System;
using Verse;

namespace RimWorld
{
	public class Alert_TatteredApparel : Alert_Thought
	{
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}

		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}
	}
}
