using Verse;

namespace RimWorld
{
	public class Alert_UnhappyNudity : Alert_Thought
	{
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}

		public Alert_UnhappyNudity()
		{
			base.defaultLabel = "AlertUnhappyNudity".Translate();
			base.explanationKey = "AlertUnhappyNudityDesc";
		}
	}
}
