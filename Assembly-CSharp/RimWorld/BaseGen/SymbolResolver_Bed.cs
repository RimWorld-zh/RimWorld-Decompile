using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A7 RID: 935
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x0600103C RID: 4156 RVA: 0x00088A88 File Offset: 0x00086E88
		public override void Resolve(ResolveParams rp)
		{
			ThingDef singleThingDef = rp.singleThingDef ?? Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = singleThingDef;
			bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit = rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit;
			resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit = new bool?(skipSingleThingIfHasToWipeBuildingOrDoesntFit == null || skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value);
			BaseGen.symbolStack.Push("thing", resolveParams);
		}
	}
}
