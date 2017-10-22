using Verse;

namespace RimWorld
{
	public class CompProperties_Usable : CompProperties
	{
		public JobDef useJob;

		public string useLabel;

		public int useDuration = 100;

		public CompProperties_Usable()
		{
			base.compClass = typeof(CompUsable);
		}
	}
}
