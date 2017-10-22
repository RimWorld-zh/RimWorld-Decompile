using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Hibernatable : CompProperties
	{
		public float startupDays = 15f;

		public CompProperties_Hibernatable()
		{
			base.compClass = typeof(CompHibernatable);
		}

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (parentDef.tickerType == TickerType.Normal)
				yield break;
			yield return "CompHibernatable needs tickerType " + TickerType.Normal + ", has " + parentDef.tickerType;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
