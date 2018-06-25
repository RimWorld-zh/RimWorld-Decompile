using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A7 RID: 935
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x0600103B RID: 4155 RVA: 0x00088A98 File Offset: 0x00086E98
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
