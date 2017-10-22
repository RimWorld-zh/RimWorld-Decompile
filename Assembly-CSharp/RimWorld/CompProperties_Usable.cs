using Verse;

namespace RimWorld
{
	public class CompProperties_Usable : CompProperties
	{
		public JobDef useJob;

		public string useLabel;

		public CompProperties_Usable()
		{
			base.compClass = typeof(CompUsable);
		}
	}
}
