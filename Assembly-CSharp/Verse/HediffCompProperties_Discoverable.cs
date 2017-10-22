namespace Verse
{
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		public bool sendLetterWhenDiscovered = false;

		public string discoverLetterLabel = (string)null;

		public string discoverLetterText = (string)null;

		public HediffCompProperties_Discoverable()
		{
			base.compClass = typeof(HediffComp_Discoverable);
		}
	}
}
