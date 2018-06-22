using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024E RID: 590
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x0005F664 File Offset: 0x0005DA64
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0005F680 File Offset: 0x0005DA80
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

		// Token: 0x040004A2 RID: 1186
		public ThingDef target;

		// Token: 0x040004A3 RID: 1187
		public float radius;
	}
}
