namespace Verse
{
	public class HediffCompProperties_GetsOld : HediffCompProperties
	{
		public float becomeOldChance = 1f;

		public string oldLabel;

		public string instantlyOldLabel;

		public HediffCompProperties_GetsOld()
		{
			base.compClass = typeof(HediffComp_GetsOld);
		}
	}
}
