using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024E RID: 590
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x0005F608 File Offset: 0x0005DA08
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0005F624 File Offset: 0x0005DA24
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return err;
			}
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Concat(new object[]
				{
					"CompProximityFuse needs tickerType ",
					TickerType.Rare,
					" or faster, has ",
					parentDef.tickerType
				});
			}
			if (parentDef.CompDefFor<CompExplosive>() == null)
			{
				yield return "CompProximityFuse requires a CompExplosive";
			}
			yield break;
		}

		// Token: 0x040004A4 RID: 1188
		public ThingDef target;

		// Token: 0x040004A5 RID: 1189
		public float radius;
	}
}
