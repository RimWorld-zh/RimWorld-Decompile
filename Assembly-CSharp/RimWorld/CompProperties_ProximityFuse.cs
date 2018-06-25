using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000250 RID: 592
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x040004A4 RID: 1188
		public ThingDef target;

		// Token: 0x040004A5 RID: 1189
		public float radius;

		// Token: 0x06000A87 RID: 2695 RVA: 0x0005F7B0 File Offset: 0x0005DBB0
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0005F7CC File Offset: 0x0005DBCC
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
	}
}
